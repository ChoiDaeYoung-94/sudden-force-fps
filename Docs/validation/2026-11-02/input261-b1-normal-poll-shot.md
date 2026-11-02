# C14 / cohort13 B1 정상 poll 사격 결과

2026-11-02 새 정상 두 피어 경기에서 공개 조이스틱 콜백으로 사선을 확보한 뒤, 공개 PlayerInputSource를 통한 단1회 사격 요청이 정상 시뮬레이션의 shot sequence와 탄약에 반영되는 B1 범위를 확인했습니다. 해제 후 관측 완료·추가 사격0과 자기 검증 객체 부재도 확인했습니다.

이 결과는 **명중·헤드샷·처치 성공이나 native held FIRE/LOOK 사망·리스폰(L2), 정밀 시각 수용이 아닙니다.** target HP100·gameplayAccepted=false·allProductStateRestored=false를 그대로 보존합니다. #259는 OPEN, PR261은 Draft입니다.

## 고정 입력과 실제 준비

- 공개 QA 프로젝트는 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`의 detached `f055e4a84391bd9d8741c4e12855887b1ef72166` 및 기존 로컬 변경18개입니다. 최신 일반 main 전체를 실행한 결과가 아닙니다. PlayerInputSource/CombatPointerControl 두 소스의 PR261 동등성 확인과 프로젝트 전체 최신 통합은 구분합니다.
- C14 소스32개+manifest의 raw manifest SHA256은 `80478BDF8B9F7A6C6E41355B26E09BD1C7AB947EE3380F493462886ABDBFEE88`입니다. 전체 소스 독립 검토 `BAC595198FE7DE09FCE48B9A8B1E4C30446B76E74F1778C8CEE0A218CCE177FB`, 통제검토 `295545D44429ADF091C2B6E1A694A4444D9BEFFC88A5DBE500C453CD3B93832A`와 새 Root/native/runtime lease를 결속했습니다. 과거 승인·nonce·프로세스 성공을 재사용하지 않았습니다.
- 새 P11 PID38372 / 시작 `2026-11-02T08:34:22.4679884Z` / Game MVID `910b7bab-1bc5-4578-8a7d-8f50992f0d4e`입니다. native identity의100ns 정밀도와 일반 이벤트 시각 표현을 구분합니다.
- Host·Queue caller·Navigation·Shot geometry의 미호출 compile/defaultfalse8개는 실제 진단0으로 확인했습니다. clean Untitled·Direct3D11·DOTNET1/BEE1·기존 frame cap -1을 확인한 뒤 정상 로비·닉네임·방 생성·Android READY와 host START를 진행했습니다.
- PC Game 화면과 읽기 전용 동일 epoch의 peers2를 확인했습니다. 이번 Android의 실제 Game 화면·설치 APK/ABI/SHA를 새로 확보한 것으로 확대하지 않습니다. 이후 Android 정상 로비·EXIT/OK·Home은 Root가 화면에서 확인했습니다.

## 이동과 읽기 전용 사선

초기 owner `(1.875,0,-13.5)`에서 첫 충돌은 `Wall_04`였습니다. 새 관측 숫자로 계산한17지점 모델 제안에 fresh observation·Root/lease·scope를 별도로 결속해 제한된 공개 Joystick 콜백을 실행했습니다.

| 항목 | 실제 결과 |
|---|---|
| 완료 지점 | 17/17 |
| 계획 / 표본 이동 거리 | 21.4798832m / 21.5498753m |
| 실행 경과 | 20.597289초 |
| Down / Drag / Up / Cancel | 17 / 507 / 17 / 17 |
| 콜백 관측 | 620 |
| 최종 waypoint 오차 | 0.03967453m |
| 해제 후 자연 프레임 / 안정 표본 | 5 / 3 |
| 공개 joystick 중립 / 해제 / 콜백 제거 / 위치 안정 표본 | true |
| 저장된 normal poll 중립 / input packet acceptance 입증 | false |

각 구간의 공개 해제와 자연 프레임 안정 표본을 확인했으며, 저장 입력 조회 불가와 완전 위치 고정 미입증을 유지합니다. 이동 후 별도 읽기 전용 predicted/direct query 모두 `EXPECTED_HEAD`·거리10.5698566m였지만, 이는 다음 실제 탄환의 명중 증거가 아닙니다.

## 단1회 B1 결과

새 metadata→READY→start request와 Queue caller1회 뒤 Host는 `B1_FIRST_ACCEPTED_SHOT`으로 종료했습니다. writer session24626의 실제 도구 종료 코드는0, stdout의 `b1Accepted=true`를 Host first-shot/terminal raw와 대조했습니다. stdout 사후 캡처 파일을 writer 자체가 별도로 emit한 원본 결과 파일로 표현하지 않습니다.

- owner shotSequence `0→1`, ammo `30→29`, acceptedShotDelta1 / ownerAmmoDelta1입니다.
- `seedAimCalls=1`, `fireTrueCalls=1`, terminal의 Fire(false)4회·Move(0)5회이며 공개 false/zero가 반환됐습니다.
- first-shot의 자기 객체 부재·post-stop 완료는 false였고, 후속 terminal에서 자기 Component/GameObject 부재·postStopObservationComplete=true·postStopAdditionalShots0을 확인했습니다. 두 시점의 값을 합쳐 최초부터 복구가 끝난 것처럼 처리하지 않습니다.
- target HP100·deathSequence0·respawnVersion0·kill0이며 명중/피해/처치·리스폰은 관측하지 못했습니다. preflight head query와 실제 탄환 수용을 구분합니다. first-shot owner aimYaw322.5와 preflight318.8816의 차이만으로 미명중의 원인을 확정하지 않습니다.
- `nativeInstrumentationLaunched=false`, `nativeL2Accepted=false`, `timingAccepted=false`, `targetRespawnObserved=false`, `gameplayAccepted=false`입니다. normalStoredInput.inputAvailable=false와 `allProductStateRestored=false`를 보존합니다. 복구 주장은 공개 Fire(false)/Move(0) 범위입니다.

오늘의 이전 파일 공유 실패는 [C12 기록](input261-route-and-prefire-io-stop.md), 엄격한 시각 형식 차단은 [C13 기록](input261-c13-compile-and-utc-boundary-stop.md)에 남아 있습니다. 새 IO04 경로에서 교환·사격을 완료한 이번 결과만으로 이전 code13의 원인 확정이나 실제 Winerror32 재시도 분기 통과를 주장하지 않습니다. Native 입력·강제 OnInput 호출·health/actor transform/private input flag 쓰기를 추가하지 않았습니다.

## 정상 종료와 보존

정상 Stop Play→clean Untitled→frame cap -1 복원→소유 Editor close를 요청했습니다. close CLI exit6/COMMAND_FAILED·wrapper1은 실패로 보존했고, 종료 전부터 보유한 **같은 native handle** watcher session43776에서 PID38372의 실제 ExitCode0·도구 exit0·Dispose 오류0을 별도로 확인했습니다. 프로세스 부재나 shell0만으로 Exit0을 추정하지 않았습니다.

Android는 정상 로비에서 EXIT/OK→Home→FPS 창 정상 닫기, 멀티매니저의0/1/2 미실행 확인→manager 종료를 진행했습니다. 새 GUI 목록과 scoped native guard에서 소유/미식별 Unity0·Temp lock 없음, 관련 창0을 확인하고 `08:45:10Z`에 공유 자원 반환을 보고했습니다. 이는 Root 반환 체크포인트이며 정확한 kernel/window 소멸 시각이 아닙니다. 부모 신규 입력 중단08:46:26.6220280Z·완전 반환08:48:26.6220280Z 전입니다.

same-native Exit0 뒤 검토된 cleanup-only 절차로 자기 CS/meta4개만 제거했습니다. 새 cleanup driver의 staged event 시각 파서는 `Z`/`+00:00`의 동일 UTC 표현을 처리하며 native identity/ticks 파서는 변경하지 않았습니다. 공개3,984개 raw 일치·기존dirty18·private6 부재/indexS를 재감사했고 P2 기존 설정5개도 Root가 새로 해시해 일치를 확인했습니다. 비공개6개 내용 읽기/해시는0입니다. 프로젝트 인자·복원 helper 이름·감사 출력 경로의 operator 호출 오류는 실행/쓰기 전 차단됐고 재호출과 구분해 외부 기록에 보존합니다.

## 핵심 증거

외부 근거 루트는 `D:/codex/validation/sudden-force-fps/20261102-work/resume-0452`입니다. private raw를 공개 기록으로 복사하지 않습니다.

| 공개 파일 | SHA256 |
|---|---|
| `input261-runtime-area04/input261-b1-phase13/l2-bridge/first-shot.public.json` | `3829E40BBC2B5F4B7A923062634EF9D4E433380912D1BA7DAC16E2648537BF4B` |
| 같은 bridge의 `terminal.public.json` | `17B92790B5789D3BEF6FC5C725AE2D8F236D5BD9BFDA39A952B87EAEBCA95D99` |
| `input261-b1-cohort13/native-exit.public.json` | `95B31303B41C2FBAF232D0D703BFECF6714845D8059D504F4CCC5B705A4E2AC0` |
| `cohort13-postcleanup-public-audit01.public.json` | `0227757B9C880A61F476786BD1E16E52D2EA9D7C5CEBDCC8CB78C3C0C16D1542` |
| `p2-protected5-after-cohort13-runtime01.public.json` | `6E760A5FE05311227BC8F60AE5006D7070A76E6D2723889CBAFE7263B99D8D12` |

## 잔여 범위

다음은 native held FIRE/LOOK의 실제 사망·리스폰·stale 차단·fresh Down 재무장과 남은 route/native OS 수명 경계입니다. L2 수집·판정 도구의 소스/통제 검토와 실제 실행을 구분하며 B1 성공으로 PR261을 병합하거나 #259를 닫지 않습니다.

현재 제공본 Android34/1.0.196/내부 release12·source61218033ff36a2da3770ad93b9cc6c10c105c92d 및 권위 원장196,2,34는 그대로입니다. 최신 main의 FIRE248 배치·Quad D 인트로를 이 제공본에 포함됐다고 처리하지 않습니다. 새35 예약/Android 빌드/서명/업로드0, 키 변경/이동/백업0이며 테스트 APK 외부 업로드0입니다. 실폰·실청·실제16KB·발열/장시간 성능은 후순위 합의를 유지합니다. 현재 필요한 사용자 조치와 질문은 없습니다.
