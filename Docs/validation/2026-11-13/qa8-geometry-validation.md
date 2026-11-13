# 제품43 Android QA8 검증 후보

Refs #402, #401. 제품 코드 기준은 `8fcc73ce`이며 일반 빌드 PR #400의 로그인은 변경하지 않습니다.

기존 QA7은 gaitraw1-localqa 접속 세대라 제품43의 gaitprod1과 연결되지 않습니다. 이 QA 브랜치는 Android에 `SUDDEN_FORCE_LOCAL_QA`가 명시된 경우에만 개발용 로그인 경로를 허용합니다. 변경은 기존 조건문 한 줄이며 QA7의 해당 Login blob과 같습니다. 일반 Android PGS/Play 경로 수용을 의미하지 않습니다.

QA8 목표는 package `com.AeDeong.SuddenForceFPS.localqa`, code8/name1.0.197-qa8, `gaitprod1-window1-shoulderref1-localqa` 접속 세대입니다. Source freeze·컴파일·export·APK·서명·설치는 각 단계에서 새로 확인합니다. 운영 code34/release12와 원장196,2,34는 변경하지 않습니다.

`LocalQaGeometryDiagnostics`는 Android+LOCAL_QA와 위 package/version이 모두 맞을 때만 자체 collector를 생성합니다. Game의 유일한 유효 input owner·제품 QA namespace·owner/MainCamera·조준점·Canvas가 정합할 때, 자연 `Canvas.willRenderCanvases`에서 같은 프레임의 값을 최대8회/0.25초 간격으로 읽습니다. 최초 owner 후3초 readiness와 전체300초 상한을 두고, 중복이나 실제 namespace 불일치·성공 후 이탈은 거부합니다. 이후 자체 callback을 제거하고 collector를 파괴합니다.

기록은 `[SFF_QA8_GEOMETRY]` 접두사의 화면 크기·safeArea/캐시·조준점 화면 좌표·안전 영역 RectTransform·카메라 pixelRect/중앙 ray와 순수 SimulationShotOrigin getter 결과입니다. 닉네임·계정·AppID·디바이스 ID를 수집하지 않습니다. 자체 collector 생명주기와 제한된 로그 외에 파일·네트워크·입력·Gameplay 상태·캔버스 강제 갱신·설정 저장을 호출하지 않습니다. Origin 읽기는 같은 틱의 실제 발사 방향/명중 승인과 구분합니다.

최초 진단 후보는 전환 첫 프레임의 readiness 부족으로 조기 종료될 위험이 있어 외부 보존했고, 현재 후보는 제한된3초 대기를 적용했습니다. 아래 실제 결과를 반영하며, 진단 성공이나 #401 정렬 수정 통과로 처리하지 않습니다.

짧은 실기기 검증은 시작 전에 관측/입력/종료 script를 준비하고, 연속 터치 이동·geometry를 한 묶음으로 받은 뒤 필요한 경우 검토된 timer10초 fixture를 사용합니다. 분석은 경기 복귀 후 수행하며 5분 만료를 기다리지 않습니다. 이전 Windows의 URP 파생 변경을 고려해 Android import/export 전후 전체 소스와 실제 산출물을 구분해 보존합니다.

## 2026-11-13 실제 QA8 결과

실제 빌드 소스는 `d8ff8a73a157c868c4c117fddbf14c07584c6f39`입니다. export89.56초/errors0/warnings4, Gradle23분3초/150tasks/launcher exit0로 QA8 APK를 생성했습니다. export 직후 Prefs 반환 보류와 Gradle 최종 daemon alive 관측으로 남은 원래 wrapper exit1은 보존하고, 각각 별도 fresh 반환 검증을 수행했습니다. SDK 검사 배치01의 PowerShell 함수 이름 충돌과 own PS 중단도 보존했으며 별도02에서 검사·기존 키 서명을 완료했습니다.

설치 가능한 로컬 파일은 `SuddenForceFPS-LocalQA-1.0.197-qa8-ARM64.apk`, 100,496,464 B, SHA256 `BD9F6C8D9CA1EE5CC0BE68A450B320C216BC00EE5C90BE78F96BA475FBB67ACC`입니다. package/code8/nameqa8·min25/target36·ARM64·v2 기존 인증서를 확인했고, 무서명→서명 ZIP359 payload와 native7 LOAD/RELRO·ZIP16KB 정적 검사를 통과했습니다. 실제16KB 기기 수용은 아닙니다.

SM-A175N 폰에 게임8/helper8을 데이터 삭제 없이 설치했습니다. 폰 내부 base APK의 크기/SHA는 로컬 서명본과 일치했습니다. host 전체 readback은 실패 상태를 보존하며, helper 전체 설치 bytes 비교 성공과 구분합니다. LD QA8 설치는 아직 없습니다. Play34/release12·원장196,2,34와 일반 Android PGS 수용 범위는 그대로입니다.

Editor43props/gaitprod1-localqa와 폰으로 세 번의 짧은 경기를 진행했습니다. 경기 전 RAM timer compile/Arm(false)=DISABLED를 확인하고 START 직후299초대에서10초 fixture를 Arm했습니다. 세 경기 모두 DRAW/TIME EXPIRED 및 양쪽 정상 RETURN→Lobby를 확인했습니다. 명시적인 APPLIED/cleanup 조회는 첫 경기만 있으며, 둘째/셋째는 ARMED와 결과 화면 관측 범위입니다. 둘째의 passive observer CS0029 실패/등록0·native입력0은 보존한 뒤, 올바른 delegate를 비활성 컴파일하고 셋째를 진행했습니다.

셋째에서 실제 two-pointer 이동+FIRE 28이벤트·약1450ms·error0·포인터 해제·동일 폰PID를 확인했습니다. 5초 범위197 passive 표본 중 상대44개가 속도 비영이며 gait phase0→0.5188767, 위치 약1.55m 변화, pose status가 유지됐습니다. Host state-authority 관측으로 pure proxy·연속 시각·부위 피해·kill/respawn 수용은 아닙니다. 입력 이벤트와 각 표본을 잇는 절대시각 anchor도 없습니다.

Geometry의 성공 샘플은0개입니다. 첫 조회0행/후속 JSONDecodeError였고, prefix-only 진단은 sample0/readiness-timeout 1행·payload1004 B·중간 잘림을 관측했습니다. 진단 소스는 MainCamera 태그를 필수로 요구하지만 실제 Soldier 카메라는 Untagged입니다. 같은 실행 Editor의 읽기 전용 관측에서도 own active child camera1/MainCamera0/Camera.main!=own을 확인했습니다. 이는 진단 소스의 결정적 불일치이며, 폰 마지막 실패 predicate를 직접 관측한 것은 아닙니다. #401의 safeArea 원인·조준점 오프셋 수정은 아직 미수용입니다.

폰은 정상 EXIT/OK 뒤17:56:00 KST에 game/helperPID0·force-stop0·원설정·Dozing·공유 ADB 동일 세대·forward0으로 반환했습니다. Editor는 QA define 원형 복구·clean Untitled·native normalexit0·Prefs 복구 후, 형식 차이3개만 원본/관측본 보존과 exact allowlist CAS로 복구했습니다. 별도 공개4132 rawexact/Gitclean/private6 absent-indexS 감사로18:01:12 원기한 내 전체 반환을 확인했습니다. 이 문서 변경은 이후의 의도한 문서 delta이며 원형 감사가 문서 변경까지 포함했다고 표현하지 않습니다.

후속 QA9 외부 비활성 후보는 별도 Editor에서 Android+LOCAL_QA 전처리 RAM compile/errors0/warnings0·Begin(false 상태)collector0를 확인하고18:03:58 소스4132 rawexact/복구쓰기0·자원을 반환했습니다. 활성 런타임·APK9는0입니다. 독립 검토에서 성공 뒤 error의 sample 중복을 reader가 잘못 묶는 문제를 찾아 새 후보를 준비 중입니다. QA8 실패 자료와 source01 컴파일은 그대로 보존합니다.

상세 공개 증거는 로컬 `D:/codex/validation/sudden-force-fps/20261113-resume/qa8-runtime-control01/ACTUAL_RESULTS.ko.md`, `full-return01.public.json`, `independent-actual-review01/` 및 `qa8-signing-preparation01/actual02/`에 있습니다. 원시 로그·키·credential·설치 APK 본문은 공개 저장소/PR에 추가하지 않습니다. 이 PR은 진단 미완료로 Draft를 유지합니다.
