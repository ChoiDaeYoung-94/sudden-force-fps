# QA6 통합 APK·공기계 설치·cold 기동과 정상 종료 기록

2026-11-13 KST, issue #347의 local QA 통합 산출물과 공기계 한 대의 설치·기동·정상 종료 범위를 기록합니다. APK 빌드 입력은 P13의 고정 source `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`이며, 현재 main `75dc2c2`를 새로 빌드한 APK로 표현하지 않습니다. main은 문서 기록 기준이고 PR345·PR386은 Draft를 유지합니다. 운영 제공 code34/release12와 원장 `196,2,34`는 그대로이며, QA6를 운영 버전 예약·Play 배포로 합산하지 않습니다. Refs #347, #341.

## 고정 APK와 정적 산출물

| 항목 | 이번 기록 |
| --- | --- |
| 게임 패키지·버전 | `com.AeDeong.SuddenForceFPS.localqa`, code6 / `1.0.197-qa6` |
| 플랫폼 | ARM64, minSdk25 / targetSdk36의 QA6 manifest 검사 |
| 서명 APK | `D:/codex/releases/sudden-force-fps/localqa-1.0.197-qa6-20261113/SuddenForceFPS-LocalQA-1.0.197-qa6-ARM64.apk` |
| APK 전체 크기·SHA256 | **100,496,464B** / `F444239477FB1DC42610B689D4888DCAA021F133373674A65C7517228941E504` |
| unsigned APK | 100,489,992B / `12117E9D8AA18306854D930282FE94CADE7BCAF7CBED62ABD3F18CB221C4EC2A` |

Root의 이번 payload/SDK 공개 결과는 ZIP359항목 CRC·크기, ARM64 ELF64 native7개, Unity 내부 버전과 Intro→Login→Lobby→Room→DesertHouse의5scene 순서, QA6 manifest·zipalign 및 PlayGamesInitProvider 부재를 기록합니다. 서명 전후 ZIP의 이름·크기·CRC·각 payload SHA가 같고 서명본의 signing block이 존재하는 검사도 통과했습니다. unsigned의 `signed:false`는 예상 상태이며 SDK checker의 예상 unsigned native exit를 호출자가 실패로 오인했던 응답은 원본 보존·재실행0의 별도 공개 정정으로 구분했습니다.

선택 Intro 영상은88,032B/SHA `79839BA1B4CC0EE0D17BCD13AD8118F35B67B5FFEA5C5346D7F812253666C360`, 고지 TextAsset 본문은725,685B/SHA `F551A5F4B593DD6E0D5487C40EF3CED26EC5AC13295FC8620617E0EB3740C2E2`로 이번 payload 결과에 연결돼 있습니다. 이 정적 검사는 실제 실청·모든 고지 문자 표시·전체 에셋 권리 수용이 아닙니다. 최신 APK의 외부 업로드·Play/Drive 전달은0입니다.

## 빌드 완료와 관측 worker 반환의 구분

`q6-r2/build02`의 warm 실행은 `BUILD SUCCESSFUL in 23m 29s` 로그 요약과 소유 native handle의 **ExitCode0·forced:false·captureComplete:true**를 남겼습니다. source head/branch·Gradle3핀·공개 tool8핀·공유 ADB 세대도 유지됐고 cleanupFailures는0입니다. 정확한 native exit 시각은 별도로 제공되지 않아 exit 영수증 시각을 그 시각으로 대체하지 않습니다.

다만 exit 영수증 당시 관측 Java 한 세대가 살아 있어 원 래퍼의 `localUnsignedBuildConditionsMet:false`와 `resourceClosureAccepted:false`는 그대로 보존했습니다. 이를 원래 래퍼가 통과했다고 고쳐 기록하지 않습니다. 뒤의 Root 공개 관측은 원 native와390관측 descendant를 합한 **391개의 정확한 PID/birth 세대**가 모두 반환 또는 PID재사용 상태임을 확인했고, knownWriters0·추가 export tool0·SDK candidate0을 원 full 이전에 기록했습니다. 독립 검토 `CDAB7C7A…`는 정상 빌드 완료와 이 후속 관측 세대 반환을 별도 근거로 수용했습니다. 전체 가능한 SDK process tree의 전수·영구 부재를 입증한 것으로 확대하지 않습니다.

앞선 COMSPEC 경로 형식 실패, r2 첫18분 timeout, warm01의 StartTime null 사전검사 중단은 각각 보존된 실패이며, 이번 성공 횟수로 합산하지 않습니다. 원 q6와 r2/build01의 로그·영수증·캐시와 warm01 실패 자료도 보존했습니다.

## 공기계 설치·선택 기동 화면·정상 종료

공기계 `SM-A175N`의 기존 `.localqa` QA5를 **`install -r`**로 QA6로 교체했고 데이터 초기화 호출은0입니다. 설치 뒤 versionCode6/`1.0.197-qa6`를 확인하고 설치 APK 전체를 읽어 위100,496,464B/`F4442394…`와 일치시켰습니다. 이는 APK readback 동일성과 데이터 초기화 없는 업데이트 근거이며, 모든 사용자 저장 데이터의 전후 바이트 감사를 수행했다는 뜻은 아닙니다.

별도 test-only native helper도 `-r -t`로 설치한 뒤 전체 readback **16,528B/SHA `6E9096EE4CD579BA4E78EF54C64D244899435F7290AAF2F215CBD3CDA4B9B81C`**가 고정 helper와 같았습니다. 이 helper의 QA6 게임 버전 gate와 입력·HOME 프로필을 실제 실행해 통과했다는 수용은 아직 false입니다.

설치 후 cold-start 캡처는 연속 동영상이 아닌8개 선택 캡처입니다. Root는 프레임0·3·5·7과 로비 캡처를 확인했고, 첫 선택 프레임은 black, 프레임3은 Quad D 로고, 이후 로비 화면으로 기록했습니다. 프레임 번호는 보존 캡처의 번호이며 실제 표시 시간/FPS가 아닙니다. 선택 캡처만으로 인트로 구간 전체의 연속 흰 깜빡임0이나 실청을 수용하지 않습니다.

Root는 로비의 정상 **EXIT→OK** 경로 뒤 게임 PID 부재를 확인했습니다. `forcedStopFallbackUsed:false`, helper PID 부재·게임 focus 해제이며, `font_scale=1.0`, `accelerometer_rotation=0`, `user_rotation=0`이 설치 전후 같았습니다. 공유 ADB는 최종 호출에서도 같은 세대였고, 이번 창의 Editor/LD 기동은0이며 LD2는 OFF, LD2의 QA6 설치·테스트는0으로 유지했습니다. 이 한 번의 Android 정상 종료 표본을 모든 플랫폼 종료 안정화나 Windows 종료 원인 해결로 일반화하지 않습니다.

산출물·기기 작업은 빌드의 후속 반환 확인 뒤 새 별도 원창에서 진행했습니다. 최종 Root 반환은 UTC `2026-11-12T17:22:18.1710703Z`이며 해당 원창의 full 이전, SDK candidate PID 목록0·공기계 정상 반환을 기록합니다. 초기 window의 pending 상태를 사후에 덮어쓰지 않고 별도 return 영수증으로 결속했습니다.

## 공개 근거와 남은 범위

아래 경로는 이 PC에 보존한 검증 자료이며 빌드 필수 입력이나 외부 다운로드 주소가 아닙니다. `N`은 `D:/codex/validation/sudden-force-fps/20261112-night`, `R2`는 `D:/codex/q6-r2`입니다.

| 근거 | bytes / SHA256 | 범위 |
| --- | --- | --- |
| `R2/build02/gradle-exit.public.json` | 124,062 / `7D1DF57CCE375A9E8087A0894A3CD7838652EA8A730612F4406E0FA3FFB00859` | Exit0 및 원래 local/resource false 보존 |
| `R2/build02/root-known-worker-observation01.public.json` | 44,613 / `62025769ACC48813A24928BC530A2F40D2FCDE41DB36C14A22809A4BCF94B4C9` | 후속391관측 세대 반환 |
| `N/qa6-gradle-warm-retry01/actual-warm02-independent-review.public.json` | 8,327 / `CDAB7C7A69C0314F950D89A807845F67750098F1D68A4CE0F5FD25DA7F4F4514` | 빌드/후속 반환의 독립 검토; 서명·기기 수용과 별도 |
| `R2/unsigned-payload.public.json`, `unsigned-sdk.public.json` | 각각2,610B / 313B | 이번 unsigned payload/SDK 결과 |
| `R2/signed-payload.public.json`, `signed-sdk.public.json` | 각각5,218B / 334B | 이번 서명본과 전후 payload 동일성/SDK 결과 |
| `R2/device01/game-installed-readback.public.json` | 374 / `E6A4A8AE2DA8178E01AC5CFE666C15DE6056BF12A592EB479A196C1552D95E96` | 게임 설치 전체 readback |
| `R2/device01/helper-installed-readback.public.json` | 332 / `31236B725FA6862328F0DC2DA8B3CAD8563AB972EDEEEF4B88EF3B1694BB9789` | helper readback·실행 gate 미수용 |
| `R2/device01/cold-start-and-phone-return01.public.json` | 843 / `0383103F0B9CFDEF6FFEF956719D508649075E97B894F342444E9F8631F79F1A` | 선택 화면·정상 종료·설정 보존 |
| `N/qa6-artifact-and-device01/return.public.json` | 원 APK/설치 반환 영수증의 핀 포함 | 별도 산출물·공기계 원창 반환 |

새 산출물·설치 자체의 독립 검토는 아래 별도 절의 실제 영수증에 exact 핀으로 결속합니다. 위 빌드 반환 IR을 서명·설치 독립 수용까지 완료한 자료로 사용하지 않습니다. 이 문서 후보는 Root의 공개 실제 영수증을 대조해 작성했으며 작성자가 SDK·Editor·ADB·기기를 다시 실행하거나 APK를 새로 검증한 결과가 아닙니다.

현재 완료 범위는 새 QA6 통합 APK의 Root 정적 검증 기록, 공기계 보존 업데이트와 전체 readback, 선택 cold 기동 화면, 정상 종료 및 원창 반환입니다. **두 피어 경기·gait/순수 proxy·실제 Game 회귀·실청·실제16KB 기기·전체 권리·연속 흰 깜빡임0은 미수용**입니다. PR345의 standalone core39나 PR386 전체 제품 수용, 운영34 교체·외부 배포로 소급하지 않습니다. 현재 필요한 사용자 조치나 질문은 없습니다.

## 별도 실제 산출물·기기 독립 검토

`20261112-night/qa6-artifact-and-device01/independent-actual-artifact-device-review.public.json`(12,802B/SHA256 `032758193BF0A38C5A3D23F58FFDB3571E5B405EE9C2C37DFD363B14D332F0F9`)에서 두 서명 APK의 fresh ZIP/CRC/전체 SHA, 설치 readback·반환 영수증 연결과 선택 PNG 여섯 개를 독립 확인했습니다. 검토자는 SDK·기기·현재 프로세스·키·원시 로그를 조작하거나 조회하지 않았습니다. 이 문서는 02:22 반환 체크포인트이며, 이후 별도 Editor+공기계 경기 준비나 실경기 결과는 포함하지 않습니다.
