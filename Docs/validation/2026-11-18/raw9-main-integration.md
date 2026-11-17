# 일반 빌드 Raw9 보행·달리기 통합

기존 main의 상대 캐릭터에는 이동 속도에 맞는 걷기·달리기 다리 애니메이션이 없습니다. 이 변경은 검수한 Raw9 다리 회전 레이어를 일반 빌드에 연결하고, 상체 소총 지지·기존 이동 충돌·생명 처리를 유지합니다.

## 변경 범위

- 검수한 C#9개·새 C#meta7개·Raw9리소스22개·원바이트용 attributes·프로토콜 suffix를 선정하고 PR418 병합 main47b98ba를 반영했습니다.
- GamePlayerNetworkData의 네트워크 선언은39→43입니다. 새4개는 WorldGaitPhase, WorldGaitLocalVelocity, WorldGaitEligible, WorldPoseClimbing입니다.
- 제품 namespace는 `sff-q20261022-ladder-v1-intro1-riflecore2-gaitprod1-window1-shoulderref1`입니다. 기존39필드 및 `-localqa` 검수 앱과 섞지 않습니다.
- 9개 클립과 meta/GUID, 계약 SHA256 `456A133706BAC9536BAC29869F09E0ADDA3649F39BE5DFC07F65CA6229FEB3F6`를 보존합니다. Contact03/Raw18 접지 보정은 활성화하지 않습니다.
- QA 로그인 우회·진단 helper·과거 씬/PlayerSettings는 포함하지 않습니다. PR411의 재장전 구도, PR414의 Quad D 시작 설정과 PR418의 후반 높이 수정도 보존합니다.

## 소스 검증과 남은 검증

선정40경로를 실제 적용한 첫 결과에서 Raw9리소스22개의 줄바꿈이 CRLF로 바뀌어 계약 해시 불일치를 독립 검토에서 발견했습니다. 첫 적용본을 외부 근거에 보존한 뒤 해당22개만 원 Git blob 바이트로 복원했습니다. attributes를 먼저 stage하고 실제 index와 warm checkout의 계약·clip/meta 바이트를 다시 확인했습니다.

## 실제 일반 noQA 검증

기능 소스68c8c6b의 공개4134개 입력을 고정한 별도 warm Editor에서, QA define 없는 Standalone assembly의43선언·MVID75061a72-f7ce-4140-8f6b-a01d2aa2200d·제품 namespace를 확인했습니다. 기존 일반 Windows43 Player는 파일363개를 다시 대조하고 새 프로세스로 참가했습니다. Editor HOST/Windows CLIENT의 실제 두 피어 START와 두 배우의 valid graph/input9/output1/rawOnly/idle weight1을 확인했습니다. Windows Player 자체는 과거8fcc73ce 소스이며 이번 소스를 새로 빌드한 Player로 표현하지 않습니다.

- 새 runtime02의 native W8·Shift+W40·A60 입력에 대해 각약5초99행, 총297행을 수집했습니다. W 표본의 forward-walk weight 최대.626736, 옆걸음 표본의 walk index3 최대.901700과 하퇴 회전 변화가 관측됐습니다. Windows remote-input 배우를 HOST 현재 상태에서 읽은 표본이며, gap0/fault0·rawOnly·source life alignment와 weight 합을 확인했습니다.
- Shift+W는 Sprint0/run weight0이므로 연속 달리기 성공으로 계산하지 않습니다. 도구의 짧은 키 누름과 연속 hold를 구분하며, 첫 구간의 Editor 부모 창 W5 입력 뒤 변위0도 미확정 포커스 사례로 보존했습니다.
- 전투 위치 fixture는 원형 A609681A…를 그대로 RAM에 컴파일했습니다. 첫 장애물 구간은 위치 적용을 거부했고 배우 상태 변경0·후속 조회에서 잔류 객체0을 확인했습니다. 실제 옆이동 뒤 새 RAM 인스턴스가 평지·시야5m를 확인해 HOST local의 NCC.Teleport/velocity0만 한 번 적용했습니다. HP/dead/life/aim/점수 직접쓰기0, NonSetupFieldsUnchanged·후속 조회의 객체0을 확인했습니다.
- 새 방에서는 Windows native 단발 사격으로 Head100·shot1/hit1·정확 target player·ammo30→29·kill1/blue1과 피해자 HP0/death1을 확인했습니다. 10초197행의 죽음59행과 부활 뒤68행에서64Hz/192tick 타이머·TargetTick6775를 지나 life0→1/HP100/alive·pose ready/aligned/fault0으로 복귀했습니다. 첫 dead와 첫 alive 표본의 벽시계 차이는 약2.9795초이며 정확3.000초 측정으로 표현하지 않습니다.
- 걷기 표본의 배우1032는 생존했고 피해자는 정지한 HOST local1031입니다. 따라서 같은 이동 배우의 walk/run→death→respawn graph/phase 초기화나 pure proxy 생명 검수를 완료한 것은 아닙니다. 첫 방의 단발 miss·장애물 거부·기한/바인딩 거부도 성공 표본으로 합산하지 않습니다.

일반 Player와 Editor는 각각 정상 Exit0/forcedfalse로 종료했습니다. 종료 지연의 원인은 미확정입니다. sampler callback·AppDomain key를 회수하고 EDM 형식 차이 한 파일의 원본/발생본을 보존해 CAS 복구한 뒤 공개4134 raw exact/Git clean/private6 absent-indexS·SDK0·원설정1,1·공유 ADB 세대 보존으로03:30:50 KST 전체 반환했습니다. 이전 EDM 발생 해시를 기대한 첫 복구 시도는 쓰기 전에 거부됐으며, 이번 LF/빈값 뒤 공백2개의 실제 차이를 새로 분류했습니다.

외부 근거: `20261117-night/raw9-noqa-runtime02/actual-2147e984cbfe47cbafdbb3253e1d7c89/`, `noqa-next-runtime-independent01/native-input-actual-review01/`, `noqa-next-runtime-independent01/combat-life-actual-review01/`. 독립 판독본과 실제 실행 본문·source/exit/full-return 영수증을 함께 보존했습니다. 이 문서 수정은 실행 뒤 문서만 갱신한 것이며 기능 소스40경로는68c8c6b와 같습니다.

## 남은 검수

연속 달리기/정지·전방위 전환·crouch/climb, 동일 이동 배우의 생명 경계·pure proxy, 화면상 발 접촉/총 파지/가림과 일반 Android/다른 기기·화면비는 후속입니다. 기존 QA13 Android full-stick 표본은 별도 근거로 보존하고 이번 일반 noQA 실행의 실제 run weight로 대체하지 않습니다.

CI/CD 확장·운영 code35 예약·Play 업로드는 수행하지 않았습니다. 소스 검증만으로 전체 접지·경사·다른 기기/화면비·성능·Play 설치를 통과 처리하지 않습니다.
