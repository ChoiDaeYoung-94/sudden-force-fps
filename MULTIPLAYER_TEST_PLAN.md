# Sudden Force FPS 멀티플레이 검증 계획

## 현재 검증 상태와 후속 단계

**현재 상태: 단계 2 새 Windows64 빌드/실제 2 peer에서 로컬 Camera·AudioListener·몸체 표시 분리, Blue 초기 yaw/Aim seed, 이동·시점 입력과 복제, 지속 걷기/달리기 및 이탈 회귀를 아래 범위에서 확인했습니다. 단계 1 카메라 FAIL은 역사 결과이며 이번 연결 후 관측 범위에서는 PASS입니다. 벽·계단·경사/점유 스폰 회피/Android/전투는 별도 미검증으로 유지합니다.**

### 단계 2 이동·로컬 카메라 실제 검증 (2026-10-07)

Refs #33, #19, #35. 기준 HEAD `07cb3525bae273d525523345c4fed6b5c99fa751`의 전체 SHA는 증거 `head-before.txt`를 우선합니다(이동 코드 `999e1f0` + 프리팹 연결 `07cb352`). 신규 증거 루트는 `D:\meee\git\sudden-force-fps-validation\20261007-stage2-movement-01`입니다. 이전 exe는 사용하지 않았습니다. 새 Windows64 Development manifest: 성공/오류0/경고5, 268,076,937 bytes, 빌드 시간45.39초. Standalone URP_COMPATIBILITY_MODE는 승인된 임시 빌드 define이며 원복합니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| P01 Camera/Listener/몸체 | PASS(직접 관측 범위). Editor Host Red와 역할 교환 Editor Client Blue 모두 local Camera·AudioListener 각1 enabled, body renderer8개 ShadowsOnly. remote는 Camera·Listener0 enabled/body On. Player는 실제 맵/HUD·시점 화면으로 확인했으며 내부 enabled 목록을 직접 열람하지 못했습니다. |
| P02 Blue seed | PASS. 첫 경기 remote Blue transform/AimYaw180, 역할 교환 local Blue transform/AimYaw180·stored input yaw180. 포커스 복귀 후 Snapshot HasAim=true/yaw180/pitch0/move0으로 첫 입력이 0도 방향을 덮지 않음을 확인했습니다. |
| P03 실제 입력/포커스 | PASS(범위 제한). Player Blue W와 마우스 drag→Host 복제 위치/aim 변경, Red 불변. Editor Red 실제 유지 WASD/마우스·ShiftW, 역할 교환 Editor Blue Game view 클릭 후 W/drag 위치·aim 변경, remote Red 불변. 실제 창 전환은 focused=false·move0/sprintfalse·CursorNone 및 다음 경기 input owner 해제까지 확인. **키를 누른 채 focus를 잃는 경계는 NOT RUN**(관측된 focus 전환 직전 키가 이미 해제됨). Android pause/touch도 NOT RUN. |
| P04 속도/아날로그/예측 | 지속 키 관측 PASS: 수동 실제 입력 90초 read-only frame observer에서 W524프레임, ShiftW160프레임. 10프레임 이상 동일 입력/상한 유지 구간은 walk402프레임/peak5.00054, sprint127프레임/peak7.50051. 전환 직후 이전 tick 값은 제외했습니다. 조이스틱 실제 drag와 후속 입력에서 moveX/Z가 비단위 값으로 유지되며 속도 변화 확인(23 active unique frames). **아날로그 고정 크기별 정상 속도비·대각선 과속·지연/손실 예측/보정 정량 비교는 NOT RUN**. Player 순간 W 변위 약0.055m는 속도 시험의 대체 근거로 쓰지 않습니다. |
| P05 벽/ground/동선 | 부분 확인: 실제 Red가 spawn→중앙 통로→Blue 측 통로를 이동, 양쪽 grounded 및 해당 표본 capsule overlaps[] 확인. CC height1.8/radius0.3/stepOffset0.3/slopeLimit45. **벽 밀기·모서리·낮은 천장·벽 관통 재현시험은 NOT RUN**. 유지 W 중 정지 접촉을 확인할 샘플은 없었으므로 통로 이동을 벽 충돌 합격으로 확장하지 않습니다. 아트의 정적 capsule sweep은 별도 보조 증거입니다. |
| P06 계단/경사 | NOT RUN. 이번 실제 이동 기록은 평면 통로이며 허용/초과 계단·경사 동선의 합격을 주장하지 않습니다. Jump는 이번 구현 범위 밖입니다. |
| P07 각1 spawn | PASS(정상 경로). 두 경기 각각 Red/Blue Id1030/1031 각1, 이후 ActivePlayers2/roster2/spawn dictionary2 유지. 같은 NetworkId는 **새 Runner 경기에서 재사용**될 수 있으며 경기 사이의 식별자를 전역 고유로 간주하지 않습니다. 반복 callback/동시 Start 강제 호출은 하지 않았습니다. |
| P08 안전 spawn | 부분 확인: 실제 1vs1 초기 pose 및 이동 표본에 static/player capsule 겹침 없음. **같은 팀 점유 spawn 회피는 NOT RUN**, 실제 추가 peer와 점유/fallback 정책 검증이 필요합니다. 소스는 현재 임의 후보 선택이며 점유 회피가 구현됐다고 단정하지 않습니다. |
| P09 이탈 회귀 | PASS. 정상 Editor Host Game Exit→새 manager -138396/LobbyConnected/roster0/inputBoundfalse, Player 자동 kr 로비/빈 목록. Player Host 새 공개방 생성→Editor 실제 수신 목록 참가→Ready→다음 경기 성공. Editor Client Game Exit→manager -141560/LobbyConnected/receivedtrue/roster0/inputBoundfalse/manager1. Player 정상 CloseMainWindow 종료 확인. |

입력은 실제 네이티브 키/클릭/drag 및 사용자 수동 입력을 관측했습니다. Ready/session/input 상태 직접 주입이나 HUD 숨김으로 합격을 만들지 않았습니다. 입력 observer는 transient Editor update callback으로 관측만 하고 종료 시 스스로 구독을 해제하며 증거 폴더에 JSONL을 남깁니다. 첫 observer 요청의 delegate 타입 오류와 초기 Login 전 manager=null 읽기 오류는 QA eval 오류이며 제품 컴파일/게임 예외와 구분합니다. 실제 제품 C# SHA256은 검증 전후 변경0입니다.

주요 증거: `initial-two-peer-presentation.json`, `player-blue-w-after.json`, `player-blue-mouse-after.json`, `manual-held-input-samples.jsonl`, `analog-editor-samples.jsonl`, `two-peer-capsule-physics.json`, `after-movement-counts.json`, `role-exchange-blue-presentation.json`, `blue-focused-aim-seed.json`, `role-exchange-editor-mouse.json`, `host-leave-lobby.json`, `client-lobby-connected.json`, `player-A.log`, `editor-cleanup.json`. native 화면은 Computer Use 출력에 관측했습니다. 신규 경로 Player의 Windows 방화벽 알림은 사용자 직접 처리 후 닫힘을 재관측했고 에이전트가 보안 설정을 조작하지 않았습니다.

후속 우선순위: PC 벽/계단/경사와 held-focuslost, 아날로그 고정 크기·대각선/예측 tick 비교, 추가 peer 점유 스폰 회피, Android 오른쪽 touch와 조이스틱 입력 분리. HUD 중앙 mouse drag는 실제 재현에서 회전이 가능했지만 다른 raycast 영역/touch 전체를 합격으로 확장하지 않습니다.

### 2026-10-07 실제 실행 결과

증거 루트: `D:\meee\git\sudden-force-fps-validation\20261007-stage12-01`. 빌드 산출물은 `player-retry\SuddenForceFPS.Validation.exe`, manifest는 `player-retry\validation-build.json`입니다. 성공 빌드: 오류 0, 경고 5, 268,516,831 bytes, manifest 파일 364개. 최초 빌드는 URP compatibility 사전 검사에서 실패했으며 성공 빌드와 구분합니다. 총괄 승인으로 Standalone에 URP_COMPATIBILITY_MODE를 빌드 중 임시 추가했습니다. Renderer/RenderGraph 설정은 변경하지 않았습니다.

| 범위 | 실제 결과와 관측 한계 |
| --- | --- |
| L01/L02 공개방 | PASS. Player Host `SFMP-20261007-1234-01` 생성, Editor는 수신된 AD.Room 공개 목록 handler로 참가. 역할 교환 시 Player는 화면의 Editor Host 공개방을 직접 클릭해 참가. 양쪽 로그 Region kr. Editor 실제 SDK client AppVersion 빈 문자열, ClientServer/Default 확인. Player 실제 AppVersion 직접 관측 수단은 없어 동일 namespace는 공개 발견·참가 성공과 동일 소스/설정에 따른 추론으로만 기록합니다. |
| L03 3회 재참가 | PASS. 정상 ExitButtonClick→Connected Lobby→실제 수신 목록 JoinRoom을 3회 반복. 새 manager -65808→-67228→-68648, roster 0, manager 1, 닉네임 보존을 snapshot으로 확인. 최종 재참가 2인 roster와 Host 화면 확인. |
| R01 Ready/Start | PASS(실행 범위). Client 미준비 상태의 Host 시작은 방 유지, Client OnStartButtonClicked도 방 유지. 정상 ReadyButtonClick→RPC 요청 후 Host 화면/복제 상태 모두 Ready. Host 정상 시작 성공. 팀은 Red/Blue 각 1명으로 일치. 불법 팀·초과 정원·동시 요청은 NOT RUN. Ready 상태 직접 주입은 하지 않았습니다. |
| R02 맵/스폰 | PASS(씬/스폰), FAIL(카메라). Player Host 경기에서 Editor 실제 scene DesertHouse/Game, SoldierRed Id1036/InputPlayer1·SoldierBlue Id1037/InputPlayer5 각각 1개. 역할 교환 Editor Host는 roster 2·spawn dictionary 2, Red Id1030/InputPlayer1·Blue Id1031/InputPlayer2. ClientServer StateAuthority PlayerNone은 서버 권한 표현입니다. Player의 경기 HUD/맵 화면은 관측했으나 Player 프로세스 내부 객체 수는 직접 열람하지 못했습니다. |
| 카메라/HUD/입력 | FAIL/NOT RUN. Editor 클라이언트에 활성 Camera 2개(Untagged), AudioListener 2개. 첫 Player Host 경기 화면은 회색 배경/HUD, 역할 교환 Player Client는 맵/HUD 표시. 이동·전투·로컬 카메라 분리 미구현 상태이며 각 spawn 합격과 분리합니다. |
| Client 게임 이탈 | PASS. 정상 ExitButtonClicked 후 새 manager -71838, Lobby/Connected, roster 0, 공개방 count 0. Host Player는 정상 CloseMainWindow 종료. |
| Host 방/게임 이탈 | PASS. Editor Host 방 Exit 후 새 Runner 로비, Player Client도 서버 종료 수신→kr 로비 재연결·빈 공개 목록. 새 Host 방 `hostleave-03`을 Player가 실제 목록 클릭 재참가. 역할 교환 게임 시작 후 Host 게임 Exit도 Editor manager -75854/Lobby/Connected/roster0/manager1과 Player 빈 목록 복귀 확인. |
| 전투/승패/Android | NOT RUN. 구현 통합 및 별도 기기 필요. 이번 결과로 전체 게임 합격을 주장하지 않습니다. |

주요 증거: `editor-public-list.json`, `editor-joined-room-final.json`, `client-lobby-after-exit-{1,2,3}.json`, `client-ready-request.json`, `two-peer-game.json`, `two-peer-game-editor-host.json`, `client-game-leave-lobby.json`, `host-leave-new-runner.json`, `hostleave-rejoin.json`, `host-game-leave-lobby.json`, Player B/C 로그. 네이티브 화면은 Computer Use 도구 출력에서 관측했습니다. Editor handler 호출은 실제 UI와 동일한 public handler/RPC 경로이며 직접 Ready 상태 변경이나 session 목록 주입을 하지 않았습니다. Editor Host 방 생성은 public CreateRoom 설정 요청을 사용했습니다.

Host 종료 시 Player Fusion `Code 104 / Server has disconnected / DisconnectedByPluginLogic`가 debug console error 카운트에 누적되지만 이후 재연결·재참가·새 경기까지 성공했습니다. 이 종료 로그를 미처리 제품 예외로 간주하지 않습니다. Editor 콘솔 수집에는 이전 Pipeline main thread timeout 이력이 1개 포함되며 최종 groundTruth compilationFailed=false/consoleErrors=0였습니다. 과거 빌드/QA 요청 오류와 실제 게임 오류를 구분합니다.

닉네임·방 이름의 TMP textComponent에서 trailing U+200B가 관측됐습니다. exact 문자열 매칭과 정규화 후보로 인계하며 이번에는 코드 수정하지 않았습니다. 빌드 시점 C# SHA256 inventory와 종료 후 비교: 변경 0개. 종료 정리: Player 정상 종료, Editor Play=false·컴파일/import=false·빈 단일 씬 dirty=false·autotick=false, target StandaloneWindows64 유지. 원 Standalone define은 live getter와 ProjectSettings 직렬화 양쪽에서 복원 확인합니다. 직접 변경 파일은 문서/신규 빌드 helper 및 자동 생성 meta입니다.

- 다음 시험: PC 벽·계단·경사/held-focuslost·아날로그 고정 크기와 Android 실제 입력, 이후 전투 구현 단계의 C02 이후를 검증합니다.
- 후속 선행 조건: 총괄 Editor 제어권 배정, 통합 snapshot 및 관측 가능한 전투 상태. Player 실제 AppVersion 직접 관측은 아직 필요합니다.
- 실행 수단: 신규 `MultiplayerValidationBuild.BuildWindows64(root, output)` 또는 전용 batch의 `BuildFromCommandLine`. 정확한 사용법은 마지막 부록에 있습니다.
- 이번 합격 범위: 공개 목록 발견·참가, roster 2명/서로 다른 PlayerRef, 퇴장·재참가 3회와 역할 교환, 양쪽 실제 로그·화면 증거.
- 전체 합격: 피해·권한·재장전·사망/리스폰·점수/승패 일치→로비→새 경기, Android 두 기기 및 Play 설치본 확인. 추가 peer 필요 항목은 별도 배정합니다.
- 직접 작성 범위: 이 문서와 신규 `Assets/Scripts/Editor/MultiplayerValidationBuild.cs`. 기존 BuildScript/설정/씬/패키지/Login은 해당 담당이 관리합니다.

아래 조사는 첫 작성 시점의 snapshot입니다. 다른 담당 수정이 들어오면 후속 실행 인계 시 현재값으로 다시 확인합니다.

2026-10-07(KST) 작성. 실제 저장소는 `D:\meee\git\sudden-force-fps`입니다. `PROJECT_PLAN.md`, `ANDROID_RELEASE_CHECKLIST.md` 및 현재 소스를 읽어 작성했습니다. 조사 시 HEAD는 `a2ad245fe5a4ef5f52bbf585070a67d452bcf65d`이며 작업 트리에 사용자 업그레이드와 여러 담당 변경이 있습니다. HEAD만으로 테스트 빌드를 식별하지 않습니다.

아래 초기 조사는 실행 전 snapshot이며 위 실제 실행 결과가 현재 상태입니다. 기존 단일 Lobby smoke는 다른 담당의 보조 근거로만 유지합니다. Photon Dashboard·계정 설정은 조회하거나 변경하지 않았습니다.

## 1. 읽기 조사 결과와 실행 수단

| 근거 | 현재 확인 내용 | 검증에 주는 영향 |
| --- | --- | --- |
| `Assets/Photon/Fusion/Resources/NetworkProjectConfig.fusion` | PeerMode 저장값 0, LagCompensation.Enabled=false, NetworkConditions.Enabled=false, ConnectionTimeout=10초 | Multi-Peer 활성 여부는 후속 Editor 배정 시 enum 이름으로 다시 확인합니다. 지연 보정 테스트는 해당 기능 구현·활성화 이후 진행합니다. |
| Fusion SDK `FusionBootstrap.cs`, `NetworkSceneManagerDefault.cs`, Runtime XML | Multiple 모드 다중 Runner와 개별 PhysicsScene/씬 지원 코드가 존재합니다. SDK XML은 Single을 프로세스별 한 peer로 설명합니다. | SDK 지원과 제품의 다중 Runner 호환은 별도입니다. 설정 숫자만 보고 제품 호환으로 판정하지 않습니다. |
| `Assets/Scenes`, `Assets/Prefabs`의 Bootstrap script GUID 검색 | SDK FusionBootstrap의 직렬화 참조를 찾지 못했습니다. | SDK 자동 다중 클라이언트 실행 흐름을 제품 씬이 사용한다고 가정하지 않습니다. |
| `NetworkRunnerManager`, `AD.Managers`, `RoomManager`, `SpawnPoints`, `UIManager` | static Instance가 존재하며 spawn·입력·UI가 전역 접근을 사용합니다. 일반 Unity SceneManager 호출도 존재합니다. | 동일 프로세스의 다중 Runner는 입력·팀 스폰·씬·UI를 잘못 공유할 위험이 있습니다. 현재 주 검증 수단으로 부적합하다는 소스 기반 판단이며 실제 재현 결과는 아닙니다. |
| `Packages/manifest.json` | Multiplayer Center는 있지만 `com.unity.multiplayer.playmode` 직접 의존성은 없습니다. | Unity Multiplayer Play Mode의 가상 Player를 지금 사용할 수 있다고 가정하지 않습니다. 설치하지 않았습니다. |
| `Assets/Scripts/UI/Login.cs` | Editor는 Lobby 진입, Android는 Google 로그인 성공 시 진입. Windows Standalone 분기가 없습니다. | 현재 Login 시작 PC 빌드는 로비로 진행하지 못할 것으로 예상됩니다. 네트워크/안정화 담당이 개발용 진입 경로를 구현·확인해야 합니다. Android 인증을 생략하는 배포본 변경은 요구하지 않습니다. |
| `ProjectSettings/ProjectSettings.asset` | runInBackground=1, forceSingleInstance=0 | 두 PC 프로세스 실행을 막는 저장 설정은 없습니다. 실제 비활성 창 tick·입력 해제는 후속 실행에서 확인합니다. |
| `ProjectSettings/EditorBuildSettings.asset` | Login → Lobby → Room → Game/DesertHouse, 모두 enabled | 각 클라이언트가 동일 목록·참조로 빌드돼야 합니다. 하드코딩 Room index 2 및 경기 씬 계산은 담당 수정 후 재확인합니다. |
| `PhotonAppSettings.asset` | AppVersion, FixedRegion이 빈 값입니다. | 이전 kr 연결은 런타임 관측이며 지역 고정의 증거는 아닙니다. 두 클라이언트의 실제 적용 AppVersion과 Region 일치가 필요합니다. |
| `NetworkRunnerManager.cs` | ClientServer 로비, CreateRoom=Host, JoinRoom=Client, NetworkSceneManagerDefault 전달 | Shared/Single 테스트로 대체하지 않습니다. 각 프로세스의 Host/Client 역할을 증거에 남깁니다. |
| `CanvasLobby.cs`, `NetworkRunnerManager.cs` | UI는 IsPrivateRoom 키에 `!toggle.isOn` 전달, manager는 이를 IsVisible로 사용합니다. 시작 시 IsVisible=false이며 현재 StartGame에 IsOpen=false 설정은 보이지 않습니다. | 키 이름만으로 공개/비공개 반전 버그로 단정하지 않습니다. 실제 공개 UI→IsVisible=true를 확인합니다. 목록 숨김과 경기 중 신규 참가 차단은 별도 검증합니다. |
| 저장소 추적 제외 포함 파일 검색 | 저장소 안 exe/APK/AAB를 찾지 못했습니다. | 외부 빌드가 없다는 뜻은 아닙니다. 총괄/빌드 담당으로부터 실행 파일 경로·빌드 결과를 받아야 합니다. |

[Photon Multi-Peer 공식 문서](https://doc.photonengine.com/fusion/v2/manual/testing-and-tooling/multipeer)는 Runner별 독립 simulation/physics/scene과 static·singleton 충돌 주의를 설명합니다. [씬 로딩 문서](https://doc.photonengine.com/fusion/v2/manual/scene-loading)는 기본 SceneManager의 Multi-Peer 지원을 설명합니다. 이를 근거로 SDK 가능성과 프로젝트 적합성을 구분했습니다.

| 방식 | 적합성·선행 조건 | 담당 배정 이후 용도 |
| --- | --- | --- |
| Editor + Windows Development build | 우선 권장. 단일 Editor 소유권, PC Login 진입 수정, PC 입력 경로·모듈·빌드 성공 확인이 필요합니다. | 초기 공개 방→대기실→경기 검증. Editor와 build의 다른 코드 시점 문제를 기록합니다. |
| 동일 Windows build의 두 별도 프로세스 | 동일 산출물로 버전 차이를 줄일 수 있습니다. PC Login·입력 지원이 필요합니다. 두 PID·서로 다른 로그 파일 필수입니다. | Editor 점유 없이 회귀 검증. 한 PC의 창 전환은 동시 조작 검증을 제한하므로 각자 정지 표적 시험 후 두 기기/두 운영자로 보완합니다. |
| PC/Editor + Android | APK·PGS 인증·지정 실기기·각 입력 지원이 필요합니다. | 플랫폼 간 동기화 및 모바일 조작. Host/Client 역할을 바꿔 재검증합니다. |
| Android 두 실기기 | 지정 기기·테스터 인증·APK 또는 Play 설치본이 필요합니다. | 최종 필수. 실제 두 사용자가 한 판 완료·로비 복귀·재참가, Play 설치본 검증까지 수행합니다. |
| 동일 프로세스 Multi-Peer | 현재 제약으로 바로 실행할 수 없습니다. 총괄이 필요성을 결정하고 담당이 Runner별 관리자·씬·입력·visibility·AudioListener/EventSystem을 분리한 후 배정합니다. | 보조 진단만 가능하며 두 실제 프로세스/실기기 합격을 대체하지 않습니다. 계획 문서의 Multi-Runner 선행 순서는 이 제약을 반영해 총괄이 확정합니다. |

두 번째 Editor나 다른 Unity 프로젝트를 열어 해결하지 않습니다. PC 빌드 담당이 없다면 가장 빠른 대체 경로는 안정화 담당의 APK + 현재 Editor이며, APK/인증이 준비되기 전에는 실행 선행 조건 미충족으로 기록합니다.

## 2. 후속 실행 전 인계 조건

총괄이 실행 수단, 단일 Editor 소유권, 테스트 시간대, 테스트 방 생성과 종료를 배정한 이후에만 실행합니다. 현재 파일 외 수정은 소유 담당에게 요청합니다.

1. 코드/씬/프리팹 통합 완료 시점을 정하고 그 시점의 commit + 작업 트리 diff 식별값 또는 보존 snapshot, Unity/Fusion 버전, 산출물 SHA-256, 플랫폼/Development 여부를 받습니다. 실행 도중 소스가 바뀌면 기존 빌드와 섞지 않습니다.
2. PC 방식이면 Login→Lobby와 두 클라이언트 입력 가능 여부, Windows 빌드 모듈·실제 build 성공을 먼저 확인합니다. 현재 Android 전용 BuildScript를 Windows 빌드 명령으로 사용하지 않습니다.
3. 같은 기존 Fusion App ID를 사용합니다. 기록에는 App ID 전체 대신 일치 판정/지문을 사용합니다. 앱·지역 allowlist·계정·요금제를 바꾸지 않습니다.
4. 두 peer의 실제 Region=kr, 실제 Photon AppVersion 동일, SessionLobby=ClientServer, custom lobby 사용 여부 동일을 기록합니다. FixedRegion/AppVersion이 빈 현재 구성에서는 자동 선택 결과를 관측해야 하며, 불일치 시 네트워크 담당의 기존 프로젝트 내 실행 설정을 먼저 받습니다.
5. 두 클라이언트 A/B의 구분 가능한 닉네임, PID 또는 기기 모델·OS, 서로 다른 로그 경로, 테스트 운영자·입력 방법을 지정합니다. 개인 계정 정보와 토큰은 남기지 않습니다.
6. 실행 시 적용 무기/경기 설정을 읽어 HP·부위 피해·탄창·발사 간격·재장전 시간·리스폰 시간·점수 제한·시간 제한을 확정합니다. 계획 초안 값으로 실제 동작을 합격 처리하지 않습니다.
7. 필요한 관측값이 로그/UI/Inspector에 없으면 담당에게 최소 진단 추가를 요청합니다. 실제 Runner state/authority/HP/tick/score를 확인할 수 없는 항목은 관측 부족으로 남깁니다.

[Photon 지역 문서](https://doc.photonengine.com/fusion/v2/manual/connection-and-matchmaking/regions)에 따르면 지역은 서로 분리되고 지역 미지정 시 Best Region을 선택합니다. [Matchmaking 문서](https://doc.photonengine.com/fusion/v2/manual/connection-and-matchmaking/matchmaking)는 Host/Server/Client의 ClientServer 로비와 IsVisible/IsOpen의 별도 역할을 설명합니다.

## 3. 공개 방 두 클라이언트 기본 절차

아래 절차는 후속 배정에서 실행합니다. 테스트 방 이름 예시는 `SFMP-20261007-01`이며 실행마다 충돌하지 않는 순번을 사용합니다. 타인의 방에는 참가하지 않습니다.

1. A/B를 Login에서 시작해 Lobby까지 진행합니다. 화면과 로그로 각각 연결 중→성공 및 Region/AppVersion/ClientServer를 확인합니다. 인터넷 reachability 값만으로 클라우드 성공이라 하지 않습니다.
2. A가 공개 방, DesertHouse, 최대 2명을 선택해 한 번 생성합니다. 실제 IsVisible=true/IsOpen=true, StartGameResult.Ok, Host/IsServer와 정확한 SessionName을 확인합니다. 요청 시각을 남깁니다.
3. B는 실제 OnSessionListUpdated에서 이름·MapName·인원·MaxPlayers를 확인하고 UI 목록의 해당 방을 눌러 참가합니다. 직접 SessionName 주입은 공개 목록 검증을 대체하지 않습니다. 생성 성공 후 30초 안에 목록이 안 나오면 이번 정상 경로는 실패/진단 대상으로 기록합니다. 30초는 시험 관찰 기준이며 Photon 보장 시간이 아닙니다.
4. B의 StartGameResult.Ok와 Client/LocalPlayer를 확인합니다. A/B 동일 SessionName, Room 씬, 서로 다른 PlayerRef, roster 2명 및 각 플레이어 Room 객체 1개를 양쪽에서 확인합니다.
5. B가 정상 퇴장합니다. A roster가 1명으로 줄고 유령 객체/준비 상태가 남지 않음을 확인합니다. B의 로비 복귀, 새 Runner 필요 여부와 중복 callback 여부를 기록합니다.
6. B가 목록으로 재참가해 2명으로 복원합니다. 반대 팀 선택 및 준비를 하고 Host만 경기 시작을 수행합니다. 시작 전에는 한 명/미준비 상태에서 시작 불가를 별도 확인합니다.
7. 두 클라이언트가 DesertHouse에 진입하고 각 player 1개, 해당 InputAuthority, Host StateAuthority, 자기 카메라·HUD·AudioListener만 활성화되어 있는지 확인합니다.
8. 아래 전투/경기 케이스를 수행하고 양쪽 결과 화면·로비 복귀를 확인합니다. 새 이름의 방에서 재참가합니다.
9. A/B의 Host/Client 역할을 바꿔 기본 절차를 반복합니다. 정상 경로는 최초 Host 역할별 1회 이상, 퇴장/재참가는 총 3회 이상 확인합니다.

## 4. 흐름별 합격 기준과 증거

### C1 Hitbox 기반 query 검증 준비 (NOT RUN)

Refs #33, #19, #35. C1은 프리팹 등록·부위 식별·지연 보정 query의 기반만 검증합니다. C2 실제 사격/피해가 연결되기 전에는 HP 감소·헤드샷 피해·발사 권한·탄약 검증 완료를 주장하지 않습니다. 아래 준비에서는 제품 파일/씬/Editor/컴파일을 조작하지 않았습니다. C1 담당의 코드·프리팹·설정 통합 및 제어권 반환 후 새 snapshot과 신규 Player 빌드로 실행합니다.

현재 읽기 snapshot의 NetworkProjectConfig는 LagCompensation.Enabled=false입니다. 담당이 history/query를 활성화하고 해당 플랫폼에서 실제 Runner가 history를 생성하는 것을 확인하기 전에는 animated history 시험을 BLOCKED/NOT RUN으로 유지합니다. 준비 중 파일은 담당 작업에 따라 바뀔 수 있으므로 실행 직전 적용값을 다시 읽습니다.

| ID | 확인 절차 | 합격 기준 / 증거 |
| --- | --- | --- |
| H01 프리팹 등록/bake | Red/Blue의 실제 prefab GUID·Fusion prefab table ID·NetworkObject baked NetworkedBehaviours·HitboxRoot와 child Hitbox 등록 배열을 읽고 실제 spawn과 대응 | 각각 root1/부위12, null·중복 참조·등록 누락 없음. Hitbox.Root/HitboxIndex가 배열 항목과 일치하고 해당 player NetworkObject/PlayerRef로 연결. import/compile 또는 prefab table 숫자만으로 실제 등록 성공을 대신하지 않음. |
| H02 Owner/분류 | 12개 각각 hierarchy/bone path·owner NetworkId/InputAuthority·부위 enum·좌우/상하 segment·shape/offset/size/layer를 inventory로 기록. 담당의 확정된 12개 구성 계약과 비교 | Head/Torso/Arm/Leg 및 좌우 구분이 명세와 일치, 원격 hitbox가 로컬 owner를 참조하지 않음. 12개 수만 맞고 모두 Torso로 매핑되는 경우 FAIL. 실제 12개 구성의 세부 부위 수는 계약 확인 전 임의 지정하지 않음. |
| H03 broad bounds | idle·이동·회전 및 실제 연결된 애니메이션 pose에서 각 hitbox의 world geometry 최대 범위와 root broad sphere 중심/반경을 비교. 끝부분에 향한 query도 수행 | 모든 활성 부위가 broad sphere 안에 포함되어 narrow phase 후보에서 누락되지 않음. 한 idle pose의 자동 반경 계산만으로 동적 pose 전체를 PASS 처리하지 않음. 미연결 애니메이션은 NOT RUN. |
| H04 부위 query | Host에서 표적과 ray pose/tick을 관측해 Head/Torso/좌우 Arm/Leg의 노출된 면을 각각 query. 각 결과의 HitboxIndex/Owner/부위/거리/point/normal을 기록. 두 팀 역할 교환 | 가까운 실제 부위가 올바른 분류로 반환되고 무관한 owner/부위가 선택되지 않음. 겹치는 팔·몸통/경계 ray는 nearest hit 또는 확정된 우선순위로 처리하며 배열 순서만으로 임의 선택하지 않음. ray는 실제 query API 경로를 사용하되 진단 query를 실제 발사로 표현하지 않음. |
| H05 애니메이션 history | Runner tick/history 버퍼 범위를 기록한 뒤 움직이거나 애니메이션 중인 표적의 과거 pose와 현재 pose가 다른 ray를 query. shooter PlayerRef·선택된 tick/alpha와 현재 geometry 대조 query를 함께 기록 | rewind 대상 부위가 해당 과거 pose와 맞고 현재 Physics hit만 반환한 것을 history 성공으로 간주하지 않음. Subtick 옵션 유무도 구분. 버퍼 밖·생성 전·despawn 뒤 요청의 실패/제한 정책 확인. 지연·애니메이션 연결이 없으면 NOT RUN. |
| H06 CC/자기 hitbox 제외 | query mask/layer와 IncludePhysX/IgnoreInputAuthority 실제 옵션을 기록. 표적 Head/Arm으로 향하는 ray, shooter 자기 capsule 통과 ray, CC만 포함하는 대조 query | 캐릭터 이동용 CC가 먼저 맞아 부위가 가려지거나 Torso로 대체되지 않음. shooter 자기 hitbox 제외는 실제 shooter PlayerRef 기준이며 원격 적군까지 제외하지 않음. IncludePhysX는 정적 월드 포함 범위와 분리해 평가. 일반 Physics.Raycast 성공으로 Fusion Hitbox 경로를 대신하지 않음. |
| H07 월드 벽 차폐 | 같은 표적 부위에 대해 노출/벽 뒤, 표적 앞/뒤 벽, 벽 모서리 ray를 query하고 world collider·hitbox의 최단 거리를 비교 | 표적 앞 벽이 더 가까우면 벽 차폐 결과, 표적 뒤 벽은 표적 query를 가리지 않음. player 부위 layer 제외/월드 mask 누락을 따로 확인. 정적 PhysX world의 현재 pose와 rewind hitbox의 과거 pose 사용을 명시하며 움직이는 문/엄폐물은 별도 history 정책 시험으로 남김. |
| H08 lifecycle/회귀 | actual2peer 정상 생성/Ready/Start→각12부위/root1 등록, Client/Host leave 및 새 Runner 경기 재참가에서 등록 수/owner/history 참조를 확인 | 현재 ActivePlayer별 등록 일치, 유령 root·despawn hitbox·중복 history 없음. 이동·로컬 카메라/Listener·Blue seed의 이전 gate 유지. query 수행 전후 HP/Ammo가 변하지 않으면 기반 query 상태로 기록하며 피해 완료라고 표현하지 않음. |

증거 최소 필드: build manifest/HEAD/플랫폼·config, query 종류와 호출 권한, shooter/target PlayerRef/NetworkId, input/authority tick 및 history tick/alpha(관측 가능한 경우), origin/direction/range/mask/options, 결과 종류(Hitbox/월드/없음)·HitboxIndex/Owner/부위/거리/point/normal, root broad bounds, 실제 pose·활성 부위 수. 부위12 inventory와 query 표본을 분리 보존합니다. 관측값이 노출되지 않으면 단일 작성 담당에게 최소 진단을 요청하며 QA는 제품을 임의 패치하지 않습니다.

SDK 근거는 설치된 Fusion.Runtime.xml의 Hitbox.Root/HitboxIndex, HitboxRoot.Hitboxes/BroadRadius/Offset 및 HitOptions.IncludePhysX/SubtickAccuracy/IgnoreInputAuthority입니다. BroadRadius 자동 계산은 현재 pose의 대략적인 범위이므로 animated bounds 전체 검증과 구분합니다.

이동 잔여 항목(Android touch/실기기, 계단·경사, held-focuslost, 같은 팀 occupied spawn 회피)은 기존 NOT RUN 상태로 계속 추적하며 C1 합격으로 대체하지 않습니다.

아래 표는 전체 시험의 합격 기준입니다. 이번에 실행한 범위의 실제 결과는 문서 첫 부분에 기록했습니다. 그 밖의 항목은 NOT RUN이며 코드 읽기·컴파일·단일 클라이언트 smoke를 PASS로 쓰지 않습니다.

| ID | 실행 케이스 | 합격 기준 | 필수 증거 |
| --- | --- | --- | --- |
| L01 | A/B 로비 접속, 빈 목록, 재시도 | 연결 성공/실패/방 없음 구분, 로딩 종료, 중복 요청·Runner 없음 | 양쪽 LobbyStatus·LastLobbyResult/Reason·SessionList callback·화면 |
| L02 | 공개 생성→목록→참가 | 동일 앱/kr/AppVersion/ClientServer와 실제 방 1개, B 목록 노출·참가 성공 | Host/Client StartGameResult, 목록 snapshot, SessionInfo |
| L03 | 퇴장→재참가 3회 | roster/객체 정확히 증감, 유령 플레이어·중복 이벤트·멈춘 로딩 없음 | PlayerJoined/Left/Shutdown 순서, Runner 수/PlayerRef 전후 |
| R01 | 1명·미준비·팀 불균형·Client 시작 시도 | 확정 규칙 위반 시작 차단, Host만 정상 시작, 준비/팀이 양쪽 일치 | 양쪽 roster/team/ready, 시작 요청과 호스트 판정 |
| R02 | 동시 경기 씬 진입 | 동일 맵·경기 instance, player 각 1개, 자기 입력·카메라, 올바른 팀 스폰 | NetworkId/PlayerRef/authority, 양쪽 씬·화면 |
| C01 | A 이동/회전/달리기/점프 후 B 반복 | 원격 이동·시점 반영, 자기 입력이 원격 player에 적용되지 않음, 엄폐/지면 통과 없음 | 양쪽 영상, 위치/회전 tick 표본, 입력 주체 |
| C02 | 정지 표적 몸통·머리·왼/오른팔·왼/오른다리 단발, 역할 교환 | 실제 적용 피해와 Host HP 감소가 정확히 일치, 양쪽 최종 HP 일치, 머리만 헤드샷 피드백 | shooter/target/shot 또는 tick/부위/전후 HP·피해량, HUD |
| C03 | 연속 발사·트리거 반복·빈 탄창 | Host가 발사 간격/탄약을 검증, 탄약 음수·허용량 초과 피해 없음, 반동이 확정 설정대로 발생·회복 | Host 승인 shot tick/탄약, 시점 영상, 설정 |
| C04 | 일부 탄약·빈 탄창 재장전 및 도중 발사 | 재장전 시간·탄약 확정 규칙 일치, 중복 재장전/도중 발사·피해 없음 | 양쪽 ammo/reload 상태 및 Host shot 승인 기록 |
| C05 | 벽 뒤 표적·벽 앞 표적 | 벽에 가린 표적 HP 변화 없음, 노출 표적만 유효 피해 | 표적 배치 양쪽 화면, ray/hit 판정·HP |
| C06 | 치명타·연속 중복 명중·사망 중 발사·리스폰 | 사망/점수는 1회, 사망 중 입력·발사 차단, 정해진 지연 후 팀 스폰, HP/탄약/상태 초기화 | Host death/kill/respawn tick·NetworkId, 양쪽 score·화면 |
| C07 | 아군 피해 | 팀킬 꺼짐일 때 아군 HP/점수 불변 | team·Host hit/HP/score. 정상 1대1 적군 테스트로 대체 불가 |
| M01 | 점수 제한 도달 | 확정 목표점수에서 한 번 종료, 양쪽 승패·최종 점수 동일, 종료 후 피해/점수 증가 없음 | Host end reason/tick, A/B 최종 scoreboard·result |
| M02 | 시간 종료·동점 | 실제 타이머 만료, 점수 우세 승패와 동점 각각 정확, 양쪽 최종 결과 동일 | 시작/종료 tick·시간·점수·result, 전체 경과 기록 |
| M03 | 결과→로비→새 방→새 경기 | 잔여 player/카메라/점수/타이머·Runner 없음, 초기 상태로 다시 플레이 가능 | 양쪽 씬·Runner/roster 전후, 새 SessionName·경기 ID |
| F01 | 방 생성/참가 버튼 연속 클릭, 방 종료와 참가 경쟁, 만원 | 요청 수렴 또는 명시 실패, 중복 방/객체·무한 로딩 없음 | 요청 순서/결과·ShutdownReason·UI |
| F02 | 경기 중 신규 참가 시도 | 목록 숨김과 별개로 실제 참가 거절, 기존 경기 상태 보존 | IsOpen/IsVisible, 추가 peer Join 결과·기존 roster |
| F03 | Client 정상 종료/프로세스 종료, Host 정상 이탈/종료 | 확정 정책대로 roster 정리, Host 이탈 시 종료 사유·로비 복귀, 호스트 이전을 기대하지 않음 | 살아있는 peer 로그·화면·ShutdownReason·복귀 시간 |
| F04 | 테스트 클라이언트만 연결 끊김·복구, Android 일시 정지/복귀 | 무한 로딩·입력 고착 없음, 실패 사유·재시도 또는 종료 정책 일치, 새 방 참가 가능 | 끊김/복귀 시각·tick·callback·입력 상태 |
| A01 | Android 두 기기 한 판·재참가·10분 | 실제 터치 이동+시점+발사/재장전 가능, 동기화·결과 일치, 크래시/ANR 없음 | 기기/OS/설치본 hash·양쪽 logcat·영상·성능 측정 |
| A02 | Play 테스트 설치/업데이트 두 기기 | Play 설치본 PGS 로그인, 두 명 경기·로비·재참가 및 지정 다음 versionCode 업데이트 성공 | 트랙/버전·설치 경로·로그인 결과·경기 증거 |

C02는 각 부위 최소 3회·Host/Client 공격 역할 양쪽 수행하며 비치명타 시험 전 표적 HP를 정상 리스폰으로 초기화합니다. HP 100/머리100/몸통25/팔다리18은 현재 계획의 초안입니다. 실제 설정이 같다면 단발 HP 0/75/82, 몸통 4발·팔다리 6발 치명타를 확인하며, 수치가 바뀌면 기대값을 실제 설정으로 계산합니다.

C07은 정상 팀전 최소 인원을 유지하는 4클라이언트 시험을 추가 배정하는 것이 권장됩니다. 제품 UI가 2명 같은 팀에서 경기 시작을 허용하지 않으면 임의 코드/팀 주입으로 우회하지 않고 추가 인원 필요로 기록합니다. F01 만원/F02 신규 참가도 세 번째 독립 peer가 필요합니다. 2 peer만 있으면 해당 케이스를 합격 처리하지 않습니다.

M02는 우세 점수로 시간 만료, 0:0 동점 만료의 별도 경기를 수행합니다. 실제 5분 설정이면 실제 5분 경과를 검증합니다. 짧게 바꾼 개발 설정 시험은 보조 결과이며 최종 규칙 한 판을 대체하지 않습니다. 타이머 비교는 같은 Host tick/end tick 기준으로 수행하고 비동기 화면 캡처의 숫자 차이를 곧바로 오류로 단정하지 않습니다.

지연·손실 시험은 기본 경로 통과 후 담당의 기존 프로젝트 내 지원 수단을 배정받아 진행합니다. 적용 방향, delay/jitter/loss, 실제 RTT와 tick을 기록하고 정지/이동 표적·가림·죽음 중복·결과 동기화를 반복합니다. 처음에는 정상 환경, 이후 예시 RTT 약 100/200ms·loss 1/3%를 별도 케이스로 확정합니다. 지연 보정 허용 범위가 정해지지 않거나 현재 비활성이면 Lag Compensation 합격을 선언하지 않습니다. 시스템 전역 네트워크/방화벽 변경이나 다른 앱 연결 차단은 사용하지 않습니다.

## 5. 증거 기록과 실패 분리

실행 담당이 프로젝트 밖 전용 evidence 디렉터리를 배정받아 A/B 원본 로그·화면/영상·build report·산출물 hash를 보존합니다. 이번 조사에서는 evidence 디렉터리를 생성하지 않았습니다. PC의 실제 실행 명령/PID/로그 경로, Editor Console 및 해당 구간 Editor.log, Android의 해당 앱 logcat을 각각 기록합니다. 두 peer 로그를 하나로 섞지 않습니다.

각 케이스 기록 형식:

- 실행 ID/케이스 ID/실행 시각(KST)/운영자, A/B/C 환경·PID 또는 기기, Host 역할.
- 소스 snapshot/작업 트리 식별값, build hash/Unity/Fusion/앱 버전, 실제 앱 지문·Region·Photon AppVersion·Lobby·SessionName.
- 사전 상태·조작 순서·기대값·실제값, Host tick/NetworkId/PlayerRef/권한·양쪽 관측값.
- PASS/FAIL/BLOCKED/NOT RUN, 증거 경로·영상 시각, 재현 횟수, 담당에게 필요한 수정 범위.

| 실패 구분 | 분리할 증거와 다음 확인 |
| --- | --- |
| 실행 환경/미구현 | PC Login 정지, 빌드 없음, 미구현 HUD/전투, 필요한 기기 없음은 BLOCKED로 구분합니다. 네트워크 실패로 기록하지 않습니다. |
| Google 로그인 | PGS 인증 결과·로그인 화면·설치본 종류를 먼저 확인합니다. Lobby 도달 전 실패를 Photon 실패로 오인하지 않습니다. |
| Photon 접속/매칭 | JoinSessionLobby/StartGame 결과·ShutdownReason, 실제 AppVersion/Region/Lobby, 방 visibility/open/capacity를 비교합니다. 계정 설정을 바꾸지 않습니다. |
| 목록 UI | 실제 SessionList callback에는 방이 있는데 UI만 없으면 UI 문제, callback도 없으면 매칭/연결/필터 문제부터 확인합니다. |
| Runner 수명 | 첫 참가 성공 후 두 번째만 실패하면 Shutdown 완료·Runner 재생성·callback 중복·로비 재접속을 확인합니다. 종료된 Runner의 재사용을 정상으로 가정하지 않습니다. |
| 씬/스폰 | 세션 참가 성공과 map 진입 실패를 분리합니다. build scene 목록·SceneRef·LoadDone·prefab NetworkId·roster·authority를 확인합니다. |
| 입력/포커스 | Host tick이 계속 증가하는데 창 전환 후 조작이 안 되면 local focus/input을 확인합니다. 원격 입력과 연결 실패를 구분합니다. |
| 피해/동기화 | Host 판정 자체 오류와 Host는 맞지만 Client state/HUD가 다른 경우를 분리합니다. shot/tick/부위/authority/HP 근거를 함께 제출합니다. |
| 경기/UI | Host score/end state 오류, replication 오류, 최종 state는 맞지만 결과 표시 오류를 각각 구분합니다. |
| 근거 부족 | 권한·피해·부위·종료 중복을 관측할 방법이 없으면 증거 부족입니다. 화면상 그럴듯함으로 PASS를 작성하지 않습니다. |

정상 경로의 새 Error/Exception/Assert는 원인 확인 전 통과하지 않습니다. 의도한 실패 케이스의 예상 종료 로그는 정상 케이스와 분리합니다. 재실행 전에 실패 로그를 보존하고, 수정 담당이 인계한 새 snapshot/build로 실패 케이스와 영향받는 기본 흐름만 다시 검증합니다.

## 6. 현재 상태와 다음 배정 제안

- [x] 기존 계획/Android checklist 및 제품·SDK 실행 경로 읽기 조사
- [x] 이 문서 작성
- [ ] 총괄의 실행 수단·Editor 제어권·방 생성/프로세스 실행 배정
- [ ] PC 로그인/입력 개발 경로와 첫 Windows build 또는 검증 가능한 APK 인계
- [ ] 두 peer의 실제 kr/AppVersion/ClientServer 일치 확인
- [ ] L01~L03 실제 두 클라이언트 공개 방 검증
- [ ] R/C/M/F 흐름 및 추가 peer가 필요한 케이스 검증
- [ ] Android 두 실기기 및 Play 설치본 최종 검증

### 추가 확인 기록 — 단일 호스트 방 수명주기

2026-10-07 12:22–12:23 KST, 네트워크 담당이 실제 Editor에서 고유 공개 진단 방 2개를 순차 생성해 Room 진입→퇴장→새 Runner/manager 로비 복귀를 확인했습니다. QA 담당은 전달받은 원본 `C:\Users\pc_17\Documents\ChatGPT\sudden-force-fps\network-room-retry-validation-20261007.jsonl`의 Room1/LobbyAfterRoom1/Room2/LobbyAfterRoom2 기록을 읽어 확인했습니다.

- 첫 방: 실제 Host/Room, IsVisible=true/IsOpen=true, MaxPlayers=2, team capacity=1, DesertHouse, RoomPlayers=1. 유효 팀 변경·팀7 거절·원래 팀 복귀·단독 호스트 시작 거절을 확인했습니다.
- 두 번째 방: StartGame 결과 Ok, LastRoomError 빈 값, 실제 local room NetworkObject와 GetPlayerObject(LocalPlayer) 일치, 단독 호스트 준비/시작 불가를 확인했습니다.
- 각 퇴장 후 manager -47036→-48118→-49190으로 새 instance, Lobby Connected/list received, manager 1개·roster 0·Room unload·닉네임 유지. 최종 목록에서 두 진단 방이 없음을 확인했습니다.
- 중복 create/return pending Task 동일, 새 runtime errors=[] 및 최종 Playing=false/Untitled dirty=false/autotick=false 복원은 네트워크 담당의 추가 보고입니다. QA는 Editor를 직접 조작하지 않았습니다.

**한 peer의 방 생성/복귀만 확인한 보조 결과입니다. L02의 두 클라이언트 공개 목록 참가, L03의 두 peer 재참가, 양팀 준비/경기 진입 및 전투 합격을 의미하지 않습니다.** 실제 Windows 빌드와 두 프로세스 시험은 총괄의 다음 배정을 기다립니다.

권장 첫 배정은 네트워크 담당의 로비/Runner 수명 수정 완료 후 **Editor + Windows build의 L01~L03**입니다. PC 준비가 Android보다 늦으면 **Editor + APK**로 대체합니다. 현재 SDK Multi-Peer 가능성을 확인한 사실은 실제 두 클라이언트 완료 증거가 아니며, 실행 전 선행 조건과 미실행 항목을 그대로 유지합니다.

## 부록: Windows64 개발 빌드와 두 프로세스 실행

### 빌드 함수 계약

신규 `Assets/Scripts/Editor/MultiplayerValidationBuild.cs`는 자동 실행 callback/MenuItem이 없는 명시적 함수입니다. 기존 BuildScript를 호출하지 않습니다. 자동 target 전환, PlayerSettings/define/버전/서명/Fusion setter, 씬 저장, Player 실행을 포함하지 않습니다.

- active target=StandaloneWindows64, PlayMode=false, import/compile/build 완료, Windows64 support 존재를 확인합니다. target 전환은 배정받은 Editor 담당이 먼저 수행합니다.
- `EditorBuildSettings.scenes`의 enabled 씬을 순서대로 사용하고 빈 목록/누락 파일을 거절합니다. Development + CompressWithLz4 + StrictMode이며 AutoRunPlayer/ConnectWithProfiler/AllowDebugging은 추가하지 않습니다.
- 허용 root와 새 output은 절대 로컬 Windows 경로입니다. root는 프로젝트 내부/상위/드라이브 루트를 거절하며 output은 root 자식만 허용합니다. 기존 output·junction/symlink·alternate stream은 거절합니다. 덮어쓰기/삭제는 하지 않습니다.
- 성공 조건: BuildResult.Succeeded + totalErrors=0 + exe 존재 + 전체 Player 파일 해시와 `validation-build.json` 기록 완료. manifest에는 UTC 시간, Unity/Player version, 씬 목록, 결과/에러·경고 수, 상대경로/SHA-256을 기록합니다. 소스 snapshot/diff 식별값은 증거에 따로 연결합니다.
- build/hash 실패 시 success=false와 안전한 예외 타입을 기록합니다. preflight 또는 디스크 쓰기 실패 시 manifest가 없을 수 있습니다. 강제 종료 후 Running 상태/manifest 없음은 성공이 아닙니다.
- CLI 함수는 batch만 허용하고 `-sfValidationRoot`, `-sfValidationOutput`, `-logFile`을 각각 한 번 요구합니다. log도 root 아래/output 밖이어야 합니다. 성공 Exit(0), 실패 Exit(1)입니다. 로그 경로 검사는 함수 진입 후이므로 launcher가 시작 전에 경로를 확인해야 합니다.
- live 함수는 실패를 throw하고 Editor를 닫지 않습니다. batch 함수를 live Editor에서 호출하지 않습니다. 기존 Editor 원본 로그는 기존 위치에 유지하고 시험 구간 사본을 증거 폴더에 보존합니다.
- App ID/키/서명/Photon 설정과 임의 SDK 예외 메시지를 직접 출력하지 않습니다. Unity/SDK의 자체 원본 로그는 로컬 보존하고 외부 전달 시 App ID·토큰·계정 식별값을 검토합니다.

네트워크 담당이 대상 Unity에서 실제 refresh/recompile을 수행해 `recompile_status=completed`, `failed=false`, `errors=[]`, `compilationFailed=false`를 확인하고 전달했습니다. 초기 manifest 오류·경고 필드의 `uint` 선언으로 발생했던 CS0266 두 건은 실제 API 반환값에 맞춰 `int`로 수정했습니다. QA 담당은 Editor를 직접 조작하지 않았습니다. Windows build 함수 실행·산출물·두 클라이언트 동작은 **미검증**입니다. 빌드/import/cache/target 전환 영향을 실행 담당이 diff로 확인하고 기존 변경을 보존합니다.

### 제어권 인계 후 사용 예시 — 현재 실행하지 않음

소유 중 Editor라면 target 전환·컴파일 완료 후 Pipeline의 실제 eval 명령을 확인해 다음 C#를 한 번 호출합니다. 동기 build이므로 CLI timeout만 보고 중복 호출하지 않고 실제 build 상태부터 확인합니다.

```csharp
MultiplayerValidationBuild.BuildWindows64(
    @"D:\meee\git\sudden-force-fps-validation",
    @"D:\meee\git\sudden-force-fps-validation\pc-20261007-01");
```

전용 batch는 기존 프로젝트 Editor를 정상 종료한 뒤 사용합니다. 같은 프로젝트의 Editor와 병행하지 않습니다. Unity CLI의 현재 버전·업데이트 확인·`run --help`를 실행 배정 후 확인하며 임의 설치/업데이트는 하지 않습니다. 허용 root가 프로젝트/상위와 겹치지 않고 reparse point가 없는지 먼저 확인하고 log의 부모 폴더를 준비합니다.

```powershell
unity run 'D:\meee\git\sudden-force-fps' --editor-version 6000.3.25f1 --timeout 1800 --no-tail -- -buildTarget StandaloneWindows64 -executeMethod MultiplayerValidationBuild.BuildFromCommandLine -sfValidationRoot 'D:\meee\git\sudden-force-fps-validation' -sfValidationOutput 'D:\meee\git\sudden-force-fps-validation\pc-20261007-01' -logFile 'D:\meee\git\sudden-force-fps-validation\build-20261007-01.log'
```

`unity run`이 batchmode/quit/projectPath를 관리하므로 `--` 뒤에 중복 전달하지 않습니다. timeout은 첫 측정 후 조정합니다. `-nographics`는 그래픽 의존 빌드 영향 확인 전 사용하지 않습니다. CLI exit code와 manifest.success/result/errors 및 파일 hash를 함께 확인합니다. CLI가 해당 기능을 지원하지 않으면 설치 대신 총괄에게 정확한 가용 실행 경로를 보고합니다. Unity.exe 직접 실행을 배정받았다면 같은 executeMethod/validation 인수와 명시적인 batchmode/projectPath/buildTarget/logFile을 사용합니다.

두 게임 프로세스는 성공한 **동일 build 폴더 전체**를 사용합니다. exe만 복사하지 않습니다. 아래는 실제 실행 배정 이후의 예시입니다. 로그 부모 폴더는 새 증거 디렉터리로 미리 준비하고 같은 경로가 있으면 중단해 새 순번을 배정합니다.

```powershell
$playerDirectory = 'D:\meee\git\sudden-force-fps-validation\pc-20261007-01'
$playerExe = "$playerDirectory\SuddenForceFPS.Validation.exe"
$playerA = Start-Process -FilePath $playerExe -WorkingDirectory $playerDirectory -ArgumentList '-screen-fullscreen 0 -screen-width 960 -screen-height 540 -logFile "D:\meee\git\sudden-force-fps-validation\run-20261007-01\player-A.log"' -WindowStyle Normal -PassThru
$playerB = Start-Process -FilePath $playerExe -WorkingDirectory $playerDirectory -ArgumentList '-screen-fullscreen 0 -screen-width 960 -screen-height 540 -logFile "D:\meee\git\sudden-force-fps-validation\run-20261007-01\player-B.log"' -WindowStyle Normal -PassThru
```

게임 창은 실제 사용자가 UI를 조작하는 도구이므로 표시합니다. 각 반환 객체의 PID·실행 경로·시작 시각·로그 경로·build manifest 지문을 증거에 저장하고 창을 A/B 닉네임과 연결합니다. A는 공개 생성, B는 목록 참가를 실제 UI로 수행합니다. 위 인수에는 자동 Host/Join/nickname 기능이 없으며 있다고 가정하지 않습니다. 별도 프로세스도 같은 Windows 계정의 PlayerPrefs를 공유할 수 있으므로 제품 UI에서 닉네임을 각각 지정하고 재시작 시 확인합니다.

한 PC에서는 각 창에 순서대로 포커스·입력을 전달하며 비활성 peer의 tick이 계속 증가하는지 확인합니다. 동시에 움직이며 사격하는 시험은 두 운영자/두 기기로 보완합니다. 해당 A/B 로그의 새 줄과 각 PID 종료 여부/최종 exit code를 수집합니다. 정상 종료는 제품 퇴장/닫기 흐름을 사용하며 강제 종료 케이스는 저장한 프로세스 객체/PID·경로·시작 시각을 다시 확인해 해당 peer만 종료합니다. 이름으로 모든 Unity/게임 프로세스를 종료하거나 증거 폴더를 삭제하지 않습니다.

[Unity 6.3 BuildPipeline.BuildPlayer](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/BuildPipeline.BuildPlayer.html), [BuildOptions](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/BuildOptions.html), [Player 명령행 인수](https://docs.unity3d.com/6000.3/Documentation/Manual/PlayerCommandLineArguments.html)를 확인해 설계했습니다. 특히 먼저 build target 전환·컴파일이 완료되어 플랫폼 define이 맞도록 제한했습니다.

실행 전 ProjectSettings 백업과 종료 파일의 차이는 Unity 빌드가 생성한 Standalone batching 직렬화 항목(m_StaticBatching=1, m_DynamicBatching=0)입니다. 원 define은 복원됐으며 해당 자동 직렬화 차이는 임의 덮어쓰기하지 않고 총괄에 인계합니다.

전체 tracked diff block 비교 추가 확인: 빌드 전후 차이 파일은 ProjectSettings.asset, BurstAotSettings_StandaloneWindows.json, URP-Performant.asset, URP-Balanced.asset, URP-HighFidelity.asset, UniversalRenderPipelineGlobalSettings.asset 총 6개입니다. URP shader prefilter/일부 SSAO prefilter 값, global settings의 build list/resource 직렬화 및 Burst schema Version3→5가 Unity 빌드 과정에서 저장됐습니다. 직접 Renderer/RenderGraph 변경은 하지 않았지만 자동 asset 변경은 존재합니다. 기존 사용자 변경을 보존하며 이 자동차이들을 임의 덮어쓰기하지 않고 총괄에 인계합니다. 자세한 비교 파일: tracked-diff-block-changes.json/source-before.patch/source-after.patch.

## 이동·로컬 카메라 검증 표준 (실제 실행 범위는 문서 첫 부분 참고)

Editor 제어권을 다시 배정받은 뒤 PC↔PC를 먼저 수행하고, PC↔Android에서 양쪽 Host 역할을 교환합니다. 같은 통합 소스 snapshot의 manifest·플랫폼 define·설정 차이를 기록합니다. 이번 helper/증거 inventory는 그대로 보존하며 제품 코드·씬·컴파일 영향 파일은 이 준비 단계에서 수정하지 않습니다. 각 항목은 실제 UI/입력으로 수행하고 로그·권한·위치/tick 관측과 양쪽 화면을 함께 남깁니다. Android 설치·실행과 생명주기 조작은 담당 배정 후 진행합니다.

| ID | 절차 | 합격 기준 / 필요한 증거 |
| --- | --- | --- |
| P01 로컬 카메라 | Red/Blue를 각각 로컬 역할로 시작, 양쪽 camera root·player·InputAuthority를 연결. 원격 플레이어가 움직이고 회전할 때 로컬 화면을 관찰 | peer당 gameplay Camera 1개와 AudioListener 1개만 활성. 원격 Camera/Listener는 inactive 또는 enabled=false. HUD/시점 대상은 로컬 PlayerRef이며 원격 입력으로 자기 시점이 전환되지 않음. network authority와 활성 객체 목록·양쪽 화면 필요. 별도 UI camera는 설계가 명시된 경우 분리 기록. |
| P02 Blue 초기 yaw | Blue spawn 직후 입력 전 pose.rotation, network yaw, camera yaw/forward와 선택된 spawn Transform의 forward를 표본. Red도 동일 비교 | Blue가 지정된 spawn 방향을 보며 첫 tick/첫 Render에서 yaw가 0으로 덮이지 않음. 양쪽 복제 rotation과 카메라 방향이 일치. 180도 고정은 규칙이 확인된 경우에만 기대값으로 사용. |
| P03 포커스 상실 | PC 이동·마우스 회전·달리기/점프 입력을 유지한 채 다른 창으로 전환, 비활성 중 키 해제 후 복귀. Android joystick/look pointer를 유지한 채 홈/화면 잠금·복귀 | focus/pause 경계에서 이동·look delta·버튼·pointer 상태 reset. 복귀 후 입력 없이 이동/회전/점프 반복 없음, 새로운 실제 입력에는 정상 반응. 비활성 Runner tick 진행/연결 유지 정책과 카메라 cursor lock 복원도 기록. 연결이 종료되면 로비 회복 경로를 별도 판정. |
| P04 아날로그/예측 | joystick 0·작은·중간·최대 입력 및 대각선, PC 동일 방향 입력을 각각 일정 tick 구간으로 반복. Host/Client 역할 교환 | dead zone 밖 입력 크기에 따른 속도 변화가 명세와 일치, magnitude가 의도치 않게 1로 정규화되지 않음. 대각선 과속 없음. 자기 입력은 자기 player에만 적용. Host 권위 위치와 Client 예측 위치/보정·remote 보간을 같은 tick으로 비교. 지연/손실 시험은 소유권 배정 및 합의한 조건으로만 수행. |
| P05 CharacterController 벽 | 정면/비스듬한 벽 접근, 모서리 이동, 좁은 통로, 머리 위 낮은 천장 아래 점프를 양쪽 역할로 수행 | 벽/바닥/천장 관통과 지속 침투 없음. 벽을 따라 이동 시 불필요한 정지·튕김·누적 보정 없음. capsule radius/height/skinWidth·collision flags와 충돌면 근처 위치 표본 필요. 단순 화면으로 충돌 판정을 확정하지 않음. |
| P06 계단/경사 | 실제 CC stepOffset/slopeLimit와 geometry를 읽은 뒤 허용/초과 계단·경사, 올라가기/내려가기·정지·점프 착지를 시험 | 허용 높이/경사 통과, 초과 면 차단. grounded 상태·중력/수직 속도 전환 정상, 계단 떨림/낙하·경사에서 무한 상승 없음. 해당 지형이 맵에 없으면 NOT RUN으로 남기고 제품 씬을 QA가 임의 추가하지 않음. |
| P07 스폰 중복 | 정상 Ready/Start, 경기 진입 callback 반복 가능 경계, leave/rejoin·새 경기에서 ActivePlayers와 network player 목록 대응을 확인 | 활성 PlayerRef별 valid game NetworkObject 1개, Host spawn dictionary/roster 일치, Client에도 같은 NetworkId. RoomPlayer 잔존·유령 player·중복 spawn 없음. 재참가 PlayerRef 변경은 새 참가자로 기록. 강제 SpawnGamePlayer 호출은 사용자 시작 경로 검증을 대체하지 않음. |
| P08 스폰 겹침 회피 | spawn 후보 pose와 capsule 범위를 관측. 같은 팀 2명 이상을 실제 추가 peer로 시작하거나 승인된 다인 시험에서 반복 | 살아 있는 player/벽과 capsule이 겹치지 않는 사용 가능한 pose 선택. 점유 spawn의 재선택·안전 fallback 정책 확인. 1vs1 두 팀의 최초 스폰 성공만으로 같은 팀 점유 회피를 PASS 처리하지 않음. 추가 peer 없으면 NOT RUN. |
| P09 종료 회귀 | 이동 중 Client leave, Host leave, focus 복귀 이후 정상 로비/재참가·다음 경기 | 새 Runner, 빈 roster/input cache, 중복 callback 없음. 다음 경기 카메라/Listener도 다시 각 1개. stage1 공개방/Ready/각1spawn gate가 유지됨. |

값이 확정되지 않은 속도·회전 감도·허용 오차는 첫 통합 snapshot에서 담당 명세와 실제 적용값을 기록한 뒤 판정합니다. 정상 입력 테스트로 서버 권한·cheat 검증까지 합격했다고 주장하지 않습니다. 진단 수단이 부족하면 담당에게 최소 관측 추가를 요청하고 상태를 관측 부족으로 남깁니다.

### 빌드 자동 저장 6파일의 분류 근거

동결 이전 사용자 업그레이드 변경은 `source-before.patch`, QA 종료 시점은 `source-after.patch`에 보존했습니다. HEAD 대비 전체 diff가 아니라 두 patch의 동일 파일 block 차이만 QA 중 추가차이로 분리했습니다. 현재 Android 담당이 Editor를 소유하므로 이 판정은 저장된 종료 snapshot 기준이며 후속 변경은 포함하지 않습니다.

- URP 3개 asset 추가차이는 `m_Prefilter*`만입니다. SDK `UniversalRenderPipelineAssetPrefiltering.cs`는 SSAO 필드에 `ShaderKeywordFilter.RemoveIf`를 지정하고 `UpdateShaderKeywordPrefiltering`에서 strip 플래그로 갱신합니다. `Editor/ShaderBuildPreprocessor.cs:499` 이후는 해당 값을 갱신·SetDirty·SaveAssetIfDirty합니다. 따라서 Balanced SSAO 차이는 RendererFeature의 SSAO 품질/활성 설정 변경이 아니라 **빌드 shader variant 제거 규칙** 변경입니다. 빌드 포함 variant에는 영향을 줄 수 있으므로 기능적으로 무의미한 차이라고 단정하지 않습니다.
- GlobalSettings 차이는 `m_Settings.m_RuntimeSettings.m_List`의 채움과 Renderer2DResources의 `m_FallOffLookup` 참조 채움입니다. Core `RenderPipelineGraphicsSettingsContainer.cs`의 OnBeforeSerialize/build stripping 경로가 runtime 목록을 채웁니다. FallOffLookup은 SDK에서 URP_COMPATIBILITY_MODE 조건부 필드로 선언한 2D lighting resource입니다. 임시 빌드 define 경로에서 리소스 참조가 채워진 것으로 해석하며 단순 목록 cache와 구분합니다. 저장된 두 patch 비교에 RenderGraph/compatibility flag·RendererDataList 변경은 없습니다. 이전 업그레이드 snapshot에 있던 `m_EnableRenderGraph=0`/`m_EnableRenderCompatibilityMode=1`을 QA가 바꾼 것으로 귀속하지 않습니다.
- ProjectSettings 추가차이는 Standalone batching 항목 직렬화이며 원 define의 임시 추가는 복원됐습니다. Burst 추가차이는 schema Version3→5와 신설/폐지 옵션 직렬화입니다. Burst 컴파일 결과 영향은 별도 담당 확인 대상이며 rendering/SSAO 설정 변경과 구분합니다.

자동차이 rollback은 하지 않았습니다. 원 사용자 변경과 검증 빌드 증거를 보존하며 플랫폼별 다음 빌드에서 prefilter가 다시 계산될 수 있음을 후속 비교 기준으로 남깁니다.

단계2 종료 정리 실제확인: Player PID2176 CloseMainWindow 정상 shutdown 후 프로세스/창 소멸(첫10초 wait는 timeout이었고 이후 정상종료확인, 강제kill없음). EditorPlayfalse/compilingfalse/updatingfalse/빈단일씬 path빈값 dirtyfalse/StandaloneWindows64/autotickfalse, 원Standalonedefine live+disk동일, recompile completed failedfalse errors[]. 실제게임 최종console groundTruth errors0/warnings2, 과거Pipeline timeout이력은 분리. 이번 빌드 자동저장추가차이는 URP-Balanced의 SSAO shader prefilter4값과 GlobalSettings runtime목록 rid4338206851167682583 제외입니다. RenderGraph/compatflag 직접변경없음. 기존importtrace 일부가Editor자동재생성/refresh후clean으로보이므로 시작status/patch와 종료patch를 함께 보존하며 임의rollback없음.
