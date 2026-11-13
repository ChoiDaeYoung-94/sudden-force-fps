# 시각 참고에 따른 어깨 견착 QA7 소스 후보

2026-11-13, #381/#342 후속입니다. QA7 창문 후보 `4f468db19ff4e085eee752e68a319c998b550022` 위에 정지 렌더와 수학 검사로 선별한 pair05를 결합합니다. 현재 단계는 QA 소스 준비이며 새 컴파일·Android export/APK·기기 경기 또는 제품 채택 완료가 아닙니다.

## 변경 범위

- `WorldRifleBodyPoseSettings.Forward`: `.19` → `.24`.
- `LeftGripOffset`: `(-.04, .03, 0)` → `(-.04695866, .03, -.04951340)`.
- QA 세션 suffix에 `-shoulderref1`을 추가합니다. 기대 namespace는 `sff-q20261022-ladder-v1-intro1-riflecore2-gaitraw1-window1-shoulderref1-localqa`입니다. 실제 두 peer의 cached 값 확인은 후속입니다.

기존 yaw -8°의 pitch0에서 총을 actor forward로 5cm 옮기면서 왼손 목표 세계 위치를 유지하도록 grip offset을 보상한 후보입니다. Down/Inward/Euler·오른손 calibration·무기/카메라/피격 판정·43 Networked 선언은 변경하지 않습니다. 창문 prefab 두 개는 부모 QA7 소스에 들어 있는 main의 검증된 blob을 유지합니다.

## 실제 관측과 제한

QA6 source `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`, GUI Editor native31656/MVID `643d29f7-a2c9-4f03-8844-e32f450e93fb`의 격리된 preview clone에서 source-derived 수학을 적용했습니다. live `ApplyAfterStance`나 권한 guard를 우회하지 않았습니다.

현재 baseline과 pair05를 동일한 front/side/rear-oblique 카메라로 비교했습니다. pair05는 개머리판 뒤쪽 돌출을 줄였고 오른팔 reach `.3924682` → `.46848616`, 왼손목 세계 위치 차이 약 `1.35e-7m`를 관측했습니다. stock 뒤쪽 band 중심과 shirt의 최근접 unsigned 거리는 약44.0mm → 2.27mm였습니다. 이것은 피부 접촉·관통 없음의 판정이 아닙니다. 손바닥 bone proxy 거리도 실제 손 피부와 총의 간격이 아닙니다.

별도 168행 검사는 양팀×yaw2×기립/앉기×pitch7×수학 라벨3입니다. 서로 다른 기하 조건은56개이며 라벨3은 실제 Fusion callback 순서의 증거가 아닙니다.

- math168/168, reach 거부0, source invariant 실패0, broad 실패0.
- 최대 reach: 오른팔 `.5355455`, 왼팔 `.932070732`이며 왼팔 최대는 +30° 앉기입니다. `.95` 상한까지 여유는 약 `.0179293`입니다.
- 168행 모두 muzzle local과 원본 보존 검사를 통과했습니다.
- torso OBB 진단 양성168행을 보존합니다. 엄격한 피부 교차 검사·GPU 렌더168개·실제 hitbox history·current/proxy 생명 경계 수용은 아닙니다.

12:36:39.928891 KST에 원 native Exit0/강제 종료0, Prefs2 복구, 기존 공유 ADB 세대와 공개 입력4,122개 raw exact/private6 absent-indexS를 확인한 뒤 GUI를 반환했습니다. 공식 `quit` 명령은 EditMode의 DontDestroyOnLoad 오류로 실패했고, 소유 Editor의 직접 Exit API로 정상 종료했습니다. 실패 원본을 보존했으며 실제 native Exit0과 RPC 응답 실패를 구분했습니다. Scene asset 저장은0입니다.

공개 검증 자료는 `D:/codex/validation/sudden-force-fps/20261113-resume/` 아래에 있습니다.

| 근거 | SHA256 |
| --- | --- |
| preview03 실제 baseline result | `BA4DD8FF302CF090A41ABEE7212B1C74DC9C0C53DF466565BE632D3B31ED56BD` |
| preview04 실제 pair05 result | `F6FB36AA77F4E16D44F5512BDD798DF4E80EBAA2FAA1F87498598820168C9D87` |
| pair05-range01 실제168행 result | `B7AEC1FCE3D4773DC27CF2C03C7D16AD8228AA81FD7CF14B01C85E858759ED49` |
| pose-gui01 종료 후 공개4,122개 감사 | `49AEBCA9E777748A68978018391A8A960A59FAEF78078A291FA42C38CE2C4CC6` |

## 다음 수용 단계

이 새 소스의 fresh raw freeze·독립 검토·실제 compile과 새 산출물 결속이 먼저입니다. QA7 code7/version `1.0.197-qa7`은 미예약·미빌드 후보입니다. 운영 원장196,2,34 및 Play34/release12는 변경하지 않습니다.

이후 같은 QA namespace의 peer에서 실제 앉기·상하 조준·이동·발사·재장전·순수 proxy·사망/리스폰을 확인합니다. 연속 시각 품질과 의상/손 접촉이 남아 있으므로 PR은 Draft로 유지합니다.

1인칭 translation 후보는 팔 말단 노출로 미수용했습니다. 위치를 유지한 -6° 회전은 정지 QA 비교 후보이나 총열/flash 시각축과 동작 검증이 남아 이번 소스에는 넣지 않습니다. 추가 양팔 framing preview는 별도 조사입니다. CI/CD 구축·확장과 광고/IAP는 범위 밖입니다.
