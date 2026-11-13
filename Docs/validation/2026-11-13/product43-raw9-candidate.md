# 일반 빌드 Raw9 이동 후보

Refs #399

## 변경과 경계

이전 Raw9 이동 공장은 `SUDDEN_FORCE_LOCAL_QA`가 없는 일반 빌드에서 항상 실패해 걷기·달리기를 표시하지 못했습니다. 이 후보는 main `725e794`에서 QA7 `003fe93`의 공통 소스·Raw9 리소스 40경로를 선택해 이식하고, 일반 빌드 전용 계약과 `gaitprod1` 접속 세대로 분리합니다. 기존 main 로그인과 Input261을 유지하며 QA 로그인 우회를 포함하지 않습니다. QA7과 접속 세대가 달라 기존 QA7 폰을 제품 후보의 상대 피어로 사용할 수 없습니다.

Raw9는 Idle·전후좌우 걷기·전후좌우 달리기 9클립, 다리 경로 12개, 기존 Animator와 Manual 그래프 하나를 사용합니다. 클립·메타 바이트 및 GUID를 보존했습니다. 소총은 어깨 기준 전방 거리 .24와 대응 왼손 그립을 함께 사용합니다. 상체 자세가 바뀌므로 12개 피격체의 shape 데이터 불변만으로 피격 정합성을 승인하지 않습니다.

`Raw9Contract.json` SHA256은 `456A133706BAC9536BAC29869F09E0ADDA3649F39BE5DFC07F65CA6229FEB3F6`입니다. `productCandidateActivation=true`, `actualRuntimeAccepted=false`를 명시합니다. Contact03·Raw18 보정은 활성화하지 않으며 해당 승인의 빈 해시를 채우지 않습니다. 계약의 미수용 표시는 실행 가드를 유지하는 후보 식별이며 실제 검증을 통과했다는 뜻이 아닙니다.

## 수행한 확인

- 선택 40경로의 index/필터 적용 working blob을 QA7 원본과 대조했고, 나머지 main 4,094경로의 mode/OID를 보존했습니다.
- 독립 검토에서 Raw9 클립·메타 18파일과 GUID, 9클립 행·12개 다리 경로를 대조했습니다.
- 공장·계약의 변경은 별도 독립 소스 검토를 받았습니다. 소스 `8fcc73ce`의 실제 noQA Editor에서 새 MVID `4dea59ea-4d6f-4061-a731-1218044cc3c6`, Networked 선언 43개, `gaitprod1` 접속 세대와 9클립 로드를 확인했습니다. 선언 수 확인은 전체 weave 배치 검사와 구분합니다.
- 새 Windows 개발 Player와 정상 입장·READY·START로 두 피어 경기에 진입했습니다. Host가 관측한 두 actor에서 Manual 그래프·9클립·12개 다리 경로·10 playables/출력1/Animator1·fault0을 확인했습니다. 짧은 native W/Shift+W 뒤 위상은 0→.0055159945→.0110461488이며, 사후 표본 속도는0이므로 연속 걷기·달리기 시각 통과로 합산하지 않습니다. LMB·R 입력 후 shot0→1/탄약30→29→30, hit0/HP100/life0입니다.
- Windows bundleVersion은 `1.0.194`이며 빌드는60.3325초·오류0/경고2·363파일입니다. helper 전체71.718초와 구분합니다. 정상 메뉴 복귀·EXIT/OK 후 Player와 Editor의 Exit0, 원설정·공유ADB·공개4,129파일 raw 복원을 확인했고16:29에 기한 내 전체 반환했습니다. Hidden 창 실행은 Root가 강제 종료한 실패 이력으로 별도 보존했습니다.
- `git diff --check`를 확인했습니다. 외부 검토 자료는 `D:/codex/validation/sudden-force-fps/20261113-resume/product43-integration01/`과 `product43-gait-candidate01/`에 보존합니다.

빌드 후6개 형식 변경과 URP2개 의미 변경을 발견해 원본·생성본 보존 후 CAS 복원했습니다. Balanced의 SSAO prefilter4개 값과 Global runtime 목록의 리소스 참조1개 변화는 설치 URP/Core17.3의 빌드 계산·저장/stripping 규칙과 부합합니다. 실제 해당 writer 시점·packing 값은 미관측이므로 `pristineBuildProvenanceAccepted=false`를 유지합니다. 복원 성공은 기존 Windows 산출물의 원freeze 입력 동일성을 소급 증명하지 않습니다.

실제 결과 독립 IR SHA256은 `B05588169DEC9D05CDFDD0DA10336AC9C82E9D622DCD6FA977CC77E36F24B060`이며, URP 파생 규칙 조사는 `DB4B459149960A1F53FD34A6ED9BBB106AF94CC0BF62823CC30F31328FFA72AB`입니다. 외부 자료는 같은 날짜 검증 루트의 `product43-runtime-control01/`과 `product43-urp-derived-state-investigation01/`에 보존합니다. 이 문서 갱신은 위 실행 이후이며 제품 코드를 변경하지 않습니다.

## 병합 전 남은 확인

연속 걷기·달리기·회전·웅크림·사격 피격·사망·리스폰 및 프록시 생명 전환, 손·소총·피격체 정합성은 각각 확인합니다. Host authority 표본과 순수 proxy 관측, 시험용 직접 상태 입력과 실제 사용자 입력을 구분합니다.

새 제품 Android 산출물·설치·실제 PGS/Play 인증 검증도 남아 있습니다. QA7 APK의 테스트 결과를 이 후보에 복사하지 않으며 Store code34/release12와 빌드 원장 `196,2,34`는 변경하지 않습니다. 1인칭 미리보기와 조준점 정렬 조사는 이 변경에 포함하지 않습니다.
