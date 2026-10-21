# Android 검증 도구

## ARM64 ELF LOAD·RELRO 정렬 검사

`verify_native_alignment.py`는 Python3.12 이상에서 AAB의 모든 모듈 또는 APK에 들어 있는 `lib/arm64-v8a/*.so`를 추출 없이 읽습니다. LOAD alignment·주소/파일 오프셋 congruence와 GNU_RELRO를 검사합니다. RELRO 끝 주소가16KB 정렬되거나, RELRO가 자체 writable LOAD의 전체 VA/메모리 구간과 일치하고 보호 페이지가 다른 writable LOAD의 페이지와 겹치지 않을 때 통과합니다. 모든 프로그램 헤더를 모은 뒤 판정하므로 헤더 순서에 의존하지 않습니다. 원본과 기존 보고서를 덮어쓰지 않고 Unity·Gradle·서명 도구를 실행하지 않습니다.

```powershell
python -B Tools/Android/verify_native_alignment.py '<검사할 AAB 또는 APK>' --output '<새 외부 JSON 보고서>'
```

종료0은 선택한 ARM64 ELF 검사 통과,1은 잘못된 ELF 또는 정렬 실패,2는 ZIP/입력/보고서 처리 오류입니다. RELRO가 없는 라이브러리는 정렬 실패로 보지 않지만 보안 강화가 됐다는 뜻도 아닙니다. ZIP CRC는 선택한 native entry에 대해 검사하며 전체 ZIP 엔트리 CRC 검사가 아닙니다. ELF64 little-endian ARM64 ET_DYN만 지원하고, 라이브러리128개/각256MiB 한도를 넘으면 실패합니다.

**APK ZIP alignment·BundleConfig·서명·기기 실행은 검사하지 않습니다.** ELF 정렬 통과만으로 전체16KB 호환을 확정하지 않습니다. [Android 공식 LOAD·RELRO 조건](https://developer.android.com/guide/practices/page-sizes#relro)과 실제16KB 환경 검증을 함께 적용합니다.

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

Gradle 검사14개와 ELF/ZIP 검사19개는 임시 fixture와 mocked Gradle 이벤트로 기대값·실패 거부·입력 보존 로직을 확인합니다. pip 설치나 custom secret, Unity license, SDK/JDK·Gradle 실행이 필요하지 않습니다. 실제 Gradle/AGP·init script, Unity 빌드·AAB·서명·배포·기기 실행은 CI 검증 범위에 포함되지 않습니다.

실제 GitHub Actions 실행 상태와 로그는 workflow를 추가한 PR의 checks 결과에서 확인합니다. 로컬 unittest 결과와 GitHub 실행 결과는 별도로 기록합니다. CI 실행이 성공해도 기존 export의 현장 dry-run 및 실제 Android 산출물 검증은 각각 필요합니다.
