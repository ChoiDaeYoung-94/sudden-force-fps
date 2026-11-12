# 인트로의 유효 영상 프레임 대기와 PC 전환 검증

프로젝트 소유 `SuddenForceIntroCanvasOutput.LateUpdate`의 영상 표시 조건 한 곳을 바꿨습니다. texture가 먼저 할당돼도 `VideoPlayer.frame<0`이면 RawImage를 끄고 기존 검정 배경을 유지합니다. 유효 frame과 정상 texture가 준비된 뒤 기존 binding으로 이어집니다. 패키지의 재생·오디오·완료·장면 전환과 건너뛰기 동작은 변경하지 않았습니다.

Unity6000.3 문서는 [VideoPlayer.frame](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityengine/video/videoplayer/frame)의-1을 유효 frame 부재로 정의합니다. **격리 GPU Editor에서 새 코드 컴파일과 정상 영상 binding·완료 정리·Login 전환을 확인했습니다. 현재 Android 설치본 QA2에 포함됐거나 기기의 흰 화면을 해결했다는 뜻은 아닙니다.** 시작 배경색 수정과 실제 새 APK의 콜드 비교는 [별도 기록](android-startup-background.md)을 따릅니다.

## 실제 실행에서 확인한 상태

P13 D7 `d7a3371296357a5ceb1b7b657d468415130e7047`에 후보 C# 한 파일만 일시 적용하고, 기존 QA define용 rsp/meta2를 별도 stage했습니다. 후보4,637B/SHA `9CC017A2A9493AE4259B466DEB57D8BC2F1E495F4A989B51AFCE106EA618B580`는 제품 PR의 같은 소스 본문입니다. 이 회차에서 ProjectSettings 배경색·버전·서명이나 패키지 재생 소스는 바꾸지 않았습니다.

새 원창은 UTC17:27:20/신규 중단17:37:20/전체 반환17:42:20, nonce `dcb93bed1278409e9d54f077df905a8b`이며 연장하지 않았습니다. Direct3D11/compile·update false/compilationFailed false와 새 Assembly-CSharp 쓰기 시각, 관측 코드의 기본 DISABLED·진단0을 확인했습니다. Play 전 active scene은 empty였으나 기존 초기화 뒤 실제 안정 Play는 Lobby였고, 프레임267→2,199 증가를 확인한 상태에서 관측기를 먼저 등록하고 정상 `LoadScene(Intro, Single)`를 요청했습니다. Empty가 Play 내내 유지됐다고 표현하지 않습니다.

관측기는 `sceneLoaded`부터 읽기를 시작하고 Login 도달 시 끝났습니다. [Unity의 sceneLoaded 순서](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityengine/scenemanagement/scenemanager/sceneloaded)는 OnEnable 뒤·Start 전입니다. 관측 core는 영상 재생/컴포넌트 setter나 scene load를 호출하지 않으며, Root의 마지막 바인딩만 정상 scene load를 요청했습니다.

| 실제 표본 | 관측 결과 |
| --- | --- |
| Intro `sceneLoaded` 첫 표본 | frame -1·미준비·texture 없음·image off·HasBoundTexture false |
| texture가 먼저 준비된4표본 | 1920×1080·prepared/playing true·frame -1·image off·HasBoundTexture false |
| 첫 **관측된 표시 binding** | video frame2·1920×1080·image on·현재 texture 결속·HasBoundTexture true |
| 완료 전환 표본 | completed true·CleanupFailed false·image/canvas off |
| 종료 | LOGIN_REACHED·총226표본·관측 callback5개 해제, 뒤 Lobby에서 adapter/인트로 Canvas0 |

첫 표시 binding이 frame2였다는 것은 첫 decoded frame이2라는 뜻이 아닙니다. texture가 존재하면서 frame-1인4표본은 확인했지만, 메서드 branch coverage·정확한 LateUpdate 순서나 원자적 스냅샷을 계측한 것은 아닙니다. 영상 픽셀·GPU 표시 완료·실청·Android의 흰 구간·정확한 초기 지연은 이 PC 읽기 검증에서 수용하지 않습니다. 관측기가 video를 Play/Pause/Stop하거나 완료를 강제로 호출한 횟수는0입니다.

## 실행 전 차단과 원복

관측기 첫 두 준비본은 각각 Play domain reload와 LoadScene의 다음 frame 완료 순서를 제대로 다루지 못해 소스 검토에서 차단했고 실제 실행은0입니다. 셋째 준비본은 이미 안정된 Play에서 callback5개를 등록한 뒤 정상 Intro 로드를 기다리는 방식입니다. Domain Reload 설정을 바꾸지 않았습니다.

첫 stage 도구도 OWN2의 누락/변조가 final ready 검사에 빠지는 RAM 반례2건으로 실행 전에 차단했습니다. 원01/실패 검토를 보존하고, 새02에서 OWN2 전체 bytes·intent readback과 untracked 정확 집합 검사를 추가했습니다. 독립 RAM15개 검토 뒤에만 실제02를 실행했습니다. 이 도구 검사 수는 게임 기능 통과 수가 아닙니다.

보유 Editor14632는 UTC17:30:11.4118167에 정상 Native Exit0/not forced·EOF2·Dispose0으로 종료했습니다. 관측 자손8개 동일 generation 부재/direct-parent 후보0/all native Editor0, 두 ADB 종료 Prefs의 원래DWORD1 복원과 같은 server generation/exact connected peer를 확인했습니다. 전체 birth history/unknown child 전수 종료 증명은false이며 daemon 시작·종료·재시작과 기기 protocol 명령은0입니다.

첫 return은 후보 C# 원본 복원·rsp/meta2 회수를 완료했지만 EDM1개 차이로 전체 baseline=false/exit1을 반환했습니다. 이를 통과로 바꾸지 않았습니다. 생성1904B/SHA `3F5496C530EACDCBF09AFF9A5F2A3836D5321DD975606503EB8FE2BA684BD193`와 원본1983B/SHA `01E2562923C891ACD39BCBD35BCA486695032950F41637A0FF4B8A410C2B1B6F`를 외부 보존하고 정확한 원본 pin의 그 경로만 별도로 복원했습니다. 새 Root 감사에서 공개4,095개 raw exact·private6 absent/indexS·D7 Git clean·임시2 부재와 P2 기존 dirty5의 bytes/status 보존을 확인했습니다.

holder 승인4개를 회수하고 **UTC17:31:27.6561962에 실제 전체 반환**했습니다. 원 new/full 이전이며 현재 이 회차의 공유 호출·예약은0입니다. 원창·실패 결과·frozen core를 지우거나 새 성공 proof로 바꿔 쓰지 않았습니다. 전역 Save·새 Android 빌드·서명·업로드는0이고 운영34/release12·원장196,2,34 및 설치 QA2는 그대로입니다.

## 근거와 후속

외부 루트는 `D:/codex/validation/sudden-force-fps/20261111-work`입니다. `intro-frameguard-candidate01/actual-0a43967c5a08477d9e226bd7631b0014/summary.public.json`, `root-resume01/frameguard367-full-return.public.json`, `intro-frameguard-stage-return02`의 stage/return 및 별도 resolver 복구, holder/Prefs·P2 보호 전후 기록을 함께 따릅니다. 원시 로그·기기 식별자·키/자격정보는 공개하지 않습니다.

다음은 배경색과 이 guard를 함께 담은 새 localqa/code3·1.0.197-qa3의 정상 Unity export/정적 검증·기존 키 서명, 데이터 보존 갱신과 공기계 콜드 비교입니다. QA3는 운영 앱과 분리된 기존 테스트 패키지의 갱신 후보이며 아직 생성하지 않았습니다. 현재 필요한 사용자 조치·질문은 없습니다.

독립 실제 검토는 저장된 공개52개/Root 반환7개 pin을 대조해 제한된 정상 흐름과 기록된 반환만 수용했습니다. 원본 관측기를 실제 요청 code와 대조했고, bound.cs 보관본에는 요청 code보다 마지막 LF가1개 더 있는 차이를 명시했습니다. reviewer의 실제 기기/Editor/Git·전체4095 감사 재실행은0입니다.

| 자료(위 외부 루트 기준) | bytes | SHA256 |
| --- | ---: | --- |
| `intro-frameguard-observer-independent-review01/verification.public.json` | 8922 | `31CB61269FD62435D4925FFBCB06574064B1698E0D9EF0C478DCBCDF9A44568B` |
| `intro-frameguard367-actual-independent-review01/verification.public.json` | 6278 | `CB1790D6905E6395CCDCC3CEAE9EB01529F491BB9D38146B63332978B2A47B4B` |
| `root-resume01/frameguard367-full-return.public.json` | 3854 | `7CE37DAAC37E75CA212017E1B1881315272EFC85C9FE1C6A327D4FA5EDDF86C3` |
