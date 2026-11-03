# 통합 소스의 Android 대상 Editor 컴파일 관측

PR261의 UI·Intro 결합 source `f79664e39a7a79bc3b4d34074f831a2c913a2de2`를 별도 P13에서 Android target으로 열어 소스 bytes·로드된 두 입력 타입과 컴파일 실패 flag=false를 확인했습니다. 제품 입력 두 파일은 **PR261 Draft에 있으며 main에는 미병합**입니다. 이번 관측은 Android Player·IL2CPP/Gradle·AAB·실기기 또는 게임 L2 수용이 아닙니다.

## 실제 결과와 종료

- Unity6000.3.25f1, batchmode/Null, active target Android, DOTNET_PROCESSOR_COUNT=1/BEE_BUILD_THREADS=1을 확인했습니다. 원본 LF/전체 CRLF의 고정 raw·정규화 SHA와 public PlayerInputSource/CombatPointerControl 타입을 대조했습니다.
- native PID45352/start09:09:01.8331487Z/direct ticks639292937418331487입니다. 두 타입의 같은 Assembly-CSharp MVID는 `7a033485-6b36-4ec5-a804-efb071234447`입니다. 임시 Editor helper를 포함한 관측이며 loadedIL-source 직접 동일성이나 clean checkout 통과를 증명하지 않습니다.
- same-held native 정수 ExitCode0, 실제 operator tool session11492/terminal680ffb exit0, Dispose0·강제종료0·관측한 직접 compiler 잔존0·임시 환경 복원 오류0을 확인했습니다. 모든 미확인 후손 프로세스까지 추적했다는 뜻은 아닙니다.
- 실제 차례09:07:46.0979510Z, 원 새실행 중단09:17:46.0979510Z/전체반환09:22:46.0979510Z를 고정했고 실제반환은09:12:23.7939800Z입니다. 임시 파일3개를 외부 보존 후 제거하고 빈 폴더1개만 제거했습니다. recursive 삭제는0입니다.

전역 진단 수는 unknown(-1)입니다. 컴파일 실패 flag=false·타입 로드를 “전역 진단0”으로 표현하지 않습니다. 실제 Play·영상 decode/청취·입력·LD/ADB·예약/빌드/서명/업로드는0입니다.

## 첫 사후 불일치와 새 감사

f796의 공개 추적 파일4,039개/750,357,480B를 실행 전에 고정했습니다. 이는 이전 Windows source989의4,038개 증거를 재사용한 것이 아닙니다. Root가 실제 차례 시작을 먼저 기록하고 CreateNew before audit의 UTC가 같은 원시작 이후임을 별도로 검사했습니다. lifecycle source 자체에는 이 시각 floor가 없다는 독립 검토의 외부 전제를 실제로 충족했습니다.

첫 after01은 Resolver asset 한 개가1,983B→1,904B로 달라 **불일치**했습니다. 다른4,038개 행은 같았습니다. Root는 원본·생성본을 보존하고 빈 YAML `m_Name`/`value` 뒤 공백2개와 CRLF/LF 차이만임을 분류한 뒤, Editor 종료 상태에서 그 한 경로의 정확한 원문만 복원했습니다. 첫 실패는 그대로 보존했습니다.

별도 after02에서4,039개 모든 path/size/SHA 행과 전체 byte 수가 before와 같음을 확인했습니다. P13 Gitclean·TempLock없음, P2 기존 설정5개 원문 보존과 private6 실제부재/indexS를 확인했습니다. 키/비밀 파일의 내용 읽기·해시는0입니다. ignored cache·모든 메모리 또는 캐시의 무변화는 주장하지 않습니다.

## 근거와 다음 단계

R=`D:/codex/validation/sudden-force-fps/20261103-work/resume-0843`입니다.

| 공개 근거 | SHA256 |
| --- | --- |
| p13-android-compile-return-checkpoint01.public.json | `A9B1923997610779441639C212A7A8AA7BD983166A2F1E9624621464381811D9` |
| p13-android-compile-actual-independent-review01/verification.public.json | `CF88D98D26EE6D5163A843EF93B0304ADA1AE186719D352BB42F36AC0FEA9A20` |
| p13-public-inputs-before01.public.json | `F2FD6B446C7358B3DB324739EDB6C4B949174827B41F195D539D6DFC1EA5E83B` |
| p13-public-inputs-after02.public.json | `4F4EE280A226EACF0FD95CB3962946B5D17DA456E8B00C5AA6057A6BD3290B47` |

독립97개 대조는 공개 JSON/고정 source의 일관성 검토 범위입니다. Root의 실제 tool 결과·OS/Gitclean·보존 원문·서식 분류를 독립 검토자가 다시 관측한 것은 아닙니다. 첫 사후 감사 실패를 이97개 통과에 합산하거나 성공으로 고치지 않았습니다.

기본 Unity/Fork는 P2를 유지하며 P13은 `D:/codex/worktrees/sudden-force-fps-intro-android-20261031`의 별도 검증 경로입니다. 최신 main의 후속 Tools/문서와 컴파일 source f796을 구분합니다. 이번 새35 예약·AAB/APK·서명·제공은0이고 마지막 확인 제공34/1.0.196/release12를 유지합니다. 오늘 새 Console 최대코드 조회는0입니다.

다음은 최종 포함 source·fresh Console/권위원장·공개 unsigned 설정의 저장/재시작 지속성 확인 후 새 Android 산출물 준비입니다. “35”는 다음 후보 이름이며 예약한 번호가 아닙니다. #66/#259/#302·PR261 Draft, 실폰/실제16KB/장시간 성능 후순위는 그대로입니다. 현재 필요한 사용자 조치와 질문은 없습니다.
