# 2026-11-07 보행 후처리 진단과 통합 준비

Refs #341. 새 보행은 아직 제품에서 활성화하지 않았습니다. 이 기록은 별도 CPU 진단과 Android 대상 컴파일 관측이며, PR344의 보행 품질 완료나 새 APK 포함을 뜻하지 않습니다. 기존 실패와 미완료 범위는 유지합니다.

## C11의 별도 1mm 진단

고정 source `31789E517B1C94D597C2B84D5AD5742686A5F4629CD4A386012E6311E39A19C3`의 Blue/Red·32개 위상·32개 blend 조합에서 새 `runtimeVerticalContact1mm` 2,048개가 수치 조건을 만족했습니다. 1,222개는 추가 solve 없이, 826개는 한 iteration에서 총 1,068회 leg solve로 조건을 만족했습니다.

| 직접 집계한 값 | 결과 |
| --- | --- |
| 원 raw mixer 대비 최대 최종 foot 변위 | 79.22352998 mm |
| 최소 양발 sole / 전체 CPU skin Y | 0.9995997 / 0.9995997 mm |
| 최대 보고 target residual | 0.0004571039 mm |
| 보고 foot world orientation error 최대 | 0° |

Boots 704개는 총집합이며 calf 영향 453개는 그 부분집합입니다. 61개 bone·1,428개 influence의 전체 가중치를 사용했습니다. 제한된 12개 witness의 CPU LBS 대조는 전체 GPU skin 수용으로 확대하지 않습니다. 위 수치는 고정 rig/clip과 유한 표본의 오프라인 평면 진단이며, 실제 지면 접촉이나 시간 연속성·자연스러운 footplant의 증거가 아닙니다.

원 blend 494/2,048 실패와 두 8mm trial의 종합 판정 실패(`all=false`), `allQuantitative=false`는 그대로입니다. 두 trial의 개별 outcome success 2,044/2,048·1,910/2,048을 전부 실패했다고 표현하지 않습니다. 실제 C11의 managed/native/tool 종료도 1이므로 정상 실행 전체 통과로 세지 않습니다. 기존 cardinal 66·dense 4,614·continuity 9와 새 post-IK 연속성 검증은 별개입니다.

독립 실제 검토: `D:/codex/validation/sudden-force-fps/20261107-work/contact11-actual-independent-review01/verification.public.json`, SHA256 `A4571962453AA9367B4954A15C4B246052BA1E2DD652FCB07C001723480F2B67`. 실제 출력 281개/57,197,889B와 이전 입력 보존을 대조한 범위입니다.

## 별도 Android 대상 컴파일 관측

제품 후보03의 정확한 제안 소스 7개를 P13의 source `98d191c806bec27c9ee532b80bd804e1115bd26a`에 임시 적용했습니다. 원본 4개와 새 파일은 전체 4,067개 공개 입력 백업 후 다뤘으며, 프로젝트 파일로 적용하기 어려운 기존 malformed diff는 사용하지 않았습니다. 이 기록 커밋에는 그 제안 소스 7개를 추가하지 않습니다.

첫 Native는 원 시간창 부족으로 강제 종료되어 결과가 없습니다. 다음 Native는 검사 entry까지 실행했으나 `ACTUAL_DEFINE_MODE`에서 중단했습니다. 동일 보유 프로세스 42516은 2026-11-07T13:13:52.8851223Z에 시작해 13:14:39.2249163Z에 ExitCode1/forcedfalse로 끝났고 Dispose 오류는0입니다. 전체 closed-compile 검사는 실패 상태를 유지합니다.

이후 실행 없는 Cecil 메타데이터 관측에서는 helper의 QA 상수 true, 게임의 QA 접속 버전 분기 문자열 1개, 신규 네트워크 속성 4개의 위빙 정보를 확인했습니다. `WorldGaitPhase`, `WorldGaitLocalVelocity`, `WorldGaitEligible`, `WorldPoseClimbing`의 offset/word는 각각 `(1,1)`, `(2,2)`, `(4,1)`, `(5,1)`입니다. 합계 5word 확인을 전체 클래스 위빙 검사·정상 경기나 실제 snapshot 수용으로 확대하지 않습니다.

근거는 `root-resume01/gait03-native02-offline-metadata.public.json` SHA256 `17EBFA2CFEFA6EE42DDC91836783DF2FFB8E237F647CE01EF5C17B4B4A4F6293`입니다. 대상 게임 assembly를 load하거나 제품 메서드를 호출하지 않았습니다. API defines 배열과 응답 파일의 실제 컴파일 정의를 구분하는 후속 검사 소스는 별도 검토·실행 대상입니다.

복구 첫 시도는 EDM 설정의 자동 서식 차이를 만나 중단했습니다. 원본과 생성본을 보존한 뒤 CRLF와 줄 끝 공백만 다름을 확인하고 같은 exclusive 파일 핸들에서 비교·복원했습니다. 두 번째 복구는 전체 4,067개 raw exact·임시 11개 부재·Git clean을 확인했습니다. Editor0·공용 ADB 설정 DWORD1 복귀·기존 서버 동일 세대 보존 후 13:19:25.2548763Z에 실제 반환했습니다. 이 P13 복구와 P2의 기존 dirty5는 구분합니다.

복구 근거는 `D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-editor-window08-return.public.json`, 같은 폴더의 `gait03-native02-edm-format-restore.public.json`과 `D:/codex/validation/sudden-force-fps/20261107-work/root-gait03-stage-restore-candidate02/actual-4e9ffa8f47444f0f87f174596ef17113/restore-02/restore-result.public.json`입니다. 첫 중단 기록은 별도 restore-01에 보존합니다.

## 남은 작업

- 공유 raw9+corrected9 clip과 JSON3개의 실제 materialization/import/Resources readback 및 독립 검토. 현재는 21asset/45파일 생성 소스 준비이며 실제 생성0입니다.
- Builtin Plane의 실제 identity/topology와 고정 평면 지원 범위 확인. geometry의 owner-origin plane offset은 null/pending이며 0으로 승인하지 않습니다.
- 원 SkinnedMeshRenderer를 사용하는 실제 GPU·rifle 후처리·704개 전체 가중치 검증과 post-IK 시간 연속성.
- 실제 NCC 이동·resimulation·snapshot 보간·새 life·두 peer와 기기에서의 보행 검증.

제품의 승인 SHA3개는 비어 있고 positive flags는 false입니다. 새 로컬 Android QA의 source98에는 input261·main HUD/Intro·rifle345가 포함되며 gait344는 미포함입니다. 제공34/release12와도 구분하며, 현재 자료만으로 PR344를 병합하거나 #341을 닫지 않습니다.
