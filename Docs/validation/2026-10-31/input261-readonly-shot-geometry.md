# 정상 두 피어의 첫 충돌 읽기 전용 진단 (#259)

2026-10-31 C8의 새 `attempt02`입니다. 대상 P11 HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`/기존 dirty18이며 최신 main 통합 실행이 아닙니다. C7의 READY 전 실패·원래 phase/nonce·모든 source/marker를 보존했습니다. source27 manifest `0850BBD48AFA3A776D3CB3CC165D6306E863CDF1E34C3B6338BA07C39253AD76`, 전체 sourceIR `B1772E874F0C8ECAFA156F777556EA8930EEC7FFA87B5FBC0140CFD99E4C4641`/28,899B와 offline controlled62/62는 실행 전 검토이며 실제 결과와 구분합니다.

새 native PID44380/start `2026-10-31T02:14:27.8841596Z`, 직접 ticks `639290096678841596`, game/source MVID `445a7e56-1066-4181-85cf-df403206b77c`를 fresh Root20·stage/runtime lease·native binding·사전 보유 handle에 결속했습니다. Host C#2+literal meta2만 임시 배치했고, 원래13/15분 구간을 연장하지 않았습니다.

실제 컴파일/기본 비활성 검사6개가 각각 innerSuccess=true·진단0·sourceStable=true였습니다. Host/진단 typed Func2개는 생성만 하고 호출하지 않았으며 Host·Queue caller·Close caller·새 진단의 기본 false4개는 등록/입력/조회/파일 IO/종료 호출0입니다. Python 구문/통제 모델만으로 C# 컴파일 통과를 추정하지 않았습니다.

normal helper binder 첫 호출은 Root가 빈 출력 디렉터리를 아직 만들지 않아 `ROOT_PRECREATED_EMPTY_OUTPUT_ONLY`로 실패했습니다. 출력0·phase 디렉터리 부재를 확인하고 Root가 외부 phase/normal 디렉터리2개를 CreateNew한 뒤 새9개 helper를 정상 생성했습니다. 기존 실패 source/툴 기록은 보존했으며 B1/Queue 재시도는0입니다. Root note는 원시 오류 파일이 아닌 실제 tool traceback 관측과 source 대조 범위를 명시합니다.

PC 호스트와 기존 Android34 FPS가 정상 닉네임/방 참가·서로 다른 팀 READY/START로 DesertHouse에 진입했습니다. actor 원점은 Host `(1.875,0,-13.5)`/적 `(-5.625,0,13.5)`였습니다. C7의 Host 시작 x=-5.625와 다르므로 다음 결과로 과거 C7 실패 원인을 확정하지 않습니다.

첫 활성 진단은 입력을 놓은 Game focus=true에서 C7과 같은 one-tick recoil predicted ray와 현재 head.Position 직접 ray를 공개 `CombatShotQuery.TryCast`로 각1회 조회했습니다. 두 경로 모두 약2.95971918m 앞 `Maps/Wall_04`·layer8/non-trigger를 첫 충돌로 반환했습니다. target head 거리는 약27.90605m, 두 ray의 각도차는 보고 float0·head 거리에서 lateral 약0.00000723m였습니다. 이 관측은 현재 배치의 월드 충돌을 구분한 것이며 실제 발사나 다음 명중을 입증하지 않습니다.

Android 조이스틱을 native UI로 왼쪽 드래그1회 `(114,654)→(50,654)`했습니다. 이후 **진단 호출1회에서 query2개**를 추가해 총 diagnostic2회/query4개입니다. 두번째 관측은 actualFocused=false·noHardware=true를 그대로 보고했으며 focused Host preflight 통과로 바꾸지 않습니다. 서버 target 위치의 전후 순변위가0이므로 native 조이스틱 이동 수용은 false입니다. 터치 이벤트 계측이나 중간 전체 궤적 관측은 없으며 이동하지 않은 원인을 확정하지 않습니다.

각 조회 안에서 선택 actor 공개 필드가 전후 같았고 양쪽 HP100/ammo30/shotSequence0을 유지했습니다. 진단 본문의 input/SeedAim/Queue/callback 등록/state write/file write는0입니다. 이는 닉네임·READY·native 드래그 등 정상 UI 조작 전체가0이라는 뜻이 아닙니다. query 내부 SDK 캐시/lag compensation 메모리 무변화·실제 다음 발사·B1 수용은 모두 미증명입니다.

B1 Writer/Host/Queue/fire 설정 발행·Writer 프로세스·enabled Queue는0입니다. Rootbinding·stage/runtime lease·normal admission·진단 파생·close 설정은 실제 존재하며 '모든 config0'으로 표현하지 않습니다. B1/native L2/timing 수용은 false이고 C7 실패를 같은 phase에서 재Queue하지 않았습니다.

정상 StopHost→clean Untitled→framecap -1 복원·idle/clean scene1을 확인한 뒤 자기 PID/start/exe를 검사하는 close caller를1회 실행했습니다. CLI는 exit6/COMMAND_FAILED로 보존하고, 별도 동일 epoch의 보유 native handle은 알려진 정수 ExitCode0·retained through read·wait signaled/hasExited를 확인했습니다. watcher tool0·disposeErrors0이며 강제 종료는0입니다. Android는 Public Rooms 복귀→EXIT GAME/OK→홈 화면을 확인했습니다.

같은 stage/initial/runtime lease/native/held exit에 fresh closed/audit를 결속해 소유4파일만 제거했습니다. 최종 Root 감사는 공개3,984개 raw exact·기존 dirty18 보존·private6 actual absent/indexS·owned4 absent·native0/unknown0/TempLock=false입니다. 전역 저장·키/자격정보 사용·Android 예약/빌드/서명/업로드는0입니다.

독립 실제 검토는 현재 C8의 컴파일/기본 비활성·미호출6개, query4개의 현재 첫 충돌 관측, 동일 native 정상 종료·소유4 정리·보존을 한정 수용했습니다. Root 감사3개의 공개3,984행 metadata가 exact이며 **ProjectSettings.asset을 제외한 공개3,983개/749,467,382B를 직접 재해시**해 모두 일치시켰습니다. PS1행과 private6 부재/indexS는 Root metadata 대조 범위이고 독립 실물 조회·해시는0입니다. 실제 IR `resume-0124/c8-actual-independent-review01/verification.public.json`/SHA `EEBC0AECC7614DDB878EFD112DB996D1C3486CF48B7257B9B9C321FDF169CDB4`/42,742B는 별도 실제 결과이며 운영 sourceIR B177…/28,899B를 변경하거나 대체하지 않습니다.

source-only IR/62개 통제 시험을 실제 진단 결과나 B1/L2 성공으로 대신하지 않습니다. 독립 검토자가 UI JPEG5개를 파일로 확인했으나 화면·원바이트·저장 시각의 좁은 범위이며 capture UTC는 별도 확보하지 않았습니다. 캐시 Host 화면 추가 보존도 새 실행/재캡처가 아닙니다.

근거 root는 `D:/codex/validation/sudden-force-fps/20261031-work/input261-b1-attempt02/`의 compile/defaultfalse6개·shot-geometry 관측2개·UI/준비 Root note·정상 RETURN/self-close·held native·cleanup/public-after-cleanup입니다. full source IR와 controlled 검토는 같은 날짜 `resume-0124/input261-family08-full-source-review01/` 및 `input261-family08-controlled-tests01/`입니다. 원시 CLI는 private quarantine이며 공개·독립 원문 재열람 범위로 합산하지 않습니다.

다음은 별도 새 단위에서 검토된 공개 조이스틱 callback의 작은 이동·중립/정지를 확인하고 정상 재위치 후 B1/L2를 진행하는 것입니다. 이동 초안 revision02의 delegate 타입·동일 frame 시간해제·최종 목표거리 결함3개와 revision03 수정/모델14개는 source-only 결과로 보존하며 아직 실제 컴파일·입력 승인이 아닙니다. PR261 Draft/#259 OPEN·제공 Android34/release12/source612 QA·원장196,2,34·새35 예약/빌드/서명/업로드0을 유지합니다. 현재 필요한 사용자 조치와 질문은 없습니다.
