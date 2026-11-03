# #332 offline receipt graph 첫 단위

`receipt_graph01.py`는 표준 라이브러리만 사용하는 순수 함수입니다. import/default/`cli(('--enable',))`는 파일·경로 resolve·시계·환경·프로세스·SDK를 조회하지 않습니다. CLI는 잠금 결과만 반환합니다. 실제 명령줄 인수나 운영 경로를 읽는 진입점은 없습니다.

`verify_graph(descriptor_raw, expected_descriptor_pin, expected_clock_raw, blobs, descriptor_schema_raw, receipt_schema_raw, role_table_raw)`에 bytes와 정확한 dict를 주입합니다. expected descriptor pin과 expected clock은 신뢰 가능한 호출자가 제공한다는 외부 전제입니다. 공격자가 그것도 교체하면 자기 선언끼리 일치시킬 수 있습니다. filesystem reader callback 대신 명시 bytes map을 받아 verifier 자체의 I/O를 차단합니다. source에는 외부 파일 로딩 경로가 없으며 동봉 schema/table bytes를 호출자가 주입합니다.

고정 proposal01 top17/schema와 proposal02 exact7/20role facts의 모든 분기를 검사합니다. JSON은 UTF8 strict, 2MiB, 깊이64/노드100000/int64 한도이며 중복키·BOM·NaN/Inf·overflow·surrogate·bool-int 및 integer-float 혼동을 거부합니다. subset compiler는 anchor/ref, allOf/oneOf/anyOf, type/const/enum, closed object/propertyNames, array/string/number bounds를 구현합니다. 알 수 없는 schema keyword와 원격/미해결 ref는 선택되지 않은 분기에서도 거부합니다. `$schema/$id/$comment/$anchor/$defs/default`는 각각 식별·등록·annotation의 범위이며 `default`로 값을 채우지 않습니다. 고정 schema/table의 원문 SHA를 먼저 확인합니다.

성공 범위는 descriptor 신뢰핀, wrapper20의 path/size/W(SHA), 고정 부모 role set와 descriptor parent W핀, DAG, raw8의 facts.body path/size/B(SHA), 명시 B/W selectors, beforeCall의 B핀·구간 projection, nativeBinding identity projection입니다. 모든 capture.clock, heldHandleOwner, paired sample.clock을 descriptor/expected clock과 정확히 대조합니다. capture readStartNs<=readStopNs와 windowStart 하한만 검사합니다. lexical path의 dot/중복slash/stream/trailing dot-space/Windows reserved alias를 거부합니다. 이것은 OS canonical/no-reparse 또는 현재 파일·수신 clock·프로세스 birth 관측 증명이 아닙니다.

미구현 범위는 raw8의 고정 원문 registry·각 body semantic resolver, 새 producer12의 실제 provenance, source17 raw files/current source before-after/command/artifact/paired sample raw 등 중첩 pin 해석, 원본문 manifest/config/source/review 내부 결속, 전체 summary 의미 projection, calendar/UTC-ticks·freshness/13·15·return2·anchor/geometry/child deadline, held native handle·실기기/SDK 관측입니다. raw8 body는 합성 원문도 구조·핀 단계에서 통과할 수 있습니다. raw8 semantic 또는 producer12 전체 구현으로 해석하시면 안 됩니다. Source07 IR `noBlockersScope` 의미 해석과 Source17 자체 metadata→raw17 재검사는 이 단위에 구현되지 않았으며 child audit를 선행 재resolve하지 않습니다.

양성의 `graphRawPinsVerified:true`도 `semanticAccepted/actualAdmissionVerified/executionAuthorized/nativeBasicAccepted/readyToRun:false`, `runtimeHardblocked:true`를 유지합니다. 실제 admission이나 승인·nonce·lease를 발행하지 않습니다. descriptor의 approvals 값을 실행 권한으로 사용하지 않습니다.

재현 명령은 `python -X utf8 -I -B -m unittest discover -s <이 폴더> -p 'test_*.py' -v`입니다. 테스트가 자기 폴더의 공개 source/schema만 읽습니다. 테스트는 스키마에서 합성20역할 fixture를 메모리에 생성하며 운영 영수증을 발행하지 않습니다. 외부 저자 보고서 재생성기는 저장소에 반입하지 않았습니다. 모든 경로는 이 도구 폴더를 기준으로 읽습니다. 독립 검토 수와 저자 수를 합산하지 않습니다.

## 고정 원본문 의미 대조

[원본문8종 검증](RAW_BODY_VALIDATION.md)은 별도 순수 API로 추가했습니다. 고정 원문6종과 주입된 의존 source3/역사적17원문을 대조하고, config/before2종은 호출자 기대값에 결속합니다. 구조 검사와 자동 연결하지 않으며 실제 producer·실행 권한·게임 수용은 계속 false입니다.
