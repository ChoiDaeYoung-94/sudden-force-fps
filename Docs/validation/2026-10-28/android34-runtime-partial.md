# 코드34 Android 연결·경기 진입과 입력 검증 중단

2026-10-28 Play 코드34/1.0.196을 지정된 LDPlayer14 FPS 인스턴스에서 실행해 정상 로그인·방 참가·양 팀 READY·START와 두 피어의 경기 진입을 확인했습니다.
자동 입력 helper는 전경 보호 검사에서 중단됐고, 필요한 입력 경계 수용은 완료하지 못했습니다. 실패·미실행을 통과에 합산하지 않으며 [Draft261](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/261)을 유지합니다.
이 기록은 [#259](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/259)·[#41](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/41)·[#43](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/43)의 부분 근거입니다.

## 설치 대상과 소스

- 대상은 **LDPlayer14 FPS/index2·emulator-5558·Android14/API34**입니다. 보고된 모델 PJJ110을 실제 휴대전화로 분류하지 않습니다.
- 기존33의 Play 인증서와 같은 `01B2846B…100D86`을 확인한 뒤 코드34를 `-r`로 업데이트했습니다. 설치 child 종료0이며 uninstall/data-clear 명령은0입니다. 앱 데이터 전체의 해시 보존을 검증했다는 뜻은 아닙니다.
- 설치 APK는 [공식 Play 다운로드 검증](play34-internal-delivery.md)의 `SuddenForceFPS-1.0.196-code34-Input261-QA-PlaySigned.apk`입니다. 105,690,443B/SHA256 `09403527CFC04B74B85942BB7DFAFB0E727FA30BB0B8660AB2EB010E116E7F76`이며 APK 원격 업로드는0입니다.
- 실제 빌드 source는 `61218033ff36a2da3770ad93b9cc6c10c105c92d`의 입력 QA 후보/release12입니다. [산출물 기록](../2026-10-27/android34-input-qa-artifacts.md)을 따릅니다.
- PC 호스트 P11은 기존 HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`을 바탕으로 후속 C# overlay와 QA 구성을 포함한 공개3,984개 준비 기준의 Editor입니다. 최신 main `10675…` 전체의 새 빌드가 아닙니다.
- P11의 실제 Photon 접속 버전과 P11/P12/P2 공개 설정의 AppVersion 문자열은 같았습니다. P11/P12 설정 raw SHA도 같고 P2는 줄바꿈 정규화 뒤 같았습니다. 이 설정 대조만으로 Android 실행 성공을 판정하지 않았고 실제 연결 관측과 구분합니다.

## 직접 확인한 실행 범위

Root가 정상 로그인 경로와 닉네임 `qa34`로 방에 참가했습니다. host snapshot03은 양 팀 중 한쪽 미READY,04는 두 플레이어 모두 READY이며, 정상 START 뒤05는 DesertHouse/Game의 server 실행을 기록합니다.
별도 `p11-native34-match-players02.public.json`은 Game/running/server·activePeers2, local/remote 아바타2개와 각각의 중첩 x/y/z 위치를 기록합니다.
Android 화면도 경기 HUD를 표시했고 QA 태그는 android/mobile/game/local/running과 소유 연결 조건을 확인했습니다. 두 피어 경기 진입 근거이며 전체 경기·피해·리스폰·종료/재참가 수용은 아닙니다.
첫 match-players01의 Vector3는 직렬화 fallback 문자열이어서 아바타 좌표 근거에서 제외했습니다. 중첩 수치를 제공한02와 구분합니다.

| 항목 | 실제 관측 | 수용 경계 |
| --- | --- | --- |
| 자동 입력 시도 | helper errorCode4/events0·injectedAll=false·activePointersCleared=true | 전경 guard 안전 중단. 제스처 성공0이며 제품 결함 원인을 확정하지 않음 |
| 중단 뒤 전경 | Root CUA와 WMS 관측은 게임 전경 | guard 검사와 같은 순간의 자료가 아니므로 실패 원인·오검출을 단정하지 않음 |
| QA recorder | av/rd2047·owner 조건 true인 시작/120초 Timeout의2행 | 단일/두 손가락 capture·이동·해제 완료 구간0. 계측 연결과 입력 수용을 구분 |
| 수동 빠른 drag | Root CUA 동작 뒤 화면 탄약30→29 | recorder 기간 밖의 별도 표본. 같은 손가락 사격+시점 회전·정상 해제 통과가 아님 |

두 공개 JPG에서도 탄약30/30과29/30을 각각 확인할 수 있지만, 이 정지 화면만으로 시점 회전이나 동시 손가락 시계열을 입증하지 않습니다.
recorder 분석은 singleFinger/twoFinger completed segment·Ended/Cancelled/menu clear와 native acceptance를 모두 false로 남겼습니다. Timeout을 정상 시나리오 완료로 바꾸지 않습니다.

## PC 종료와 입력 보존

PC Play 정상 중지 요청은 진단0·종료0으로 반환됐고, 이어 동일 보유 process handle로 P11 PID57252의 실제 ExitCode0을 확인했습니다. 강제 종료·Save·종료 후 창 조회는0입니다.
종료 전/후 공개3,984개는 각각 준비 기준과3,984개 raw exact이며 변경·누락0, private6은 부재/indexS입니다. private 내용·해시는 읽지 않았습니다.
이 비교는 명시 overlay를 포함한 준비 기준의 파일 보존이며 최신 main 전체·모든 메모리/dirty/cache 불변 또는 Android 앱 데이터 전체 검증이 아닙니다.

## 미완료와 다음 단계

먼저 새 읽기 전용 진단으로 helper 전경 guard의 관측 경로·시점·권한 범위를 분리합니다. 이후 같은 손가락 FIRE+LOOK, 이동+사격의 서로 다른 해제 순서, MENU/사망/rebind와 복귀를 실제 Android 입력·화면·recorder의 같은 구간으로 대조해야 합니다.
진단 준비·설치·두 피어 진입·탄약 감소만으로 이 경계를 통과시키지 않으며 수용 전 Draft261을 유지합니다.
실제 휴대전화·16KB 환경·장시간 성능·실제 음향 지연·전체 권리 검증은 미완료입니다. 에뮬레이터 API34 표본을 이들 결과로 확대하지 않습니다.
PR282/287/288/290의 후속 제품 수정은 코드34 APK에 미포함입니다. 후속 통합은 새 입력과 버전35이상의 별도 작업이며 이번 기록을 위해 새 빌드/예약하지 않습니다.

## 공개 근거

기준 폴더는 `D:/codex/validation/sudden-force-fps/20261028-work/`입니다. 이 문서 작성은 기존 공개 영수증·JSON·지정 instrumentation/recorder 출력과 JPG 대조이며 새 Editor/GUI/ADB/SDK 실행은0입니다.

- 설치/인증서: `native34-target-installed01.public.json`, `native34-device-identity01/result02.public.json`.
- 연결/아바타: `p11-native34-host-snapshot03/04/05.public.json`, `p11-native34-match-players02.public.json`, `photon-public-version-comparison01.public.json`.
- 입력/한계: `native34-actual-modes01/single.instrumentation.txt`, `recorder-after-manual-drag01.txt`, `recorder-evidence01.public.json`과 `native34-game-after-guard01.jpg`·`native34-game-manual-drag01.jpg`.
- 종료/보존: `p11-native34-stop-play01.public.json`, `p11-native34-normal-close01.public.json`, `p11-native34-before-close01.public.json`, `p11-native34-after-close01.public.json`.
- 문서 후보의 전용 source 핀은 외부 `native34-runtime-report-candidate01/source-pin-manifest.public.json`에 보존합니다. 이번 문서가 원본 증거·실패·기존 기록을 대체하지 않습니다.

완료 범위는 코드34 에뮬레이터 업데이트·정상 연결/경기 진입과 PC 정상 종료의 부분 기록입니다. 다음은 입력 guard 읽기 진단과 미완료 경계 수용입니다. 현재 필요한 사용자 조치·질문은 없습니다.
