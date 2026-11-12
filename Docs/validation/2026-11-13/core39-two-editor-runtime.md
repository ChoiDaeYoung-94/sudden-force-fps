# Core39 두 GUI Editor 경기·pose·종료 확인

2026-11-13 KST 기준 PR345의 정확 소스 `b84a6de5b9429d53c2c3f2e9523acdffcdbe42c9`를 두 GUI Editor에서 실행했습니다. 제한된 pose 표본과 정상 결과·복귀·자원 반환을 확인했으며, **Core39의 실제 hitbox 명중·피격·사망·재출현과 crouch는 미수용**입니다. PR345는 Draft를 유지합니다. QA6 layout43·LOCAL_QA·raw gait/pitch/HP 보완의 결과를 이 layout39 실행으로 소급하지 않습니다.

## 실행 소스와 보존한 실패

Primary는 `sudden-force-fps-input-main-20261112`, Secondary는 `sudden-force-fps-core39-20261113`입니다. 둘 다 Networked property39·LOCAL_QA=false·GUI/Direct3D11이며 cached protocol은 `sff-q20261022-ladder-v1-intro1-riflecore2`로 동일했습니다. native 31760의 MVID는 `a8136e34-1f02-4b5d-b0fa-aa20cbd2498d`, 39484는 `3f15bb82-2859-420c-8c4c-391ce4f26d40`입니다. 각 assembly를 해당 원 native/birth/nonce에 결속했고 MVID 동일성을 가정하지 않았습니다.

첫 startup의 native path null 처리 실패와 양쪽 forced Exit−1은 원 `lease-44961c12cd5d471f8568761718ab070d`에 보존했습니다. 후속 02 실행의 성공으로 바꾸지 않았으며 원 공통 new/full 시각도 늘리지 않았습니다.

## H01: 실제 두 피어와 pose 표본

`C39N13H01`의 Host/Client 두 피어에서 actor당 12 hitbox configured/rootAligned와 poseReady를 읽었습니다. 약 30초 관측은 Primary 1196 actor행/598 표본 프레임, Secondary 1202행/601프레임이며 errors0·pose failure=null·Ready=true였습니다. Secondary의 원격 Host actor는 input/state authority 모두 false인 pure proxy 표본입니다. 이는 해당 공개 pose context·reach/wrist 숫자의 제한된 관측이며 전체 렌더 프레임이나 모든 각도의 파지·접촉 품질 통과는 아닙니다.

총괄의 native 사격 후 shot counter가 0→5로 증가한 반면 대상 HP100/life0가 유지되어 적 명중·kill·death·respawn은 확인하지 못했습니다. 별도 target-head query는 `Wall_04 (2)` BoxCollider를 1.34930027m에서 먼저 만났고 hitHitbox=null이었습니다. 이 query 방향은 lastShotDirection과 달라 이전 5발 모두의 차단 경로/원인으로 확정하지 않습니다. LastShotHit=true도 적 피해를 뜻하지 않습니다.

H01은 원래 5분 timer를 유지하며 경로 질의·자리잡기 입력·사격·pose 수집을 진행한 경기입니다. 그 작업 중 TimeExpired/Draw로 종료했고 양쪽 RETURN으로 Lobby/Connected에 복귀했습니다. 합성 JoyStick/uGUI callback 조작은 자리잡기 보조로 구분하며 native 입력 회귀 통과로 합산하지 않습니다.

## H02: 10초 정상 만료·재복귀·Unbind

같은 Play 세션에서 앞서 COMPILE_DISABLED로 검증한 timer를 새 방 `C39N13H02`의 정확 Host에 한 번 Arm했습니다. disabled 단계 fixture0·scene/frame/roots 보존, 실제 `APPLIED … remaining=10`과 후속 frame fixture0을 확인했습니다. timer는 기존 forward callback에서 MatchTimer만 단축했으며 HP/phase/result를 직접 설정하지 않았습니다. TimeExpired/Draw 결과와 양측 RETURN을 확인했고, 두 Editor는 새 Runner로 Lobby/Connected에 복귀했습니다.

H02 observer는 Primary 916 actor행/458프레임, Secondary 924행/462프레임, errors0·poseReady/failureNull을 기록했습니다. 모두 crouch=false였고 Control_L pulse 시도에서 crouch 상태를 포착하지 못했습니다. 당시 CUA 키 입력의 실제 GameView 포커스 유효성은 확정하지 않았으므로 crouch 성공이나 제품 결함으로 판정하지 않습니다. 두 경기 모두 피격·사망의 실제 표본은 없습니다.

각 경기 전에 보관한 managed pose 참조를 RETURN 뒤 읽었습니다. **첫 경기와 둘째 경기 각각 양 Editor 합계 4개**에서 nativeObjectPresent=false·managedReferencePresent=true·Ready=false·status=`unbound-original-bindings-restored`·failure=null을 관측했습니다. 이는 저장된 참조의 Unbind 상태이며 살아 있는 객체가 남았다는 뜻이나 모든 asset byte 복원의 증명은 아닙니다.

## 종료·공개 입력 반환

후속 실행의 두 원 native handle은 Exit0·forced=false였고 cleanupFailures=[]였습니다. Prefs 두 DWORD는 baseline1로 돌아왔으며 shared ADB14100도 같은 세대로 보존됐습니다. 공개 입력의 형식 변화는 **전체 3경로**(Primary Resolver1, Secondary Resolver/ShaderGraph2)로 분류해 생성본을 보존한 뒤 한정 복원했습니다. 이후 각 프로젝트 공개 4093개 raw exact·Git clean·private6 absent-indexS를 확인했습니다. 개인 파일 내용 읽기/해시/복사는 없었습니다.

최종 반환 확인은 05:02:49 KST로, 원 full 05:09:48 이전입니다. 이 실행의 phone/LD 호출은0이며 최종 관측의 SDK 후보 프로세스도0개였습니다. 전체 SDK worker tree의 인과적 추적 증명으로 확대하지 않습니다. 전체 권리·실제 16KB 환경·Android39·Windows Player 방화벽/권한창 문제의 해결도 이 기록의 완료 범위가 아닙니다.

추가 로그 분류에서 C# compile error/Crash!!! literal은 양쪽0이었으나, 일반 CLI status의 알려진 호환 오류 "No command named 'editor status'" literal은 Primary7/Secondary1이었습니다. 이를 Console 전체 오류0으로 바꾸지 않았습니다. 이후 조회는 저장소의 `Tools/Unity/EDITOR_STATUS_COMPATIBILITY.md`에 따른 등록명 `editor_status`를 직접 사용하며 기존 로그를 보존합니다.

## 공개 근거

외부 기준 폴더는 `D:/codex/validation/sudden-force-fps/20261112-night/`입니다.

- `core39-two-editor-preparation01/runtime-checkpoint-independent-review01.public.json`: SHA256 `7951B66FB299B1F47087718C42C4440B2140D8138A450F55849D14CBE478DA33`. 이 원 checkpoint 자체는 반환 전 표본 검토이며 이후 반환을 선반영하지 않습니다.
- `core39-two-editor-preparation01/full-return01.public.json`: `25D7DF6C4ACAAD63948614805D5854A9C96F3AB9548F4F3A9B6AA5B587542DCA`.
- `core39-two-editor-preparation01/source-return-fcec8b8367b4416aa99544fc1fe10159/result.public.json`: `FD4A2B70403EE6FABE4026AEBB4D35289C2FCC8F30D4E21F151C663CB64FBDBF`.

원 runtime 영수증은 `riflecore2-runtime01/actual-00c22429e8e443dfa61fea307aa1e8c4/`와 `actual-093315374c074eaa9d2dac187b162496/`의 game/context-observer/timer/result/return/unbound 파일에 보존했습니다. 이 기록은 위 관측 범위의 요약이며 소총/보행 후보 전체 완료나 새 Android 배포의 근거가 아닙니다.
