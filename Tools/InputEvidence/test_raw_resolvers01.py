"""Author tests for fixed-role and synthetic config/observation semantics only."""
import base64
import copy
import hashlib
import importlib.util
import json
from pathlib import Path
import unittest

HERE=Path(__file__).parent

def encode(v):return json.dumps(v,separators=(',',':'),allow_nan=False).encode('utf-8')
def pin(path,raw):return dict(path=path,bytes=len(raw),sha256=hashlib.sha256(raw).hexdigest().upper())
def load():
    spec=importlib.util.spec_from_file_location('raw_resolvers01',HERE/'raw_resolvers01.py');m=importlib.util.module_from_spec(spec);spec.loader.exec_module(m)
    f=json.loads((HERE/'fixed-inputs.public.json').read_bytes())
    bodies={k:base64.b64decode(v) for k,v in f['bodyRawBase64'].items()}
    blobs={k:base64.b64decode(v) for k,v in f['blobsBase64'].items()}
    primitive=base64.b64decode(f['primitiveBase64'])
    return m,bodies,blobs,primitive

def synthetic(m):
    server=dict(pid=123,startUtc='2026-11-03T00:00:00Z',executablePath='C:/Program Files/Unity/Hub/Editor/6000.3.25f1/Editor/Data/PlaybackEngines/AndroidPlayer/SDK/platform-tools/adb.exe',executableSha256='1E1C2280B90B3F01AD84CD8DF4858B1B1995012814F3CA8893BCC3BA3848EDEC',listenerPort=5037,remotePort=5559,establishedLinks=1)
    target=dict(own=dict(index=2,name='FPS',ready=True,playerPid=124,vmPid=125),server=server,target=dict(serial='emulator-5558',state='device',transportId=1))
    window=dict(issuedUtc='2026-11-03T00:00:00Z',actualStartUtc='2026-11-03T00:00:00Z',newInputStopUtc='2026-11-03T00:13:00Z',hardReturnUtc='2026-11-03T00:15:00Z')
    c=dict(schema='SFF_OWN_FPS2_DEVICE_PREP_SCOPE_V1',enabled=True,rootApproved=True,rootSessionId=m.ROOT_ID,resourceScopeId='a'*32,**window,priorResourcesReturned=True,ownFPS2Only=True,noOtherVmControl=True,allowHelperInstall=False,collectorSourceSha256=m.REGISTRY['source07Source']['sha256'],consolePath='D:/downloads/LDPlayer/LDPlayer14/ldconsole.exe',consoleSha256='A'*64,expectedServer={k:server[k] for k in ['pid','startUtc','executablePath','executableSha256']},maxChildSeconds=5)
    cr=encode(c);cp=pin(m.CONFIG_PATH,cr)
    e=dict(rootSessionId=m.ROOT_ID,resourceScopeId='a'*32,window=window,nowUtc='2026-11-03T00:01:00Z',deviceScopePin=cp,deviceBeforePin={},expectedTarget=target,historicalGameReceipt=dict(path='D:/codex/validation/sudden-force-fps/synthetic-history.public.json',sha256='B'*64),consolePath=c['consolePath'],consoleSha256=c['consoleSha256'])
    game=dict(package='com.AeDeong.SuddenForceFPS',installed=True,installedPath='/data/app/game/base.apk',apkSha256='09403527CFC04B74B85942BB7DFAFB0E727FA30BB0B8660AB2EB010E116E7F76',apkBytes=105690443,splitCount=0,versionCode=34,versionName='1.0.196',testOnly=False,instrumentationRegistered=None)
    helper=dict(package='sff.qa.nativeinput',installed=True,installedPath='/data/app/helper/base.apk',apkSha256='DFFC4933C50C4C8C9AD61A171553ABA816AFB3C3A5EF7D50144DF2B745A106F3',apkBytes=16588,splitCount=0,versionCode=1,versionName='1',testOnly=True,instrumentationRegistered=True)
    observed='2026-11-03T00:00:30Z';pid=126
    installed=dict(schema='L2_MAPPER_GAME_INSTALLED_V1',observedUtc=observed,index=2,serial='emulator-5558',package=game['package'],pid=pid,method='SINGLE_BASE_APK_FULL_SHA256',files=[dict(path=game['installedPath'],bytes=game['apkBytes'],sha256=game['apkSha256'])],historicalProvenance=e['historicalGameReceipt'])
    android=dict(schema='L2_MAPPER_ANDROID_PREPARATION_V1',observedUtc=observed,index=2,serial='emulator-5558',port=5559,running=True,console=dict(path=c['consolePath'],sha256=c['consoleSha256']),game=dict(package=game['package'],versionCode=34,versionName='1.0.196',pid=pid,foreground=True,width=1280,height=720),server=dict(before=server,after=server,protocol=41),helper=dict(package=helper['package'],targetPackage=helper['package'],testOnly=True,installedPath=helper['installedPath'],apkSha256=helper['apkSha256'],apkBytes=16588,splitCount=0,instrumentationRegistered=True))
    before=dict(schema='SFF_OWN_FPS2_DEVICE_OBSERVATION_V1',observedUtc=observed,configSha256=cp['sha256'],collectorSourceSha256=m.REGISTRY['source07Source']['sha256'],resourceScopeId=c['resourceScopeId'],target=target,game=game,helper=helper,gamePid=pid,gameForeground=True,width=1280,height=720,helperMatchesExpected=True,gameInstalledBytesVerified=True,gameInstalledAttestation=installed,androidPreparation=android,mapperRolesComplete=True,missingForMapper=[],observedNotRunningGame=False,gameInstalledBytesProofWithoutPid=None,rawSdkOutputStored=False,**{k:False for k in 'executionAuthorized readyToRun l2Accepted nativeL2Accepted timingAccepted packetAccepted nativeInputExecuted'.split()})
    br=encode(before);bp=pin('D:/codex/validation/sudden-force-fps/synthetic-before.public.json',br);e['deviceBeforePin']=bp
    return c,cr,cp,before,br,bp,e

class ResolverTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):cls.m,cls.bodies,cls.blobs,cls.primitive=load()
    def call(self,role,raw=None,p=None,blobs=None,e=None):
        return self.m.verify_role(role,self.bodies[role] if raw is None else raw,self.m.REGISTRY[role] if p is None else p,self.blobs if blobs is None else blobs,{} if e is None else e,self.primitive)
    def test_actual_fixed5(self):
        for role in ['manifest','sourceReview','controlReview','source07Source','source07Review']:
            with self.subTest(role=role):
                r=self.call(role);self.assertEqual(r['rawRolesSemanticVerified'],[role]);self.assertIs(r['actualAdmissionVerified'],False);self.assertIs(r['executionAuthorized'],False)
    def test_metadata_missing_raw17_blocks(self):
        with self.assertRaisesRegex(self.m.Rejected,'MISSING_INJECTED_DEPENDENCY'):self.call('source17BaselineMetadata')
    def test_manifest_missing_source3_blocks(self):
        b=copy.deepcopy(self.blobs);b.pop(self.m.ACTIVE_SOURCES[0]['path'])
        with self.assertRaises(self.m.Rejected):self.call('manifest',blobs=b)
    def test_manifest_mutated_source3_blocks(self):
        b=copy.deepcopy(self.blobs);b[self.m.ACTIVE_SOURCES[0]['path']]=b'x'
        with self.assertRaises(self.m.Rejected):self.call('sourceReview',blobs=b)
    def test_unknown_role(self):
        with self.assertRaisesRegex(self.m.Rejected,'UNKNOWN_RAW_ROLE'):self.m.verify_role('foreign',b'{}',{},self.blobs,{},self.primitive)
    def test_wrapper_W_instead_of_B(self):
        p=dict(self.m.REGISTRY['source07Review']);p['sha256']='A'*64
        with self.assertRaisesRegex(self.m.Rejected,'FIXED_ORIGINAL_B_PIN'):self.call('source07Review',p=p)
    def test_fixed_source_modified(self):
        with self.assertRaises(self.m.Rejected):self.call('source07Source',raw=self.bodies['source07Source']+b' ')
    def test_primitive_modified(self):
        with self.assertRaisesRegex(self.m.Rejected,'FIXED_PURE_PRIMITIVE_SOURCE'):self.m.verify_role('manifest',self.bodies['manifest'],self.m.REGISTRY['manifest'],self.blobs,{},self.primitive+b' ')
    def test_synthetic_config_and_before(self):
        c,cr,cp,v,br,bp,e=synthetic(self.m);b=copy.deepcopy(self.blobs);b[cp['path']]=cr
        for role,raw,p in [('deviceScope',cr,cp),('deviceBefore',br,bp)]:
            r=self.call(role,raw,p,b,e);self.assertIs(r['physicalEvidenceVerified'],False);self.assertIs(r['beforeActionProvenanceVerified'],False);self.assertIs(r['runtimeHardblocked'],True)
    def test_config_typed_negative(self):
        for field,value in [('allowHelperInstall',True),('allowHelperInstall',0),('enabled',1),('maxChildSeconds',True),('maxChildSeconds',61),('resourceScopeId','b'*32),('collectorSourceSha256','A'*64)]:
            with self.subTest(field=field,value=value):
                c,cr,cp,v,br,bp,e=synthetic(self.m);c[field]=value;cr=encode(c);cp=pin(cp['path'],cr);e['deviceScopePin']=cp
                with self.assertRaises(self.m.Rejected):self.call('deviceScope',cr,cp,e=e)
    def test_before_typed_false_negative(self):
        for field,value in [('gameForeground',False),('gamePid',True),('executionAuthorized',True),('mapperRolesComplete',False),('width',True)]:
            with self.subTest(field=field):
                c,cr,cp,v,br,bp,e=synthetic(self.m);v[field]=value;br=encode(v);bp=pin(bp['path'],br);e['deviceBeforePin']=bp;b=copy.deepcopy(self.blobs);b[cp['path']]=cr
                with self.assertRaises(self.m.Rejected):self.call('deviceBefore',br,bp,b,e)
    def test_missing_expectations(self):
        c,cr,cp,v,br,bp,e=synthetic(self.m)
        with self.assertRaises(self.m.Rejected):self.call('deviceScope',cr,cp,e={})
    def test_schema_unknown_extra_and_duplicate(self):
        c,cr,cp,v,br,bp,e=synthetic(self.m)
        for raw in [encode(dict(c,unknown=True)),cr.replace(b'{',b'{"enabled":true,',1),b'\xef\xbb\xbf'+cr]:
            p=pin(cp['path'],raw);x=copy.deepcopy(e);x['deviceScopePin']=p
            with self.assertRaises(self.m.Rejected):self.call('deviceScope',raw,p,e=x)
    def test_before_wrong_config_source_pin(self):
        c,cr,cp,v,br,bp,e=synthetic(self.m);v['configSha256']='A'*64;br=encode(v);bp=pin(bp['path'],br);e['deviceBeforePin']=bp;b=copy.deepcopy(self.blobs);b[cp['path']]=cr
        with self.assertRaises(self.m.Rejected):self.call('deviceBefore',br,bp,b,e)
    def test_all8_missing17_remains_blocked(self):
        c,cr,cp,v,br,bp,e=synthetic(self.m);b=copy.deepcopy(self.blobs);b[cp['path']]=cr
        inputs={r:dict(raw=self.bodies[r],pin=self.m.REGISTRY[r]) for r in self.m.REGISTRY};inputs.update(deviceScope=dict(raw=cr,pin=cp),deviceBefore=dict(raw=br,pin=bp))
        with self.assertRaisesRegex(self.m.Rejected,'MISSING_INJECTED_DEPENDENCY'):self.m.verify_raw_roles(inputs,b,e,self.primitive)
    def test_default_false(self):
        for argv in [(),('--enable',)]:
            r=self.m.cli(argv);self.assertEqual(r['rawRolesSemanticVerified'],[]);self.assertIs(r['sourceAccepted'],False);self.assertIs(r['readyToRun'],False)

    def historical(self):
        f=json.loads((HERE/'fixed-inputs.public.json').read_bytes())
        return {k:base64.b64decode(v) for k,v in f['historical17BlobsBase64'].items()}
    def test_historical_source17_actual_injected_positive(self):
        b=copy.deepcopy(self.blobs);b.update(self.historical())
        r=self.call('source17BaselineMetadata',blobs=b);self.assertEqual(r['rawRolesSemanticVerified'],['source17BaselineMetadata']);self.assertIs(r['physicalEvidenceVerified'],False)
    def test_historical_source17_one_missing(self):
        h=self.historical();h.pop(next(iter(h)));b=copy.deepcopy(self.blobs);b.update(h)
        with self.assertRaisesRegex(self.m.Rejected,'MISSING_INJECTED_DEPENDENCY'):self.call('source17BaselineMetadata',blobs=b)
    def test_historical_source17_changed(self):
        h=self.historical();h[next(iter(h))]=b'x';b=copy.deepcopy(self.blobs);b.update(h)
        with self.assertRaises(self.m.Rejected):self.call('source17BaselineMetadata',blobs=b)
    def test_fixed6_synthetic2_whole8_positive(self):
        c,cr,cp,v,br,bp,e=synthetic(self.m);b=copy.deepcopy(self.blobs);b.update(self.historical());b[cp['path']]=cr
        inputs={r:dict(raw=self.bodies[r],pin=self.m.REGISTRY[r]) for r in self.m.REGISTRY};inputs.update(deviceScope=dict(raw=cr,pin=cp),deviceBefore=dict(raw=br,pin=bp))
        r=self.m.verify_raw_roles(inputs,b,e,self.primitive)
        self.assertEqual(len(r['rawRolesSemanticVerified']),8)
        for k in ['actualAdmissionVerified','nativeBasicAccepted','executionAuthorized','readyToRun','physicalEvidenceVerified','filesystemNoReparseVerified','clockObserved','roleProducerProvenanceVerified','beforeActionProvenanceVerified']:self.assertIs(r[k],False)

if __name__=='__main__':unittest.main()
