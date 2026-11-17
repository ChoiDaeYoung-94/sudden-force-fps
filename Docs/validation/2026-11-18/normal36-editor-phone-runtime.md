# 정상36 폰·일반 Editor의 경기 진입과 닉네임 입력

2026-11-18, 관련 #66/#347/#399/#163/#341/#342. [정상36 로컬 산출물](normal36-local-release.md)의 **일반 Android36 폰과 일반 Editor가 같은 방에 참가해 경기를 시작**했습니다. 폰의 닉네임 입력·방 참가·READY와 Game HUD를 확인했습니다. 수집한 166개 그래프 표본은 모두 idle이므로 실제 달리기·사망/새 life 검수는 미완료입니다.

## 실제 실행 기준

| 항목 | 이번 기준 |
| --- | --- |
| 게임 소스 | P13 `e5a7210f3aa0aeb390015cdd9865626f5e9a6298` / `codex/issue-66-normal-android36` |
| 저장소 문서 main | PR421의 `5581dd6753507559ba42281fa38690a267911360`; P13의 게임 소스 HEAD와 구분 |
| Editor | Unity6000.3.25f1, native36552 / birth `2026-11-17T21:36:46.6755994Z`, GUI/Android/DX11 |
| 실제 컴파일 정체성 | MVID `ba53d34c-6dc3-4b41-8074-c6fe9ce6856e`, Networked43, activeBuildTarget Android·활성 define/Android define의 QA 심볼 부재 |
| 앱 | `com.AeDeong.SuddenForceFPS` / 1.0.199/code36 / APK SHA `36700E65C29C47910C8ACBB700FD0351496AB70AE5F11080C33BB778846ED4F2` |
| 폰 | SM-A175N / 2340×1080, 기존 정상36 UID10351·helper UID10352 및 QA13 유지, 재설치/초기화0 |
| 실제 연결 | room `SF36N354834`, 일반 namespace `sff-q20261022-ladder-v1-intro1-riflecore2-gaitprod1-window1-shoulderref1`, Host runner−5940 |
| 관찰 배우 | 폰 remote actor−9002 / object−9004 / network1032 / input player3 / life0; Host local actor1031/player2와 구분 |

재사용 Identity 본문의 Standalone 심볼 판정만으로 Android를 수용하지 않았습니다. 별도 `android-active-pre.result.json`에서 실제 활성 타깃·define·Networked43·MVID를 읽었습니다. 폰의 private namespace 값을 진단 API로 읽은 것은 아니며, 같은 방의 실제 두 피어 Game 연결로 호환 경로를 확인했습니다.

## 확인한 동작

1. 정상36 폰을 데이터 초기화 없이 재실행하고 PGS 뒤 닉네임 창에 **SF36PHONE**을 Android 입력으로 입력했습니다. 키보드 확인과 게임의 OK를 거쳐 팝업이 닫혔습니다.
2. Editor는 정상 UI callback으로 전용 방을 만들었습니다. 폰에서 실제 방 목록의 `SF36N354834`/1 of 2를 보고 참가했습니다. roster의 기존 Editor 이름과 SF36PHONE, 폰 READY를 확인했습니다.
3. Editor의 정상 START callback 뒤 두 피어가 Game으로 진입했습니다. 폰 원본 PNG의 HP100·탄약30/30·0:0·진행 중 타이머와 1인칭 총/팔·터치 HUD를 확인했습니다.
4. Editor의 actual binding에서 match1030/−7778, 카메라−8286, remote 배우1032/life0/HP100을 고정했습니다. 다른 배우의 과거 사망 자료를 이번 배우의 생명 검수로 합산하지 않습니다.

이번 Editor 방 생성/START는 UI callback의 관리 호출이며 물리 클릭이 아닙니다. 폰의 입력은 Android native 입력입니다. 한 경기 진입이며 사격 명중·재장전·정상 결과 RETURN·전체 보행·손 접촉 수용으로 확대하지 않습니다.

## 이동 입력과 166행의 한계

기존 helper36으로 두 손가락 이동+SPRINT 위치의 native28 events와 추가 hold2000ms를 실행했습니다. 응답의 `injectedAll/activePointersCleared/sessionFinishRequested=true`, `errorCode=0`을 확인했습니다. 입력 API의 성공은 게임이 그 입력을 해석했다는 단독 증거가 아닙니다.

별도 endCameraRendering sampler는 **166행/aligned166/gap0/초기·최종 fault0**입니다. 실제 mixer9와 clip time·동일 배우/권위/current-cached context를 읽었으나 모든 행은 **속도0·Sprint=false·step0·idle weights·HP100/life0**입니다. run/death/newlife 수용은0이며, 입력 주입의 절대 시각과 렌더 tick 대응을 직접 결속하지 못했습니다. 준비 지연과 10초 표본 창의 미정렬은 가능성으로 남기고 제품의 달리기 실패 원인으로 확정하지 않습니다.

원본 10초/200행 후보와 실제 166행은 보존했습니다. 후속 `qa13-run-life-next02/r02`는 최대30초/600행·20Hz로만 보완한 비활성 후보이며 독립 소스 검토를 마쳤습니다. helper가 반환하면 즉시 Stop하고 30초를 의무로 기다리지 않습니다. 이 후속 후보의 실제 실행은0입니다.

helper 동작 본문은 끝났지만 활성 앱의 `user_rotation`이0→1이어서 **해당 operation은 complete=false/SETTINGS_OR_IME_CHANGED**입니다. IME·다른 설정3개는 동일했습니다. 원 실패를 통과로 바꾸지 않았습니다. 별도 finish에서는 user_rotation/화면 회전 모두0, QA13 whole APK/UID/최초 설치 시각·설정/IME/awake·normal game/helper PID0·동일 ADB를 확인했습니다.

## 냉시작의 한정 확인

별도 새 폰 범위에서 동일 APK/UID를 채택해 완전 종료 뒤 다시 실행했습니다. **닉네임 재입력 창 없이 PUBLIC ROOMS로 도달**했습니다. 이 재시작 뒤 정확한 저장 문자열을 읽거나 room roster를 다시 보지는 않았으므로 SF36PHONE 문자열의 완전한 저장성 검수로 확대하지 않습니다.

이후 방 생성 화면은 열었지만 방 이름 text 요청이 stale observation 검사에서 입력 전 거부됐습니다. 방 생성·1인 START 차단은 미실행입니다. 소유 앱 정리 후 별도 finish에서 원상태와 PID0/동일 ADB를 확인했습니다. 이 두 번째 범위에서도 재설치·데이터 초기화는0입니다.

## 반환과 원 실패 보존

- sampler 두 callback을 제거하고 파일 쓰기/표본 실패 없음을 확인했습니다. 실제 run 수용 여부와 callback 반환을 구분합니다.
- Editor는 Play 중지·clean empty scene 뒤 queued exit 응답을 받았으나 종료가 지연됐습니다. 별도 직접 Exit 요청 뒤 **native Exit0/forcedfalse·watcher CLI0·Prefs1,1/SDK0·동일 ADB**를 확인했습니다. 직접 Exit eval의 CLI6는 따로 보존했습니다.
- 첫 소스 감사는 관측본의 긴 경로 생성에서 WinError206으로 실패했습니다. 짧은 경로의 별도 flat 감사로 재수집했고 시스템 LongPaths 설정은 바꾸지 않았습니다.
- 공개 입력18개 발생본을 보존했습니다. 독립 분류는 **EOL15 + EOL/빈 값 공백1 + 실제 값/코드2**입니다. 실제 변경은 Gradle의 portable Maven 탐색→P13 절대 URL, Resolver `projectExportEnabled False→True`입니다. 이 관측을 새 export 실행의 근거로 삼지 않습니다.
- CAS 복원 후 **공개4134 raw exact/private6 indexS/Git clean**, P2 원장 `199,1,36`/P13 사본 `196,2,34`를 확인했습니다. **06:54:51 실제 전체 반환**은 원 full06:55:42 UTC/QPC 안이며 이전 실패를 수정하지 않았습니다. 이번 범위의 새 빌드·서명·업로드는0입니다.

5분 만료를 기다리지 않았습니다. 이번 새 경기의 10초 match-timer fixture는 미실행이며, 폰 소유 앱 종료와 Editor Play/프로세스 종료를 정상 결과/RETURN 검수로 합산하지 않습니다.

## 공개 로컬 근거

기준 경로는 `D:/codex/validation/sudden-force-fps/20261117-night/`입니다. 계정 표시명이 담긴 시작 이미지는 저장소에 첨부하지 않았습니다.

| 범위 | 경로 |
| --- | --- |
| Editor 실제 정체성/방/배우/종료 | `normal36-editor-phone-actual01/actual-354834291f6f478f9573ab210ff153ef/`의 각 result·`exit.public.json`·`full-return.public.json` |
| 폰 닉네임·방·Game/최종 반환 | `normal36-phone-actual02/run-8ba7ba61de424f21b5de8cdbe2afd58d/`의 원 PNG와 `op-5ac3b9999ccf495ba3cb0747640ac194/result.public.json` |
| helper 원 감사 실패 | 위 폰 run의 `op-710d8dca8a7d498a8c2fbde2db71ab71/result.public.json` |
| actual166행 | `qa13-run-life-next01/run-life-af1b353d69b642dcbbfbd27802fb92f3.public.json` |
| 별도 냉시작/반환 | `normal36-phone-actual02/run-f134c24192ed4b0592ffbcbce9bb0120/`의 원 PNG와 `op-01532455d7e64a1bb3c6f01bd9c8121d/result.public.json` |
| Runtime 독립 판독 | `normal36-editor-phone-runtime-independent01/REPORT.ko.md` |
| 소스 차이 독립 분류 | `normal36-runtime-source-delta-independent01/REPORT.ko.md` |
| 실제 소스 복원 | `D:/codex/a36/runtime-return01/source-restored.public.json` · SHA `09C9D2C04A81B4E776AEF3056A3574A7F96E4D571941CD70FB54A4618EBA2B45` |

다음은 같은 배우의 actual run weight를 입력 구간과 결속하는 짧은 재검증, 같은 배우 death/respawn·반복/중단 재장전·전방위 파지/접지, LD PGS/종료, Play 테스터 설치입니다. CI/CD 확장·장기 성능은 보류/후순위 기준을 유지합니다.
