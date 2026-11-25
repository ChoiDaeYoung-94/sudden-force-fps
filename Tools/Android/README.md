# Android 검증 도구

## 기존 앱 데이터를 보존하는 설치 경로

기기에 앱이 설치되어 있으면 먼저 패키지 이름, 버전 코드, APK 서명 인증서를 확인합니다. 아래 표는 Sudden Force에서 사용하는 설치 경로입니다.

| 설치하려는 파일 또는 경로 | 패키지 | 기존 데이터가 있는 기기에서 확인할 점 |
| --- | --- | --- |
| Google Play 내부 테스트 트랙 | `com.AeDeong.SuddenForceFPS` | Play가 전달하는 APK는 Play 앱 서명 인증서를 사용합니다. 등록 테스터가 참여 링크를 열고 같은 계정의 Play 스토어에서 설치 또는 업데이트합니다. |
| 일반판 `LocalQA.apk` | `com.AeDeong.SuddenForceFPS` | 로컬 QA 인증서로 서명합니다. 기존 일반판과 패키지가 같으므로 서명과 버전이 호환되는 전용 기기에서 사용합니다. |
| 수동 플레이용 `Playtest.apk` | `com.AeDeong.SuddenForceFPS.sessioncheck` | 일반판과 함께 설치할 수 있습니다. 기존 수동판을 갱신할 때는 이 패키지의 인증서와 버전을 비교합니다. |

1. Android SDK의 `adb -s <기기> shell pm path <패키지>`에서 `/base.apk`로 끝나는 행을 찾습니다. 분할 APK 설치본은 여러 행을 반환합니다. `package:` 접두어를 뺀 경로를 `adb -s <기기> pull <base.apk 경로> <새 임시 폴더>/installed.apk`에 사용합니다.
2. 설치본과 후보 APK 각각에 `aapt2 dump badging <파일>`과 `apksigner verify --verbose --print-certs <파일>`을 실행합니다. 패키지, `versionCode`, 서명 검증 성공 및 서명자 SHA-256을 확인합니다. Play가 서명한 전달 APK는 Console의 **앱 서명 인증서**와 비교하고, 업로드하는 AAB는 **업로드 인증서**와 대조합니다. AAB를 로컬 QA 키로 변환한 APK에는 그 QA 인증서가 표시됩니다.
3. 이 프로젝트의 기존 단일 서명 설치본과 인증서가 다르고 유효한 서명 키 교체 이력도 확인되지 않으면 기존 앱을 보존합니다. 수동 플레이는 별도 패키지를 사용합니다. Play 업데이트 검증은 이전 Play 설치본을 보유한 기기에서 더 높은 버전으로 진행합니다. 새로 설치한 경우에는 신규 설치로 기록합니다.
4. 테스터 검수에서는 참여 여부, Play에서 설치된 버전, Google Play Games 로그인 성공과 로비 진입을 각각 확인합니다. 기기에 APK 파일을 직접 설치한 시험은 전달 경로에 맞게 별도로 기록합니다.

업로드 키를 재설정해도 Google Play가 사용자에게 배포하는 앱 서명 키는 유지됩니다. [Android 앱 서명](https://developer.android.com/studio/publish/app-signing) · [apksigner](https://developer.android.com/tools/apksigner) · [Google Play 내부 테스트 설정](https://support.google.com/googleplay/android-developer/answer/9845334)

## Google Play Games 권한

인증에는 Google Play Games를 사용하고 온라인 경기는 Photon Cloud에 연결합니다. Google Play Games 설정에서 Nearby Connections 서비스 ID를 비워 두면 플러그인이 생성하는 매니페스트에서 주변 기기 검색 권한이 빠집니다.

릴리스 APK는 `aapt2 dump permissions game.apk`, AAB는 `bundletool dump manifest --module=base --bundle=game.aab`로 확인합니다. 두 매니페스트에 `INTERNET`과 `ACCESS_NETWORK_STATE`가 있고, Nearby용 `BLUETOOTH`, `BLUETOOTH_ADMIN`, `ACCESS_WIFI_STATE`, `CHANGE_WIFI_STATE`, `ACCESS_COARSE_LOCATION` 선언이 없는지 확인합니다. 로그인용 `com.google.android.gms.games.APP_ID`와 `NativeBridgeActivity`는 유지합니다.

## APK·AAB의 IL2CPP 문자열 확인

Unity 6000.3의 IL2CPP 메타데이터 버전 39에서 APK와 AAB의 메타데이터가 같은지, 지정한 문자열이 완전한 레코드로 들어갔는지 검사합니다. 두 산출물을 만든 뒤 로그인 안내 등 사용자에게 보이는 문구를 확인할 때 사용합니다.

```powershell
python Tools/Android/verify_packed_literals.py game.apk game.aab --require 'Google Play Games sign-in did not finish. Select Retry to sign in.' --forbid 'Google sign-in was canceled. Please retry.'
```

`--require`와 `--forbid`는 반복할 수 있습니다. 출력에는 인수 순서대로 일치 개수가 표시되며, 조건을 만족하면 종료 코드 0을 반환합니다. 실제 기기에서 어느 안내가 표시되는지는 해당 로그인 경로를 실행해 확인합니다.

### 일반판과 수동 플레이 APK 구별

일반 APK와 수동 플레이 APK에는 같은 버전 번호를 사용할 수 있습니다. 먼저 APK와 AAB에서 패키지 이름을 읽습니다.

```powershell
aapt2 dump badging game.apk | Select-String '^package:'
bundletool dump manifest --module=base --bundle=game.aab --xpath='/manifest/@package'
```

일반판은 두 결과 모두 `com.AeDeong.SuddenForceFPS`여야 합니다. 수동 플레이 APK의 `com.AeDeong.SuddenForceFPS.sessioncheck`는 기기에 함께 설치해 닉네임부터 테스트하는 별도 앱입니다. 스토어 제출에는 일반판 AAB를 사용합니다.

일반 APK와 AAB를 한 쌍으로 검사해 Google Play Games 재시도 문구가 들어 있고 수동 플레이용 패키지 문자열이 빠졌는지 확인합니다.

```powershell
python Tools/Android/verify_packed_literals.py game.apk game.aab --require 'Google Play Games sign-in did not finish. Select Retry to sign in.' --forbid 'com.AeDeong.SuddenForceFPS.sessioncheck'
```

수동 플레이 APK와 일반 AAB를 섞으면 메타데이터 불일치로 종료 코드 1이 반환됩니다. 통과한 일반판도 업로드 서명과 실제 Google 로그인을 각각 확인합니다.

## ARM64 ELF LOAD·RELRO 정렬 검사

`verify_native_alignment.py`는 Python3.12 이상에서 AAB의 모든 모듈 또는 APK에 들어 있는 `lib/arm64-v8a/*.so`를 추출 없이 읽습니다. LOAD alignment·주소/파일 오프셋 congruence와 GNU_RELRO를 검사합니다. RELRO 끝 주소가16KB 정렬되거나, RELRO가 자체 writable LOAD의 전체 VA/메모리 구간과 일치하고 보호 페이지가 다른 writable LOAD의 페이지와 겹치지 않을 때 통과합니다. 모든 프로그램 헤더를 모은 뒤 판정하므로 헤더 순서에 의존하지 않습니다. 원본과 기존 보고서를 덮어쓰지 않고 Unity·Gradle·서명 도구를 실행하지 않습니다.

```powershell
python -B Tools/Android/verify_native_alignment.py '<검사할 AAB 또는 APK>' --output '<새 외부 JSON 보고서>'
```

종료0은 선택한 ARM64 ELF 검사 통과,1은 잘못된 ELF 또는 정렬 실패,2는 ZIP/입력/보고서 처리 오류입니다. RELRO가 없는 라이브러리는 정렬 실패로 보지 않지만 보안 강화가 됐다는 뜻도 아닙니다. ZIP CRC는 선택한 native entry에 대해 검사하며 전체 ZIP 엔트리 CRC 검사가 아닙니다. ELF64 little-endian ARM64 ET_DYN만 지원하고, 라이브러리128개/각256MiB 한도를 넘으면 실패합니다.

**APK ZIP alignment·BundleConfig·서명·기기 실행은 검사하지 않습니다.** ELF 정렬 통과만으로 전체16KB 호환을 확정하지 않습니다. [Android 공식 LOAD·RELRO 조건](https://developer.android.com/guide/practices/page-sizes#relro)과 실제16KB 환경 검증을 함께 적용합니다.

### 16KB Android 에뮬레이터에서 AAB 분할 설치

전용 AVD에 실제 AAB에서 생성한 APK 세트를 설치해 패키지 선택과 실행을 확인합니다. Windows x64에서는 Android 16의 `system-images;android-36;google_apis_ps16k;x86_64` 이미지를 사용할 수 있습니다. Device Manager에서 전용 AVD를 만들고, SDK의 emulator로 설정한 이름을 지정해 스냅샷을 사용하지 않고 실행합니다(예: `emulator -avd Sff16k -no-snapshot`). PowerShell에서 Unity가 사용하는 SDK의 `platform-tools/adb.exe`, OpenJDK의 `bin/java.exe`, bundletool의 실행용 JAR 경로를 지정합니다. 연결 목록에서 전용 AVD의 시리얼을 골라 모든 기기 작업에 사용합니다.

```powershell
$adb = (Get-Item -LiteralPath (Read-Host 'adb.exe 경로')).FullName
$java = (Get-Item -LiteralPath (Read-Host 'java.exe 경로')).FullName
$bundletool = (Get-Item -LiteralPath (Read-Host 'bundletool-all JAR 경로')).FullName
& $adb devices -l
$serial = Read-Host '전용 AVD 시리얼'
& $adb -s $serial emu avd name
& $adb -s $serial shell getconf PAGE_SIZE
& $adb -s $serial shell getprop ro.product.cpu.abilist
& $adb -s $serial shell getprop ro.dalvik.vm.native.bridge
```

본인이 띄운 전용 AVD 이름과 페이지 크기 `16384`를 확인합니다. ARM64 전용 게임을 x86_64 이미지에서 실행하려면 ABI 목록에 `arm64-v8a`가 있고 ARM 변환기가 제공되어야 합니다. 호환 모드에 의한 통과를 구별하려면 이 AVD의 현재 설정을 기록하고, 아래 두 값을 적용한 뒤 각각 `false`, `true`로 읽히는지 확인합니다. 이 속성은 해당 Android 환경의 다른 앱에도 적용됩니다. 시험이 끝나면 전용 AVD를 스냅샷 저장 없이 종료하고 다음 사용 시 콜드 부팅합니다.

```powershell
& $adb -s $serial shell getprop bionic.linker.16kb.app_compat.enabled
& $adb -s $serial shell getprop pm.16kb.app_compat.disabled
& $adb -s $serial shell setprop bionic.linker.16kb.app_compat.enabled false
& $adb -s $serial shell setprop pm.16kb.app_compat.disabled true
& $adb -s $serial shell getprop bionic.linker.16kb.app_compat.enabled
& $adb -s $serial shell getprop pm.16kb.app_compat.disabled
```

검증 대상 AAB에서 `bundletool build-apks`의 기본 모드로 만든 서명된 `game.apks`를 준비합니다. 로컬 설치에는 로컬 QA 서명을 사용할 수 있습니다. 새 검증 디렉터리에서 다음 명령을 실행합니다. 서명이 달라 기존 앱을 업데이트할 수 없으면 기존 데이터를 보존하고 새 전용 AVD를 사용합니다.

```powershell
& $java -jar $bundletool get-device-spec "--adb=$adb" "--device-id=$serial" --output=target.json
if ($LASTEXITCODE) { throw '기기 사양 수집 실패' }
& $java -jar $bundletool extract-apks --apks=game.apks --device-spec=target.json --output-dir=parts
if ($LASTEXITCODE) { throw '기기용 분할 APK 추출 실패' }
$parts = @(Get-ChildItem -LiteralPath parts -Recurse -Filter '*.apk' -File)
if ($parts.Count -lt 2) { throw '기본 APK와 기기용 분할 APK를 확인하세요.' }
$paths = @($parts.FullName)
& $adb -s $serial install-multiple -r @paths
if ($LASTEXITCODE) { throw '분할 APK 설치 실패' }
$package = 'com.AeDeong.SuddenForceFPS'
& $adb -s $serial shell pm path $package
& $adb -s $serial shell dumpsys package $package | Select-String 'versionCode=|primaryCpuAbi='
& $adb -s $serial shell am start -W -n "$package/com.unity3d.player.UnityPlayerActivity"
```

설치된 버전과 ABI를 확인하고, `pm path`에 나오는 각 APK를 `& $adb -s $serial pull`로 별도 폴더에 복사합니다. 설치본과 추출본의 파일명은 달라질 수 있으므로 양쪽 SHA-256 목록을 정렬해 개수와 값이 일치하는지 비교합니다. 앱이 실제로 인트로를 지나 로그인 화면에 도달하는지 확인하고, 서명·계정 조건에 따라 재시도 안내 또는 로그인 성공을 기록합니다. x86_64 AVD와 ARM64 변환기를 썼다면 결과에 함께 적습니다. 실제 ARM64 16KB 기기와 Play 설치 결과는 각각 따로 기록합니다. [Android 16KB 테스트](https://developer.android.com/guide/practices/page-sizes#test) · [bundletool 기기별 APK 추출](https://developer.android.com/tools/bundletool#extract-device-specific)

### 첫 경기에서 화면이 멈추거나 연결이 끊길 때

같은 APK와 방장 기기로 비교합니다. 첫 경기 전에 설치 상태(신규 설치·업데이트·기존 설치 재실행), 가상 기기의 CPU·메모리, 호스트 PC의 빌드 실행 여부를 기록합니다. 그래픽 가속을 지원하는 PC에서는 전용 AVD를 `-gpu host`로 열어 첫 경기를 확인할 수 있습니다. 실제로 선택된 렌더러는 아래 명령의 `GLES:` 행에서 확인합니다. 소프트웨어 렌더러를 비교할 때는 같은 시스템 이미지와 설정을 가진 별도 시험 AVD를 사용합니다. [Android Emulator 그래픽 가속](https://developer.android.com/studio/run/emulator-acceleration)

```powershell
& $adb -s $serial shell dumpsys SurfaceFlinger | Select-String 'GLES:'
```

1. 수동 플레이 APK로 방에 참가해 READY를 누릅니다. 방장의 START부터 양쪽 HP·탄약·경기 시간이 표시될 때까지 관측합니다. 방장의 MENU → QA · LAST 10 SEC를 누른 다음 양쪽 결과와 로비 복귀를 확인하면 5분을 모두 기다릴 필요가 없습니다.
2. 앱이 사라지면 해당 패키지의 Android 종료 사유를 확인합니다. 앱이 살아 있으면 양쪽 화면과 Photon 종료 사유를 확인합니다. 프레임이 늦은 시각, 게임의 Update 간격, 연결이 끊긴 시각을 각각 남기면 재실행 때 비교할 수 있습니다. [Android 프로세스 종료 사유](https://developer.android.com/reference/android/app/ApplicationExitInfo)
3. 첫 경기 실패 후 같은 프로세스의 새 방에서 성공하면 두 결과를 따로 기록합니다. 코드64 검증에서는 같은 이미지·가상 CPU 2개·메모리 3GiB에서 첫 경기 중 관측한 최대 표시 프레임 간격이 호스트 GPU에서 0.72초, SwiftShader에서 8.52초였습니다. 두 경기 모두 시간 만료와 로비 복귀를 마쳤습니다. SwiftShader 쪽은 첫 앱 실행이 Android의 LOW_MEMORY로 종료되어 다시 실행한 뒤 첫 경기를 치렀습니다. 이 수치는 해당 PC에서 각 한 번 측정한 값입니다.

### 독립 LOAD 예외 누락 정정 (2026-10-21, #142)

보고서 schemaVersion2는 각 LOAD의 VA/메모리 크기/flags와 RELRO의 alignmentBasis·ownLoadIndex·overlappingWritableLoadIndices를 기록합니다. 기존 schemaVersion1은 끝 주소의 나머지만 검사해 [공식 문서의 독립 LOAD 예외](https://developer.android.com/guide/practices/page-sizes#relro)를 누락했습니다. 부분 RELRO나 다른 writable LOAD가 같은 보호 페이지를 사용하는 경우에는 이 예외를 허용하지 않습니다. 예외 통과가 LOAD 정렬 실패를 무시하지도 않습니다.

동일 코드27 무서명 AAB SHA256 `4FD0420B1DA57A426A638A0A143386C894F65B7A352AE738DA31E5509507EC16`을 새로 읽은 결과, ARM64 라이브러리7개 모두 LOAD/RELRO 제한 정적 검사를 통과했습니다. Burst/NanoSockets2개는 끝 주소 정렬, libc++_shared·IL2CPP·main·Swappy·Unity5개는 자체 LOAD 전체와 일치하고 다른 writable LOAD의 보호 페이지 겹침이 없어 예외 통과입니다. 원본 파일은 변경하지 않았습니다. 이전 도구의 종료1/실패5 보고서는 보존하며 현재 플랫폼 실패5로 해석하지 않습니다. 실제16KB 실행·APK ZIP 정렬·서명·배포 검증은 별도입니다.

### RELRO 분리 재링크 후보 (2026-10-20, #140)

외부 검증 폴더의 기존 link 입력을 사용해 common-page-size=16384·linker threads1을 추가하고 출력만 별도 경로로 바꿨습니다. IL2CPP는 기존 response file, Unity는 실제 Bee DAG의 엔진 link node, Swappy는 CMake Ninja의 link 명령을 대조했습니다. 전체 C++ 소스를 새로 컴파일한 결과가 아닙니다.

| 외부 후보 | 크기(B) | SHA256 | LOAD/RELRO | 원본과 동일한 defined dynamic export 수 |
| --- | ---: | --- | --- | ---: |
| libil2cpp.so | 972878432 | `8108D70064242B13C1ADB644E2A5AC46BC38FE1B606F88F66D2F8C3F0F18C1E5` | 4개/끝 정렬 통과 | 2416 |
| libunity.so | 32456552 | `9A71F9EF5CCE247F0E8B4727B24F1CCCF2F5463F02CADD6307E72F525C462AA9` | 4개/끝 정렬 통과 | 7 |
| libswappywrapper.so | 4693632 | `23F469E290E72EB2953E268954ADE0C42B7320EB78E23712EF6C6932187F4E20` | 3개/끝 정렬 통과 | 497 |

후보는 심볼 제거 전 파일이며 특히 IL2CPP 약973MB는 배포용 교체 파일이 아닙니다. 이 표는 standalone ELF의 프로그램 헤더·명시된 동적 심볼 이름/타입/binding/visibility 검사입니다. AAB/APK 전용 CLI에 raw SO를 전달해 통과시킨 것이 아닙니다. 큰 후보는 전체 stream SHA와 header/dynamic metadata seek를 사용했고, 원본 AAB·Gradle 출력·공급자 파일·소스/설정은 수정하지 않았습니다. 실행 중인 다른 프로젝트나 키/암호에 접근하지 않았습니다.

각 실행은 소유 JobObject 안의 stdin gate 이후에만 linker를 시작하고 종료/예외 시 해당 job을 닫았습니다. 선택19개 원본 핀을 전후 대조했지만 모든 object·archive·toolchain 전이 입력을 고정한 것은 아닙니다. 분리 후보의 정렬/심볼 대조는 ABI·실제 실행·새 source build·패키징·서명·16KB 기기 통과가 아닙니다. 이는10월20일의 실험 이력이며10월21일 공식 예외 재평가에 따라 원본5개를 필수 교체 대상으로 간주하지 않습니다. 후보를 제품에 적용하지 않으며 [#140](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/140)의 판정 정정과 후속 범위를 따릅니다. 후보 SO·일회성 절대 경로 계획·원문 명령/환경은 저장소에 반입하지 않습니다.

## Gradle export 사전 검사

`verify_gradle_export.py`는 Unity가 이미 생성한 Android Gradle 프로젝트를 **offline bundleRelease dry-run**으로 검사합니다. Python3.12 이상, Gradle9.3.1/AGP9.0.0을 대상으로 합니다. Unity Editor 빌드·Gradle 실제 패키징·서명·버전 예약 기능은 없습니다.

## 사용

현재 export의 package/versionName/versionCode를 먼저 확인하고 그대로 전달합니다. 배포된 버전이나 원장 값으로 강제 변경하지 않습니다. 예를 들어 코드18 배포 후에도 현재 export의 원래 값은 코드15이며 원장은194,10,18입니다. 아래 자리표시자는 실제 경로로 바꾸고 보고서 폴더는 매번 새 외부 경로를 사용합니다.

```powershell
python Tools/Android/verify_gradle_export.py `
  --project '<Unity 프로젝트>' `
  --export '<Gradle export>' `
  --java-home '<OpenJDK>' `
  --gradle-home '<Unity AndroidPlayer/Tools/gradle>' `
  --gradle-user-home '<기존 의존성 캐시>' `
  --sdk '<Android SDK>' --ndk '<Android NDK>' `
  --android-user-home '<외부 Android 사용자 폴더>' `
  --cmake-staging '<export에 설정된 실제 CMake staging>' `
  --expected-code 15 --expected-name '1.0.194' `
  --report-directory '<새 외부 검증 폴더>'
```

캐시 의존성이 없으면 offline 상태에서 실패합니다. 다운로드나 SDK 설치를 자동으로 시도하지 않습니다. 연결된 Unity가 import/compile/build 중이면 먼저 완료를 기다려야 입력 보존 검사를 정확하게 판단할 수 있습니다. 이 도구는 Editor를 시작하거나 중지하지 않습니다.

## 확인 범위

- 실행 중인 Gradle JVM의 C1/tiered 활성·명시적4GiB/실제 힙 상한·processor1·worker1·parallel=false와 dryRun=true.
- 실제 Gradle/AGP 버전, 패키지·기존 DSL 및 최종 release output 버전. 버전 대입이나 원장 예약 없음.
- 메모리 내 release signingConfig 해제, signing flags와 정확한 무서명 finalizer의 signing data/files/code transparency 부재. AGP 내부 finalizer 검사는 위 버전에 한정하며 모양이 달라지면 실패합니다.
- 요청 작업 고정, 알려지지 않은 signing task 차단, 실제 task 실행 감지 시 즉시 실패, CMake staging 루트 대조.
- 실행 전후 Assets/Packages/ProjectSettings/BuildInfo·export 입력·export 내부의 기존 AAB 해시 보존. 외부 배포 폴더의 AAB는 별도 SHA 대조 대상입니다. 키 파일은 열거나 해시하지 않습니다. Gradle root·launcher·unityLibrary 및 unityLibrary 바로 아래의 build.gradle이 있는 모듈에서만 build/.gradle/.cxx 출력 폴더를 제외합니다. 소스 내부의 IL2CPP/build/deploy 실행 도구와 기존 jniLibs는 이름에 관계없이 검사합니다. 이 구조 밖의 사용자 모듈 출력은 보수적으로 입력에 포함돼 변경 시 실패할 수 있습니다.
- 상속 JVM 옵션·Gradle project 환경 속성·사용자/설치 init-script 충돌은 시작 전에 차단합니다. 비밀번호/token/secret 환경값 및 프로젝트의 알려진 서명 암호 환경값은 자식에 전달하지 않습니다.

Gradle `--dry-run`은 **설정 단계 코드를 실행하고 Gradle 캐시를 바꿀 수 있습니다**. 신뢰하는 프로젝트·기존 export에만 사용하며 전체 파일 시스템 읽기 전용 도구로 취급하지 않습니다. 캐시·SDK·JDK 변경을 되돌리거나 프로젝트를 자동 복구하지 않습니다. 입력이 바뀌면 원인을 확인한 뒤 새 검증 폴더로 다시 실행합니다.

CMake 검사는 설정된 staging 루트만 대조합니다. 기존 Ninja 내용·캐시 참조 경로·새 네이티브 컴파일의 재현성을 검증하지 않습니다.

raw stdout/stderr는 저장·출력하지 않습니다. 외부 보고서는 allowlist JSON 이벤트·입력 해시·결과만 포함하며 계정 환경 전체나 signing DSL을 덤프하지 않습니다. 실패하면 exit code와 마지막 안전 이벤트로 차단 단계를 확인합니다. 결과 `passed=true`는 이 사전 검사 통과이며 AAB 생성·clean Unity 빌드 성공·서명 검증·기기 실행을 의미하지 않습니다.

## 도구 검증

```powershell
python -m unittest discover -s Tools/Android -p 'test_*.py' -v
```

실제 Gradle JVM과 AGP 검증은 별도 현장 dry-run으로 확인합니다. 단위 검사의 가상 이벤트 성공을 실제 Gradle 통과로 대체하지 않습니다.

공식 참고: [Gradle dry-run](https://docs.gradle.org/9.3.1/userguide/command_line_interface.html#sec:command_line_execution_options), [Unity Gradle export](https://docs.unity3d.com/6000.3/Documentation/Manual/android-export-process.html), [Gradle JVM 구성](https://docs.gradle.org/9.3.1/userguide/build_environment.html#sec:configuring_jvm_memory).

## PR용 Python 단위 검사

`.github/workflows/android-export-verifier.yml`은 `Tools/Android/**` 또는 해당 workflow가 바뀌는 PR과 main push, 수동 실행에서 Python3.12 단위 검사만 수행합니다. GitHub-hosted Windows runner를 사용하며 checkout/setup-python은 검토된 공식 commit SHA로 고정합니다.

```powershell
python -B -m unittest discover -s Tools/Android -p 'test_*.py' -v
```

Gradle 검사14개와 ELF/ZIP 검사31개는 임시 fixture와 mocked Gradle 이벤트로 기대값·실패 거부·입력 보존 로직을 확인합니다. pip 설치나 custom secret, Unity license, SDK/JDK·Gradle 실행이 필요하지 않습니다. 실제 Gradle/AGP·init script, Unity 빌드·AAB·서명·배포·기기 실행은 CI 검증 범위에 포함되지 않습니다.

실제 GitHub Actions 실행 상태와 로그는 workflow를 추가한 PR의 checks 결과에서 확인합니다. 로컬 unittest 결과와 GitHub 실행 결과는 별도로 기록합니다. CI 실행이 성공해도 기존 export의 현장 dry-run 및 실제 Android 산출물 검증은 각각 필요합니다.

## Unity 6 구형 URP 에셋 형식 정리 (#234)

별도 샘플 에셋 `Assets/ThirdPartyAssets/Objects/LowPolyFPSLite/Scenes/Settings/URP_FPSLite.asset`의 version11/previous11을 설치 URP17.3의 정상 생성 형식13/13으로 갱신했습니다. 기존104개 공통 필드·renderer 참조는 그대로이며, 현재 Graphics/Quality의 기본 파이프라인은 이 에셋을 사용하지 않습니다. 새 기본 기능 필드와 제거된 구형 필드는 설치 migration/initializer 및 실제 Unity 생성 바이트와 대조했습니다.

GUI의 기존 loaded13/previous12·clean 관측을 SaveAssetIfDirty 성공으로 처리하지 않았습니다. 별도 import에서 검토한 정확한 Unity 생성 파일1개를 가져와 native13/13·clean과 GUID/localID를 확인했습니다. 글로벌 Save/Refresh·private migration 호출·version/dirty 수동 setter는 사용하지 않았고, 공개3947개 중 다른3946개는 raw exact였습니다. Unity가 생성한 빈 YAML값의 후행 공백을 보존하기 위해 `.gitattributes`는 이 파일 한 경로의 whitespace 예외만 추가하며 text/eol 규칙이나 전역 Git 설정은 바꾸지 않습니다.

제품 커밋538b245의 별도 Android 검증 공간을 새 Editor 프로세스로 재시작해13/13·clean/compilationFailed=false·DOTNET1/BEE1·Graphics Null을 확인했습니다. 정상 native handle ExitCode0 뒤에도 대상 파일4730B/SHA8BC37FA549ADD38C68DD189885F301A0A258B3674972668DB4C412627AB80B9F는 raw exact였습니다. GUI의4587B/LF와 체크아웃4730B/CRLF는 LF 정규화 본문이 같습니다. 다른 종료 변화는 DOTweenSettings의 줄바꿈1건뿐이었고 전후 보존 후 닫힌 상태에서 자기 변화만 복원해 fresh 공개3947개 일치를 확인했습니다.

이는 해당 legacy 변환의 반복 제거 표본입니다. 새 Library/clean checkout 전체, 모든 Editor 종료 무변경, 현재 기본 렌더의 시각/성능이나 새 AAB·서명·기기 검증으로 확대하지 않습니다. 근거는 외부 `20261025-work/urp234-*`와 `unity6-serialization-adoption01/`에 보존합니다.

### Android 방장 경기에서 마지막 10초로 이동

별도 수동 플레이 APK를 만들 때 `BuildPlayerOptions.extraScriptingDefines`에 `SFF_LOCAL_PLAYTEST`를 추가합니다. 방장의 `com.AeDeong.SuddenForceFPS.sessioncheck` 앱에서 경기를 시작하고 MENU → QA · LAST 10 SEC를 누르면 경기가 재개됩니다. 참가자 메뉴에는 이 버튼이 없습니다. ADB에서도 아래 명령으로 같은 기능을 사용할 수 있습니다. `DEVICE_SERIAL`은 방장 기기의 ADB 식별자로 바꿉니다.

```powershell
adb -s DEVICE_SERIAL shell input keyevent KEYCODE_F10
```

남은 시간이 10초보다 많으면 다음 방장 시뮬레이션 틱에서 10초로 바뀝니다. 참가자 화면에도 같은 타이머가 표시되며 기존 시간 만료 로직이 승패를 결정합니다. Unity Editor 방장은 기존 **Tools → Sudden Force → Testing → Skip to Final 10 Seconds** 메뉴를 사용합니다. 일반 APK·AAB 빌드는 기본 컴파일 심볼을 유지합니다.
