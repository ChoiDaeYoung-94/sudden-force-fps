# 기존 사다리 이동

DesertHouse의 기존 `Stairs_01` 두 개를 사용합니다. 원 사다리 mesh/solid collider와 맵 배치는 유지하고, 각 인스턴스에 `LadderVolume`과 layer2의 진단용 trigger 자식만 추가했습니다. 진입 판단은 trigger callback이 아닌 고정 기하·시선·이동 입력을 사용합니다.

## 조작과 이동 권한

- 하단에서 사다리를 바라보고 **W/앞 입력**으로 진입·상승합니다. 상단에서 바깥쪽을 바라보고 **S/뒤 입력**으로 진입·하강하며, 하단에 도착한 뒤 S로 빠져나옵니다.
- **A/D/좌우 입력의 절댓값이 .75 이상**이면 등반을 해제합니다. 모바일 stick의 작은 대각 흔들림에는 해제하지 않습니다. 별도 등반 버튼은 추가하지 않고 기존 HUD 안내를 사용합니다.
- 등반 중 새 crouch 요청을 무시합니다. 이미 crouch 중인 플레이어를 천장 검사 없이 강제로 세우거나 사다리에 진입시키지 않습니다. 사망·respawn·경기 종료에는 기존 생명 주기와 함께 등반 상태를 초기화합니다.

기존 GamePlayer가 이동 dispatcher를 계속 소유합니다. `FixedUpdateNetwork`에서 `TrySimulate`를 한 번 호출하며, motor가 소비한 tick에는 기존 NCC.Move를 함께 호출하지 않습니다. Motor에는 별도 FUN이 없고 CharacterController.Move 한 경로를 사용합니다. 기존 NCC의 위치 복제 소유권을 유지하고 NCC velocity/grounded를 맞춥니다. 전역 중력/NCC gravity/Teleport를 바꾸지 않습니다.

등반 상태·사다리 id·생명 버전·상단 탈출 단계·재진입 delay는 새 Motor NetworkBehaviour의5개 networked 필드입니다. 서버 상태 권한과 입력 권한 예측이 같은 경로를 사용하며, proxy presentation은 snapshot의 등반 상태를 읽습니다. 상단은 floor normal/높이와 full standing capsule·이동 sweep을 검사하고, 막히거나 buffer가 포화되면 통과시키지 않습니다.

등반 presentation 동안 거리 기반 발소리와 이동 bob 누적을 초기화하고 기존 locomotion audio를 멈춥니다. 발소리가 등반 중 계속 누적되어 착지 후 몰아서 재생되지 않도록 합니다. 기존 fire/combat/recoil queue, viewmodel offset 합성, crouch Geometry673D, 별도 FPS 팔 Avatar/rig는 보존합니다.

## 네트워크 호환성

실제 Photon AppVersion은 **`sff-q20261022-ladder-v1`**입니다. 이는 Photon 방 탐색·매칭 namespace를 분리하기 위한 protocol tag이며 Android versionCode/versionName 예약과 별개입니다. 같은 표시 room 이름이라도 다른 AppVersion의 이전 crouch/locomotion binary를 함께 사용하지 않습니다.

새 Motor NetworkBehaviour와5개 필드가 추가됐으므로 동일한 새 소스·prefab bake·scene·AppVersion을 포함한 새 Editor/Windows 두 피어가 필요합니다. 기존39-field crouch binary의 성공을 새 사다리 schema의 검증으로 재사용하지 않습니다. 후속 schema 변경에도 새 tag와 양쪽 새 binary 검증을 함께 기록합니다.

## 확인된 범위와 남은 검증

실제 제품9소스 Unity compile/weave는 completed/failedfalse/errors[]로 관측됐고, 실제 helper가 prefab2와 지정 scene1만 저장했습니다(globalSave0). 적용 후 독립 읽기 전용 감사의 **43개 구조 조건을 모두 통과**했습니다.

- source9/context5/Avatar/대상 meta3 exact. 양 prefab406→407, Motor1 추가 외 기존404 records raw exact. 기존 component/Fusion 배열 상대 순서와 12 metadata·12 hitbox·본체/FPS rig·Animator를 보존했습니다.
- Scene1290→1302, 기존1288 records raw exact. 기존변경2개는 두 사다리 prefabinstance의 추가 목록뿐이고, Volume2/trigger2 및 원 solid 참조를 확인했습니다. 원 사다리 prefab·meta도 before/after/current SHA exact입니다.
- 앞선 full standing capsule(height1.8/radius.3/margin.04)의 두 사다리3점 overlap/직선 상단 후보 sweep은 blockers0/saturatedfalse/floor2였습니다. 이는 정적 probe이며 실제 moving CC나 두 피어 이동 성공이 아닙니다.

새 동일 schema Windows02 development 빌드는 Succeeded/오류0·경고2/53.1922956초이며 산출물은278,504,817B입니다. **실제 두 피어 route는 아직 미실행**입니다. Windows 보안 알림을 현재 제어 도구에서 선택할 수 없어 native client는 로비까지만 확인했습니다. 보안 설정은 변경하지 않았고, 별도 개발용 자동 client 후보를 준비 중입니다. 이 후보는 기본 게임 코드와 구분하며 실제 native UI 검증으로 확대하지 않습니다. 첫 Windows01의 겹친 Pipeline 요청/3 errors 실패는 보존하며 새 시도의 성공과 합산하지 않습니다. 상·하단 실제 이동, 원격 복제 수신, A/D 탈출, 죽음/respawn, crouch·천장/상단 장애물, 모바일 실제 입력과 두 번째 사다리 검증은 결과가 있는 항목만 개별 기록합니다.

route fixture v3는 실제 Running2피어/own input authority/동일 Runner/standing/own physics/source pins를 먼저 확인한 뒤 합성 이동 입력만 제공합니다. 종료에도 source9/context5를 다시 확인하고, 마지막 하강 attached 관측→첫 detached 관측의 tick 차이가0..1일 때만 하단 거리·요청 id 조건과 함께 route 완료로 수락합니다. Editor update 공백으로 이를 증명하지 못하면 incomplete입니다. route 완료는 모든 기능이나 native 원격 로그 수신의 통과가 아닙니다. Transform/Teleport/직접 CC.Move/직접 network state setter로 접근이나 결과를 만들지 않습니다.

공개 before/after 비교의 CJK font·PlayerSettings 자동 저장 변화는 별도 보존·복원 대상입니다. 이 감사로 기존 dirty5 exact나 프로젝트 전체 무변경을 주장하지 않습니다. 근거는 `20261022-quality/ladder-apply-01/post-readback/`와 새 `route-runtime-01/`에 보존합니다.
