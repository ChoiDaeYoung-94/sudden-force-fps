# 공개 Joystick 콜백의 작은 이동과 해제 관측 (#259)

2026-10-31 C9의 새 `attempt03`입니다. P11은 HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`/기존 dirty18의 별도 QA 작업 경로이며 최신 main 통합 Player가 아닙니다. source32 manifest `8DDEFFD65715A6F038C426D4B629363BB5E71D1C233B021237540C92918D7D02`, sourceIR `5F651D0F02BEC2DD3112DD915852CD017C29C8BB09D4E11DA372BFF2F619AA93`/21,778B와 controlled89/89(함수·AST66+portable 모델23, Python 구문10은 별도)는 실행 전 검토입니다. C8 source27·manifest와 이전 marker는 불변이며 HostMain `9B6152…`의 사격 입력 로직도 바꾸지 않았습니다.

새 native PID42700/start `2026-10-31T02:36:48.1665190Z`, 직접 ticks `639290110081665190`, game/source MVID `70d22ef4-f2ef-4d14-a8df-dee79f7e62b9`를 결속했습니다. Root22·NavRootContext13과 기존 Writer21/Host23/normal19/close17을 분리했고, source/control IR의 top-level 단일 폴더 alias는 원본과 같은 원바이트/SHA입니다. Native 시작 시각 끝의0과 직접100ns ticks를 보존했으며 Root/lease의7자리 UTC는 발행 전에 형식을 정했습니다. 기존 lease/원장/실패 기록을 수정해 맞추지 않았습니다.

실제 컴파일/비활성 검사6개가 각각 innerSuccess=true·진단0·sourceStable=true였습니다. Host와 Navigation의 typed Func2개는 생성만 했고 호출하지 않았습니다. Host·Queue caller·Close caller·Navigation 기본 false4개는 등록/입력/파일 IO/종료 호출0입니다. 새 Navigation 본문41,132B도 미호출 함수 안에서 실제 C# API/타입 검사를 받았습니다. `EditorUtility.scriptCompilationFailed=false`와 새 MVID·idle clean scene을 확인한 뒤 정상 두 피어 Game을 열었습니다.

소유 CS/meta4개만 임시 배치했으며 normal helper9개는 첫 binder 호출에서 성공했습니다. 디렉터리 생성 script의 stdout에는 이전 C8에서 복사된 `priorFailedBindHadNoOutputDirectory=true` 필드가 남았으나 C9 실패 사실이 아닙니다. 원본을 보존하고 `root-normal-directory-observation.public.json`에 최초 부재 assertion·외부 디렉터리2개 생성·C9 binder 실패0/성공1을 정정 기록했습니다. 실행 권한과 후보 source는 이 정정으로 변경하지 않았습니다.

기존 Android34와 PC가 정상 닉네임/방 참가·서로 다른 팀 READY/START로 DesertHouse에 진입했습니다. Game focus=true·noHardware=true·Joystick neutral의 새 read-only18 epoch, 현재 native/assembly의 compile/defaultfalse 영수증과 source/control IR를 검증한 publisher로 proposal→Root의5개 예상 SHA 승인→scope/lease/config/활성 소스4개를 CreateNew했습니다. Root input stop13분 안에 별도40초 Nav 유효 구간을 발행했고 route상한5초·hold0.75초·sample 누적1m·단일 waypoint만 허용했습니다. `navigation-dispatch-intent`와 실제 start intent는 각각1개이며 같은 phase 재실행은0입니다.

활성 Navigation source SHA는 `7EB03C5EFC48EF4062B796ABA84750790320182BF357164649176ED4F76D8521`입니다. 같은 Host/life/source에서 `JoyStick.PointDown→Drag→PointUp/조건부 Cancel`의 합성 공개 UI 콜백만 사용했습니다. desktop에서 MovementArea가 숨겨져 있어도 별도 활성 Joystick component가 보유한 NormalizedMove를 자연 Source.Update의 joystick fallback이 읽는 경로입니다. 보이는 터치 UI·실제 Android/OS 하드웨어 입력 통과로 확대하지 않습니다. preview/SetActive·private field·HP·actor transform setter·직접 Source.Update/Snapshot/OnInput 호출은0이며 기존 공개 콜백의 UI handle transform 변경은 발생했습니다.

계획은 Host `(5.625,0,-13.5)`에서 x+0.25m의 목표 `(5.875,-13.5)` 한 점이었습니다. 결과는 `ROUTE_TARGET_REACHED_AFTER_NEUTRAL_POLLS`, helper 측정 약0.4126637초, Down1/Drag7/Up1/Cancel1·callback poll14입니다. 결과 마지막 sample x=5.903931로 순 XZ 변위0.278931m, sample 누적3D 이동0.294861734m, 목표 오차0.02893114m<허용0.1m였습니다. Y는 sample에서 약0.03m로 관측됐으며 기기 성능·물리 이동 전체의 일반 통과로 합산하지 않습니다.

`released/callbacksRemoved/joystickNeutral/releasedPositionStationaryObserved=true`, 해제 뒤 자연 frame6개와 stable frame3개를 기록했습니다. 안정 판정은 code의3frame/0.12초·0.03m 허용 관측입니다. 별도 후속 read-only 관측의 동일18 epoch에서 x=5.928792로 추가0.024861m가 있었고, 총 순 XZ 변위0.303792m·목표 오차0.053792m는0.1m 안이었습니다. **완전한 위치 고정·즉시 정지 증거는 아닙니다.** 원래 sample과 후속 위치를 수정하거나 하나의 시각으로 합치지 않았습니다.

stored-input은14개 sample 모두 `inputAvailable=false`, observedMoveTicks=0·normalPollNeutralObserved=false·inputPacketAcceptanceProven=false입니다. 결과 reason의 `NEUTRAL_POLLS` 문자열을 실제 neutral packet 입증으로 읽지 않습니다. Source.Update 직접 계측도false이며 실제 Android touch·native held 입력·사격·사망/리스폰·B1/L2 수용은false입니다. 공개 UI 중립과 범위 내 위치 안정의 좁은 관측으로 기록합니다. game state를 시작 위치로 되돌렸다는 productStateRestoreClaim도false입니다.

B1 Writer/Host/Queue/fire 설정·Writer 프로세스·enabled B1 Queue는0이고 자동Queue도0입니다. 새 Navigation용 Root/lease/config/승인 source는 실제 존재합니다. 결과/intent/소비된 승인/원본 source를 보존했으며 재발행·기존 nonce 재사용·추가 이동·재arm은0입니다.

정상 StopHost→clean Untitled→framecap -1 복원과 idle scene1/dirtyfalse를 확인했습니다. 자기 PID/start/exe를 검사하는 close1회는 CLI6/COMMAND_FAILED로 보존했고, 별도로 같은42700의 사전 보유 handle에서 알려진 정수 ExitCode0·handle retained·wait signaled/hasExited를 확인했습니다. watcher6828 tool0·disposeErrors0, 강제 종료0입니다. Android는 정상 Public Rooms 복귀→EXIT GAME/OK→홈 화면을 확인했습니다.

현재 stage/initial ownership/runtime lease/native binding/held exit/fresh closed·audit를 결속해 소유4파일만 제거했습니다. 최종 Root 감사는 공개3,984개 raw exact·기존 dirty18·private6 actual absent/indexS·owned4 absent·native0/unknown0/TempLock=false입니다. 전역 저장·키/자격정보·Android 예약/빌드/서명/업로드는0입니다. 변경 전후 공개 파일 보존은 실행 중 모든 SDK/엔진 메모리·캐시나 game state 무변화의 주장이 아닙니다.

독립 실제 검토는 compile/false6개·이번 단일 합성 UI 위치 관측·같은18 epoch·정상 반환/동일 native 종료·소유4 정리·보존을 한정 수용했습니다. Root의 공개3,984행 metadata가 전후 exact이며 **ProjectSettings.asset을 제외한 공개3,983개/749,467,382B를 직접 재해시**해 모두 일치시켰습니다. PS1행과 private6 부재/indexS는 Root metadata 대조이며 독립 실물 조회·해시는0입니다. 실제 IR `resume-0124/c9-actual-independent-review01/verification.public.json`/SHA `4EBAD4CAD98D67F68D98D2BDD1D5BD8909AEE195F93450C577CEADD281690DAD`/49,229B는 실제 결과용이며 운영 sourceIR21,778B를 변경하거나 대체하지 않습니다.

운영 sourceIR/89개 통제 시험은 별도 준비 근거입니다. 독립 검토자가 UI JPEG3개를 파일로 확인했으나 Host 결과 직후 캐시/Android Rooms/Home 화면·원바이트·저장 시각의 좁은 범위이며 정확 capture UTC는 별도 미확보입니다.

외부 근거는 `D:/codex/validation/sudden-force-fps/20261031-work/input261-b1-attempt03/`의 compile/defaultfalse6개·fresh navEpoch/postneutral·publication proposal/explicit approvals·dispatch/start/result·RETURN/self-close/held native·currentepoch cleanup·public-after-cleanup입니다. source/controlled 검토 원본은 같은 날짜 `resume-0124/input261-family09-full-source-review01/`와 `input261-family09-controlled-tests01/`이며 top-level alias와 구분합니다. 원시 CLI·로그·프로세스 commandline·private6 본문/해시는 공개하지 않습니다.

다음은 사선을 확보하는 별도 계획과 B1 정상 입력 관측 방법을 검토한 뒤 새 scope에서 사격·사망/리스폰 수명 경계를 검증하는 것입니다. 이번 단일 작은 이동을 긴 route·native 이동·PR261 전체 완료로 일반화하지 않습니다. PR261 Draft/#259 OPEN·제공 Android34/release12/source612 QA·원장196,2,34·새35 예약/빌드/서명/업로드0을 유지합니다. 현재 필요한 사용자 조치와 질문은 없습니다.
