# 제품43 Android QA8 검증 후보

Refs #402, #401. 제품 코드 기준은 `8fcc73ce`이며 일반 빌드 PR #400의 로그인은 변경하지 않습니다.

기존 QA7은 gaitraw1-localqa 접속 세대라 제품43의 gaitprod1과 연결되지 않습니다. 이 QA 브랜치는 Android에 `SUDDEN_FORCE_LOCAL_QA`가 명시된 경우에만 개발용 로그인 경로를 허용합니다. 변경은 기존 조건문 한 줄이며 QA7의 해당 Login blob과 같습니다. 일반 Android PGS/Play 경로 수용을 의미하지 않습니다.

QA8 목표는 package `com.AeDeong.SuddenForceFPS.localqa`, code8/name1.0.197-qa8, `gaitprod1-window1-shoulderref1-localqa` 접속 세대입니다. Source freeze·컴파일·export·APK·서명·설치는 각 단계에서 새로 확인합니다. 운영 code34/release12와 원장196,2,34는 변경하지 않습니다.

`LocalQaGeometryDiagnostics`는 Android+LOCAL_QA와 위 package/version이 모두 맞을 때만 자체 collector를 생성합니다. Game의 유일한 유효 input owner·제품 QA namespace·owner/MainCamera·조준점·Canvas가 정합할 때, 자연 `Canvas.willRenderCanvases`에서 같은 프레임의 값을 최대8회/0.25초 간격으로 읽습니다. 최초 owner 후3초 readiness와 전체300초 상한을 두고, 중복이나 실제 namespace 불일치·성공 후 이탈은 거부합니다. 이후 자체 callback을 제거하고 collector를 파괴합니다.

기록은 `[SFF_QA8_GEOMETRY]` 접두사의 화면 크기·safeArea/캐시·조준점 화면 좌표·안전 영역 RectTransform·카메라 pixelRect/중앙 ray와 순수 SimulationShotOrigin getter 결과입니다. 닉네임·계정·AppID·디바이스 ID를 수집하지 않습니다. 자체 collector 생명주기와 제한된 로그 외에 파일·네트워크·입력·Gameplay 상태·캔버스 강제 갱신·설정 저장을 호출하지 않습니다. Origin 읽기는 같은 틱의 실제 발사 방향/명중 승인과 구분합니다.

최초 진단 후보는 전환 첫 프레임의 readiness 부족으로 조기 종료될 위험이 있어 외부 보존했고, 현재 후보는 제한된3초 대기를 적용했습니다. 실제 C# 컴파일와 Android IL2CPP stripping 후 field 접근·8샘플·자체 정리는 아직 검증 전입니다. 이 문서는 소스 준비 상태이며 #401 원인 확정이나 정렬 수정 통과가 아닙니다.

짧은 실기기 검증은 시작 전에 관측/입력/종료 script를 준비하고, 연속 터치 이동·geometry를 한 묶음으로 받은 뒤 필요한 경우 검토된 timer10초 fixture를 사용합니다. 분석은 경기 복귀 후 수행하며 5분 만료를 기다리지 않습니다. 이전 Windows의 URP 파생 변경을 고려해 Android import/export 전후 전체 소스와 실제 산출물을 구분해 보존합니다.
