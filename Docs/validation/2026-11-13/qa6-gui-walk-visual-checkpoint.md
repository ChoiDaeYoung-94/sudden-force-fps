# QA6 GUI 걷기 시각 체크포인트 · 2026-11-13

GUI Editor Host에서 폰 actor의 걷기 표본01→02 다리 위상 변화와 양손 소총 지지를 제한적으로 수용했습니다. 실제 URP 카메라 출력과 정상 자원·소스 반환을 확인했으며, 전구간 보행 품질이나 제품 전체 합격으로 확대하지 않습니다. 시각 판단은 독립 검토자의 원본 PNG 직접 관찰에 근거합니다.

검증 소스는 `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`/43 Networked이며 폰은 기존 `.localqa`/code6/`1.0.197-qa6` APK 그대로입니다. 이 문서는 main `26af` 기반 기록 후보로, 새 제품 APK 빌드·설치·배포는 없습니다. 시각 검증 소스를 문서용 main이나 제품39 layout으로 바꾸어 표기하지 않습니다. 아래 시각은 KST입니다.

자료 기준 `N=D:/codex/validation/sudden-force-fps/20261112-night`, `A3=N/qa6-runtime-launch01/actual-00de9a9f02854953a496f1bf512e561b`입니다. 새 Editor33288의 birth는 `2026-11-12T18:42:16.9072857Z`입니다. `fresh-gui-stopped01/02.result.json`에서 batchMode=false와 실제 URP asset/pipeline을 확인했고 QA flag 정착 후 MVID가643→eb8로 바뀌었습니다. 기존 reader·stopped bootstrap·camera 계열 inline을 새 nonce/native/MVID로 결속했습니다. batch 전용 RAM3 fixture/observer/compiler 재사용은0이며, inline eval 자체의 컴파일과 이를 혼동하지 않습니다.

`camera-idle01.result.json`은 caller의 출력경로 오결속으로, `camera02-idle01.result.json`은 inactive remote rig의 enabled renderer까지 거부한 기존 predicate로 중단됐습니다. 원문 실패는 보존했습니다. camera v3는 `!enabled || !activeInHierarchy` 한 predicate만 보완했고 원v1 역치환 byte-exact 독립 검토 후 defaultfalse/DISABLED를 확인했습니다. 출력은 기존 `N/qa6-runtime-preparation01/position-diagnostics01/actual-render-{renderNonce}/remote.png` 규칙 그대로입니다.

실제 PNG_WRITTEN은5회/고유 SHA4개입니다. 각 결과의 actor·renderer·기존 camera·scene roots/dirty·frame/tick 보존과 임시 객체 cleanup=true, actor/original-camera/renderer-flag wrapper write0를 확인했습니다. V03의 `camera03-idle01.result.json`은 별도 actor의 정면 idle 한 각도입니다. 이를 V04의 시작 idle이나 같은 actor의 연속 frame으로 합치지 않습니다.

V04의 `v04-camera-walk01/02.result.json`은 약0.91초 간격의 같은 정면 구도에서 다리 벌어짐·무릎 굽힘·들린 발의 변화와 상체/양손 소총 지지를 보여 줍니다. `v04-camera-walk03.result.json`은 벽으로 정면 LOS가 막혀 검증 카메라가45도로 바뀐 표본입니다. actor 회전으로 해석하지 않았고, 직후 Reader의 velocity0과 같은 위치 때문에 벽 근처 정지/전환 범위로 한정했습니다. 뒤 `v04-camera-idle01.result.json`의 PNG SHA가03과 같으므로 새 보행 위상으로 중복 합산하지 않습니다.

`native-walk-held01.public.json`은 일반 joystick 전진 hold의52event/error0/injectedAll·pointer clear를 기록합니다. profile 이름이 held-life여도 death/respawn/old-held 차단 검증은 아닙니다. 입력 상대시계와 render frame은 정밀 동기화되지 않았으며, 좌표차/UTC차를 순간 속도로 취급하지 않았습니다. 세 정지 표본만으로 부드러움·foot sliding0·모든 cycle의 발 접지·정지 후 양발 plant·세밀한 손 접촉까지 수용하지 않습니다. 이번 양 actor는 Host state authority 관측이며 pure proxy나 질주 검증도 아닙니다.

V04는 제품 RETURN callback을 합성 호출한 뒤, Reader에서03:57:43 Connecting→03:58:06 Connected와 actor/fixture0를 확인했습니다. 버튼의 실제 클릭 검증으로 표기하지 않습니다. QA 원문 symbols 복원 후 stopped/compile·update false, MVID643/QAflagfalse로 정착했습니다. 종료 RPC의 Invalid response format 실패는 보존하고 별도 원핸들 Exit0/forcedfalse(03:59:38), Prefs2 복구·capture 완료로 실제 종료를 확인했습니다. 폰 최종 영수증은03:59:11 game/helper 부재·설정1/0/0·공유 ADB 동일 세대를 기록합니다.

이번 생성 변경2개는 Root의 새 분류에서 Resolver의 CRLF/끝공백, ProjectSettings의 CRLF만이었습니다. Resolver bool3 의미 변화는 없으며 원본·생성본 보존 후 CAS2 복원했습니다. Root 최종 영수증은04:01:05 공개4,122개 exact/private6 absent-indexS/Git clean, SDK candidate0를 기록합니다. 모두 원full04:12:10 이전입니다. 독립 반환 검토는 영수증·보존2쌍 hash·PNG 핀 대조이며, 본인의 새4,122개 전수감사·Git/프로세스/기기 재검사는0입니다.

남은 연속 외형 품질, 순수 proxy에서의 피격·사망·재출현 경계, 전체 권리·실제16KB·제품 기본 gait/39 layout 수용은 별도 범위입니다. Host의 사격·사망·재출현은 [앞선 A2 기록](qa6-host-combat-and-visual-checkpoint.md)의 한정 완료 범위를 유지합니다. 과거 실패는 보존하되 A3의 새 긍정 표본은 위 범위로 수용하며, A1의 과거 미수용 상태를 A3 전체에 일반화하지 않습니다. 현재 필요한 사용자 조치나 질문은 없습니다.

핵심 근거 SHA256(원본 불변):

- 시각 IR `A3/independent-a3-gui-walk-visual-review.public.json`: `2B13064D20484A9DE4D26E2BE7A1E109D65C2FBC74F47D7F3FD933BFE8F9A9B5`
- 반환 IR `A3/independent-a3-final-return-review.public.json`: `9F8A90C6C5955D5BBFCDA93B39F1085E1486ECB5BE9071A154BD93EF84B6738F`
- v3 source IR `N/qa6-runtime-preparation01/position-diagnostics03/independent-review.public.json`: `A4378ECB7D91C196A006C046A22BAABDE94ECC101832BD22895817A035CECD3D`
- 최종 소스 반환 `A3/source-return01/final-source-return.public.json`: `D1DA4E9B6DCA76917923AE388687E432D56195145FF5305A474E7384DF06E71A`
