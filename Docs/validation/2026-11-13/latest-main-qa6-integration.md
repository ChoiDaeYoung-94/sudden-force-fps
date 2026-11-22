# 최신 입력·소총·이동 애니메이션 QA6 통합 후보

Refs #341, #342, #384. 이 변경은 운영 main과 분리한 QA 통합 후보입니다.

입력261이 반영된 main435e 및 core2b84를 바탕으로 Raw353·AimPitch379·HP385 소스9개, attrs1개, Raw9 리소스22개를 결합했습니다. core2의 소총 기본값 .19/.06/-.01/yaw-8과 최신 닉네임·세션 복구 및 입력2파일은 보존합니다. 과거 후보와 검증 문서는 그대로 둡니다.

Networked 선언은39→43이며 새 접속 버전은 `sff-q20261022-ladder-v1-intro1-riflecore2-gaitraw1`입니다. LOCAL_QA일 때만 `-localqa`를 추가하고 Android Login의 개발 우회를 허용합니다. 원 Photon 설정 asset은 바꾸지 않습니다. 기존 core2·QA5와 혼합 접속하지 않습니다.

Raw9 QA 전용 gate 및 contract는 유지합니다. 제품에서 걷기 graph가 활성화됐거나 접지 보정·발 피부 접촉 품질이 수용됐다는 뜻이 아닙니다. 새 통합 조합의 실제 compile/weave, Android 포함·패키징·서명, 동일 버전 두 피어 경기와 이동·사격·피격·사망/리스폰·정상 만료/결과/RETURN 확인이 남아 있습니다. Windows core2의 빌드 성공과 권한 창으로 미실행한 경기를 이 후보의 성공으로 합산하지 않습니다.

테스트 package/version 제안은 `.localqa`/`1.0.197-qa6`/code6입니다. 실제 산출물 생성 전에는 제안일 뿐입니다. 운영 코드34·원장196,2,34·Play release12를 유지하며 코드35 예약·외부 APK 업로드·CI/CD 신규 구축은 하지 않습니다.

소스 후보 작성 시점의 실제 SDK/기기/Android 빌드 성공은0입니다. 독립 source 검토와 새 실제 근거가 갖춰질 때까지 Draft로 유지하고 #341 전체를 닫지 않습니다.
