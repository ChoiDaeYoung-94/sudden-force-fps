# 공개 경로 이동 성공과 발사 전 파일 교환 중단

2026-11-02 #259의 별도 QA worktree P11에서 C12/cohort11을 실행했습니다. 공개 콜백 경로 이동과 이동 후 읽기 전용 head query는 확인했지만, B1은 파일 교환 오류로 실제 발사 전에 중단했습니다. PR261 Draft와 #259 OPEN을 유지합니다.

## 실제 실행 범위

- P11은 detached `f055e4a84391bd9d8741c4e12855887b1ef72166`이며 최신 main 전체 체크아웃이 아닙니다. 입력 관련 두 소스의 PR261 대응과 제공 Android34 QA를 기준으로 하며, Quad D 인트로 등 후속 main 통합 빌드 수용으로 확대하지 않습니다.
- C12 소스 32개와 독립 소스/제어 시험 180항목을 대조한 뒤, 새 Editor PID4776/start `2026-11-02T06:45:26.5057945Z`에 연결했습니다. 미호출 Func 컴파일과 기본 비활성 진입점 등 명시적 진단 8개는 진단0으로 통과했습니다. 소스 시험과 실제 실행 결과는 별도입니다.
- 정상 닉네임/방 생성/팀·READY·START 절차로 PC와 기존 FPS 에뮬레이터 Android34의 Game 진입을 확인했습니다. 첫 방 생성은 닉네임 팝업 때문에 거부됐으며, 닉네임을 설정한 뒤 두 번째 정상 생성 결과와 구분해 보존했습니다.
- 새 읽기 전용 query에서 초기 배치의 첫 충돌이 `Wall_04`임을 관측했습니다. 현재 위치를 입력한 순수 경로 계산 뒤 공개 Joystick Pointer 콜백만으로 18개 지점을 이동했습니다. 결과는 18/18 도달, 실제 이동 22.5316944m/21.5436733초, 해제 후 자연 프레임6/안정 프레임3/마지막 목표 오차 약0.019m입니다.
- Down18/Drag530/Up18/Cancel18 뒤 `released/callbacksRemoved/joystickNeutral/stationary=true`를 관측했습니다. 반면 `normalPollNeutralObserved=false`, `observedMoveTicks=0`, `storedPollRequired=false`이므로 저장된 정상 입력 패킷의 중립이나 native 손가락 이동의 통과는 아닙니다.
- 이동 후 예측/직접 query 모두 현재 상대의 `EXPECTED_HEAD`를 반환했습니다. 읽기 전용 충돌 판정이며 실제 탄환·피해·킬의 검증은 아닙니다.

## B1은 발사 전에 실패

B1 writer와 Queue caller는 각각 한 번만 시작했습니다. Host terminal의 이유는 `SIGNAL_CONTEXT_OR_QUERY_IOException`이며 `requestConsumed=false`, `pcT0Ns/startRequestSha256=null`, `SeedAim/Fire(true)/acceptedShotDelta/ammoDelta=0`입니다. 양쪽 HP100·탄약30·shot0/life0을 보존합니다.

Root가 실제 writer session18163의 도구 종료1과 `FILE_OR_OBSERVATION_ERROR/PermissionError`를 관측했습니다. 이 요약은 원래 stdout을 대체하지 않는 Root 관측 기록입니다. 당시 정확한 Windows 오류 번호와 실패 파일명은 수집하지 않았으므로, 두 예외의 인과나 파일 공유 경쟁을 확정하지 않습니다. 별도 Windows 제어 시험과 새 소스 검토가 후속입니다. 같은 nonce/config/marker로 자동 재시도하지 않았습니다.

중단 때 공개 `Fire(false)`/`Move(Vector2.zero)` 반환과 자기 Component/GameObject 부재를 관측했습니다. `postStopObservationComplete=false`, `allProductStateRestored=false`이므로 전체 제품 상태 복구·추가 발사 없음의 완전한 후속 관측으로 확대하지 않습니다. B1 성공·native L2·사망/리스폰·정밀 타이밍 통과는 모두 아닙니다.

## 정상 종료와 보존 상태

StopHost → clean Untitled → frame cap -1 복원 → 자기 Editor 정상 닫기 순서로 종료했습니다. 닫기 CLI의 종료6/`COMMAND_FAILED`는 그대로 보존합니다. 별도 held process handle은 동일 PID/start의 실제 native 정수 ExitCode0, tool exit0/Dispose0을 확인했으며 강제 종료0입니다. native 종료 시각은 `06:57:39.8225147Z`입니다.

FPS LDPlayer index2의 게임 EXIT/OK와 인스턴스 미실행을 확인한 뒤 `06:59:50Z`에 공유 자원을 반환했습니다. 다른 프로젝트 Editor·LD 인스턴스는 제어하지 않았습니다. 별도 독립 검토된 cleanup02로 자기 source/meta 네 파일만 정리했습니다. 새 closed guard는 P11 Editor0/unknown0/TempLock없음을 확인했고, 정리 후 공개3,984개 raw SHA·기존dirty18·개인6 absent/skip-worktree와 P2 보호설정5개 raw SHA가 보존됐습니다. 기존 guard/marker/실패 영수증은 삭제하거나 덮지 않았습니다. 처음 cleanup01 후보는 stage 시각의 +00:00 형식을 거부하는 검토 결함으로 미실행 보존했고, cleanup02는 해당 event 형식만 명시 수용하며 native의 strict Z/100ns는 유지합니다.

## 근거와 다음 작업

외부 근거 루트는 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/`입니다. 공개 문서에는 원시 로그·개인 키·자격정보·Photon 계정 식별값을 싣지 않습니다.

| 근거 | SHA256 또는 범위 |
| --- | --- |
| C12 `source-manifest.public.json` | `151620BBC3D970F2A9EC0CC01A2FD49A50E4805A9674C10B476924D83120AB52` |
| `input261-family11-full-source-review01/verification.public.json` | `31867CBF0E195E1B754251B87E4B1DF21C31D1D69AA5A4C3027896613F234805` |
| `input261-family11-controlled-tests01/verification.public.json` | `D0D515A22980B3332F319D8329AB70C18A5424822589113273C9AB71D89E9B54` |
| `input261-b1-cohort11/navigation-result.public.json` | `E0749653609DF5E20288009FE4A6ED2A6068C9141FF13A6F37A7931E3FF3C906` |
| 같은 폴더의 `shot-geometry-post-nav.public.json` | `D8CD67268200CADF830C60E759543F51095469EEE10DA99EC0B4F1E631171665` |
| 같은 폴더의 `native-exit.public.json` | `480C84C9A7595506867123103734939CA528B0C0666FF3E72E50B70826CD4F7C` |
| `input261-runtime-area02/input261-b1-phase11/l2-bridge/terminal.public.json` | 발사0·공개 해제 반환·자기 객체 부재·IO 실패 원본 |
| `cohort11-writer-completion01.public.json` | Root의 실제 writer 도구 종료 관측 요약 |
| `input261-b1-cohort11/normal-cleanup-result01.public.json` | `21FF2A7A458C58523E05029FAB8DE1CEC45212722E02828DDDA53AFFDA567FB4` |
| `fresh-public-after-cleanup-cohort11-01.public.json` | `4716BDAB85204C9C31EE85D3D3AE3C47FCE2E0EB8053983F948EB03464551A97` |
| `p2-protected5-before-cohort11-docs01.public.json` | 기본 작업 폴더의 기존 설정5개 raw SHA 일치 |

다음 우선순위는 파일 공유 실패의 제어된 재현과 최소 수정/독립 검토, 새 회차의 B1 재검증입니다. 실제 제공은 Android34/1.0.196/internal release12/source612이며 새35 예약·게임 APK/AAB 빌드·서명·업로드는0입니다. 실폰/실제16KB/장시간 성능은 후순위를 유지합니다. 현재 사용자 조치와 질문은 없습니다.
