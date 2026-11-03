# C13 실제 컴파일과 시각 형식 경계의 입력 전 중단

2026-11-02 #259 후속 C13/cohort12에서 입출력 수정 소스의 실제 컴파일과 정상 두 피어 구성을 확인했습니다. 경로 발행 준비 중 시각 문자열 형식이 맞지 않아 Nav·B1 입력 전에 중단했습니다. PR261 Draft/#259 OPEN을 유지하며, 이 결과를 실제 발사 또는 native 수명 경계 통과로 합산하지 않습니다.

## 실제 확인과 미실행

- P11 QA는 detached `f055e4a84391bd9d8741c4e12855887b1ef72166`과 기존 공개3,984개/dirty18을 사용했습니다. 최신 main 전체 체크아웃이나 Quad D 인트로 포함 Android 빌드가 아닙니다.
- C13 source32/manifest를 새 네이티브 Editor PID57964/start `2026-11-02T07:51:03.7470360Z`에 결속했습니다. 명시적 컴파일·기본 비활성 진단8개는 진단0으로 통과했습니다. 실제 MVID는 `9cd52b70-5918-4878-af1d-bfb8015f76ab`입니다.
- 정상 닉네임·새 방 `SFFNOV02C12` 생성과 Android의 방 참가·팀·READY 화면을 관측했습니다. PC의 정상 START 처리 결과는 준비2명/양 팀이며, 실제 Game의 읽기 전용 query도 peers2와 상대 Actor를 확인했습니다. 이번 회차의 Android Game 직접 화면·설치 APK SHA/ABI 재조회는 별도 확보하지 않았습니다.
- 새 배치의 PC 위치는 `(5.625,0,-13.5)`, 상대 위치는 `(1.875,0,13.5)`였습니다. 예측/직접 query의 첫 충돌은 `Wall_01 (1)`입니다. 기존 회차의 Wall_04·18지점 경로를 재사용하지 않고 새15지점 모델 경로만 계산했습니다. 모델 제안은 실제 이동 성공이 아닙니다.
- Nav publisher의 `--prepare`는 `NAV_PUBLISHER_UTC_Z7`로 중단됐습니다. 공개 관측은 `2026-11-02T07:59:23.367008Z`인데 publisher가 소수부7자리만 허용했습니다. 실제 문자열의 정밀도와 7자리의 끝0을 동일하게 처리할 수 없는 형식 경계이며, 관측 시각을 임의로 바꾸거나 기한을 늘리지 않았습니다.
- Nav publication/dispatch·B1 Queue/Fire 호출은0입니다. 후속 B1의 파일 공유 충돌 수정이 실제 사격 경로에서 해결됐다는 근거도 아직 아닙니다.

초기 상태 기록에서는 별도 import-mode 진단을 `editor-beforeplay` 이름으로 기록한 운영 실수가 있었습니다. 읽기 전용 원본을 해시 그대로 `editor-importmode-observation01.public.json`에 보존하고, 정상 Editor 상태 진단을 새로 수행한 뒤에만 normal admission을 발행했습니다. 첫 경로 준비 관측도 별도 이름으로 보존하고 새 현재 관측을 사용했습니다. lease·nonce·intent·실패 마커를 삭제하거나 덮어쓰지 않았습니다.

## 입출력 수정과 두 시각 파서의 범위

앞선 cohort11의 원 IOException/PermissionError에는 정확한 Windows 번호와 실패 파일명이 없어 원인을 확정하지 않습니다. C13에 포함한 IO04는 현재 bridge의 알려진 message leaf에 한해 read-open의 정확한 공유 위반32만 제한적으로 다시 읽습니다. 접근 거부·파일 없음·read-body·JSON·SHA·write 오류는 재시도하지 않습니다. CreateNew·원 nonce·원 기한과 public 입력 경계를 유지합니다.

고정 Python의 기본 파일 읽기는 공유 위반에서도 errno13만 반환할 수 있어, 해당 message 읽기에만 lazy CreateFileW→FD 소유권을 사용합니다. 읽기 공유는 FILE_SHARE_READ로 강화해 열려 있는 writer의 부분 데이터를 읽지 않도록 했습니다. 파일 API 사용과 게임/native instrumentation을 구분합니다. 최대4 open·요청 간격2ms·local20ms는 재시도 결과의 수용 범위이며, 동기 OS 호출이나 Unity Pump를 강제로20ms 안에 끝내는 보장이 아닙니다. 기존 첫 IO 후보의 extra 성공 read 뒤 local20ms 검사 누락은 반례를 보존하고 수정했습니다.

이번 Nav 형식 문제의 별도 최소수정은 정확한 Z 시각의 소수부0~7자리를 수용하며 8자리·offset·잘못된 형식은 거부합니다. 기존 현재성30초·Nav45초·상위13/15분과 native100ns는 유지합니다. 독립103검사는 작은 시각 파서 소스의 근거이며, 새 C14 전체·실제 Nav/B1 성공은 별도입니다.

## 정상 종료와 자기 파일 정리

StopPlay → clean Untitled → frame cap -1 복원 → 자기 Editor 닫기를 진행했습니다. 닫기 CLI의 종료6/COMMAND_FAILED와 wrapper 종료1은 보존합니다. 별도 watcher77254는 동일 native57964의 실제 정수 ExitCode0·tool0·Dispose0을 확인했습니다. native 종료는 `08:01:48.5862742Z`, 공유 자원 실제 반환은 `08:02:42.2653463Z`이며, 강제 종료0입니다. FPS index2의 EXIT/OK → 홈 → 정상 닫기 → manager의 미실행을 확인했고 다른 VM은 조작하지 않았습니다.

첫 cleanup 입력 준비는 stage의 `+00:00` 시각을 strict-Z parser에 전달해 `UTC_Z_0_TO_7`로 쓰기 전에 실패했습니다. 별도 독립 검토한 cleanup 전용 패치는 stage event 시각에만 정확한 Z/+00:00를 허용합니다. native utc/ticks·원100ns·상위 기한·Source/lease·held Exit0·fresh guard·자기4 백업/해시 조건은 그대로입니다. 이 권한으로 경기 입력을 재개하지 않았습니다.

패치 후 Root가 새 guard/audit와 실제 watcher 완료 증거로 cleanup plan을 발행하고 자기 source/meta4개만 정리했습니다. 정리 후 공개3,984개 raw SHA·기존dirty18·P11 개인6 absent/skip-worktree/내용읽기·해시0·P2 보호설정5개 raw SHA를 확인했습니다. 원본·실패 영수증·각 후보·키/기존 산출물은 보존합니다.

## 재현 근거와 다음 단계

근거 루트는 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452/`입니다. 원시 CLI·로그는 공개하지 않습니다.

| 근거 | SHA256 |
| --- | --- |
| C13 source manifest | `2830CDE5159BF8B5CEFDC24CCDF531D4FD94511A8A45EE17EC54E3FEE611E354` |
| Source IR | `5B1D6B4B73C0CCF2CE7C5D95B327FCEFD92E6C8DD0C78BF098EA180ECAA4765E` |
| Controlled IR | `39FD6FE889F9C3C1694FBF896951C8EAB28F51E2A95EFE14EC4ABE3979717CD3` |
| IO04 최소수정 IR | `029C421F941C67801724DAD1E7178017D5A24A80CD9DAEF8A25A33FBA4EB2163` |
| Nav UTC 최소수정 IR | `031BAC367D30F2B76FCAEA9E6F8F661FB88B08C67F553EEEC7BDC900D2150C90` |
| cleanup 전용 최소수정 IR | `F026FDC85FD19CA6CD3420306ED22FD542E53148F951A6562D4B5BA85147446D` |
| `input261-b1-cohort12/owned-source-cleanup.public.json` | `04D70A2F58F2AAF31DE49C914B412E89A96C5FFD71A71561E635D9813C5137AB` |
| `fresh-public-after-cleanup-cohort12-01.public.json` | `2A48FB23276906294D11557B31B904304DC1DBC4D8ED5D8D94C9360D0DB10129` |

실제 상세는 `cohort12-final-pc-checkpoint01.public.json`, `cohort12-abort-before-navigation01.public.json`, `cohort12-cleanup-preparation-failure01.public.json`과 같은 native·read-only·cleanup 영수증을 함께 읽습니다. 다음은 새 C14 전체 Source/Control 검토와 Driver07 결속 뒤 별도 fresh 회차의 Nav/B1입니다. 사망·리스폰 native held FIRE/LOOK·fresh Down·native OS 수명 경계는 남습니다. 실제 제공은 Android34/1.0.196/internal release12/source612이며 새35 예약·게임 APK/AAB 빌드·서명·업로드는0입니다. 현재 필요한 사용자 조치와 질문은 없습니다.
