# B1 사전검사 중단과 정상 복구 (#259)

2026-11-02 첫 실행 회차(cohort10)의 결과입니다. **새 Editor 열기와 임시4파일 배치 뒤 native 연결 사전검사에서 중단했으며, 명시적 C# 진단8개·Play·실제 두 피어·Navigation·B1·L2 입력은 실행하지 않았습니다.** 원래120초 보호 조건은 유지했습니다. 이후 별도 종료 전용 계약으로 같은 native 프로세스의 정상 ExitCode0을 확인하고 자기4파일만 정리했습니다.

대상 P11은 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`, HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`/기존 dirty18의 별도 QA 경로입니다. 최신 main 통합 Player가 아닙니다. 기본 P2는 `D:/codex/worktrees/sudden-force-fps-ui-20261018`이며 당시 HEAD/origin/main `f3b59d404b5eebd79a888a244d97148b9a8379a5`와 기존 설정5개를 보존했습니다.

## 소스 준비와 실제 실행의 구분

C11 source32 manifest는 `6069088AC7E1C040878DB3156C64BE1AAF28A84018F881C87A66623B41034ECC`입니다. 독립 Source IR 11,224B/SHA `50FB08D5B277568816A060B6A40CFA3F0119AD0D2FDBFD5F0913BFA590EE6EA0`, Control IR 5,256B/SHA `F20BE86440E076D1AF8E8492A777653460965C754702D18CCBAABCC5DF12E766`의176개 함수·닫힌 식 시험이 통과했습니다. 이는 실제 Unity API·C# 진단·입력 수용과 별도입니다. C10의 immutable legacy2 경로 불일치와 실패 IR은 보존하고 C11에서 두 consumer의 whitelist만 바로잡았습니다.

실제 준비는 Root22·pre-editor stage lease·scope를 새로 발행하고 CS2/meta2를 CreateNew한 범위입니다. 배치 직후 공개3,984개 raw exact·기존 dirty18·추가 자기4개 exact를 확인한 뒤 P11을 한 번 열었습니다. 계정·키·버전 원장·전역 저장은 사용하지 않았습니다.

## 중단 이유와 보존

| 관측 | 결과 |
| --- | --- |
| 고정 Python `-X utf8 -I -B`의 driver02 기본 실행 | sibling import가 격리 모드에서 실패해 exit1. Enable/권한 발행 전 발견했으며 원본 보존 |
| driver03 Python6 | 명시 sibling file load·common SHA 결속으로 기본 비활성6개 exit0. 소스 bootstrap 읽기와 operational IO0을 구분 |
| driver03 native binder | 매개변수 `$Home`이 PowerShell의 읽기 전용 `$HOME`과 충돌해 본문 전 exit1 |
| driver04 native binder | `$HomePath` 최소 수정 후 호출했으나 `FRESH_NATIVE_120_SECONDS`에서 exit1. 보호 조건을 완화하지 않음 |

마지막 거부 뒤 Root는 이 회차를 최종 중단하고 추가 runtime lease·owned-start·입력 config 발행을 금지했습니다. 원래 Root/stage/source/실패 자료를 바꾸거나 marker를 지워 재시도하지 않았습니다. 새 진단8개는0회입니다. Editor의 자연 import/컴파일 가능성과 읽기 전용 `editor_status`의 ready·stopped 관측을 typed 진단 컴파일 성공으로 합산하지 않습니다.

## 종료 전용 메타와 같은 native Exit0

원래 runtime binding의120초 조건을 통과한 것으로 꾸미지 않고, 별도 중단 영수증에 결속한 **늦은 정상 종료 전용** metadata7·sidecar·intent만 발행했습니다. sidecar의 `executionAuthorized/freshNativeGateAccepted/ownedStartIssued/runtimeLeaseIssued/cleanupAuthorized`는 false이며 `noFurtherRuntimePublicationOrInput=true`입니다. 이것이 B1 입력 권한은 아닙니다.

현재 P11 native PID22580/start `2026-11-02T06:04:20.3830761Z`·실행 파일·canonical project를 Process/CIM으로 재확인했습니다. raw commandline은 내부 식별에만 일시 사용하고 출력·저장하지 않았습니다. 종료 전에 실제 handle을 열어 보유한 Watcher의 HANDLE_READY를 확인한 다음, 제목/화면으로 식별한 clean Untitled P11 main 창에 정상 Alt+F4를 한 번 보냈습니다. 강제 종료·전역 저장·게임 입력은0입니다.

동일22580의 결과는 native **정수 ExitCode0**, known/held-retained/wait-signaled/HasExited=true·timeout=false입니다. Watcher tool session59974의 실제 완료 exit0·Dispose 오류0을 별도로 확인했습니다. native 종료 시각은 `2026-11-02T06:13:46.3463753Z`이며, fresh owned0/unknown0/TempLock=false를 확인하고 공유 GUI/Editor/heavy 자원을06:14:32 UTC에 반환했습니다. 당시 임시4개는 아직 보존 중이었으므로 그 반환을 파일 정리 완료로 표현하지 않았습니다.

종료 전용 metadata의 반환 기한은 공유 자원 한도를 따라 원 stage 기한보다1.03798초 이릅니다. 복구 코드의 비교는 이 더 짧은 기한을 허용하는 `<=`로 한정했고, 기존 stage/Root/중단 영수증의 시각은 수정하지 않았습니다. 입력/반환 권한을 연장한 수정이 아닙니다.

## 자기4파일 정리와 사후 감사

runtime lease/owned-start가 없는 중단 회차에 기존 정상 runtime cleanup13을 위조해 사용하지 않았습니다. 별도 recovery-only cleanup02를 독립 검토한 뒤 Root가 명시 Enable했습니다. Root의 fresh closed0·공개3,984행/dirty18·자기4개 audit, 중단/원 stage/종료 전용 sidecar, 같은 native held Exit0과 실제 tool0/Dispose0을 결속했습니다.

stage의 정확한 절대 target4개와 C11 CS2/별도 meta2 보존본을 **전체 삭제 전에** 바이트·SHA로 확인했습니다. Root audit의 owned4 경로는 상대경로이므로 고정4개 relative literal로 대조하고, 실제 파일 조회·삭제에는 검증된 stage 절대경로만 사용했습니다. 이전 절대/상대 경로 혼동 후보는 실행하지 않고 보존했습니다. intent를 먼저 만들고 비재귀 `Remove-Item -LiteralPath`로 자기4개만 제거했습니다. marker·다른 asset·워크트리·키 삭제는0입니다.

실제 cleanup receipt SHA는 `BF98D61FA5F15DF9946342E100A0B360966711BFAA39C4AB46D26FA15201A6C5`입니다. 그 뒤 별도 감사 SHA `7D26FE5DF11C785D78238E0E7221155EE6411D90ADCA4F5F61963016C4E609B7`에서 공개3,984개 raw exact·기존 dirty18·자기4개 부재·P11 private6 actual absent/indexS와 내용조회/해시0을 확인했습니다. Root 공개 PS1행은 원바이트 해시에 포함했고 비공개6과 구분했습니다. P2 설정5개도 다시 해시해 기존 값 일치를 확인했습니다.

이 보존 결과는 실행 중 SDK/엔진 메모리·Library cache 또는 게임 상태 전체 무변화를 뜻하지 않습니다. 사후 scoped guard는 owned0/unknown0/TempLock=false이며, 이번 typed 진단8/Play/Navigation/Queue/Fire/직접ADB/Android 실행은0입니다.

## 다음 회차와 미완료 범위

C11 회차의 source/권한/marker를 재사용하지 않고, 새 C12/cohort11 source32·별도 H/AREA/phase를 준비했습니다. source manifest는 `151620BBC3D970F2A9EC0CC01A2FD49A50E4805A9674C10B476924D83120AB52`이며 독립180개 통제 시험은 소스 준비 수용입니다. Rootdriver05의 기본 비활성8개·현재 IR consumer 대조도 별도 소스 검토 범위입니다. 실제 C12 실행 결과는 후속 기록으로 남겨야 합니다.

다음에는 peer 실제 반환/새 자원 동의 후 남은 hard stop/return 초에서 여유를 빼 초기 기한을 발행하고, **Open→현재 native PID 식별→binder를 같은 Root tool에서 연속 실행**합니다. 정상 두 피어·새 geometry/route·B1 첫 accepted shot·중립과 실제 사망/리스폰 L2는 아직 남아 있습니다. B1 lease를 L2에 재사용하지 않습니다.

PR261 Draft/#259 OPEN과 실제 제공 Android34/1.0.196/internal release12/source `61218033ff36a2da3770ad93b9cc6c10c105c92d`, 권위원장 `196,2,34`를 유지합니다. 이번 회차의 새35 예약·게임 AAB/APK·서명·Play 업로드는0입니다. 실기기·발열/장시간 성능·전체 권리·실제16KB는 기존 후순위/미검증 범위입니다. 현재 필요한 사용자 조치와 질문은 없습니다.

외부 근거는 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/`의 `cohort10-final-pc-checkpoint01.public.json`, `input261-b1-cohort10-abort-before-runtime01.public.json`, `input261-b1-cohort10/`의 stage/open/closure-only/native-exit/recovery-cleanup/fresh-closed 영수증, 전후 public audit 및 독립 source/control/driver/recovery 검토 폴더입니다. 원시 CLI/로그·commandline·계정·키 내용은 공개하지 않습니다.
