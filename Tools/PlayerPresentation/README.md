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
