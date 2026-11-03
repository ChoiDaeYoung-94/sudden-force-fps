# Input261 L2 cohort02: 컴파일 확인 후 사전 준비 중단

2026-11-02 C14의 실제 PC 검증에서는 미호출 컴파일 4건과 기본 false 실행 4건을 확인했습니다. 이어진 첫 `publish-l2-artifacts01.py --action prepare-directories` 호출이 `LOCAL_SLASH_PATH`로 실패해 PC Play 전에 중단했습니다. 동일 native handle의 종료를 확인하고 임시 소스 4개를 정리한 뒤 자원을 반환했습니다. 실제 L2·Native 입력·timing·packet·gameplay는 수용하지 않았습니다.

기록 단위는 [#324](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/324)입니다. [전회 L2 컴파일 기록](input261-l2-compile-readiness.md)과 이번 회차를 구분합니다. [#259](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/259)는 열림, [PR261](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/261)은 Draft 범위를 유지합니다.

## 실제 범위

| 항목 | 결과와 한계 |
|---|---|
| 자원 창 | 시작 `14:00:13.3337310Z`, 반환 `14:04:04.8897812Z` |
| 원래 상한 | 실행 중단 `14:13:00.0000000Z`, 정상 반환 `14:15:00.0000000Z`; 재설정 없음 |
| C14 | 63개 source, manifest `2F7AFE304816BFA15E36A1030CBF750CBE41C79A7FA845CB04D6511108041654` |
| Editor | PID 25488, Direct3D11, clean Untitled, Play false, cap -1 |
| 컴파일 | Host bridge·Queue caller·Nav·shot geometry 4건, 각각 진단 0 |
| 기본 false | 위 4계열 비활성 경로, 각각 진단 0·입력 0 |
| normal helper | 9개 결속·admission 준비 성공, 실제 실행 0 |
| 첫 publisher | `prepare-directories` exit 1, `LOCAL_SLASH_PATH` |
| 중단 이후 | Play·Nav·Queue·normal helper·ADB·LD·Native instrumentation 모두 0 |
| Operator03 | 이번 회차 실제 호출 0 |
| 종료·정리 | held native ExitCode 정수 0, watcher tool 0·Dispose 오류 0, owned4 제거 |

외부 검증 루트 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452`를 `N`, `N/input261-l2-cohort02`를 `H`로 줄여 적습니다. 시각은 UTC입니다. 원본 반환 기록은 `N/l2-cohort02-return-checkpoint.public.json`입니다.

준비 영수증에는 원 상한보다 이른 `newRunStopUtc=14:12:54.9275590Z`, `normalReturnDeadlineUtc=14:14:54.9275590Z`가 남아 있습니다. 이 기한도 보존했으며, 준비·검증 지연을 이유로 lease나 종료 시각을 갱신하지 않았습니다.

## 컴파일과 비활성 경로

`H`의 `bridge-compile-only`, `queue-compile-only`, `navigation-compile-only`, `shot-geometry-compile-only` 공개 영수증은 각각 CLI exit 0, success/innerSuccess/sourceStable true, diagnosticCount 0입니다. delegate 생성 후 실제 동작을 호출하지 않았고, Nav callback 등록·shot query도 0입니다.

대응하는 네 `defaultfalse` 영수증은 비활성 경로를 실제 호출한 결과입니다. 각각 진단 0이며 공개 결과의 입력·파일 읽기/쓰기·Queue·query는 0입니다. 이를 true 경로의 Host 구동·이동·사격·Android 생애주기 성공으로 확대하지 않습니다. source 검토·합성 검사·실제 컴파일·기본 false 호출 개수는 합산하지 않습니다.

`editor-beforeplay.public.json`과 `editor-before-close.public.json`의 result는 같습니다. PID·시작 시각·MVID가 같고, sceneCount 1, previewCount 0, scenePath 빈 문자열, sceneDirty false, playing false, frameCap -1, DOTNET/BEE 각각 1입니다. 이는 공개 Editor 상태의 일치이며 모든 내부 메모리·제품 상태의 완전 복원 증명이 아닙니다.

native 시작은 `2026-11-02T14:00:18.2382543Z`, ticks 문자열 `639292248182382543`, MVID `f8603513-81cf-4ecb-bfbc-2b3ed6f52a94`입니다. 100ns 일곱 자리와 정수 ticks를 보존했습니다.

## 경로 계약 실패와 입력 전 중단

실패 지점은 반환 체크포인트의 `config2_producer01.validate_scope -> local(native.projectPath)`입니다. 실제 `native-binding.public.json`의 projectPath/nativeExe는 Windows 역슬래시 경로이고 소비자의 local 계약은 forward slash를 요구했습니다. projectPath 검사에서 먼저 거부됐습니다.

이는 Python 사전 준비 계약의 실패입니다. C# 컴파일 실패와 구분하며, 앞선 컴파일 통과로 이 실패를 무시하지 않습니다. 원 native-binding·수용 IR·실패 호출은 수정하지 않고 보존했습니다. native 신원 관측이 틀렸다고 단정하는 근거도 아닙니다.

첫 호출은 bridge/reader 디렉터리 생성과 PC Play 시작 전에 멈췄습니다. normal Play 영수증, Source17 actual audit, device02 config도 없었습니다. fresh prefix→config2→reader SDK batch→Queue/READY→Native gesture→외부 death/respawn 연결→L2 판정에는 도달하지 않았습니다. 과거 PID·기기 관측·epoch·설치 영수증을 이번 성공으로 재사용하지 않습니다.

## 선행 source 이력과 현재 판정

| 단계 | 보존한 이력 |
|---|---|
| C13 M5 | 원 true IR와 M5 재현 후 false IR(`17B1…`, `63DD…`)을 함께 보존했습니다. |
| C14 provenance | helper XML의 N 공개 alias와 metadata projection 연결을 보완했습니다. 별도 review `098639…`는 metadata 범위이며 새 서명·crypto 재통과·키 사용이 아닙니다. |
| Operator01 | 숨은 private namespace 선행 읽기 M1과 false IR `281D…`를 보존했습니다. |
| Operator02 | 고정 Python clock API 차이 M2와 false IR `39B2…`를 보존했습니다. |
| Operator03 | namespace 거부와 `time.monotonic_ns()` 정합의 소스 검토 IR `F05C…`가 있으며 실제 호출은 0입니다. |
| C14 M6 | 이번 실제 경로 실패에 대한 후속 Source/Control false addendum을 별도 연결합니다. |

`root-prepared-scope.public.json`에 기록된 당시 Source SHA는 `73A1C8D041AE6021ED0C3D762BFC1416D0EE4FBEECA0BBFD82D96C1DC64B3C1D`, Control SHA는 `5299D6F359D996A57DB51B7FC91C409B5867D68078D7F9C28B2ADF964C9E74B5`입니다. 이 선행 기록으로 M6 후속 판정을 대체하지 않습니다. C14 향후 runtime 사용은 차단 상태입니다.

후속 독립 검토에서도 M6를 재현해 Source·Control의 현재 승인을 철회했습니다. 실제 영수증 검토는 컴파일·기본 false·정상 반환·보존의 부분 범위만 수용하며 전체 준비/L2의 noBlockers는 false입니다.

- C14 Source M6: `N/l2-family14-gated-independent-review01/addendum01/verification.public.json — SHA812845A95B31673E785977C55DCA2FDE833105BBF57D7D3DDBDF92E60E63EA40, 현재 noBlockers=false`
- C14 Control M6: `N/l2-family14-controlled-tests01/addendum01/verification.public.json — SHA8718AC1B4ABB1A84412C1562E173864A043D82BBD85F2A84B7A25F997CC3287D, 현재 controlledTestsAccepted=false`
- cohort02 Actual 독립 검토: `N/l2-family14-actual-independent-review01/verification.public.json — SHA35D3451AA7B01986CE4B938E72C69B32D60E007D92082F812BFE2C57D096FD6A, 공개영수증26검사·부분수용/overall noBlockers=false`

## CLI 실패와 동일 held native 종료

`editor-close.public.json`은 CLI exit 6, success false, COMMAND_FAILED입니다. wrapper exit 1도 보존했으며 성공으로 바꾸지 않았습니다. 별도 `native-exit.public.json`은 종료 전에 보유한 같은 PID/start handle로 waitSignaled/hasExited/exitCodeKnown true, timedOut false, nativeExitCode 정수 0을 기록합니다. 종료 완료는 `14:02:33.8855451Z`입니다.

watcher 세션 37255의 실제 tool 완료·exit 정수 0·Dispose 오류 0은 `native-waiter-completed.public.json`에 따로 연결돼 있습니다. native exit에서 tool/Dispose 성공을 추론하지 않았습니다. 종료 intent의 nativeExitVerified false도 발행 당시 의미대로 보존했습니다. 강제 종료·다른 프로젝트 제어·원시 CommandLine 저장은 없습니다.

## cleanup과 보존

첫 cleanup 준비는 hash 입력 파일 경로 오타로 Get-FileHash에서 실패했습니다. producer 미호출인 이 실패를 보존하고, 올바른 기존 fresh audit(`349A…`)를 명시해 다시 준비했습니다. 이후 `cleanup-config.public.json` SHA는 `E7F07256A3CFE42AF140B7B9822EF19D490916F9C9E450AF0F1D05E96949E914`입니다.

fresh guard는 owned P11 0, unknown Unity 0, TempLock false를 확인했습니다. `H/public-before-cleanup.public.json`은 cleanup 입력용 audit alias이며 SHA는 `5A65386F6518F98304E936CC2EB71A7814CF02512067F7C77128406E6888BD57`입니다. 원 fresh audit 이름·SHA와 alias 핀을 구분합니다.

`owned-source-cleanup.public.json`은 staged C# 2개와 meta 2개만 제거했습니다. sourceOwnedRemoved 4, ownedStagedPathsRemaining 0, 재귀 디렉터리 제거 0입니다. 그 영수증의 public3984PostAuditPending true는 삭제 직후 상태로 보존했습니다. 후속 post audit `3A0462AE992802306CE0A4ABA76EFAC64B5A0CC5FCE74EB57BE496A134586671`의 공개 3,984개 exact·dirty18 보존은 반환 체크포인트에 별도 연결돼 있습니다.

private6는 absent/sparse 등 메타데이터만 확인했고 내용 읽기·해시는 0입니다. P2의 기존 보호 설정5도 별도 exact 관측입니다. 두 집합을 혼동하지 않습니다. 원시 로그·trace·계정 식별자·credentials·큰 dump는 공개하지 않습니다. 고지 변경·서명 키 이동도 없습니다.

## 다음 우선순위와 미완료

새 revision에서 native-binding 생산 바이트와 모든 소비자의 경로 계약을 정합시키고 실제 producer→consumer 반례를 보완해야 합니다. native100ns·Root/lease/IR 핀·시간 상한은 유지합니다. C14·원 실패·false IR을 보존한 채 새 Source/Control 검토를 받아야 하며, 다음 실제 창은 fresh 감사·closed guard·현재 신원과 새 lease부터 확인합니다. 이번 창이나 과거 성공을 승계하지 않습니다.

실제 L2/Native input/timing/packet/gameplay 수용은 false입니다. 사용자에게 전달된 Android 코드34에는 이번 외부 검증 소스가 미포함이며 코드35 예약·빌드·서명·업로드는 0입니다. 정상 두 피어·Android 생애주기·사망/리스폰 연결·실제 receiveNs 증거는 별도 미완료입니다. 현재 필요한 사용자 조치나 질문은 없습니다.

주요 공개 원본의 SHA는 반환 체크포인트 `FF252ADBD91864CDD84830FE592BDEC7CE5AF1BF62D2F4F5CD5369D5CAD42237`, held native 결과 `D357113A31B0F64ED11D4F0349904FF43DEEE0B989B5AA25237B7663A28235E5`, watcher 완료 `7796441F13F4C1416F2B8E90630FA907D5AFF82A568D01E4AD7842657AEC38A9`입니다. 이 문서는 공개 영수증과 연결 메타데이터를 정리한 기록이며 OS·프로젝트를 다시 관측한 결과가 아닙니다.
