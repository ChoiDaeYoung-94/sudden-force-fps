# L2 Host 보조 코드의 컴파일·비활성 경계 확인

2026-10-29 별도 P11 Editor에서 첫 후보의 C# 컴파일 실패를 보존하고 최소 수정한 새 후보의 실제 컴파일·타입·기본 비활성 호출을 확인했습니다. 두 Editor의 정상 종료 코드는 각각 같은 보유 핸들에서0을 읽었습니다. 이번 검사는 Play·공개 사격 입력·B1 normal poll·Android native held lifecycle을 실행하지 않았으며 PR261 Draft/#259 OPEN을 유지합니다.

## 대상과 첫 실패

P11은 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`, detached `f055e4a84391bd9d8741c4e12855887b1ef72166`과 기존 QA/설정 변경18개입니다. 최신 main clean checkout이나 새 Windows Player 빌드가 아닙니다. 기존 Player 폴더에 외부 검증용 C#2개와 meta2개만 임시 배치했습니다. 제품 저장소에는 보조 소스를 추가하지 않았습니다.

첫 실행 PID60676/start `2026-10-29T05:45:25.6544396Z`에서 메인 보조 코드197행133열의 CS0029를 확인했습니다. Unity6000.3.25f1의 `SceneHandle`을 JObject 값으로 바로 대입할 수 없었습니다. 05:48 중간 로그에서 같은 오류 표식9개·고유 위치1개를 관측했으며, 이를 최종 전체 로그 오류 수로 표현하지 않습니다. 타입 관측·미호출 wrapper·Queue(false,null)·B1·L2는 실행하지 않았습니다. 소스 검토의 noBlockers가 실제 컴파일 성공을 뜻하지 않았습니다.

candidate01의 소스·manifest·실패 원본은 보존했습니다. 별도 candidate02는 sceneHandle JSON 값의 명시적 int cast 한 곳과 외부 Home 경로만 수정했습니다. 타입/파일명·프로토콜·공개 입력 호출 범위는 유지했고 독립 delta 검토 후 새 자원 동의와 별도 출력에서 실행했습니다.

## 새 후보의 실제 확인

두 번째 실행의 실제 시작은 `06:08:23.5127272Z`, 새 단계 중단 한도는 `06:21:23.5127272Z`, 전체 반환 한도는 `06:23:23.5127272Z`입니다. lease는 자기 P11 GUI/컴파일·비활성 검사만 허용했고 FPS VM·직접 ADB·합성 Host 입력·helper·빌드/서명·키·Chrome은false였습니다. 첫 상태 조회 EDITOR_TIMEOUT과 창 활성화 도구 timeout도 실패로 보존했습니다.

| 관측 | 실제 두 번째 결과 |
| --- | --- |
| 프로세스 | PID6668/start `2026-10-29T06:08:54.4485184Z`, 직접 start ticks `639288509344485184` |
| Editor | nonbatch/DX11, DOTNET1/BEE1, Play·compile·update·compilationFailed=false |
| 타입/어셈블리 | `SffL2Validation.SffL2BoundedHostBridge02`, Assembly-CSharp/MonoBehaviour, main MonoScript class·소유 GUID2개 일치 |
| 모듈 | bridge/game/source/pointer의 MVID가 모두 `1fee4a1d-5206-43f8-9e02-5a796bce8a51` |
| 기본 계약 | Queue 인자2개, 기본 enabled=false/phase=null, Queue(false,null) 결과의 disabled 계약 일치 |
| 비활성 전후 | 보조 instance0→0, callback 배열 불변, clean Untitled/scene1/preview0·SceneDirty false→false |
| 미호출 wrapper | delegate 생성, invoked=false/registered0/inputCalls0, CLI/inner success·진단0·source stable |

위 MVID는 이번 프로세스 안의 네 타입 결속 관측이며 이전 회차 전체와의 모듈 유일성을 주장하지 않습니다. 타입 검사기는 메타데이터를 읽고 Queue(false,null)을 한 번 호출했습니다. uncalled wrapper는 별도 delegate를 생성했으며 그 본문을 실행하지 않았습니다. Host 활성 Arm·B1·L2는0입니다.

## 정상 종료와 원본 보존

두 실행 모두 `unity close <P11> --timeout 30 --format json`으로 정상 종료를 요청했고 force를 사용하지 않았습니다. 별도 watcher가 실제 native Process.Handle을 종료 전 보유한 뒤 WaitSignaled/HasExited·strict int ExitCode0을 읽고 영수증을 쓴 후 Dispose했습니다. watcher tool0·disposeErrors0도 별도로 확인했습니다.

- 첫 native 종료 관측 영수증의 finishedUtc는 `05:49:02.9980001Z`, 소유4파일 정리·3,984개 감사 후 실제 반환은 `05:50:06.1362872Z`였습니다. compile/workflow 전체 수용은false입니다.
- 두 번째 native 종료 관측 영수증의 finishedUtc는 `06:14:08.7506193Z`, 소유4파일 정리·3,984개 감사 후 실제 반환은 `06:14:33.0382732Z`였습니다. finishedUtc를 정확한 kernel 종료 시각으로 확대하지 않습니다. 이번 컴파일/비활성 검사만 통과했으며 fullRuntimeAcceptance=false입니다.

각 실행에서 새 closed guard의 native/unknown0·TempLock=false, 외부 원바이트와 정확한 소유4파일 대조를 확인한 뒤 네 파일만 제거했습니다. 새 폴더·재귀 삭제는0입니다. 각 전후 감사의 기존 공개3,984개 path/bytes/SHA가 같고 비공개6개는 absent/indexS, 기존 dirty18·untracked0을 유지했습니다. private 내용 읽기/해시는0입니다. 게임·VM·ADB·SDK 설치·Player 빌드·서명·키·Chrome·global Save Project는 실행하지 않았습니다.

이번 native0은 새 두 프로세스의 관측입니다. [이전 실제06의 exitCode=null](input-witness-forward-ticks-and-exit-unknown.md)을 수정하거나 #154 Windows Player 종료 원인을 해결한 근거로 사용하지 않습니다. 종료 로그의 일부 패턴0도 native0이나 전체 오류 부재를 대신하지 않습니다.

## 근거와 다음 단계

외부 근거 root는 `D:/codex/validation/sudden-force-fps/20261029-work`입니다. 원시 Editor/CLI 로그·quarantine·키/개인 파일은 Git과 독립 raw 검토에서 제외했습니다.

| 공개 근거 | SHA256 |
| --- | --- |
| l2-host-bounded-bridge02-scenehandle-fix-independent-review01/verification.public.json | A3D466DAA8EF8C38846B5A7E813330645DECA66D2A9967FEECDD7907479F8357 |
| l2-bridge-first-compile-actual-independent-review01/verification.public.json | 6267814A011E4296C8EA074CAEA9739F89910B97488C21D66D77368606473798 |
| l2-bridge-second-compile-actual-independent-review01/verification.public.json | 2BC553E060C84533FC486C7727171ECAFDC923D8587E14318501632339A3FD5F |
| l2-bridge-compile-attempt02/bridge-type-disabled01.public.json | 39A9359A913D051ED814A332B65C31D7A53B50FA2412DD5CA68C4F65B92616B6 |
| native-exit-handle-retention-candidate03/native-l2-bridge-compile02.public.json | 9CC16AAB70F353D5720921903CE80B744FDA0D7DCA13A1A9918CC9BFF1A9ED55 |
| p11-after-l2-bridge-cleanup02.public.json | 7A414BEA59324F9AE06D551191EA600FBF24CB159A27FA239ADB2C8F795BE360 |
| l2-bridge-compile-attempt02/actual-completion-compile-disabled-pass.public.json | 247D2F0D44CAD03BBCF15F4A31935D80CDCBB2936AC7323A76B7D50429F88DF5 |

다음 단계는 별도 fresh lease·새 프로세스/epoch/원문 핀으로 정상 두 피어 B1의 공개 입력→SDK normal poll과 bounded stop을 확인하는 것입니다. 이후 별도 Android native held FIRE/LOOK·사망/정상 리스폰·stale 차단·fresh Down 재무장과 route/실제 OS 경계를 검증합니다. 지금은 그 결과를 채우지 않습니다.

제공본은 코드34/1.0.196/source612 QA/내부 release12이고 권위 원장196,2,34·새35 예약/통합 빌드0을 유지합니다. 실기기·실제16KB·발열·장시간 성능은 후순위이며 현재 사용자 조치와 질문은 없습니다.
