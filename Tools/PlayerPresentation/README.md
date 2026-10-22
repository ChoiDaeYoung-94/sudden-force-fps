# 1인칭 팔 표현과 검증 범위

PR169는 1인칭 팔의 소스 개선입니다. 기존 Play/Drive APK27·무서명 AAB28에는 미포함이며 Android 빌드·서명·기기 검증은 후속입니다.

## 리그·무기·물리 분리

기존 Slayver Military의 전용 FPS 장갑/셔츠 리그를 ViewModelRoot 아래에 독립 clone합니다. 두 mesh의61 bone 참조는 clone 내부를 가리키며, 원본 캐릭터의 월드 skeleton을 공유하거나 움직이지 않습니다. 원래 World/View muzzle, WeaponSocket, OffhandGrip와12 hitbox는 유지합니다. 새 Networked/RPC·hit/ammo/fire 규칙 변경은 없습니다.

같은 Avatar와 기존 CC0 기반 RifleLife의 Idle_Aiming만 사용하고 root motion은 false입니다. Animator의 호흡 자세를 유지하면서 새 rig root만 기존 view weapon anchor에 맞춥니다. 기존 ViewModelRoot가 recoil/이동/재장전 lowering을 팔+무기에 함께 적용합니다. input authority의 살아 있는 로컬 카메라에서만 팔 renderer를 허용하고 다른 카메라에서는 숨깁니다. 원본 FPS prefab/FBX/clip/controller를 변경하지 않습니다.

## 양면 셔츠 재질

영구 파생 재질 경로는 `Assets/Materials/Player/FirstPersonShirt.mat`입니다. fresh URP/Lit material에 기존 셔츠의 color/normal/occlusion texture를 재사용하고 opaque/Cull0·명시된 scalar 표면 값과 필요한 keyword만 지정합니다. source ShaderGraph material과 texture는 그대로 두고 두 player prefab의 독립 FPS Shirt3 참조만 바꿉니다.

원본의 카메라 쪽 소매 절단면이 큰 빈 테두리로 보였고, 소매를 숨긴 A/B에서는 피부 절단면이 더 드러났습니다. 양면 URP/Lit의 실제 Editor A/B에서는 안쪽 천이 표시되어 최초보다 개선됐습니다. **geometry를 밀봉하거나 절단 테두리를 제거한 변경은 아닙니다.** cap/IK나 무기·총구 위치 보정은 넣지 않았습니다. ShaderGraph와 Lit의 조명·roughness 표현이 완전히 같다는 주장도 하지 않습니다.

## 확인한 PC 근거

실제 Editor와 Windows peer가 있는 Photon 경기에서 Editor input authority의 팔을 관측했습니다. 동기 statement1회에서 owner camera만 허용, begin renderer2 on/end·other camera off, 최종 숨김 복구, 그립 position/angle0과 원래 non-arms Transform110·hitbox12·동일 network snapshot exact를 확인했습니다. 이 exact 관측을 전체14초 보존 검사로 확대하지 않습니다.

별도 합성 NetworkInput fixture의 host 발사는 두 번 각각 local11/confirmed11·Ammo30→19입니다. 클라이언트 예측 지연이나 물리 마우스/터치 테스트와 구분합니다.

walk/sprint/시선/reload fixture04는673 frame과24.04203m 이동, Ammo19→30·실제 reload 상태를 관측했습니다. 함께 돌린14초 read-only grip observer는837 sample, 최대 위치 오차9.83025e-7m·각도0·wrapper 이동.06523427m·reload87 frame입니다. 이는 Editor update 시점의 측정이며 GPU render callback·양쪽 새 Windows 팔 빌드·충돌 전체·장시간/기기 성능 검증은 아닙니다. 앞선 full magazine fixture03의 reload=false와 정지 sampler의 무변화 결과도 보존했습니다.

임시 양면 A/B는12초 후 원 sharedMaterials와 원 material object exact 복구를 확인했습니다. primitive cap이나 새 texture를 추가하지 않았습니다.

## 남은 검증

- 영구 재질1개와 두 prefab을 명시 저장했습니다. 이후 실제 경기에서 재질과 팔 표시를 확인하고1920×1080·2560×1080 카메라 이미지를 관찰했습니다. 새 material은 opaque/Cull0/metallic0/smoothness.5이며 원 texture3개를 공유합니다.
- pitch 극단·실제 fire/reload 손동작·death/respawn·양쪽 새 Player와 Android 화면은 추가 확인이 필요합니다. 재장전은 기존 wrapper lowering을 따라가며 전용 탄창 교환 손동작은 없습니다.
- 양쪽 새 Windows Player에 같은 팔 소스를 빌드한 검사, Android 포함·서명·제공·실기기 검증.
- 열린 소매 geometry 자체의 개선과 장치 성능은 별도 후속 범위입니다.

외부 근거는 `20261022-quality/arms-apply-01`, `arms-runtime-01`, `arms-fire-smoke-01/02`, `arms-motion-smoke-04`, `arms-material-02`에 보존합니다. 실패한 material dry-run은 native JSON wrapper 위치 진단으로 원인을 확인했으며, 의도한 m_Materials 한 property 외에 비교 예외를 추가하지 않았습니다.

## 출처와 저장 감사

기존 Slayver Survivalist character의 전용 FPS variant5와 취득 대응은 [에셋 목록](../../THIRD_PARTY_ASSETS.md), 기존 CC0 기반 소총 Idle/호흡/그립의 출처는 [애니메이션 기록](../AnimationBaking/README.md)을 따릅니다. 새 외부 모델·texture·출처 미확인22클립은 추가하지 않았습니다.

원래 각234 prefab record 중233개는 줄바꿈 정규화 후 동일하고 기존 ViewModelRoot의 자식1줄만 추가했습니다. 새172 record에 별도 리그가 들어가며, 후속 재질 보완은 해당 FPS Shirt3의 material 참조1곳씩만 바꿨습니다. 원 source material·texture·기존 prefab meta는 유지합니다.

명시 저장 API만 호출했어도 실제 디스크 감사에서 자체 CJK의 infinity glyph/atlas, 기존 Android alias flush, URP 설정 줄끝 변화3개가 함께 발견됐습니다. 생성본을 보존하고 Editor 정상 종료 요청·프로세스 부재를 확인한 뒤 전체 CAS 사전검사로 원래 raw3개를 복원했습니다. 종료가 글꼴 캐시를 비운 뒤 첫 CAS가 거절된 이력도 보존했습니다. 마지막 공개3941→3944개 감사에서 의도한 prefab2개와 material/folder/meta3개만 바뀌고 기존 dirty 설정5개는 처음 raw 바이트와 같습니다. 종료 watcher 보고 형식 오류로 ExitCode는 확보하지 못했으며 정상 종료 요청/부재를 ExitCode0으로 확대하지 않습니다.

## 본체와 1인칭 팔 Humanoid 바인딩 분리 (2026-10-22, #166)

별도의 팔 리그에 원본 본체와 같은 bone 이름·Avatar를 쓰면 root Animator.GetBoneTransform이 FPS 쪽 bone을 반환했습니다. 양 캐릭터의 격리 재개방에서7개 관측 bone이 모두 잘못된 FPS 계층을 가리켰고 root Rebind+Update0만으로 회복되지 않았습니다. 이 상태에서 준비 중이던 앉기는 잘못된 골격을 선택해 카메라·hip 누적 오차가 발생했으므로 첫42pose 실패를 보존했습니다.

FPS skin joint61개에만 SFF_FP_ 이름을 부여하고 기존 human map·bindpose로 전용 Humanoid Avatar를 새로 만들었습니다. 원본 FBX/Avatar·본체 Animator·기존 Idle/Death clip과 controller는 바꾸지 않습니다. Transform과 SMR bone 참조/pose·무기/그립/카메라·피격 판정의 IDs는 유지합니다. 잘못된 binding을 숨기려고 본체 Animator를 끄지 않습니다.

첫 AvatarBuilder 후보는 HumanDescription의 SkeletonBone을 복사하면서 원 FBX 부모 이름이 남아 valid human 검사를 실패했습니다. 새 후보는 원 bindpose를 적용한 독립 top-most clone의 실제 계층에서 SkeletonBone을 새로 구성합니다. Save0 검사에서 양 리그61/61 registry와 각 GetBone7 분리를 확인한 뒤 새 Avatar1개·대상 prefab2개만 저장하고 fresh contents로 다시 열었습니다. 비교 대상 Animator native JSON wrapper 내부의 Avatar1필드만 허용하며, probe의 sampled pose를 저장하지 않도록 저장용 contents를 새로 엽니다. global/scene Save0입니다.

독립 직렬화 감사에서 각 prefab의 기존406 record ID를 보존했습니다. rig PR의344 record는 LF정규화 바이트 동일,62변경은 FPS GameObject 이름61개와 FPS Animator Avatar 참조1개뿐입니다. 모든 Transform/SMR/기존 hitbox/본체 Animator record 및 기존 meta2개는 보존합니다. 현재 Editor에 준비 중인 crouch schema의 _IsCrouching 기본값2줄도 저장됐으나 이 rig PR index에서는 제외해 후속 crouch 변경으로 따로 둡니다. 공개3961→3963 전수 감사에서 이 저장 단계의 차이는 prefab2개·새 Avatar/meta2개뿐입니다.

추가로 별도 미병합 crouch 소스를 함께 import한 Editor에서, 독립 body registry와 원본 Avatar map을 측정 기준으로 양 캐릭터10회 crouch/stand 반복·총42pose를 검사했습니다. 카메라/발높이/hip 반복오차0, 발목 최대4.09e-7m, FPS local91개 및188 Transform scale 보존,11Box+HeadSphere의 broad containment를 확인했습니다. 이 결과는 rig PR 단독 clean checkout이나 실제 Runner physics/늦은 접속·연속Idle/Death·실제기기 검증이 아닙니다. 기존 standing head sphere가1.8m CC 상단을 벗어나는 advisory는 숨기지 않습니다. 실제 런타임·CanStand·새 Windows 경기의 후속 결과는 별도 기록합니다.

근거는 외부20261022-quality/arms-binding-candidate/revision-02/·apply-02/ 및 crouch-pose-01/pose-observation.v5.json·독립 리뷰입니다. 최초 Avatar 생성 실패와 미실행 Apply01·Posev4 검토 중 발견한 잘못된 guard도 보존합니다. APK27/AAB28에는 이번 변경이 없습니다.
