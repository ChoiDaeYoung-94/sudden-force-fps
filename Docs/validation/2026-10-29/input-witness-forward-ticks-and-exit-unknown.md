# Forward 입력 13 tick 관측과 종료 코드 미확인

2026-10-29 실제06에서 정상 두 피어 경기의 Forward simulation 저장 입력을 수집했습니다. 기대한 Move(0.5,0), life0, Fire·버튼 false가 서로 다른13 tick에서 일치했고 수집기·입력 공급기의 정리를 확인했습니다. 원래 pulse 본문의 complete=false/inputAvailable=false는 바꾸지 않았습니다. native Editor 종료 코드는 UNKNOWN이며, PR261 Draft/#259 OPEN과 사망·리스폰 L2 미완료를 유지합니다.

## 실행 대상과 사전 검사

- Windows 대상은 P11 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`, detached `f055e4a84391bd9d8741c4e12855887b1ef72166`과 기존 QA·설정 변경18개입니다. 최신 main의 clean checkout·새 Windows Player 빌드가 아닙니다.
- 입력 제품 소스2개의 Git blob은 PR261의 문서 동기화 전후 및 P11 filtered blob과 같습니다. PR261 head는 `f3eabfb86a1db7a2ba21cd8f07cf8c194e4eea47`, base는 `c29767c76ed7ff5c32b6d6c22a7fcd3a1fa27584`입니다. 동기화는 main 문서8개/+299/-0이며 제품 소스2개를 바꾸지 않았습니다.
- 새 Editor PID39700/start `2026-10-29T03:51:13.7252583Z`, DX11/nonbatch/DOTNET1/BEE1을 관측했습니다. 첫 상태 조회의 EDITOR_NOT_RUNNING은 시작 직후 실패로 보존했고 재launch하지 않았습니다. 후속 상태와 실제 before/type/after를 확인했습니다.
- MVID `e94bd0ea-00f6-41d7-a474-e9eec784d802`는 이전 회차와 같습니다. 새 모듈의 증거가 아니며 새 PID/start와 원문 핀으로 회차를 구분합니다. 같은 회차의 미호출·disabled wrapper20개, 원바이트 alias6개, 소유 CS/meta2개와 공개3,984개 감사를 확인했습니다.
- Android는 기존 FPS/index2의 제공34/1.0.196/source612 QA/내부 release12입니다. 정상 GUI로 닉네임3434, 방 SFFQA34_SW29_06의 두 팀2명 Ready→시작을 진행했습니다. 이번 회차의 직접 ADB·SDK·새 APK·서명·업로드는0입니다.

## probe와 paired 수집

Q01/Q02는 각각 자기 handler를 제거하고29→29·원 hash 복구를 확인한 뒤 실제 관측했습니다. probe는 false proposal을 검토하고 단일 D leaf를 발행한 뒤, 정상 Unity 활성화·Game 탭 선택 직후 같은 exec에서 queue와 terminal 확인을 이어 진행했습니다. D는 첫 tick READY/bodyInvoked=true였고 F probe는 COMPLETED/complete=true·복구6항목 true입니다. 정상 포커스와 요청 간격을 줄인 이번 단회 결과이며 이전05의30초 대기 원인 전체를 증명하거나 gate를 바꾼 결과가 아닙니다.

probe 완료 뒤 새 game02→direct02→Q02 순서를 지켜 pulse를 바인딩했습니다. paired 입력 핀12개와 source evidence50개, false 승인8개를 검토한 뒤 W4/C4를 발행하고 C의 witness SHA를 승인 W 원문에 다시 연결했습니다. C만1회 queue했으며 W Arm→BeginPairedPulse→F 호출 사이에 별도 IO를 넣지 않았습니다.

| 항목 | 실제06 결과 |
| --- | --- |
| probe D / F nonce | 110d82c5d54845fa999ceaac9d6aa525 / 9df03ab0a4eb400d939219191d175425 |
| pulse F / W / C nonce | 1fce7ee1f319407f8ccb9d4e511be60c / b3831e7612344522aba2bd066feb9e62 / 54692f6aa2fb496483478b76d34046f8 |
| handler 경계 | 원29→W30→F 추가31→최종29, 원 hash·registration 배열 복구 |
| 수집 | FixedUpdateNetwork에서 Runner.TryGetInputForPlayer로 읽은128행, 수집기 input writer0 |
| exact 일치 | F startTick17619 초과·sampledTick17660 이하 중17648~17660의13개 서로 다른 Forward tick |
| 느슨한 집계와 차이 | 같은 payload를 넓게 세면15개이나 seq42/43의 tick17661/17662는 pulse 상한 뒤이므로 제외 |
| 원본 pulse 결과 | reason=COMPLETED/errorType=null, complete=false/inputAvailable=false 보존, source·joy·handler·global·pointer·move 복구6항목 true |
| W 결과 | pairedBodyComplete=false 보존, error/cleanupError null, cleanupComplete·자기 watchdog/임시 객체 부재·기존 등록/객체ID·handler 복구 true, evidenceReady=true |

첫 exact 표본 seq29/tick17648과 seq30/tick17649도 Move(0.5,0), life0, Fire/모든 버튼 false와 동일 owner/source/Runner/scene·입력/상태 권위를 만족합니다. independent raw 재계산에서13을 확인했습니다. 의도된 owner pose 이동은 원복 조건이 아니며 전체 객체 상태·캐시·메모리 원복의 증명도 아닙니다. 이는 저장 입력과 복구의 관측 범위이며 원래 본문 aggregate를 true로 고치거나 Android native 입력·전송 구간 전체·경로·사망·전체 게임 수용으로 확대하지 않습니다.

## 정상 복귀, UNKNOWN 종료 코드, 별도 파일 정리

정상300초 경기의 DRAW/TimeExpired·양쪽 DONE을 관측했습니다. Android RETURN→PUBLIC ROOMS→EXIT/OK 뒤 Home을 확인하고 own index2를 정상 quit/list2 stopped로 확인했습니다. 첫 OK 시도의 unknown screenshotId 오류는 보존하고 새 창 식별·관측 뒤1회 정상 선택했습니다. UI 근거는 Root 도구 화면 관측이며 별도 PNG·독립 픽셀 검증이 아닙니다.

Host 결과 복귀의 최초 Connecting/list=false는 통과로 합산하지 않았고, 후속 Lobby Connected/list=true·retired=false·witness0을 확인했습니다. Play stop→clean Untitled→frame cap60에서-1 복귀 뒤 정상 종료용 Exit(0) 코드의 eval 요청은 CLI exit6/COMMAND_FAILED, runtime shell1로 끝났습니다. 이 실패는 통과가 아닙니다.

waiter session78239는 같은 Process 객체의 WaitForExit 뒤 반환됐지만 native exitCode=null을 기록했습니다. waiter shell/tool0과 native Exit0를 구분하며 원문을 고치거나 종료 코드를 추정하지 않습니다. 명시적 native handle 사전 보유가 없었다는 사실과 관측값을 보존하며, 원인 확정이나 향후 보강의 성공을 이번 종료에 소급하지 않습니다.

- 공유 자원 반환 기록은 `2026-10-29T04:05:51.1164456Z`, 원 한도는 `04:05:51.1995202Z`입니다. native/unknown0·TempLock=false, own VM stopped와 설정5개 승인14false를 기록했지만 당시 소유 파일2개 정리는 대기였습니다. 이 resource-only 시각을 전체 workflow 완료로 합산하지 않습니다.
- cleanup08은 FileInfo.Parent가 상위 경로를 순회하지 않는 결함으로 BLOCKED/실행0입니다. 새09는 FileInfo.Directory→DirectoryInfo.Parent 순회를 보완해 소스 검토를 마쳤지만 Exit0 조건을 충족하지 못해 실행0입니다. 이 발견은 이전 정리 소스의 전체 ancestor 검사 보장을 새로 입증하지 않습니다.
- 별도 cleanup10은 UNKNOWN을 보존한 비활성 소유 파일2개 정리 범위로 독립 검토했습니다. 새04:15:03 guard/native0, 04:15:04 공개3,984개·개인6개 제외 감사를 확인한 뒤04:15:25에 정확한 CS/meta2개만 제거했습니다. 이후04:15:26 감사는 공개3,984개 path/bytes/SHA 일치·개인6개 부재/indexS, untracked0과 기존 dirty18 보존입니다. 파일 정리는 GUI 한도 뒤의 경량 작업이며 native Exit0/wholeWorkflow 통과가 아닙니다.

Q/D/F는 승인2개씩, W/C는4개씩 모두false로 퇴역하고 원문·퇴역본·actual·consumed를 보존했습니다. 사용한 F pulse leaf를 삭제하지 않았으며 C의 승인 당시 witness SHA도 유지했습니다. 다음 회차에서 원래 W/C template나 과거 D baseline을 복원해 반복하지 않습니다. 기존 publisher의 원본 baseline 및 probe의 F 부재 조건을 다시 쓰려면 별도 전이 revision·검토가 필요합니다.

Root는 종료된 자기 Editor 로그157,413B에서 NullReferenceException/MissingReferenceException/Crash!!!/Unhandled Exception literal이 각각0임을 확인했습니다. 공개한 것은 원문 없는 count receipt뿐이며 독립 raw 로그 검토·Android logcat·모든 오류 부재·native 종료 코드의 증거가 아닙니다. CLI private quarantine와 보호된 개인6개 원문은 독립 검토에서 열지 않았습니다.

## 근거와 남은 범위

근거 root는 `D:/codex/validation/sudden-force-fps/20261029-work`, phase06은 `D:/codex/validation/sudden-force-fps/20261028-work/resumed-1448/simulation-witness-attempt06-20261029`입니다.

| 근거 | SHA256 |
| --- | --- |
| C actual receipt | 2EEEF90BC26EA3F3B5D38BC03E01FEDB7E9789D4504993E087B902A17BBBA470 |
| F pulse actual result | 91EAB06D8544F44CECE5FE1307697C6B79F4861762D856F3D45A3B8DD4C9EC0B |
| W actual result | 3C2C41D60ED9E3EA5FBB14E4B0B1DD71E8BB917B49CB468C554F98A72AA39919 |
| attempt06-owned-unknown-exit-cleanup-source-review01/verification.public.json | C376DFF663C5FE83FB118013748E6E01F2E2447D7E1A0240F7E64C32736EDAEB |
| 소유 파일 정리 receipt | 6D2E3E8AB86C93D56871233B48C7D5EDF5B7A560C96F5FF28C264DDF744D2FA6 |
| 정리 후 공개 감사 | 654F4FEE69B8B300390842E0CF5EFD01359D60B8F2F73CFFACE9B47614190F9F |
| UNKNOWN 보존 완료 기록 | 8D433BDEF69B7DC3BA940B957E05CC70529571B331CE4EE17174E8ACD94BE416 |
| packet06-actual-paired-review01/verification.public.json | B8AFE3E80C131C4A69C35C3CC5C60E257226D2195E1CBB8C76CCBC541ED63805 |

수집 입력·복구 근거와 종료 코드 UNKNOWN을 구분하며 PR261의 남은 held FIRE/LOOK 사망·정상 리스폰·stale 차단·fresh Down 재무장 및 route/native OS 경계를 통과로 채우지 않습니다. 제공 APK34와 현재 source를 구분하고 권위 원장196,2,34·새35 예약/빌드0을 유지합니다. 실기기·청취·실제16KB·장시간 성능·발열은 후순위이며 현재 사용자 조치와 질문은 없습니다.
