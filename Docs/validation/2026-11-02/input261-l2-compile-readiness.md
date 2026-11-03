# L2 C12 컴파일·기본 false 검증과 자원 반환

관련 작업은 [#322](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/322)입니다. **C12에서 Unity 미호출 compile4와 기본 false4를 확인하고 정확 own4를 정리해 자원을 반환했습니다. 독립 검토는 이 공개 영수증 범위를 수용했습니다.** Play·Queue·게임 입력·Android/Native L2는 실행하지 않았습니다.

## 소스 수정과 검토 범위

C12는 RUNTIME_GATED_DEFAULT_DISABLED의 외부 소스63개, manifest18,759B/SHA `CD760EBA0A0948AA52DAE486C68E0ABE533E4C1F9A7F2432895DF5E8FF44982D`입니다. M1의 현재 respawn tuple/body/SHA 결속, M2의 READY seed1/progress seed1·firetrue0, M3의 bootstrap 검증 후 고정 command leaf 읽기 순서를 보존했습니다. 이는 실제 사망·리스폰·SDK 관측과 구분합니다.

C11 M4는 manifest에 등록된 compile-queue-l2-only01.cs가 RuntimeAdmission03.STATIC_RUNTIME에서 빠져 CLI 전 입장에서 거부된 결함입니다. C# 문법 오류나 Unity 컴파일 실패가 아닙니다. C12는 정확한 이름1개를 추가하고 revision 경로·DAG·참조·embedded wrapper SHA를 재결속했습니다. current63 역변환과 compile4 RUNTIME 입장, unknown/다른 경로/RETURN 음성을 새로 확인했습니다.

원 C11 false IR와 M4 이전 superseded true IR는 보존하며 현재 승인으로 재사용하지 않습니다. C12 Source IR의 sourceAccepted/noBlockers=true와 Control IR의 controlledTestsAccepted/noBlockers=true는 각각의 소스·합성 범위입니다. Source379·별도 consumer12·Control408·이번 실제 결과의 개수는 합산하지 않습니다. 소스 IR가 actualCompile/actualRuntime/ready=false였던 시점과 이후 실제 검증도 구분합니다.

## 원 시간창과 Editor 신원

실제 자원 창은 **2026-11-02T12:35:46.9877595Z 시작 → 12:39:08.6907958Z 반환 체크포인트**입니다. 원15분 산술값12:50:46.9877595Z보다 짧은 peer cap을 적용해 신규입력12:48:00Z/반환12:50:00Z를 유지했습니다.

첫 initial은 FRESH_120_SECOND_RECEIPT에서 H/stage/native 발행 전에 중단됐습니다. 실패와 옛 감사시각을 보존하고 새 audit03/guard02로 재검사했으며 원 창을 다시 시작하지 않았습니다. 실제 lease는12:36:46.6427770Z 시작, 신규입력 stop12:47:39.6427770Z, 반환 deadline12:49:39.6427770Z로 더 짧았습니다. Rootbinding raw expiry로 실제 lease/peer cap을 대체하지 않았습니다.

실제 PID2384/start `2026-11-02T12:36:49.4593700Z`/ticksString `639292198094593700`/game MVID `910cc62f-c725-4cf1-89fb-3e381255f2c9`입니다. 전·후 InspectEditor는 같은 신원과 Direct3D11/batchfalse/playingfalse/compiling·updatingfalse/Untitled scene1·sceneDirtyfalse/frameCap−1/DOTNET1·BEE1을 기록했습니다. UTC 소수부 표기는 오른쪽 zero padding만 적용해100ns 의미를 유지했으며 반올림·절삭이나 신원 교체가 아닙니다.

## 실제 compile4·default4

| 대상 | 결과 | 범위 |
|---|---|---|
| Host compile-only | 성공·진단0·sourceStable | delegate 미호출, 등록/input0 |
| Queue compile-only | 성공·진단0·sourceStable | admission 통과, Queue 미호출/input0 |
| Nav compile-only | 성공·진단0·sourceStable | navigationInvoked=false, input/callback/file0 |
| Shot compile-only | 성공·진단0·sourceStable | diagnosticInvoked=false, query/state/input/Queue0 |
| Host 기본 false | 성공·진단0·sourceStable | DISABLED_PRE_BODY, 등록/input/file0 |
| Queue 기본 false | 성공·진단0·sourceStable | enabled=false, Queue/file0·native 미실행 |
| Nav 기본 false | 성공·진단0·sourceStable | DISABLED_PRE_BODY, input/callback/file0 |
| Shot 기본 false | 성공·진단0·sourceStable | enabled=false, query/state/input/Queue/file0 |

compile4는12:37:35~38Z, default4는12:37:39~41Z의 별도 영수증입니다.8건 모두 outer/inner success·sourceStable=true, diagnosticCount0이고 sourceSha256Before와 실행 후 sourceSha256이 같습니다. Queue compile 입장 누락 수정은 실제 미호출 경로에서도 통과했습니다.

이는 enabled 동작의 성공이 아닙니다. geometry compile은 실제 head query·적중이 아니며 Queue compile은 Queue 실행이 아닙니다. Play0/normal helper0/게임 입력0/Queue0이며 새 방·두 피어 경기·Nav 이동을 하지 않았습니다.

## CLI 실패와 같은 held native 종료0

12:38:01Z editor-close는 **CLI exit6/success=false**, wrapper exit1입니다. 이 응답을 성공이나 native 종료0으로 바꾸지 않고 실패로 보존합니다.

사전에 보유한 같은 PID/start handle의 native-exit은12:38:22.2030956Z에 waitSignaled/hasExited=true, timedOut=false, handleRetainedThroughExitRead=true와 **정수 nativeExitCode0**을 기록했습니다. 별도 native-waiter-completed는 watcher tool session93636의 tool 종료0/Dispose errors0과 같은 native-result rawSHA를 결속합니다. native0과 tool0/Dispose0은 서로 다른 공개 관측입니다.

이른 closed-after-c12-native-exit01.public.json은 이름과 달리12:38:12Z에 ownedPID2384/owned1/scopePassed=false였습니다. 실패를 보존하고 이 상태에서는 삭제하지 않았습니다. held exit 뒤12:38:36Z guard02는 owned0/unknown0/TempLockfalse·산술일치였습니다. raw CommandLine 반환·저장과 타프로젝트 제어는0입니다.

## own4 정리와 공개 바이트 보존

후속 guard02·fresh audit·held/tool/Dispose 증거를 결속한 cleanup을 수행했습니다. staged-create는 bridge source2개와 각 meta2개, 정확4개를 만들었고 새 디렉터리는0이었습니다. cleanup은 그4개 bytes/SHA를 확인한 뒤 제거했습니다.12:38:38Z receipt는 sourceOwnedRemoved4/ownedStagedPathsRemaining0·재귀 디렉터리 삭제0을 기록합니다. 원본·실패·기존 자료는 보존했습니다.

| 감사 | 결과 |
|---|---|
| precompile03 | public3984 일치·원 dirty18·owned 추가0 |
| precleanup | public3984 일치·원 dirty18+정확own4만 추가 |
| postcleanup | public3984 일치·원 dirty18·owned 추가0 복귀 |

세 감사의 공개3984 행과 원 dirty18 목록이 같고 changed/missing/added는 비었습니다. private6은 absent/indexS 메타만 확인하고 내용 읽기·해시0입니다. 공개 PS hash 포함과 private6 보호는 별개입니다. 최종 체크포인트의 P2 보호5개 rawSHA도 기존값과 일치합니다. P2 설정5개와 P11 개인6은 서로 다른 보호 범위입니다.

cleanup receipt의 public3984PostAuditPending=true는 삭제 직후 상태이며 사후 보존의 근거는 이후12:38:39Z postcleanup audit입니다.12:39:08.6907958Z는 반환 체크포인트이며 정확한 OS 창 파괴 시각·종료 지연의 계측값은 아닙니다.

## 실제 독립 검토와 남은 범위

실제 독립 IR는 ACCEPT_PARSED_ACTUAL_COMPILE4_DEFAULT4_NATIVE_EXIT0_CLEANUP_RECEIPTS/noBlockers=true입니다. 미호출 compile4/default4·무Play Editor·같은 native 종료0·반환/cleanup 영수증 연결 범위만 수용합니다. receipt-chain29·final-binding3·직접 공개 입력111핀·current alias2를 확인했으며 소스·합성 검사 개수와 합치지 않았습니다.

고정 공개 소스·영수증·감사 metadata를 대조한 검토입니다. 프로젝트 디스크·P2 설정·P11 PS·private6·raw CLI/SDK/trace나 OS/기기를 다시 관측하지 않았습니다. public3984/d18/private6/P2보호5는 공개 감사 수준의 수용입니다. tool 완료/Dispose0도 별도 공개 영수증이며 독립 검토자가 그 tool 실행을 직접 관측한 결과는 아닙니다.

nativeL2/timing/packet/L2/ready·새 실행 권한은 false입니다. 보편적인 Editor 종료 안정성, live 입력 동작, 물리 gesture 연속성이나 L2 준비 완료를 추론하지 않습니다. 최초 freshness·CLI·early guard 실패도 성공에 합산하지 않습니다.

이번 Android/LD·SDK·Chrome·native instrumentation은0입니다. recorder prefix/원anchor 실제 검증·held gesture·Host 사망/리스폰·Android association·local lifecycle은 미실행입니다. recorder에는 공유 NetworkID·HP/death/kill 자체가 없으므로 실제 Host↔Android 연결과 외부 attestation이 별도로 필요합니다. 누락된 facts/receiveNs를 추정하지 않습니다.

사용자가 받은 **Android34에 이 외부 준비 소스·정책 변경이 포함됐다고 주장하지 않습니다**.35 예약·새 빌드·서명·업로드와 배포본 변경은 없습니다. #259와 PR261 Draft의 남은 범위를 유지하며 이번 기록을 L2 실행 완료로 확대하지 않습니다.

## 공개 근거

기준은 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452`이며 개별 compile/default·native/guard/cleanup 영수증은 `input261-l2-cohort01/`에 있습니다.

- C12 Source IR: `l2-family12-gated-independent-review01/verification.public.json`, SHA `1C2AA521E508BB0C813F246DC7EA73BFF38B2F85661607C602ABA72E50644872`
- C12 Control IR: `l2-family12-controlled-tests01/verification.public.json`, SHA `48DE196A1F87BE72D82BECE30D84FEEEB709BA593FC69905BFE3BA6102A04583`
- 실제 독립 IR: `l2-family12-actual-independent-review01/verification.public.json`(9,410B), SHA `D1166D3BBA905CAC5B8D6A2ED4568C4FC3A8E9A941183374D9A1B3D20ED2DA61`
- 반환 체크포인트: `l2-c12-compile-return-checkpoint01.public.json`, SHA `14A279EF6D87E753A75E92685133D231736DA3E7B935AC7E27007D8337346F1C`
- postcleanup audit: `l2-c12-postcleanup-public-audit01.public.json`, SHA `D244FEE20F7575A4BD9D133AB9D6791132350F0BF9415F18E0432CF457F2E29A`

공개 문서에는 판정·필요 핀만 남기고 raw 로그·credentials·계정 식별자·개인 내용·대형 dump를 싣지 않습니다. 다음은 별도 새 자원 창에서의 L2 준비·실행 검증입니다. 현재 사용자님께 필요한 조치나 질문은 없습니다.
