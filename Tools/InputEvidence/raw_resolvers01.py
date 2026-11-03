"""Offline raw8 semantic resolver candidate. Explicit injected bytes only.
No operational wiring, filesystem reads, actual clock or producer provenance.
Source07 is never imported/executed. Only the exact pinned pure primitive is run.
"""
import datetime as dt
import hashlib
import json
import math
import re
import types

REGISTRY = {'manifest': {'path': 'D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-candidate01/revision02/manifest.public.json', 'bytes': 6526, 'sha256': 'DE70FB5A34E66F7B3BC241ED771AF77753A88C23B54C63300CC6D3CF1F9DF41F'}, 'sourceReview': {'path': 'D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-source-independent-review02/verification.public.json', 'bytes': 6936, 'sha256': 'F0EFE71473D425EF355F5DAA2EE03224E4601B0729A620840E91B1FFD8CDE4EB'}, 'controlReview': {'path': 'D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-controlled-review02/verification.public.json', 'bytes': 8978, 'sha256': 'BC3C60EE566CFC9F8F3185F562D7E96774568CD4CAD857A081DCD51D47ED6B16'}, 'source17BaselineMetadata': {'path': 'D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/input261-l2-standalone-candidate01/revision16/sourcepins03.public.json', 'bytes': 4310, 'sha256': '7AA959BE1B8FA094FCFF4BF8831D819FF5E061A843B8D81C484B2F12E31AE12A'}, 'source07Source': {'path': 'D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/device-preparation-candidate01/revision04/device_preparation07.py', 'bytes': 35765, 'sha256': '3739547A0B59E3053A4BC29313EBA298E4A1291A0704CC64EFCC6CA81E278571'}, 'source07Review': {'path': 'D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/device-preparation-independent-review01/addendum03/verification.public.json', 'bytes': 10568, 'sha256': '260DEEBFD653082283C4FD579A87EDD12B30AF8CCCD85B927246E5C6BC6799D2'}}
TOP_KEYS = {'manifest': ['schema', 'canonicalCandidatePath', 'parentManifest', 'sourceProfile', 'activeSourceCount', 'activeSources', 'artifacts', 'artifactCountExcludingManifestPostfreezeFinalPins', 'parentFalseIRs', 'functionalDelta', 'coreAndObserverRawExact', 'driverOtherASTExact', 'original161RegressionChecks', 'newM1SupplementChecks', 'checkGroupsNotSummed', 'sourceOnly', 'draftSourceOnlyLock', 'defaultEnabled', 'productionAdmissionImplemented', 'productionTransportImplemented', 'pairedObserverReceiveProducerImplemented', 'csharpCompiled', 'csharpExecuted', 'sourceAccepted', 'noBlockers', 'executionAuthorized', 'readyToRun', 'nativeBasicAccepted', 'gameFireCaptureReleased', 'replicatedAimAccepted', 'androidViewAccepted', 'timingAccepted', 'packetAccepted', 'l2Accepted', 'actualCliCalls', 'actualSdkCalls', 'actualProcessCalls', 'projectGitApiCalls'], 'sourceReview': ['schema', 'scope', 'sourceOnly', 'reviewCompleted', 'sourceAccepted', 'noBlockers', 'mandatoryFindings', 'resolvedFindingIds', 'parentFalseReviewPreserved', 'candidateManifest', 'candidateSources', 'oneHunkWholeByteInverseExact', 'coreAndObserverRawUnchanged', 'finalClockSamplePairReused', 'childBeforeNoCurrentOrClockRefresh', 'strictEndEqualityRejected', 'draftSourceOnlyLockPreserved', 'actualSchemaApproved', 'productionAdmissionImplemented', 'productionTransportImplemented', 'pairedObserverReceiveProducerImplemented', 'observerRuntimeSideEffectsProven', 'runtimeSourceReviewed', 'csharpCompiled', 'csharpExecuted', 'actualDriverCliExecuted', 'actualSdkCalls', 'executionAuthorized', 'actualExecutionAuthorized', 'readyToRun', 'nativeBasicAccepted', 'gameFireCaptureReleased', 'replicatedAimAccepted', 'androidViewAccepted', 'helperExitAccepted', 'resourceReturnAccepted', 'l2Accepted', 'timingAccepted', 'packetAccepted', 'independentCheckCount', 'parentOrAuthorOrControlCountsInherited', 'inputs', 'inputCount', 'checks', 'report', 'script', 'actualReviewerCalls', 'limitations'], 'controlReview': ['schema', 'reviewScope', 'sourceManifest', 'sourceManifestSha256', 'noBlockers', 'controlledTestsAccepted', 'sourceControlAccepted', 'sourceAccepted', 'sourceAcceptanceMeaning', 'mandatoryFindings', 'resolvedFindings', 'currentSourcePins', 'parentFalseIR', 'parentFalseIRPreserved', 'parentFalseIRChangedOrPromoted', 'independentChecks', 'fixtureReuse', 'inputPins', 'defaultIOTrapCalls', 'draftLockPreserved', 'memoryLockChangeOnly', 'verifiedScope', 'limitations', 'productionAdmissionImplemented', 'productionTransportImplemented', 'pairedObserverReceiveProducerImplemented', 'csharpCompiled', 'csharpExecuted', 'actualAcceptance', 'actualCompile', 'actualRuntime', 'runtimeAuthority', 'executionAuthorized', 'readyToRun', 'nativeBasicAccepted', 'gameFireCaptureReleased', 'replicatedAimAccepted', 'androidViewAccepted', 'helperExitAccepted', 'resourceReturnAccepted', 'l2Accepted', 'timingAccepted', 'packetAccepted', 'actualCliCalls', 'actualSdkCalls', 'actualProcessCalls', 'actualInput', 'projectSettingsLedgerPrivateMediaSdkDeviceEnvironmentOsGitApiUnityCalls', 'candidateSourceWrites'], 'source17BaselineMetadata': ['schema', 'sourceOnly', 'files'], 'source07Review': ['schema', 'sourceOnly', 'proposalOnly', 'reviewCompleted', 'sourceAccepted', 'devicePreparationSourceReviewed', 'noBlockers', 'noBlockersScope', 'candidateSource', 'collectorSourceSha256', 'sourceSha256', 'candidateManifest', 'parentSource', 'parentAcceptedReview', 'parent131NotRerunOrAdded', 'exactTwoLiteralRawInverse', 'onlyASTAssignmentsChanged', 'allCallableBodiesUnchanged', 'callableCount', 'sourceAcceptedMeaning', 'mandatoryFindings', 'optionalFindings', 'preservedGuards', 'independentVerification', 'authorEvidence', 'newScopeOutputPath', 'newConfigPath', 'newOutputOrConfigExistenceMetadataQueried', 'RootPlannedAction', 'RootPlannedAllowHelperInstall', 'optionalInstallCapabilityRemoved', 'actualRootSourceScopeRequired', 'RootSeparateL2BindingRequired', 'config18ContainsL2RootBindingSHA', 'rootProducerConstraints', 'oldActual01FactsInherited', 'oldActual01ReceiptsOrDeviceStateRead', 'actualCohort02FactsEstablished', 'actualOSSDKApiWindowsPortsProjectPSPrivate6RawLogsHAreaNativeGuiUnityAdbGitCalls', 'actualScopeOrConfigCreated', 'actualExecution', 'actualInstall', 'executionAuthorized', 'actualExecutionAuthorized', 'readyToRun', 'l2Accepted', 'nativeL2Accepted', 'timingAccepted', 'packetAccepted', 'currentUserAction', 'questions']}
ACTIVE_SOURCES = [{'path': 'D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-candidate01/revision02/basic_contract01.py', 'bytes': 16359, 'sha256': '1B9137C8444E7231C6D0F8B79625562EBFC6428048A2AD02B5B3D6F4745CE40E'}, {'path': 'D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-candidate01/revision02/bounded_sdk_driver01.py', 'bytes': 6235, 'sha256': '39D37E2E9663BC1A3217F38FA56917594BAEE1761925EB892E3D27A8E36A809E'}, {'path': 'D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-candidate01/revision02/NativeBasicReadOnlyObserver01.cs', 'bytes': 4568, 'sha256': 'FDF753D3C3F5B3306AD1F4E85B5785AA5D731A4F315DFDEE13DA837CEB0A731B'}]

ROLES = tuple(REGISTRY) + ('deviceScope', 'deviceBefore')
PRIMITIVE_SHA = '45DF478F1328DA53FC31C0A8EAE8819358FDF52C91352C3F1BC9D1DFD3CBDD08'
PRIMITIVE_BYTES = 17223
ROOT_ID = '01a18266-d131-7e42-910e-ca5c54b10bce'
CONFIG_PATH = 'D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/device-prep-cohort02-scope.public.json'
CONFIG_KEYS = 'schema enabled rootApproved rootSessionId resourceScopeId issuedUtc actualStartUtc newInputStopUtc hardReturnUtc priorResourcesReturned ownFPS2Only noOtherVmControl allowHelperInstall collectorSourceSha256 consolePath consoleSha256 expectedServer maxChildSeconds'.split()
EXPECTED_KEYS = 'rootSessionId resourceScopeId window nowUtc deviceScopePin deviceBeforePin expectedTarget historicalGameReceipt consolePath consoleSha256'.split()
NO_AUTHORITY = dict(sourceAccepted=False, semanticAdmissionAccepted=False, actualAdmissionVerified=False,
    nativeBasicAccepted=False, executionAuthorized=False, readyToRun=False, runtimeHardblocked=True,
    physicalEvidenceVerified=False, filesystemNoReparseVerified=False, clockObserved=False,
    roleProducerProvenanceVerified=False, beforeActionProvenanceVerified=False,
    producerCountImplemented=0, l2Accepted=False, timingAccepted=False, packetAccepted=False)

class Rejected(ValueError):
    pass

def need(ok, message):
    if not ok: raise Rejected(message)

def exact(value, keys, label):
    need(type(value) is dict and set(value)==set(keys), label)

def eq(a,b):
    if type(a) is not type(b): return False
    if type(a) is dict: return a.keys()==b.keys() and all(eq(a[k],b[k]) for k in a)
    if type(a) is list: return len(a)==len(b) and all(eq(x,y) for x,y in zip(a,b))
    return a==b

def sha(raw):
    need(type(raw) is bytes, 'RAW_BYTES_REQUIRED')
    return hashlib.sha256(raw).hexdigest().upper()

def raw_pin(raw, pin, cap=2097152):
    exact(pin, ('path','bytes','sha256'), 'PIN_EXACT3')
    need(type(pin['path']) is str and len(pin['path'])<=1024 and len(pin['path'])>0, 'PIN_PATH_TEXT')
    need(type(pin['bytes']) is int and 0<pin['bytes']<=cap, 'PIN_SIZE_TYPED_BOUND')
    need(type(pin['sha256']) is str and re.fullmatch('[A-F0-9]{64}',pin['sha256']) is not None,'PIN_SHA')
    need(type(raw) is bytes and len(raw)==pin['bytes'] and 0<len(raw)<=cap,'RAW_SIZE')
    need(sha(raw)==pin['sha256'],'RAW_SHA')

def strict(raw):
    need(type(raw) is bytes and 0<len(raw)<=2097152,'JSON_BOUND')
    def pairs(rows):
        d={}
        for k,v in rows: need(k not in d,'DUPLICATE_KEY');d[k]=v
        return d
    def integer(s):
        need(len(s.lstrip('-'))<=20,'INT64_BOUND');v=int(s);need(-(2**63)<=v<2**63,'INT64_BOUND');return v
    def bad(_): raise Rejected('NONFINITE_JSON')
    try:
        text=raw.decode('utf-8','strict');need(not text.startswith('\ufeff'),'BOM_FORBIDDEN')
        value=json.loads(text,object_pairs_hook=pairs,parse_int=integer,parse_constant=bad)
    except (UnicodeError,ValueError,RecursionError,OverflowError) as exc:
        if isinstance(exc,Rejected): raise
        raise Rejected('STRICT_JSON') from None
    stack=[(value,0)];nodes=0
    while stack:
        v,depth=stack.pop();nodes+=1;need(depth<=64 and nodes<=100000,'JSON_COMPLEXITY')
        if type(v) is str: need(not any(0xD800<=ord(c)<=0xDFFF for c in v),'SURROGATE')
        elif type(v) is float:need(math.isfinite(v),'NONFINITE_JSON')
        elif type(v) is dict:stack.extend((x,depth+1) for x in list(v)+list(v.values()))
        elif type(v) is list:stack.extend((x,depth+1) for x in v)
    need(type(value) is dict,'JSON_OBJECT');return value

def primitive(source_raw):
    need(type(source_raw) is bytes and len(source_raw)==PRIMITIVE_BYTES and sha(source_raw)==PRIMITIVE_SHA,'FIXED_PURE_PRIMITIVE_SOURCE')
    m=types.ModuleType('pinned_pure_raw_evidence_primitive')
    exec(compile(source_raw,'<pinned-pure-primitive>','exec'),m.__dict__)
    return m

def fetch(pin, blobs):
    need(type(blobs) is dict and type(pin['path']) is str and pin['path'] in blobs,'MISSING_INJECTED_DEPENDENCY')
    raw=blobs[pin['path']];raw_pin(raw,pin);return raw

def fixed(role, raw, pin):
    need(role in REGISTRY,'FIXED_ROLE_REQUIRED')
    need(eq(pin,REGISTRY[role]),'FIXED_ORIGINAL_B_PIN')
    raw_pin(raw,pin)
    if role=='source07Source':
        try:raw.decode('utf-8','strict')
        except UnicodeError:raise Rejected('SOURCE07_UTF8') from None
        return None
    v=strict(raw);exact(v,TOP_KEYS[role],'FROZEN_TOP_KEYS');return v

def manifest(blobs):
    v=fixed('manifest',fetch(REGISTRY['manifest'],blobs),REGISTRY['manifest'])
    need(v['schema']=='SFF_NATIVE_BASIC_DRAFT_SOURCE_MANIFEST_REVISION02_V1','MANIFEST_SCHEMA')
    need(v['sourceOnly'] is True and v['draftSourceOnlyLock'] is True and v['sourceProfile']=='DRAFT_SOURCE_ONLY_LOCK','MANIFEST_DRAFT_SCOPE')
    need(type(v['activeSourceCount']) is int and v['activeSourceCount']==3 and eq(v['activeSources'],ACTIVE_SOURCES),'MANIFEST_EXACT_SOURCE3')
    for row in ACTIVE_SOURCES:
        raw=fetch(row,blobs)
        try:raw.decode('utf-8','strict')
        except UnicodeError:raise Rejected('MANIFEST_SOURCE_UTF8') from None
    for k in ('defaultEnabled','productionAdmissionImplemented','productionTransportImplemented','pairedObserverReceiveProducerImplemented','sourceAccepted','noBlockers','executionAuthorized','readyToRun','nativeBasicAccepted'):
        need(v[k] is False,'MANIFEST_FALSE_GUARD_'+k)
    return v

def source07(blobs):
    return fixed('source07Source',fetch(REGISTRY['source07Source'],blobs),REGISTRY['source07Source'])

def expectations(e,p):
    exact(e,EXPECTED_KEYS,'EXPECTED_FACTS_EXACT10')
    need(e['rootSessionId']==ROOT_ID,'EXPECTED_ROOT')
    need(type(e['resourceScopeId']) is str and re.fullmatch('[a-f0-9]{32}',e['resourceScopeId']) is not None,'EXPECTED_SCOPE')
    exact(e['window'],('issuedUtc','actualStartUtc','newInputStopUtc','hardReturnUtc'),'EXPECTED_WINDOW4')
    for value in list(e['window'].values())+[e['nowUtc']]:p.utc(value)
    p.target(e['expectedTarget']);p.digest(e['consoleSha256'])
    need(e['consolePath'] in p.CONSOLES,'EXPECTED_CONSOLE')

def config(raw,pin,blobs,e,p):
    expectations(e,p);need(eq(pin,e['deviceScopePin']),'TRUSTED_SCOPE_B_PIN')
    need(pin['path']==CONFIG_PATH,'SOURCE07_CONFIG_PATH')
    source07(blobs);raw_pin(raw,pin,262144);c=strict(raw);exact(c,CONFIG_KEYS,'EXACT_CONFIG18')
    need(c['schema']=='SFF_OWN_FPS2_DEVICE_PREP_SCOPE_V1' and c['rootSessionId']==e['rootSessionId']==ROOT_ID,'CONFIG_ROOT_SCHEMA')
    need(all(c[k] is True for k in ('enabled','rootApproved','priorResourcesReturned','ownFPS2Only','noOtherVmControl')) and c['allowHelperInstall'] is False,'CONFIG_OBSERVE_ONLY_TYPED_BOOLS')
    need(c['resourceScopeId']==e['resourceScopeId'],'CONFIG_EXPECTED_SCOPE')
    need(c['collectorSourceSha256']==REGISTRY['source07Source']['sha256'],'CONFIG_SOURCE07_B')
    for k,v in e['window'].items():need(c[k]==v,'CONFIG_ORIGINAL_WINDOW_'+k)
    a,b,d=(p.utc(c[k]) for k in ('actualStartUtc','newInputStopUtc','hardReturnUtc'))
    need(p.utc(c['issuedUtc'])<=a<=p.utc(e['nowUtc'])<b<d and 0<b-a<=7800000000 and 0<d-a<=9000000000 and d-b>=1200000000,'CONFIG_ORIGINAL13_15_RETURN2')
    need(c['consolePath']==e['consolePath'] and c['consoleSha256']==e['consoleSha256'],'CONFIG_EXPECTED_CONSOLE')
    server=e['expectedTarget']['server'];expected_server={k:server[k] for k in ('pid','startUtc','executablePath','executableSha256')}
    need(eq(c['expectedServer'],expected_server),'CONFIG_EXPECTED_SERVER4')
    need(type(c['maxChildSeconds']) is int and 1<=c['maxChildSeconds']<=60,'CONFIG_CHILD_CAP')
    return c

def verify_role(role,raw,body_pin,blobs,expected,primitive_source_raw):
    """Verify one raw role from injected bytes, NEVER actual admission."""
    need(type(role) is str and role in ROLES,'UNKNOWN_RAW_ROLE')
    p=primitive(primitive_source_raw)
    try:
        v=fixed(role,raw,body_pin) if role in REGISTRY else None
        if role=='manifest':
            need(fetch(REGISTRY['manifest'],blobs)==raw,'MANIFEST_INPUT_BINDING');manifest(blobs)
        elif role=='sourceReview':
            m=manifest(blobs)
            need(v['schema']=='SFF_NATIVE_BASIC_DRAFT_SOURCE_INDEPENDENT_REVIEW_REVISION02_V1' and v['scope']=='REVISION02_EXACT_M1_DELTA_LOCKED_DRAFT_SOURCE_ONLY','SOURCE_IR_SCHEMA_SCOPE')
            need(all(v[k] is True for k in ('sourceOnly','reviewCompleted','sourceAccepted','noBlockers')),'SOURCE_IR_TYPED_ACCEPTANCE')
            need(eq(v['candidateManifest'],REGISTRY['manifest']) and eq(v['candidateSources'],m['activeSources']),'SOURCE_IR_MANIFEST_SOURCE_B')
        elif role=='controlReview':
            m=manifest(blobs)
            need(v['schema']=='SFF_NATIVE_BASIC_REVISION02_INDEPENDENT_CONTROLLED_REVIEW_V1' and v['reviewScope']=='FROZEN_DRAFT_SOURCE_DELTA_AND_SYNTHETIC_MEMORY_ONLY','CONTROL_IR_SCHEMA_SCOPE')
            need(all(v[k] is True for k in ('sourceAccepted','sourceControlAccepted','controlledTestsAccepted','noBlockers')),'CONTROL_IR_TYPED_ACCEPTANCE')
            # Only exact fixed IR is admitted; map its registered backslash text pairs.
            registered_manifest=dict(REGISTRY['manifest'],path=REGISTRY['manifest']['path'].replace('/','\\'))
            registered_sources=[dict(x,path=x['path'].replace('/','\\')) for x in ACTIVE_SOURCES]
            need(eq(v['sourceManifest'],registered_manifest) and eq(v['currentSourcePins'],registered_sources) and v['sourceManifestSha256']==REGISTRY['manifest']['sha256'],'CONTROL_IR_MANIFEST_SOURCE_B')
        elif role=='source17BaselineMetadata':
            need(v['schema']=='SFF_L2_BOUNDED_BRIDGE_STANDALONE_SOURCE_PINS_V3' and v['sourceOnly'] is True,'METADATA_SCHEMA')
            need(type(v['files']) is list and len(v['files'])==17,'SOURCE17_ROWS')
            raw17={row['path']:fetch(row,blobs) for row in v['files']}
            p.verify_source17(raw,body_pin['sha256'],body_pin['bytes'],raw17)
        elif role=='source07Source': pass
        elif role=='source07Review':
            source07(blobs)
            need(v['schema']=='SFF_DEVICE_PREPARATION_SOURCE07_PATH_PORT_INDEPENDENT_REVIEW_V1' and v['noBlockersScope']=='SOURCE07_PATH_PORT_SOURCE_ONLY','SOURCE07_IR_SCHEMA_SCOPE_SELECTOR')
            need(all(v[k] is True for k in ('sourceOnly','proposalOnly','reviewCompleted','sourceAccepted','devicePreparationSourceReviewed','noBlockers')),'SOURCE07_IR_TYPED_ACCEPTANCE')
            need(eq(v['candidateSource'],REGISTRY['source07Source']) and v['sourceSha256']==REGISTRY['source07Source']['sha256'] and v['collectorSourceSha256']==REGISTRY['source07Source']['sha256'],'SOURCE07_IR_SOURCE_B')
        elif role=='deviceScope':config(raw,body_pin,blobs,expected,p)
        elif role=='deviceBefore':
            expectations(expected,p);need(eq(body_pin,expected['deviceBeforePin']),'TRUSTED_BEFORE_B_PIN')
            raw_pin(raw,body_pin,262144);strict(raw)
            scope=fetch(expected['deviceScopePin'],blobs);c=config(scope,expected['deviceScopePin'],blobs,expected,p)
            p.verify_device_before(raw,body_pin['sha256'],body_pin['bytes'],config_sha256=expected['deviceScopePin']['sha256'],collector_source_sha256=REGISTRY['source07Source']['sha256'],resource_scope_id=c['resourceScopeId'],expected_target=expected['expectedTarget'],console_path=c['consolePath'],console_sha256=c['consoleSha256'],historical_game_receipt=expected['historicalGameReceipt'],actual_start_utc=c['actualStartUtc'],new_input_stop_utc=c['newInputStopUtc'],hard_return_utc=c['hardReturnUtc'],now_utc=expected['nowUtc'])
        if role in ('sourceReview','controlReview','source07Review'):
            for k in ('executionAuthorized','readyToRun','timingAccepted','packetAccepted'):
                need(v[k] is False,'IR_FALSE_CLAIM_'+k)
    except p.Rejected as exc:raise Rejected('PINNED_PRIMITIVE:'+str(exc)) from None
    return dict(rawRolesSemanticVerified=[role],originalBodyPinVerified=True,injectedBytesOnly=True,**NO_AUTHORITY)

def verify_raw_roles(role_inputs,blobs,expected,primitive_source_raw):
    exact(role_inputs,ROLES,'EXACT_RAW8_INPUTS')
    verified=[]
    for role in ROLES:
        exact(role_inputs[role],('raw','pin'),'ROLE_INPUT_EXACT2')
        verify_role(role,role_inputs[role]['raw'],role_inputs[role]['pin'],blobs,expected,primitive_source_raw)
        verified.append(role)
    return dict(rawRolesSemanticVerified=verified,injectedBytesOnly=True,**NO_AUTHORITY)

def cli(argv=()):
    return dict(rawRolesSemanticVerified=[],reason='DRAFT_RAW_RESOLVERS_ONLY',**NO_AUTHORITY)
