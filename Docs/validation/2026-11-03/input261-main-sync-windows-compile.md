# PR261 최신 main 동기화와 Windows 통합 컴파일 관측

2026-11-03 최신 main과 기존 QA 입력 변경을 결합한 source989c9e9f586db1d519131307a65e81f0bf65b0a7에서 Windows Editor 컴파일 관측·정상 종료·정리를 확인했습니다. 임시 Editor helper가 포함된 관측이며 Android/Player 빌드·실제 입력·전체 L2 수용은 아닙니다. #259 OPEN/PR261 Draft를 유지합니다.

## 소스 동기화와 범위

기존 QA parent는 f3eabfb86a1db7a2ba21cd8f07cf8c194e4eea47, 결합한 main parent는69de6cd8df9425a152bddf9831a188b4991cd759입니다. main 대비 제품 변경은 PlayerInputSource.cs·CombatPointerControl.cs 두 파일뿐이며 각각 기존 QA blob과 같습니다. 나머지 tree entry는 main과 같습니다. 독립 소스 검토 뒤 정상 fast-forward push했고 로컬 aa56f389와 원격 f3eabfb8의 이력을 보존했습니다.

P12는 `D:/codex/worktrees/sudden-force-fps-intro-windows-20261030`입니다. 이전 ba10efd2f07a8ec895ac779634f2b5201a424364는 로컬 `codex/preserve-p12-ba10-20261103`에 보존하고 기존 경로에서 PR261 branch로 전환했습니다. 활성 기본 Unity/Fork 경로 P2를 대체하지 않습니다. 개인 파일6개는 sparse 제외·실제 부재/indexS를 확인했으며 내용·해시는 읽지 않았습니다.

## 실제 관측

| 범위 | 결과와 한계 |
| --- | --- |
| 원 자원 창 | 시작04:28:18.9157784Z, 새 작업 중단04:33:18.9157784Z, 정리 상한04:38:18.9157784Z |
| native 신원 | PID46860/start04:28:27.9335223Z/ticks639292769079335223, 시작 직후 보유한 같은 handle |
| Editor | Unity6000.3.25f1·batchmode·graphics Null·StandaloneWindows64, compiling/updating/playing/compilationFailed=false, DOTNET1/BEE1 |
| 입력 source2 | raw LF 또는 전체CRLF의 고정 핀과 CRLF→LF의 Git QA 핀을 동시에 확인; 입력 메서드 호출0 |
| public type metadata | PlayerInputSource·CombatPointerControl의 Assembly-CSharp/MVID c22e5782-8db1-4cd0-9d69-f75024edf3a3 확인 |
| 종료·도구 | -quit 경로의 same-held native 정수Exit0, 실제 operator session47531/terminal6e63cd 완료0, Dispose0·환경 Process 변수 복원 오류0·강제 종료0 |
| 알려진 compiler | 관측한 Bee12748·UnityShaderCompiler29520의 원 PID/birth 부재 확인; 모든 unknown 자손 부재 주장 아님 |
| 정리·실제 반환 | own 파일3개·빈 Editor 폴더1개만 제거, 실제 반환04:32:49.4071699Z; 원5/10분 창 안 |

시각은 UTC입니다. C# proof의 sourceCommitClaim은 provenance 문자열일 뿐 sourceCommitVerified=false입니다. Root가 실행 전후 Git head와 공개 원문 audit를 별도로 결속했습니다. helper proof·same-native Exit0·실제 tool 완료0을 각각 확인했으며 exit0만을 컴파일 통과로 세지 않았습니다.

관측은 scriptCompilationFailed=false·source bytes/type metadata 범위입니다. 전역 diagnostics 수는 unknown(-1)이며 모든 진단0·loaded IL과 source의 직접 동일성·clean checkout 빌드 성공으로 확대하지 않습니다. Play/게임·Android target·Player/APK/AAB·새35 예약·서명·업로드는0입니다. 큰 FIRE·Intro가 합쳐진 소스의 Windows C# 관측이지 Android 화면·영상/음성·터치·사망/리스폰 수용이 아닙니다.

## 자동 직렬화와 보존 감사

사전 공개 추적 입력은4,038개/750,351,106B입니다. 종료 뒤 첫 audit는 `ProjectSettings/ExternalDependencyManagerResolverSettings.asset` 한 파일이1983B→1904B로 바뀌어 false였으며 그 결과를 보존했습니다. 다른4,037개는 rawexact였습니다. Root가 원문 두 버전을 먼저 보존하고, CRLF/LF 및 빈 m_Name·podToolOverridePath value 뒤 공백2개만 달라 의미 설정값 변화0임을 확인했습니다. Editor가 종료된 상태에서 해당 한 경로를 정확한 원1983B/SHA01E2562923C891ACD39BCBD35BCA486695032950F41637A0FF4B8A410C2B1B6F로 복원했습니다. 전체 프로젝트 reset은 하지 않았습니다.

별도 새 after02 audit는4,038개 rawexact·Gitclean을 확인했습니다. 첫 false·생성본·원본·복원 receipt를 편집하거나 성공으로 고치지 않았습니다. P2 기존 dirty5도 byteexact를 확인했습니다. Library 등 ignored cache·모든 내부 메모리 무변화는 주장하지 않습니다.

## 검토와 근거

R=`D:/codex/validation/sudden-force-fps/20261103-work/resume-0210`입니다. helper SourceIR ED5E1755…DF3E6E40와 Operator02 SourceIR369ED981…D87BE9D1을 별도로 수용했습니다. Operator01의 문화권 BOM 비교·raw ExitCode 선행 cast 두 문제와 false IR는 보존하고, Ordinal 비교·raw Int32 선행 검증만 바꾼 새02로 실행했습니다.

독립 실제 검토 `R/p12-combined-compile-actual-independent-review01/verification.public.json`은3,299B/SHA61B6557E83FC24300A61006DD00C36E77054B22D76B5408E6888DAECEADE2987입니다. 공개JSON19개/49항목을 대조해 위 좁은 Windows Editor 관측·종료/정리·복원/반환을 수용했습니다. 사전/첫after/복원후after2의4,038개 metadata 목록을 직접 비교했으며 YAML 분류·P2보호5·실제 tool 출처는 Root의 공개 관측을 읽은 검토입니다. 실제 OS/프로젝트/설정/기기·private 원문 재관측은 하지 않았습니다.

- `R/p12-combined-compile-return-checkpoint01.public.json`: SHA A1E4A6A79CEEA3CC471ECA8CE1AAB4EFC83ACD8DFC21B762AEFD98D9FF0CDBF5.
- `R/p12-public-inputs-before01.public.json`, `after01`, `after02` 및 `p12-auto-setting-restore02.public.json`을 함께 읽습니다. after02 SHA는6247C565637B4218713D179EA5AF11997ED2DD97F478E7E962DBC0CE1E67772B입니다.
- `R/p12-combined-compile-01/`의 scope·staging·proof·native launch/result/dispose·operator result·cleanup은 이번 소비된 회차의 원본입니다. 같은 출력/기한/신원을 새 실행에 재사용하지 않습니다.

현재 추가 문서는 컴파일 뒤에 작성했으며 위 source989의 제품 코드를 바꾸지 않습니다. 마지막 확인된 제공 기준은34/1.0.196/release12이고 이번 Console 조회·새 배포는0입니다. 다음은 통합 Android target/산출물과 실제 입력·L2 검증이며 실폰·장시간 성능은 합의대로 후속입니다. 현재 필요한 사용자 조치와 질문은 없습니다.
