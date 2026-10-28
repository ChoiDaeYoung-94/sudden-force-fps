# 코드34 Android FIRE+LOOK 세 입력 구간의 실제 검증

2026-10-28 Play 코드34/1.0.196에서 단일 FIRE+LOOK과 이동·사격 두 손가락의 서로 다른 해제 순서를 실제 Android native 입력으로 확인했습니다.
기존 [전경 guard 중단 표본](android34-runtime-partial.md)은 보존하고, 새 준비 대기·명시 실행 구간의 화면·recorder·서버 상태를 대조했습니다. 세 모드 한정 독립 검토는 noBlockers입니다.
[#259](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/259)·[#41](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/41)의 부분 근거이며 [PR261](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/261) 전체 수용·병합 완료를 뜻하지 않습니다.

## 대상과 소스

- 대상은 LDPlayer14의 FPS/index2·emulator-5558·Android14/API34입니다. 실제 휴대전화 시험으로 분류하지 않습니다.
- 게임은 source `61218033ff36a2da3770ad93b9cc6c10c105c92d`의 입력 QA/release12입니다. 후속 PR282/287/288/290은 이 APK에 미포함이며 최신 main 전체 통합 빌드가 아닙니다.
- P11 호스트는 기존 HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`에 명시한 후속 C# overlay·QA 구성을 담은 준비 기준입니다. 동일 host PID42004/start05:56:06.962709UTC·Game MVID030e720d…4c72를 세 모드에서 대조했습니다.
- 정상 UI로 두 차례 Game에 진입했습니다. Android PID2590에서 앞 두 모드를, 정상 앱 종료·재실행 뒤 새 PID3394에서 셋째 모드를 수행했습니다. 각 모드의 host는 running/server/Game·activePeers2이며 상태 쓰기는0입니다.
- 입력 helper는 시작 때만 제한된 target-root 준비 대기를 추가하고 각 MotionEvent 직전의 엄격한 전경 검사를 유지했습니다. 다른 패키지 관측false·error0·injectedAll/activePointersCleared=true, cleanup Cancel 전송0입니다. guard 우회·게임 상태 직접 주입으로 처리하지 않습니다.

## 모드별 실제 결과

| 모드 | native 주입·준비 | recorder·서버에서 확인한 범위 |
| --- | --- | --- |
| 단일 FIRE+LOOK |14events·ready203ms(2회 관측)|pointer0 capture2→Moved4→Ended/release29. 탄약30→22/shot0→8, yaw180→172.6875·pitch0→−5.0625 |
| 두 손가락, FIRE 먼저 해제 |28events·ready3ms(1회)|pointer1 capture48→2finger Moved50→release84. 탄약22→14/shot8→16, 수평4.3999m·yaw172.6875→165.375·pitch−5.0625→−10.125 |
| 두 손가락, 이동 먼저 해제 |28events·ready203ms(2회)|pointer1 capture20→2finger Moved22. seq46 id0 Ended 뒤47–55에서tc1/id1 capture·held·look 유지,48/50/54 Moved→seq56 id1 Ended→57 tc0/capture0/held0/look−1. 탄약30→19/shot0→11·수평3.8943m |

셋째는 FIRE drag가 원 좌표로 복귀하므로 최종 yaw180/pitch0이 시작값과 같습니다. 동작 중 서버 각도 표본은 없으며 이 전후값만으로 회전 누락이나 셋째 회전 실측 성공을 단정하지 않습니다. 실제 서버 각도 변화 근거는 앞 두 모드입니다.
셋째의 parser single/two true는 같은 pointer1 segment가2touch→1touch를 거친 결과이며, 단일 모드를 추가로 한 번 더 수행한 횟수로 세지 않습니다.
원본 JPG5장도 직접 대조했습니다. 첫 실행30→22→14, 새 실행30→19탄 표시와 구도·위치 변화는 서버 관측과 양립합니다. 정밀 궤적·조이스틱 vector·각 발의 지연·명중/피해를 증명하는 사진으로 확대하지 않습니다.

## 시계열·recorder 예산과 판정 범위

독립 검토는 첫 PID의 전체87행/prefix, 둘째 PID의 전체57행을 먼저 검증한 뒤 각 모드의 **포함 범위** seq1–29·29–84·1–57을 잘랐습니다. 공유 경계29는 한 행의 문맥이며 모든 행을 새 사건으로 중복 합산하지 않습니다.
segment 안의 actor/life/source/runner/control/pointer 연속성과 capture→Moved→Ended→clear를 확인했습니다. 서로 다른 PID의 seq·actor 숫자가 같아도 하나의 연속 생명주기로 합치지 않았습니다.
첫 trace87행의 마지막 reason5는 context end이며, 이미 손을 뗀 뒤 Menu/퇴장이므로 held 상태의 MENU 수용이 아닙니다.
새 trace의 최종58행은 앞57행과 정확한 prefix이고 reason6은120초 recorder Timeout입니다. 세 번째 제스처는06:06:29UTC에57행까지 이미 완료됐습니다. Timeout을 제스처 종료 시각·실행 실패·128행 초과로 바꾸지 않습니다.
128행 예산은 정상 앱 종료·재실행으로 분리했고 기기 로그 clear·앱 데이터 삭제는 하지 않았습니다. parser의 자동 nativeAcceptance=false는 보존하며 Root가 화면·실행 영수증·서버와 결합한 좁은 판정과 구분합니다.

## 정상 종료와 공개 입력 보존

기기 공개 종료 영수증은06:08:45.536951UTC의 정상 GUI EXIT와 game/helper pidof exit1·부재bool, forceStop/dataClear=false를 기록합니다. 보조가 새 기기를 조회한 결과는 아닙니다. 첫 PID2590 부재는 Root scoped 관측 전사 범위입니다.
P11 Play 정상 중지 요청은 진단0이고, 동일 보유 process handle로06:09:55.374609UTC 실제 ExitCode0을 확인했습니다. 강제 종료·Save·종료 뒤 창 조회는0입니다.
P11 종료 전후 공개3,984개는 준비 기준과 각각3,984개 raw exact·변경/누락0이며 private6은 부재/indexS입니다. private 내용·해시를 읽지 않았습니다. 전체 메모리/cache/dirty 불변이나 최신 main clean 빌드의 증명은 아닙니다.

## 한계·다음 판단과 로컬 APK

세 모드의 에뮬레이터 capture/move/release·화면·서버 상관 범위입니다. held MENU·사망·리스폰/rebind의 native 경계, 실제 휴대전화, 전체 경기/UX·성능·실청취·16KB 실행·전체 권리는 통과하지 않았습니다.
PR261 판단은 기존 소스/합성 근거와 이번 native 결과·미완료 경계를 독립 수용 matrix로 결합한 뒤 Root가 결정합니다. 이 문서가 Draft 해제·병합을 미리 승인하지 않습니다.
후속 최신 main Android 통합은 별도 source·검증과35이상의 새 예약이 필요하며 현재35 예약은0입니다.
로컬 게임 APK: `D:/codex/releases/sudden-force-fps/1.0.196-code34-input261-qa/SuddenForceFPS-1.0.196-code34-Input261-QA-PlaySigned.apk`.
105,690,443B/SHA256 `09403527CFC04B74B85942BB7DFAFB0E727FA30BB0B8660AB2EB010E116E7F76`입니다. [공식 Play 검증](play34-internal-delivery.md)과 연결하며 APK 원격 업로드는0입니다.

## 공개 근거

기준은 `D:/codex/validation/sudden-force-fps/20261028-work/resumed-1448/`입니다. 각 모드의 `native-actual01/02/*-result.public.json`·instrumentation·전후 server snapshot·tag 출력, 원본 JPG5장을 보존합니다.
실제 독립 검토는 `native-input-actual-independent-review02/verification.public.json`·`COMMENT.ko.md`이며, 최종 기기 정리는 별도 `cleanup-supplement01.public.json`·`CLEANUP-SUPPLEMENT01.ko.md`입니다. 당시 pending 기록을 덮어쓰지 않았습니다.
종료는 `device-normal-exit01.public.json`·`p11-normal-close01.public.json`, 공개 입력은 상위 R28의 `p11-rootready-before-close01.public.json`·`p11-rootready-after-close01.public.json`입니다. 전용 source 핀은 외부 `native-input-runtime-doc-candidate01/source-pin-manifest.public.json`에 있습니다.
이 문서 작성자의 새 Editor/GUI/ADB/SDK/키·제품·Git/GitHub 변경은0입니다. 다음은 Root의 독립 matrix와 남은 native 경계 판단이며 현재 필요한 사용자 조치·질문은 없습니다.
