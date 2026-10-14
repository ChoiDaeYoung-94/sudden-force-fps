# Sudden Force FPS

> Photon Fusion 2 기반의 호스트·클라이언트 멀티플레이 Unity 3D FPS 프로젝트입니다. 로비에서 방을 만들고 팀을 선택해 DesertHouse에서 한 판을 진행합니다.

2026-10-14 기준으로 기본 전투·경기와 기존 Android 실기기 PGS 로그인·두 피어 경기 검증을 진행했고, `1.0.194 / versionCode 5`를 Google Play 내부 테스트에 출시했습니다. Play 설치본의 로그인·경기, 물리 멀티터치·저사양·실제16KB 기기와 남은 에셋 출처 검증은 미완료입니다. 최신 범위와 날짜별 근거는 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)와 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)을 따릅니다.

새 PC에서는 원격 `f61be81`을 새 워크트리에 복제했습니다. SDK·NDK 설치와 도구 실행, 공식 OpenJDK의 별도 경로 및 실제 Win64 컴파일을 확인했습니다. 2026-10-14 후속 진단에서 임시 Android compiler 옵션 `-parallel-`로 편집기 컴파일108개는 오류0으로 완료됐지만, 후속 비서명 ARM64 Gradle export는 Unity APIUpdater의 AccessViolation/CLR 오류로 스크립트 빌드 단계에서 실패했습니다. 설정·소스·버전은 원복했으며 최종 컴파일 실패 플래그는 남아 있습니다. BIOS 업데이트는 사용자 결정에 따라 보류했고 원인 및 새 PC 빌드 복구는 미완료입니다. 기존 Play 버전5의 서명된 범용 APK는 사용자 지정 Drive에 전달했으며 설치 전입니다. 과거 외부 백업과 미커밋 변경의 복구를 가정하지 않습니다. 최신 근거는 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)를 따릅니다.

HUD 이미지7개는 사용자가 직접 제작 또는 AI 생성했다고 확인해 유지합니다. 캐릭터·무기·맵·로딩의 공식 출처 후보와 남은 취득·원 패키지 확인은 [에셋 기록](THIRD_PARTY_ASSETS.md)을 따릅니다.

## 주요 구현

| 기능 | 내용 및 관련 코드 |
| --- | --- |
| 멀티플레이 | Fusion의 ClientServer 로비, Host/Client 방 접속, 네트워크 입력·상태 동기화, 호스트 권한의 사격·피해 판정 · [NetworkRunnerManager.cs](Assets/Scripts/Managers/NetworkRunnerManager.cs) |
| 방 목록 UI | 항목을 재사용하는 스크롤 뷰 · [RoomManage.cs](Assets/Scripts/UI/RecyclableScrollView/RoomManage.cs) |
| 전투 | 이동·시점·달리기, 총기 반동, 부위별 피해, 탄약·재장전, 사망·리스폰 · [GamePlayerNetworkData.cs](Assets/Scripts/Creature/Player/GamePlayerNetworkData.cs) |
| 경기·HUD | 팀 점수·타이머·KD·킬 피드·스코어보드·결과 화면 · [NetworkMatchState.cs](Assets/Scripts/Game/Match/NetworkMatchState.cs) |
| 씬·복귀 | 일반 씬 전환과 Fusion 경기 씬 로드, persistent Managers가 종료된 Runner를 정리하고 새 로비 Runner를 생성 · [Managers.cs](Assets/Scripts/Managers/Managers.cs) |
| 로그인 | Android Google Play Games Services v2 인증·취소/실패/30초 시간 초과·재시도 처리. 기존 업로드 키 APK의 실기기 인증을 확인했으며 Play 앱 서명 설치본 인증은 별도 미검증 · [Login.cs](Assets/Scripts/UI/Login.cs) |

Fusion은 방 접속뿐 아니라 입력 전달, 권한에 따른 상태 복제, Hitbox 지연 보정 쿼리와 경기 씬 전환에 사용합니다. 지연·손실 환경의 전체 품질 검증은 별도이며, 상세 통과 범위와 미검증 항목은 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)에 구분합니다.

## 개발 목표

- [x] Photon Fusion 기반 멀티플레이
- [x] Google Play Games Services v2 로그인 경로 구현
- [x] 기존 업로드 키 APK의 Android 실기기 인증·두 피어 모바일 경기 확인(최종 품질 검증과 구분)
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
| Android 빌드 확인 | IL2CPP, API 25 이상·target/compile 36. 기존 개발 APK와 별도로 내부 테스트 code5는 ARM64 |

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

현재 전달용 파일은 Play Console의 기존 내부 테스트 버전5에서 다운로드한 `SuddenForceFPS-1.0.194-code5-PlaySigned.apk`입니다. 크기는99,434,034 bytes, SHA256은 `FC64BA8EFF0521A8CD12081293C15B84019D1B481E5B69B0E844DFBCCFFAAABF`이며 Play 앱 서명 인증서를 사용합니다. 사용자 지정 Drive에 전달한 파일로 공개 다운로드 URL은 이 문서에 게시하지 않습니다. 새 PC에서 새로 빌드한 APK가 아니며 Drive 직접 설치 성공도 Play 스토어 설치 검증을 대체하지 않습니다.

이전 2026-10-07 소스 `ea29fe6`의 `SuddenForceFPS.DebugValidation.apk`는 Android Debug 서명 개발본입니다. 위 Play 서명 APK와 구분하며 당시 파일·검증 범위는 [Android 릴리스 점검표의 APK 기록](ANDROID_RELEASE_CHECKLIST.md#최신-전투로그인복귀-포함-개발-apk-2026-10-07)에 보존합니다.

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

APK 메뉴는 Development/LZ4, AAB 메뉴는 non-Development/LZ4HC/CleanBuildCache이며 둘 다 상세 빌드 보고서를 생성합니다. APK 메뉴를 non-Development 릴리스 APK로 표현하지 않습니다. 도구체인·서명·Resolver 재실행 시 템플릿 점검과 배포 선행 조건은 [Android 릴리스 점검표](ANDROID_RELEASE_CHECKLIST.md)에 있습니다.

### 기존 CI/CD 구성의 현재 상태

- Unity2022/macOS/self-hosted `buildpc` → App Center 구성은 [원문 보관 파일](.github/legacy/cicd.yml.disabled)로 이동했습니다. 활성 `.github/workflows`에 실행 정의가 없으므로 이 소스에는 자동 Android 빌드·배포나 PR 검증 CI가 없습니다. 과거 [unity-cicd](https://github.com/ChoiDaeYoung-94/unity-cicd) 안내는 현재 배포 절차가 아닙니다.
- [App Center 배포 기능은 종료](https://learn.microsoft.com/en-us/appcenter/retirement)되었습니다. 로컬 빌드는 Unity6000.3.25f1 절차를 사용하며 기존 Sudden Force 앱의 Google Play 내부 테스트 code5 업로드·출시는 완료했습니다. Play 설치·로그인·경기 검증은 남아 있습니다.
- 기존 queued/running Actions가 이 파일 이동만으로 취소됐다고 가정하지 않습니다. GitHub의 실제 실행 상태와 필요한 취소는 별도로 확인합니다. runner의 현재 OS·Unity6 라이선스·Android 도구체인·secrets 가용성은 아직 검증하지 않았습니다.
- 후속 CI는 확인된 runner에서 수동 `workflow_dispatch` 단일 빌드 작업으로 검증한 뒤 추가합니다. 로컬 빌드·실기기 검증과 GitHub CI 결과를 구분하며, PR 병합으로 빌드·배포를 자동 실행하지 않습니다. 진행 범위는 [이슈 #44](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/44)에 기록합니다.

## 계획·검증·라이선스

- [프로젝트 계획과 범위](PROJECT_PLAN.md)
- [멀티플레이 검증 결과·미검증 항목](MULTIPLAYER_TEST_PLAN.md)
- [Android 빌드·서명·Google Play 테스트 배포 점검](ANDROID_RELEASE_CHECKLIST.md)
- [외부 에셋과 라이선스](THIRD_PARTY_ASSETS.md)
- [기존 이슈와 현재 작업 연결](ISSUE_TRACKING.md)
