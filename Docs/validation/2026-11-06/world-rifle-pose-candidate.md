# 3인칭 소총 파지·판정 연결 후보

관련: #342, #166, #41. 현재는 **Draft**이며 제품 컴파일은 확인했고 실제 판정 capture·두 피어 경기·Android 수용 전입니다.

코드34의 상대 캐릭터는 총이 얼굴 가까이 있고 팔이 가슴 앞에서 교차했습니다. 같은 모델의 양팀·세 각도 기준 화면6장과 배치 후보18장을 확보한 뒤, 균형 배치를 유지하고 왼손 상대 위치만 바꾼 비교18장을 추가했습니다. Root는 A안의 손 위치를 우선 선택했습니다. 이 화면들은 격리된 preview의 시각 실험이며 새 제품 코드의 실제 경기 화면이 아닙니다. 손 피부·총의 비관통을 확정하지 않았습니다.

## 후보 동작

- 하나의 시각 골격 소유자가 source pose를 한 번 복사하고, 양팔 IK를 적용한 뒤 기존12 hitbox child와 기존 월드 소총을 같은 자세에 맞춥니다. 보행 클립9개가 없어도 소총 자세를 적용할 수 있습니다.
- 기존 소총 renderer7개와 캐시된 월드 renderer15개를 재사용합니다. 추가 총 renderer를 만들지 않으며 로컬1인칭의 ShadowsOnly 정책을 유지합니다.
- capture 직전에는 stance 적용 뒤 simulation context를 사용합니다. 렌더에서는 stance 뒤, 즉시 총구 효과를 만드는 RenderState 앞에서 적용하고, order5000 stance 복원 뒤 order6000에서 다시 적용합니다.
- source humanoid 본·shape·BodyPart·기존 serialized ID·capsule·eye·aim/raycast·HP/ammo·권한은 수정하지 않습니다. 기존 dedicated hitbox child와 RiflePresentation의 runtime Transform이 의도한 변경입니다. 실패·disable·unbind는 attachment local pose와 skin 참조를 복구합니다.
- 이전 모델과 혼용하지 않도록 테스트 접속 protocol suffix를 분리합니다. 새 phase/velocity Networked 필드는 추가하지 않았습니다.

기본 배치는 전방0.15m/하강0.08m/안쪽0.025m이고, 왼손의 rifle-local 위치 보정은 A안 `(-0.04,+0.03,0)m`입니다. 회전·손가락 local pose는 이번 비교에서 바꾸지 않았습니다. 기존 actual02의 양팀 보정값을 고정해 사망·재enable 중 자세에서 재보정하지 않습니다.

## 확인된 범위

제품 후보의 독립 소스 검토는 확정 source blocker 없이 컴파일·fixture 단계로 진행하는 범위만 수용했습니다. 원 코드의 source-only 검토와 실제 runtime 검증을 구분합니다. 통합 제안의 EOF 빈 줄1개씩만 정리했고 `git diff --check`를 통과했습니다.

별도 P12의 detached `535abffbda6e52dd0d08cf9961e58df60795207d`를 Unity 6000.3.25f1 Windows batch/nographics로 열어 컴파일하고, 현재 공개 입력 6개 C#·4개 meta의 raw SHA를 전후 대조했습니다. loaded runtime assembly의 네 타입·11개 method signature metadata를 확인했고 managed complete=true, native/tool 종료0·강제 종료 없음·Dispose 오류0입니다. 제품 생성자·메서드·getter나 Runner를 실행한 시험은 아니며 source와 loaded IL 본문 일치를 별도로 증명한 것은 아닙니다.

종료 뒤 자동 직렬화된 EDM 한 경로의 개행/빈 값 끝공백 변화만 원 SHA로 복원했고, 공개 추적 입력 4,065개가 검증 전 바이트와 일치했습니다. 임시 helper/metadata 3개를 외부에 보존한 뒤 정리했고 P12 Git clean/Temp lock 부재를 확인했습니다. 기본 P2의 기존 설정 5개는 그대로입니다. 실행 근거는 외부 `rifle-product-compile-native01/`, `rifle-product-compile-candidate01/managed-actual01/`, `window07-root-completion.public.json`이며 독립 자료 검토를 별도로 남깁니다.

기존 시각 분리 prototype은 부위별 판정이 그대로인 실험입니다. 기존 helper의 수치·count 보존이나 CPU mesh 표본은 새 제품의 mapped hitbox·네트워크 기록 정합성 근거가 아닙니다. 부위별 피해는 Head100/Torso25/Arm18/Leg18이므로 전체 외접 상자만으로 수용하지 않습니다.

## 남은 필수 확인

1. 등록된 HitboxRoot에서12 child 매핑·부위별 coverage·broadphase·원복 확인.
2. 실제 callback/NCC/stance/muzzle 순서, 사망·리스폰·앉기·climb fallback·재enable·fault latch 확인.
3. 두 피어의 표시·capture·실제 사격/피해와 정상 종료. 기존 Windows 종료 조사 전체를 해결한 것으로 확대하지 않습니다.
4. 실제 SkinnedMeshRenderer 경계·재진입, 새 Android 포함·기기/접촉·실청 검증.

공통 시각 골격은 보존된 Draft344의 구현을 공유합니다. 기본 P2와 기존 dirty5, 기존 보행 Draft와 실패 자료는 보존하며 별도 P14에서 이 후보를 작성했습니다. private6는 no-checkout 단계에서 먼저 제외했고 내용 조회·해시 없이 부재/indexS를 확인했습니다.

현재 제공 APK34/release12와 별개입니다. 새 Android 예약·빌드·서명·업로드는 하지 않았습니다. 이 문서는 preview의 기술 완료를 제품 완료로 표시하지 않습니다.
