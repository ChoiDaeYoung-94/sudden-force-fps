# #392 DesertHouse 창문 collider 수정 검증

2026-11-13, 수정 전 main `e70259a8452aef437c74af0f59a3c0b20f443fac`의 Primary stopped GUI에서 Wall_01/Wall_04 prefab 두 개를 수정했습니다. 원 full Box가 mesh의 창문을 메우던 문제를 네 개 frame Box로 바꾸고, DesertHouse6곳의 고정 양방향 정적 query 개선을 확인했습니다. 실제 게임 명중·피해·캐릭터 접촉·새 APK 검증은 포함하지 않습니다.

## 변경과 직렬화 범위

원 prefab GUID·root Box localID(Wall01 `1307016084558668629`, Wall04 `5203659734841271377`)는 유지했습니다. 기존 Box를 하단으로 줄이고 같은 root에 좌/우/상단 Box3개만 추가했으며 새 GameObject는 없습니다. prefab root layer0과 Scene instance layer8, 원 mesh GUID/localID·FBX bytes·기존 material/renderer 필드값은 유지됐습니다. Wall04의 Brick3개 source/변환/기존 override도 보존했습니다.

로컬 미터 기준 저장값은 다음과 같습니다.

| Box | center | size |
| --- | --- | --- |
| 하단 | (0,.75,0) | (3.2025313,1.5,.3) |
| 좌/우 | (±1.8006328,1.2,0) | (.3987343,2.4,.3) |
| 상단 | (0,2.2,0) | (3.2025313,.4,.3) |

enabled/nontrigger/material을 유지하며 새 collision include/exclude bits=0/0, layer priority=0, providesContacts=0입니다. 단, **다른 component까지 byte exact인 저장은 아닙니다.** Unity 저장으로 root Transform에 version2·proportional-scale 필드가 추가되고 RootOrder가 제거됐으며 pose/children은 유지됐습니다. Renderer에는 shadow/raytracing/culling/LOD/mask 관련 필드8개, Collider에는 version2→3과 위 neutral 필드가 추가됐습니다. Wall04의 nested PrefabModification3개에는 version3·빈 removed/added GO/component 목록과 flow-map 줄감쌈 변화가 생겼습니다. 독립 검토는 이 migration과 의도한 geometry를 분류한 뒤 기존 나머지 token/참조를 대조했습니다. 모든 renderer 버전·플랫폼 동작의 동일성을 주장하지 않습니다.

## 실제 정적 query

벽 원 mesh를 가진 직접 instance는 prefab당3개, 총6개였습니다. 원 child Box는 Wall01=0개, Wall04=3개로 전후 같았고 source geometry·metadata도 새 Scene instance ID를 제외해 일치했습니다. root Box는1→4개이며 child를 무시하지 않고 별도 query했습니다.

각 instance의 슬롯3점/frame4점에서 앞뒤 두 방향을 조회해 **Before84개/After84개 모두 기대와 일치**했습니다. Before 슬롯36개는 root full Box에 막혔습니다. After 슬롯36개는 root/child 직접 hit0·global child hit0이고, frame48개는 root 차단을 유지했습니다. layer8 전역 hit와 다른 geometry도 별도 기록했으며 mode당 SyncTransforms1회, Simulate/Scene 저장0회였습니다.

과거 exact target-head query와 같은 origin/target(거리3.400185m)을 비교한 결과, Before는 `Wall_04 (2)`가1.34930027m에서 먼저 막았고 After는 target 앞 차단이1→0개가 됐습니다. 뒤 `Wall_06 (1)`은4.867542m에 그대로 남았습니다. 이는 창문 앞 world collider 차단 해제이며 원래 사격 방향 전체·현재 player hitbox 명중이나 실제 피해의 증명이 아닙니다. 두 query 모두 damageCalls=0입니다.

## 실패 보존·감사·반환

ray probe01의 CS0165 컴파일 실패와 probe02 BEFORE의 EXACT_SIX_PREFAB_INSTANCES 거부는 원 영수증에 보존했습니다. 수정된 probe03의 실제 전후 query와 구분합니다. prefab 수정 helper01은 enabled로 실행하지 않았고, root/child 구분을 고친 helper02에서만 두 저장·성공 반환·Unload를 확인했습니다.

GUI 종료 요청은 지연됐으나 총괄이 소유 원창을 활성화한 뒤 완료됐습니다. native54916은 Exit0·forced=false·ownKillAttempted=false였고 cleanupFailures=[]입니다. 종료 후 CUA capture의 window-gone 오류는 별도 도구 실패로 남기며 강제 종료나 제품 충돌로 바꾸지 않습니다. Prefs 두 DWORD baseline1·공유 ADB 동일 세대·SDK 후보0을 확인했습니다.

Root는 Editor 종료 뒤 초기 audit의 Resolver 형식 변화1개를 생성본 보존 후 별도 CAS로 복원했습니다. 최종 공개4089개는 **의도 prefab2개 새 hash 보존/나머지4087개 exact**이며 Git도 의도 두 prefab 변경만 남았습니다. 최종 반환은05:33:08.450 KST로 원 full05:45 이전입니다. CI·추가 SDK/폰/LD 작업은0이며 이 변경을 담은 새 APK는 없습니다. 운영 코드34와 기존 배포 원장은 그대로입니다.

## 공개 근거와 미완료

외부 근거 폴더: `D:/codex/validation/sudden-force-fps/20261112-night/map-window-collision-fix01/`.

- 실제 독립 검토 `prefab-actual-independent-review01.public.json`: SHA `712C597BDD14C5076B6DE7901B89BE273BD1D66F09C36C94FB46448A9267129B`.
- 최종 `public-audit-d9447a234a1a47689b5efad2b156214a/audit.public.json`: `3A8DA29D59480043131A971CB1141C11EEA1D66498077773C31601CD601BBDAE`.
- `full-return01.public.json`: `188AF51C5427B031B782AA2EC201EFF4D36E56DA2477FA524656221A60A2DDB0`.

원 전후 ray·observed-shot·실패·종료 영수증은 `actual-32330729919d463fb6b1b10a4cd71112/`에 보존했고, prefab before/after 핀은 `prefab-snapshots02/pins.public.json`에 결속했습니다. 고정 표본 밖의 모든 벽/각도·실사격·피해·기기 품질은 후속이며, 이 문서는 공개 자료를 요약한 변경·검토 기록입니다.
