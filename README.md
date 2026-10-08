# Sudden Force FPS

> Photon Fusion 2 기반의 호스트·클라이언트 멀티플레이 Unity 3D FPS 프로젝트입니다. 로비에서 방을 만들고 팀을 선택해 DesertHouse에서 한 판을 진행합니다.

현재 상태는 `b757997` 기준입니다. Windows 두 피어 경기·종료 복귀를 검증했고 최신 Android 개발 APK 빌드와 산출물 검사를 완료했습니다. Android 실기기 설치·PGS 인증·모바일 한 판과 이번 변경의 Google Play 테스트 트랙 배포는 아직 완료하지 않았습니다.

## 주요 구현

| 기능 | 내용 및 관련 코드 |
| --- | --- |
| 멀티플레이 | Fusion의 ClientServer 로비, Host/Client 방 접속, 네트워크 입력·상태 동기화, 호스트 권한의 사격·피해 판정 · [NetworkRunnerManager.cs](Assets/Scripts/Managers/NetworkRunnerManager.cs) |
| 방 목록 UI | 항목을 재사용하는 스크롤 뷰 · [RoomManage.cs](Assets/Scripts/UI/RecyclableScrollView/RoomManage.cs) |
| 전투 | 이동·시점·달리기, 총기 반동, 부위별 피해, 탄약·재장전, 사망·리스폰 · [GamePlayerNetworkData.cs](Assets/Scripts/Creature/Player/GamePlayerNetworkData.cs) |
| 경기·HUD | 팀 점수·타이머·KD·킬 피드·스코어보드·결과 화면 · [NetworkMatchState.cs](Assets/Scripts/Game/Match/NetworkMatchState.cs) |
| 씬·복귀 | 일반 씬 전환과 Fusion 경기 씬 로드, persistent Managers가 종료된 Runner를 정리하고 새 로비 Runner를 생성 · [Managers.cs](Assets/Scripts/Managers/Managers.cs) |
| 로그인 | Android Google Play Games Services v2 인증·취소/실패/30초 시간 초과·재시도 처리 구현. 실제 계정 인증은 미검증 · [Login.cs](Assets/Scripts/UI/Login.cs) |

Fusion은 방 접속뿐 아니라 입력 전달, 권한에 따른 상태 복제, Hitbox 지연 보정 쿼리와 경기 씬 전환에 사용합니다. 지연·손실 환경의 전체 품질 검증은 별도이며, 상세 통과 범위와 미검증 항목은 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)에 구분합니다.

## 개발 목표

- [x] Photon Fusion 기반 멀티플레이
- [x] Google Play Games Services v2 로그인 경로 구현
- [ ] Android 실기기 인증·모바일 경기 검증
- [ ] 최신 버전 Google Play 테스트 배포·테스터 설치 검증
- [ ] 스킬 시스템 및 장비 시스템 구현
- [ ] 프리팹 구성과 재사용 방식 개선

시연 영상과 Google Play 공개 링크는 준비 중입니다.

스킬·장비는 기본 경기 이후의 별도 단계입니다. 광고·인앱 결제는 현재 및 향후 작업 범위에서 제외합니다. PlayFab 로그인 통합도 완료된 기능으로 표시하지 않습니다.

## 개발 환경

| 항목 | 내용 |
| --- | --- |
| 엔진 | Unity 6000.3.25f1 LTS |
| 언어 | C# |
| 네트워크 | Photon Fusion 2.0.13 |
| 빌드 대상 | Android APK·AAB, Windows x64 개발/QA 클라이언트 |
| Android 빌드 확인 | IL2CPP, API 25 이상·target/compile 36, ARMv7 + ARM64 |

## 프로젝트 열기

1. 저장소를 복제합니다.

   ```bash
   git clone https://github.com/ChoiDaeYoung-94/sudden-force-fps.git
   ```

2. Unity Hub에서 복제한 `sudden-force-fps` 폴더를 프로젝트로 추가합니다.
3. **Unity 6000.3.25f1**로 프로젝트를 엽니다. Android 빌드에는 Android Build Support와 SDK/NDK/OpenJDK가 필요합니다.
4. `Assets/Scenes/Login.unity`를 열고 로그인 및 네트워크 설정을 확인합니다.

Photon 서비스 설정이 유효해야 실제 로비·방에 연결됩니다. Android 인증에는 기존 PGS 프로젝트의 패키지·서명 인증서 연결과 테스트 계정 권한이 필요합니다. 저장소만 복제한 새 계정에서 인증 성공을 보장하지 않습니다.

Editor와 Windows Standalone은 개발/QA용으로 Google 인증을 생략하고 로비로 진입합니다. 이 경로의 성공은 Android PGS 인증 성공을 의미하지 않으며, Android 빌드는 실제 Google 인증 경로를 사용합니다.

### 두 피어 실행

1. Editor에서 `Login.unity`를 열고 Play를 실행합니다. 두 번째 피어는 별도의 Windows 개발 Player 또는 인증 가능한 Android 기기로 준비합니다.
2. 각각 닉네임을 설정합니다. 한 피어가 `Create Room`에서 방 이름과 `DesertHouse`를 선택해 공개방을 만듭니다.
3. 다른 피어가 방 목록에서 참가하고, 서로 다른 팀을 선택합니다. Client가 `READY`를 누른 뒤 Host가 `START`를 누릅니다.
4. 결과 화면의 로비 복귀 버튼으로 돌아가 새 방에 재참가할 수 있습니다. 경기 중 Host 연결이 종료되면 Client는 새 Runner로 로비에 자동 복귀합니다. 실제 PC 양방향 검증 근거는 검증 기록의 P01–P06에 있습니다.

Windows 빌드는 활성 대상을 Windows x64로 바꾸고 4개 씬을 포함한 Development Build로 만듭니다. 기존 PC QA는 빌드 동안 Standalone define에 `URP_COMPATIBILITY_MODE`를 추가하고 완료 후 원래 define을 복원했습니다. [검증 빌드 도구](Assets/Scripts/Editor/MultiplayerValidationBuild.cs)는 대상 전환이나 Player 실행을 자동으로 하지 않습니다. 상세 빌드·설정 보존 절차는 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)을 따릅니다.

### 경기 규칙과 조작

현재 기본 규칙은 팀별 20점 선취 또는 300초 제한입니다. 적 처치로 팀 점수가 1 올라가며, 시간이 끝나면 높은 점수 팀이 승리하고 동점은 무승부입니다. 상대 팀 참가자가 모두 이탈하면 남은 팀이 승리합니다. 경기 종료 후에는 전투·리스폰이 중단됩니다.

기본 소총은 탄창 30발, 재장전 2초이며 피해는 머리 100·몸통 25·팔/다리 18입니다. 호스트가 명중·피해를 판정하며 아군 피해는 적용하지 않습니다. 사망 후 3초부터 안전한 팀 스폰을 찾고, 가능할 때 HP 100·탄약 30으로 리스폰합니다. 안전한 스폰이 없으면 대기하므로 항상 정확히 3초에 부활하는 것은 아닙니다.

| 동작 | PC | 모바일 구현 |
| --- | --- | --- |
| 이동 | WASD 또는 방향키 | 왼쪽 조이스틱 |
| 시점 | 게임 영역 클릭으로 커서 캡처 후 마우스 이동 | 오른쪽 빈 영역 터치 드래그 |
| 사격 | 마우스 왼쪽 버튼 누르기/유지 | 사격 버튼 누르기/유지 |
| 달리기 | Shift 유지 | 달리기 버튼 유지 |
| 재장전 | R | 재장전 버튼 |
| 스코어보드 | Tab 유지 | 점수 버튼으로 열고 닫기 |
| 커서 해제 | Esc, 게임 영역 클릭으로 재캡처 | 해당 없음 |

스코어보드를 열면 전투 입력이 차단됩니다. Editor의 `Preview Touch Controls`는 터치 UI 표시·포인터 동작을 점검하는 기능입니다. Editor는 PC 입력 경로를 유지하므로 이 미리보기로 실제 다중 터치, 기기 포커스·백그라운드 복귀, 성능을 검증했다고 할 수 없습니다. 모바일 조작 표는 구현 설명이며 실기기 통과 결과는 아닙니다.

### 씬 구성

| 씬 | 용도 |
| --- | --- |
| `Assets/Scenes/Login.unity` | 로그인 |
| `Assets/Scenes/Lobby.unity` | 로비 |
| `Assets/Scenes/Room.unity` | 방 |
| `Assets/Scenes/Game/DesertHouse.unity` | 게임 맵 |

## 다운로드

[과거 APK 산출물](https://drive.google.com/file/d/1SQojyQafq9IdmNvTONF80G8ri3QPqeXk/view?usp=sharing) — 기존 링크이며 최신 전투·로그인·복귀 수정이 포함된 빌드로 검증하지 않았습니다.

최신 개발 APK는 2026-10-07 소스 `ea29fe6` 기준의 `SuddenForceFPS.DebugValidation.apk`이며 공개 다운로드 URL은 아직 없습니다. Android Debug 서명 개발본으로, Google Play 배포용 서명 AAB와 구분합니다. 파일 크기·SHA-256·빌드 및 검사 근거는 [Android 릴리스 점검표의 최신 APK 기록](ANDROID_RELEASE_CHECKLIST.md#최신-전투로그인복귀-포함-개발-apk-2026-10-07)을 확인합니다. APK 빌드·서명 검사는 실기기 로그인·한 판·Play 배포 검증을 대신하지 않습니다.

## 외부 라이브러리

- [Photon Fusion](https://doc.photonengine.com/fusion/current/getting-started/sdk-download)
- [play games plugin](https://github.com/playgameservices/play-games-plugin-for-unity/releases)
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
- [MiniJSON](https://github.com/Unity-Technologies/UnityCsReference/blob/master/External/JsonParsers/MiniJson/MiniJSON.cs)
- [Keystore Helper](https://assetstore.unity.com/packages/tools/utilities/keystore-helper-58627)
- [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068)
- [Safe Area Helper](https://assetstore.unity.com/packages/tools/gui/safe-area-helper-130488)

Google 로그인 참고: [Google Play Games Services 로그인](https://developer.android.com/games/pgs/android/android-signin?hl=ko)

## 빌드

Android 배포용 빌드 진입점은 [BuildScript.cs](Assets/Scripts/Editor/BuildScript.cs)입니다. Android를 활성 대상으로 선택하고 Play·컴파일·임포트가 끝난 상태에서 실행합니다. 활성 씬 4개와 기존 앱 패키지 `com.AeDeong.SuddenForceFPS`, 기존 서명 키·alias 및 `BuildInfo/buildinfo.txt`가 필요합니다.

| 방식 | 방법 |
| --- | --- |
| Unity 에디터 | `Build > AOS > APK` 또는 `Build > AOS > AAB`. `Build/AOS`는 스크립트가 생성 |
| 전용 배치 빌드 | Android 대상으로 `BuildScript.BuildAOSAPK` 또는 `BuildScript.BuildAOSAAB` 실행. 성공/실패에 따라 전용 배치 Editor exit 0/1 |
| 개발 APK 검사 | 별도 [AndroidValidationBuild.cs](Assets/Scripts/Editor/AndroidValidationBuild.cs)의 `Queue` 경로. 기존 Android debug key로 외부 새 디렉터리에 생성하며 릴리스 경로와 구분 |

배포용 빌드는 기존 서명 비밀번호를 Editor에 안전하게 입력하거나 `SUDDEN_FORCE_KEYSTORE_PASSWORD`와 `SUDDEN_FORCE_KEYALIAS_PASSWORD` 환경변수로 전달해야 합니다. 값은 소스·README·로그에 기록하지 않습니다. 새 키 생성으로 기존 앱 서명을 대체하지 않습니다.

기본 versionCode는 로컬 ledger와 PlayerSettings 중 큰 값에 1을 더합니다. `SUDDEN_FORCE_VERSION_CODE`를 지정하면 두 로컬 값보다 크고 2,100,000,000 이하인 정수여야 합니다. Play에 올리기 전 Console의 기존 업로드 코드도 확인해야 합니다. 실패한 빌드도 예약된 코드에 빈 번호를 남길 수 있습니다. 최신 debug code3은 Play 업로드 코드 예약을 뜻하지 않습니다.

APK 메뉴는 Development/LZ4, AAB 메뉴는 LZ4HC 옵션입니다. 최신 개발 APK의 성공은 기존 업로드 키로 서명한 릴리스 APK/AAB 성공을 의미하지 않습니다. 도구체인·서명·Resolver 재실행 시 템플릿 점검과 배포 선행 조건은 [Android 릴리스 점검표](ANDROID_RELEASE_CHECKLIST.md)에 있습니다.

### 기존 CI/CD 구성의 현재 상태

- [워크플로](.github/workflows/cicd.yml)는 커밋 메시지에 `ci skip`이 있으면 Checkout 작업을 건너뛰도록 작성되어 있습니다.
- CI의 Unity 실행 경로는 `2022.3.43f1`이며, 현재 프로젝트는 `6000.3.25f1`입니다. 재사용 전에 빌드 PC 환경·버전·서명 입력·배포 단계를 갱신해야 합니다.
- CI 내부 작업 폴더 이름 `SuddenForceFPS`는 체크아웃 경로로 사용됩니다. 저장소 이름과 별도로 관리되는 경로입니다.
- 기존 `main` push → 자체 빌드 PC AAB → App Center 구성과 [unity-cicd](https://github.com/ChoiDaeYoung-94/unity-cicd) Python 안내는 과거 구성입니다. Google Play 테스트 배포 경로로 아직 전환하지 않았으며 현재 동작하는 릴리스 절차로 검증하지 않았습니다.
- 기존 구현은 AAB 업로드 후 배포 그룹에 이메일 알림을 보내는 방식이었습니다.

## 계획·검증·라이선스

- [프로젝트 계획과 범위](PROJECT_PLAN.md)
- [멀티플레이 검증 결과·미검증 항목](MULTIPLAYER_TEST_PLAN.md)
- [Android 빌드·서명·Google Play 테스트 배포 점검](ANDROID_RELEASE_CHECKLIST.md)
- [외부 에셋과 라이선스](THIRD_PARTY_ASSETS.md)
- [기존 이슈와 현재 작업 연결](ISSUE_TRACKING.md)
