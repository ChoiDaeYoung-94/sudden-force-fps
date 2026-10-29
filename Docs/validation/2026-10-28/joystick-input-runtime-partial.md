# Joystick 공개 입력의 실제 부분 관측과 정리 — 2026-10-28

**동기 probe 완료 / 짧은 pulse 전체 미통과 / 정상 정리 완료**입니다. 이동이나 복구 성공을 pulse aggregate 통과로 합산하지 않습니다.
[#259](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/259)/PR261의 남은 수명 경계 준비 중 기존 desktop joystick fallback만 좁게 관측했습니다. 합성 public PointDown/Drag/PointUp이며 native mouse·keyboard·Android touch 시험이 아닙니다.

## 실행 범위와 준비

P11 GUI PID72976/start `2026-10-28T09:11:35.4935151Z`, Game MVID `db62160d-f13a-41b2-b222-f685dc73a32b`의 정상 두 피어 Game에서 진행했습니다. P11은 f055 계열에 정확한 입력2개 C#을 overlay한 QA 기준이며 최신 main clean build가 아닙니다.
현재 제품 소스14·059E body·70ED readonly·0EF6 input dispatcher 핀을 대조했습니다. binder06/IR `BB09…4316`/`2FF69…3837`, readonly binder03 IR `77CA…4A24`와 실제 바인딩 영수증을 연결했으며 역사적04 proof를 현재 generator 근거로 대신하지 않았습니다.
초기 Joystick/readonly/input dispatcher의 미호출 compile·disabled12개는 before/after 같은 epoch 구간입니다. 별도 native producer4개도 diag0이지만 초기 구간 밖이며, 합계16을 native proof·설치·주입 통과로 세지 않습니다.
직접 eval의 focus=false 시도는 보존했습니다. 이번 D885 원문 deferred_eval nonce `fcd7ccbe986848f39d1d492222922a51`은 실제 focus true·정상 server2peer/HP100·ammo30·shot0·정수ID16개, input/productWrites0과 원 handler29 복원을 기록했습니다.
원 active CanvasUISafeArea/FixedArea1·radius100/현재계산100·first/rest/idle 조건에서 시작했으며 stale cache 수복·숨김 UI 활성·새 actor/SourceBind·HP/life/pose setter는 사용하지 않았습니다.

## 결과와 판정

| 범위 | 실제 관측 | 판정 |
|---|---|---|
| probe `85fff6a3484847d39d9e8248d2857415` | complete=true, 동기 같은 frame 조건·tick10556 동일, pose 불변, 복구6항목 true | 공개 Pointer API의 좁은 동기 반응/정리 완료; network input/이동 sampled=false |
| pulse `d6afbf3d88c34a88940cfde8fef62196` | sourceMove≈[.5,0], tick15186→15220, XZ0.401972055m, 복구6항목 true | **complete=false/inputAvailable=false로 전체 미통과** |

복구6항목은 sourceSame/joySame/handlersSame/globalsSame/pointerCleared/normalizedMoveZero입니다. pulse의 pose 변화는 의도한 이동이며 Y도 약0.03m 달라져 전체 pose 불변·복원으로 표현하지 않습니다.
pulse inputMove [0,0]은 유효한 network input 0을 얻었다는 증거가 아닙니다. reason=COMPLETED/errornull·실제 이동·queue READY만으로 complete=false를 통과로 바꾸지 않습니다.
probe dispatcher nonce `d322dfec898d453fbc18a1e19de95c42`는 자기 해제 후29→29입니다. pulse dispatcher `5456aec3c6434e3f93af4f4f3d4f7962`의29→30은 본문 소유 pulse1개를 허용한 중간 단계이며, raw 최종 handlersSame=true가29 복구 관측입니다.
TryGetInputForPlayer의 Editor.update 조회 시점 문제인지 제품 문제인지 **원인 미확정**입니다. 진단은 별도이며 현재 결과를 원인 해결이나 전체 입력 수용으로 확대하지 않습니다.

## 실제 설정 원본과 바인딩

아래 `A`는 `D:/codex/validation/sudden-force-fps/20261028-work/resumed-1448`입니다. 공개 JSON만 선택했으며 전체 SHA 연결은 actual IR/cleanup 보충의 evidencePins에 있습니다.

| 보존한 실제 설정5개(A 상대경로) | SHA256 앞16자리 |
|---|---|
| `joystick-readonly-dispatch01/actual-fcd7ccbe986848f39d1d492222922a51/config.before-pulse.public.json` — probe body | E7B4A25292F872F3 |
| `joystick-attempt02/input-dispatch-config.before-pulse.public.json` — probe dispatch | 6122D2A3881C48B6 |
| `joystick-attempt02/readonly-config.actual.public.json` | B7C7B369196EF7DD |
| `joystick-attempt02/body-pulse-config.actual.public.json` | C6DA2208EC9CF897 |
| `joystick-attempt02/dispatch-pulse-config.actual.public.json` | 983E9BE79346C312 |

실제 bound-probe/bound-pulse는 readonly actual 폴더의 `bound-probe.public.json`(D071…36AA)/`bound-pulse.public.json`(D35D…A97B) 및 attempt02의 input-dispatch-binding 두 영수증에 연결됩니다. body/dispatcher nonce·typed IDs·epoch·config SHA가 맞으며 소비/actual 마커는 보존했습니다.
중간 IR DB375…BC5E의 cleanupPending=true는 원문 그대로입니다. 보충5D92…DD49D가 cleanupPending만 false로 해소했으며 전체 수용 noBlockers=false/pulseAccepted=false는 유지합니다.
보충 당시 mutable3은 true·TTL만료 상태였습니다. **그 이후** Root의09:29 별도 config-retirement 영수증으로 enabled/rootApproved=false로 퇴역했고 위 원바이트5개와 runtime/소비마커는 보존했습니다. 보충이 이미 false 퇴역을 확인했다고 기록하지 않습니다.

## 정상 정리와 한계

Host 정상 RETURN→StopPlay→clean scene·frame cap60 관측 후 원 -1 복원을 확인했습니다. 같은 held PID의 정상 Exit0는 `2026-10-28T09:22:17.7462620Z`이며 force/save0, 자원 반환09:23:49는 lease 한도 안입니다.
Root의 종료 전후 public3984 path/bytes/SHA tuple을 overlay baseline과 대조해 일치를 확인한 보충 근거입니다. 검토자는 제품14만 새로 해시했고 전체3984 실파일 재해시·전체 메모리 무변화를 주장하지 않습니다. private6 부재/sparse는 Root 공개 감사의 간접 근거입니다.
Android는 Host Return 후 자동 Lobby를 Root가 관측했고 Android RETURN 클릭0입니다. `android-result.jpg`는 파일명이 잘못된 **Lobby** 사진으로 Result/DRAW 증거가 아닙니다. EXIT/OK 후 Home은 확인했지만 직접 pidof를 수행하지 않아 게임 프로세스 완전 부재를 증명하지 않습니다.
이번 실행의 새 설치/SDK서명·키·직접ADB 입력0이며, 별도 약16KB test helper APK를 게임 APK로 취급하지 않습니다. 발사·죽음/리스폰·전체 route·native/전체 PR261 수용은0/미검증입니다.
제공 게임34/release12는 source612 QA이며 후속 main의 PR282/287/288/290은 미포함입니다. 이 문서로 제품 수정·PR261 병합·새35 예약/게임APK/AAB 빌드를 수행하지 않았습니다. 다음은 조회 원인 진단과 별도 수명 경계 검증이며 Draft261 판단과 최신 main 통합은 분리합니다.

## 핵심 공개 원본 핀(A 상대경로)

| 근거 | SHA256 |
|---|---|
| `joystick-input-candidate03/actual-85fff6a3484847d39d9e8248d2857415/result.public.json` | FF5B313A72AA3D7E64607D34E0092B9A823926807DC89D2FF44B5DD1D9E31109 |
| `joystick-input-candidate03/actual-d6afbf3d88c34a88940cfde8fef62196/result.public.json` | BB45FCEBA3C76517A7181648103457DE2FBD695232F8B7AE4C36AF6FB86E277B |
| `joystick-actual-partial-review01/verification.public.json` — 중간 원문 | DB375CE8F35361B0543972F5CF96B52CEE5A8EBC15774467A3AE71844B99BC5E |
| `joystick-actual-partial-review01/verification.cleanup-supplement01.public.json` — 정리 보충 | 5D92B1B3097224824F9782E511649C1B0669E3518BA992169FAA67B28BDDD49D |
| `joystick-attempt02/config-retirement.public.json` — 보충 뒤 Root 퇴역 | 656765D86D15558F41A5B73AC6527D393BB50662B8C6DB64E22F0DB5AF816B51 |

공개 실행/held process/audit의 수행 사실은 Root 영수증 범위이며, 독립 검토는 지정 JSON의 수치·해시 연결과 사진의 보이는 내용을 확인한 범위입니다. 실패·미실행·부분 관측을 통과로 합산하지 않습니다.
