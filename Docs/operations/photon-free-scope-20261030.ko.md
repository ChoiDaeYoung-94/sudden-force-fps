# Photon 무료 사용 범위 — 2026-10-30 확인

관련: [#301](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/301). 공식 일반 조건 조사이며 계정의 현재 플랜·실제 청구액 확인과 구분합니다. 계정·요금제·App ID·리전·결제 설정은 변경하지 않았습니다.

## 지정 계정 화면 후속 확인 — 2026-10-30 20:55~20:56 KST

사용자가 지정한 기존 작업용 Photon 탭을 Root가 읽기 전용으로 확인했습니다. 유일하게 대응한 기존 탭의 목록과 상세에서 **SuddenForce / Multi play FPS game / Fusion 2**를 확인했습니다. 해당 앱은 **개발용 Fusion 20 CCU·월60 GB**, 합계20/CCU Burst 없음이며 subscription·one-time·coupon 기여는 각각0으로 표시됐습니다. 무료100 CCU 안내 배너는 있었지만 이 앱에100을 적용한 상태는 아닙니다.

목록의 실제 트래픽 사용량은 `-`이고, 통계 기본 기간의 CCU 그래프에서도 숫자를 확보하지 못했습니다. **`-`/빈 그래프를 사용량0으로 해석하지 않습니다.** 결제수단·청구서·계정 전체의 무료 슬롯 자격을 전수 검사하지 않았으며, 제품 설정의 App ID와 직접 대조한 결과도 아닙니다. 사용자 지정 탭·앱 이름·FPS 설명·Fusion2를 근거로 식별한 관측입니다.

현행100 CCU 무료 옵션의 범위 조사는 완료했으며, 실제 적용은 별도 변경 작업입니다. 플랜 변경·계약/결제 동의·계정/앱 생성·설정 수정·업로드는0이고 원래 목록 화면으로 복귀했습니다. 출시 준비에서는 현재20을 무료100으로 이미 적용했다고 쓰지 않고, 해당 앱의 자격/변경 조건을 확인한 뒤 필요한 사용자 지시를 받아 적용해야 합니다. 청구액0·공개 출시 허가·전체 계정 권리 확인을 이 관측으로 확정하지 않습니다.

외부 공개 영수증은 `20261030-work/photon-account-observation01.public.json`(SHA256 `5F228E5BBEDD455AAC52A2B2077DC2EE1EBE79E9689BE585F90F57D3AF841629`)입니다. Root UI 관측의 최소 전사이며 독립 검토자가 계정을 다시 조회한 결과가 아닙니다. App ID·라이선스 해시·계정/결제정보는 공개 영수증과 이 문서에 복사하지 않았습니다. 앞선 PR304의 계정 미확인 기록은 Git 이력에 보존하며, 현재 판단은 위 실제20 CCU 관측을 따릅니다.
## 무료로 가능한 범위

| 플랜 | 동시 연결 한도 | 월 포함 트래픽 | 용도 |
| --- | ---: | ---: | --- |
| 개발 무료 | 20 CCU | 60 GB | 개발 전용 |
| 출시 무료 | 100 CCU | 0.3 TB | Fusion/Quantum 게임의 개발·출시·상업 이용, 고객/스튜디오당 앱 하나 |

현재 [Fusion 공식 가격표](https://www.photonengine.com/fusion/pricing) 기준입니다. 100 CCU 무료 플랜은 자동으로 모든 앱에 적용되는 조건이 아니며 해당 앱에서 활성화돼 있어야 합니다. 개발용 20 CCU만으로 앱스토어 배포가 허용된다고 가정하지 않습니다. [공식 라이선스 설명](https://doc.photonengine.com/photon/current/pricing#free-plans--licensing)을 함께 확인했습니다.

CCU는 누적 가입자나 한 방의 정원이 아니라 **연결된 모든 클라이언트**입니다. 방 밖 로비도 연결을 유지하면 예산에 포함합니다. 과금의 Peak CCU는 월별 각 리전 최고치의 합입니다. 예를 들어 한국 최고60·일본 최고50이면 서로 다른 시각이어도 합110입니다. 약4만 MAU라는 안내는 추정치이며 보장 인원·무료 이용 한도가 아닙니다. [공식 CCU 설명](https://doc.photonengine.com/photon/current/pricing#understanding-ccu), [Fusion 용어집](https://doc.photonengine.com/fusion/v2/reference/glossary#concurrent-user-ccu).

## 우리 게임에 적용할 때

현재 방 생성은 Host/Client 방식이며 총인원은 짝수 2~8명입니다. 내부 roster 상한16과 실제 방 생성 정원8을 구분합니다. 채팅은 Fusion RPC를 사용하며 별도 Photon Chat 구독을 전제하지 않습니다. SDK·호출 근거는 아래 프로젝트 근거를 따릅니다.

같은 앱·리전의 모든 사용자가 경기 안에 있고 다른 로비/시험 연결이 없다는 계산 예시는 다음과 같습니다. 실제 수용 인원·서버 성능 검증 결과가 아닙니다.

| 방당 연결 인원(호스트 포함) | 20 CCU에서 가득 찬 방 | 100 CCU에서 가득 찬 방 |
| --- | ---: | ---: |
| 2 | 10 | 50 |
| 4 | 5 | 25 |
| 8 | 2, 잔여4명 | 12, 잔여4명 |

플랜이 허용하는 CCU 안이어도 트래픽은 별도입니다. 실제 사용량을 재지 않았으므로 월 무료 경기 수나 동시8명 방12개의 FPS·지연을 보장하지 않습니다. Host는 플레이어 기기가 게임 서버 역할을 하며, Photon 요금에 별도 전용 게임 서버 컴퓨팅이 포함되는 것으로 해석하지 않습니다. 전용 서버로 바꾸면 자체/외부 호스팅 비용이 추가됩니다. [공식 topology 설명](https://doc.photonengine.com/fusion/v2/fusion-choose#server-complexity).

## 한도를 넘을 때

20/100 CCU에는 CCU Burst가 없습니다. 한도에 도달하면 추가 연결이 거절될 수 있고, 경기 뒤 로비로 재접속하는 사용자도 영향을 받을 수 있습니다. 무료 플랜이 자동으로 무제한 유료 플랜으로 바뀐다고 가정하지 않습니다. [공식 연결 오류 설명](https://doc.photonengine.com/realtime/v5/troubleshooting/subscription-error-cases#ccu-limit-reached).

Photon Cloud로 들어가고 나가는 트래픽이 포함량을 소모합니다. 초과분은 무료가 보장되지 않습니다. 공식 FAQ는 초과 트래픽 안내·청구와 리전별 GB당 US$0.05/0.10을 설명하며, 한국은 현재 가격표의 US$0.10 그룹입니다. 실제 무료 계정의 청구/결제 수단 처리와 세금·환율은 확인하지 않았습니다. 초과 시 무조건 무료 차단이라고 안내하지 않습니다. [트래픽 FAQ](https://doc.photonengine.com/realtime/v5/troubleshooting/faq#how-much-traffic-is-included-in-my-photon-plan-and-what-happens-if-my-app-generates-traffic-beyond-the-included-limit), [Fusion 가격표](https://www.photonengine.com/fusion/pricing).

## 더 많은 인원이 필요할 때

현재 가격표에는 무료100에 12개월 US$95 일회성100을 더한 총200 CCU와 월500/1,000/2,000 CCU의 US$125/250/500 옵션이 있습니다. 일회성 플랜은 자동 갱신 없이 만료하며, 유료 월 구독과 무료100의 중복은 허용되는 것으로 쓰지 않습니다. 2024년 도입 블로그의 결합 불가 문구보다 **현재 제품 가격표와 현재 공통 가격 문서의 결합 허용 조건**을 기준으로 했습니다. 유료 플랜 앱을 무료 플랜으로 내리는 것은 현행 규칙상 허용되지 않으므로 시험 삼아 유료 전환하지 않습니다. 실제 구매·플랜 변경은 하지 않았습니다. [현재 가격표](https://www.photonengine.com/fusion/pricing), [현재 결합 규칙](https://doc.photonengine.com/photon/current/pricing#rules-of-the-free-100-ccu-plan), [과거 도입 글](https://blog.photonengine.com/new-free-100-ccu-for-photon-fusion-and-quantum-games/).

## 현재 판단과 남은 확인

지정 대시보드의 SuddenForce는 현재 개발용20 CCU이며 무료100은 미적용입니다. 무료100이 해당 앱에 실제 적용되고 100 CCU 및 월0.3 TB 안이면 소규모 테스트·출시를 무료로 운영할 수 있는 공식 옵션이 있습니다. 코드나 SDK 버전만으로 계정 플랜을 판정하지 않고 위 Root UI 관측과 구분합니다.

공식 범위와 지정 앱의 현재 플랜 조사는 완료했습니다. 실제 트래픽/월 peak·청구 상태·계정 전체 무료100 슬롯 자격과 제품 설정 App ID 직접 대조는 미확인입니다. 무료100 적용은 별도 변경 작업으로 남기며 이번 조사 완료를 활성화·계약 동의·결제로 확대하지 않습니다. 기존 코드34/release12 제공 사실과 구분하고, 이 문서 때문에 재배포·유료 전환·새 App ID를 만들지 않습니다.

프로젝트 근거는 main `92a4882`의 공개 소스입니다. 동봉 `Assets/Photon/Fusion/release_history.txt:1~20`은 Fusion 2.0.13 Stable/build2379와 Realtime SDK4.1.8.21을 명시합니다. 실행 DLL 버전을 새로 조회한 결과는 아닙니다. `Assets/Scripts/Managers/NetworkRunnerManager.cs:257,351~388,422~427`은 ClientServer lobby·Host/Client·2/4/6/8 총원 전달, `Assets/Scripts/Game/Match/NetworkMatchState.cs:8`은 내부16명 상한, `Assets/Scripts/Room/ChatManager.cs:74~84`와 `Assets/Scripts/Room/CanvasRoom.cs:138~146`은 Fusion RPC 채팅의 근거입니다. 제품 Scripts에는 명시적 리전 override를 찾지 못했지만 설정/실제 연결을 확인하지 않아 현재 리전·자동 BestRegion 여부는 미확정입니다. 별도 서비스 사용 미발견을 계정 전체의 부재 증명으로 확대하지 않습니다.

외부 공개 조사 근거는 `D:/codex/validation/sudden-force-fps/20261030-work/photon-project-facts01/`입니다. 비밀 App 설정·자격정보·결제정보·키 내용은 읽거나 해시하지 않았습니다. 실기기·실제16 KB·장시간 성능·입력 PR261 전체 수용을 이 조사로 완료 처리하지 않습니다.
