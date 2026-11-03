# Quad D 인트로의 단일 GameView와 Windows 빌드 검증

> 후속: 사용자의 권한 창 닫기 이후 새 두 Player에서 [전체 인트로 표시·기존34 분리·경기/복귀/재참가·정상 Exit0](quad-d-intro-windows-regression.md)을 확인했습니다. 아래는 최초 실행의 부분 검증/강제 종료 기록이며 새 결과로 덮어쓰지 않습니다.

2026-10-31 KST, #302/PR307의 **부분 검증 기록**입니다. 새 Windows 빌드와 단일 GameView 표시를 확인했으며, Windows 두 피어는 실제 경기까지 진입했습니다. 방화벽 권한 창이 나타난 뒤 GUI 입력을 보류했습니다. 결과·RETURN·재참가·Player 정상 EXIT 수용과 기존34 매칭 공간 분리는 아직 완료하지 않았으므로 PR307은 Draft로 유지합니다. 테스트 Player 두 개는 아래와 같이 강제 정리했습니다.

## 소스와 실행 범위

- 빌드 소스는 `ba10efd2f07a8ec895ac779634f2b5201a424364`, 당시 main은 `02a482c9e97d293077553d76884295ac1b776fa5`입니다. 이후 문서 변경을 바이너리의 추가 제품 변경으로 계산하지 않습니다.
- 별도 P13 `D:/codex/worktrees/sudden-force-fps-intro-windows-20261030`에서 진행했습니다. 개인6은 실제 부재/index S이며 내용·해시를 읽지 않았습니다. 기본 P2와 기존 dirty5는 별도로 보존합니다.
- Unity 6000.3.25f1/DX11/StandaloneWindows64/Mono, 실제 `DOTNET_PROCESSOR_COUNT=1`, `BEE_BUILD_THREADS=1`입니다. Editor PID10712/start `2026-10-30T23:38:39.5893956Z`, Assembly-CSharp MVID `8a5bb2b0-0138-4a0b-8853-e5e234a3c235`를 새로 관측했습니다.
- 초기 준비 요청의 `EDITOR_NOT_RUNNING`/`EDITOR_NOT_READY`는 import 대기 관측이며 C# 컴파일 실패로 세지 않습니다. 실제 준비·후보 비활성 실행은 inner success/진단0으로 확인했습니다.
- 고정 회사 패키지 `55bf9e4aa48c2f95942f3c2c995f1d55414e97c0`/0.1.0, Pipeline 0.8.0-exp.1입니다. 패키지 runtime raw5044B와 SHA `D789D2ED5E997F2BA51BA1A5C94389BBFCD0F2618EB4424ADA2F56591DD640AF`, strict UTF8 CRLF→LF SHA `865B3D03067315C349C73067002AEDE44D2C8749BB56C94F964117EB24C55368`를 실행별로 대조했습니다.

## 단일 GameView의 실제 표시

새 P13에서 기존 Simulator 하나를 닫고 GameView 하나만 준비했습니다. 기존 16:9 프리셋을 사용했고 실제 targetSize는1038×584, Display0/Simulator0입니다. 로고의 심벌·글자와 로비 닉네임 팝업 및 보이는 메뉴 경계가 잘리지 않는 화면을 직접 확인하고 독립 검토했습니다. 팝업에 가려진 모든 로비 영역, 실제 청취, 모든 화면비의 수용으로 확대하지 않습니다.

STOP 요청 뒤 playing/transitioning/dirty scene=false를 확인했습니다. 별도 자기 뷰 닫기는 inner success=false/진단0으로 끝나 미완료로 보존했습니다. 이후 Editor 정상 close/보유 native handle의 **실제 int ExitCode0**, fresh native0/unknown0/TempLock 부재를 확인해 해당 창의 수명을 끝냈습니다. CLI 성공과 native 종료 근거를 구분합니다. 이전 P2 다중 view의 검은 출력·잘림 및 원본 observer false는 변경하지 않았으며, 원인 해결로 선언하지 않습니다.

초기 이미지 저장 시 JPEG 바이트에 `.png` 확장자를 붙인 기록을 보존하고, 바이트가 같은 `.jpg` 사본을 만들었습니다. 현재 참조는 R31 `gameview-actual01/logo-view-1.jpg`, `lobby-view-1.jpg`입니다.

## Windows 산출물

새 core03을 실제 컴파일·기본 false detached terminal까지 확인한 뒤, 새 PID/startTicks·nonce·120초 시작 TTL의 config로 BuildPlayer를 **한 번** 호출했습니다. 문서화된 `--detach`/`job status` 경로를 사용했으며 지연 callback wrapper를 추가하지 않았습니다.

| 항목 | 실제 결과 |
| --- | --- |
| BuildReport | Succeeded / 오류0 / 경고2 /145.7320617초 |
| 실행 파일 | R31 `intro-windows-build-attempt01/Player/SuddenForceFPS.Intro.Validation.exe` |
| EXE SHA256 | `9985CEA53B68F2D8787AAB4F496A228C335A139B39703A3915BEF6676716662A` |
| 실제 payload |363파일 /279,003,544B, 독립 전체 크기·SHA 일치 |
| 보고서 합계 |278,805,958B |
| 차이 |197,586B = 보존 Burst DoNotShip 텍스트1개의 실제 크기; 파일별 보고서 제외 provenance 자체는 미확보 |
| 영상 |3.4초/1920×1080/30fps/audio track1, packed source 경로 보고됨 |
| 입력 | 선택8개·패키지 runtime의 실행 전후 raw 일치 |
| Windows 버전 metadata |1.0.194 — Android 새 버전 예약과 무관 |

빌드·import·Play 후 공개 입력4017개 중9개가 바뀌었습니다. 생성본을 보존한 뒤 Editor가 종료된 상태에서 초기 바이트로 복원했고, 전체4017개 exact/추가·삭제0 및 Git clean을 확인했습니다. 독립 분류상6개는 CRLF→LF만, CJK font는 줄바꿈·빈 YAML 값 뒤 공백10개, EDM은 줄바꿈·공백과 설정값3개 변경입니다. ProjectSettings는 총괄의 비공개 본문 비교에서 CRLF→LF만이었습니다. 따라서 당시 전체 입력을 처음부터 raw 불변 또는 모두 형식 변화였다고 보고하지 않습니다. 빌드 helper의 직접 global save·PlayerSettings/BuildSettings setter·Android 예약·서명 호출은0이며, 진단의 GameView/Play/SessionState setter 및 SDK/빌드 자동 변경과 구분합니다.

## 실제 두 Player의 현재 범위와 제한

같은 payload의 Host37576/Client17456을 각1280×720으로 실행했습니다. Client startup 캡처8장 중0~6은 Unity splash/빈 화면이고,7에서 주황 심벌2개가 보이는 프레임1장을 확인했습니다. Windows의 전체 Quad D 글자·애니메이션 진행은 미검증입니다. `host-intro`, `client-intro-late` 파일은 실제로 post-intro Loading 화면이므로 인트로 표시 근거에서 제외합니다. 두 Player 모두 로비에 도달했습니다. `Intro31H`/`Intro31C`로 `FPSIntro31A` 방 생성→목록 수신→참가→READY/START→서로 다른 시점의 Desert House 경기를 실제 조작했습니다. Host의 한 번 사격으로 탄약30→29, R 재장전 뒤30을 확인했습니다. 피해·사망/리스폰·물리 멀티터치나 Draft261 수용이 아닙니다.

같은 실행 파일의 두 창을 배경 상태로 캡처했을 때 전경 화면이 반환된 경우가 있었습니다. `client-game-start01`, `client-after-host-shot01`은 독립 Client 근거에서 제외합니다. 실제 전경 전환 후 Client의 다른 시점/탄약30과 Host의 탄약29를 다시 확인했습니다. 기존 잘못 연결된 이미지와 관측은 삭제하거나 새 성공으로 바꾸지 않았습니다.

종료 watcher01은 JSON ISO 날짜의 자동 DateTime 변환 뒤 문자열 parse에서 freshness guard가 실패했습니다. 프로세스 조회 전에 멈춘 실패를 보존했습니다. 새 watcher02는 `ConvertFrom-Json -DateKind String` 한 곳만 수정했으며, 독립 통제 입력에서 ISO 보존·정상 경과시간을 확인했습니다. 두 실제 PID/start/exe에 각각 보유 handle을 연결했습니다.

이후 2026-10-31T00:13:40Z 총괄이 띄운 두 테스트 Player만 PID/startTicks/exe를 다시 대조해 강제 정리했습니다. 보유 handle은 양쪽 실제 정수 ExitCode=-1을 읽었습니다. Root의 Stop-Process 호출2개와 watcher 자체의 killCalls0을 구분하며 **정상 EXIT 통과는0**입니다. 두 로그의 Crash!!!/Fatal/NullReference 표식은0이지만 강제 종료를 정상 종료나 #154 해결로 바꾸지 않습니다. 종료 후 자기 Player 창0을 확인했으며 방화벽 권한 창은 계속 남아 있습니다. 권한 창 입력·보안 설정 명령은0입니다.

새 EXE의 Windows 방화벽 권한 창은 총괄이 조작하지 않았습니다. 화면 조작·결과/RETURN·재참가·old34 음성 비교·정상 EXIT는 그 뒤의 대기 범위입니다. 기존 FPS 에뮬레이터에서 앱 실행까지 관측했지만 이번 설치 버전/인증서·새 매칭 비교는 확인하지 않았습니다. 기존 앱 제거·데이터 삭제·새 설치·ADB 서버 시작/재시작은 하지 않았습니다.

## 후속과 근거

권한 창 처리 후 같은 검증 binary의 새 두 Player와 소유 handle을 준비해 결과·RETURN·새 방 재참가·정상 EXIT를 다시 확인합니다. 종료된 PID37576/17456을 재사용하지 않습니다. 이후 old34의 실제 설치 신원·Connected/새 목록이라는 양성 조건과 새 버전 방 미노출을 함께 비교합니다. 현재 브라우저 inventory에는 정확히 허용된 FPS Console 탭이 없어 claim/선택/탐색을 하지 않았으며, Android 다음 예약에는 fresh Console 최대 코드·권위원장·최종 포함 소스를 다시 결속해야 합니다.

R31은 `D:/codex/validation/sudden-force-fps/20261031-work`입니다. 실제 근거는 `p13-build-binding01.public.json`, `intro-windows-build-attempt01/`, `windows-payload-audit01.public.json`, `p13-after-restore01.public.json`, `native-exit-p13/p13-import-build01.public.json`, `windows-runtime01/`, `players-forced-cleanup-intent01.public.json`, `players-forced-cleanup-result01.public.json`, `host-exit01.public.json`, `client-exit01.public.json`입니다. 독립 검토는 `gameview-results-independent-review01/`, `intro-windows-payload-independent-review01/`, `intro-windows-watch02-independent-review01/`, `intro-windows-source-preservation-review01/`에 보존했습니다.

현재 제공은 계속 코드34/release12/source612 QA이며 원장196,2,34·새35 예약/Android AAB·APK/서명/업로드는0입니다. 이번 UI·인트로와 새 Windows 결과가 기존 제공34에 포함됐다고 보고하지 않습니다.
