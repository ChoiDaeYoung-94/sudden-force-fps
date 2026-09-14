# Sudden Force FPS

> Photon Fusion을 활용한 멀티플레이 기능을 갖춘 Unity 3D FPS 게임 프로젝트입니다.

## 주요 구현

| 기능 | 내용 및 관련 코드 |
| --- | --- |
| 멀티플레이 | Photon Fusion 활용 · [NetworkRunnerManager.cs](Assets/Scripts/Managers/NetworkRunnerManager.cs) |
| 방 목록 UI | 항목을 재사용하는 스크롤 뷰 · [RoomManage.cs](Assets/Scripts/UI/RecyclableScrollView/RoomManage.cs) |
| 씬 전환 | 대상 씬을 Additive로 로드한 뒤 현재 씬을 해제 · [SceneManager.cs](Assets/Scripts/Managers/SceneManager.cs) |
| 로그인 | Google Play Games Services v2 적용 |

## 개발 목표

- [x] Photon Fusion 기반 멀티플레이
- [x] Google Play Games Services v2 적용
- [ ] 스킬 시스템 및 장비 시스템 구현
- [ ] 프리팹 구성과 재사용 방식 개선

시연 영상과 Google Play 공개 링크는 준비 중입니다.

## 개발 환경

| 항목 | 내용 |
| --- | --- |
| 엔진 | Unity 2022.3.52f1 LTS |
| 언어 | C# |
| 네트워크 | Photon Fusion |
| 빌드 대상 | Android APK·AAB |

## 프로젝트 열기

1. 저장소를 복제합니다.

   ```bash
   git clone https://github.com/ChoiDaeYoung-94/sudden-force-fps.git
   ```

2. Unity Hub에서 복제한 `sudden-force-fps` 폴더를 프로젝트로 추가합니다.
3. **Unity 2022.3.52f1**로 프로젝트를 엽니다.
4. `Assets/Scenes/Login.unity`를 열고 로그인 및 네트워크 설정을 확인합니다.

Photon Fusion과 Google Play Games 연동 기능을 실행하려면 해당 서비스 설정이 필요합니다.

### 씬 구성

| 씬 | 용도 |
| --- | --- |
| `Assets/Scenes/Login.unity` | 로그인 |
| `Assets/Scenes/Lobby.unity` | 로비 |
| `Assets/Scenes/Room.unity` | 방 |
| `Assets/Scenes/Game/DesertHouse.unity` | 게임 맵 |

## 다운로드

[APK 다운로드](https://drive.google.com/file/d/1SQojyQafq9IdmNvTONF80G8ri3QPqeXk/view?usp=sharing)

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

Android 빌드 결과는 프로젝트 루트의 `Build/AOS`에 생성됩니다. 시작 전 루트에 `Build` 폴더를 준비합니다.

| 방식 | 방법 |
| --- | --- |
| Unity 에디터 | `Build > AOS > APK` 또는 `Build > AOS > AAB` |
| Python CLI | [unity-cicd](https://github.com/ChoiDaeYoung-94/unity-cicd)의 `build.py` 사용. AAB와 APK 빌드 |
| GitHub Actions | 기존 구성은 `main` push → 자체 빌드 PC에서 AAB 빌드 → App Center 배포 |

### 기존 CI/CD 구성의 현재 상태

- [워크플로](.github/workflows/cicd.yml)는 커밋 메시지에 `ci skip`이 있으면 Checkout 작업을 건너뛰도록 작성되어 있습니다.
- CI의 Unity 실행 경로는 `2022.3.43f1`이며, 현재 프로젝트 설정은 `2022.3.52f1`입니다. 재사용 전에 빌드 PC 환경과 버전을 맞춰야 합니다.
- CI 내부 작업 폴더 이름 `SuddenForceFPS`는 체크아웃 경로로 사용됩니다. 저장소 이름과 별도로 관리되는 경로입니다.
- App Center의 배포 기능은 2025년 3월 31일 종료되었습니다. 기존 배포 단계는 현재 사용할 서비스에 맞게 교체해야 합니다. [Microsoft 안내](https://learn.microsoft.com/en-us/appcenter/retirement)
- 기존 구현은 AAB 업로드 후 배포 그룹에 이메일 알림을 보내는 방식이었습니다.
