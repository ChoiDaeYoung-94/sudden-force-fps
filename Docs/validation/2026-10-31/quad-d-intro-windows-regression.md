# Quad D 인트로 Windows 전체 표시와 두 Player 회귀 (#302, PR307)

2026-10-31 KST, 첫 부분 검증 이후 **같은 Windows payload의 새 실행 두 개**를 사용한 후속 기록입니다. 사용자가 방화벽 권한 창을 직접 닫았다고 알려주신 뒤 재개했습니다. 총괄의 권한 창 입력·보안 설정 변경은0입니다. 첫 실행의 강제 종료2회/ExitCode=-1과 잘못 연결된 배경 캡처는 [당시 기록](quad-d-intro-windows-partial.md)으로 그대로 보존합니다.

## 실행과 인트로 표시

- 제품 빌드 소스는 `ba10efd2f07a8ec895ac779634f2b5201a424364`입니다. 이후 `cf681e`와 main 문서 병합 `c30bbf3`은 제품9개 blob이 모두 같으며 새 제품 빌드를 한 것은 아닙니다.
- EXE SHA256 `9985CEA53B68F2D8787AAB4F496A228C335A139B39703A3915BEF6676716662A`, 전체363파일/279,003,544B는 이전 독립 payload 감사에 결속했습니다. 새 launcher도 manifest SHA와 EXE/UnityPlayer/Assembly-CSharp3개를 다시 확인했습니다.
- 새 Host PID38900/startTicks `639290031513139035`, Client PID8744/startTicks `639290032094891744`를 사용했습니다. 각1280×720 창이며, 이전 종료된 PID37576/17456은 사용하지 않았습니다.
- Host의 새 시작16프레임에서 Unity splash→빈 화면→Quad D의 주황 심벌4개·회사 글자·Games & Development 표시와 애니메이션 진행→Loading까지 직접 확인했습니다. 이후 로비 도달은 별도 화면에서 확인했습니다. 전체 로고가 보이는 새 Windows 근거이며, 첫 실행의 심벌1프레임을 확대 해석한 결과가 아닙니다. 두 Player 모두 로비에 도달했습니다.
- 동일 EXE의 배경 캡처 혼선에 따라 이번 입력·화면 검증은 대상 창을 실제 전경으로 활성화한 후 fresh state로 진행했습니다. Host와 Client의 서로 다른 게임 시점을 각각 확인했습니다.

실제 음향 청취·모든 화면비·실제 Android decoder/복귀의 수용은 아닙니다. 영상과 오디오 에셋 metadata·설정 및 포함 근거는 이전 통합/빌드 기록과 구분합니다.

## 기존34와 새 접속 버전의 좁은 비교

기존 FPS 에뮬레이터의 설치 metadata는 `com.AeDeong.SuddenForceFPS`, 코드34/1.0.196입니다. 기존 앱·데이터를 지우거나 새 설치하지 않았습니다. 기존34의 공개 소스612와 새 Windows 빌드 소스의 PhotonAppSettings Git blob은 모두 `51e407fa56cc727596dc153f286fbc1e6e01ea12`이며 본문이나 AppId를 출력하지 않았습니다. 실제 기존 앱의 scoped Unity 로그와 새 두 Player 로그에서 `kr` 연결을 확인했습니다.

1. 기존34의 `Legacy34`가 `FPSLegacy31A` 방을 생성해 실제1인 방/Ready 상태를 표시했습니다.
2. 그 방이 살아 있는 동안 새 Host가 `FPSIntro31B`를 생성했고, 새 Client는 **새 방1/2만** 표시했습니다. 기존 방은 표시하지 않았습니다.
3. 기존34가 자기 방을 나와 새 Connected 로비·목록0을 받은 동안, 새 Client에는 새 방1/2가 계속 표시됐습니다. 새 방이 가득 차거나 경기가 시작되기 전의 비교입니다.
4. 새 Client가 그 방에 참가해 양쪽 닉네임과 READY를 표시하고 실제 경기에 진입했습니다.

이 결과는 이 기존34·고정 새 Windows payload·kr 표본에서 접속 공간이 분리되고 새 양쪽 접속이 동작한 근거입니다. 모든 리전·모든 이전/향후 버전의 혼용 차단 또는 새 Android runtime 수용으로 일반화하지 않습니다. 기존 앱은 EXIT/OK 후 에뮬레이터 홈 복귀를 확인했으며 Windows식 보유 native ExitCode를 얻은 것은 아닙니다.

Android 입력은 SDK ADB의 소유 loopback 대상만 사용했습니다. 첫 창구의 닉네임 붙여넣기 불일치는 확인 버튼을 누르기 전에 Android 직접 입력으로 수정하고 정확한 `Legacy34`를 확인했습니다. 예상 밖 문자열을 Git/새 증거 파일에 복사하지 않았으며, 당시 도구 화면에는 보였다는 한계를 별도 기록했습니다. SDK Editor 종료 뒤 필요했던 ADB 서버 시작은 총2회이며, 종료 시 다른 등록 대상1개가 있어 단독 대상 guard가 변경 전에 멈췄습니다. 자기 loopback 연결만 해제하고 서버·기존 VM을 보존했습니다. 다른 대상 신원 조회·앱 강제 종료·키 내용/해시 조회는0입니다.

## 실제 경기·복귀·재참가

`Intro31H2`/`Intro31C2`가 `FPSIntro31B`에 참가해 READY/START→Desert House 양쪽 실제 게임 시점을 확인했습니다. Client 한 번 사격은 탄약30→29, R 재장전과 애니메이션 후30을 표시했습니다. Draft261 입력 경계·지속 물리 입력·피해/사망/리스폰 검증으로 합산하지 않습니다.

5분 자연 시간 만료 뒤 **양쪽 DRAW / TIME EXPIRED / RED0·BLUE0 / DONE2행**을 확인했습니다. 각 RETURN TO LOBBY로 돌아온 뒤 Host가 `FPSIntro31C`를 새로 만들고, Client가 새1/2 목록을 받아 재참가했습니다. 같은 두 닉네임이 보존됐습니다. 두 번째 경기는 시작하지 않았으며, 이 재참가를 두 번째 전체 경기 통과로 세지 않습니다.

## 정상 종료와 로그 범위

Client는 새 방 퇴장→로비 EXIT GAME/OK, Host도 자기 방 퇴장→EXIT GAME/OK로 종료했습니다. 확인 버튼 이후 종료 중 창 조회·Alt+F4·kill은0입니다. 실행 직후 별도 watcher가 보유한 동일 PID/startTicks/exe의 native handle에서 다음 실제 정수를 읽었습니다.

| 역할 | 종료 UTC | 실제 ExitCode | 보유 handle | Root 강제 종료 |
| --- | --- | ---: | --- | ---: |
| Client8744 |2026-10-31T00:46:13.9744304Z|0|종료 코드 읽기까지 유지|0|
| Host38900 |2026-10-31T00:46:29.1194156Z|0|종료 코드 읽기까지 유지|0|

후속 자기 payload 프로세스 수는0입니다. 종료 후 flush된 실제 로그는 Host87,264B/Client82,799B이며, 각각 `Crash!!!`·`Fatal error!`·`NullReferenceException` 표식은0입니다. 이는 모든 오류 부재나 #154 원인 해결을 뜻하지 않습니다. 원시 로그는 비공개 위치에 보존하고 Git에 복사하지 않았습니다. 첫 실행의 강제 종료와 날짜 parse guard 실패도 소급 정정하지 않습니다.

## 소스·후속·근거

패키지 고정·출력 adapter의 텍스처 수명/비raycast Canvas·완료 정리·실패 Login 전환·SceneRef 경로 해석과 같은 `-intro1` 복사본의 Lobby/StartGame 전달을 독립 소스 검토했습니다. 필수 코드 문제는 발견되지 않았습니다. [Android 대상 Editor 컴파일](quad-d-intro-android-editor-compile.md)은 별도 bounded 결과이며 Player 컴파일/실행이 아닙니다.

R31은 `D:/codex/validation/sudden-force-fps/20261031-work`입니다. `windows-runtime01/round2-host-start-frames01.public.json`, `windows-runtime01/round2-*`, `round2-version-isolation01.public.json`, `legacy34-scoped-lobby-metadata01.public.json`, `host/client-started02.public.json`, `host/client-exit02.public.json`, `round2-exit-log-summary01.public.json`, `round2-players-after-exit01.public.json`, `own-adb-disconnect02.public.json`과 `intro-final-source-review01/`을 함께 읽습니다. 원장196,2,34와 실제 제공34/release12/source612 QA는 그대로이며 새35 예약·Android AAB/APK·서명·업로드는0입니다.
