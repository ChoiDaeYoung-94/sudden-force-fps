"""Offline proposal02 structure/raw-pin verifier. No operational admission API.

All bytes, schemas and trusted expectations are explicit inputs. This module
does not read files, clocks, environment, processes, handles, or SDK state.
"""
import hashlib
import json
import math
import re

MAX_BYTES = 2097152
SCHEMA_SHA = {
    'descriptor': 'D7A0993D8E7CC2698F38C559088E16FC4E62A209ABC0C4A52E14449A2C613A28',
    'receipt': '062F1FA3CCCF303FF49830F756A95AC1002C5B471C1285C4131D13785DE98AF1',
    'roles': '68AEBE6B5ECF97981094747ADC4180AA9083D4B04CD6AC8859E3A89F564FF833',
}
KEYWORDS = frozenset(('$schema', '$id', '$comment', '$defs', '$anchor', '$ref',
    'type', 'const', 'enum', 'default', 'properties', 'additionalProperties',
    'propertyNames', 'minProperties', 'maxProperties', 'minItems', 'maxItems',
    'items', 'minimum', 'maximum', 'minLength', 'maxLength', 'pattern',
    'allOf', 'oneOf', 'anyOf'))


class Rejected(ValueError):
    pass


def require(ok, message):
    if not ok:
        raise Rejected(message)


def sha(raw):
    require(type(raw) is bytes, 'bytes required')
    return hashlib.sha256(raw).hexdigest().upper()


def _pairs(pairs):
    out = {}
    for key, value in pairs:
        require(key not in out, 'duplicate JSON key')
        out[key] = value
    return out


def _integer(value):
    require(len(value.lstrip('-')) <= 20, 'JSON integer too large')
    n = int(value)
    require(-(2**63) <= n <= 2**63 - 1, 'JSON integer outside int64')
    return n


def _constant(value):
    raise Rejected('nonfinite JSON constant')


def strict_json(raw, limit=MAX_BYTES):
    require(type(limit) is int and 1 <= limit <= MAX_BYTES, 'invalid JSON limit')
    require(type(raw) is bytes and 0 < len(raw) <= limit, 'JSON bytes bound')
    try:
        text = raw.decode('utf-8', errors='strict')
        require(not text.startswith('\ufeff'), 'BOM forbidden')
        value = json.loads(text, object_pairs_hook=_pairs,
                           parse_int=_integer, parse_constant=_constant)
        stack = [(value, 0)]
        count = 0
        while stack:
            item, depth = stack.pop()
            count += 1
            require(count <= 100000 and depth <= 64, 'JSON complexity bound')
            if type(item) is str:
                require(not any(0xD800 <= ord(c) <= 0xDFFF for c in item), 'surrogate forbidden')
            elif type(item) is float:
                require(math.isfinite(item), 'nonfinite JSON number')
            elif type(item) is dict:
                stack.extend((k, depth + 1) for k in item)
                stack.extend((v, depth + 1) for v in item.values())
            elif type(item) is list:
                stack.extend((v, depth + 1) for v in item)
        return value
    except (UnicodeError, json.JSONDecodeError, RecursionError, OverflowError) as exc:
        raise Rejected('invalid strict JSON') from exc


def typed_equal(left, right):
    if type(left) is not type(right):
        return False
    if type(left) is dict:
        return left.keys() == right.keys() and all(typed_equal(left[k], right[k]) for k in left)
    if type(left) is list:
        return len(left) == len(right) and all(typed_equal(a, b) for a, b in zip(left, right))
    return left == right


def compile_schema(schema):
    """Compile only the documented subset; inspect ALL branches before validation.

    No remote references, unknown keywords, ignored assertion siblings, or
    implicit type coercion. Annotation keywords have no assertion semantics.
    """
    anchors = {}
    refs = []

    def walk(node):
        require(type(node) is dict, 'schema object required')
        require(not (set(node) - KEYWORDS), 'unsupported schema keyword')
        if '$anchor' in node:
            a = node['$anchor']
            require(type(a) is str and a not in anchors, 'bad/duplicate schema anchor')
            anchors[a] = node
        if '$ref' in node:
            r = node['$ref']
            require(type(r) is str and r.startswith('#') and len(r) > 1 and '/' not in r,
                    'unsupported schema reference')
            refs.append(r[1:])
        if 'type' in node:
            require(node['type'] in ('object', 'array', 'string', 'integer', 'number', 'boolean', 'null'),
                    'unsupported schema type')
        for k in ('properties', '$defs'):
            if k in node:
                require(type(node[k]) is dict, 'bad schema map')
                for n in node[k].values():
                    walk(n)
        for k in ('items', 'propertyNames'):
            if k in node:
                walk(node[k])
        if 'additionalProperties' in node:
            a = node['additionalProperties']
            require(type(a) in (dict, bool), 'bad additionalProperties')
            if type(a) is dict:
                walk(a)
        for k in ('oneOf', 'allOf', 'anyOf'):
            if k in node:
                require(type(node[k]) is list and len(node[k]) > 0, 'bad schema alternatives')
                for n in node[k]:
                    walk(n)
        for k in ('minProperties', 'maxProperties', 'minItems', 'maxItems', 'minLength', 'maxLength'):
            if k in node:
                require(type(node[k]) is int and node[k] >= 0, 'bad schema bound')
        for k in ('minimum', 'maximum'):
            if k in node:
                require(type(node[k]) in (int, float) and math.isfinite(node[k]), 'bad numeric bound')
        if 'enum' in node:
            require(type(node['enum']) is list and len(node['enum']) > 0, 'bad schema enum')
        if 'pattern' in node:
            require(type(node['pattern']) is str, 'bad schema pattern')
            try:
                re.compile(node['pattern'])
            except re.error as exc:
                raise Rejected('bad schema regex') from exc

    walk(schema)
    require(all(r in anchors for r in refs), 'unresolved schema reference')

    def check(value, node=None, depth=0):
        node = schema if node is None else node
        require(depth <= 128, 'schema recursion bound')
        if '$ref' in node:
            check(value, anchors[node['$ref'][1:]], depth + 1)
        if 'type' in node:
            t = node['type']
            ok = {'object': type(value) is dict, 'array': type(value) is list,
                  'string': type(value) is str, 'integer': type(value) is int,
                  'number': type(value) in (int, float), 'boolean': type(value) is bool,
                  'null': value is None}[t]
            require(ok, 'schema type mismatch: ' + t)
        if 'const' in node:
            require(typed_equal(value, node['const']), 'schema const mismatch')
        if 'enum' in node:
            require(any(typed_equal(value, e) for e in node['enum']), 'schema enum mismatch')
        if type(value) is dict:
            require(len(value) >= node.get('minProperties', 0), 'missing object properties')
            require(len(value) <= node.get('maxProperties', 100000), 'extra object properties')
            for key, item in value.items():
                if 'propertyNames' in node:
                    check(key, node['propertyNames'], depth + 1)
                if key in node.get('properties', {}):
                    check(item, node['properties'][key], depth + 1)
                else:
                    ap = node.get('additionalProperties', True)
                    require(ap is not False, 'unknown object property: ' + key)
                    if type(ap) is dict:
                        check(item, ap, depth + 1)
        elif type(value) is list:
            require(len(value) >= node.get('minItems', 0), 'array too short')
            require(len(value) <= node.get('maxItems', 100000), 'array too long')
            if 'items' in node:
                for item in value:
                    check(item, node['items'], depth + 1)
        elif type(value) is str:
            require(len(value) >= node.get('minLength', 0), 'string too short')
            require(len(value) <= node.get('maxLength', MAX_BYTES), 'string too long')
            if 'pattern' in node:
                require(re.search(node['pattern'], value) is not None, 'string pattern mismatch')
        elif type(value) in (int, float):
            require(math.isfinite(value), 'nonfinite number')
            require(value >= node.get('minimum', float('-inf')), 'number below minimum')
            require(value <= node.get('maximum', float('inf')), 'number above maximum')
        for k in ('allOf', 'oneOf', 'anyOf'):
            if k in node:
                successes = 0
                for branch in node[k]:
                    try:
                        check(value, branch, depth + 1)
                        successes += 1
                    except Rejected:
                        pass
                require(successes == len(node[k]) if k == 'allOf' else
                        successes == 1 if k == 'oneOf' else successes > 0,
                        'schema ' + k + ' mismatch')

    return check, anchors


def lexical_path(path):
    """Text guard only; cannot establish filesystem canonicality/no-reparse."""
    require(type(path) is str and path.startswith('D:/codex/validation/sudden-force-fps/'), 'pin path root')
    require(len(path) <= 1024 and '\\' not in path and ':' not in path[2:], 'pin path syntax')
    for seg in path[3:].split('/'):
        require(seg not in ('', '.', '..') and not seg.endswith(('.', ' ')), 'pin path alias')
        require(not any(ord(c) < 32 or c in '<>"|?*' for c in seg), 'pin path character')
        name = seg.split('.')[0].upper()
        require(name not in ('CON', 'PRN', 'AUX', 'NUL', 'CONIN$', 'CONOUT$') and
                re.fullmatch(r'(COM|LPT)[0-9\u00b9\u00b2\u00b3]', name) is None, 'reserved pin path')
    return path


def pin(raw, path):
    return {'path': lexical_path(path), 'bytes': len(raw), 'sha256': sha(raw)}


def validate_dag(parents):
    require(type(parents) is dict, 'DAG map required')
    state = {}
    order = []

    def visit(role):
        require(role in parents, 'unresolved parent role')
        require(state.get(role) != 1, 'receipt cycle')
        if state.get(role) == 2:
            return
        state[role] = 1
        require(type(parents[role]) is list, 'DAG parents list required')
        require(len(set(parents[role])) == len(parents[role]), 'duplicate parent role')
        for parent in parents[role]:
            require(type(parent) is str and parent != role, 'self/bad parent')
            visit(parent)
        state[role] = 2
        order.append(role)

    for role in parents:
        require(type(role) is str, 'role string required')
        visit(role)
    return order


def disabled_result(reason='DRAFT_OFFLINE_ONLY'):
    return {'graphRawPinsVerified': False, 'semanticAccepted': False,
            'actualAdmissionVerified': False, 'executionAuthorized': False,
            'nativeBasicAccepted': False, 'readyToRun': False,
            'runtimeHardblocked': True, 'reason': reason}


def cli(argv=()):
    """No argument parsing I/O; --enable and every other argument remain locked."""
    return disabled_result('ENABLE_LOCKED' if '--enable' in argv else 'DRAFT_OFFLINE_ONLY')


def verify_graph(descriptor_raw, expected_descriptor_pin, expected_clock_raw, blobs,
                 descriptor_schema_raw, receipt_schema_raw, role_table_raw):
    """Raise Rejected on graph/pin mismatch; a successful return NEVER admits runtime.

    expected_descriptor_pin and expected_clock_raw must originate in a trusted
    caller. Bytes maps cannot prove that files, clocks or producers ever existed.
    """
    for name, raw in (('descriptor', descriptor_schema_raw), ('receipt', receipt_schema_raw),
                      ('roles', role_table_raw)):
        require(type(raw) is bytes and 0 < len(raw) <= MAX_BYTES, 'contract bytes bound')
        require(sha(raw) == SCHEMA_SHA[name], 'frozen contract digest mismatch: ' + name)
    descriptor_check, da = compile_schema(strict_json(descriptor_schema_raw))
    receipt_check, ra = compile_schema(strict_json(receipt_schema_raw))
    table = strict_json(role_table_raw)
    roles = {row['role']: row for row in table['roles']}
    require(len(roles) == 20, 'role table count')
    descriptor = strict_json(descriptor_raw)
    descriptor_check(descriptor)
    descriptor_check(expected_descriptor_pin, da['pin'])
    require(typed_equal(pin(descriptor_raw, expected_descriptor_pin['path']), expected_descriptor_pin),
            'trusted descriptor raw pin mismatch')
    clock = strict_json(expected_clock_raw)
    descriptor_check(clock, da['clock'])
    require(typed_equal(clock, descriptor['clock']), 'trusted clock mismatch')
    require(type(blobs) is dict and 28 <= len(blobs) <= 128, 'injected blobs exact dict/bound')
    total = 0
    for path, raw in blobs.items():
        lexical_path(path)
        require(type(raw) is bytes and 0 < len(raw) <= MAX_BYTES, 'injected bytes bound')
        total += len(raw)
    require(total <= 64 * MAX_BYTES, 'injected total bound')
    evidence = descriptor['evidence']
    require(set(evidence) == set(roles), 'evidence role set')
    paths = [p['path'] for p in evidence.values()]
    require(len(set(p.casefold() for p in paths)) == 20, 'duplicate wrapper path')
    require(len(set(p['sha256'] for p in evidence.values())) == 20, 'duplicate wrapper digest')

    def resolve(p):
        receipt_check(p, ra['d6'])
        lexical_path(p['path'])
        require(p['path'] in blobs, 'unresolved injected path')
        raw = blobs[p['path']]
        require(typed_equal(pin(raw, p['path']), p), 'injected raw path/size/SHA mismatch')
        return raw

    receipts = {}
    graph = {}
    body_pins = {}
    for role, p in evidence.items():
        receipt = strict_json(resolve(p))
        receipt_check(receipt)
        require(receipt['role'] == role, 'receipt role mismatch')
        for field in ('rootSessionId', 'scopeId'):
            require(receipt[field] == descriptor[field], 'receipt common identity mismatch')
        require(receipt['clockSessionId'] == clock['sessionId'], 'receipt clock session mismatch')
        parent_roles = roles[role]['parentRoles']
        require(set(receipt['parentPins']) == set(parent_roles), 'exact parent role set mismatch')
        for parent, pp in receipt['parentPins'].items():
            require(parent != role and parent in evidence, 'self/foreign parent')
            require(typed_equal(pp, evidence[parent]), 'parent descriptor W pin mismatch')
        graph[role] = list(receipt['parentPins'])
        facts = receipt['facts']
        require(typed_equal(facts['capture']['clock'], clock), 'capture clock mismatch')
        require(facts['capture']['readStartNs'] <= facts['capture']['readStopNs'], 'capture monotonic order')
        require(facts['capture']['readStartNs'] >= clock['windowStartReceiveNs'], 'capture precedes window')
        if role == 'heldReady':
            require(typed_equal(facts['heldHandleOwner'], clock), 'held handle owner clock mismatch')
        if roles[role]['rawBodyRequired']:
            bp = facts['body']
            require(bp['path'].casefold() not in {x.casefold() for x in paths}, 'body aliases wrapper path')
            require(bp['sha256'] not in {x['sha256'] for x in evidence.values()}, 'B equals W')
            resolve(bp)
            body_pins[role] = bp
        if role == 'pairedReceiver':
            for which in ('before', 'after'):
                require(typed_equal(facts[which]['body']['clock'], clock), 'sample clock mismatch')
        receipts[role] = receipt
    order = validate_dag(graph)

    def facts(role):
        return receipts[role]['facts']

    # Hash selectors explicitly distinguish original B and receipt W.
    def digest(value, role, body=False):
        target = body_pins[role] if body else evidence[role]
        require(value == target['sha256'], ('B' if body else 'W') + ' selector mismatch: ' + role)

    for field, role in (('configBodySha256', 'deviceScope'), ('sourceBodySha256', 'source07Source'),
                        ('reviewBodySha256', 'source07Review'), ('beforeBodySha256', 'deviceBefore')):
        digest(facts('deviceBeforeInvocation')[field], role, True)
    digest(facts('source17BaselineRawAudit')['metadataBodySha256'], 'source17BaselineMetadata', True)
    digest(facts('heldReady')['nativeBindingReceiptSha256'], 'nativeBinding')
    digest(facts('prefix')['preArmReceiptSha256'], 'preArmHistory')
    digest(facts('androidContext')['prefixReceiptSha256'], 'prefix')
    digest(facts('geometry')['androidContextReceiptSha256'], 'androidContext')
    digest(facts('pairedReceiver')['associationReceiptSha256'], 'association')
    for field, role in (('configSha256', 'deviceScope'), ('sourceSha256', 'source07Source'),
                        ('bodySha256', 'deviceBefore')):
        digest(descriptor['beforeCall'][field], role, True)
    require(descriptor['beforeCall']['scopeId'] == descriptor['scopeId'], 'beforeCall scope mismatch')
    for name in ('callStartReceiveNs', 'callEndReceiveNs'):
        require(descriptor['beforeCall'][name] == facts('deviceBeforeInvocation')[name], 'beforeCall interval mismatch')
    require(typed_equal(facts('nativeBinding')['identity'], descriptor['native']), 'native projection mismatch')
    result = disabled_result('SEMANTIC_RESOLVERS_AND_PRODUCERS_UNIMPLEMENTED')
    result.update({'graphRawPinsVerified': True, 'descriptorStructureVerified': True,
                   'receiptStructuresVerified': 20, 'wrapperPinsVerified': 20,
                   'originalBodyPinsVerified': len(body_pins), 'topologicalRoles': order,
                   'declaredClockBindingVerified': True, 'lexicalPathsVerified': True,
                   'filesystemCanonicalityVerified': False, 'clockObserved': False,
                   'rawSemanticResolversImplemented': 0, 'producersImplemented': 0})
    return result
