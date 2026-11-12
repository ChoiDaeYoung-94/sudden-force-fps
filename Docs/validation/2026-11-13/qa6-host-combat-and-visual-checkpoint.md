# QA6 A2 호스트 전투·재출현·폰 자세와 반환

2026-11-13 KST의 #347/#341 후속 표본입니다. 실행 입력은 P13 `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`/QA 통합43이며 문서 기준 main은 `c891fcd`입니다. A1과 별도 nonce·경기입니다. 동일 QA6 APK를 사용했고 운영 code34/release12·원장196,2,34와 제품 소스는 변경하지 않았습니다. PR345 단독 core39의 경기 성공이나 PR386 전체·제품 raw gait 수용으로 소급하지 않습니다.

## Host 사격과 재출현

native15012의 원창은03:13:12~new03:38:12/full03:43:12, Play MVID는 `eb8a07bf-0c5a-4aa0-b1fa-26996fde19ed`입니다. `Q6N13H02`에서 Editor Host/폰 client, Red1031/Blue1032·상반 팀·roster2를 관측했습니다. 양 actor는 Host StateAuthority 관측이며 pureproxy 피해 표본이 아닙니다.

정상 폰 tap1과 tap3 뒤 사수1032의 누적 shot/hit는0→1→3→4, ammo30→29→27→26이고 피해자1031은 **HP100→75→25→0/dead**입니다. 첫25피해는 같은 life0의 생존 피격입니다. kill 표본은 Blue K1/Red D1·점수0:1·source-death-pose입니다. 뒤 같은 target instance/NetworkID에서 life1/HP100/deadfalse/ammo30·poseReady/12 configured/root-aligned hitbox를 관측했습니다. 3tap 직후 첫 표본은 shot3이고 다음 kill 표본은 shot4이므로 tap별 즉시 반영·발사간격이나 모든 hitbox/recoil 경우를 확정하지 않습니다.

폰 결과 PNG의 VICTORY/TIME EXPIRED/RED0/BLUE1은 기존 독립 검토자가 직접 확인했습니다. Editor의 실제 Result 패널 정상 RETURN callback을 합성 호출한 영수증(syntheticCallback=true) 뒤 Lobby Reader는 **Connected·actors0·fixtures0**입니다. 물리 마우스 클릭이나 전체 재접속 경우로 확대하지 않습니다.

## 폰 자세와 이동 수치의 범위

`phone-close-idle02.png`는 실제 폰 한 정지 각도입니다. 독립 검토에서 상체 앞 소총, 뒤 손의 그립 부근·앞 손의 전방 하단 지지 외형을 확인했습니다. foreground viewmodel이 일부를 가리며 손가락/피부 완전 접촉·관통0·모든 각도·이동 중 자연스러운 파지의 수용은 아닙니다.

별도 Host Update 관측은15초 한도·876 frame행/1752 actor표본입니다. 폰1032의 PreviousSprint는43행, NCC 평면속도 관측 최대는 **7.31566429**, gaitPhase0→1.88981771입니다. 비networked 로컬 maxSpeed 필드 최대7.315191은 별도 값입니다. ActualSprintingAvailable=false이며 내부 sprint 분기·속도 사양 합격으로 추정하지 않습니다. 두 actor의876행은 poseReady/12configured·gaitFault/lifeHold0이지만 LastContext의 적용frame stamp와 native event의 정밀 시계 동기화는 없습니다. 연속 보행·질주 외형과 발 접지는 미수용입니다. TIME_LIMIT 뒤 늦은 frame의 자기 fixture0을 관측했습니다.

## 거부·실패와 별도 proxy 관측

위치 준비는 REJECT_NO_CLEAR_FLAT_POSITION_5_TO_8M/Teleport0·Before/After 동일·NonSetupFieldsUnchanged=true와 cleanup/fixture0만 남겼습니다. 첫 wrapper FIXED_BINDING 실패도 보존합니다. native 프로필12개 중11개는 주입 성공이며 `look-right01`은 **events0/error2/terminal1** 실패입니다. 뒤 성공으로 덮지 않습니다. 별도 validation camera도 URP/context guard에서 거부되어 **성공 PNG0**이며 폰 PNG와 합산하지 않습니다. 후속 읽기는 URP asset과 currentPipeline=null을 기록했고 제품 원인으로 판정하지 않았습니다.

다른 방 `Q6N13P03`의 Editor client pureproxy 관측은874행 뒤 **ENVIRONMENT_OR_CONTEXT_FAILED:InvalidOperationException**으로 끝나 cleanRun=false입니다. 저장 행은 HP100/life0이며 proxy 피격·사망·life 경계 수용이 아닙니다. UTC capture 종료와 stopwatch 순서의 불일치 경로가 정합하지만 예외 Message가 없어 원인을 확정하지 않습니다.

그중1행은 snapshot gap2496tick과 freshContext=false/SourceLifeAligned=false이고 다음 frame에 복구됐습니다. HP100/life0/deadfalse·hold0을 유지하여 실제 사망·HP equality 회귀로 해석하지 않습니다. 큰 gap 거부와 정합하되 실제 TickRate가 보고되지 않은 한계가 있습니다. 최종 generic failure와 이1행 fallback은 별개입니다. fixture0은 자기 객체 정리 근거이며 clean 관측 성공을 대신하지 않습니다.

## full 이전 반환

종료RPC의 Invalid response format 실패는 보존하고 별도 held handle의 Exit0/forcedfalse·Prefs2원복·cleanupFailures0·globalUnity0을 근거로 합니다. native 종료03:35:24, 폰 최종03:35:57의 game/helper 부재·동일ADB·설정1.0/0/0을 확인한 공개 영수증이 있습니다. Resolver/ProjectSettings generated2개 보존·원복 뒤 **03:36:16 public4122 exact/Gitclean/private6 absent-indexS**,03:36:30 SDK 후보0 재확인까지 모두 원 full03:43:12 이전입니다. A1의 full 이후 CPU 소스 원복과 구분합니다. 이 문서 작성자는 현재 전수해시·프로세스·기기를 재검사하지 않았습니다.

## 공개 근거

기준 폴더는 `D:/codex/validation/sudden-force-fps/20261112-night/qa6-runtime-launch01/actual-64bebba872b94c4a865d156e2828d9f2/`입니다. 기존 IR은 사격·시각·수치·proxy 실패를 나누어 검토했고, 최종 자원·source 반환은 뒤의 Root 영수증으로 연결합니다.

| 상대경로 | bytes | SHA256 |
| --- | ---: | --- |
| `independent-a2-combat-phone-pose-review.public.json` | 27150 | `BBCCAF8E7074F6D2861AF93EE1A9E57B706CAC9784FB11EE75F89447460B9807` |
| `pose-input-observer-independent-analysis01.public.json` | 9361 | `B8EDD81D5283E3874A0AA94F757242A1491341E21209D66C721CC5FF72AB5937` |
| `proxy-observer-failure-independent-analysis01.public.json` | 8087 | `14F05372134D91EF1C620D29EAFD1756E79C66E8B824CE5AE70DB3CDA2AC9E45` |
| `exit.public.json` | 595 | `E21D96C3090F08FE4EBD86531A5D43776B481DA3C820394C6CC2FA0D0FE305E0` |
| `phone-final-return01.public.json` | 260 | `07CE25945856BD4A10D20A163ED9F032A3F8F573A880095585E984774A61314F` |
| `source-return01/final-source-return.public.json` | 616 | `8CD037B0BA46ACB8BAD49C96EDD1258940BEB064CFED12C618BD29C9E7D5A416` |
| `native-sdk-final-return01.public.json` | 191 | `6E7F3F1B67944303E3D02D7376909792F458D236AF896747C7B048B082582405` |

전체 QA6 Game·순수proxy 피해·연속 이동 외형/접지·실청·실제16KB·전체권리·production 병합/배포는 미완료입니다. QA Login/LOCAL_QA/.localqa adapter를 제품 기능으로 수용하지 않습니다. 이 초안의 새 실제 검증·SDK/Editor/기기/Git 호출은0이며 원본과 실패를 변경하지 않았습니다. 현재 필요한 사용자 조치나 질문은 없습니다.
