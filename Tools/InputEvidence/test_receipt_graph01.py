"""Synthetic/in-memory author tests. Disk reads occur only in setUpClass/main.

Run: python -X utf8 -I -B -m unittest discover -s <this directory> -p 'test_*.py' -v
"""
import copy
import importlib.util
import json
from pathlib import Path
import unittest


def encode(value):
    return json.dumps(value, ensure_ascii=False, separators=(',', ':'), allow_nan=False).encode('utf-8')


def load_module(directory):
    spec = importlib.util.spec_from_file_location('receipt_graph01', directory / 'receipt_graph01.py')
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module


def synthetic_fixture(module, ds_raw, rs_raw, table_raw):
    """Generate every closed facts field from bundled schema, then bind real bytes."""
    ds, rs, table = map(module.strict_json, (ds_raw, rs_raw, table_raw))
    dc, da = module.compile_schema(ds)
    rc, ra = module.compile_schema(rs)
    prefix = 'D:/codex/validation/sudden-force-fps/synthetic-receipt-graph01/'
    clock = {'api': 'ROOT_PYTHON_TIME_MONOTONIC_NS', 'sessionId': 'a' * 32,
             'receiverPid': 123, 'receiverStartUtc': '2026-11-03T00:00:00.0000000Z',
             'receiverStartTicks': '639292608000000000', 'windowStartReceiveNs': 100}

    def make(node, anchors):
        if '$ref' in node:
            return make(anchors[node['$ref'][1:]], anchors)
        if 'const' in node:
            return copy.deepcopy(node['const'])
        if 'enum' in node:
            return copy.deepcopy(node['enum'][0])
        if 'allOf' in node:
            out = {}
            for n in node['allOf']:
                out.update(make(n, anchors))
            return out
        if 'oneOf' in node or 'anyOf' in node:
            return make(node.get('oneOf', node.get('anyOf'))[0], anchors)
        kind = node.get('type', 'object' if 'properties' in node else None)
        if kind == 'object':
            if set(node.get('properties', {})) == set(clock):
                return copy.deepcopy(clock)
            out = {k: make(v, anchors) for k, v in node.get('properties', {}).items()}
            if set(out) == {'path', 'bytes', 'sha256'}:
                out = module.pin(b'auxiliary-unverified', prefix + 'auxiliary.public.json')
            if 'readStartNs' in out:
                out['readStartNs'], out['readStopNs'] = 100, 200
            return out
        if kind == 'array':
            return [make(node['items'], anchors) for _ in range(node.get('minItems', 0))]
        if kind == 'string':
            pattern = node.get('pattern', '')
            if pattern == '^[A-F0-9]{64}$':
                return 'A' * 64
            if pattern == '^[a-f0-9]{32}$':
                return 'a' * 32
            if '(-[a-f0-9]{4})' in pattern:
                return '11111111-1111-1111-1111-111111111111'
            if '[.][0-9]{7}Z' in pattern:
                return '2026-11-03T00:00:00.0000000Z'
            if pattern == '^[1-9][0-9]{0,18}$':
                return '639292608000000000'
            if pattern == '^[a-f0-9]{40}$':
                return '1' * 40
            if pattern.startswith('^D:'):
                return prefix + 'auxiliary.public.json'
            return 'synthetic'
        if kind in ('integer', 'number'):
            return max(1, node.get('minimum', 0))
        if kind == 'boolean':
            return False
        if kind == 'null':
            return None
        return {}

    descriptor = make(ds, da)
    descriptor['clock'] = copy.deepcopy(clock)
    descriptor['rootSessionId'] = '11111111-1111-1111-1111-111111111111'
    descriptor['currentSource2'][1]['relativePath'] = 'Assets/Scripts/UI/CombatPointerControl.cs'
    roles = {r['role']: r for r in table['roles']}
    order = module.validate_dag({r: row['parentRoles'] for r, row in roles.items()})
    bodies, blobs = {}, {}
    for role, row in roles.items():
        if row['rawBodyRequired']:
            raw = encode({'syntheticOriginalBodyFor': role, 'notSemanticEvidence': True})
            bp = module.pin(raw, prefix + role + '-body.public.json')
            bodies[role] = bp
            blobs[bp['path']] = raw
    receipts, evidence = {}, {}
    for role in order:
        row = roles[role]
        facts = make(rs['$defs'][row['factsDefinitionPointer'].split('/')[-1]], ra)
        if row['rawBodyRequired']:
            facts['body'], facts['validatorId'] = bodies[role], row['rawValidatorId']
        if role == 'deviceBeforeInvocation':
            for field, target in (('configBodySha256', 'deviceScope'), ('sourceBodySha256', 'source07Source'),
                                  ('reviewBodySha256', 'source07Review'), ('beforeBodySha256', 'deviceBefore')):
                facts[field] = bodies[target]['sha256']
            facts['callStartReceiveNs'], facts['callEndReceiveNs'] = 120, 140
        if role == 'source17BaselineRawAudit':
            facts['metadataBodySha256'] = bodies['source17BaselineMetadata']['sha256']
        for r, key, parent in (('heldReady', 'nativeBindingReceiptSha256', 'nativeBinding'),
                              ('prefix', 'preArmReceiptSha256', 'preArmHistory'),
                              ('androidContext', 'prefixReceiptSha256', 'prefix'),
                              ('geometry', 'androidContextReceiptSha256', 'androidContext'),
                              ('pairedReceiver', 'associationReceiptSha256', 'association')):
            if role == r:
                facts[key] = evidence[parent]['sha256']
        if role == 'nativeBinding':
            facts['identity'] = copy.deepcopy(descriptor['native'])
        receipt = {'schema': 'SFF_NATIVE_BASIC_RECEIPT_PROPOSAL_V2', 'role': role,
                   'rootSessionId': descriptor['rootSessionId'], 'scopeId': descriptor['scopeId'],
                   'clockSessionId': clock['sessionId'],
                   'parentPins': {p: copy.deepcopy(evidence[p]) for p in row['parentRoles']}, 'facts': facts}
        raw = encode(receipt)
        p = module.pin(raw, prefix + role + '-wrapper.public.json')
        blobs[p['path']] = raw
        evidence[role], receipts[role] = p, receipt
    descriptor['evidence'] = evidence
    descriptor['beforeCall']['scopeId'] = descriptor['scopeId']
    for key, role in (('configSha256', 'deviceScope'), ('sourceSha256', 'source07Source'), ('bodySha256', 'deviceBefore')):
        descriptor['beforeCall'][key] = bodies[role]['sha256']
    descriptor['beforeCall']['callStartReceiveNs'], descriptor['beforeCall']['callEndReceiveNs'] = 120, 140
    raw = encode(descriptor)
    return {'descriptor_raw': raw, 'expected_descriptor_pin': module.pin(raw, prefix + 'descriptor.public.json'),
            'expected_clock_raw': encode(clock), 'blobs': blobs,
            'descriptor_schema_raw': ds_raw, 'receipt_schema_raw': rs_raw, 'role_table_raw': table_raw}


class ReceiptGraphTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.directory = Path(__file__).parent
        cls.m = load_module(cls.directory)
        cls.fixture = synthetic_fixture(cls.m, (cls.directory / 'descriptor.schema.json').read_bytes(),
                                        (cls.directory / 'receipt.schema.json').read_bytes(),
                                        (cls.directory / 'role-resolution.table.json').read_bytes())

    def check(self, fixture=None):
        return self.m.verify_graph(**(fixture or self.fixture))

    def mutate(self, role, change):
        """Rehash affected descendants: semantic negatives cannot fail on stale W."""
        f = copy.deepcopy(self.fixture)
        d = self.m.strict_json(f['descriptor_raw'])
        old = copy.deepcopy(d['evidence'])
        receipts = {r: self.m.strict_json(f['blobs'][p['path']]) for r, p in old.items()}
        change(receipts[role], d)
        order = self.m.validate_dag({r: list(v['parentPins']) for r, v in receipts.items()})
        for r in order:
            rec = receipts[r]
            rec['parentPins'] = {p: copy.deepcopy(d['evidence'][p]) for p in rec['parentPins']}
            for key, parent in (('nativeBindingReceiptSha256', 'nativeBinding'), ('preArmReceiptSha256', 'preArmHistory'),
                                ('prefixReceiptSha256', 'prefix'), ('androidContextReceiptSha256', 'androidContext'),
                                ('associationReceiptSha256', 'association')):
                if key in rec['facts']:
                    rec['facts'][key] = d['evidence'][parent]['sha256']
            raw = encode(rec)
            path = old[r]['path']
            f['blobs'][path] = raw
            d['evidence'][r] = self.m.pin(raw, path)
        f['descriptor_raw'] = encode(d)
        f['expected_descriptor_pin'] = self.m.pin(f['descriptor_raw'], f['expected_descriptor_pin']['path'])
        return f

    def reject(self, f):
        with self.assertRaises(self.m.Rejected):
            self.check(f)

    def test_synthetic_20_role_positive_stays_hardblocked(self):
        r = self.check()
        self.assertEqual((r['wrapperPinsVerified'], r['originalBodyPinsVerified']), (20, 8))
        self.assertTrue(r['graphRawPinsVerified'])
        for key in ('semanticAccepted', 'actualAdmissionVerified', 'executionAuthorized', 'nativeBasicAccepted', 'readyToRun'):
            self.assertIs(r[key], False)
        self.assertTrue(r['runtimeHardblocked'])

    def test_strict_json(self):
        bad = (b'{"x":1,"x":2}', b'{"x":NaN}', b'{"x":Infinity}', b'{"x":-Infinity}',
               b'{"x":1e999}', b'\xef\xbb\xbf{}', b'{"x":"\xff"}', b'{"x":"\\ud800"}',
               b'[' * 66 + b'0' + b']' * 66, b' ' * (self.m.MAX_BYTES + 1), b'{"x":9223372036854775808}')
        for raw in bad:
            with self.subTest(raw=raw[:40]), self.assertRaises(self.m.Rejected):
                self.m.strict_json(raw)

    def test_unknown_schema_and_reference(self):
        for schema in ({'type': 'integer', 'exclusiveMinimum': 0}, {'oneOf': [{}, {'typo': 1}]},
                       {'$ref': '#missing'}, {'$ref': 'https://remote/schema'}, {'type': ['integer', 'null']}):
            with self.subTest(schema=schema), self.assertRaises(self.m.Rejected):
                self.m.compile_schema(schema)

    def test_type_bool_float_const_enum(self):
        for schema, value in (({'type': 'integer'}, True), ({'type': 'integer'}, 1.0),
                              ({'const': 1}, True), ({'enum': [1]}, True), ({'const': False}, 0)):
            with self.subTest(schema=schema), self.assertRaises(self.m.Rejected):
                self.m.compile_schema(schema)[0](value)

    def test_unknown_missing_role(self):
        for key in ('unknown', 'missing'):
            f = copy.deepcopy(self.fixture)
            d = self.m.strict_json(f['descriptor_raw'])
            p = d['evidence'].pop('manifest')
            if key == 'unknown':
                d['evidence']['other'] = p
            f['descriptor_raw'] = encode(d)
            f['expected_descriptor_pin'] = self.m.pin(f['descriptor_raw'], f['expected_descriptor_pin']['path'])
            self.reject(f)

    def test_unknown_missing_fact_and_type(self):
        for change in (lambda r, d: r['facts'].update(extra=False),
                       lambda r, d: r['facts'].pop('nativeExe'),
                       lambda r, d: r['facts']['identity'].update(pid=True),
                       lambda r, d: r.update(role='manifest')):
            self.reject(self.mutate('nativeBinding', change))

    def test_all_20_roles_closed_facts_and_strict_ns(self):
        d = self.m.strict_json(self.fixture['descriptor_raw'])
        for role in d['evidence']:
            for mutation in ('unknown', 'missing', 'bool_ns'):
                def change(r, ignored, mutation=mutation):
                    if mutation == 'unknown':
                        r['facts']['unrecognized'] = False
                    elif mutation == 'missing':
                        r['facts'].pop(next(iter(r['facts'])))
                    else:
                        r['facts']['capture']['readStartNs'] = True
                with self.subTest(role=role, mutation=mutation):
                    self.reject(self.mutate(role, change))

    def test_parent_missing_foreign(self):
        for change in (lambda r, d: r['parentPins'].clear(),
                       lambda r, d: r['parentPins'].update(source07Source=d['evidence']['source07Source'])):
            self.reject(self.mutate('heldReady', change))

    def test_digest_size_body_and_wrapper_mutations(self):
        for target in ('wrapper', 'body', 'missing'):
            f = copy.deepcopy(self.fixture)
            d = self.m.strict_json(f['descriptor_raw'])
            p = d['evidence']['manifest']
            if target == 'body':
                p = self.m.strict_json(f['blobs'][p['path']])['facts']['body']
            if target == 'missing':
                del f['blobs'][p['path']]
            else:
                f['blobs'][p['path']] += b' '
            self.reject(f)

    def test_wrapper_vs_body_selector(self):
        self.reject(self.mutate('deviceBeforeInvocation', lambda r, d:
                               r['facts'].update(configBodySha256=d['evidence']['deviceScope']['sha256'])))

    def test_clock_and_monotonic(self):
        self.reject(self.mutate('geometry', lambda r, d: r['facts']['capture']['clock'].update(receiverPid=99)))
        self.reject(self.mutate('geometry', lambda r, d: r['facts']['capture'].update(readStopNs=99)))
        self.reject(self.mutate('heldReady', lambda r, d: r['facts']['heldHandleOwner'].update(receiverPid=99)))
        self.reject(self.mutate('pairedReceiver', lambda r, d: r['facts']['before']['body']['clock'].update(sessionId='b' * 32)))
        f = copy.deepcopy(self.fixture)
        c = self.m.strict_json(f['expected_clock_raw'])
        c['receiverPid'] = 99
        f['expected_clock_raw'] = encode(c)
        self.reject(f)

    def test_path_aliases(self):
        base = 'D:/codex/validation/sudden-force-fps/'
        for tail in ('../x.public.json', './x.public.json', 'a//b.public.json', 'a\\b.public.json',
                     'a:x.public.json', 'a./b.public.json', 'a /b.public.json', 'NUL.public.json', 'COM1.public.json'):
            with self.subTest(tail=tail), self.assertRaises(self.m.Rejected):
                self.m.lexical_path(base + tail)

    def test_dag_cycle_self_unresolved(self):
        for graph in ({'a': ['b'], 'b': ['a']}, {'a': ['a']}, {'a': ['foreign']}, {'a': ['b', 'b'], 'b': []}):
            with self.subTest(graph=graph), self.assertRaises(self.m.Rejected):
                self.m.validate_dag(graph)

    def test_trusted_descriptor_pin(self):
        for key, value in (('bytes', 1), ('sha256', 'A' * 64)):
            f = copy.deepcopy(self.fixture)
            f['expected_descriptor_pin'][key] = value
            self.reject(f)

    def test_default_and_enable_locked(self):
        for args in ((), ('--enable',), ('arbitrary',)):
            r = self.m.cli(args)
            self.assertFalse(r['executionAuthorized'])
            self.assertTrue(r['runtimeHardblocked'])

    def test_schema_tamper(self):
        f = copy.deepcopy(self.fixture)
        f['receipt_schema_raw'] += b' '
        self.reject(f)


if __name__ == '__main__':
    unittest.main()
