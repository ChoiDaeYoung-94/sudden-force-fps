# 코드34 유지 상태의 입력 도우미 갱신

2026-11-02 소유 FPS2 환경에서 기존 서명된 테스트용 입력 도우미를 한 번 갱신하고 설치 바이트와 등록 상태를 확인했습니다. 게임 APK는 코드34/1.0.196 그대로입니다. **instrumentation 실행은0이며 native L2·사망/리스폰·정밀 timing·packet 수용을 검증한 결과가 아닙니다.** PR261은 Draft, #259는 OPEN을 유지합니다.

독립 실제 검토는 **정확 Source06·이번 scope의 도우미1회 갱신, 게임 APK 보존과 반환 체크포인트** 범위를 수용했습니다. noBlockers/actualDevicePreparationAccepted/installationReviewAccepted/resourceReturnAccepted=true이며 새 실행 권한·ready·native L2 수용은 false입니다.

## 실제 범위와 결과

Root 자원창은 UTC `10:17:30.5173938Z`에 시작했고 신규 작업 중단은10:30:30.5173938Z, 완전 반환 한계는10:32:30.5173938Z였습니다. 실제 반환 확인은 `10:29:06.8116850Z`로 원기한 안입니다. 반환 시각은 Root의 새 관측 체크포인트이며 정확한 kernel/창 소멸 시각은 아닙니다.

검토된 수집기 Source06의 고정 공개 입력·자기 FPS2 범위로 observe-before→install-helper-once→observe-after를 진행했습니다. 명령 metadata는 각각38/79/38개, 총155개이며 모두 정수 exitCode0·timedOut=false·outputExceeded=false·ownedChildTerminated=false입니다. 그중 `install-exact-helper-once` label은1개입니다. raw SDK 출력이나 명령줄 원문은 공개하지 않았습니다.

| 대상 | 설치 전 | 설치·후속 관측 |
|---|---|---|
| `sff.qa.nativeinput` | 16,588B / SHA `7AE1DA50FCF9B19C788873A3374CD023699045DFEADB50769CBA5B0BF0877A6E` | 16,588B / 기대 SHA `DFFC4933C50C4C8C9AD61A171553ABA816AFB3C3A5EF7D50144DF2B745A106F3` |
| 도우미 범위 | testOnly=true | self-target/testOnly=true·instrumentation 등록=true |
| 게임 | 코드34 / 1.0.196 / 105,690,443B | SHA `09403527CFC04B74B85942BB7DFAFB0E727FA30BB0B8660AB2EB010E116E7F76` 동일 |

도우미 설치는 고정된 기존 서명 APK에 `-r -t`를 적용한1회이며 재빌드·새 서명이나 키 접근을 수행하지 않았습니다. 설치 후와 후속 관측에서 기대 도우미 SHA를 다시 확인했습니다. 게임은 재설치0·split0이며 game PID2404·foreground=true·1280×720 관측이 이어졌습니다. 이는 당시 소유 환경의 관측이며 실폰 검증이나 향후 실행 시점의 설치 상태를 대신하지 않습니다.

## 반환 판정의 정정과 정상 종료

첫 `resource-return.public.json`은 returned=false로 보존했습니다. ISO 시각 문자열을 기본 ConvertFrom-Json이 System.DateTime으로 자동 변환한 뒤 문자열로 비교해 기존 server의 동일성을 잘못 거부한 결과입니다. false receipt를 덮거나 원 서버를 재시작하지 않았습니다.

새 `resource-return02.public.json`은 `-DateKind String`으로 문자열을 보존하고 프로세스·server·연결을 다시 관측해 returned=true를 기록했습니다. 소유 player/VM PID45604·42920은 부재였고 GUI 대상 창이 없었습니다. 기존 server PID54172·시작09:59:04.8454536Z·실행파일은 동일하며5037 listener 소유자도 같고, 소유5559 established link는0입니다. 다른 server나 VM을 종료한 결과로 합산하지 않습니다.

Root는 정상 종료를 위해 게임 닉네임 안내에 테스트 값 `11`을 입력한 뒤 EXIT/OK→Android Home→FPS 창 정상 닫기→멀티매니저0/1/2 미실행 확인·정상 닫기를 진행했습니다. 따라서 **APK 바이트 보존과 PlayerPrefs 전체 무변화는 다릅니다.** 한 좌표 입력은 Shell 알림이 대상을 가려 도구가 거부했고, 대상 재활성화 후 Return으로 회복했습니다. 공개 반환 기록의 외부 앱 입력 발행0은 이 거부·재선택 경계와 함께 읽습니다.

## 핵심 공개 근거

외부 근거 루트는 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452`입니다. 아래는 공개 JSON 원raw SHA256이며 개인 경로·원시 출력·비밀 파일을 옮기지 않습니다.

| 공개 파일 | bytes / SHA256 |
|---|---|
| `device-prep-cohort01/observe-before.public.json` | 2698 / `A625C86C46F14BFF35B4BDA392C64485C4CA96276960E23C3CE1FB75FEA3DD2A` |
| `device-prep-cohort01/install-helper-once.public.json` | 5514 / `9386B53DD970BC61679A9FA32C8ECBE59580B03AE3CF2FE79480DD90F1F1CB96` |
| `device-prep-cohort01/observe-after.public.json` | 3959 / `BDEFE85C8B890D774C0EB6023004BD47A57510716674AD0109BE65803CCA0189` |
| `device-prep-cohort01-resource-return.public.json`（첫 false） | 1839 / `DEBCFD0210CD09ED5DCF787424669911ACC25F7EBA7358BE7D5CC9964F731E50` |
| `device-prep-cohort01-resource-return02.public.json`（새 true） | 2400 / `12B99FF649E93D9565AA002B7A4BF5E041003A5DB8A37A0920D271F046C5C720` |
| `device-preparation-actual-independent-review01/verification.public.json` | 11949 / `1365BF0DD4DD98DBE26F043DD21062069FA45384741B421609ACB58A3B0EF027` |

설치 receipt는 before/source/config/signed source·APK provenance를 결속하며, 후속 observation과155개 command metadata를 함께 추적할 수 있습니다. 수집기 source SHA는 `ACFF99020B1774521E12F70FF4745463458C89C59AFE797B322E5A0C3ADF5849`입니다. 독립 검토의 사전 체인12·설치 체인8·최종 공개 receipt 결속4검사와 합성 변조23개를 구분하며, 합계를 실제 기기 성공47회로 표기하지 않습니다. 고정 입력177개 원raw 핀과 source/통제 시험 수도 명령155개와 별개입니다.

## 한계와 다음 작업

이번 작업은 도우미 설치·관측·자원 반환 범위입니다. 실제 instrumentation/native 입력0, nativeL2Accepted/timingAccepted/packetAccepted=false입니다. 도우미 등록=true는 실제 held 입력32회나 게임의 입력 소비·사망·리스폰·fresh Down 재무장 증명이 아닙니다. [앞선 B1 정상 poll 사격](input261-b1-normal-poll-shot.md)의 공개 입력 결과와도 구분합니다.

다음은 수용된 설치 IR를 원SHA로 결속하고 새 standalone L2 자원창에서 설치/게임/server/recorder를 fresh 관측하는 작업입니다. 종료된 이번 scope·PID·receipt를 새 L2 권한이나 현재 사실로 복사하지 않습니다. 실제 수집과 외부 두피어 association·사망/리스폰 증거, 이후 판정이 남습니다.

제공34/release12와 PR261 Draft/#259 OPEN은 그대로이며 새35 예약·빌드·서명·업로드0입니다. 키 변경·이동·새 접근0이고 게임 전체 상태 복원이나 PlayerPrefs 보존을 주장하지 않습니다. 현재 필요한 사용자 조치와 질문은 없습니다.
