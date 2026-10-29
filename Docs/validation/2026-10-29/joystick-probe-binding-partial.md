# 조이스틱 probe와 바인딩 부분 검증

2026-10-29 정상 두 피어 경기에서 공개 조이스틱의 같은 프레임 probe와 복구, 실행 설정의 발행과 퇴역을 확인했습니다. 후속 pulse와 네트워크 입력 수집기는 실행하지 않았습니다. Refs #259, #166이며 PR261은 Draft로 유지합니다.

## 실제 실행 범위

P11 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`의 HEAD는 `f055e4a84391bd9d8741c4e12855887b1ef72166`이고 기존 검증용 소스 overlay를 보존했습니다. 최신 main 전체의 체크아웃이나 Android 통합 빌드 결과가 아닙니다. 기존 코드34 QA 앱과 정상 방 생성, 참가, 서로 다른 팀의 READY와 START를 진행했습니다.

자원 사용은10:26:31.8862251부터10:38:59.6405177 KST까지였으며 반환 한도10:41:31.8862251 안에 마쳤습니다. Editor PID14736의 시작 UTC는 `2026-10-29T01:26:55.0696439Z`입니다. Game, witness, pointer의 MVID `e94bd0ea-00f6-41d7-a474-e9eec784d802`와 MonoScript 타입 및 GUID를 새 프로세스에서 관측했습니다. 과거와 같은 MVID 값을 새 모듈 생성의 근거로 사용하지 않습니다.

| 단계 | 실제 결과 | 수용 범위 |
| --- | --- | --- |
| 미호출 컴파일과 기본 비활성 검사 | 20개 모두 exit0, outer/inner success, diagnostics0, source SHA 유지 | frozen, readonly, probe, witness, caller의 컴파일과 비활성 경계 |
| 정상 callback에서 읽기 관측 | 실제 관측1회, server/Running/activePeers2, 입력0 | 자기 callback 부재와 handler29개 및 hash 복구 |
| 공개 조이스틱 동기 probe | complete=true, 오류null, 복구6항목 모두true | 같은 프레임의 합성 Pointer 입력과 복구 |
| pulse와 paired 수집 | 미실행 | witness Arm0, paired 본문0, 수집 패킷0 |

동기 probe는 기존 활성 FixedArea 조이스틱에 공개 PointerDown, Drag, Up을 같은 프레임에 전달했습니다. 원 Source, 조이스틱, global 참조와 handler의 동일성, pointer 해제와 normalized move0을 확인했습니다. 원 결과의 `sampled=false`, `inputAvailable=false`, 변위0을 유지하며 simulation 입력 소비나 경로 이동 성공으로 해석하지 않습니다.

probe dispatch는 본문 호출 전에 자기 handler를 제거했고 호출 직전과 동기 probe 직후의 handler 복구를 확인했습니다. 읽기 관측과 probe는 CLI 응답 수신보다 먼저 끝날 수 있는 Editor.update callback입니다. 등록 응답을 본문 성공으로 대신하지 않았습니다.

probe dispatcher의 false 제안을 검토한 뒤 설정 leaf1개를 실제1319B로 발행했습니다. 승인된 바이트는 false 제안과 enabled/rootApproved 두 bool만 다르고, 이전 false 바이트와 발행 marker는 보존됐습니다. 이 결과는 probe 설정의 발행이며 paired 설정2개의 발행이나 패킷 수집 결과가 아닙니다.

## 시각 표기 차단과 복구

첫 읽기 예약은 Root가 작성한 UTC `+00:00` 표기로 `UTC_Z`에서, 두 번째는 점이 포함된 lease basename으로 `BOUNDED_PUBLIC_LEASE_PATH`에서 차단됐습니다. 두 응답 모두 registered=false, inputCalls0, output=null이며 해당 nonce의 actual 폴더와 consumed marker가 생성되지 않았습니다. 실패 응답과 설정 원문을 보존했습니다.

원래 lease는 수정하지 않았습니다. 네 시각을 같은 UTC instant의 `Z` 표기로 작성하고 허용 basename인 `lease-utc-z.public.json`에 새로 발행했습니다. 두 Z 사본의 raw SHA가 같고, 원 lease와는 네 시각의 표현 외 모든 필드가 같습니다. 자원, 동의, 시작·중단·반환 시각을 늘리지 않았습니다. 세 번째 읽기 예약에서 정상 callback 관측을 완료했습니다.

이 교정 뒤 probe까지 진행했고, Root의 남은 경기 시간 판단에 따라 후속 pulse와 paired 수집을 새 경기로 이월했습니다. 후속 binder의 시간 guard를 실제 호출해 실패시킨 기록은 아닙니다. 이전 Arm의 NullReferenceException 해결은 이번에도 입증되지 않았습니다.

## 정상 종료와 보존

Android에서 DRAW, TIME EXPIRED, 양팀0과 DONE, 정상 RETURN 뒤 PUBLIC ROOMS, EXIT 확인 뒤 Android 홈을 Root가 관측했습니다. 저장된 새 화면 파일이나 독립 픽셀 검토는 없습니다. PC는 활성 결과 버튼의 정상 handler를 호출했고 별도 읽기 응답에서 Lobby/Connected, 목록 수신과 witness 객체0을 확인했습니다.

StopPlay, 저장 없는 clean Untitled 전환 뒤 실제 frame cap60을 원래 값인−1로 복원했습니다. 종료 eval의 COMMAND_FAILED/exit6은 보존했고, 계속 보유한 같은 프로세스 핸들에서 UTC01:37:11.1047156/ExitCode0을 별도로 확인했습니다. 강제 종료나 종료 중 Unity 창 조회는 없었습니다.

stage 기준과 stage 후, Play 전, 종료 후, 임시 파일 정리 후의 공개3,984개는 path/bytes/SHA가 같고 private6은 부재/indexS였습니다. 정확한 바이트와 외부 사본을 확인한 자기 CS/meta2개만 제거해 untracked0을 확인했습니다. 기존 P2 dirty5와 P11 overlay를 보존했습니다.

읽기, probe dispatch, frozen probe의 승인 설정3개는 실제 원문과 퇴역 사본을 남기고 enabled/rootApproved만 false로 바꿨습니다. 직렬화와 EOL 차이는 원바이트와 구분했습니다. 완료된 frozen probe의 mutable leaf1개만 제거했고 actual 결과, consumed marker, source와 template는 보존했습니다. 현재 probe dispatch false SHA는 `495603385E4A461AF155E9D3B1781698413C97038B17FC9D8FC0EE57ACABBC6C`이며 witness/caller의 초기 false 바이트는 그대로입니다.

## 다음 작업과 근거

다음은 새 경기의 실제 pulse와 witness 등록 및 패킷 수집입니다. 사망/리스폰과 전체 PR261 수용도 남아 있습니다. 제공 코드34/source612 QA, 원장196,2,34를 유지하며 새35 예약, 게임 AAB/APK 생성, 서명과 업로드는0입니다.

공개 근거는 `D:/codex/validation/sudden-force-fps/20261028-work/resumed-1448/simulation-witness-attempt03-20261029/`와 `20261029-work/`의 before-stage03, before-play03, after-packet03-final 및 after-packet03-cleanup 감사입니다. 읽기 관측은 `joystick-readonly-dispatch01/actual-70f55a25d47d4266a98cd5bfc9d292e5/observation.public.json`, probe는 `joystick-input-candidate03/actual-baef52b33f174af7aed2401ad00d1082/result.public.json`, dispatch는 `joystick-input-dispatch01/actual-e35d85ca1f064b3eb5fc5b1aa73ede6e/receipt.public.json`입니다.

독립 실제 검토 `20261029-work/packet-attempt03-actual-review01/verification.public.json`의 SHA256은 `7CAF0D9F2E43B9B4D4FAE2CA8F0847790106FE9D365A5E3B9EF1F310E6042D2B`입니다. 검토자는 공개 영수증과102개 근거 해시를 대조했으며 새 GUI 조작이나 P11 전체 파일 재해시를 수행한 것으로 확대하지 않습니다. 판정은 이번20개 검사, 읽기 관측, 동기 probe와 정리 범위의 noBlockers=true입니다. 현재 필요한 사용자 조치와 질문은 없습니다.
