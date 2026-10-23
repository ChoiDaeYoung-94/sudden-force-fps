# 1인칭 팔 표현과 검증 범위

## 코드30 포함과 시각 후속 경계 (2026-10-23, #188/#192)

PR189/191을 포함한 source15196b8에서 코드30 무서명 AAB·새 IL2CPP와 정적 검사/심볼6/7을 확인했습니다. [배포 점검표](../../ANDROID_RELEASE_CHECKLIST.md)를 따릅니다. CanShow/LateUpdate·본/탄창 참조·좌표 변환·단일 solve·선택 복원·실패 finally·OnDisable·Finite 등 생성 C++11대표 본문이 C# 제어 흐름과 대응합니다. P2 LF/P6 CRLF의 raw SHA 차이는 정규화 본문 동일로 구분했고 최종 native 함수 생존·기기/GPU로 확대하지 않습니다.

기존 idle(frame3193) 뒤 별도 QA가 HasAim/yaw90/pitch−60을 보내 middle(frame3359)을 촬영했으므로 고정 시선 비교쌍이 아닙니다. 캡처 도우미 inputProduced=false도 별도 QA 입력0을 뜻하지 않습니다. effective Aim/camera quaternion·recoil·마우스·writer 순서 전수 시계열이 없어 정확 각도·독점 원인은 단정하지 않으며 새 제품 버그 확정/기존 PC 표본 취소는 아닙니다.

다음에는 idle 이전부터 같은 조준을 유지하고 사전 발사 후 recoil 회복을 확인한 뒤 같은 FOV·화면비로 비교하며 input/Aim/Recoil/camera/frame/tick을 남깁니다. 전체 손·탄창 화면 포함/접촉·사망/리스폰·Android는 미완료입니다. 제공27에 미포함이며 무서명30도 서명·기기 실행 전입니다. 근거는 외부 `20261023-work/android30-artifact-readiness01/revision02/actual-01/`, `android30-reload-cpp-body-independent-review01/`, `android30-visual-followup-audit01/`입니다. 아래는 각 시점의 기록입니다.

## 왼쪽 소매 프레이밍과 재장전 합성 (2026-10-23, #190)

독립 FPS 왼쪽 쇄골을 무기 기준 뒤로.10m 이동하고, 왼손은 원래 Idle 그립 또는 재장전 목표로 한 번만 IK 계산합니다. 화면 아래쪽의 열린 소매 입구가 덜 드러나도록 한 보정이며 geometry 밀봉·새 모델/클립 추가는 아닙니다. 새 serialized 필드·prefab 저장 없이 C# 한 개만 변경합니다. 오른손 anchor·무기/총구·본체/피격/네트워크는 유지합니다.

Idle 보정 상태와 재장전 상태를 분리해 idle/phase0·1은 support=true/reload=false입니다. 이전 출력과 같은 쇄골 위치/팔 회전만 선택 복원해 누적을 막고 fresh Animator 값을 보존합니다. 명시 Reset·잘못된 참조·disable·실패한 IK는 원래 쇄골/팔/탄창으로 복구합니다. 정상 재장전 완료는 원래 그립의 idle 보정으로 돌아옵니다. 새 쇄골 대응이 없는 리그는 보정/재장전을 거절하고 기존 Idle 팔 표시로 남습니다.

두 팀7단계×3화면비(1024×768/1280×720/1728×729)의 CPU BakeMesh42장과 반복100회6조건·fresh clone30조건을 확인했습니다. 기존 보호 범위에서 의도한 쇄골 localposition 하나를 추가 허용한183 Transform의 오차0, 원복0, source/meta/기존 설정15파일과 scene 보존을 확인했습니다. 기존 helper의 Dictionary key 형식 오류는 실행 전에 발견해 원본을 보존하고 새 revision02에서 수정했습니다. 미호출 컴파일과 실제 검사를 구분합니다.

새 Windows 빌드는 오류0/경고2·22.0313716초이며360파일 SHA를 확인했습니다. 실제 predicting client의272표본에서 support272, idle153개 모두 idle 보정, 최대 offset 오차 약1.02e-6m,4발→재장전30→26→30·탄창 약.18m 하강·오른손 그립 최대 약1.44e-6m를 확인했습니다. 원격 host도 탄약 순서를 수신했습니다. 실행 중 URP 카메라의1280×960 idle/중간2장을 직접 관찰했지만 손/탄창 일부가 화면 밖이므로 전체 접촉·모든 자세의 시각 품질은 미수락입니다.

메뉴는 `PlayerInputSource`의 로컬 입력을 막고 화면의 idle 보정은 유지합니다. `CanProvideInput` 자체가 false가 된다는 첫 검증기 기대값은 틀려 보존했습니다. 실제 후속 한 표본씩에서 Menu/inputBlocked=true와 정상 Resume/Closed·idle support/탄창 원위치를 확인했으며 물리 키·지속 누르기 통과는 아닙니다. 사망/리스폰·Android·장치 성능은 후속입니다. 코드29에는 미포함이며 다음 Android 통합은30 이상입니다.

정상 Lobby/닉네임 복원·actor/QA0·소유 host와 Editor의 열린 handle ExitCode0 이후 생성4파일을 보존·복원하고 임시 QA source/meta만 외부 보존 후 제거했습니다. 기존 dirty5는 시작 raw 바이트와 같고 공개3968개 중 의도한 당일 제품/문서6개 외3962개가 일치합니다(.env1개는 후속 감사에서 조회 제외). 빌드 Running 중 조기 읽기 거부는 성공으로 세지 않았습니다. 근거는 외부 `20261023-work/shoulder-framing-validation/`, `shoulder-runtime-01/`, `shoulder-camera-runtime-01/`, `Shoulder-host-final-summary.public.json`, `final-pc-restoration-01/`과 독립 검토입니다.

## 왼손·탄창 재장전 (2026-10-23, #188)

로컬 FPS 리그의 왼팔 IK와 기존 소총의 탄창 mesh를 실제 `ReloadRemaining/ReloadDuration`에 맞춰 움직입니다. 오른손 그립과 무기·총구, 본체 골격, 발사/피해/탄약 규칙은 바꾸지 않습니다. 두 player prefab에는 기존 탄창 Transform 참조만 추가했습니다. 기존 CC0 Idle 이외의 출처 미확인 재장전 clip은 사용하지 않습니다.

이전 출력과 같은 팔 자세만 선택 복원해 새로운 Animator 자세를 보존하며, 완료·잘못된 참조·표시 불가·비활성화에서는 탄창과 팔을 복구합니다. 격리된 두 팀의7단계·반복100회·잘못된 참조/비활성화 검사에서 보호 Transform184개의 변화0과 복원을 확인했습니다. 첫 Edit 미리보기는 IK가 이미지에 반영되지 않아 시각 통과로 쓰지 않습니다. 후속 CPU BakeMesh 미리보기14개와 실제 경기의 실행 중인 URP 카메라 캡처를 별도 근거로 둡니다.

새 Windows 검증 빌드는 오류0/경고2이며360개 출력 파일의 SHA를 확인했습니다. 실제 Photon 두 피어 경기의 Blue predicting client에서 합성 입력으로 두 번 각각4발 발사→재장전하여 탄약30→26→30을 관측했습니다. 각272/273표본에서 탄창 최대 하강 약.18m·왼손 이동·오른손 그립 최대 오차 약1.27e-6m를 확인했고 별도 Red host도 두 탄약 순서를 수신했습니다. 정상 로비 복귀·QA 입력/객체 제거·닉네임 복원과 소유 host ExitCode0을 확인했습니다.

실행 중인1280×960 카메라의 idle/중간 캡처에서도 왼손 이동이 보입니다. 일부 손과 탄창이 화면 밖으로 내려가므로 모든 손가락 접촉·전체 교환 동작의 완성도를 통과한 것은 아닙니다. 실제 사망/리스폰·물리 터치·Android 화면과 다른 화면비는 후속입니다. 코드29는 이 변경 전 산출물이며 다음 Android 통합은30 이상입니다. 빌드가 함께 flush한 CJK/기존 설정 생성본은 별도로 보존하고 제품 변경과 분리합니다.

근거는 외부 `20261023-work/reload-implementation/`, `reload-pose-validation/revision-01/02`, `reload-windows-01/`, `reload-runtime-01/02`, `reload-camera-runtime-01/`, `Reload-host-final-summary.public.json`, `reload-actual-review-01/`입니다. 최초 미리보기의 한계와 실패한 외부 검증 helper도 보존합니다.

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

## 앉기 자세·머리 위 공간·터치 조작 (2026-10-22)

아래 검증 자료의 기준 경로는 외부 `D:/codex/validation/sudden-force-fps/20261022-quality/`입니다.

앉기는 `CustomPlayerInput`, `GamePlayerNetworkData`, `PlayerInputSource`, `PlayerCrouchGeometry`, `CombatHudView`, `CombatPointerControl`의 C#6개와 새 CROUCH 조작으로 연결했습니다. NetworkButtons의 Crouch와 터치 Control은 모두3이며 Networked `IsCrouching`을 추가했습니다. 네트워크 schema39의 새 Windows 검증 빌드를 사용하며 기존 schema의 Player와 혼용한 통과는 아닙니다. Ctrl 또는 CROUCH 요청을 처리하고 앉은 상태가 달리기보다 우선하며, 소스의 속도 설정은 앉기2.5m/s·일반 이동5m/s입니다. 이 값은 실제 속도 비교 측정 결과가 아닙니다.

실제 자세는 몸 전체 scale을 줄이지 않고 원본 Humanoid 몸체 골격의 Hips 하강과 양쪽 허벅지·종아리 IK로 구성합니다. 실제 캡슐 높이는1.8→1.3m, 로컬 eye Y는1.75→1.15m이며 뒤로 .08m 이동합니다. SimulationShotOrigin도 같은 stance eye 계산을 사용합니다. 골격은 원본 body skin registry와 Avatar map으로 선택하고 FPS 리그를 제외하며 실제 Hips 대응은 이름이 pelvis인 자식이 아닌 `root`입니다. Animator를 끄는 회피 없이 본체 Animator와 일치하는7개 bone을 요구하고 hitbox 등록 전/표시 단계에서 자세를 적용합니다. 원래12 hitbox의11Box+1HeadSphere 형상과 피해 판정 경로를 임의 변환하지 않습니다.

머리 위 공간은 자기 Runner의 실제 PhysicsScene에서 서기 위한 상단 swept capsule을 조회합니다. 자기 CharacterController·자기 하위 collider·CombatHitbox·trigger를 제외하고 실제 collision matrix의 solid mask를 사용하며 포화는 안전하게 거절합니다. 장애물이 있으면 Ctrl/CROUCH를 놓거나 입력 Reset·메뉴 진입이 발생해도 앉기를 유지하고 공간이 생기면 서기로 복귀합니다. 메뉴·수명주기 입력 초기화는 물리적으로 키/손가락을 놓았다고 가정하지 않으며 새로운 유효 입력을 요구하는 소스 gate를 유지합니다. 이 gate의 실제 물리 Ctrl·터치 수명주기 검증은 아래 합성 검증과 별도입니다.

첫 격리 pose v2는 몸체/FPS에 같은 bone 이름과 Avatar가 있던 상태에서 본체 Animator가 FPS bone을 선택해42자세 계약이 실패했습니다. 잘못된 cache가 camera를 복원하고 FPS hips를 누적 이동시키던 관측과 원문을 보존했습니다. FPS 복제 골격에 고유 prefix와 별도 `FirstPersonArmsPrefixedAvatar.asset`를 적용하고, 원본 body registry/Avatar 및 두 Animator의 bone identity를 각각 확인하도록 보완했습니다. 원본 모델 FBX를 일괄 수정하거나 본체 Animator를 비활성화하지 않았습니다. v4의 정상 head skin rootBone을 거절하던 fixture guard 오류도 보존하고 v5에서 actor 직계 body root 기준으로 고쳤습니다.

최종 Edit 격리 pose v5는 두 prefab 각21자세, 총42자세에서 checks=true/contractFailures0입니다. FPS local91 Transform·전체188 Transform scale을 보존하고 eye/hip/repeat 오차0, 양발 위치 오차 최대4.09e-7m를 확인했습니다. 원본 형상별 Box8모서리·Sphere broad containment와 crouch head containment도 통과했습니다. 기존 standing head sphere가1.8m capsule 밖에 있는 자세의 advisory는 별도 보존하며 이번에 해결했다고 쓰지 않습니다. 이 검사는 임시 prefab contents의 Save0/Networked getter0인 격리 검사이며 실제 FUN·천장·예측·피격·death/respawn 검증을 대신하지 않습니다. 작성자 분석과 별도 독립 pose 검토도 구분합니다. 근거는 `crouch-pose-01/pose-observation.v5.json`과 `independent-v5-review.md`입니다.

원본 구현5개에 붙었던 UTF-8 BOM·여분의 파일 끝 줄바꿈으로 발생한 형식 검사 실패를 보존했습니다. `crouch-apply-01/source-format-01/`의 before/after SHA와 원문 대조에서 BOM 제거·최종 newline1개 외 본문 동일을 확인했고 Geometry673D는 그대로 유지했습니다. 새 HUD fixture의 현재8핀은 형식 정리5개를 모두 포함하며 이전 후보/핀을 덮지 않았습니다. 실제 schema39 Windows02 development 빌드는 Succeeded/오류0·경고2/72.3299322초입니다. 첫 Windows01의 Pipeline 요청 timeout 실패는 별도로 남기고, delayCall에 queue한 것 자체가 성공이라는 뜻으로 쓰지 않습니다. 성공 근거는 `crouch-windows-02/Player/validation-build.json`이며 추가 APK/Android 빌드 검증은 아닙니다.

실제 Photon2피어 경기의 로컬 호스트 입력 소유자에서 `crouch-headroom-runtime-01`의6단계가 통과했습니다. 새 source/prefab/scene/DLL10핀과 실제7 bone registry·12 hitbox·root Animator enabled를 확인한 뒤, 합성 NetworkEvents.OnInput186회(Crouch31회)를 실제 FUN에 전달했습니다. 같은 Runner physics scene에만 QA BoxCollider를 바닥1.45m/중심1.6m/높이.3m/폭1.2m로 만들고 자기 crouch CC/head와2.5cm 여유·다른 actor 근접·mask·실제 query 반환을 검사했습니다.

| 실제 단계 | IsCrouching | CC 높이 / eye Y | CanStand / 메뉴 |
|---|---|---|---|
| 초기 서기 | false | 1.8m / 1.75m | true / Closed |
| 앉기 요청 | true | 1.3m / 1.15m | true / Closed |
| QA 천장 아래 SetCrouch(false) | true | 1.3m / 1.15m | false / Closed |
| QA 천장 아래 ResetInput | true | 1.3m / 1.15m | false / Closed |
| QA 천장 아래 메뉴 | true | 1.3m / 1.15m | false / Menu |
| QA 박스 제거·메뉴 복귀 | false | 1.8m / 1.75m | true / Closed |

총158표본에서 geometry와 stance가 일치했습니다. 각 단계는 정상 관측 사이30tick 이상 span을 요구하지만 Editor.update에서 중간 tick을 건너뛴 구간도 있으므로 모든 FUN tick을 개별 검사한 것은 아닙니다. QA 박스는 호스트 전용이며 원격에 복제되지 않습니다. 검증 fixture의 Networked/actor Transform 직접 setter·Teleport·Physics.Simulate·Save는0이고 최종 QA 박스/fixture 제거·callback 해제·실제 입력 중립·메뉴 Closed/unblocked·10핀 일치를 확인했습니다. errors/cleanupErrors는 모두0입니다. 이 결과는 합성 NetworkInput을 통한 실제 host FUN/CC/headroom 경로의 근거이며 물리 Ctrl gate·클라이언트 예측/재시뮬레이션·지속 누르기·피격 lag compensation·이동 속도 비교로 확대하지 않습니다. 독립 대조는 `crouch-headroom-runtime-01/independent-review.md`에 보존합니다.

CROUCH 터치 버튼은 기존 styled SPRINT를 새 복제본으로 만들고 Control3·자기 feedback pointer·흰 앉은 사람 실루엣을 부여했습니다. 기존 세 control/joystick/HUD 연결은 보존했고, 저장 씬의 기존1,245레코드 중1,244개는 바이트 동일합니다. 기존 변경1개는 TouchControls 자식 목록에 새 버튼을 추가한 것뿐이며 삭제0·신규45개는 새 버튼 하위입니다. 새 장식은 raycast를 차단하지 않습니다. 1280×720·1920×1080·2340×1080·2560×1080의4해상도/3화면비 렌더에서 TMP overflow0을 확인했고 2340만 합성 안전 여백입니다. 실제 기기 safe area 검증은 아닙니다.

실제2피어 host의 `crouch-hud-runtime-01/revision-02/state.json`은 4 pointer+joystick·메뉴5캡처 검사를 통과했습니다. Crouch3이 sprint 대신 crouch 입력을 세우고 소유자의 Up/Cancel과 다른 포인터 거절·색/장식 크기·메뉴 해제 및 최종 중립을 확인했습니다. 보호 객체31개/Rect46개의 diff0·검사 source8핀 동일·네트워크 관측 불변이며 단일 콜백의 Fusion tick은9498→9498입니다. direct callbacks/ExecuteEvents와 private Update175회는 합성 호출이며 물리 터치·native input module·경과 프레임 응답 시간 검증이 아닙니다. 터치 미리보기는 원래 false를 보관해 먼저 켜고 실제 프레임 경과 뒤 검사했으며 별도 복구 응답에서 원래 false 복원을 확인했습니다. 실제3040×1440 화면은 같은 폴더 `actual-screen-touch.png`에 있고 Android 화면이 아닙니다. 독립 HUD 검토와 실제 화면 대조도 같은 자료 폴더에 보존합니다. 물리 터치 통과로 확대하지 않습니다.

추가 `crouch-client-key-01`의 호스트 읽기 전용 관측은 원격 client stance의 true1표본(tick13351) 뒤 false(tick13352)를 기록했으며 host input/Networked/actor Transform/collider 쓰기는0입니다. 해당 구간에 새 Windows client 창으로 네이티브 Control_L 입력을 전달한 기록과 독립 대조했습니다. 따라서 원격 native client 입력에 따른 host authoritative stance의 짧은 전환 근거이며, client 화면에서 수신 stance를 측정하거나 긴 키 유지·예측 지연을 통과한 것은 아닙니다. 실제 물리 키의 긴 held/release gate, client 화면·예측 지연·지속 복제·속도 비교와 death/respawn·경기 종료 조합은 미검증입니다. 기존 APK27/AAB28에는 이번 앉기·prefix Avatar·새 HUD가 미포함이며 추가 APK/Android/실기기 통과를 주장하지 않습니다.

Editor와 Windows Player는 각각 열린 process handle로 정상 ExitCode0을 확인했습니다. 종료 후 생성 CJK/설정 원문을 외부에 보존하고 CAS 사전검사로 복원했습니다. 공개3,959개 최종 원장에서 제품 변경10개와 기존 dirty5개 외 차이가 없는지 별도 감사합니다. 원래 설정4개는 최초 raw 바이트와 같고 PlayerSettings는 의도해 병합한 아이콘 목록2필드 외 최초 raw 바이트를 유지합니다. key 내용 조회·새 버전 예약·서명·업로드는0입니다. 근거는 `crouch-apply-01/final-restoration-proof.json`, `public-after-restoration.json`과 종료 handle 기록입니다.
