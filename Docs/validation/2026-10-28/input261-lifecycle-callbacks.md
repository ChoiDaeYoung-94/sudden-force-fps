# PR261 L1: held FIRE/LOOK의 Focus·Pause component callback 검증

2026-10-28 정상 두 피어 Game의 실제 local owner에 연결한 합성 component fixture로 **4시나리오·30검사항목**과 즉시 복구·정상 종료를 확인했습니다.
최종 독립 보충 검토는 noBlockers=true/cleanupPending=false입니다. 초기 cleanupPending=true IR은 원문 그대로 보존합니다.
[#259](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/259)·[PR261](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/261)의 부분 근거이며, 실제 OS focus/pause 전이·Android native touch·사망/리스폰(L2)·전체 PR261 수용은 아닙니다.

## 대상·소스와 실행 방식

- P11은 기존 HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`에 기존 C# overlay와 PR261 input2를 명시 적용한 Windows Editor 준비 기준입니다. 최신 main 전체의 clean build가 아닙니다.
- 실제 PID34276/start`2026-10-28T07:21:43.6106979Z`, Game MVID`db62160d-f13a-41b2-b222-f685dc73a32b`, GUI/Direct3D11·Game/server/running·activePeers2를 대조했습니다. 입력 직전 actualFocused=true/HP100/ammo30/shot0/life0·idle입니다.
- PR261 head`aa56f389`의 input2 blob은 기존`61218033`과 같다는 Root/독립 검토 근거를 사용합니다. CRLF raw SHA F878…6777/E72E…3B67과 LF 정규화 SHA D4CA…73B7/D99C…137A를 구분합니다. author의 새 Git 조회나 최신 main 통합 검증은 없습니다.
- 실제 설정은 sceneHandle을 JSON Integer `-5858`로 고정했습니다. 이전 `{}` 직렬화 shape 오류와 focus=false/actions0의 actual03/04는 보존하며 성공으로 합산하지 않습니다. 당시 focus 상실 원인은 확정하지 않았습니다.
- frozen body를 eval에서 컴파일한 뒤 일회성 Editor.update callback으로 호출했습니다. OS focus flag·owner validity·HP/life/private 제품 필드·static Instance의 reflection setter는 쓰지 않았습니다. 정상 공개 Bind/Unbind와 합성 Pointer/Touch·자기 Source/Control의 Focus/Pause callback을 한정 사용했습니다.

## 실제 결과와 복구

| 시나리오 | callback 순서 | 통과 검사 |
| --- | --- | ---: |
| Focus 상실·복귀 | Source→Control | 7 |
| Focus 상실·복귀 | Control→Source | 7 |
| Pause·복귀 | Source→Control | 8 |
| Pause·복귀 | Control→Source | 8 |

합계는30검사항목이며 callback 호출 횟수가30이라는 뜻은 아닙니다. held FIRE pointer0의 Moved 뒤 callback reset으로 capture/look/fire 정리·physical held/rearm gate 유지, stale Moved 거절, 복귀 뒤 held 재사용 차단, fresh Down 재무장와 Up 정리를 확인했습니다. Pause에서는 실제 해당 component의 pause field가 capture/Snapshot을 차단하는 검사도 포함합니다.
결과는 complete=true/errors0이며 원 Source 공개 재결합과21개 필드·UI hit list identity, global handlers, owner HP/ammo/shot/life·tick/frame·world/scene/cursor·객체 수·input2 바이트가 same입니다. 자기 임시 객체2개를 제거하고 원 Instance/Presentation/참조를 복구했습니다. 본문 내부 frame6714/tick11242·life0/HP100/ammo30/shot0이 보존됐습니다.
이것은 한 synchronous component callback fixture의 경계이며, 실제 Android touch나 OS 전이를 발생시킨 시험이 아닙니다. body 결과의 osFocusOrPauseTransitionTested/nativeTouchOrPhoneTested/deathOrRespawnTested=false를 유지합니다.

## dispatch의 수용 범위

queue receipt는 READY/bodyInvoked=true/ticks1이며 자기 handler ownAbsent=true, update handler 목록29→29와 hash 동일을 기록합니다. queued07:26:22.6301506Z→finished07:26:22.7469108Z, 약116.76ms입니다.
본문 완료는 CLI 응답07:26:23.369531Z보다 앞입니다. **eval 동기 코드의 후행 callback**으로 실행됐지만 클라이언트가 응답을 받은 뒤 실행된다는 보장은 아닙니다. queue 등록 응답의 QUEUED_NOT_BODY_PASS와 별도 실제 body result를 구분합니다.
29개 handler 복구는 본문 실행 직후의 관측입니다. 이후 active callback/InputGetter를 새로 조회했다는 뜻이 아니며, 해당 runtime 종료는 별도 held process handle로 확인했습니다.
실행 당시 body/queue config 원문은 dispatch-attempt01의 동결 actual copy를 근거로 합니다. 당시 true 승인값은 보존하되07:28:03 TTL 만료·consumed marker·해당 epoch 종료와 함께 읽으며 새 실행 권한으로 재사용하지 않습니다. 현재 mutable config leaf를 문서 핀으로 사용하지 않습니다.

## 정상 종료·공개 입력 보존

Root의 정상 ReturnToLobby 버튼 호출→StopPlay→저장 없는 clean empty nonPlay scene 전환 뒤 별도 framecap 복원을 확인했습니다. StopPlay의 -1 요청 뒤 실제60이 남은 관측을 보존하고, RestoreFrameCap이60→원 -1로 복원했습니다. 최종 inspector는 동일 epoch/nonPlay/settled·scene1/empty/dirtyfalse/roots0·preview0/framecap-1입니다.
동일 held process handle은07:40:56.762609Z 실제 ExitCode0입니다. 강제 종료·Save·종료 뒤 창 조회는0이며, lease의 정상 반환 기한07:41:17Z 안입니다. Root의 정리 사전준비 assertion1회 중단/변경0은 전사 간접 근거로 남기며 정상 호출 성공에 합산하지 않습니다.
P11 공개3,984개는 **input2 적용 후 baseline21E432…7332**와 종료 전후 모두 path/bytes/SHA tuple이 일치합니다. 처음 input2의 의도2개 변경과 이후3,984 보존을 구분합니다. 문서 author는 공개 inventory tuple을 비교했으며3,984 실제 파일을 전수 재해시하지 않았습니다. private6 부재/indexS·내용/해시 조회0은 Root 감사 메타데이터 근거입니다.
Android 원본 사진2장은 DRAW/TIME EXPIRED·양팀0/DONE·Blue3434·RETURN TO LOBBY와 이후 FPS 에뮬레이터 홈을 보여줍니다. 정상 RETURN→EXIT/OK 조작 순서는 Root 관측이고 사진만으로 전체 조작이나 process 부재를 증명하지 않습니다. 이번 Android pidof 직접 확인은0이며 정확 ExitCode도 없습니다.

## 남은 범위와 기존 배포본

PR261은 사망/리스폰의 held 입력 경계 등 L2 판단 전 Draft를 유지합니다. 실제 OS 전이·native held MENU/death/rebind·실제 휴대전화·전체 경기/성능·최신 main Android 통합을 통과 처리하지 않습니다.
기존 [Android 세 native 모드](android34-native-fire-look.md)와 이번 PC 합성 callback은 서로 다른 근거입니다. 코드34/source612 QA/release12에 후속 PR282/287/288/290은 미포함이며35 예약·빌드는0입니다. 로컬 APK·서명·기존 제공 범위는 [Play34 기록](play34-internal-delivery.md)을 따릅니다.

## 공개 근거와 작성 범위

기준 폴더는 `D:/codex/validation/sudden-force-fps/20261028-work/resumed-1448/`입니다. body는 `held-lifecycle-candidate03/actual-596a83febc6c4bb8a68d5a81d113d142/result.public.json`, dispatch는 `held-lifecycle-dispatch-candidate01/actual-3acad1d1a07e4873a26ce894fd0dc905/receipt.public.json`입니다.
동결 설정은 `dispatch-attempt01/body-config.actual.public.json`(SHA16DC…103A), `queue-config.actual.public.json`(19EE…205C)입니다. 초기 `held-lifecycle-actual-success-review01/verification.public.json`(0BC815…B534)와 최종 `CLEANUP-SUPPLEMENT02.ko.md`(33C541…DCA3)·`verification.cleanup-supplement02.public.json`(8DD86E…311E)을 함께 보존합니다.
종료·사진·Root 공개 영수증은 dispatch-attempt01, audit는 상위 R28의 `p11-dispatch-before-close01.public.json`·`p11-dispatch-after-close01.public.json`입니다. 이 문서의 외부 작성 후보 `l1-callback-doc-candidate01/source-pin-manifest.public.json`에 수동 선정한 공개 원본의 전체 SHA와 직접/간접 범위를 고정했습니다.
문서 author의 새 제품·Editor/SDK/기기/GUI/ADB/키/Git/GitHub·Notion 변경은0입니다. 다음은 Root의 문서 독립 검토와 남은 L2 설계·실행 판단이며 현재 필요한 사용자 조치나 질문은 없습니다.
