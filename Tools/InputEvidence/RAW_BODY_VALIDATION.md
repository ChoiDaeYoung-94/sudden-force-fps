# Raw8 순수 의미 resolver 외부 후보

`raw_resolvers01.py`는 명시적으로 주입된 bytes만 사용합니다. Source07은 고정 bytes/정적 데이터로만 다루며 import·실행하지 않습니다. Source17/deviceBefore 재검증에 쓰는 pure primitive만 17,223B/SHA45DF478F1328DA53FC31C0A8EAE8819358FDF52C91352C3F1BC9D1DFD3CBDD08 확인 후 메모리에서 실행합니다. 호출자는 `primitive_source_raw` bytes도 주입합니다. import/default/--enable은 잠금 결과만 반환합니다.

API는 `verify_role(role, raw, body_pin, blobs, expected, primitive_source_raw)`와 전체 집합용 `verify_raw_roles(role_inputs, blobs, expected, primitive_source_raw)`입니다. `role_inputs`는 role8 exact dict이며 각 값은 raw/pin exact2입니다. blobs는 원문 path→bytes입니다. role 출처·canonical path·물리 상태를 증명하지 않습니다. wrapper W 대신 original B핀만 받습니다. 고정6는 내장 registry의 path/size/SHA와 원bytes를 먼저 대조한 뒤 strict JSON·고정 topkeys/schema/typedacceptance·원본문 교차를 검사합니다. source07Source는 고정 원bytes+UTF8만 검증하며 코드의 실행 안전성을 새로 승인하지 않습니다.

manifest는 source3 실제 injected bytes를 모두 검사합니다. source/control IR는 B(manifest)와 해당 source3에 결속합니다. controlIR에 고정된 역슬래시 path는 동일 wholebyte registry를 통과한 데이터에서만 등록된 exact pair로 비교합니다. 일반 경로 정규화는 없습니다. Source07 IR는 실제 noBlockersScope selector를 사용합니다. Source17metadata는 자체 files17에서 injected bytes를 직접 얻어 pinned primitive로 재검증하며 child audit resolve를 선행하지 않습니다.

동적2 역할의 `expected` exact10은 rootSessionId/resourceScopeId/window/nowUtc/deviceScopePin/deviceBeforePin/expectedTarget/historicalGameReceipt/consolePath/consoleSha256입니다. window exact4는 issuedUtc/actualStartUtc/newInputStopUtc/hardReturnUtc입니다. config18의 기존 Root·scope·source·원13/15·return2·expected server4/console·child cap을 대조하고 allowHelperInstall은 정확히 false여야 합니다. deviceBefore28는 검증된 config에서 기대 핀/시각/scope/console을 얻고 별도 주입 expectedTarget와 historicalGameReceipt로 pure primitive를 다시 실행합니다. 이 기대값의 실제 Root 출처·현재 시간은 외부 전제입니다.

`rawRolesSemanticVerified`는 좁은 주입 원문 의미 검사만 뜻합니다. actualAdmission/nativeBasic/execution/ready/sourceAccepted는 항상 false이며 runtimeHardblocked=true입니다. producer12·실제 before action provenance·physical/noReparse/clock·역할 출처·current source/compiled causal proof·association/paired receiver·실행 권한은 후속 hardblock입니다. operational graph와 연결하지 않았습니다.

fixed-inputs는 고정원문6+manifest source3+총괄의 외부 historical17 번들의 bytes/base64를 담습니다. 프로젝트 경로를 직접 읽지 않습니다. actual config/before는 발행·읽지 않았고 테스트가 메모리에 만드는 synthetic2와 명확히 구분합니다. 전체8 양성도 fixed6+synthetic2입니다. Source17historical17은 현재 source나 실기기 증명이 아닙니다.

재현: `python -X utf8 -I -B -m unittest discover -s Tools/InputEvidence -p 'test_*.py' -v`. 기존 graph16개와 raw 본문20개 테스트를 실행합니다. 외부 저자 보고서 재생성기는 반입하지 않았으며 테스트 fixture는 source17/manifest source3 등 고정 공개 원문과 순수 primitive를 base64로 담습니다. 생산 source 자체에는 파일/환경/프로세스/SDK/시계 reader가 없습니다. author method 수와 내부 subcase·독립 검토 수를 합산하지 않습니다.
