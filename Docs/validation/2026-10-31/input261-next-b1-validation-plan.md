# 다음 B1 검증의 경로와 정상 입력 관측 계획

2026-10-31 source-only 인계 계획입니다. 기존 C9 실제 결과를 마무리한 뒤 공개 제품/검증 소스와 설치된 SDK 문서를 검토했습니다. 새 helper/cohort/C10 코드·컴파일·Editor/두 피어 실행·입력·권한 발행은0입니다. 이 계획은 과거 lease나 실행 증거를 연장하지 않습니다.

## 현재 확인한 범위

[C7](input261-b1-head-preflight-stop.md)은 첫 충돌/head 사전 검사에서 발사 전에 중단했습니다. [C8](input261-readonly-shot-geometry.md)은 다른 실제 배치의 두 ray에서 Wall_04를 먼저 맞았으며, C7 원인 전체를 확정한 것은 아닙니다. [C9](input261-small-public-joystick-navigation.md)은 공개 joystick 합성 콜백의 작은 이동과 해제를 확인했습니다. 결과 순변위0.278931m와 후속0.303792m 사이 추가0.024861m를 보존하며 완전 정지·패킷 수용·현재 head LOS·B1/L2 통과로 확대하지 않습니다.

## 다음 최소 순서

| 단계 | 수행 범위 | 완료와 중단 기준 |
|---|---|---|
| 새 배치 확인 | fresh native/Runner/두 actor의 life·source/UI와 읽기 전용 direct/predicted 첫 충돌을 확인합니다. | epoch/life/focus/hardware 조건이 바뀌면 중단합니다. 이전 pose·ray·navEpoch를 새 근거로 복사하지 않습니다. |
| 작은 위치 조정 | 실제 장애물을 바탕으로 새 범위의 작은 waypoint를 정해 공개 joystick 콜백으로 이동하고 같은 pointer를 해제합니다. 후속 위치·중립과 두 ray를 다시 확인합니다. | 적 configured head가 두 ray의 첫 hit이면 이동을 끝냅니다. 예상 밖 충돌·변위 부족·시간/거리 상한·pointer 소유 변경은 partial/실패로 보존합니다. |
| 정상 입력과 B1 분리 | 필요하면 짧은 정상 FixedTick 관측을 별도 설계하고, head 사전 검사와 소유권/정리 조건을 통과한 새 단위에서 bounded B1을 진행합니다. | 실제 shot/ammo 및 FIRST_SHOT·정상 해제/정리 조건으로 판정합니다. public Fire 호출 횟수나 stored input 미확보만으로 성공/실패를 대신하지 않습니다. |

현재 C9의 한 goal/route5초/travel1m·waypoint≤0.5m/oneintent는 외곽 전체 이동에 충분하지 않습니다. C9는 정상 종료·cleanup했으므로 같은 cohort/intent/lease/config/compile 영수증을 재사용하지 않습니다. 여러 구간이 필요하면 구간 수·총 거리/시간·취소·실패 보존을 새 계약으로 제한하고 독립 source/controlled 검토와 fresh 실제 컴파일/기본 비활성 검증을 수행합니다. 현재 cap을 임의 확대하거나 같은 phase를 자동 재시도하지 않습니다.

정적 후보인 positive-x 외곽 x≈8.8 우회는 root BoxCollider71개의 단순 모델에 근거합니다. nested collider/mesh·기울어진 형상·바닥 연속성·runtime 물체·lag history를 완전하게 모델링하지 않았으므로 안전한 경로나 실제 LOS로 확정하지 않습니다. 실제 장애물 조회가 지지할 때만 후보로 사용하고, 가까운 위치에서 LOS가 확보되면 외곽 전체 이동은 생략합니다. actor transform/HP/private flag 쓰기는 필요하지 않습니다.

## stored input과 B1 판정의 차이

동결 C9 writer의 `check_event`는 `normalStoredInput.inputAvailable`의 bool 형식과 nullable 필드 형식만 확인하며 true를 요구하지 않습니다. `b1Accepted`의 직접 조건은 실제 FIRST_SHOT, shot 증가1..12와 같은 ammo 감소, SeedAim1회·Fire true1회 이상 및 stopped/중립 발행과 반환/요청 소비/소유 객체 부재/후속 추가 shot0 등입니다. 따라서 stored false 자체는 직접 차단 조건이 아니지만, 실제 제품의 `GetInput`과 현재 life/Fire 등 소비 조건은 여전히 필요합니다. shot/ammo counter는 ray 판정 전에 바뀌므로 이 수용은 실제 head hit·kill·L2 사망/리스폰을 증명하지 않습니다.

제품 desktop `PlayerInputSource.Update`는 NoHardware의 mouse=false에서 Fire를 false로 덮습니다. Host는 LateUpdate에서 public SetFire(true)를 호출합니다. 설치된 Fusion SDK XML은 Fusion update 구간의 끝이 일반 MonoBehaviour.Update보다 앞이라고 설명하므로 LateUpdate(n)의 true가 다음 Fusion poll까지 남는 흐름이 문서상 가능합니다. 실제 current Runner의 poll 순서·gate·forward tick·소비 성공은 미확정이며, 항상 발사 실패 또는 성공한다고 단정하지 않습니다. C9의 stored14회 false 원인도 확정하지 않습니다.

권장 진단 후보는 자기 SimulationBehaviour 하나를 정상 Runner에 등록해 자연 FixedUpdateNetwork/forward tick에서 공개 current-tick 입력·life·Fire/Move·shot/ammo를 예를 들어2초/128샘플 이내로 관측하는 것입니다. 정확한 한도와 구현은 후속 검토 대상이며 frozen B1의 새 필수 승인 gate가 아닙니다. Snapshot/OnInput 강제 호출·input.Set·manual tick·Source.Update 억제·private getter/writer는 필요하지 않습니다. 자기 등록·버퍼·출력이 변하므로 전체 상태 무변화라고 주장하지 않습니다. 기존 witness02는 과거 pulse/private Source 읽기 범위가 포함되어 그대로 재사용하지 않습니다. 이 후보도 제품의 실제 GetInput 호출이나 정확한 callback 순서 자체를 직접 계측한 증거와 구분합니다.

## 근거와 현재 상태

공개 소스 기준은 P11 detached `f055e4a84391bd9d8741c4e12855887b1ef72166`, 동결 C9 manifest `8DDEFFD65715A6F038C426D4B629363BB5E71D1C233B021237540C92918D7D02`입니다. HostMain `9B6152ECD409CE661AFE935A089C8A6B48365AB01E95644DF6CFEBECCE1670A0`, writer `33F27CF429F8EFAC9417054E123773E7FDEBADE9E4D4DAD9D06F98C56423122E`, 제품 PlayerInputSource `F8788B018F41066597D0441B9FE3CBCD7C3E7388C5784062DB1FAC4222A66777`와 Fusion.Runtime.xml `9F467F11C4AED193B0E7FCF23EFECB7F843F2DEC8739342FEEB4E984DE66B51A`의 공개 파일만 검토했습니다. 이 source-only 검토에서 실제 PS/private6·raw log·Game/SDK 실행 질의·OS 입력은0입니다.

별도 로컬 검토 자료는 `D:/codex/validation/sudden-force-fps/20261031-work/resume-0124/` 아래 `b1-normal-poll-feasibility-source-review01/verification.public.json`(SHA256 `88F22EB1DAC0417311345B90F9E84A352A85D7B7A1B44618C405B2559897332D`)과 `next-route-source-readiness01/verification.public.json`(SHA256 `8C265C09E15181F1DA6949D552594755D6B793960F74BA747F09C51B55DCDDF9`)입니다. 실제 C9 수용 범위는 기존 실제 결과 문서와 해당 actual IR을 그대로 따릅니다.

#259 OPEN/PR261 Draft를 유지합니다. 제공34/1.0.196/release12/source612 QA·원장196,2,34와 최신 main 통합을 구분하며 새35 예약/게임 APK·AAB/서명/업로드0입니다. 실폰·장시간 성능 검증은 후순위입니다. 현재 필요한 사용자 조치와 질문은 없습니다.
