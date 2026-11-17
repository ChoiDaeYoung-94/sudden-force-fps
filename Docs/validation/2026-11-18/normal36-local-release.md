# 정상 Android 36 로컬 빌드·서명과 검증 범위

2026-11-18, 관련 #66/#43/#399/#302/#362. main `e5a7210f3aa0aeb390015cdd9865626f5e9a6298`의 일반 앱 `com.AeDeong.SuddenForceFPS`를 **1.0.199/code36, ARM64, min25/target36, 비개발 빌드**로 생성하고 기존 업로드 키로 서명했습니다. 별도 localqa 앱과 구분합니다. Play Console 최신 최대 코드·업로드·Play 서명본·테스터 설치는 확인하지 않았습니다.

## 포함한 제품 변경

- PR411: 재장전 추가 X 이동 제거와 yaw 조정.
- PR414: Unity 시작 로고 비활성화. Quad D 전체 시작 및 기기별 재생 검수는 별도입니다.
- PR418: 재장전 팔 높이에 `reloadHandWeight`를 적용해 복귀 후반의 팔 끝 노출을 줄였습니다.
- PR420: Raw9 보행/소총 표현 소스 통합. [통합 근거와 남은 범위](raw9-main-integration.md)를 따릅니다.

별도 QA13 APK의 한 재장전 주기 검수와 일반 Windows의 보행·전투 관측을 이 Android36의 실제 경기 통과로 합산하지 않습니다.

## 산출물

로컬 디렉터리: `D:/codex/releases/sudden-force-fps/android-1.0.199-code36-20261118-r02/`. 바이너리는 저장소·Drive·GitHub Release·Play에 업로드하지 않았습니다.

| 파일 | 바이트 | SHA-256 |
| --- | ---: | --- |
| `SuddenForceFPS-1.0.199-code36-UploadKeySigned.apk` | 106,624,379 | `36700E65C29C47910C8ACBB700FD0351496AB70AE5F11080C33BB778846ED4F2` |
| `SuddenForceFPS-1.0.199-code36-UploadKeySigned.aab` | 99,315,405 | `2329D8CC7AB0838EAE9F2A5E399408163A4BA47D64E463CF786AFEEAAA2DDE8D` |

APK v2 서명·v1/v3/v4 비활성, 기존 인증서 SHA-256 `4D072592228C991D3F6A0EDA768C8CBEE40895C3B1F4A6FB9379540F048965E0`입니다. AAB의 모든 payload는 예상 leaf 인증서 한 개로 서명됐으며 서명 항목 외 payload 바이트는 무서명 원본과 같습니다. 인증서 신뢰 체인과 Play 서명은 주장하지 않습니다.

별도 native 입력 helper는 패키지 `sff.qa.nativeinput.normal36`, code36/name `1.0.199-native36`, 16,528B/SHA `FE6AD1FF2238AD941800CD871D9B75C6EBBF0B009B207603941EDCA2615A9513`입니다. 게임 APK와 같은 인증서로 서명했고, 기존 localqa helper를 대체하지 않습니다. helper 성공은 게임 인증·경기 성공 근거가 아닙니다.

## 예약·빌드와 반환

P2의 권위 원장은 `199,0,35`에서 **`199,1,36`으로 단일 예약**했습니다. code35는 실패 예약으로 보존하고 재사용·롤백하지 않았습니다. P13의 추적 원장 사본 `196,2,34`는 그대로이며 두 원장을 같은 값이라고 보고하지 않습니다. 최신 Console 최대값을 확인하지 않았으므로 현재 후보의 업로드는 허용하지 않았습니다.

Unity export의 실제 BuildReport는 **Succeeded/오류0/경고68/269.1598129초**, Native33288은 Exit0/강제종료0입니다. 원 감시 wrapper는 소유를 확인하지 못한 compiler48224 때문에 **CLI1/SDK idle=false**로 끝났습니다. compiler의 소유는 끝내 증명하지 못했고, 보유 조회 handle로 자연 Exit0을 관측했으며 종료·shutdown 호출은 하지 않았습니다. 이 원 실패를 수정하지 않았습니다.

발생한 공개 입력25개를 별도 보존·검토했습니다. EOL20개, EOL/빈 YAML 값 공백2개, 실제 값3개(v7a 제외·GPGS Maven 경로·Resolver ARM64/export)로 구분하며 전부 형식 변경이라고 표현하지 않습니다. CAS 복원 뒤 공개4134개 raw exact/private6 absent-indexS/Git clean·SDK0·Prefs1,1·동일 공유 ADB를 확인해 **04:59:07 후속 반환**으로 연결했습니다. pristine build provenance와 단일 Unity 빌드 안정화는 수용하지 않습니다.

같은 export에서 Gradle 한 호출로 `:launcher:bundleRelease :launcher:assembleRelease`를 수행했습니다. Native49724는 **Exit0/BUILD SUCCESSFUL/13분27초**였지만 즉시 SDK idle 표본false로 원 wrapper는 **CLI1/cleanupErrors1**입니다. 이후 실제 SDK0/source4134 및 준비 입력2827 exact·Prefs/ADB 반환을 확인한 **05:24:36 후속 반환**을 별도로 연결했습니다. 한 호출을 JVM 한 개 또는 원 wrapper 통과라고 표현하지 않습니다.

## 정적·서명 검증

- APK/AAB의 일반 package/code/name/ARM64/min25/target36/nondebug, PGS provider/APP_ID 존재, 지정 광고·결제 제외를 확인했습니다. provider 존재는 PGS 인증 성공이 아닙니다.
- APK/AAB native7개가 동일하며 ELF LOAD/RELRO와 ZIP 16KB 정렬·BundleConfig를 한정 검사했습니다. 실제 16KB 기기 실행은 미확인입니다.
- C++/header580개 raw QA marker0과 별도 metadata39 타입 이름 검사를 구분했습니다. metadata는 **15,386타입·Assembly-CSharp156/이미지1**, QA 이름/namespace4규칙 일치0이며 APK/AAB의 13,746,980B metadata와 일치합니다. 모든 QA 동작 부재·전체 IL2CPP semantic 검증으로 확대하지 않습니다.
- 첫 signer 경로 길이 preflight 실패와 후속 APK_SIGN Exit2/소비된 시도를 보존했습니다. 같은 암호 파일의 중복 인자를 제거하고 stdin을 닫는 별도 r04에서 APK/AAB 서명·검증 native9회 모두 Exit0·강제종료0·출력 회수·SDK0/동일ADB·실행창 내 반환을 확인했습니다. 원 실패의 stderr를 관측하지 않았으므로 정확한 EOF 원인은 확정하지 않습니다.
- wrapper는 키·암호 파일 내용/메타데이터/줄 수를 읽거나 복사하지 않았습니다. 기존 native 서명 도구의 사용과 wrapper의 조회0을 구분합니다.

## 기기 검증

실물 SM-A175N에 게임/helper36을 신규 설치하고 기기 내 전체 APK 해시·버전·UID·최초 설치 시각·dataDir를 확인했습니다. 기존 QA13 두 앱의 전체 APK·UID/최초 설치 시각과 설정4개·IME를 보존했습니다. 갱신 설치·삭제·데이터 초기화는 하지 않았습니다.

설치 뒤 첫 cold 관측은 `CURRENT_FOCUS_UNAVAILABLE`로 중단됐습니다. 실제 조회에서 `dumpsys window windows`에는 mCurrentFocus가 없고 `displays`에는 user0의 단일 focus가 있어, 이 명령 한 단어만 수정·독립 검토했습니다. 첫 baseline의 `ROTATION_UNAVAILABLE`와 이 실패는 각각 보존합니다.

새 실행 범위에서 기존 설치 APK/UID/최초 설치 시각을 재확인한 뒤 **Quad D 로고·PGS 환영 표시 → 닉네임 입력이 있는 PUBLIC ROOMS 화면**을 관측했습니다. 계정 선택·인증 우회 입력은 하지 않았습니다. 일반 Android의 기존 계정 자동 로그인과 로비 진입 경로를 한정 수용하며 전체 인트로 프레임·Android 실청·닉네임 저장·Photon 경기·Play 설치를 통과로 확대하지 않습니다.

후속 닉네임 tap 요청은 관측 이미지의 유효 시간 초과로 실제 입력 전에 거부됐습니다. 실패와 소유 앱 종료를 보존하고 별도 finish에서 **게임/helper PID0·QA13 whole APK 동일·설정/IME/awake/rotation 동일·공유 ADB 세대 동일·returnFailures0/leaseOpen=false**를 확인했습니다.

폰 근거 기준은 `D:/codex/validation/sudden-force-fps/20261117-night/normal36-phone-actual02/`입니다. 설치 run `run-b483ae92be8e4234be4a297f53728709`의 `install-game.public.json`/`install-helper.public.json`, 새 관측 run `run-e8e825e12dc0418b96b4b69509e9f2c9`의 cold `op-7998a8bab9f84ac2aa7f3bc93890d411`·로비 `op-e5f9ccba57af4d0bb07e66484e5e48ed`·최종 `op-0f74be7cbbf449178229866e5e1aed2d/result.public.json`을 대조합니다. PGS 계정 표시명이 있는 이미지는 저장소에 첨부하지 않았습니다.

### LD3 설치·시작과 로그인 제한

LD3(API34/1280×720)에도 **동일 정상36 게임/helper APK를 신규 설치**하고 기기 내 전체 해시·UID/최초 설치 시각을 확인했습니다. 기존 QA11 두 앱과 설정4개를 보존했습니다. 실제 정지 PNG에서 **완성 Quad D 로고**를 확인했으나, 다음 화면은 **Google sign-in was canceled / Retry 안내**였으므로 로비·경기는 수용하지 않습니다. Retry·계정 선택·인증 우회는 하지 않았으며 미로그인·설정 오류·사용자 취소 등 세부 원인은 확정하지 않습니다.

별도 helper 기본 비활성 호출은 `disabledVerified=true/inputEventsRequested=0` 단계 응답을 Root가 관측했습니다. 원시 instrumentation 출력을 파일로 저장한 증거와 구분해 Root 관측 영수증으로 기록합니다. 앱 반환 영수증은 QA11 whole APK/UID/최초 설치 시각·설정4 동일, 정상36 두 APK/UID 유지·PID0/오류0입니다.

처음 LD3/VM 종료는 둘 다 Exit0이었으나 반환 때의 정상 종료 요청에서 **manager25592는 0xC0000005, VM27776은 Exit0**이었습니다. #415 종료 오류 해결로 처리하지 않습니다. 첫 endpoint 미준비·기존 전용 포트 검사 실패·admission SHA 오타 거부도 원 단계 실패로 보존했습니다. Root가 생성한 전용 연결 규칙만 소유 VM 세대를 확인해 정리했고 공유 ADB 재시작·미식별 프로세스 종료는 하지 않았습니다.

첫 후속 검증은 ADB 목록이 즉시 controller final과 달라 실패했습니다. 별도 **06:28:23 읽기 전용 후속 검증**에서 설정8개 raw exact·임시 adbDebug 키 부재·NAT 초기값·5561 listener 부재·타3 VM/공유 ADB/SVC 세대·**초기 ADB 목록 동일**을 확인했습니다. 즉시 controller final 목록과의 비교는 false로 유지합니다. 원래 LD3 guest 세션 자체는 재시작됐으며 복원했다고 주장하지 않습니다.

LD 앱 근거는 `D:/codex/validation/sudden-force-fps/20261117-night/normal36-ld-actual01/r03/run-41bc75db7abe45ee8be7c95092c05ff9/`의 admission/install 두 개·cold/after-intro PNG·`return.public.json`입니다. Session 근거는 `ld415-own3-restart01/run-f7a6d3b740854346bd30899e37930c25/`의 `normal36-root-return.public.json`, `normal36-return-stop-off.public.json`, `post-controller-return-fresh02.public.json`입니다. 이 두 상대 경로의 기준은 아래와 같습니다.

## 재검토할 공개 근거

아래 경로는 이 PC의 공개 검증 자료이며 자격 파일이나 원시 비공개 로그가 아닙니다.

| 근거 | 로컬 경로·SHA-256 |
| --- | --- |
| 예약36 | `20261117-night/android36-canonical-reservation-preparation01/actual-code36/receipt.public.json` · `273D8E64DEA719ABE0C84FC7F06525B93550F5DD1400D55CE3E94AD9614206BC` |
| export 후속 반환 | `D:/codex/a36/root-full-return.public.json` · `81708CF2A755ED38C1AEAC40F10E5F22B9D60A8DEC46397C4159799D137EDDD7` |
| Gradle 후속 반환 | `D:/codex/a36/gradle-run/root-full-return.public.json` · `1FDA2FDAE1A7C17B587373011A474FB134D927E794F36E92EDED71565BB8F2D0` |
| 한정 정적 검사 | `D:/codex/a36/artifact-static/result.public.json` · `09E62A18B375144A7DA2DDAC8D116A9D3ECDA42DFB5FDBEF9B7F19FDB90D4811` |
| metadata 결속 | `D:/codex/a36/metadata-packaging-proof.public.json` · `55D4C25CFE0191EA7000A2DC0BE06E928012F1D3E82E64BEA70226A81B266627` |
| 서명 결과 | 위 release 디렉터리 `result.public.json` · `7B49B4D6B79EFD9648B74151AAF0E1A838630A558150653CDB268EE22AB7D393` |
| 서명9회 반환 | `D:/codex/a36/normal36-sign-run02/return.public.json` · `B97ED100984071DCAF3C64FC30D91EF0AB9004C3F62F8D6988B14142EAB81B93` |

상대 경로 `20261117-night/...`의 기준은 `D:/codex/validation/sudden-force-fps/`입니다. #66 빌드 안정화, #43 PGS/Play 설치, #42 권리 전체, #341/#342 보행/소총 및 #302/#362 시작 전체는 열어 둡니다. CI/CD 확장은 보류하고 광고·인앱 결제는 추가하지 않았습니다.
