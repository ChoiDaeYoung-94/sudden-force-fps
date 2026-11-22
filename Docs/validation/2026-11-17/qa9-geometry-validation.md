# QA9 실제 화면 정렬 진단 준비

Refs #404, #402, #401. 기준은 Draft PR403의 `fd4a2a1`이며 일반 제품 PR400과 구분하는 QA 전용 후속입니다. QA8 실패 코드·산출물·원래 문서는 기존 Git 이력과 검증 자료에 보존합니다.

QA8은 실제 Untagged 플레이어 카메라에 MainCamera 태그를 요구해 readiness-timeout이 발생할 수 있었고, 긴 JSON 로그도 잘렸습니다. QA9는 유일한 input owner의 직렬화된 `_localCamera`가 활성 자식 카메라 정확1개와 같은지 확인합니다. 카메라 태그·입력·조준선·게임 상태는 변경하지 않습니다.

진단은 Android+LOCAL_QA, package `com.AeDeong.SuddenForceFPS.localqa`, versionName `1.0.197-qa9`, 실제 package versionCode9에서만 시작합니다. 현재 게임 namespace `gaitprod1-window1-shoulderref1-localqa`를 그대로 확인합니다. 일반 제품·다른 package/version에서는 collector/JNI 작업을 시작하지 않습니다. APK 안에는 PC lease/native/PID binding이 없으므로 실제 기기 수집은 별도 새 원창·실행 PID·서명 APK·ADB 세대로 결속합니다.

화면/safeArea·조준점·부모 RectTransform·카메라 pixelRect/중앙 ray·순수 SimulationShotOrigin의30필드를 같은 자연 Canvas 렌더 프레임에서 최대8회 읽습니다. 최초 owner 뒤3초 준비·전체300초 상한과 자체 callback 제거/collector 파괴를 유지합니다. 실패 시 마지막 준비 조건을 고정 enum으로 남깁니다.

UTF8 JSON은 row당8192바이트·384바이트 조각·최대22parts/row로 제한합니다. 각 로그 전체 길이850바이트를 전부 검사한 다음 기록하며, run/wireRowIndex/frame/sample/part index/count/크기/SHA와 Base64로 재조립합니다. wireRowIndex는 성공 뒤 같은 sample의 오류행도 구분합니다. 계정·닉네임·AppID·장치 식별자를 진단 로그에 넣지 않습니다.

11/13 source02의 독립 오프라인13사례와 Android+LOCAL_QA 비활성 RAM 컴파일은 준비 근거입니다. 이번 활성 후보는 DiagnosticsEnabled false→true 한 곳만 변경하며 reader 알고리즘은 유지합니다. 옛 비활성 Begin 호출 helper를 활성 후보에 재사용하지 않습니다. 기존 script GUID는 같은 진단 역할로 유지하고 파일명/클래스를 LocalQa9GeometryDiagnostics로 맞춥니다.

실제 활성 C# 컴파일·새 freeze/export/서명 APK9·설치·실제8행 수집·collector 정리·#401 원인 확정은 아직 진행 전입니다. 운영 code34/release12·원장196,2,34·기존 키·일반 Android PGS/Play는 변경하지 않습니다. 타 프로젝트와 공유 자원을 조율하고 경기 시작 전 스크립트를 준비한 뒤 필요한 만료 검사는10초 fixture로 줄여 확인합니다. 이 문서는 준비 상태이며 실제 정렬 수용이나 배포 완료가 아닙니다.
