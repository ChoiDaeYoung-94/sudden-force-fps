# 첫 발소리 오디오 데이터 준비 (#286)

플레이어 초기화에서 아직 Unloaded인 발소리 clip에만 `LoadAudioData`를 요청합니다. 기존 총성·재장전 효과와 같은 null/Unloaded 조건이며, 기존 발소리 재생·SFX 경로·볼륨·보폭·거리 감쇠는 유지합니다. 음원·import/meta·믹서·DSP 설정은 변경하지 않았습니다.

실제 다섯 clip은 background loading이 꺼져 있어 데이터 준비 비용과 메모리 사용 시점이 초기화로 이동할 수 있습니다. 이 변경은 첫 발소리의 준비 경계를 다루며, 입력부터 스피커까지의 지연을 측정하거나 해결했다고 주장하지 않습니다.

## 실제 확인

별도 Windows QA 체크아웃 P11의 새 소유 Editor에서 확인했습니다. 기존 QA 체크아웃에 현재 제품 C#을 반영한 표본으로, 최신 main의 clean checkout 전체 빌드나 새 Android 검증은 아닙니다.

- 프로세스22304, 시작 `2026-10-28T02:01:59.0899141Z`, 실제 Game 어셈블리 MVID `0a76856e-bb06-482e-8ea1-99a3b77de452`입니다. batchmode/Graphics Null/StandaloneWindows64, DOTNET·BEE·job worker1과 non-Play·compilationFailed=false·빈 clean scene을 확인했습니다.
- helper 미호출 CompileOnly와 Disabled 실행은 각각 진단0이며, Disabled에서는 query/clone/file0입니다. 실제 fixture는 새 PID/start/MVID·공개 source9·helper SHA·4분 TTL·새 nonce를 묶은 뒤 한 번 실행했습니다.
- 실제 Blue prefab에서 clip5개 모두 `Unloaded → Loaded`로 변했습니다. 같은 Awake를 다시 호출했을 때 모두 Loaded를 유지했습니다.
- 같은 clip을 공유하는 Red prefab은 처음부터 모두 Loaded였고, 첫 호출·반복 호출에서도 유지했습니다. 고유5개 cold 전이와 두 팀의 명시 Awake 총4회를 구분합니다. 상태 관찰을 정확 API 호출 수 계측으로 표현하지 않습니다.
- 각 clone의 기존 자식188개·AudioSource1개는 identity와 개수가 같았고, 새 locomotion source는 null·ObservedStepCount는0을 유지했습니다. 양쪽 PrefabContents load/unload는2/2, 오류 배열은 비어 있었고 원래 scene 상태를 보존했습니다.
- 공개 입력3,984개는 import 후·fixture 후·정상 종료 후 모두 새 준비 기준과 바이트가 같았습니다. 어제 기준과의 의도한 차이는 이번 locomotion C#1개이며, QA의 private6개는 부재/indexS를 확인했습니다. 전체 메모리나 캐시가 같다는 뜻은 아닙니다. 오디오 데이터 로드는 의도한 캐시 변화이며 강제 unload로 되돌리지 않았습니다.
- `2026-10-28T02:04:10.334255Z`의 실제 보유 process handle로 정상 ExitCode0을 확인했습니다. 강제 종료·Save·종료 후 창 조회는 하지 않았습니다.

## 범위와 후속

fixture의 Awake는 실제 컴포넌트에 대한 명시적 호출입니다. Unity/Fusion의 자동 생성·Spawn 순서, 자연 보행·실제 청취, Android 포함·기기·성능을 검증한 결과는 아닙니다. 직접 fixture 재생·Render·강제 `UnloadAudioData`·global Save는0입니다. 기존 PlayStep 본문은 변경하지 않았으며 #162/#166 전체를 완료 처리하지 않습니다. 코드34는 이 변경 이전 QA 산출물입니다.

외부 원문은 `D:/codex/validation/sudden-force-fps/20261028-work/`에 보존합니다.

| 근거 | SHA256 |
| --- | --- |
| 실제 결과 `shot-feedback-audit01/step-warmup-fixture-candidate01/actual-918cfc25b3e94749ab9c7c1fdc306f94/result.public.json` | `EA5EFD99BBA5C3FCD5A724DFC13CE1367DE04C964ACA9F7396961CDE6DC53C3D` |
| 소스 독립 검토 `step-warmup-source-independent-review01/verification.public.json` | `83196BF9AA586FE1A5582276C685F1D4601C374E051E62FF288A52856A18F120` |
| helper 독립 검토 `step-warmup-fixture-independent-review01/verification.public.json` | `68AE660FF9A64D38F0DB7126DFF8B95C14C9FE24C20B271424FD78B7EC9A5689` |
| 정상 종료 `p11-step-normal-close01.public.json` | `5CB4D532C11BED0D29D3434FE74D8D929D89F2773FDE1AF20B9EF8C35690647C` |
| 종료 후 입력 `p11-step-after-close01.public.json` | `FD20245CC303E2DF5D1E7213CFD9B5C44233A22C6C8D496A4C770F9265425E08` |
