# QA6 순수 proxy 이동·조기 Stop과 반환

2026-11-13 KST, #347 후속 표본입니다. 실행 source는 P13 `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`/Networked43입니다. 기존 `.localqa` code6/`1.0.197-qa6` 폰 APK(100,496,464B/SHA256 `F444239477FB1DC42610B689D4888DCAA021F133373674A65C7517228941E504`)를 재설치 없이 사용했습니다. 기존 설치 readback은 [산출물 기록](qa6-artifact-and-phone-install.md)에 연결하며 새 APK/readback은 없습니다.

## 새 capture와 한정 완료

native38700의 원창은05:56:12~new06:21:12/full06:26:12입니다. 새 RAM MVID8dec0dcf…를 Load1회하고3×Arm(false), Reader(false)1회/IO0·fixture0·scene/roots/dirty/frame 보존을 확인했습니다. 원 observer17519B/C3503622…와 wrapper v2/536B56C0…를 새 native·원창·MVID·binding에 결속했고 capture-end 수정 후보는 사용하지 않았습니다.

`Q6P13R4`는 폰 Host Red1031과 Editor Client Blue1032의 두 피어 방입니다. 관측 Red는 input/state authority 둘 false인 pureproxy입니다. 폰 Move+SPRINT 두 pointer1회는28event/마지막 상대시각1461ms·errorCode0·injectedAll/pointer clear를 기록했습니다. legacy profile명 two-fire-up-first의 두 번째 pointer는 실제 SPRINT 원이며 사격 증거가 아닙니다.

15초 상한 안에서 **ROOT_STOP 약3.9765576초/230연속행(frame59085~59314)**을 회수했습니다. Stop frame59314의 fixture1은 Destroy 예약 상태이며 후속59378에0입니다. Stop/absence의 행은 같고 read failure0입니다. 정확15초 자동만료·즉시객체부재·TimeExpired 검증이 아닙니다. Position/Timer enabled write와 wrapper game setter는0입니다.

## 전행 fresh·commit은 아닙니다

PoseReady/GetterRead/statebuffer 유효는230행이지만 committed·LastSourceLifeAligned·FreshContextAvailable은229행입니다. 초반139행은 FromTick=ToTick=5893인 오래된 idle buffer입니다. StateTick==RunnerTick230행을 interpolation의 현재성으로 읽지 않습니다.

frame59224의 쌍5893→12392(간격6499tick)는 원 attachment 대기/LastSourceLifeAligned=false·fresh context 불가를 남겼고 다음59225의12392→12394에서 commit이 복구됐습니다. 공백·stale행을 제거하지 않았으며 정확 원인이나 network latency를 단정하지 않습니다.

가용행의 phase0→1.01923752와 복제 velocity 진행을 관측했습니다. 실제 Sprint boolean/속도사양·시각보행·접지는 미수용입니다. getter/from/to/selected HP100·life0·deadfalse, HP/life 선택 불일치0을 유지했습니다. 이번 순수 proxy 표본의 피격·죽음·재출현은 미실행이며 한행 LastAlive=false도 사망이 아닙니다. 적용 pose frame stamp가 없고 fresh context가 실제 적용 pose를 대신하지 않는 한계도 유지합니다.

앞선 [A2](qa6-host-combat-and-visual-checkpoint.md)의874행/generic InvalidOperationException 종료와 별도gap은 원본 그대로입니다. 이번 성공을 과거 원인의 확정 해결이나 전체proxy/HP 회귀 통과로 소급하지 않습니다.

## 정상 퇴장과 source 반환

회수 뒤 폰 Host 정상 LEAVE와 폰 Lobby를 확인했습니다. Editor는06:07:51 새 Lobby Runner-9766/Connected·actors0/fixtures0이며06:09 cached AppVersion `sff-q20261022-ladder-v1-intro1-riflecore2-gaitraw1-localqa`가 정확히 일치했습니다. Reader의 예상값만이 아닌 별도 cached값 관측입니다. 5분 대기나 TimeExpired 결과는 이번 범위가 아닙니다.

폰 정상 EXIT 뒤 game/helper PID 부재·forcedAppStopfalse·font_scale1.0/rotation0·0·같은 ADB 세대를 기록했습니다. Android ExitCode 전수 관측은 아닙니다. Play Stop 뒤 QA define 복원·stopped MVID643d…/QAfalse/clean Intro를 확인했고 clean Untitled로 복귀한 원 handle은 **06:12:38 Exit0/forcedfalse·Prefs2 복원·capture 완료·globalUnity0**을 남겼습니다.

public4122 감사의 차이는2개였습니다. ProjectSettings.asset은29741→28718B로 CRLF1023개→LF만 다릅니다. Resolver는1983→1904B이며 형식 외에 promptBeforeAutoResolution1→0, useFullMavenWhenNotExport0→1, versionHandler.verboseLogging0→1의 **설정값3개 의미 변화**가 있습니다. 모두 format-only라고 쓰지 않습니다.

원본/생성본 exact핀을 보존한 독립분류4368C3… 뒤 Root가 해당2path의 current핀을 재확인해 CAS 복원했습니다. PS 원본/생성 SHA는 EC1EC396…/9A26F046…, Resolver는01E25629…/3F5496C5…이며 full핀·공급 경로는 분류IR에 있습니다. 전역저장·의도 밖 파일 복원은 없었습니다. 최종 새4122 감사는 changed0/6A4911…이며 Root의 Gitclean·private6 absent-indexS를 연결했습니다. **전체 반환06:16:27은 원full06:26:12 이전**입니다. 문서 갱신만을 위한 추가 기기 실행은 없습니다.

## 별도 새 LD의 미수용

Root의 새빈 LD3 창은05:42:16~full06:12:16이고 실제 반환은05:53:36입니다. 공식 installapp 완료 UI·입력 APK핀은 확인했지만 guest transport/readback은 미수용이며 cold/warm 모두 black으로 Intro/Lobby도 미수용입니다. vendor manager C0000005와 VM exit0을 구분하며 정상 game 종료/호환성 통과로 쓰지 않습니다. 기존 개별 config/VMXML6개는 exact·새 VM은 OFF 보존·공유 ADB 동일 세대입니다. aggregate는 전후 보존했으므로 전체 설정 byte동일을 뜻하지 않습니다. 다른 Paw의 후속 LD 사건은 포함하지 않습니다.

QA7은 #395/PR396·source4f468…의 별도 code7 준비 후보이며 runtime/APK0입니다. main map수정bea…는 이번 QA6에 미포함입니다. 운영 code34/release12·원장196,2,34·Store35 예약0·Play/외부업로드0·CI/CD 확장0을 유지합니다. 전체 시각품질·피격/생명경계·권리/실제16KB·제품39 수용은 미완료입니다.

## 공개 근거

N=`D:/codex/validation/sudden-force-fps/20261112-night`, A=`N/qa6-runtime-launch01/actual-3288dd89dca34d22b97d0af7ef87412c`, C=`N/qa6-proxy-clean-stop-retry01`입니다.

| 경로 | bytes | SHA256 |
| --- | ---: | --- |
| A/proxy-motion-independent-analysis01.public.json | 12624 | `62B14FBD55E4CF4A7440A4B6A30BD21341226F611F6223D1F12807A9447388DB` |
| C/changes2-independent-classification01.public.json | 6364 | `4368C3F45501D43C6DDB71FE870B2017898763568C6DFA249275F61CD71EB2D2` |
| C/audit-0e48201567e846b495352a4da6a736f1/audit.public.json | 2435179 | `6A4911F3C3AC093EC4740CD1D64FF922656F609133D52491CF892CDCE117A53C` |
| C/full-return01.public.json | 1627 | `48D3FF66A3C84C17F4DA01346A55439FF0435F6334586679D97DC0D15050FF64` |

C/root-source-cas01.public.json(1263B/32D27C8B…)은 CAS를, N/ld-fresh-qa6-actual01/full-return01.public.json(945B/837F1E8E…)과 독립IR9088B/AF9707C0…는 LD 범위를 연결합니다. 공개 기록에는 지정 영수증의 요약만 포함하고 rawlog·개인 파일은 옮기지 않았습니다. Refs #347. 현재 필요한 사용자 조치나 질문은 없습니다.
