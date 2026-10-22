# PC 이동·사다리 경계 검증

## 추가 격리 충돌 질의 검사

실제 제품 SoldierBlue 프리팹의 비활성 복제본을 독립 Preview PhysicsScene에 만들고, 모든 Behaviour·Renderer·기존 Collider를 비활성화한 상태에서 `PlayerLadderMotor.Clear/SweepClear`만 호출했습니다. 복제본의 `_cc`·`_owner` 캐시 두 개만 초기화하며 Spawned/FUN/Render·Networked 접근자·CC.Move·Photon·자산 저장은 검사 코드에서 호출하지 않았습니다. 이는 Unity/Fusion 내부 전체 getter를 계측했다는 뜻은 아닙니다.

12조건은 빈 공간/경로 통과, 천장 겹침 거부/비활성 천장 통과, 목적지는 비었지만 경로의 실체 장애물 거부, trigger/layer2 무시, 자기 하위 충돌체63개가 모두 무시될 때 통과/64개로 overlap 버퍼가 포화되면 거부입니다. 모두 기대값과 일치했고 별도 독립11조건 대조도 일치했습니다. preview 정리·원래 scene identity/count/dirty 보존·제품 source9/context5 및 프리팹/meta 일치, 검사 Editor41708의 열린 handle ExitCode0을 확인했습니다. 시작 과정의 PlayerSettings 변경1개만 새 외부 사본에 보존·복원해 직전 공개3967개 입력과 raw exact를 확인했습니다.

고정 함수의 합성 질의 검사이며 실제 이동하는 CC/막힌 상단 탈출/네트워크 검증은 아닙니다. 막힌 목적지에서 SweepClear를 호출하는 분기와 CapsuleCast 배열 자체64-hit 포화도 이번 조건에 없습니다. 근거는 외부 `20261022-quality/extension-1500/ladder-guard-runtime-01/`, `ladder-guard-candidate/independent-runtime-review.md`와 `final-guard-restoration-proof.json`입니다.

## 2026-10-22 PC 이동 경계 후속 관측

P2의 **headless Editor host(pid4740, batch/graphics Null)**와 별도 headless Windows Development Player를 실제 Photon 방에 연결해 합성 이동 입력을 관측했습니다. GUI 키보드/터치·화면 렌더·오디오 실청취 시험과 구분합니다. 프로토콜은 `sff-q20261022-ladder-v1`이며 native client bootstrap은 정상 nickname/Join/팀 선택/READY 뒤 **게임 입력을 공급하지 않는 관찰자**입니다. Bootstrap의 `passed=false`는 그대로 유지하고 각 이동 fixture의 결과와 별도로 읽습니다.

### 빌드와 준비 이력

| 시도 | 실제 결과 | 범위 |
|---|---|---|
| GUI Windows04 queue | 소유 callback1개만 제거하고 Build 시작 전에 취소, GUI Editor55168의 열린 handle ExitCode0 | 빌드 성공/실패 실행으로 합산하지 않음 |
| Windows05 | Development/Succeeded, 오류0·경고5,84.8419791초,BuildReport total278,518,965B | B174 및 C174 준비에 사용. 실제360파일의 manifest SHA 불일치0 |
| Windows06 | Development/Succeeded, 오류0·경고2,21.3116046초,BuildReport total278,518,965B | D174에 사용. 실제360파일의 manifest SHA 불일치0 |

두 빌드는 각각 임시 bootstrap v3/v4를 포함하며 서로 다른 산출물입니다. 각 manifest와의 일치가 두 binary 사이의 바이트 동일성을 뜻하지 않습니다. 검증용 Player 표시 버전은1.0.194이며 Android 제공본 버전과 별개입니다. artifact verification의 `runtimeInvoked=false`는 실행 전 작성된 원장 값이고, 이후 별도로 기록한 실제 참가/경기가 없었다는 의미가 아닙니다.

C174는 client가 먼저 참가한 뒤 팀 변경을 요청한 준비 순서에서 targetTeam0/READY를 충족하지 못했습니다. `gameObserved=false`, `targetTeamObserved=false`, `readyObserved=false`로 보존했고 정상 stop→Lobby 복귀→client35800 handle ExitCode0을 확인했습니다. 준비 실패를 D 성공으로 바꾸거나 경기 성공으로 합산하지 않습니다. D174는 **host가 단독 Room에서 먼저 Blue/Team1임을 관측한 뒤** 새 Red/Team0 client를 참가시켰으며 실제 Room/Game의 두 팀 bind를 따로 확인했습니다.

### B174: 첫 사다리 D/A 해제

Red host의 실제 입력 공급으로 첫 사다리를 짧게 오른 뒤 합성 `MoveX=+1/MoveZ=0`(D 의미), 별도 `MoveX=-1/MoveZ=0`(A 의미)를 제공했습니다. 실제 물리 키 D/A를 누른 시험은 아닙니다.

| 관측 | D 의미 /MX+1 | A 의미 /MX−1 |
|---|---|---|
| elapsed/표본/입력 | 1.062364초/65/75 | .9678077초/59/68 |
| 최대 상승 | .52878207m | .5320143m |
| 마지막 attached→첫 detached | 15494→15495,gap1 | 22510→22511,gap1 |
| side 입력 공급 | 각1회/고유 OnInput tick1 | 각1회/고유 OnInput tick1 |
| 최종 상태 | grounded/planar speed0/climbingfalse | grounded/planar speed0/climbingfalse |

두 fixture에서 HP100/ammo30, standing capsule1.8/.3, 요청 LadderId1, preflight blockers0/포화false, 종료 pin·cleanup 오류0을 확인했습니다. side tick은 입력을 공급한 tick의 근거이며 내부 FUN 소비 시점이나 유일한 detach 원인 이벤트의 직접 증명이 아닙니다. 후속 readback은 활성 `__QA` Fusion.NetworkEvents0을 확인했으나 숨김·비활성 포함 모든 GameObject 전수 검사와는 다릅니다. A 후 readback의 `combatAllowed=false/CanProvideInput=false`는 이미 끝난 경기 상태이며 A 입력 복구 실패로 해석하지 않습니다. 이 시험들은 해제·착지 후 종료됐으므로 **등반 중 경기 종료 reset 시험은 아닙니다**.

B의 별도 Win05 client14940 동결 관측은360표본이며 remote host320표본에서 climbing0/PresentationClimbing0입니다. 약1초 표본이 짧은 상승을 놓쳤을 수 있으나 **B의 원격 등반/해제 수신 성공 근거는 없습니다**. 이 관측 부재만으로 복제 결함을 확정하지도 않습니다. 최종 client-auto-02는363표본/예외없음·정상 stop→Lobby 복귀 및 열린 process handle ExitCode0으로 후속 종료를 확인했습니다.

### D174: Blue host의 사다리2 접근·왕복

Blue spawn(1.875,0,13.5)에서 실제 합성 입력으로4개 waypoint를8.5822939초/517표본·564입력으로 통과했습니다. 최종 yaw180/planar speed약.001381/standing/BottomEntrytrue/climbingfalse이며 HP100/ammo30을 유지했습니다. 네 구간의 실제 full capsule preflight는 blockers0/포화false였고 종료 pin·cleanup 오류0입니다. **이 한 spawn의 접근 결과이며 모든 spawn 경로 성공은 아닙니다.**

별도 왕복 fixture의275표본·381입력에서 요청/상승/하강 LadderId2를 유지하며 W 상승→상단 착지·방향전환→S 하강→하단 이탈을 관측했습니다. 최대Y3.59528136m, down attached13828→첫 detached13829의 gap1이며, 그 시점의 실제 하단 오차는 수평.001141071m·수직.025868325m입니다. 최종 grounded/velocity0/climbingfalse/standing1.8·HP100/ammo30, step delta0 및 최종 pin·cleanup 오류0을 확인했습니다. Transform/Teleport/직접CC.Move/Networked setter로 fixture가 위치나 결과를 만들지 않았습니다.

별도 Win06 Red client26088의 동결210표본 중 remote Blue host184표본에서 **climbing4/PresentationClimbing4·최대Y3.5302734m와 하단 복귀 위치**를 관측했습니다. 이는 실제 다른 process의 원격 상태·위치 수신 근거입니다. B의 remote climb0과 혼합하지 않으며,1초 주기4표본을 모든 tick의 정확성·지연/손실·client 자신의 입력 권한 예측 성공으로 확대하지 않습니다. 이후 client-auto-04 최종211표본의 Lobby 복귀/예외없음과 열린 process handle ExitCode0을 따로 확인했습니다.

D route 뒤 host readback은 players2/gameActors2/활성 QA events0이었고, 명시적 정상 Return 이후에는 LobbyConnected/players0/actors0/활성 QA events0을 확인했습니다. 이 자료로 최종 Editor 프로세스 종료·임시 source/meta 제거·기존 dirty 복원까지 완료했다고 주장하지 않습니다.

### 원문 보존과 남은 범위

왼쪽 side state의 transition 문구에는 `request-MX1`이 남아 있어 실제 A 방향은 고정 source의 `mx=-1f`와 예약 경로로 확인했습니다. L2 route JSON의 `not ... second-ladder coverage`도 L1 템플릿에서 남은 metadata 오류입니다. 원 JSON은 보존하고 실제 requestedId2/source·scene 기하·표본·예약 근거로 이번 사다리2 범위를 정정해 해석했습니다. 조건을 완화하거나 원 기록을 덮지 않았습니다.

독립 검토는 B D/A 실제30조건, D L2 실제29조건이 각각 일치하며 해당 관측 범위에서 추가 확정 제품 P1/P2를 발견하지 못했습니다. 근거는 외부 `20261022-quality/extension-1500/independent-sides-runtime-review.md`, `independent-ladder2-runtime-review.md`와 각 evidence/audit, 원 side·L2 state 및 client 동결/최종 state·종료 handle·빌드 verification입니다. 표본 전체·원시 로그·Player는 Git 문서에 전재하지 않습니다.

다음은 여전히 별도입니다: client 자신이 등반하는 입력 권한 예측, 장애물/막힌 상단·천장, 사망/respawn·등반 중 경기 종료, packet loss/latency/correction, 전체 spawn, 실제 native GUI/모바일 조작·실청취·실기기. 기존 GUI native8204의 보안 popup 때문에 종료 조작이 막혔던 이력은 보존하며 이번 B/D headless ExitCode0으로 그 경로를 해결했다고 하지 않습니다. 보안 설정은 변경하지 않았습니다.

별도 #179 팀 만석 팝업/Windows07·E174 시험은 이 이동 검증의 통과 범위에 포함하지 않습니다. 해당 범위는 [팀 변경 안내](../UI/README.md)를 따릅니다. 이후 검사 Editor4740의 열린 process handle ExitCode0을 확인하고 생성 변경4개와 임시 source/meta2개를 외부 보존했습니다. 알려진 생성 변경4개만 이전 바이트로 복원하고 자신이 만든 임시 파일2개만 제거해 의도한 RoomManager·UI 문서 외 공개3964개 입력 일치를 확인했습니다. 기존 dirty4개는 raw exact이며 PlayerSettings는 원래 dirty 바이트 중 이미 커밋한 아이콘2블록만 달라진 상태를 유지합니다.

별도로 임시 검사 소스가 없는 제품 Windows08을 새 headless Editor62200에서 생성했습니다. Succeeded/오류0·경고2/28.3113661초/BuildReport total278,505,444B·실제360파일 합278,702,887B/SHA 일치, source9/context5/RoomManager 핀 일치와 실행 초기화 목록·Managed Assembly의 임시 QA 타입 부재를 확인했습니다. 이 바이너리는 실행하지 않았고 clean checkout/Android 빌드가 아닙니다. 새 Editor도 열린 handle ExitCode0으로 종료한 뒤 생성 변경4개를 다시 별도 보존·복원해 직전 공개3966개 입력의 raw exact를 확인했습니다. 첫 복원 proof를 두 번째 성공 근거로 복사하지 않았습니다. 외부 근거는 `windows08-product-artifact-verification.json`, `final-player-editor-exit-handle.json`, `final-product-restoration-proof.json`입니다.

이번 PC 변경·임시 QA는 기존 Android 코드27/28 산출물에 미포함이며 코드29는 미예약 상태입니다. 새 Android 빌드·서명·제공·최신 설치/실기기 통과로 합산하지 않습니다. 실기기·발열·장시간 FPS/메모리 측정은 기존 후순위 합의를 유지합니다. 기기·암호 입력은 요청하지 않았습니다. 기존 Windows 보안 알림의 취소는 도구 제어 한계로 별도 사용자 조치로 남습니다.
