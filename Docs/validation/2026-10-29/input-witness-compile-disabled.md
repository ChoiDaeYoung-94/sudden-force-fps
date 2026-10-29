# 입력 계측기 최초 실제 컴파일과 비활성 경계 — 2026-10-29

Refs #259, #166. PR261은 Draft이고 #259는 OPEN입니다. 이 기록은 새 후보의 컴파일·비활성 경계와 정리 범위이며, 활성 입력이나 게임 수용 기록이 아닙니다.

## 대상과 완료 기준

이전 수집기는 활성 Arm에서 NullReferenceException으로 중단되어 paired 본문과 패킷 수집이0이었습니다. null-safe witness02와 caller03의 독립 소스 검토 후, 별도 P11에서 자연 컴파일·실제 타입·미호출/비활성 경계를 처음 확인했습니다. 완료 기준은8개 개별 응답의 inner success/진단0·기대 상태, 소유 임시2파일의 정확한 바이트와 타입, 원본3,984개 보존과 종료·정리입니다.

- P11: `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`, HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`에 기존 검증용 소스 overlay를 보존했습니다. 최신 P2/main과 동일한 전체 제품 체크아웃으로 간주하지 않습니다.
- witness02 SHA256: `A39B46A4F291A273401435D605BA3BF492AD7B40F1CC66B13C2870E55F3D456A`.
- caller03 SHA256: `85EF92A8B470ED86ADE46C52EA465A32269850F5D198A5E02D40F58477B8C283`.
- 기존 독립 source IR SHA256: `2C56FC89B4653E71DF2C2667A292E8513AF4839D264869D467C8FB2F7AE6AFDD`. 소스 판정과 이번 실제 검증은 별도 근거입니다.

## 실제 확인한 범위

2026-10-29 09:17–09:23 KST의 자기 Editor에서 GUI/D3D11·DOTNET_PROCESSOR_COUNT=1·BEE_BUILD_THREADS=1, clean Untitled·Play/compile/update/compilationFailed=false를 실제 관측했습니다. PID1564/시작 UTC00:17:22.5028851Z와 Assembly-CSharp MVID `e94bd0ea-00f6-41d7-a474-e9eec784d802`는 당시 신원이며 다음 실행에 재사용할 승인이 아닙니다.

배치한 파일은 `Assets/Scripts/Creature/Player/SffSimulationInputWitnessCandidate01.cs`와 `.meta` 두 개뿐입니다. 자연 Assembly-CSharp 타입, MonoScript 타입/GUID, witness/source/pointer MVID 일치와 기존 witness 객체0을 확인했습니다. Editor 전용 소스이며 새 asmdef나 제품 변경을 추가하지 않았습니다.

| 대상 | 미호출 컴파일 | 기본 비활성 실행 |
| --- | --- | --- |
| witness Arm | Func 미호출, COMPILE_ONLY_NOT_INVOKED | DISABLED, armed=false·componentId0 |
| witness detach | Func 미호출, DETACH_COMPILE_ONLY_NOT_INVOKED | DISABLED |
| caller queue | Func 미호출, registered0·bodyInvoked=false | DISABLED_PRE_BODY, registered0·bodyInvoked=false |
| caller detach | 정의만 컴파일, DETACH_COMPILE_ONLY_NOT_INVOKED | DISABLED_PRE_BODY, detached0 |

8개 개별 응답 모두 CLI exit0·outer/inner success=true·diagnostics0이며, 입력 소스의 실행 전후 SHA도 동일했습니다. 각 wrapper 응답 자체에는 PID/start가 없으므로 같은 프로젝트의 실행 전·타입·실행 후 관측으로 epoch를 연결했습니다. Play·활성 등록·실제 입력·패킷 수집은0입니다.

## 보존과 실패 구분

초기 Editor 미연결1회·시간 초과2회는 컴파일 대기 중 응답이며 통과로 합산하지 않았습니다. 첫 Root gate는 PowerShell의 ISO 시각 자동 DateTime 변환으로 문자열 비교가 달라져 타입/wrapper 호출 전에 중단됐습니다. 원본 실패를 남기고 `ConvertFrom-Json -DateKind String`으로 첫 실제8개 검증을 진행했습니다.

종료 eval은 COMMAND_FAILED/CLI exit6이었습니다. 이 응답을 성공으로 고치지 않았고, 별도로 계속 보유한 같은 프로세스 핸들의 ExitCode0/UTC00:23:10.1674477과 후속 native Editor0·unknown0·TempLock 부재를 확인했습니다. 첫 종료 중 관측의 PID 잔류도 정상 종료 판정에서 제외했습니다. 강제 종료는 없었습니다.

stage 전후·종료 후·임시파일 정리 후 공개3,984개는 경로/크기/SHA가 모두 같고, private6은 부재/indexS입니다. 정확한 SHA·GUID·NoReparse와 외부 사본 보존을 확인한 자기 CS/meta2개만 종료 후 제거했고 untracked0을 확인했습니다. Library·컴파일 캐시와 전체 메모리 원복을 주장하지 않습니다.

## 근거와 다음 작업

공개 근거는 `D:/codex/validation/sudden-force-fps/20261029-work/`의 `witness-compile-attempt01/`, `p11-witness-before-stage01.public.json`, `p11-witness-after-cleanup01.public.json`, `checkpoint-compile-only01.public.json`에 있습니다. 실제 명령은 고정 프로젝트의 `unity command editor_status`, `eval_file`로 타입/8개 wrapper를 순차 실행하고 `audit-public01.py P11 ... --baseline ...`로 공개 입력을 감사했습니다. 원시 로그·자격정보는 공개하지 않았습니다.

독립 실제 검토 SHA256은 `3487E8B7CA9398970AB7EAEF467C8E17CEB02966082309AD45A1A4FD29A68F36`, 판정은 FIRST_ACTUAL_COMPILE_DEFAULTFALSE_EIGHT_WRAPPERS_AND_OWNED2_CLEANUP_ONLY 범위 noBlockers=true입니다. 검토자는 공개 실제 영수증과 배열을 대조했으며 Editor 실행이나 전체 live 재해시를 대신 수행했다고 주장하지 않았습니다.

다음은 새 fullgame 자원 차례와 fresh epoch·정상 두 피어 context·바인딩 이후의 실제 등록/패킷 검증입니다. 원 NRE의 정확 fault line과 해결·실제 OS/route·사망/리스폰·PR261 전체 수용은 미완료입니다. 제공 코드34/source612 QA와 후속 main, 원장196,2,34·새35 예약/빌드0을 유지합니다.
