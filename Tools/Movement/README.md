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

새 동일 schema Windows02 development 빌드는 Succeeded/오류0·경고2/53.1922956초이며 산출물은278,504,817B입니다. 별도 Windows03은 검토한 임시 CLI 자동 client를 포함한 개발용/headless 검증본으로 오류0·경고2/24.9708627초이며 두 빌드의360파일 해시를 독립 대조했습니다. 임시 bootstrap은 Windows+Development+!Editor 및 정확한 CLI flag에서만 실행하며 제품 PR에는 포함하지 않습니다. 닉네임 검증·방 참가·팀/READY의 정상 API를 사용하고 Actor Transform·CC.Move·Networked·게임 입력을 직접 쓰지 않습니다.

실제 Editor host와 Windows03 client의 같은 새 프로토콜 경기에서 **첫 사다리1의 상승→상단 착지→하강→하단 이탈**을 확인했습니다. 별도 독립 runtime 감사31조건이 모두 일치합니다. 호스트339표본·합성 입력364회, 요청 ladder id1 유지, 하강 마지막 attached tick10450→첫 detached10451의 간격1, 하단 수평 오차.004006m·수직 오차.049071m입니다. 최종 standing 높이1.8/grounded/속도0·health/ammo/생명 버전 보존·등반 중 발소리 증가0, 종료 source9/context5 핀 오류0·callback 제거/자체GO Destroy 요청을 확인했습니다. 이 JSON만으로 다음 프레임 객체 부재를 주장하지 않습니다.

별도 client의 동결279표본 중 원격 host234표본에서 climbing/presentationClimbing 각각4회와 최대Y3.55691m를 확인했습니다. 네트워크를 통한 원격 상태/위치 수신 근거이며 렌더 화면·지연·packet loss 검증은 아닙니다. 정상 Nickname/Join/Team/READY는 각1회 관측 후 host START를 요청했습니다. Windows 보안 알림을 제어 도구가 선택하지 못해 native UI 입력은 검증하지 않았고 보안 설정도 변경하지 않았습니다.

하단 접근 v2는 실제 입력으로 이동했지만 preflight의 raw Vector3 JSON 직렬화가 실패해 state 파일이 없었습니다. 해당 접근을 PASS로 합산하지 않습니다. 별도 읽기 전용 재확인에서 standing/실제 BottomEntry true·속도.00138/QA callback 객체0을 확인한 뒤 route를 시작했습니다. serializer 예외 재현과 향후 DTO 수정 후보는 보존했으며 현재 하단 actor에 이전 접근 fixture를 재실행하지 않았습니다.

route 뒤 root stop 요청으로 자동 client가 정상 로비 복귀 후 열린 process handle ExitCode0으로 종료했습니다. bootstrap의 passed=false는 의도된 기록이며 route 검증과 별개입니다. host는 결과 상태에 남아 있었고, 명시적 ReturnToLobbyAsync 후 LobbyConnected/actor0/QA객체0을 확인했습니다. 자동 복귀로 확대하지 않습니다. 첫 Windows01의 겹친 Pipeline 요청/3 errors 실패와 추가 readback timeout도 보존합니다.

두 번째 사다리·A/D 탈출·죽음/respawn·천장/상단 장애물·client 입력 권한 예측·모바일 실제 입력은 후속입니다. side-release는 외부 후보/부분 컴파일만 완료했고 독립 검토와 실제 실행은 하지 않았습니다. 이번 결과는 기존 dirty 설정이 있는 P2의 PC 검증이며 깨끗한 체크아웃/Android 서명·제공·기기 통과가 아닙니다. APK27/AAB28에는 미포함입니다.

route fixture v3는 실제 Running2피어/own input authority/동일 Runner/standing/own physics/source pins를 먼저 확인한 뒤 합성 이동 입력만 제공합니다. 종료에도 source9/context5를 다시 확인하고, 마지막 하강 attached 관측→첫 detached 관측의 tick 차이가0..1일 때만 하단 거리·요청 id 조건과 함께 route 완료로 수락합니다. Editor update 공백으로 이를 증명하지 못하면 incomplete입니다. route 완료는 모든 기능이나 native 원격 로그 수신의 통과가 아닙니다. Transform/Teleport/직접 CC.Move/직접 network state setter로 접근이나 결과를 만들지 않습니다.

마지막 정상 Editor 종료는 열린 process handle ExitCode0/Temp lock 부재로 확인했습니다. 종료 뒤 생성본7개를 외부 보존하고 임시 QA C#·meta2개만 제거했습니다. CJK font는 이전 원본과 raw exact, Photon 설정은 현재 HEAD와 exact입니다. URP 자동 변화(SSAO prefilter4필드·설정 참조1개)를 보존 후 되돌렸으며 기존 비PS dirty4개는 최초 raw exact입니다. PlayerSettings는 최초 dirty 원본을 보존하되 이미 병합한 앱 아이콘2필드만 유지했습니다. 첫 보존 검사의 URP 불일치 중단과 새 복원 revision을 함께 보존합니다. 최종 공개3966개 manifest를 새로 생성했고 제품 source/prefab/scene 변경은 보존했습니다. 프로젝트 전체 clean이나 빌드 당시 설정 무변경을 주장하지 않습니다. Windows02 native Lobby client는 보안 알림 때문에 종료 조작이 막혀 보존 중이며 정상 종료 완료에 합산하지 않습니다.

근거는 `20261022-quality/ladder-apply-01/post-readback/`, `route-runtime-01/`, `ladder-apply-01/final-restoration-proof.json`, `public-after-final-restoration.json`에 보존합니다.
