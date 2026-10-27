# Windows 검증 Player

## 방 채팅 실제 두 Player 검증 · 2026-10-27 (#163)

기존 Windows05 360파일을 실행 전 대조한 두 Player에서 초안 포커스 이탈 미전송·Enter/SEND 양방향 단1회 표시·빈값/공백 미전송·160단위 표시·재참가 후 채팅을 확인했습니다. 정상 종료는 보유 handle ExitCode0/고정 Crash 문자열0이었으며 #154 원인 해결이나 Android/IME 통과는 아닙니다. 빌드 출처·화면 관측의 한계와 근거는 [상세 기록](Docs/validation/2026-10-27/remote-chat-windows.md)을 따릅니다. 아래 과거 기록을 보존합니다.

## 종료 재시험 최신 상태 · 2026-10-21 (#154)

**종료 문제는 미해결입니다.** 기존 W3 Windows64/Mono/Development1.0.194(source `1d7ca5f6e143aa9bccb17fbd9837f815278f122c`)와 같은359개 payload/267,928,697B를 전수 SHA 대조해 경로·bytes 변경/추가/누락0을 확인한 뒤 재시험했습니다. 현재 Android28이나 최신 제품 전체 실행 결과로 합산하지 않습니다.

최초 W3 실행에서는 종료 후 창 조회/AltF4와 겹친 client `Crash!!!`1건·UIAutomationCore/WindowPattern 관련 스택을 관측했습니다. 후속 repro02는 양쪽1280×720의5분 경기·결과·복귀·재참가 뒤 종료 요청 후 창 조회/AltF4 없이 양쪽 exit0/선택 Crash 문자열0이었습니다. 창 크기 등 조건이 달라졌으므로 원인을 특정하거나 최초 실패를 해결한 것으로 처리하지 않았습니다.

이번 repro03은 원래 크기(host1728×729/client1280×720)로 실제 방 참가·READY/START·양쪽 사격/재장전·5분 DRAW·양쪽 결과/로비 복귀·새 방 재참가를 확인했습니다. 재참가 도중 Loading을 저장한 사진은 완료 증거에서 제외하고 후속 실제 완료 화면을 구분했습니다.

| 종료 단계 | 실제 결과 | 판정 한계 |
| --- | --- | --- |
| client 첫 EXIT/확인 입력 뒤 대기 | 208.818초 동안 창 조회/AltF4 없이 프로세스만 기다렸으나 남아 있었음 | 실제 OK onClick/Application.Quit 도달은 미확인 |
| 별도 recovery | 이후 창 조회와2입력, PopupExit 부재/Lobby 표시만 확인 | 최초 무창조회 대기와 별도; 배경/Cancel도 Quit 없이 팝업을 닫을 수 있음 |
| 최종 종료 | host exit0, 정확한 소유 client만 강제 종료한 exit-1 | 양쪽 정상 종료 PASS 아님 |

닫힌 로그의 고정 `Crash!!!`·`Exception` 문자열은 양쪽0이며 선택 패턴 범위입니다. client `code:104`2회/`LeaveRequest`3회는 별도 관측이고 전체 오류0·사건 수·새 native crash 부재 전수 증명이 아닙니다. 이번0건은 최초 crash1건을 상쇄하지 않습니다. 실제 OK 도달·종료 취소·SDK/native teardown·UI 도구 영향 중 특정 원인을 확정하지 않습니다. serialized OK target/script/localID/CallState를 읽은 범위에서는 정적 결함을 찾지 못했지만 실제 콜백 실행 증거는 아닙니다.

fixture `exit.public.json`의 `forcedTermination:false`는 잘못된 고정값입니다. 원본을 보존하고 root cleanup의 true·host0/client-1과 `EXIT_FIXTURE_CORRECTION.public.json`으로 정정했습니다. `.png` 이름의 저장사진17개도 실제 JPEG/JFIF 바이트이며 원본 이름·bytes를 보존하고 문서에서 포맷을 정정했습니다.

독립 검토 근거는 외부 `20261021-windows-exit-crash-01/repro-03-original-window-sizes/independent-static-exit-path-review-01/FINAL_REVIEW.ko.md`, `FINAL_REVIEW.public.json`, `EXIT_FIXTURE_CORRECTION.public.json`, `final-bounded-verification.public.json`입니다. 원시 로그·Player·사진은 Git에 올리지 않습니다. 다음 PC 후보는 별도 진단 빌드의 OK handler/Quit/quitting 단계 관측이며 아직 구현·빌드·실행하지 않았습니다. 사용자 조치·질문은 없습니다. 아래는 각 시점의 기록입니다.
## 새 체크아웃 빌드 · 2026-10-21 (#147)

main `ef51c22da32f23cfe6fe382e91b28158a95e4c80`에서 별도 Git worktree를 만들고, 기존 서명 키 경로 한 개는 sparse 규칙으로 처음부터 제외했습니다. 공개 tracked 입력3,926개·실행 전 clean 상태와 새 Library를 기준으로 Unity6000.3.25f1의 기존 `MultiplayerValidationBuild.BuildFromCommandLine`을 실행했습니다. 키 내용 조회·해시·복사, Android 예약/서명은 하지 않았습니다.

실제 BuildReport는 **StandaloneWindows64 / Mono / Development / Succeeded / 오류0 / 경고5 / 58.187초**입니다. manifest를 제외한 Player360개 전체의 경로집합과 SHA256이 일치합니다. main PlayerSettings에 따른 표시 버전은 **1.0.194**이며 Android 제공본1.0.195/코드27과 다른 산출물입니다. PDB/Burst 디버그 정보를 포함한 개발용 검증 빌드이며 배포용 Windows Release 판정은 아닙니다.

경고5건은 로그 패턴상 Unity Services 프로젝트 미연결1·Pipeline RuntimeConfig 없음/OFF1·UniRx의 obsolete API3에 대응합니다. 초기 임포트 진단과 반복 출력은 별도이고 전체 Editor 세션 경고0으로 표현하지 않습니다. Runtime Pipeline 서버나 hotreload 키 생성을 활성화하지 않았습니다.

임포트/빌드 후 공개 입력3,926개를 다시 해시한 결과 누락0·변경9·나머지3,917개 exact였습니다. 변경9개는7개 EOL만, 자체CJK/EDM2개는 EOL과 빈 YAML 값 뒤 공백뿐이며 값 변화는 없었습니다. 생성된 바이트와 독립 검토를 외부에 보존한 뒤, 닫힌 batch Editor의 이번 체크아웃9개만 정확한 실행 전 SHA256 바이트로 복원해 Git clean을 확인했습니다. 기본 P2의 기존 미커밋 설정4개는 변경하지 않았습니다.

근거는 외부 `20261021-clean-windows-build-01/`의 `checkout-preflight.json`, `source-before.json`, `Player/validation-build.json`, `post-build-validation.json`, `independent-build-review/`, `generated-source-preservation/`, `generated-format-restoration.json`입니다. 원시 로그·개발 Player·개인키는 Git에 추가하지 않습니다.

## 실제 시작 시험과 한계

같은360개 파일을 다시 해시한 뒤 별도 Player의 batch 실행에서 Photon `kr` 로비 접속과 첫 빈 목록 수신을 확인했습니다. 선택한6종 오류 패턴은0입니다. 관측 뒤 정확한 자식 프로세스를 종료해 exit1이므로 정상 종료 통과나 crash 판정으로 합산하지 않습니다. 기존 PlayerPrefs를 삭제하거나 초기화하지 않았습니다.

후속으로 별도 Windows Player 두 창을 실제 Computer Use로 조작했습니다. 각 창을 반환된 Window 객체로 구분하고 닉네임→실제 맵 선택→방 생성/참가→서로 다른 팀의 READY/START→양쪽 경기 HUD를 확인했습니다. 클라이언트 사격30→29·R 재장전→30·시점 조작과 W 짧은 입력, MENU/복귀 확인창/CANCEL,5분 시간 만료 후 양쪽 DRAW·0/0 결과를 관찰했습니다. 양쪽 로비 복귀 뒤 새 방 생성/재참가·닉네임 보존·퇴장도 확인했습니다. 고지 패널은1/200→2/200 이동·닫기만 확인했으며 전문200페이지 시각 검증은 아닙니다. 마지막에는 각 앱의 EXIT GAME/OK를 요청했고 두 PID 모두 없어졌습니다. 이 두 실행의 정확한 exit code는 수집하지 않았습니다.

실제 화면23장과 닫힌 뒤의 두 로그는 `visible-qa-final.json`으로 기록했습니다. 두 로그 각각 로비 연결3회/방 시작2회·선택한6종 오류와 Ellipsis 패턴0이며 전체 오류 부재 판정은 아닙니다. 결과 표의 열 제목과 실제 행 위치 어긋남은 새 #148에서 추적합니다. 헤드/몸 데미지·사망/리스폰·동시 누름/해제·held TAB 점수판은 이번 Windows 시험에서 확인하지 않았고, #41/#132 전체 완료로 처리하지 않습니다. 런타임 서버/eval 주입·PlayerPrefs 초기화·방화벽/권한 변경은 하지 않았습니다.

이 결과는 Android clean export/AAB 재현, Unity 단일 AAB 호출 안정화, GitHub Unity 빌드 CI, 실제16KB 기기, 최신 Android 로그인·경기 통과가 아닙니다. Android 코드27 AAB/APK·심볼과 원장195,8,27은 유지합니다. 실기기와 장시간 성능 검증은 기존 후순위 합의를 따릅니다.
