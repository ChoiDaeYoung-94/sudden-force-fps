# Android 첫 빌드 및 Google Play 테스트 배포 점검

## 최신 검증 상태 (2026-10-07)

아래 초기 조사 기록의 현재값과 미실행 표시는 당시 시점의 기록입니다. 첫 개발용 Android APK는 API 36으로 빌드하고 산출물 서명·아키텍처 검사를 완료했습니다. 기존 Sudden Force 앱과 업로드 인증서 일치도 확인했습니다. 실제 Android 기기의 설치·Google 계정 인증·한 판 플레이 및 서명된 AAB·Play 업로드는 아직 완료하지 않았습니다.

로그인 복구 변경은 `Login.cs` 한 파일에 적용했습니다. 실제 Win64 Login→Lobby 진입과 Android 대상 컴파일을 통과했고, 타입 이름만 바꾼 외부 Android 분기에서 가짜 SDK 응답으로 실패·취소·수동 재시도·30초 시간 초과·중복/늦은 응답·화면 이탈을 검증했습니다. 최초 검증 보조 코드의 Start 순서와 고정 씬 이름 가정 오류는 보정 후 재검증하고 원본 결과도 보존했습니다.

30초 시간 초과는 앱의 대기와 UI를 복구하며 Google SDK/OS 계정 창 자체를 취소하지 않습니다. 진행 중인 SDK 요청이 남으면 추가 요청을 쌓지 않습니다. 늦은 성공만으로 로비에 진입하지 않으며 사용자 재시도에서 인증 상태를 다시 확인합니다. 실제 계정 창·서명별 OAuth·기기 동작은 미검증입니다. PlayFab 및 서버 인증 코드 요청은 아직 통합하지 않았습니다.

근거: `D:\meee\git\sudden-force-fps-backups\20261007-login-retry-recovery`. 종료 후 Win64/빈 씬/Play 종료/자동 tick 중지 상태, 프로젝트 설정과 Android 플러그인 바이트 동일, 기존 URP 변경 해시 보존을 확인했습니다.

2026년 10월 7일(KST) 기준입니다. `PROJECT_PLAN.md` 단계 6~7의 선행 조사입니다. 초기 조사 후 별도 배정으로 광고·결제 의존성 제거와 Editor smoke test를 완료했고, 이어서 빌드 스크립트·custom Gradle 템플릿·Windows QA 로그인 코드를 정리했습니다. Android BuildTarget 전환, 빌드, target API/서명 ProjectSettings 변경, 웹 계정 조작은 하지 않았습니다. 비밀번호, 개인 키, 계정 토큰은 기록하지 않습니다.

## 판단과 우선순위

Android 빌드 도구는 설치되어 있습니다. 첫 APK/AAB 빌드는 아직 실행하지 않았으며, 다음 장애를 먼저 해결해야 합니다.

1. 프로젝트 target SDK 34를 배포 기준 API 36 이상으로 갱신하고 Android 16 동작을 검증합니다.
2. Unity 6.3.25의 Gradle/AGP 기준으로 custom 템플릿 코드 이식을 완료했습니다. 실제 Android resolve·생성 Gradle·빌드 검증은 제어권 인계 후 수행합니다.
3. 기존 keystore의 비밀번호·개인 키 접근과 Play Console 업로드 인증서 일치를 안전하게 확인합니다. 기존 키를 재생성하거나 교체하지 않습니다.
4. BuildScript의 define·버전·실패 반환·에디터 종료 코드 정리를 완료했습니다. 별도 빌드 세션에서 실제 실행을 검증해야 합니다.
5. Google Play 앱·PGS 연결, 서명 인증서, 테스터 권한은 총괄의 실제 Console 확인이 필요합니다.
6. 구형 App Center CI를 Google Play 테스트 배포 절차로 전환합니다. 첫 검증은 로컬 빌드와 Console 내부 테스트로 진행할 수 있습니다.
7. 최신 사용자 요구에 따라 광고·결제 의존성 제거를 완료했습니다. 최종 적용·검증 결과는 문서 끝에 기록했습니다.

## 현재 프로젝트 값

근거는 `ProjectSettings/ProjectSettings.asset`, `ProjectSettings/ProjectVersion.txt`, `ProjectSettings/EditorBuildSettings.asset`, `Assets/Scripts/Editor/BuildScript.cs`입니다. Editor에서 적용 중인 External Tools 경로와 변경 중인 다른 담당 작업은 이번 조사에서 조회하지 않았으므로, 빌드 직전에 다시 확인해야 합니다.

| 항목 | 저장된 현재값 | 남은 조건 |
| --- | --- | --- |
| Unity | 6000.3.25f1, revision e1dba0a9aba4 | CI도 같은 버전 사용 |
| Android package | `com.AeDeong.SuddenForceFPS` | 총괄 Console 읽기 확인: 기존 Sudden Force 앱과 일치; 신규 앱 생성 불필요 |
| versionName | `1.0.0` | 배포할 사용자 표시 버전 확정 |
| versionCode | 2 | 총괄 Console 확인 최대 사용 code2; 다음 업로드는 미사용 code3 이상 |
| 최소 SDK | 25, Android 7.1 | 지정 테스트 기기와 플러그인 최소 요구 비교 |
| target SDK | 34, Android 14 | API 36 이상으로 갱신 예정 |
| 아키텍처 | 값 3 = ARMv7 + ARM64 | AAB/APK에 실제 `arm64-v8a` 포함 확인 |
| 스크립팅 | Android backend 1 = IL2CPP | 실제 Android C#/IL2CPP 빌드 미검증 |
| managed stripping | 직렬화 값 4 | PGS/Fusion 코드 stripping 영향 실기기 확인 |
| 엔트리 | `androidApplicationEntry: 1`, custom manifest의 UnityPlayerActivity | Unity 6 출력 manifest와 엔트리 일치 확인 |
| custom templates | main manifest, main/base/settings Gradle, Gradle properties 활성 | 기존 Android 플러그인 보존하며 Unity 6 기본과 비교·이식 |
| 서명 | custom keystore 활성, `src/AeDeong.keystore`, 저장된 alias는 빈 값 | BuildScript는 alias `aedeong` 지정; 수동 빌드와 스크립트 경로 모두 확인 |
| 씬 순서 | Login, Lobby, Room, Game/DesertHouse, 모두 enabled | 네트워크 담당 변경 후 최종 씬 목록 재확인 |

## 설치된 Android 도구

설치 루트는 `C:\Program Files\Unity\Hub\Editor\6000.3.25f1\Editor\Data\PlaybackEngines\AndroidPlayer`입니다. 파일 존재 및 각 도구의 배포 metadata로 확인했습니다.

| 항목 | 확인값 | 판정 |
| --- | --- | --- |
| Android Build Support | AndroidPlayerBuildProgram, Android editor extension, Bee, Variations 존재 | 설치됨; 실제 빌드 성공은 미확인 |
| SDK platforms | android-34, android-35, android-36, android-37.0 | 필요한 API 36 설치됨; 37을 자동 선택하지 않도록 target 명시 권장 |
| Build Tools | 36.0.0 | 설치됨 |
| Command-line Tools | 16.0 | 설치됨 |
| Platform Tools | 36.0.0, adb.exe 존재 | 설치됨; 연결 기기는 조회하지 않음 |
| NDK | r27c, 27.2.12479018 | Unity 공식 지원 버전과 일치 |
| OpenJDK | Temurin 17.0.18+8 | Unity 공식 JDK 17과 일치 |
| Gradle | gradle-launcher-9.3.1.jar | 6000.3.25f1 릴리스 노트의 갱신과 일치 |
| 기본 AGP | 설치된 baseProjectTemplate.gradle의 9.0.0 | 프로젝트 custom 7.4.2와 차이 |

[Unity Android 지원 의존성](https://docs.unity3d.com/6000.3/Documentation/Manual/android-supported-dependency-versions.html)은 NDK r27c/JDK 17 및 이 버전의 SDK 도구를 명시합니다. [6000.3.25f1 릴리스 노트](https://unity.com/releases/editor/whats-new/6000.3.25f1)는 Gradle을 9.3.1로 올렸다고 명시합니다. 일반 버전 호환 표보다 해당 패치의 설치 파일과 릴리스 노트를 우선 확인했습니다. 시스템 JAVA_HOME이나 사용자 External Tools override가 적용되어 있으면 빌드 전에 별도로 비교해야 합니다.

## 수정 전 Gradle와 manifest 점검

| 파일 | 조사 결과 | 처리·검증 조건 |
| --- | --- | --- |
| baseProjectTemplate.gradle | application/library AGP 7.4.2 고정, `task clean(type: Delete)`와 `rootProject.buildDir` 사용 | 설치된 Unity 6 템플릿의 AGP 9.0.0 및 `tasks.register` 기준으로 이식; 단순 숫자 교체만으로 완료 판정하지 않음 |
| mainTemplate.gradle | Java 11 compileOptions, namespace, 구형 lint/aapt DSL, GPGS support 2.0.0 | Java source 11 자체가 JDK 17 설치 오류라는 뜻은 아님; Unity 6 템플릿·AGP와 DSL/placeholder 호환을 비교 |
| settingsTemplate.gradle | 존재하지 않는 예전 `D:/Documents/01.Projects/.../m2repository` 절대 URL | EDM Android Resolve로 현재 프로젝트 GeneratedLocalRepo 참조 갱신; 다른 PC/CI에서도 재현 가능하게 처리 |
| GeneratedLocalRepo | GPGS support 2.0.0 AAR/POM 실제 존재 | 로컬 경로 수정 후 Gradle dependency resolve 검증 |
| gradleTemplate.properties | AndroidX, Jetifier 활성 | 패키지 충돌 여부 빌드 결과로 판단 |
| AndroidManifest.xml | UnityPlayerActivity에 MAIN/LAUNCHER intent-filter; 해당 파일에 exported 명시 없음 | 병합된 최종 manifest에서 `android:exported` 및 엔트리 확인; 파일만 보고 최종 누락으로 단정하지 않음 |
| GPGS Android library | Nearby용 Bluetooth/location/Wi-Fi 권한 포함 | 실제 Nearby 기능을 안 쓴다면 포함 필요성 검토; 최종 권한·정책/런타임 거절 흐름 확인 |

GPGS POM은 `play-services-games-v2:20.1.2`, `play-services-nearby:18.5.0`을 참조합니다. Android target 36/AGP 9과의 전체 의존성 호환은 첫 resolve와 빌드에서 확인해야 합니다. 이번 조사에서는 플러그인 업데이트나 임의 SDK 수정은 하지 않았습니다.

## 기존 서명 키

`src/AeDeong.keystore`는 존재하며 2,031바이트이고 Git 추적 대상입니다. 파일을 복사·수정·내보내지 않았습니다. keytool의 읽기 전용 공개 인증서 metadata 확인 결과 JKS, 1개 PrivateKeyEntry, alias `aedeong`, RSA 2048-bit이며 인증서 만료일은 2072년 6월 15일입니다. keytool은 인증서의 SHA1withRSA를 weak로 표시했습니다. 이는 인증서 자체의 서명 알고리즘 표시이며, 최종 APK 서명 방식이나 Play 업로드 수락 여부를 확정하지 않습니다.

비밀번호를 제공하지 않아 저장소 무결성과 개인 키 복호화·서명 가능 여부는 검증되지 않았습니다. 초기 BuildScript의 빈 비밀번호 상수는 후속 수정에서 제거하고 아래 서명 환경변수/기존 Editor 입력을 사용하도록 바꿨습니다. 실제 안전한 비밀값 제공 여부는 미확인입니다. 자격 증명은 문서/소스/명령줄/빌드 로그에 기록하지 않고 안전한 입력 경로로 전달해야 합니다. Git 추적된 keystore는 기존 사용 여부·노출 이력·비밀 관리 방식을 총괄이 확인하고, 무단 삭제·회전은 하지 않습니다.

- [x] Play Console 기존 앱의 업로드 인증서와 로컬 인증서 비교: SHA-256/SHA-1 모두 일치
- [x] Play App Signing 사용 여부 및 앱 서명 SHA-256 별도 확인(총괄 Console 읽기)
- [ ] alias/private key의 비밀번호와 실제 서명 가능 여부 안전하게 검증
- [ ] 사용자 설치 APK의 인증서와 Play 설치본 인증서 차이를 PGS OAuth에 반영
- [ ] 기존 앱 업데이트 호환성 유지; 필요 시 공식 업로드 키 복구 절차를 총괄 승인 후 사용

Play App Signing의 업로드 키와 사용자에게 전달되는 앱 서명 키는 역할이 다릅니다. [공식 Play App Signing 문서](https://support.google.com/googleplay/android-developer/answer/9842756?hl=en)를 기준으로 Console 인증서를 비교해야 합니다.

2026-10-07 추가 확인: 로컬 `AeDeong.keystore`의 alias `aedeong` 공개 인증서는 Console의 **업로드 인증서**와 SHA-256 및 SHA-1 모두 일치합니다. 비밀번호를 제공하지 않는 metadata 조회였으며 비밀번호 추측, 개인 키 내보내기, 키 교체·reset은 하지 않았습니다. 저장소 무결성과 실제 개인 키 복호화·서명 가능 여부는 여전히 미검증입니다. 증거는 `D:\meee\git\sudden-force-fps-backups\20261007-android-prebuild\upload-certificate-comparison.json`입니다.

| 공개 인증서 | 확인한 지문 |
| --- | --- |
| 로컬/Console 업로드 SHA-256 | `4D:07:25:92:22:8C:99:1D:3F:6A:0E:DA:76:8C:8C:BE:E4:08:95:C3:B1:F4:A6:FB:93:79:54:0F:04:89:65:E0` |
| 로컬/Console 업로드 SHA-1 | `E9:56:B8:AD:10:19:A6:A2:E3:B8:89:B1:21:C1:EF:0E:72:74:18:E0` |
| Play 앱 서명 SHA-256(총괄 Console 확인) | `01:B2:84:6B:34:83:5C:D1:45:26:40:48:DD:5C:64:2B:6F:11:2D:35:AA:8F:0B:60:1D:BA:11:F4:9E:10:0D:86` |
| Play 앱 서명 SHA-1(총괄이 다운로드 공개 인증서 파싱) | `CF:E6:15:DA:6A:91:0B:3A:78:51:D2:29:EA:10:B2:66:BF:00:9A:53` |

업로드 인증서와 Play 앱 서명 인증서는 다릅니다. 총괄이 Console에서 다운로드한 공개 인증서 `C:\Users\pc_17\Downloads\deployment_cert.der`(1,420바이트, 2026-10-07 12:27 KST)를 X509로 파싱한 앱 서명 SHA-256은 Console DigitalAssetLinks 표시와 일치했습니다. 위 SHA-1/SHA-256은 공개 인증서 metadata이며 비밀 키나 비밀번호가 아닙니다. 인증서 조회·다운로드로 Console 설정을 변경하지 않았습니다.

PGS Android OAuth 비교 기준은 다음과 같습니다. 같은 package `com.AeDeong.SuddenForceFPS`에 로컬 기존 키로 서명한 APK는 업로드 인증서 SHA-1 `E9:56:...:18:E0`, Play 설치본은 앱 서명 SHA-1 `CF:E6:...:9A:53`이 일치해야 합니다. 두 credential이 실제 PGS 게임에 연결되어 있는지는 아직 미확인입니다. 이번 정보로 계정 설정을 새로 만들거나 변경하지 않았습니다. 실제 APK 인증서와 설치본 인증서도 빌드·설치 후 다시 대조해야 합니다.

## Google Play Games 로그인

PGS Unity 플러그인 2.0.0과 Android 로그인 코드가 존재합니다. `GameInfo.cs`의 Android ApplicationId와 생성 manifest APP_ID 값은 일치하며 WebClientId가 채워져 있습니다. Nearby service 값은 Android package와 같습니다. 식별자의 원문은 이 문서에 복사하지 않았습니다. 이 로컬 일치는 Console의 실제 게임 연결/활성 credential/테스터 권한까지 증명하지 않습니다.

`Login.cs`는 Android에서 PlayGamesPlatform을 활성화하고 Authenticate를 호출합니다. 성공 후 Lobby로 이동합니다. Editor 및 Windows Standalone은 PC 개발·QA용 로비 진입 우회 경로를 사용하므로 해당 테스트는 실제 Google 인증 검증이 아닙니다. Windows는 출시 플랫폼이나 프로덕션 인증 지원으로 추가한 것이 아닙니다. Android 인증 경로는 유지했습니다. 인증 실패는 텍스트 표시로 끝나며 자동 retry 패널 전환은 확인되지 않습니다. 실패·취소·오프라인·앱 복귀에서 재시도와 진행 상태를 실기기로 검증해야 합니다.

총괄의 Console 확인 항목:

- [ ] 기존 Play 앱 package가 `com.AeDeong.SuddenForceFPS`인지 확인
- [ ] 같은 앱에 현재 PGS game project가 연결됐는지 확인
- [ ] Android OAuth credential의 package 및 설치본 인증서 SHA-1 일치
- [ ] 로컬 서명 APK와 Play App Signing 설치본에 필요한 credential 각각 확인
- [ ] PGS 미게시 상태라면 테스트 계정 허용 또는 테스트 트랙 권한 활성
- [ ] 실제 테스터 계정에서 인증 성공, 로비 진입, 실패 후 재시도 검증

[PGS 설정 문서](https://developer.android.com/games/pgs/console/setup)는 Android package·인증서 일치와 미게시 게임의 테스터 허용을 요구합니다. Play 설치본은 Play 앱 서명 인증서를 사용합니다. [Unity 인증 설정](https://developer.android.com/games/pgs/unity/unity-start)을 기준으로 실기기 동작을 검증합니다.

## 수정 전 BuildScript의 절차와 장애

`Build/AOS/APK`, `Build/AOS/AAB` 메뉴와 `BuildScript.BuildAOSAPK`, `BuildScript.BuildAOSAAB` 진입점이 있습니다. 메뉴는 marker를 만들고 Android define을 APK=`Debug`, AAB=빈 값으로 통째로 덮어쓴 뒤 재컴파일 대기 coroutine을 등록합니다. BuildAOS는 Android 전환, AAB 설정, IL2CPP, package/서명/버전 설정, enabled scenes 빌드를 수행합니다. APK는 Development+LZ4, AAB는 non-Development+LZ4HC입니다. 출력은 `Build/AOS/1.0.<week>.<build>.apk|aab`입니다.

| 항목 | 현재 동작·문제 | 빌드 실행 전 조건 |
| --- | --- | --- |
| defines | Android 전체 define을 덮어씀 | DOTWEEN 등 필요한 현재 define 보존; APK Debug와 Development 의도 정리 |
| 버전 | 2023-01-21부터 주차 계산; buildinfo 현재 `0,0,0`; AAB일 때 code 증가 | 저장된 code2와 로컬 ledger0이 불일치; 그대로 첫 AAB면 code1이 되어 업데이트에 부적합할 수 있음; Console 최대 사용 code 확인 후 동기화 |
| shared marker | finishversionsetting이 있으면 이전 version 재사용 | 과거 marker로 code 재사용하지 않도록 빌드 단위 관리 |
| 날짜/형식 | 문자열 DateTime 변환; 잘못된 buildinfo도 LogError 후 계속 실행 | 문화권 독립 파싱, 검증 실패 시 중단 필요 |
| 결과 전달 | 실패 때 Log만 출력 | 실패 시 비정상 exit code 및 실패 증거 반환 |
| 에디터 종료 | checkedBuilding marker가 있으면 postprocess에서 Exit(0) 예약 | 사용자 작업용 Editor에서 실행 금지; 별도 빌드 프로세스/checkout 사용 |
| 비동기 진입 | 재컴파일/Editor update를 기다리는 구조 | `-quit`로 조기 종료하지 않도록 설계; 배치 빌드 timeout/완료 확인 필요 |
| 서명 | 기존 path/alias 지정, 비밀번호 상수 빈 값 | 안전한 비밀값 주입 후 검증; 키 재생성 금지 |

### 수정한 빌드 명령 준비안

아래는 **향후 재현 명령**이며 아직 실행하지 않았습니다. 코드 정리와 오프라인 C# 컴파일은 완료했습니다. target API/Android resolve 및 동일 프로젝트를 사용 중인 Editor를 보호할 별도 checkout 또는 빌드 세션을 총괄이 배정한 다음 실행합니다. 스크립트는 Android 플랫폼이 선택되지 않았으면 명시적으로 실패하며 직접 전환하지 않습니다. 실행 시 빌드 버전과 IL2CPP·AAB 설정을 반영합니다.

```powershell
$unityExe = 'C:\Program Files\Unity\Hub\Editor\6000.3.25f1\Editor\Unity.exe'
$buildProject = 'D:\meee\git\sudden-force-fps' # 총괄이 배정한 빌드 checkout 경로로 교체
$buildLog = Join-Path $buildProject 'Build\AndroidBuildLog.txt'
New-Item -ItemType Directory -Force -Path (Join-Path $buildProject 'Build\AOS') | Out-Null
# APK: 실기기 설치용 개발 빌드
& $unityExe -batchmode -buildTarget Android -projectPath $buildProject -executeMethod BuildScript.BuildAOSAPK -logFile $buildLog
# AAB: 위 APK 프로세스 완료 및 버전/서명 검증 후 별도로 실행
& $unityExe -batchmode -buildTarget Android -projectPath $buildProject -executeMethod BuildScript.BuildAOSAAB -logFile $buildLog
```

두 명령을 무조건 연속 실행하는 자동화로 사용하지 않습니다. 새 public 진입점은 동기 BuildPlayer의 성공 BuildReport를 확인하고 전용 batch 프로세스만 exit0/실패1로 종료하므로 `-quit`를 넣지 않습니다. 메뉴 빌드는 Editor를 자동 종료하지 않습니다. 생성 파일·인증서·package/versionCode/SDK도 함께 확인해야 합니다.

서명 환경변수 이름은 `SUDDEN_FORCE_KEYSTORE_PASSWORD`, `SUDDEN_FORCE_KEYALIAS_PASSWORD`입니다. 안전한 비밀 관리 경로로 프로세스에 전달하며 문서·명령줄에 값을 적지 않습니다. 환경변수가 없으면 이미 Editor에 입력된 비밀번호를 사용하고, 사용 후 원래 세션 값으로 복원합니다. 기존 key path/alias를 유지하며 누락된 자격 증명, 다른 alias, 다른 package는 preflight 실패입니다.

매 시도 versionCode는 로컬 ledger와 PlayerSettings 중 큰 값보다 1 증가합니다. Play Console에서 더 큰 사용 코드가 확인되면 `SUDDEN_FORCE_VERSION_CODE`에 그보다 큰 새 숫자를 전달합니다. 로컬 기존 값 이하 또는 Play 상한 초과 값은 실패합니다. 실패 빌드에도 예약한 코드가 남을 수 있으며 빈 코드 간격은 허용합니다. legacy finishversionsetting marker는 더 이상 읽거나 생성하지 않습니다.

실패 시 비밀번호를 출력하지 않는 `[AndroidBuild]` preflight 메시지 또는 예외 종류와 exit1을 기록합니다. 자동 재시도·키 재생성은 하지 않습니다. APK 설치는 승인된 기기가 연결된 다음 bundled adb의 `install -r <APK>`로 수행하고, Play 설치본과 인증서가 다르면 기존 앱 데이터 보존·설치 방식부터 결정합니다.

## 16 KB와 native 라이브러리

Unity IL2CPP와 Photon NanoSockets를 사용하므로 native 검증이 필요합니다. SDK 원본 `Assets/Photon/Fusion/Plugins/NanoSockets/Android/arm64-v8a/libnanosockets.so`를 llvm-readelf로 읽었습니다. LOAD 두 개의 alignment는 모두 `0x4000`이며 GNU_RELRO 끝 주소는 `0x5db0 + 0x2250 = 0x8000`입니다. 이 파일의 ELF 조건은 16 KB 기준에 맞지만, 아직 생성되지 않은 최종 APK의 모든 `.so`와 ZIP 패키징을 보증하지 않습니다.

최신 [Android 16 KB 안내](https://developer.android.com/guide/practices/page-sizes)는 API 35+의 64-bit 호환을 요구하며, 페이지에 표시된 업데이트 차단 시점은 2027-02-01입니다. 과거 2025년 공지와 날짜 차이가 있으므로 배포 시 Console 실제 적용 상태도 확인합니다. 이번 릴리스는 유예 여부에 기대지 않고 최종 산출물의 ELF·RELRO·ZIP alignment와 16 KB 기기 실행을 통과 기준으로 삼습니다.

- [ ] 최종 APK의 모든 ARM64 native 라이브러리 ELF LOAD alignment 검사
- [ ] GNU_RELRO 끝 주소의 16 KB alignment 검사
- [ ] Build Tools 36의 `zipalign -c -P 16 -v 4 <APK>` 통과
- [ ] AAB로 생성되는 split APK와 Play Console 호환성 경고 확인
- [ ] 16 KB 환경에서 로그인·방 참가·한 판·복귀 실행

## CI 조사

`.github/workflows/cicd.yml`은 main push 또는 workflow_dispatch에서 self-hosted `buildpc` runner를 사용합니다. Unity 경로는 macOS의 2022.3.43f1로 고정돼 현재 프로젝트와 다릅니다. checkout clean=false라 marker/ledger/구형 산출물 잔존도 점검해야 합니다. 빌드 호출에는 batchmode가 없으며, Android_Build 뒤 fastlane `upload_aab`가 App Center로 업로드합니다. Play API/내부 테스트 업로드 lane은 없습니다. runner 온라인 여부, 등록된 secret, 실제 workflow 실행 이력은 계정에서 조회하지 않았습니다.

[Microsoft 공식 종료 문서](https://learn.microsoft.com/en-us/appcenter/retirement)에 따라 App Center 배포는 2025-03-31 종료 대상입니다. 현재 CI 경로는 Google Play 테스트 배포 완료 경로가 아닙니다. 후속 작업은 Unity 버전·Android 모듈 일치, 재현 가능한 clean build, 실패 exit code, artifact 보존, 안전한 서명, 내부 테스트 업로드로 분리해 갱신합니다. 첫 로컬 빌드를 검증하기 전에 CI 전면 개편을 필수로 만들 필요는 없습니다.

## Google Play 요구사항과 테스트 트랙 완료 조건

[공식 target API 요구](https://developer.android.com/google/play/requirements/target-sdk)에 따르면 2026-08-31부터 일반 Android 신규/업데이트는 API 36 이상이어야 합니다. 현재 target34는 이 릴리스 기준에 맞지 않습니다. 내부 테스트를 영구 조직 전용 private app 예외로 간주하지 않습니다. 특정 트랙에서 Console이 제시하는 검사·연장 적용 상태는 총괄이 확인합니다.

새 앱의 Play 배포에는 AAB를 준비합니다. 기존 앱의 업로드 키와 package는 유지합니다. 내부 테스트는 최대 100명이며, 검색 노출 대신 테스터 참여/스토어 링크와 허용 계정으로 설치를 확인합니다. 내부 테스트의 Data safety 노출 면제는 다른 필수 Console 항목 전체의 면제를 뜻하지 않습니다. [테스트 트랙 안내](https://support.google.com/googleplay/android-developer/answer/9845334?hl=en), [AAB 배포 안내](https://support.google.com/googleplay/android-developer/answer/9844679?hl=en)를 기준으로 진행합니다.

새 개인 개발자 계정의 closed test/production 접근 조건은 [개인 계정 테스트 요구](https://support.google.com/googleplay/android-developer/answer/14151465?hl=en)에서 별도로 확인합니다. 이번 범위는 내부 테스트이며 프로덕션 접근 승인까지 요구하지 않습니다. 실제 계정 유형·기존 앱 상태가 미확인이라 해당 조건을 내부 테스트 선행 차단으로 단정하지 않습니다.

- [ ] 총괄이 기존 앱 package, 트랙, 계정 권한, 최대 versionCode, Play App Signing/업로드 인증서를 확인했습니다.
- [ ] API36+·ARM64·올바른 서명·고유 code의 non-Development AAB가 생성됐습니다.
- [ ] APK 실기기에서 설치·Google 인증·공개 방 2명 참가·한 판·결과·로비 복귀·재참가가 확인됐습니다.
- [ ] 지정 기기에서 10분 플레이, 성능/메모리, 앱 복귀, 네트워크 단절, 호스트 이탈 결과를 기록했습니다.
- [ ] 게임과 SDK의 권한/데이터 수집, 개인정보처리방침·앱 접근 안내·콘텐츠 등급·광고 여부·대상 연령 등 Console이 요구하는 필수 항목을 실제 사용에 맞게 완료했습니다.
- [ ] 필요한 에셋 라이선스와 실제 포함 SDK의 현재 정책/기술 요구를 확인하고 광고·결제 의존성 제거를 검증했습니다.
- [ ] 내부 테스트에 AAB 업로드·처리·릴리스 완료; Console의 차단 오류가 없습니다.
- [ ] PGS 테스트 권한과 OAuth 인증서가 Play 설치본에 맞습니다.
- [ ] 허용된 테스터가 참여 링크로 Play에서 설치하고 Google 로그인과 두 명 경기·로비 복귀를 완료했습니다.
- [ ] 다음 versionCode로 업데이트 설치까지 확인하고 테스트 결과·알려진 제한·APK/AAB 위치와 빌드 방법을 기록했습니다.

총괄이 Console에서 기존 **Sudden Force** 앱(package 동일, app ID `4975903190171634984`)의 초안/내부 테스트 상태와 프로덕션 비활성을 확인했습니다. AppBundle 총 2개는 code2/name1.0.0 활성(2024-12-05 업로드), code1/name1.0.0 비활성(2024-12-04 업로드)입니다. 다음 업로드 code는 3 이상이어야 하며, 실제 업로드 직전 새로 사용된 코드가 없는지 다시 확인합니다. 기존 앱을 사용하며 새 앱을 만들지 않습니다. Console 외부 쓰기/업로드는 아직 수행하지 않았습니다.

남은 핵심 외부 정보는 PGS 연결·위 두 인증서에 대응하는 OAuth credential·테스터 권한, 안전한 서명 비밀값, 테스터 계정과 지정 Android 기기입니다. 총괄이 해당 상태를 확인한 뒤 환경 수정·첫 빌드 작업을 명시적으로 배정합니다.

## 광고·결제 제거 조사와 적용 결과

사용자는 광고와 결제 시스템을 전부 제외하도록 요구했습니다. manifest의 직접 의존성, lock 전이 그래프, Assets의 C#/asmdef 및 직렬화 GUID 참조를 읽기 조사했습니다. `unity-package-management` 스킬에 따라 manifest/lock을 수동 편집하지 않고 Unity PackageManager Client API로 제거·resolve했습니다. 아래 표와 절차는 적용 전 조사 근거이며, 적용 결과는 이어지는 최종 기록을 기준으로 합니다.

| 대상 | 증거·제거안 |
| --- | --- |
| `com.unity.purchasing@5.4.4` | 직접 의존성; 앱 코드 API 호출 및 Assets의 패키지 GUID 참조 없음. 제거 대상 |
| 광고 SDK | manifest에 Unity Ads/LevelPlay/AdMob 등 광고 패키지 없음; 검색한 앱 코드에 광고 API 호출 없음. GPGS Nearby `AdvertisingResult`는 기기 검색 advertising 용어여서 광고 SDK로 오인·제거하지 않음 |
| `com.unity.microsoft.gdk@1.7.0`, `.gdk.tools@1.7.0` | 각각 독립 직접 의존성이고 IAP가 끌고 온 패키지는 아님. 앱 코드 Microsoft GDK 사용 없음; package는 Microsoft 플랫폼용이고 Android FPS 범위에 사용 근거 없음. 유일 직렬화 참조는 자동 생성 `Assets/Resources/GDKEditionAutoGen/GDKEdition.asset`의 GdkEditionAsset. 해당 원본을 백업하고 잔여 asset/meta를 함께 정리하는 조건으로 제거 대상 |
| 전이 제거 예상 | 세 직접 패키지 제거 후 `com.unity.microsoft.gdk.discovery`, `com.unity.services.deployment`, `com.unity.services.deployment.api`는 lock 그래프에서 다른 직접 의존성으로 도달되지 않음; 실제 UPM resolve 결과로 확정 |
| 보존 | GooglePlayGames/EDM, Photon Fusion, `com.unity.services.authentication`, 그 의존성 services.core/Newtonsoft, UGUI/AndroidJNI/필수 Unity 모듈, Pipeline |
| 서비스 설정 | UnityConnectSettings의 Purchasing/Ads enabled=0, Ads game ID 빈 값; 이미 비활성. 관련 Unity 기본 설정 블록을 임의 삭제할 필요 없음 |

직렬화 검사는 위 네 패키지(IAP/GDK/GDK Tools/GDK Discovery)의 meta GUID 2,537개와 Assets `.unity/.prefab/.asset/.asmdef/.asmref`의 참조를 비교했습니다. 발견한 교차 참조는 GDK 자동 생성 asset 1개뿐입니다. GDK 제거 시 해당 asset을 남기면 패키지 타입 참조가 끊기므로 함께 처리해야 합니다. 캐시 폴더 직접 삭제로 패키지를 제거하지 않습니다.

적용 단계:

1. Editor 제어권 인계와 현재 컴파일/씬 상태를 확인합니다.
2. 현재 manifest/lock 및 GDK 자동 생성 asset/meta를 프로젝트 밖에 추가 백업하고 hash·git diff를 기록합니다. 다른 담당 변경은 보존합니다.
3. PackageManager Client API로 IAP 제거 요청을 완료시킨 후, GDK의 독립 사용이 없다는 조사 결과에 따라 GDK/GDK Tools도 제거합니다. 비동기 요청을 main thread에서 busy-wait하거나 Editor를 조기 종료하지 않습니다.
4. resolution 성공과 직접/전이 패키지 제거, Authentication/Core/GooglePlayGames/Fusion 보존을 manifest/lock·UPM 결과로 확인합니다.
5. GDK 생성 asset/meta를 백업 보존 후 정리하고 재컴파일·Missing Script/패키지 참조 검사·콘솔 실제 오류 확인을 수행합니다.
6. Login→Lobby 및 네트워크 로비 smoke test로 기능 회귀를 확인합니다. 광고·결제 런타임/직렬화 참조가 남지 않았는지 최종 검색하고 변경 파일·결과를 총괄에 제출합니다.

GDK는 IAP의 전이 의존성이 아니라 별도 Microsoft 플랫폼 기능이라는 점을 구분했습니다. Android/PC 개발 검증 범위에 독립 사용 근거가 없어서 제거안에 포함했으며, Xbox/GameCore용 기본 PlayerSettings·QualitySettings 슬롯은 광고/IAP 코드로 간주하지 않습니다.

### 최종 적용 기록 — 2026-10-07 12:08 KST

- Editor 제어권을 인계받아 PlayMode=false, 원래 빈 Untitled 씬 dirty=false를 확인했습니다.
- 추가 백업: `D:\meee\git\sudden-force-fps-backups\20261007-iap-gdk-removal`. manifest/lock, BillingMode JSON/meta, 파일 hash 및 변경 전 Git/콘솔 기록을 보존했습니다.
- Pipeline `package_remove`가 실제 `UnityEditor.PackageManager.Client.Remove`를 호출하는 소스와 catalog를 확인했습니다. IAP → GDK → GDK Tools 순서로 각 비동기 제거 완료와 재컴파일을 확인했습니다. domain reload 중 일시적인 연결 실패는 요청 상태 파일과 manifest를 확인한 뒤 재접속했으며, 중복 제거 요청은 실행하지 않았습니다.
- manifest에서 `com.unity.purchasing`, `com.unity.microsoft.gdk`, `com.unity.microsoft.gdk.tools`가 제거됐습니다. 다른 직접 의존성의 버전은 변경되지 않았습니다.
- lock에서 위 세 개 및 `com.unity.microsoft.gdk.discovery`, `com.unity.services.deployment`, `com.unity.services.deployment.api` 총 6개가 제거됐습니다. 남은 패키지 버전 변경은 0개이고, lock의 의존성 대상 누락도 0개입니다.
- `Assets/Resources/BillingMode.json`은 내용이 Android GooglePlay 구매 store 선택 설정이고 앱 코드 사용이 없음을 확인했습니다. 원본/meta hash 백업 후 AssetDatabase.DeleteAsset으로 제거했습니다.
- 조사 때 있던 GDKEditionAutoGen은 제어권 인계 시 이미 folder/asset/meta가 존재하지 않았습니다. 새로 생성·복구하지 않았고, 제거 후에도 잔여 참조가 없는 상태를 확인했습니다.
- 광고 SDK 및 광고/IAP 앱 호출·직렬화 패키지 참조가 없고, purchasing/GDK 이름의 manifest/lock 잔여 항목이 없습니다. GooglePlayGames, EDM, Fusion, Authentication/Core/Newtonsoft, UGUI, AndroidJNI, Pipeline을 보존했습니다.
- 최종 recompile_status: completed, failed=false, compilationFailed=false. 정상 smoke를 위해 기존 콘솔 기록을 먼저 저장하고 콘솔을 초기화했습니다.
- Login→Lobby 자동 진입, 실제 frame 1→6 증가 및 이후 frame938 확인. LobbyStatus=Connected, LastLobbyError 빈 값, HasReceivedSessionList=true, NetworkRunner1개, LobbyInfo.IsValid=true, 로드 씬 Missing Script0개를 확인했습니다.
- 새 정상 smoke와 PlayMode 종료 후 실제 Unity 콘솔 errors0입니다. 인계 전 Lobby 실패 로그는 별도 실패 검증 기록으로 보존했습니다. 인계 전 Fusion `OnApplicationQuit` cancellationToken Assert는 이번 정상 종료에서 재현되지 않았습니다.
- 남은 경고: 메모리 부족으로 Profiler frame data를 줄였다는 Unity 경고가 발생했습니다. 다른 애플리케이션을 종료하거나 시스템 설정을 임의 변경하지 않았으며, Android 성능 검증 전에 환경 메모리 여유를 확인해야 합니다.
- PlayMode 종료, autotick=false, 원래 빈 Untitled dirty=false로 복귀했습니다. Unity는 열린 상태로 유지했습니다. Android target34/Gradle/keystore 설정과 네트워크 담당 코드·씬·프리팹은 수정하지 않았습니다.

증거 파일은 백업 폴더의 `gdk-remove-result.json`, `gdk-tools-remove-result.json`, `console-before.json`, `console-before-smoke.json`, `smoke-live-state.json`, `smoke-console-status.json`, `console-final.json`입니다. 실제 Android 빌드·PGS 인증·Play 업로드는 아직 미실행입니다.

## 첫 빌드용 코드·템플릿 수정 결과

Editor는 다른 담당의 검증 소유 상태에서 파일만 편집했습니다. 원본 백업은 `D:\meee\git\sudden-force-fps-backups\20261007-android-prebuild`이며 BuildScript, Android 플러그인 원본/meta와 hash를 보존했습니다. 실제 빌드나 자동 Android Resolver 호출은 하지 않았습니다.

| 변경 파일 | 수정 결과 |
| --- | --- |
| Assets/Scripts/Editor/BuildScript.cs | 기존 APK/AAB public 진입점·출력 형태 유지. reload marker/coroutine/postprocess 종료 제거. 기존 프로젝트 define은 건드리지 않고 APK에 extraScriptingDefines Debug 추가. ledger 검증·code 단조 증가/명시 override, 자격 증명 안전 입력, preflight 실패 중단, BuildReport 검사 및 batch exit0/1 |
| baseProjectTemplate.gradle | 설치된 6000.3.25f1 공식 기본과 byte 동일: AGP9.0.0, tasks.register clean DSL |
| mainTemplate.gradle | 공식 기본의 shared apply, NDKVERSION, Java17, MINSDK/TARGETSDK, DEBUGSYMBOLLEVEL/DEFAULT_CONFIG_SETUP/PACKAGING 등으로 이식. GPGS dependency·resolver marker·기존 ABI 제외 규칙을 AGP packaging.jniLibs DSL로 보존 |
| settingsTemplate.gradle | 구형 PC 절대 경로 제거. settingsDir 조상에서 현재 Assets/GeneratedLocalRepo를 찾고 exported Gradle은 SUDDEN_FORCE_PROJECT_PATH로 source root를 전달 가능. 미발견 시 명확히 실패 |
| AndroidManifest.xml | 기존 Activity/테마/launcher 유지, intent-filter가 있는 UnityPlayerActivity에 exported=true 명시 |
| Assets/Scripts/UI/Login.cs | 별도 QA 변경: StartLogin의 Editor 분기를 UNITY_EDITOR 또는 UNITY_STANDALONE_WIN으로 확장한 최소 diff. PC 개발 검증용이며 Android Google 로그인은 그대로 유지 |

`gradleTemplate.properties`의 AndroidX/Jetifier와 GooglePlayGames 생성 manifest/library는 수정하지 않았습니다. GPGS 좌표 `com.google.games:gpgs-plugin-support:2.0.0`은 현재 plugin version, GeneratedLocalRepo AAR 및 POM의 group/artifact/version과 일치합니다. 새 템플릿의 Unity placeholder 집합은 설치 mainTemplate과 같으며, 문서에 없는 source-root placeholder를 추가하지 않았습니다.

오프라인 검증: dotnet Roslyn을 설치된 Unity Editor/Engine 어셈블리 및 Mono 참조와 연결하여 **BuildScript C# 컴파일 성공, 오류·경고 0개**를 확인했습니다. 이는 Editor 재컴파일·Gradle export·Android APK 빌드 성공이 아닙니다. base template byte 비교, main placeholder 집합 비교, GPGS 좌표/로컬 저장소 존재, 정상 Library/Bee 경로의 조상 검색 및 scoped git diff-check를 확인했습니다. 증거는 `compile-result.txt`, `compile.rsp`, `template-validation.json`입니다.

Windows QA 조건(`UNITY_STANDALONE_WIN`)의 Login.cs도 기존 프로젝트 어셈블리를 참조한 오프라인 컴파일에서 오류0으로 통과했습니다. Unity 직렬화로 채워지는 기존 private SerializeField 3개에 CS0649 경고가 발생했으며 실제 Windows 실행은 아직 검증하지 않았습니다. 근거는 `compile-login-windows-result.txt`입니다. Android 조건의 실제 컴파일은 플랫폼 전환 후 별도로 확인합니다.

제어권 인계 후 남은 실제 검증:

- [ ] Editor 재컴파일 및 Windows Standalone QA 클라이언트의 Login→Lobby 실행
- [ ] Android target API36 설정, 해당 플랫폼 스크립트 컴파일과 안전한 서명 입력 확인
- [ ] EDM1.2.182 resolve 후 템플릿 덮어쓰기/구형 DSL 재삽입 여부 재검토
- [ ] 실제 생성 Gradle에 미치환 placeholder·외부 PC 경로가 없고 GPGS dependency가 해석되는지 확인
- [ ] APK 첫 빌드와 실패 경로의 exit1/menu Editor 유지, code 증가/credentials 복원 검증
- [ ] 최종 merged manifest/서명/native alignment·실기기 로그인·한 판 검증

CI/App Center 경로는 후속 release 작업으로 남겼습니다. package/alias/keystore를 변경하거나 새로운 키를 생성하지 않았습니다.

## 첫 실제 Android 개발 APK 검증 — 2026-10-07

Refs #35, #5, #19. 기존 Windows 두 클라이언트 QA가 끝난 뒤 Android Editor 제어권을 인계받아 수행했습니다. 실제 기기 로그인과 PlayFab 요구사항(#36), 이동·전투·로컬 카메라 완료를 의미하지 않습니다.

| 항목 | 실제 결과 |
| --- | --- |
| Unity / 도구체인 | 6000.3.25f1, IL2CPP, Gradle 9.3.1, AGP 9.0.0, Java 17 |
| 결과 | BuildReport Succeeded, errors 0, warnings 71, 743.5초 |
| APK | `D:\meee\git\sudden-force-fps-backups\20261007-android-first-build\apk-attempt-01\SuddenForceFPS.DebugValidation.apk` |
| 크기 / SHA-256 | 167,127,403 bytes / `349cd92f98fe4b52bb3bb65ae8840e056b42e10a7bf01d60f71a26ad06cb8a39` |
| 패키지 / 버전 | `com.AeDeong.SuddenForceFPS`, 1.0.0, versionCode 3 |
| SDK / ABI | min 25, target/compile 36, arm64-v8a + armeabi-v7a |
| 빌드 | Development + LZ4, Login/Lobby/Room/DesertHouse 4개 씬 |
| 서명 | 기존 사용자 `.android/debug.keystore`, Android Debug RSA 2048, APK v2 검증 성공 |
| 디버그 인증서 SHA-256 | `70:31:D6:89:71:77:B8:17:D7:2E:02:CC:9F:07:B9:14:13:14:FD:4D:93:BB:B3:9F:C9:5D:3B:00:1C:85:09:10` |
| 디버그 인증서 SHA-1 | `6A:6F:43:7F:04:6B:06:0D:E1:4F:A1:CF:3B:4B:FD:AB:47:16:42:C7` |
| 정렬 | ARM64 7개 라이브러리 모두 ELF PT_LOAD 16KB 충족, `zipalign -c -P 16 -v 4` 성공 |

APK 내 14개 `.so`는 모두 압축 저장되어 있습니다. 따라서 압축 엔트리의 ZIP offset 자체는 16KB 배수가 아니며, 비압축 `.so`용 ZIP 정렬 조건은 적용 대상이 없습니다. ARMv7의 libc++/swappy는 ELF 4KB이므로 모든 ABI가 ELF 16KB라고 주장하지 않습니다. ARM64 실제 16KB 기기 실행은 아직 미검증입니다. BuildReport totalSize는 부가 산출물을 포함하므로 APK 파일 크기와 다릅니다.

릴리스 키 암호는 Editor/환경변수 모두 사용할 수 없었습니다. 값을 출력하거나 추측하지 않았습니다. 기존 `src/AeDeong.keystore`/alias를 변경하거나 키를 새로 생성하지 않았으며, 기존 릴리스 `BuildScript`는 유지했습니다. 별도 `AndroidValidationBuild.Queue(새 외부 절대 디렉터리)`를 실제 Editor update에서 실행해 도구체인을 검증했습니다. 종료 시 custom keystore 사용 여부, appBundle, version, versionCode를 원래 값으로 복원했습니다. 디버그 APK의 code3은 릴리스 ledger에 예약하지 않았으며 `BuildInfo/buildinfo.txt=0,0,0`, PlayerSettings code2/version1.0.0을 유지합니다. 기존 Play 최대 code2 기준 다음 첫 릴리스 code3은 여전히 가능합니다.

이 APK는 기존 업로드 키 또는 Play App Signing 인증서로 서명한 배포 산출물이 아닙니다. 해당 디버그 인증서의 PGS OAuth 등록·테스터 권한, 실제 기기 인증 및 한 판 검증은 미실행입니다. 릴리스 APK/AAB 및 Play 업로드에는 기존 업로드 키의 안전한 암호 입력이 필요합니다.

### Resolve와 재현 제약

- EDM1.2.182 Force Resolve 후 `ResolveSync(false)=true`를 확인했습니다. SDK/EDM 원본이나 패키지를 추가 업그레이드하지 않았습니다.
- Resolve는 구형 `packagingOptions`와 현재 PC 절대 maven URL을 다시 삽입합니다. `mainTemplate.gradle`의 exclusion marker를 최상위 단일 블록으로 정리하고 modern `packaging.jniLibs` 및 `settingsTemplate.gradle`의 조상 검색 코드를 복원한 뒤 빌드했습니다. **Force Resolve를 다시 실행하면 두 템플릿을 재검토·정리해야 합니다.**
- 실제 `Library/Bee/Android/Prj/IL2CPP/Gradle`에서 AGP9, Java17, compile/target36, code3, GPGS2.0.0, portable repository 코드를 확인했습니다. 미치환 placeholder·과거 외부 PC 경로·legacy packagingOptions는 각각 0건입니다.
- 신규 helper가 MonoScript로 import됐지만 compile graph에 반영되지 않은 상태는 AssetDatabase 임시 rename/원위치 복구로 해결했습니다. GUID `90a395718735c06439051c5c92aa883f` 유지, 실제 Editor assembly 로드·빌드 실행을 확인했습니다. delayCall 대기는 단발 EditorApplication.update callback으로 변경했습니다.

### 보존·복원 정책과 증거

시작 시 `20261007-android-first-build`에 ProjectSettings 전체, Android plugins 전체, UserSettings, Packages 및 ledger 원본/hash를 보존했습니다. Assets/Settings의 QA 직후 상태는 `sudden-force-fps-validation/20261007-stage12-01/source-after.patch`에서 재구성해 비교했습니다. URP 5개 파일은 QA 직후와 텍스트 동일하며 Android 추가 delta가 없습니다. 기존 QA 자동 저장를 전체 rollback하지 않았습니다.

제품에 필요한 API36 및 Android `URP_COMPATIBILITY_MODE`는 유지합니다. 기존 Android define을 보존했고 Fusion2.0.13이 자동 추가한 버전 define도 유지합니다. 기존 RenderGraph/compatibility 설정은 전환하지 않았습니다. Android 빌드가 추가한 batching static1/dynamic0과 기존 Standalone batching1/0을 보존합니다. 원래 Win64 대상·빈 clean 씬·PlayMode=false·autotick=false로 복원 후 인계합니다.

경고 71개는 BuildReport 기록에 남습니다. 주로 GPGS/UniRx 등 기존 플러그인의 deprecated API 경고이며, 별도로 URP Compatibility Mode의 deprecation 경고가 있습니다. TMP Units Per EM160 직렬화 안내 로그가 발생했지만 최종 font Git 변경은 없습니다. 실기기 성능과 메모리 검증은 남아 있습니다.

주요 증거: `apk-attempt-01/android-validation-build.json`, `apk-signature.txt`, `apk-badging.txt`, `apk-zipalign.txt`, `apk-native-inspection.json`, `generated-gradle-evidence/audit.json`, `settings-after-build.json`, `projectsettings-vs-handoff.patch`, `settings-delta-vs-qa.json`. APK나 `.utmp` Gradle scratch 파일을 소스 커밋에 포함하지 않습니다.


## 최신 전투·로그인·복귀 포함 개발 APK (2026-10-07)

기준 HEAD `ea29fe6406ecf82ff726861eea200a800200478c`에서 최신 전투/경기, 로그인 재시도 및 persistent 로비 복귀를 포함한 Android Development APK를 빌드했습니다. 첫 APK와 다른 산출물이며 Google Play 배포용 서명 AAB가 아닙니다.

- 파일: `D:\meee\git\sudden-force-fps-backups\20261007-android-latest-ea29fe6-01\apk-attempt-01\SuddenForceFPS.DebugValidation.apk`
- 크기 168,781,787 bytes, SHA256 `52852c6c2386ca4d3342eeeee39831d1c4c03dff0b464786cc450595f4637ada`.
- Unity 6000.3.25f1/IL2CPP/Development/LZ4, BuildReport Succeeded/오류0/경고71/423.900초.
- 패키지 `com.AeDeong.SuddenForceFPS`, 버전 1.0.0/code3, 최소 API25/대상·컴파일 API36, ARMv7+ARM64, 활성 씬4개.
- apksigner verify/v2, merged manifest 및 UnityPlayerActivity exported=true, zipalign 검사 통과. Android Debug SHA1 `6A:6F:43:7F:04:6B:06:0D:E1:4F:A1:CF:3B:4B:FD:AB:47:16:42:C7`.
- ARM64 라이브러리 7개의 ELF PT_LOAD 16KB 정렬 확인. 네이티브 라이브러리14개는 압축 저장되어 비압축 ZIP offset 정렬 조건과 구분합니다. 실제16KB 기기 실행은 미검증입니다.
- 생성 Gradle AGP9.0.0/Gradle9.3.1/Java17/API36 확인. Force Resolve 없이 성공했고 구형 packagingOptions·미치환 placeholder·기존 PC 절대 maven 경로는 발견되지 않았습니다.
- 종료 후 Win64/Play 종료/빈 깨끗한 씬/자동 tick 중지, 기존 ProjectSettings·defines·버전·서명 설정·ledger·URP 변경 및 루트 파일 정확 복원. 시작3895파일 SHA 변경/추가/누락0, 기존 dirty patch 동일.

근거는 같은 외부 폴더의 `apk-verification.json`, `apk-native-inspection.json`, `generated-gradle-audit.json`, `preservation-final.json` 및 BuildReport에 보존했습니다. 전환 중 Pipeline 조회 timeout1건은 제품 컴파일/빌드 오류와 구분했습니다. 실기기 설치·Google 계정 인증·모바일 조작·한 판·백그라운드 및 실제 네트워크 종료 회귀는 미검증입니다. debug code3은 Play 업로드용 버전 코드 예약을 뜻하지 않습니다.


## PGS 인증 연결 재확인 (2026-10-08)

기존 Chrome 로그인 탭에서 Play Console을 읽기 전용으로 확인했습니다. PGS 프로젝트 APP_ID `311409806449`는 Android manifest의 값과 일치합니다. 출시된 Android credential3개는 모두 `com.AeDeong.SuddenForceFPS`를 사용합니다. 확인한 연결 인증서 SHA1은 `14:98:8C:4A:2E:B1:4B:56:A9:9D:9F:A7:02:EA:C8:46:AE:12:A4:63`, 기존 업로드 키 `E9:56:B8:AD:10:19:A6:A2:E3:B8:89:B1:21:C1:EF:0E:72:74:18:E0`, Play 앱 서명 `CF:E6:15:DA:6A:91:0B:3A:78:51:D2:29:EA:10:B2:66:BF:00:9A:53`입니다.

현재 최신 개발 APK의 Debug SHA1 `6A:6F:43:7F:04:6B:06:0D:E1:4F:A1:CF:3B:4B:FD:AB:47:16:42:C7`와 일치하는 연결 credential은 이3개 중 없습니다. 이는 설정 선행조건 불일치이며 실제 기기 인증 실패를 수행했다는 뜻은 아닙니다. 미연결 Cloud OAuth의 존재 여부는 아직 별도로 확인하지 않았습니다. Debug 인증을 연결하거나 기존 승인된 키로 새 검증본을 서명하는 경로 선택이 필요합니다.

PGS는 출시된 속성/credential이며 개별 테스터2개와 내부 테스트 트랙 연결을 확인했습니다. 메일 주소는 기록하지 않습니다. 실제 시험 계정의 테스터 포함 여부는 미확인입니다. PGS 출시 상태와 기존 앱의 초안·프로덕션 비활성 상태는 별개입니다. 이번 조회에서 외부 설정 저장·OAuth 생성·PGS 게시를 하지 않았습니다.

정확한 Unity6000.3.25f1 번들 adb의 `devices -l` 결과는 device0/unauthorized0/offline0입니다. 기기 정보 조회·앱 설치·실행·권한 변경은 수행하지 않았습니다.
