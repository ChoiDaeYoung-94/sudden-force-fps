# 일반 빌드 Raw9 보행·달리기 통합 후보

기존 main의 상대 캐릭터에는 이동 속도에 맞는 걷기·달리기 다리 애니메이션이 없습니다. 이 변경은 검수한 Raw9 다리 회전 레이어를 일반 빌드에 연결하고, 상체 소총 지지·기존 이동 충돌·생명 처리를 유지합니다.

## 변경 범위

- 일반 main352183b와 PR418 후보 f528f04 위에 검수한 C#9개·새 C#meta7개·Raw9리소스22개·원바이트용 attributes·프로토콜 suffix를 선정했습니다.
- GamePlayerNetworkData의 네트워크 선언은39→43입니다. 새4개는 WorldGaitPhase, WorldGaitLocalVelocity, WorldGaitEligible, WorldPoseClimbing입니다.
- 제품 namespace는 `sff-q20261022-ladder-v1-intro1-riflecore2-gaitprod1-window1-shoulderref1`입니다. 기존39필드 및 `-localqa` 검수 앱과 섞지 않습니다.
- 9개 클립과 meta/GUID, 계약 SHA256 `456A133706BAC9536BAC29869F09E0ADDA3649F39BE5DFC07F65CA6229FEB3F6`를 보존합니다. Contact03/Raw18 접지 보정은 활성화하지 않습니다.
- QA 로그인 우회·진단 helper·과거 씬/PlayerSettings는 포함하지 않습니다. PR411의 재장전 구도, PR414의 Quad D 시작 설정과 PR418의 후반 높이 수정도 보존합니다.

## 소스 검증과 남은 검증

선정40경로를 실제 적용한 첫 결과에서 Raw9리소스22개의 줄바꿈이 CRLF로 바뀌어 계약 해시 불일치를 독립 검토에서 발견했습니다. 첫 적용본을 외부 근거에 보존한 뒤 해당22개만 원 Git blob 바이트로 복원했습니다. attributes를 먼저 stage하고 실제 index의 계약·clip/meta 해시를 다시 확인합니다. 메모리 patch 성공을 실제 작업 폴더 원바이트 성공으로 대체하지 않습니다.

일반 noQA의 새 실제 컴파일·43선언/namespace·실제 graph 평가, 짧은 이동→질주→정지/방향 전환, crouch/climb·피격→사망→재출현 회귀가 남아 있습니다. 실제 검증 후 결과를 여기에 추가합니다. 기존 QA13/폰·Editor 관측과 과거 noQA Windows 실행을 이번 소스의 새 실행으로 합산하지 않습니다.

CI/CD 확장·운영 code35 예약·Play 업로드는 수행하지 않았습니다. 소스 검증만으로 전체 접지·경사·다른 기기/화면비·성능·Play 설치를 통과 처리하지 않습니다.
