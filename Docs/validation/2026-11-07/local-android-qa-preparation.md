# 별도 로컬 Android QA 준비

관련 #347, #66, #41. 이 branch는 main의 중앙 HIT 표시 제거와 Draft261의 모바일 입력, Draft345의 소총 후보를 합친 **검증용 소스 후보**입니다. 각 부모 기능의 제품 수용·main 병합을 대신하지 않습니다. 보행 후보는 포함하지 않았습니다.

사용자는 2026-11-07 질문 없이 개발·LD와 연결 공기계의 설치/검수를 이어가도록 허용했습니다. 기존 앱·데이터·키와 production 버전 원장은 보존합니다. 작업용 Sudden Force Console 탭이 없어 fresh 원격 최대 코드를 조회하지 못한 상태에서 배포 코드를 추측 예약하지 않습니다.

새 앱은 `com.AeDeong.SuddenForceFPS.localqa` / `Sudden Force QA` / `1.0.197-qa1` / QA 전용 코드1로 계획했습니다. 아직 이 설정을 Unity에 적용하거나 코드를 예약·빌드·서명·설치하지 않았습니다. production `com.AeDeong.SuddenForceFPS`·원장196,2,34·제공34/release12와 별도이며 Play 업로드용 산출물이 아닙니다.

P13 `D:/codex/worktrees/sudden-force-fps-intro-android-20261031`의 기존 Android cache worktree를 사용합니다. 기존 d874 입력 소스를 보존한 branch에서 main a433f14와 소총 d328d9d를 정상 merge했습니다. private6는 변경 전·두 merge 후 모두 실제 부재/indexS를 확인했으며 내용·해시를 읽지 않았습니다. 기본 P2와 기존 dirty5를 변경하지 않았습니다.

필수 scene은 Intro→Login→Lobby→Room→Game/DesertHouse의5개입니다. 새 패키지는 기존 Android OAuth 등록을 승계하지 않으므로, 별도 QA define·정확한 package guard에서만 기존 로비 검수 경로를 쓰는 소스 후보를 검토합니다. 인증 완료 flag·계정 token·PlayFab 데이터를 가짜로 채우거나 기본 production 인증을 바꾸지 않습니다. 두 QA Player의 접속 suffix도 별도로 고정해야 합니다.

다음은 최소 helper/인증·접속 분리 소스의 독립 검토, 실제 Android 컴파일/unsigned export, 별도 Gradle·기존 키 local 서명과 정적 산출물 검증입니다. 새 APK를 만들면 실물/LD의 실제 설치 SHA·전체 인트로·입력·접속/경기/복귀를 따로 확인합니다. Editor 전체 수명과 ADB 기기 검수는 프로젝트 사이에서 직렬화합니다.

현재 실제 QA package 설정·Android export·Gradle·서명·APK·설치는0입니다. 원격 배포·계정/가격·키 생성/교체/이동도0입니다. 기존 코드34 LD의 11월7일 방 참가 ANR는 #346으로 보존하며 이 후보가 해결했다고 표시하지 않습니다. APK는 외부에 업로드하지 않고 실제 로컬 경로와 검증 범위를 보고합니다.
