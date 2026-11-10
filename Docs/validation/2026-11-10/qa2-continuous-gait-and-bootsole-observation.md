# QA2 연속 보행·사격과 실제 밑창 높이 관측

Case05에서 같은 D7/QA2의 정상 두 피어 방을 두 번 열어 이동·정지·사격과 메뉴 퇴장→양쪽 Lobby 복귀를 확인했습니다. [Case04의 벽 가림](qa2-editor-ld-game-and-gait-observation.md)을 줄여 **진단 카메라375 PNG와 실제 신발 정점299개 시간 표본**을 확보했습니다. 보행 연결과 제한된 사격 상태 변화의 근거이며, 접지·양손 IK·제품 전체 수용은 아직 미완료입니다. [Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)과 [이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)은 유지합니다.

소스는 `d7a3371296357a5ceb1b7b657d468415130e7047`, LD2 설치본은 localqa/code2/`1.0.197-qa2`·100,492,368B/SHA `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33`입니다. 같은 APK 전체 installed readback을 새로 대조했습니다. 운영34/`1.0.196`/release12·원장196,2,34와 LD1 흰 화면 미해결은 그대로이며 휴대전화·Play/Drive 업로드는0입니다.

## 실제 이동·총기 화면

원창은 UTC06:17:38/새 작업 중지06:37:38/전체 반환06:42:38, nonce `6e82e6cfcb464e858652e82f56f60d6e`이며 연장하지 않았습니다. GPU Direct3D11 Editor와 LD2의 정상 닉네임·방 생성·팀 선택·READY/START로 Room A와 B를 구성했습니다. Room A 메뉴 퇴장 뒤 같은 두 피어가 새 Room B의 Game에 다시 참가했고, B도 정상 메뉴 퇴장으로 마쳤습니다. 두 경기를 각각5분 완료한 결과로 세지 않습니다.

진단 카메라는 실제 네트워크 상대의 서쪽에서 촬영했고 actor transform·health·Networked 상태는 쓰지 않았습니다. 각 세트는5Hz·최대15초/75 PNG입니다. `body01`의75장은 잘못된 입력 인자로 이동이 실행되지 않은 idle/phase0이며 보행 통과에 합산하지 않습니다. `body02`는 실제 좌우 swipe와 정지, `body03`은 전후 이동과 사격 상태를 포함합니다. `rifle01/02`는 정지한 측면 근접 화면입니다. 다섯 세트 모두75개·가림 pause0·cleanupErrors0을 기록했으며 정상 플레이어 카메라의 가시성이나 게임 FPS 측정이 아닙니다.

연속 표본에서 다리 자세가 변하고 소총과 앞쪽 받침손이 총 근처에 유지됩니다. 일부 순간에는 두 밑창과 지면 사이 간격이 보이고, 다른 표본에는 한 발이 지면 가까이 오거나 idle로 복귀합니다. 반대편 방아쇠손·어깨 접점은 겹치며 aimYaw/aimPitch는0으로 고정돼 있습니다. 모든 방향·달리기·수직 조준·정확한 양손 IK·foot sliding을 통과 처리하지 않습니다.

Room A의 실제 터치 사격 뒤 `body03`의 ShotSequence4→10·Ammo30→24를 관측했고, 뒤의 `rifle02`는 ShotSequence10·Ammo30이었습니다. 사격 해제 뒤 추가 사격 없이 유지되는 표본과 장전 후 탄약 복귀는 확인했지만 reloading=true와 정확한 장전 지연은 캡처하지 못했습니다. 명중/피해·킬·사격/장전 자세 전체·실청과는 구분합니다. QA2 동시 두 손가락·Sprint·사망/리스폰은 미실행입니다.

## 발목 대신 실제 밑창 정점 측정

Room B의 새 runner/actor에서 boot704 정점을 임시 Mesh에 BakeMesh하고 좌우 신발의 최저 world vertex와 layer8 바닥을 읽었습니다. actor·bone·renderer 설정 변경은0입니다.10초 요청에서299개 표본을 약33.35ms 간격으로 기록하고, elapsed10,000.6075ms/cleanupErrors0·callback 회수를 확인했습니다. Room A 이미지와 같은 프레임을 측정한 자료는 아닙니다.

모든 ground ray가 같은 평면 collider/slope0을 반환했습니다. 속도0 정지194표본의 밑창–바닥 간격 중앙값은 왼쪽3.566cm/오른쪽3.559cm입니다. 발목 본의 약17cm 높이를 부유 높이로 읽지 않습니다. actor/capsule bottom은 약3cm·CC skinWidth3cm이지만 이것만으로 원인을 확정하지 않습니다.

속도>0.15m/s 이동105표본에서 더 낮은 발의 밑창–바닥 간격은 최저-2.30cm/중앙1.42cm/p95 5.08cm/최고5.57cm였습니다. 양발이 각각 정지 기준보다1cm 이상 동시에 높았던 것은9표본·세 구간이며 첫~끝 표본 span은 약100/33/67ms입니다. 정지 기준보다3cm 동시 초과는0이고, 양발의 동시 추가 이격 최대는약2.00cm입니다. 이 수치는 통과 임계값이 아니라 분포를 설명하는 기준입니다. 짧은 추가 이격과 일부 음수 정점을 확인했으나 정상 midstride·지속 부유·접지/미끄러짐 원인이나 최종 품질을 확정하지 않습니다.

D7 Raw9는 contact correction이 비활성이며 원본 stance의 root/pelvis 위치에 다리 회전을 적용합니다. 비활성 Contact03은 위로만 올리는 침투 보정이어서 이미 높은 양발을 내리는 해법으로 간주하지 않습니다. 실제 측정 없이 본 높이·모델 import 설정·전체 visual offset을 임의 변경하지 않았습니다.

## 실패 보존과 반환

첫 Prefs05 요청은 peer ownership 확인 실패로 registry/Editor 실행 전에 멈췄습니다. 새 zero-payload 관측에서 같은 ADB generation이 확인된 뒤, 출력명·schema namespace만 바꾼 Prefs06으로 같은 원창 안에서 진행했습니다. 최초 실패 원인은 미확정이며 server lifecycle/재시작은0입니다. LD 요청55개 중 text1·swipe6개는 총괄의 인자 키 오류로 실제 입력 명령 전에 차단됐습니다. 이후 정확한 인자로 성공한 호출과 구분하며 성공48개를 기능 시험48개 통과로 세지 않습니다.

밑창 probe01의 borrowed Mesh NativeArray Dispose는 총괄 소스 검토에서 차단해 enabled 실행0으로 보존했습니다. [Unity 공식 참조 소스](https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Graphics/Mesh.bindings.cs)의 Allocator.None view를 확인한 뒤 두 Dispose만 제거한02를 기본 비활성 컴파일하고 실행했습니다. 게임 소스 수정이나 기존 배열 해제는 하지 않았습니다.

최종 Lobby에는 actor0·소유 camera0·소유 Mesh0이었고 clean Untitled와 원래 runInBackground=true로 돌아왔습니다. Exit 요청 CLI6은 invalid response로 보존하며 보유 Native18972의06:32:27.4910022 UTC Exit0/not forced·EOF2·Dispose0과 구분합니다. LD caller native0·force-stop/HOME·활성 호출0, prefs 두 값의 원래1 복원·동일 ADB peer를 확인했습니다. 전체 unknown child 종료의 증명은false입니다.

소유 rsp/meta2 회수 직후에는 EDM1개 차이가 남았습니다. 같은 자동 세 값/서식 생성 바이트를 외부 보존하고 baseline으로 복원한 뒤, **06:33:19.873994 UTC 최종 Root 감사에서 공개4,095개 raw exact·private6 absent/indexS·D7 Git clean·임시2 부재**를 확인했습니다. binding/config/lease는 회수했고 원 new/full 전에 실제 반환을 통보했습니다. 독립 검토가 이 프로젝트 전체 파일을 다시 조회한 결과로 표현하지 않습니다.

## 다음 검증과 자료

기존 helper 서명 APK16,528B/SHA `31B7971E51B49C2814CCEACA7F77C98529E05E732DC372890AB331995B7C520E`는 Java `EB57301379472C59018BAA635904FF1C2B06B7E4E6E7FAFF122AA9C37167E580`→무서명 qualification/build03→서명 payload 비교의 공개 체인으로 연결됩니다. Case05의 새 installed readback은 game만이며 helper를 새로 재조회한 결과는 아닙니다. 이 Java는 target code1/`1.0.197-qa1`만 허용하므로 같은 helper로 QA2를 시험하면 주입 전 error3 거절이 예상됩니다. 실제 QA2 helper 주입을 시도하거나 거절을 재현한 결과는 아닙니다. 이번 실제 이동/사격은 일반 터치 명령이며 이 helper 성공으로 표시하지 않습니다.

다음은 target code2/`1.0.197-qa2` 고정의 새 helper 빌드·서명·정적/설치 결속부터 준비한 뒤 Sprint+이동·두 손가락 해제 순서와 남은 방향/조준/생명주기를 검증하는 것입니다. 기존 game-only signer를 helper 승인으로 재사용하거나 소비된 서명 승인을 되살리지 않습니다. 접지 수용·반대 authority/proxy·12 hitbox·실제16KB·음향/장시간 성능·전체 권리는 후속입니다. 현재 필요한 사용자 조치·질문은 없습니다.

외부 루트는 `D:/codex/validation/sudden-force-fps/20261110-work`입니다. 실제 PNG/summary, 저장된 SDK 응답, `root-resume01/case05-root-observations.public.json`, holder/Prefs06·owned2 회수 결과와 `editor-case05-final-return01/final-audit.public.json`을 보존했습니다. 8초 `case05-media-preview01/qa2-lateral-gait-5hz-diagnostic.mp4`는 body02 첫40개 PNG를5fps로 인코딩한 진단 파생물이며 원본 화면 녹화·음향·게임 FPS 증거가 아닙니다. 원시 로그·개인/서명 입력은 저장소에 옮기지 않습니다.

독립 실제 검토는84개 공개 핀·375 PNG 메타데이터/30개 시각 표본·299개 밑창 표본·호출/반환 기록의 일관성을 수용했습니다. 전체375개 시각검토나 Root의4,095개/Git/private6 감사 독립 재현은 아닙니다.

| 자료(위 루트 기준) | bytes | SHA256 |
| --- | ---: | --- |
| `root-resume01/case05-root-observations.public.json` | 2,947 | `B8297CD54ED9324BAE427F73FEDA95FEDCFC3632F4D415F929E2105318D1B68C` |
| `root-resume01/editor-case05-final-return01/final-audit.public.json` | 1,145 | `123E71627903CFE33919DD36C20E516701F581B0B231E78E2CA612009CE77C3B` |
| `footcontact-readonly-probe-candidate01/actual-6e82e6cfcb464e858652e82f56f60d6e-probe01/summary.public.json` | 642,764 | `8F0B121B6308FE164E3FF20D1302A44C1047814ED1BD2841A9DD7757B51A3CE7` |
| `qa2-editor-ld-case05-visual-independent-review01/footprobe-math.public.json` | 8,756 | `BD83739D5E14054A2B7696204B507291CCCC71B5E27CB28C2AAFA60EBF8E04EF` |
| `qa2-editor-ld-case05-visual-independent-review01/verification.public.json` | 3,812 | `49ACA130B51E17B20CA7F1B69F36C6CF3022EDF6ED59E18F52E73EB1A8E78C20` |
| `root-resume01/case05-media-preview01/qa2-lateral-gait-5hz-diagnostic.mp4` | 203,534 | `69FEC4E3EED403C9099396C73AACD0115682A132687631D7D0DBB7A1F9A8ED9D` |
