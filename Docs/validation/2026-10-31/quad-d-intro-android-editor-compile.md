# Quad D 인트로의 Android 대상 Editor 컴파일 확인 (#302, PR307)

2026-10-31 별도 P15 `D:/codex/worktrees/sudden-force-fps-intro-android-20261031`에서 읽기 전용 관측을 수행했습니다. Root manifest의 source는 `cf681e202039cb98ed4645b0c96d86c108ca9b24`이며 P13 ba10 대비 제품 입력 차이는0이고 후속 문서만 다릅니다. 최종 Android 배포 소스를 선택하거나 Draft261을 포함한 결과가 아닙니다.

Unity6000.3.25f1의 새 PID27580/start00:22:45.2697783Z에서 Android·batch/Null·DOTNET1/BEE1, compile/update/failed/Play/build=false를 확인했습니다. 고정 관측 소스의 eval은 innerSuccess=true·진단0·sourceStable=true였습니다. Assembly-CSharp와 QuadD.Intro가 실제 로드됐고 NetworkRunnerManager·RuntimeInitialize·SuddenForceIntroCanvasOutput·QuadD.QuadDIntro 타입을 확인했습니다. 각 MVID는 `fea65d90-11cd-46da-a43e-0bd0736c7c91`, `89d026ce-37f8-4d9d-b4b9-c77a28936653`입니다.

`CompilationPipeline.GetAssemblies(Editor)`는 두 어셈블리에 UNITY_ANDROID와 UNITY_EDITOR를 함께 보고했습니다. 이는 Android 대상 **Editor 컴파일 조건**의 관측이며 Player 전용 분기 컴파일·IL2CPP 변환/링크·Gradle·Android 실행을 증명하지 않습니다. 두 수용 플래그는 false입니다.

com.quadd.intro0.1.0/Git/fullcommit55bf9e4와 Pipeline0.8.0-exp.1 등록, Intro→Login→Lobby→Room→DesertHouse의 enabled5씬을 확인했습니다. VideoClip3.4초/1920×1080/30fps/audio1은 에셋 metadata이며 재생·실청·decoder·픽셀 증거가 아닙니다. 공개 getter의 IL2CPP/ARMv7+ARM64/min25/target36·1.0.194/code5는 checkout 관측값으로, ARM64 단독 새 배포 설정이나 코드35 예약이 아닙니다. 관측 helper의 직접 설정 변경·Save·Build·Play·영상 재생·새 버전 설정/예약·AAB·서명·키 getter는0입니다. SDK/import 자동 생성 변화는 아래와 구분합니다.

정상 close 뒤 동일한 사전 보유 native handle의 정수 ExitCode0을 확인했고 후속 가드는 native/unknown0·TempLock=false였습니다. 종료 중 생성된 EDM Resolver와 ShaderGraph 설정2개는 생성본을 보존하고 바이트/의미를 분류한 뒤 원본으로 복구했습니다. 최초 변화1개 기대 assertion 실패는 변경 전 중단으로 보존했으며, 처음부터 전 입력이 불변이었다고 쓰지 않습니다.

복구 후 **Root manifest metadata의4,018개 전후 행은 exact**입니다. 독립 검토는 **ProjectSettings.asset을 실제 열기·해시 전에 제외한4,017개 실물을 재해시**해 일치시켰고, 해당 PS1행은 Root metadata 동일성만 대조했습니다. 비공개6의 내용·해시 접근0이며 종료 후 Root가6개 모두 실제 부재/indexS를 다시 확인했습니다. 이는 독립 검토자의 재조회와 구분합니다.

독립 판정은 `PASS_BOUNDED_ANDROID_TARGET_EDITOR_COMPILE_AND_PUBLIC_RESTORE`이며 Android Player 컴파일·runtime·최종 source·새35 AAB 수용은 false입니다. PR261 Draft와 기존 제공34/source612·권위원장196,2,34를 유지하고, 새 Android 설정 지속성·예약·실제 포함 빌드와 재생은 별도로 검증합니다.

근거 root: `D:/codex/validation/sudden-force-fps/20261031-work/`의 `android-intro-inspect01.public.json`, `android-intro-compile-independent-review01/verification.public.json`, `native-exit-p15/p15-android-compile01.public.json`, `p15-restore02.public.json`, `p15-after-restore02.public.json`, `p15-private-exclusion-after-close01.public.json`입니다.
