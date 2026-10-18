# Gradle export 사전 검사

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
python -B -m unittest discover -s Tools/Android -p test_verify_gradle_export.py -v
```

현재 stdlib 기반14개 테스트는 임시 fixture와 mocked Gradle 이벤트로 기대값·실패 거부·입력 보존 로직을 확인합니다. pip 설치나 custom secret, Unity license, SDK/JDK·Gradle 실행이 필요하지 않습니다. 실제 Gradle/AGP·init script, Unity 빌드·AAB·서명·배포·기기 실행은 검증 범위에 포함되지 않습니다.

실제 GitHub Actions 실행 상태와 로그는 workflow를 추가한 PR의 checks 결과에서 확인합니다. 로컬 unittest 결과와 GitHub 실행 결과는 별도로 기록합니다. CI 실행이 성공해도 기존 export의 현장 dry-run 및 실제 Android 산출물 검증은 각각 필요합니다.
