# 전신 보행 후보의 실제 검증과 차단 사항

관련 #341, #166, #41. PR344는 **Draft**이며 게임 프리팹·제품 클립·새 Android에 연결하지 않았습니다. 현재 제공 APK34의 보행은 이 후보로 바뀌지 않습니다.

## 확인된 결과

첫 Unity 6000.3.25f1 Windows batch 검증에서 C# 후보를 컴파일하고 출처가 확보된 CC0 원본 2개를 격리 import했습니다. Generic leg curve 9개와 양팀 cardinal CPU skin 표본 66개를 외부에 생성했습니다. 원본 뼈·상체·Animator identity와 기존 hitbox 12개를 유지했으나 접지 판정은 24개 통과·42개 실패, 최저 skin vertex Y는 −0.0381196477m였습니다. native/tool 종료 1이며 실패 원본을 보존합니다. 좌우·후방 걷기는 Run 파생이므로 모든 방향의 독립 Walk take 확보로 표현하지 않습니다.

단순 시각 골격 분리만으로는 부위별 판정과 움직이는 팔다리가 어긋났습니다. 별도 coverage 수집은 양팀 각 baseline 1개·기존 실패 leg key 36개·balanced rifle 1개로 총 76개 관측입니다. 기존 66개 graph 표본의 재실행이 아닙니다. baseline에서 판정 내부였던 고신뢰 정점이 새 자세에서 밖으로 나온 witness의 최근접 판정 거리는 Leg 최대 약 0.579m, balanced rifle Arm 약 0.088m였습니다. 캡처한 shape 행렬로 독립 거리 재계산을 했으나 실제 대미지·새 제품의 mapped hitbox 수용 시험은 아닙니다. 원래 baseline의 판정 밖 정점도 있어 전체 정점 100% 포함을 합격선으로 만들지 않았습니다.

후속 Grounded03은 초기 본 복사 비교에서 실패했습니다. 별도 양팀 진단은 원본 source 불변과 quaternion 복사 후 정규화의 네 미세 차이를 독립 setter/getter로 재현했습니다. Grounded05는 초기 source exact와 독립 복사, 후속 upper baseline 비교를 분리했고 Blue-bake 초기 setup/LegIdle 처리 범위에서 이를 확인했습니다. 해당 fixture의 보호 컴포넌트 26개 직렬화 JSON도 전후 일치했습니다. 예정한 전체 fixture·양팀·full gait 보호 검사를 완료한 것은 아닙니다.

Grounded05는 LegIdle 121 records 한 파일을 생성한 뒤 첫 WalkForward 처리 중 `IK_TARGET_RESIDUAL_TOO_LARGE`로 실패했습니다. 실제 잔차를 0.2mm 이하로 요구하는 검사이며, 현재 실패 출력은 정확한 side/key/iteration/잔차 숫자를 담지 않습니다. 최종 cardinal·dense·blend 검증과 화면 생성은 완료되지 않았습니다. managed elapsed 약 0.467초, native/tool 종료 1·강제 종료 없음·Dispose 오류 0입니다. 허용 오차를 완화하거나 이 실패를 보행 성공으로 처리하지 않았습니다.

이후 수식·회전 대입·0.2mm 기준을 그대로 두고 읽기 전용 좌표 snapshot과 실패 기록만 추가한 Grounded06을 별도 실행했습니다. WalkForward 키 인덱스 3(0부터 시작)·오른발·반복 0에서 실제 보정 요청 0.7374799mm, 무릎 목표 위치 오차 0.447689963mm, 최종 발 잔차 0.420842mm를 확보했습니다. 잔차는 유한하고 foot orientation 복원 중 pivot 이동은 0입니다. 원래 IK 검사 실패와 native/tool 종료 1·강제 종료 없음·Dispose 오류 0을 유지했습니다. 이 자료는 정확한 실패 위치를 확인한 것이며 작은 각도의 native 회전 처리나 특정 solver 원인을 확정한 것은 아닙니다. world getter snapshot을 raw `FromToRotation` 반환값으로 표현하지 않습니다.

Grounded06 종료 후 임시 helper/metadata 3개를 외부 보존·정리하고 해당 실행의 공개 입력 4,061개를 검증 전 바이트로 복원했습니다. 이는 문서가 추가된 별도 기준이며 앞선 4,060개 감사와 구분합니다. 코드·기준 수정이나 제품 보행 수용은 하지 않았습니다.

## 다음 수용 조건

확보한 joint/target·전후 회전·잔차를 바탕으로 작은 각도의 회전 결과와 부모 행렬/world setter 효과를 분리하고, 원인에 맞는 최소 수정 뒤 원래 key·dense·blend 접지 검사를 다시 수행합니다. 시각 pose와 부위별 hitbox를 같은 simulation/render 시점에 맞추고 실제 등록·lag capture·사격/피해를 검증해야 합니다. CPU BakeMesh 성공은 SkinnedMeshRenderer의 실제 경계·재진입·GPU 표시를 대신하지 않습니다. 사망·리스폰·앉기·방향/속도 전환과 두 피어 경기, Android 포함·기기 확인은 남아 있습니다.

외부 근거는 `D:/codex/validation/sudden-force-fps/20261106-work/resume-0330/`의 `locomotion-author01/`, `coverage-actual-independent-review01/`, `locomotion-isolation-diagnostic-candidate04/`, `locomotion-grounded-bake-candidate05/`, `locomotion-grounded-native05/`, `window06-root-completion.public.json`입니다. 마지막 cohort의 공개 추적 입력 4,060개를 검증 전 바이트로 복원했고 임시 helper/metadata 7개는 외부에 보존한 뒤 정리했습니다. 기본 P2의 기존 설정 변경 5개와 앞선 실패 자료를 보존했습니다. 새 Android 예약·빌드·서명·업로드는 0건입니다.

추가 witness 근거는 같은 외부 폴더의 `locomotion-grounded-bake-candidate06/managed-actual06/`, `locomotion-grounded-native06/`, `window08-root-completion.public.json`입니다. 앞 문단의 4,060개는 window06 당시 기준입니다.
