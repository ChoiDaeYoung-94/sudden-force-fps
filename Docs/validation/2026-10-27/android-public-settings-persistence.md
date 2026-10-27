# Android 공개 빌드 설정의 저장·재시작 확인 · 2026-10-27 (#66)

별도 QA P12 `D:/codex/worktrees/sudden-force-fps-android-main-20261026`의 제품 source는 `e19cb55eb78cee238e5dd8582243f0c0dfd628bc`입니다. 이후 main의 APK 전달 정책은 문서 변경이며 기본 Unity/Fork 경로 P2를 대체하지 않습니다. P2의 기존 설정5개와 소비 원장 `196,1,33`은 보존했습니다.

## 설정 준비와 실제 확인

새 Android batch/Null Editor에서 소유 PID·시작 시각·Game assembly MVID·clean Untitled·빌드 씬4개·PS/EUBS 단일 identity와 공개 파일3,990개를 고정했습니다. 비공개 키/환경·자격정보 helper6개는 부재/indexS이며 내용·해시는 읽지 않았습니다.

독립 검토와 미호출 컴파일 뒤 실제 setter4개를 한 번씩 적용했습니다. 목표는 ARM64 단독·exportProject=true·appBundle=true·customKeystore=false입니다. 시작 dirty520개는 기존 tuple 그대로 유지됐고 PS752/EUBS810만 추가돼522개가 됐습니다. 별도 후속 관측에서도 같은 API/identity를 확인했고 공개3,990개는 당시 디스크 바이트가 그대로였습니다.

그 뒤 별도 검토·미호출 컴파일을 거친 `File/Save Project` 메뉴를 단1회 실행했습니다. 반환 true·PS/EUBS clean을 별도 후속 관측에서 확인했습니다. 이는 프로젝트 전체 저장 호출이며 SDK·Library·모든 메모리 보존을 뜻하지 않습니다. 다른 native dirty 객체도 함께 처리될 수 있는 범위를 사전에 분류하고 실제 전후 차이를 별도로 검사했습니다.

| 검사 | 실제 결과 | 한계 |
| --- | --- | --- |
| 공개 디스크 차이 | PS1개만 변경, 다른3,989개 raw exact | 공개 tracked 입력 범위. 전체 SDK/Library/캐시 전수 아님 |
| PS 의미 변화 | `AndroidTargetArchitectures`3→2, `androidUseCustomKeystore`1→0 | 버전명1.0.194/code5와 나머지 값은 보존 |
| PS 원래 예측 바이트 | raw 일치false를 보존 | Unity 직렬화의 CRLF1,023개→LF 추가 변화 |
| PS 정규화 대조 | 예측한 두 값 변경 외 추가 차이0 | 실제28,718B/SHA `1077655B0C0F64DE9F91FC130E5246A93308312627C4B3B48EFEB7DF973F5B88` |
| 기존 Editor 종료 | 사전 보유 process handle의 ExitCode0, 강제 종료0 | Windows Player 종료 시험·#154 원인 해결 아님 |
| 새 Editor 재시작 | 새 PID/시작 시각, ARM64·export/bundle true·customfalse 유지, PS/EUBS clean | 새 AAB/Gradle/실기기 통과 아님 |
| 종료·재시작 후 공개 입력 | 저장 후 기준3,990개 전부 raw exact | 원래 checkout 전체 raw exact와 구분 |

원래 예측 SHA를 맞추려고 파일을 다시 쓰거나 저장을 반복하지 않았습니다. 이전 inspector의 공개 경로 마스킹 문제와 memory 후보의 EUBS 범위 부족은 **실행 전 정적 차단**으로 보존한 뒤 별도 revision에서 보완했습니다. 보완본의 실제 실행·저장 결과만 현재 근거로 사용합니다. 외부 실제 산출물 독립 검토는 공개3,990개 직접 재해시·PS 전후 바이트·소유 종료/재시작 근거90검사에서 blocker0을 확인했으며, 실제 실행과 독립 재검산의 역할을 구분합니다.

## SDK 도구 확인

실제 Editor getter에서 SDK/NDK는 해당6000.3.25f1 설치 경로, JDK는 이미 구성된 `D:/codex/toolchains/sudden-force-fps/unity-6000.3.25f1/OpenJDK`를 확인했습니다. 기존 JDK 경로·설정은 바꾸지 않았습니다. 최초에 전부 내장 경로라고 요구한 사전 가드가 막힌 것은 도구 실행0이며 설치 실패로 판정하지 않았습니다.

기존 구성 경로를 사용한 별도 검사에서 java/javac17.0.18, adb36.0.0, clang18.0.3의 version 명령4개가 실제 종료0입니다. NDK27.2.12479018·SDK Platform36/android.jar27,768,026B·build-tools36.0.0/aapt2/zipalign/apksigner 존재도 확인했습니다. 새 설치·약관 수락·기기 연결·Gradle/실제 AAB 빌드·환경값/원시 출력 저장은0입니다. version 실행과 경로 존재를 전체 SDK/Gradle 안정화로 확대하지 않습니다.

## 배포와 후속

현재 API의 **1.0.194/code5는 checkout 값**이며 새 배포 예약이 아닙니다. 마지막 알려진 제공본은 **코드33/1.0.196/release11**, 원장은 `196,1,33`입니다. 이번 단계의 새 코드 예약·AAB/APK·서명·Play/Drive 업로드는0입니다. 다음 코드는34 이상이 하한이며 정확한 Console 최대 코드·현재 원장·UTC 주차와 source/입력을 새로 고정한 뒤 별도 예약·버전 설정 저장/재시작·export로 진행합니다. 기존33의 승인·nonce·성공 proof나 옛 PID를 복사하지 않습니다.

테스트 APK는 사용자 최신 결정대로 외부에 직접 업로드하지 않고 실제 로컬 경로·버전·검증 범위를 보고합니다. Drive 연결은 APK 전달 선행 조건에서 제외합니다. 기존 키 유지·키 백업 보류·실기기/청취/실제16KB·발열/장시간 성능 후순위 합의는 유지합니다. #66/#43 전체를 닫는 근거는 아닙니다.

근거는 외부 `20261027-work/`의 `p12-memory4-v2-actual01.public.json`, `p12-memory4-root-assessment01.public.json`, `p12-save-actual01.public.json`, `p12-save-classification01.public.json`, `p12-owned-editor-close01.public.json`, `p12-after-restart-inspection02.public.json`, `p12-persistence-root-assessment01.public.json`, `p12-persistence-independent-review01/`, `android-tool-preflight02.public.json`입니다. 새 공개 문서 검토도 실제 빌드·기기 검증을 대신하지 않습니다.
