# 어깨와 의상 간섭을 줄이는 소총 자세 후보

#381, PR379의 상하 조준 후보 기반입니다. 개머리판이 어깨 뒤 의상과 가방에 깊게 들어가 보이는 자세를 줄이기 위해 소총 기본 위치와 yaw를 조정합니다. 양손 grip calibration·오프셋·IK 수식과 .95 도달 한도는 유지합니다.

| 설정 | 이전 | 후보 |
| --- | --- | --- |
| Forward | .15m | .19m |
| Down | .08m | .06m |
| Inward | .025m | -.01m |
| RifleEuler Y | 0° | -8° |

팔과 무기의 자세가 실제 12개 hitbox 위치에도 영향을 주므로 Photon 접속 suffix를 `-intro1-riflepose4pitch1-gaitraw04`로 분리합니다. 발사 원점·방향, 피해량, 캡슐·시선과 골격/메시 자산은 변경하지 않습니다.

## 확인한 근거

- 9개 후보의 Blue 기립·yaw0·pitch0 비교에서 기본 엄격 교차 2,905쌍을 재현했습니다. 이때 Forward .20m 후보는 2,361쌍(의상658→112, 팔2247→2249)이었으나 넓은 각도 시험의 +30°에서 왼팔 도달 .951711로 24/168조건이 탈락했습니다. 해당 값을 채택하지 않았습니다.
- Forward만 .19m로 당긴 후보는 양 팀×yaw0/90×기립/앉기×pitch7개×외부 수학 경로 표식3개의 168조건을 4.58초에 모두 통과했습니다. 최대 왼팔 도달 .9372792, 손목 오차 4.17e-7m 이하, 회전 오차0°입니다. 세 경로 표식은 실제 Fusion callback 실행/순서를 증명하지 않습니다.
- RenderComparison04는 별도 원창/실제 Editor의 같은 제품 MVID에서 baseline과 .19 후보의 정면·측면 PNG4장을 0.759초에 생성하고 결과 저장 후 정상 반환했습니다. 두 후보의 카메라 행렬·프레이밍을 고정했습니다. 총괄은 이미지에서 어깨 뒤 개머리판의 깊은 간섭 감소와 양손 구도를 확인해 다음 실제 게임 검증 후보로 선정했습니다.
- 임시 clone·Material·RT 정리, 기존 자산 메모리·원래 scene 보존을 확인했습니다. 소유 Editor 정상 Exit0·ADB 설정 원복/서버 세대 보존과 공개 입력4,095개 실행 전후 바이트 일치를 별도로 확인했습니다. 이 감사는 이 문서를 추가하기 전 d724900 소스의 기록입니다.

근거 위치는 `D:/codex/validation/sudden-force-fps/20261112-night/`입니다. `rifle-range02/actual-analysis.public.json`, `rifle-render-comparison04/actual-8d8e2d26be5a45f8ae82702d83998624/`의 결과와4PNG, 그리고 같은 nonce의 `rifle-render-comparison03/root-windows/` 정상 종료/원복 기록을 구분합니다. RenderComparison04 결과 SHA256은 `879BC309F3C78A7D84D8E1BE7F9549734DCAC182B94499920733F0AAE05F8131`입니다.

## 남은 수용

.20m의 엄격 교차2,361쌍을 .19m의 결과로 복사하지 않습니다. .19m의 엄격 표면 교차는 재계산하지 않았고 몸통 OBB 내부 정점도 남아 있습니다. 손가락 접촉·전체 피부/의상 분리, 실제 current/proxy와 HitboxCapture/Render/LateRender, 이동·상하 조준·사망/리스폰 및 Android 포함 게임은 후속입니다. 이 변경은 아직 Draft 검증 후보이며 운영 code34/release12·원장196,2,34에는 미포함입니다.

이전 RenderComparison03은 raw Vector3의 JSON 순환 참조로 baseline PNG2장만 생성하고 결과 저장에 실패했습니다. 원본·부분 PNG·실패 반환은 보존하고, 숫자 DTO로 고친04의 새 실행과 구분합니다. 9후보 비교의 HTTP timeout과 저장 후 callback 반환 미확보도 기존 기록에 유지합니다.
