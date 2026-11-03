# Basic admission proposal02: M1 보완

문서만이며 실제발행0·원본/falseIR 불변입니다. top17 descriptor는 proposal01 schema를 계속 검사하고, evidence20의 각 pin 원문은 새schema의 receipt로 검사합니다. 기존 beforeCall/prearm/paired는 요약이며 새fullfacts를 대체하지 않습니다.

wrapper는 exact7(schema/role/rootSessionId/scopeId/clockSessionId/parentPins/facts)입니다. facts/부모는 receipt.oneOf에 닫힙니다. parent pin은 descriptor.evidence[role]와 path/bytes/SHA가 모두 같고 전체graph는 DAG여야 합니다. W=wrapper rawSHA, B=facts.body의 원본문SHA입니다. parentPins에는W, 기존원본문의manifest/config/source/review 필드에는B를 대조합니다. self/foreign/미해결pin은 거부합니다.

raw8역할의 body는 재래핑하지 않고 원bytes를 읽습니다: manifest는 rev02 exactmanifest+source3 실제bytes; sourceReview/controlReview는 고정IR02 schema·수용scope·current manifest/source raw핀; source17BaselineMetadata는 C16 V3 exact17; source07Source는373954…8571 고정bytes; source07Review는 Root가 검토한 고정원IR 전체bytes; deviceScope는 Source07.validate_config18+installfalse; deviceBefore는 primitive.verify_device_before28입니다. 고정raw등록이 없거나 bodyvalidator가 없으면hardblock이며 새bool로 대체하지 않습니다.

나머지12역할 facts는 모두 새producer 제안입니다. baselineRawAudit는 injected17 원bytes로 primitive 재실행, currentAudit는 source2각1회+GameData/observer before/after raw/LF·compiledTypes/MVID, nativeBinding은 직접PID/UTC7/ticks/exe/project, heldReady는 같은native handle·미종료/미dispose를 검사합니다. installedProductProvenance.artifactBundle은 Source07.artifacts 원validator+고정game-build raw등록을 모두 재검증합니다.

beforeInvocation의 config/source/review/body SHA는 해당B와 같고 action=observe-before·같은scope/clock의 실제호출구간·정수exit0/timeoutoverflowfalse·command metadata 원bytes가 필요합니다. before/after 공통body만으로 출처를 추정하지 않습니다.

prearm은 최초정상이력/원하한, prefix는 seq1·원수신Ns, androidContext는 그prefix의 실제선택행과 동일birth/local/life입니다. geometry는 동일context·FIRE좌표/원10초입니다. association은 Android birth/local IDs와 PC networkId/PlayerRef/Runner/life를 같은room/native/currentAudit에 결속합니다. sample raw=observerSample body, 동일actor/life/native/MVID·after frame/tick/Ns>before·finite Aim/Camera를 요구합니다.

capture/sample은 같은Root Python mono receiver PID/birth/session입니다. 다른origin변환·수신앞당김0. 원13/15·return2·anchor+2<100·child잔여min·geometry10·current source2와old17분리·foreignL2거부를 유지합니다. getter/producer 출처 미확인은hardblock이며 raw/body/type/parent/time 교차검증 전 수용0입니다.

source07Review는10568B/260DEEBFD653082283C4FD579A87EDD12B30AF8CCCD85B927246E5C6BC6799D2 원bytes만 허용합니다.
