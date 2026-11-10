# QA2 실행 차단·LD 로비 확인

2026-11-10 소스 `d7a3371296357a5ceb1b7b657d468415130e7047`의 QA2로 Windows–LD2 Case02와 LD1–LD2 Case03을 진행했습니다. **두 피어 Game 진입은0이며 gait/rifle 런타임은 미수용**입니다. Windows 실행 차단, LD1의 흰 화면, LD2의 정상 로비 조작을 서로 다른 결과로 기록하고 [이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)과 [Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)을 유지합니다.

[QA2 서명·LD2 갱신 설치·기본 인트로](qa2-signed-ld-install-and-cold-intro.md)의 APK는 `com.AeDeong.SuddenForceFPS.localqa`/code2/`1.0.197-qa2`, 100,492,368B/SHA `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33`입니다. 운영 `com.AeDeong.SuddenForceFPS`의 code34/`1.0.196`/release12와 원장196,2,34는 그대로입니다. 이번 단위의 SDK·Editor·휴대전화 호출은0이며, Windows GUI는 보안창 관측 외 입력0입니다.

## Case02: Windows 보안창 관측 후 중단

새 Windows QA2의 방화벽 허용 창을 관측했으나 보안창·보안 설정에는 입력하지 않았습니다. exe와 birth를 확인한 소유 프로세스만 총괄이 외부 정리 kill로 끝냈습니다. 보유 native 핸들의 ExitCode는-1, holder tool은1, disposeErrors는0입니다. holder의 `forced=false`는 총괄의 외부 kill을 나타내는 필드가 아니므로 정상 GUI 종료로 읽지 않습니다. 보안창을 관측해 이번 진행을 중단한 사실과 실제 패킷 차단·네트워크/보안 정책의 원인은 구분합니다.

LD2 caller의 일반 호출과 force-stop/home 정리는 반환됐고 설치 pair readback도 일치했습니다. 다만 최종 CLI native ExitCode는 확보하지 못했고 Root return의 tool exit도null입니다. session closed·정리 반환을 native 정상 종료로 대체하지 않습니다. 총괄은 승인/lease 회수와 원 full 이전 자원 반환을 기록했으며, Case02의 Game 수용값은false입니다.

## Case03 LD1: 정확한 새 설치와 흰 화면

LD1에는 없던 localqa 시험 패키지를 새로 설치했습니다. install/start 명령은 Exit0이고 전체 installed APK readback은 위 QA2 크기/SHA와 일치했습니다. clear/uninstall이나 다른 패키지 명령은0입니다. 후속 helper 설치도 Exit0이며 helper 전체16,528B/SHA `31B7971E51B49C2814CCEACA7F77C98529E05E732DC372890AB331995B7C520E`와 게임 APK의 전체 readback을 다시 대조했습니다. 설치 무결성과 앱의 시각 시작 성공은 별도입니다.

기본 시작은 자기 게임 foreground/PID가 관측됐지만 흰 화면이었고, 뒤의 두 screenshot SHA가 `B2779F93431D3D7A185AE67758DAA4E5DA59B639788CBF2DC03E5C99B6D0A354`로 같았습니다. 새 승인 범위의 GLES30 단일 진단도 명령 Exit0 뒤8초 표본이 같은 흰 화면/SHA였습니다. 새 PID는 관측했으나 실제 render backend가 GLES로 선택됐다는 명시 보고는 없으며 특수 진단 추가 재시도는0입니다.

소유 PID로 한정한 로그는 총괄이 startup allocator 초기화 정보(I)8행으로 관측했습니다. 공개 집계는 Unity header8·AndroidRuntime0·libc0이며 원시 로그는 공개하지 않습니다. 이 표본으로 crash 전체 부재나 allocator/그래픽/ABI 원인을 확정하지 않습니다. 기본/GLES 시각 시작과 gameplay 수용값은false이고 원인은 미확정입니다. force-stop/home과 caller native Exit0·승인 회수는 별도로 확인했습니다.

## Case03 LD2: 닉네임·방 생성·정상 복귀

LD2는 Case01의 정확 서명 QA2 설치본으로 진행했습니다. interactive caller는 일반 호출50개와 force-stop/home 정리2개를 반환했고 failedCallCount0·sourceActiveCalls0·보유 caller native Exit0을 기록했습니다. 이는50개의 독립 기능 시험이나 두 피어 경기 통과 수가 아닙니다.

Android IME에 입력한 뒤 Back을 누른 경우 입력이 취소돼 field가 비었고, OK 뒤 빈 닉네임 validation Common 팝업이 나왔습니다. 일반 배경 탭으로 validation을 닫아 nickname prompt로 돌아왔습니다. 이후 IME 확인 버튼으로 명시 commit한 시험 닉네임이 유지되는 것을 확인했습니다. Common의 별도 OK 버튼은 prefab 소스에 존재하지만 이 기록에서 실제 화면의 가시성을 수용하지 않습니다. 닫기 불가 결함이나 이를 위한 소스 수정도 확정하지 않았습니다.

정상 UI로 Desert House/정원2 방을 만들었고 Red player1명을 관측했습니다. 1명 상태 START는 차단됐으며 정상 방 나가기→Lobby 복귀와 자기 방 목록 부재를 확인했습니다. 다른 피어 합류·READY/START·Game 진입은 없었습니다.

그 뒤 force-stop과 재시작에서 nickname prompt가 다시 나타났습니다. D7의 `Managers.SavedNickName`은 빈 문자열로 초기화되는 static 메모리 속성이고, `NetworkRunnerManager.SaveNickName`도 이를 갱신하는 경로입니다(`Assets/Scripts/Managers/Managers.cs:395`, `NetworkRunnerManager.cs:581–584`). 프로세스 사이의 영속 저장 계약으로 표현하지 않습니다. 재시작 후 prompt 재등장을 clear/uninstall이나 설치 데이터 삭제의 증거로 삼지 않습니다.

## 반환·다음 단계

Case03은 양 caller native Exit0, LD1/LD2 force-stop/home Exit0, 시작된 source 호출 전체 반환·활성 호출0, config3 Root 승인과 lease2 회수, P2 보호 설정5개 일치·원 new/full 이전 자원 반환을 기록했습니다. 독립 검토는 config3 현재 승인false·lease2 회수와 archive 연결을 확인했습니다. C20/C21의 최종 archive는 이미 disabled인 바이트여서 그 시점에 새 true→false 변경이 있었다고 표현하지 않습니다. P2 설정5개 일치는 총괄 감사이며 독립 검토자가 새로 재해시한 것은 아닙니다. guest PID 부재나 전체 unknown child 종료도 독립 수용하지 않았습니다. Case02의 native-1/tool1·LD 최종 native 미확보를 이 Case03 정상 반환에 합산하지 않습니다.

LD2의 정상 설치·인트로·로비 결과와 LD1의 실행 문제를 분리합니다. 같은 서명 APK가 LD1에서 흰 화면이었다는 이유로 LD2 성공을 취소하거나 모든 Android/QA2의 실패로 일반화하지 않습니다. 반대로 LD2 로비 확인은 LD1 원인 해결·두 피어 Game·보행/소총/12 hitbox·전체 제품 수용이 아닙니다.

다음 Editor–LD2는 같은 D7/QA2 프로토콜의 정상 참가·보행/소총 관측을 위한 **소스 기반 계획**입니다. 이번 단위에서 새 Editor 실행은0이며 실제 차례·원창·입력 핀은 총괄의 후속 범위입니다. 기존 하체 preview와 과거 QA1 경기 결과는 새 QA2 Game 성공으로 복제하지 않습니다. 휴대전화·실제16KB·전체 권리·장시간 성능/발열의 미완료 상태도 유지합니다.

## 공개 근거와 검토 범위

외부 루트 `D:/codex/validation/sudden-force-fps/20261110-work`의 공개 JSON을 실제 bytes로 읽었습니다. 최종 독립 검토는 공개 실행기록·설치 readback 메타데이터·원 scope·cleanup/retirement의 일관성을 수용했습니다. 화면/방/시작 상태는 총괄의 공개 관측에 귀속하며, 검토자의 이미지/영상 직접 수용이나 installed APK의 독립 재해시는 아닙니다. 원시 로그·보안창 screenshot·개인 식별값은 저장소에 옮기지 않습니다.

| 자료(위 루트 기준) | bytes | SHA256 |
| --- | ---: | --- |
| `root-resume01/ld2-windows-qa2-case02-actual-return.public.json` | 742 | `649A51EA31B36D27ED6C0BDCE1C423D627035CCF4EA5B3AD6BD1291910BFDE58` |
| `root-resume01/windows-qa2-case02/security-blocked-cleanup.public.json` | 398 | `082BE09DB5EAC0AEE913BF0C63EEC3245232C1127845CB7D7D4A6A4CE1D20856` |
| `root-resume01/two-ld-qa2-case03-actual-return.public.json` | 1,033 | `AFA87BFB49A97DF4EF92083BFBA487039191A83BE69789286249E1D8C510107F` |
| `root-resume01/ld1-case03-install/result.public.json` | 1,799 | `8F70854262EE2C1455C7D2860C65A3F33930738EC9547B6FB5A2220D7D9DDBBD` |
| `root-resume01/ld1-case03-helper-install/result.public.json` | 1,061 | `48E4D09FC4C0DE4853837C371457F6546577E07C74C713F9809439A6287E3E93` |
| `root-resume01/ld1-case03-default-start-observation.public.json` | 513 | `FC04FD000DD6739D10F69B612DD399219E214887893B87A832B9B85A5FBA0238` |
| `root-resume01/ld1-case03-gles30-visual-observation.public.json` | 639 | `605F5476D03BD03703BB5FBC200610DF27B1DD28CBF9D229426D4DC4971EAE98` |
| `root-resume01/ld1-case03-owned-startup-diagnostics/result.public.json` | 785 | `287CB762FB8912CADA549CD31CC5042A1BF2B790D787E3328B0B2F52486EC7A5` |
| `root-resume01/ld2-case03-room-observation.public.json` | 683 | `CC7D8CA2BF3CDAA56E12CE6D339B7D9C62B977DA4C83481F95D0AB862F8D4F50` |
| `root-resume01/ld2-case03-interactive-329e5ff5615844e0aef51dbb1c959bbb/finish.public.json` | 432 | `6B1034047C16157A07E546B9155E301C45105DC232E1EAF99611FDF592855202` |
| `qa2-case02-case03-actual-independent-review01/verification.public.json` | 7,311 | `775670DA96E67D09793D54D1114060FF5C88BC3B70674BC8AF6E5279A668D297` |
