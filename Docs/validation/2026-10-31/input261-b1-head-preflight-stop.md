# B1 실제 두 피어 실행과 발사 전 중단 (#259)

2026-10-31 C7의 새 실행입니다. 대상 P11은 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`, HEAD `f055e4a84391bd9d8741c4e12855887b1ef72166`과 기존 dirty18입니다. source25 manifest는 `049021EEBED1AA5765A9752FDD8D59C280324BE52EADB3956619102B68677D05`이며, 이전 C4 compile-only 성공을 C7의 증거로 재사용하지 않았습니다.

현재 native PID12632/start `2026-10-31T01:51:40.2122528Z`, 직접 ticks `639290083002122528`, 실제 game/source/bridge MVID `9c9bb536-9a1d-4093-ad4b-e6dde85139ed`를 결속했습니다. C7 실제 컴파일·typed delegate 미호출·Host/Queue/Close 기본 false 검사4개는 각각 innerSuccess=true·진단0·sourceStable=true입니다. 기본 false 응답의 등록·입력·파일 IO·종료 호출은0입니다.

PC 호스트와 기존 Android34 FPS 에뮬레이터가 정상 닉네임·방 참가·서로 다른 팀 READY/START로 DesertHouse Game에 진입했습니다. 읽기 전용 관측은 같은 epoch·2peer·Game focus=true/noHardware=true·양쪽 HP100/ammo30/life0·scene dirty=false를 확인했습니다. Android 닉네임은 개별 ASCII 키로 입력했고 clipboard type_text·ADB·다른 프로젝트 제어는0입니다.

Root의 새 단독 lease·정확한 false proposal 검토 뒤 Writer/Host 설정2개를 발행하고 Queue를1회 호출했습니다. Queue 응답 자체는 innerSuccess=true이나, Host는 metadata 단계의 `AimPreflight`에서 `SIGNAL_CONTEXT_OR_QUERY_READONLY_CAST_FIRST_HIT_EXPECTED_HEAD`로 중단했습니다. 정상 cast의 첫 충돌이 기대한 head hitbox인지 확인하는 조건이며, 현재 증거만으로 miss·월드 지형·다른 hitbox 중 원인을 확정하지 않습니다.

Host terminal SHA는 `3B71A8DAF723A61E059B63BD21772B886FCE95F5B49AD2F0B2B547C076513B90`입니다. 실제 Fire(true)·SeedAim·accepted shot/ammo delta는0, 양쪽 HP100/ammo30을 유지했습니다. 공개 Fire(false)/Move(0)는 각각3회이며 소유 runtime component/GameObject 부재를 확인했습니다. READY·baseline·START·first-shot은 생성되지 않았고 gameplayAccepted=false입니다. postStopObservationComplete=false이므로 기록된 postStopAdditionalShots=0을 완료된 사후 관측 통과로 합산하지 않습니다.

Writer는 `HOST_TERMINAL_BEFORE_READY`/exit1, b1Accepted/nativeL2Accepted/timingAccepted=false로 종료하고 stop request를1회 발행했습니다. 원래 stdout 파일은 별도 보존되지 않았으며, `writer-observed-completion.public.json`은 Root가 실제 tool 완료 응답을 옮긴 관측 기록입니다. 이를 원시 stdout 파일의 독립 해시 검증으로 표현하지 않습니다. 같은 phase/nonce의 재Queue·수정·자동 재시도는0입니다.

정상 StopHost → clean Untitled → frame cap -1 복원과 idle 관측 뒤 자기 PID/start를 검증하는 CloseOwnedEditor를1회 호출했습니다. close CLI는 exit6/COMMAND_FAILED이며 그대로 보존합니다. 별도로 동일 PID/start의 사전 보유 native handle에서 알려진 정수 ExitCode0을 읽었고, watcher tool0·disposeErrors0·handle retained를 확인했습니다. CLI 실패를 성공으로 바꾸거나 CLI 상태를 native 종료 근거로 대신하지 않습니다.

이번 stage→initial ownership→runtime lease→native binding→held exit→fresh closed/audit를 결속해 소유 source/meta4개만 제거했습니다. 최종 Root 감사는 공개3,984개 raw exact·기존 dirty18 보존·private6 actual absent/indexS·owned4 absent·native0/unknown0/TempLock=false입니다. Android는 호스트 종료 후 Public Rooms로 복귀했고 EXIT GAME/OK 뒤 홈 화면을 확인했습니다. 전역 저장·강제 종료·키/자격정보·Android 예약/빌드/서명/업로드는0입니다.

Root cleanup 입력 생성 첫 시도는 stage row에 없는 `name` 키를 읽어 KeyError로 멈췄습니다. 당시 source 제거는0이며 이미 보존한 native/waiter 기록은 수정하지 않았습니다. 별도 revision02에서 실제 path의 파일명을 사용해 fresh audit/config를 생성한 뒤, 검토된 cleanup으로4개 제거를 완료했습니다. 첫 실패와 원본 script는 그대로 보존했습니다.

독립 실제 검토는 B1 실패를 유지한 채 C7 컴파일/기본 false4개·동일 native 정상 종료·소유4 정리·보존만 한정 수용했습니다. Root 감사3개의 공개3,984행 metadata가 exact이며, 검토자가 **ProjectSettings.asset을 제외한 공개3,983개/749,467,382B를 직접 재해시**해 모두 일치시켰습니다. PS1행/private6 부재/indexS는 Root metadata 대조 범위이며 독립 실물 조회·해시는0입니다. 캐시 이미지2개는 JPEG 원바이트를 확인했고 잘못 붙인 .png 원본을 보존한 채 같은 .jpg 사본을 추가했습니다. Host HP100/ammo30/30·전방 구조와 Android 홈만 시각 확인한 범위이며 capture UTC나 실패 원인 확정 근거는 아닙니다. 실제 IR `resume-0124/b1-actual-independent-review01/verification.public.json`/SHA `B1BDE5AF18A3231E8C5C326FFBDF7D3A25119D5DBA0CE84068344ED7A7F3F8AF`는 운영 sourceIR와 구분합니다.

현재 B1 성공·native held 사망/리스폰·L2·route/OS·PR261 전체 수용은 미완료이며 PR261 Draft/#259 OPEN을 유지합니다. 제공 Android34/release12/source612 QA·권위원장196,2,34와 이후 일반 main/인트로·버튼 개선은 구분합니다.

외부 근거는 `D:/codex/validation/sudden-force-fps/20261031-work/input261-b1-attempt01/`의 compile/defaultfalse·game epoch·publisher·Queue·RETURN·self-close·held native·cleanup·public-after-cleanup 기록, `D:/codex/validation/sudden-force-fps/20261028-work/resumed-1448/input261-b1-r31-attempt01/l2-bridge/`의 metadata/stop/terminal 원본입니다. raw 로그·프로세스 commandline·private6 본문/해시는 공개하지 않습니다.

다음은 발사 없는 read-only query 진단으로 첫 충돌 유형을 구분하고, 정상 플레이 이동으로 열린 사선을 확보한 새 별도 실행입니다. transform/HP/private input flag를 바꾸거나 현재 실패 phase를 재사용하지 않습니다. 현재 필요한 사용자 조치와 질문은 없습니다.
