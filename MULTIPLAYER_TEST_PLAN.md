# Sudden Force FPS 멀티플레이 검증 계획

### Android 사망 중 이동·시점·재장전 및 리스폰 후 재장전 (#41, 2026-10-08)

Editor HEAD `0967351`과 기존 설치 APK 제품 소스 `6a8b203`을 구분해 한 공개방의 duration300/target20 정상 경기에서 검증했습니다. 실제 Editor OS 이동·조준·사격과 Android adb 단일 포인터 입력을 사용했고 전투 상태·타이머·callback을 주입하지 않았습니다. 아래 상태는 EditorHost에서 읽은 Android 참가자의 권한 있는 네트워크 상태이며 Android 내부 입력 gate의 직접 계측은 아닙니다.

| 항목 | 관찰 결과와 범위 |
| --- | --- |
| 사망 중 이동 | PASS(단일 입력 전후). before/after 모두 Running·HP0·IsDead=true, 실제350ms 조이스틱 입력 전후 위치 동일. RespawnRemaining2.859375→2.3125. |
| 사망 중 시점 | PASS(단일 입력 전후). before/after 모두 Running·HP0·IsDead=true, 실제 시점 입력 전후 yaw180/pitch0 동일. RespawnRemaining2.859375→2.25. |
| 사망 중 재장전 | PASS(단일 입력 전후). before/after 모두 Running·HP0·IsDead=true, 실제250ms RELOAD 입력 전후 Ammo27/Shot6/IsReloading=false/ReloadRemaining0 유지. |
| 리스폰 후 재장전 | PASS. 실제 FIRE 후 Ammo30→28 관찰, RELOAD 첫 관측에서는 마지막 발까지 반영된 Ammo27/Shot3/IsReloading=true/remaining1.8125. 이후 remaining0/Ammo30/IsReloading=false 완료까지 8개 관찰 모두 Running·alive였으며 Shot3 유지. 종료 경계와 겹치지 않았습니다. |
| 소모 탄약의 자연 리스폰 복구 | PASS(마지막 사망 사례). 사망 Ammo27/Shot6에서 자연 리스폰 HP100/Ammo30/RespawnVersion4, Shot6은 유지됐습니다. |

첫 이동 시도는 사망 화면 캡처 지연 때문에 입력이 부활 후 도달했으므로 제외했습니다. 사망하지 않은 이동 시도도 제외했습니다. 실제 입력 로그와 원자료는 유지했으며 모든 시뮬레이션 tick에서 입력이 없었음, Android 내부 gate callback 실행 또는 물리적 멀티터치 통과로 확대하지 않습니다.

최초 reloading 저장→최초 완료 저장 약1.852초는 첫 저장 때 이미1.8125초가 남아 있던 관측 구간이며 정확한 전체 재장전 시간 측정이 아닙니다. post-reload pcEpoch는 eval 요청 직전 시각으로 취득 완료 시각이 아닙니다. observer는 최소50ms polling 후 상태 변화 또는1초 heartbeat 때 저장하며 저장 간격50ms나 지연 정밀도를 보장하지 않습니다.

정상 메뉴 LEAVE 및 로비 EXIT/OK 후 Android 실행 프로세스 없음·설치 데이터 유지, Editor Win64/Play·pause·compile·import·build false/빈 clean 씬/Runner·manager·input0/match null/observer 제거/code2를 확인했습니다. QA로 변경한 font와 maximize layout만 baseline bytes로 복원한 뒤 3923파일 변화·추가·백업 해시 실패0, 기존6개 dirty와 patch 동일입니다. 최종 콘솔의 현재 groundTruth error0/warning0와 누적 수집 카운터 error7/warn4는 구분하며 전체 검증 중 오류0을 주장하지 않습니다.

근거는 외부 `D:\meee\git\sudden-force-fps-backups\20261008-android-dead-input-reload-01\qa-summary.md`, move/look/reload-check.json, post-respawn-reload.json, combat-observations.jsonl, actual-inputs.jsonl, preservation-final.json, cleanup-final.json, android-exit.json, console-status-final.json 및 PNG입니다. 독립 검토에서 원자료와 화면·제외 범위를 대조했습니다. 제품·APK·서명은 변경하지 않았습니다. 물리 멀티터치·최신 AndroidClient peerTimeout 대체 경로·저사양/실제16KB 기기·최종 고지 포함·AAB/Play 테스트 설치가 남아 #41은 유지합니다. 아래 기록은 각 이전 검증 시점의 범위입니다.

### Android 로컬 사망·리스폰·단일 터치 재개 (#41, 2026-10-08)

Editor 검증 HEAD `a8ce685`, 기존 설치 APK 제품 소스 `6a8b203`을 구분했습니다. APK 재빌드·재서명·설치 갱신 없이 실제 공개방 SFLocalDeath1008-01에서 EditorHost→AndroidClient Join/READY/START, 제품 duration300/target20의 경기 한 번만 진행했습니다. Editor의 실제 OS 키보드·마우스와 Android adb 단일 포인터 입력을 사용했으며 pose·aim·HP·타이머·전투 callback/state 주입은 없습니다. 첫 세 번의 사격 시도는 벽 충돌면에 막혀 사망 검증에서 제외했고, 실제 이동으로 우회한 뒤 명중했습니다. Editor UI/고지 변경을 기존 APK 검증 결과로 확대하지 않습니다.

| 항목 | 결과와 정확한 범위 |
| --- | --- |
| Android 자신의 피격·사망 UI | PASS. Editor 실제 FIRE의 Head100/kill→모바일 HP0/dead/Death1/DeathSequence1, Editor K1/RedScore1/Feed1. Android 화면 YOU DIED/RESPAWN IN 3s→2s/CONTROLS RETURN AFTER RESPAWN와 HEAD 킬 피드를 캡처했습니다. 이전 Android 발사→Editor 상대 사망과 별개입니다. |
| 사망 중 FIRE | PASS(단일 입력 범위). 확실히 dead인 구간의 실제250ms FIRE 전후 모바일 Ammo30/ShotSequence12 불변. HP0/RespawnRemaining2.75→1.75와 관측을 연결했습니다. 내부 Android input gate 값 직접 관측이나 모든 입력 경로 차단 통과를 뜻하지 않습니다. |
| 자연 리스폰 | PASS. 같은 PC 시계의 첫 dead→첫 alive 관측 간격3.017초입니다. observer는 최소50ms polling 후 상태 변화 또는1초 heartbeat에서만 저장하므로 실제 저장 gap은50ms가 아닙니다. 지연의 정밀도·오차 범위는 확정하지 않았으며 정확한3.017초 리스폰 보장이 아닙니다. Android HP100/Ammo30/alive/RespawnVersion1 및 overlay 해제 화면. Death1/DeathSequence1은 누적 유지, ShotSequence12도 유지됐으며 Shot0 초기화를 주장하지 않습니다. 사망 직전 Ammo30이므로 소모된 탄약의 리스폰 초기화까지 입증한 것은 아닙니다. |
| 리스폰 후 이동·look·FIRE | PASS(입력 결과 관측 제한). Android 위치(5.625,0,13.500)→(4.475,0.030,13.496), yaw180→183.380814, Ammo30→29/ShotSequence12→13을 확인했습니다. 이동 시작은 Running/remaining0.890625에서 보였고, 최종 이동·yaw·발사 변화는 첫 Finished 스냅샷에서 함께 포착했습니다. 따라서 전체 입력 변화가 별도 Running 스냅샷에 잡혔다고 주장하지 않습니다. 실제300초 종료 RedWin/TimeExpired/1:0을 확인했습니다. |
| 제외·미실행 | 리스폰 후 RELOAD는 경기 종료 경계와 겹쳐 EXCLUDED/NOT RUN입니다. 이전 일반 재장전2.011초 PASS로 이번 재개 검사를 대체하지 않습니다. 사망 중 move/look/reload gate는 NOT RUN, 물리 동시 멀티터치는 기존 사용자 질문 답변 대기/NOT RUN입니다. 추가 방·재시험 없이 종료했습니다. |
| 정리·보존 | observer 정확 제거/수집 루프 없음/실제 키·마우스 해제. Editor Win64/Play·pause·compile·import·build false/빈 clean 씬/Runner·manager·input0/match null/versionCode2. Android 실제 로비 EXIT→OK 후 재개 시 read-only pidof로 실행 프로세스 없음을 확인, 설치·데이터 유지. 콘솔을 지우지 않았고 cleanup Android 화면의 warning0/error0은 해당 화면 범위이며 전체 QA Console 오류0 주장이 아닙니다. foreground guard 거절·벽에 막힌 사격·외부 도구 인자 오류는 성공 근거에서 제외했습니다. |

새 baseline은 **3923개**입니다. QA로 변경된 동적 폰트와 maximize layout만 시작 bytes로 복원했으며, 중지 전과 재개 후 문서 편집 전 비교 모두 변화·추가·백업 해시 실패0/기존 git status·dirty patch 동일입니다. 기존 여섯 dirty 및 폰트 meta·설정·키는 보존했습니다. 이후 승인된 검증 문서3개만 별도 편집합니다.

근거: `D:\meee\git\sudden-force-fps-backups\20261008-android-local-respawn-01\qa-summary.md`, 같은 폴더 `combat-summary.json`, `combat-observations.jsonl`, `death-04-before-input.png/json`, `death-04-after-input.png/json`, `death-04-after-respawn.png/json`, `post-respawn-inputs.json/png`, `observer-cleanup.json`, `cleanup-resumed-final.json`, `android-cleanup.json`, `preservation-final.json`, `preservation-resumed-before-docs.json`. 공개 문서에는 기기 serial·계정 정보·PID 숫자를 넣지 않습니다.

전체 진행은 **약78% 추정(시간 비율 아님)**을 유지합니다. 60 FPS 목표·저사양30 FPS 허용 기준과 이전 누적623.552초 SF 측정은 그대로이며 이번에는 성능을 재측정하지 않았습니다. 남은 항목은 사망 중 기타 입력 gate·리스폰 후 RELOAD, 물리 멀티터치, AndroidClient peerTimeout 대체 경로, 실제 Android16/16KB·저사양, 전체 라이선스·고지, 서명 AAB/Play 배포·테스터 설치입니다. 아래 이전 기록의 미실행 표시는 당시 범위입니다.

### 모바일 전투·누적 10분 화면 표시 성능 (#41, 2026-10-08)

사용자 성능 기준은 **60 FPS 목표·저사양30 FPS 허용**입니다. 이번 SM-N986N 측정과 저사양 실기기 미실행 범위를 구분합니다.

검증 HEAD `75323af95e7331f2ccdc072b597fb469b8a001b9`, 설치 APK 제품 소스 `6a8b203`, SM-N986N/Android13/API33/SM8250 QTI입니다. 실제 공개방 SFCombat1008-01/-02/-03에서 EditorHost와 AndroidClient가 Join/READY/START로 경기했습니다. 제품 duration300/target20을 유지했고 상태·HP·pose·aim·타이머·전투 콜백 주입 없이 Android adb 단일 터치와 Editor 실제 키보드/마우스를 사용했습니다. 단일 10분 경기가 아닌 실제 300초 경기 세 개의 Running 구간 누적입니다.

| 항목 | 확인 결과와 범위 |
| --- | --- |
| 모바일 발사·재장전 | 탄약28→재장전 remaining2.0→30, 같은 PC 시계 첫 reload→완료 관측 간격2.011초. 최소50ms polling·상태 변화/1초 heartbeat 저장이며 실제 저장 gap은50ms 보장이 아니고 지연 정밀도는 미확정입니다. |
| 부위별 피해 | Android 발사→Editor 상대 Head100/사망, Arm18/HP82, 다음 경기 Torso25/HP75→추가 Torso25/HP50→Leg18/HP32. Host authoritative metadata로 네 부위를 확인했습니다. Head ELIMINATED100/헤드 킬 피드, Arm HIT18, Torso HIT25 화면을 캡처했습니다. Leg은 Ammo27 화면과 metadata를 확인했고 짧은 HIT18 팝업은 캡처하지 못했습니다. |
| 상대 사망·리스폰 | Head 뒤 Editor 상대 HP0/dead/Death1/DeathSequence1, 모바일 K1/BlueScore1/Feed1. 3.019초 뒤 상대 HP100/Ammo30/alive/RespawnVersion1, Shot·Hit0. Death1은 누적값으로 유지됩니다. Android 자신의 incoming death/사망 overlay/터치 gate·재개는 NOT RUN입니다. |
| 누적 성능 | Running PC 구간630.329초 중 보수적 유효 표시 간격623.552초, 로비·결과·전환247.635초 제외. SF presentation 평균 환산59.646973 FPS, median16.633490/p95 16.702083/p99 16.815521/max33.375938ms. >33.34ms 11개, >50ms0. 매 프레임60 FPS 고정을 뜻하지 않습니다. |
| 표본·관측 한계 | 1171회/명령 실패0, 유효 raw timestamp147800/unique52568/duplicate95232, 분석 표시 간격37193, timestamp reset0/감지한 unknown gap0. 127-frame ring/계획0.75초 polling/경계3초 제외/누락 프레임 보간 없음. 수집 소요 median86.587/max542.957ms, PC read-only observer의 최소50ms polling·상태 변화/1초 heartbeat 저장 및 adb 부하 포함. 실제 저장 gap은50ms가 아니며 관측 지연 정밀도는 미확정입니다. PC 시작 시각 이후 관찰값을 읽으므로 음수 phase age는 해당 수집 소요 안에서만 허용합니다. 기기와 PC 시계를 동기화했다고 주장하지 않습니다. |
| 메모리·온도 | PSS first700761/peak746087/last746023KB, RSS816784/862052/861988KB. 수집 중 Android thermal type0 CPU40.9→peak48.9→last44.3°C, thermal status 관측0. dumpsys battery32.2→peak36.4→last36.2°C. type2의0 stub값은 실제 배터리 온도로 해석하지 않습니다. PID CPU jiffies84표본17177→100729이며 CPU 비율·엔진 CPU/GPU frame time은 미관측입니다. |
| 부하 범위 | 이동/시점/일부 발사와 상대 이동·사망·리스폰을 포함하나 대체로 정지한 시간도 많습니다. 재장전은 성능 수집 전에, 세 번째 경기 Torso/Leg 명중은 성능 수집 종료 후입니다. 10분 연속 격렬한 전투·물리 동시 멀티터치·저사양30 FPS 검증을 뜻하지 않습니다. |
| 정리·보존 | 같은 Android 프로세스로 세 경기 후 로비 EXIT→OK 정상 종료, 설치·데이터 유지. 기존 Cloud104 오류4/warning0 화면 보존으로 전체 런타임 오류0을 주장하지 않습니다. observer 제거/수집 종료/Editor Win64·Play/pause/compile/import/build false/빈 clean 씬/Runner·manager·input0/match null/code2. QA 변경 폰트와 UserSettings 두 파일만 시작 bytes로 복원 후 baseline3931 변화·추가·백업 해시 실패0, 기존 git status·dirty patch 동일입니다. 이후 승인된 검증 문서3개 변경은 별도입니다. |

근거: `D:\meee\git\sudden-force-fps-backups\20261008-mobile-combat-performance-01\qa-summary.md`, 같은 폴더 `combat-summary.json`, `combat-observations.jsonl`, `performance-summary.json`, `performance-audit.json`, `performance-samples.jsonl`, 실제 PNG, `cleanup-final.json`, `android-cleanup.json`, `preservation-final.json`. 실행 전 CPU32.8°C/배터리30.6°C는 최초 도구 응답을 전사한 `prelaunch-thermal-baseline.json`에 출처를 표시했습니다. 이는 수집 중 first 값과 별개입니다.

남은 검증: 물리 동시 멀티터치, Android local incoming death/overlay/터치 차단·재개, 최신 AndroidClient의 peerTimeout 대체 경로, 실제 Android16/16KB 기기, 저사양 성능과 엔진 CPU/GPU 시간, 전체 에셋 라이선스·고지, 서명 AAB/Play 테스트 트랙 및 테스터 설치입니다. 아래 이전 기록의 미실행 표시는 당시 범위입니다.

이하 이전 검증 기록은 당시 결과와 미실행 범위를 보존합니다. 현재 완료·잔여 범위는 위 모바일 전투·성능 기록을 우선합니다.

### 최신 AndroidClient의 EditorHost Play 종료 회귀 (#41, 2026-10-08)

검증 HEAD `e3c6454`, 설치 APK의 제품 소스 `6a8b203`에서 실제 EditorHost Play 종료 후 AndroidClient가 같은 프로세스로 자동 로비 복귀하고 다음 공개방 Join/READY/START까지 통과했습니다. Editor 프로세스 자체의 OS 강제 종료·충돌이나 정상 MENU 퇴장이 아닌, 실제 Play 종료 조건의 결과입니다. 제품 상태·pose·타이머·콜백 주입은 없습니다.

Android의 안전한 로그에서 Cloud Code104→SDK RunnerShutdown/DisconnectedByPluginLogic→generation1 정리·로비 로드·Runner 생성→generation2 Completed/LobbyConnected를 확인했습니다. 동일 기기 시계의 Code104→LobbyConnected 간격은3.623초입니다. PC 요청 시각과 기기 로그 시각에 역전이 있어 서로 다른 시계의 종료→복귀 시간을 계산하지 않습니다. Android Runner의 정확한 instance ID·기존 객체 파괴·활성 개수는 관측하지 못했고, peerTimeout/ExplicitPeerFallback 마커도 없어 해당 대체 경로는 **NOT RUN**입니다.

같은 Android 앱과 새 Editor Play에서 다음 경기 Running2명/HP100/Ammo30/KD·Shot/Hit/Death/Respawn·Score/Feed0/ResultNone·Version0, duration300/target20을 확인했습니다. Code104 오류2건/warning1은 보존하며 전체 런타임 오류0을 주장하지 않습니다. 정리 후 Editor Win64/Play·pause·compile·import·build false/빈 clean 씬/Runner·manager·input0/match null/autotickfalse/code2, Android EXIT→OK 정상 종료·설치와 데이터 유지. 검증 중 동적 폰트 변경만 시작 bytes로 복원했고 baseline3931 변경·추가·백업해시실패0/기존 status·dirty patch 동일입니다.

근거: `D:\meee\git\sudden-force-fps-backups\20261008-latest-android-host-exit-01\qa-summary.md`, 같은 폴더의 `android-allowlisted-log-markers.json`, `android-process-after-recovery.json`, `next-game-reset.json`, `cleanup-final.json`, `preservation-final.json`. 아래 메뉴·이전 버전 기록과 별개의 실제 종료 경로 검증이며, 모바일 전투·물리 동시 터치·10분 성능·Play 설치 미검증은 유지합니다.

### 최신 서명 APK의 실제 MENU·정상 퇴장 회귀 (#40/#41, 2026-10-08)

HEAD `6a8b203620db0ccf8919baf95e89028a279bb550`의 peer timeout 수정과 메뉴를 포함한 새 APK를 기존 업로드 키로 빌드하고, SM-N986N/Android13/API33에 `install -r`로 설치했습니다. 아래 이전 APK·수정 Editor·표시 fixture의 결과는 당시 범위로 보존합니다. 이번에는 실제 공개방 2명/DesertHouse/300초·20점 설정으로 실행했고 HP·pose·타이머·콜백을 주입하지 않았습니다. Android 조작은 adb 단일 입력, Editor 조작은 실제 UI 버튼의 공개 클릭 경로입니다.

| 항목 | 결과 | 실제 근거와 범위 |
| --- | --- | --- |
| U01 최신 서명 APK | PASS | Build Succeeded/335.2643초/errors0/warnings2, APK 실제238,310,918B/versionCode3/min25/target36/ARMv7+ARM64. SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`. warnings2는 이번 캐시 빌드 기록이며 과거 SDK 경고71개의 해결 근거가 아닙니다. |
| U02 실제 시작·메뉴 | PASS(입력 범위) | 앱 시작 시 PGS 로그인 완료 native 안내 관찰, 닉네임→로비→두 피어 경기. MENU/RETURN TO LOBBY/LEAVE MATCH 확인 창, CANCEL→MENU와 RESUME→경기를 실제 화면으로 확인했습니다. Android Back의 경기→메뉴, 확인→메뉴, 메뉴→경기 전환을 반복 확인했습니다. SDK 인증 boolean·계정 선택·Play 설치본 OAuth 검증은 아닙니다. |
| U03 메뉴 입력 차단·시간 | PASS(제한) | MENU 버튼으로 열린 상태의 조이스틱·FIRE·시점 합성 입력에서 발사/시점 변화 없이 Ammo29/ShotSequence1/AimYaw25.60837/Pitch-1.34748161 유지. x·z 각축 약0.006m의 작은 위치 차이가 남아 완전한 위치 고정은 주장하지 않습니다. 경기 시간은 계속 감소했습니다. 실제 Editor 메뉴에서 blocked=true/move0/fire=false/reload=false/timeScale1을 읽기 전용으로 확인했습니다. 첫 Back 검사 뒤 닫힌 메뉴의 입력이 섞인 관찰은 제외했습니다. 물리 멀티터치·held-fire 경합 전체 통과 근거가 아닙니다. |
| U04 AndroidHost 정상 퇴장 | PASS | SFMenuQA1008-01에서 Android 실제 LEAVE MATCH→같은 앱 로비. EditorClient old manager-93782/Runner-93784 파괴→new-97716/-97718/liveRunner1/gen1→2/Completed/RanToCompletion/오류빈값/LobbyConnected. |
| U05 AndroidClient 정상 퇴장 | PASS | SFMenuQA1008-02에서 Android 실제 LEAVE MATCH→같은 앱 로비. EditorHost는 동일 Runner를 유지하며 Finished/RedWin/OpponentLeft/ResultVersion1/0:0 및 LEFT 로스터를 표시했습니다. 호스트의 경기가 Running으로 계속된다는 뜻이 아닙니다. 결과 RETURN→new manager-101398/Runner-101400/gen3 로비. |
| U06 재참가·초기화·역방향 호스트 퇴장 | PASS | 같은 앱/Editor 실행으로 SFMenuQA1008-02 및 -03 공개 목록 참가/READY/START/Running2명. 각 HP100/Ammo30/KD·Shot/Hit/Death/Respawn·Score/Feed0/ResultNone·Version0/300초·20점 초기화. 세 번째 경기에서 EditorHost 정상 MENU→확인→LEAVE 클릭 후 최신 AndroidClient 자동 로비 복귀, Editor fresh-105060/-105062/gen4/Runner1/Connected. Android SDK Code104 오류2건과 warning1 표시를 보존하며 전체 런타임 오류0을 주장하지 않습니다. |
| U07 정리·보존 | PASS | observer의 정확한 delegate 해제, Editor Win64/Play·pause·compile·import·build false/빈 clean 씬/Runner·NRM0/input·match null/autotickfalse. Android 로비 EXIT→OK로 실제 앱 정상 종료·프로세스 없음, 설치·데이터 유지. 메뉴의 LEAVE는 앱 종료가 아니라 로비 복귀입니다. 동적 폰트 및 EditorUserSettings의 검증 변경만 시작 bytes로 복원 후 baseline3931 변화·추가·백업해시실패0/기존git status·dirty patch 동일. 승인된 검증3문서 편집은 이후 별도 차이입니다. |

근거 root: `D:\meee\git\sudden-force-fps-backups\20261008-upload-6a8b203-01`. `apk-attempt-01/upload-validation-build.json`, `apk-verification.json`, `menu-lifecycle-observations.ndjson`, `android-host-buttonmenu-before.json`/`after.json`, `editor-host-continues-after-client-leave.json`, `second-game-reset.json`, `third-game-reset.json`, 실제 `android-*.png`, `cleanup-final.json`, `preservation-cleanup.json`에 보존했습니다. 외부 평가의 잘못된 namespace·버튼 이름/전환 중 timeout 이력은 제품 실패로 세지 않으며 모든 QA 호출에 오류가 없었다고 표현하지 않습니다.

최신 AndroidClient가 호스트 **강제 종료/peer Timeout**을 받는 방향은 NOT RUN입니다. 기존 M07 수정 재시험은 구 APK AndroidHost+수정 EditorClient 범위입니다. 물리 동시 멀티터치, 실기기 상대 피해·죽음·리스폰,10분 성능/메모리, 실제 Android16/16KB 기기 실행, 서명 AAB/Play 배포·테스터 설치는 남아 있습니다. 사용자 성능 기준은 **60 FPS 목표, 저사양 30 FPS 허용**으로 확정됐으며 측정 PASS는 아직 없습니다.

### 이전 기록: 경기 메뉴·정상 퇴장 UI의 Editor 검증 (#40, 2026-10-08)

제품 `281fbe2`: SafeArea MENU→RESUME/RETURN TO LOBBY→CANCEL/LEAVE MATCH 확인을 추가했습니다. PC Esc/Android Back 입력의 소유권은 MatchHud에 통합하고 기존 전투 입력 차단, shared Return, 결과·복귀 우선순위를 유지합니다. 실패로 완료된 Task를 성공으로 판단하지 않고 입력 차단과 재접속 안내를 유지합니다.

Editor 실제 GameView Esc와 EventSystem raycast MENU/RETURN/LEAVE 클릭, CANCEL/RESUME pointer dispatch, 표시 fixture의 Finished 우선순위와 입력 neutral, 복구 Failed 상태 fixture를 검증했습니다. 단일 Editor의 live Lobby Runner에서 확인 클릭→Returning latch·동일 shared Task→새 Lobby 생성까지 완료했습니다. MatchSnapshot/로컬 입력 상태·Failed 상태는 외부 runtime fixture이며 실제 authoritative 두 피어 경기의 정상 퇴장이나 Android 시스템 Back 검증을 대신하지 않습니다. 이슈 #40은 열어 둡니다.

근거: `D:\meee\git\sudden-force-fps-backups\20261008-match-menu-ui\HANDOFF.txt`, 같은 폴더의 `menu.png`, `confirm.png`, `input-open-qa.json`, `shared-return-qa.json`, `return-completion.json`. 최종 컴파일 실패=false/Console errors0은 마지막 clean check이며 평가 시행착오 로그를 정리한 뒤의 값입니다. 모든 QA 시도에 오류가 없었다는 뜻은 아닙니다. 기존 씬986 fileID 손실0/신규69, 기존 block 수정은 MatchHud 참조7개와 SafeArea 자식2개 추가뿐입니다. SceneTemplate 자동 migration은 시작 bytes로 복원했고, 제품4파일과 총괄 승인 문서 외의 원래 파일·dirty 변경은 보존했습니다. Editor Win64/Play 종료/빈 clean 씬/자동 tick 중지로 정리했습니다.

### 실제 Android–Editor 두 피어 검증 (2026-10-08)

**M07 수정 후 상태: PASS(수정 EditorClient 범위).** `257d30e`에서 peer 연결 끊김과 SDK 자동 종료를 구분해 복귀를 시작하도록 수정한 뒤, 기존 Android APK를 Host로 사용해 같은 force-stop을 실제 재시험했습니다. 아래 M07은 수정 전 실패 기록으로 보존합니다. 수정 AndroidClient APK 전체 회귀를 통과했다는 뜻은 아닙니다.

최신 근거: `D:\meee\git\sudden-force-fps-backups\20261008-fixed-peer-timeout-01\result.json`. 종료04:05:42.152Z→복귀 시작04:05:52.142Z→Completed52.535Z→LobbyConnected53.349Z, 약11.20초. old manager -66122/Runner -66124 파괴, new -70034/-70036 한 개, generation1→2, 공유 Task 완료/오류 빈 값, automaticShutdown=false/ShutdownIssued=true 확인. 같은 Editor Play에서 Android 재실행·새 공개방 목록 Join/READY/START로 두 피어 Running을 재확인했고 HP100/Ammo30/KD·Shot/Hit/Death/Respawn·Score/Feed0/ResultNone·Version0으로 초기화됐습니다. `editor-fresh-lobby.png`, `recovery-transitions.json`, `next-game-reset.json`에 실제 근거를 보존했습니다. observer 해제/Editor Play 종료·Win64·빈 clean 씬·Runner0·autotickfalse, 검증 앱만 종료/데이터 유지. 재시험 baseline3931 변경·추가·해시 실패0/기존 dirty 동일. 초기에 로비 연결 완료 전 닉네임 조회의 외부 평가 오류1건은 시험에서 제외하고 전체 Console 오류0을 주장하지 않습니다.

수정 단일 세션 검증 근거: `D:\meee\git\sudden-force-fps-validation\20261008-peer-timeout-01\HANDOFF.txt`. 컴파일 오류0, 정상 명시적 Return의 공유 Task·old Runner 파괴·새 Runner1·로비 연결 확인. synthetic peer callback은 두 프레임 경계 이후 종료→새 로비 연결을 확인했으며 실제 통신 끊김과 구분합니다. SDK Update 안에서 종료를 예약한 외부 fixture에서는 SDK 자체 종료에 합류하고 제품 fallback 요청이 없었음을 확인했습니다. 복귀 대기 중 Play 종료는 취소 상태/추가 종료 요청 없음(종료 순간 pending Task의 완료는 미확인), 프레임을 일시정지한 시험은 약30.05초에 Failed·안내 표시·새 Runner 생성 차단을 확인했습니다. 무효 외부 helper와 의도한 timeout/Pipeline 진단 오류도 보존하여 전체 Console 오류0으로 표현하지 않습니다. 비범위 tracked3895 해시 변화0, 기존 dirty 보존, Editor Win64/빈 clean 씬/Play 종료/자동 tick 중지로 반환했습니다.

제품 소스 `55024a5`, QA HEAD `420ee00`(그 사이 문서 변경만), 기존 업로드 키 서명 APK 사용. SM-N986N/Android13/API33, ClientServer/kr/공개방2명/DesertHouse, 제품 경기 시간300초·목표20점 그대로 실행했습니다. HP·점수·타이머·pose 주입이나 강제 콜백을 성공 근거로 사용하지 않았습니다.

| 항목 | 결과 | 실제 관측과 한계 |
| --- | --- | --- |
| M01 설치·로비 | PASS | 서명 검증 APK 설치·Activity 실행 후 실제 Android 로그인 경로에서 Photon kr 로비/목록0 도착. 로그인 코드는 인증 상태 또는 성공 응답에서만 진입하지만 SDK 인증 boolean 자체를 별도 조회하지 않았습니다. 계정 선택 창이나 계정 정보는 조작·기록하지 않았습니다. |
| M02 공개방·경기 | PASS | EditorHost 방을 Android 실제 목록에서 선택, 반대 팀/READY/START로 Running2명. 양쪽 HP100/Ammo30/KD0/점수0, 실제 모바일 HUD 확인. |
| M03 모바일 단일 입력 | PASS(제한) | adb 단일 터치 제스처의 이동·시점이 Editor에 복제, FIRE Ammo29/ShotSequence1, RELOAD IsReloading→Ammo30, roster2명 표시. 물리적인 동시 멀티터치 통과 근거는 아닙니다. |
| M04 경기 종료·다음 방 | PASS | 실제 300초 경과 후 Draw/TimeExpired/0:0/ResultVersion1, Android 결과 화면과 실제 RETURN 버튼. 양쪽 fresh Lobby 후 다음 방에서 HP100/Ammo30/KD·shot/hit/death/respawn·score/feed/result0 초기화 확인. |
| M05 EditorHost 종료 | PASS | 실제 Play 종료 후 Android는 재실행 없이 같은 프로세스로 fresh Lobby 자동 복귀. SDK Code104 등 예상 연결 종료 오류 로그는 보존하며 전체 오류0으로 주장하지 않습니다. |
| M06 AndroidHost 생성 | PASS | 실제 방 이름·맵·Confirm UI로 생성, Editor 발견/Join/READY, Android START 후 Editor server=false/Running2명 확인. 키보드 닫힘 직후 첫 맵 터치는 선택되지 않아 화면 재관측 후 다시 선택했습니다. |
| M07 AndroidHost 프로세스 소실 | FAIL | 허용된 검증 앱만 adb force-stop, 프로세스 소멸 확인. 1분 이상 Editor DesertHouse/sessionGame 유지, LobbyDisconnected/LastLobbyError Timeout, old manager -40286/Runner -40288 생존, remaining274.65625 고정. IsRunning=true/_shutdownObserved=false/_disconnectShutdownPending=true. OnDisconnectedFromServer 이후 OnShutdown 관측 없이 복귀 미시작. 강제 콜백·수동 복귀를 PASS로 세지 않았습니다. |
| M08 고지·정리 | PASS(범위) | Android 고지 열기/닫기 및 Liberation Sans OFL 표시. Editor Play 종료/Win64/빈 clean 씬/Runner0/autotickfalse, 관측 delegate 해제. baseline3930 변경·추가·해시 실패0 및 기존 dirty patch 동일. 앱 설치·데이터는 유지하고 force-stopped 상태입니다. |

근거: `D:\meee\git\sudden-force-fps-backups\20261008-mobile2peer-01\qa-result.md`, 같은 폴더의 `androidhost-observations.ndjson`, `androidhost-failure-details.json`, `preservation-final.json` 및 실제 화면 캡처.

당시 구 APK의 정상 Android 게임 종료는 Back 입력에도 종료 UI가 없어서 NOT RUN이었습니다. 최신 메뉴·로비 종료 결과는 상단 U02–U07을 따릅니다. 실제 상대 피해·죽음·킬피드·리스폰, 물리 동시 멀티터치, 10분 성능/메모리, 실제 Android16/16KB 기기 실행은 미검증으로 유지합니다. UI 저장 닉네임의 마지막 U+200B도 후속 항목입니다.

## 현재 검증 상태와 후속 단계

**최신 상태(2026-10-08): PC 양방향 종료 복귀 P01–P06과 수정 EditorClient의 실제 AndroidHost 강제 종료 복귀·후속 재참가는 통과했습니다. M07 수정 전 실패 기록과 문서 위의 수정 후 실제 결과를 구분합니다. 최신 APK의 정상 메뉴 퇴장은 상단 U01–U07에서 통과했습니다. 최신 AndroidClient peerTimeout 대체 경로·물리 멀티터치·모바일 자신의 사망 UI는 남아 있습니다. 모바일 단일 입력 상대 전투와 누적 SF 성능 결과는 상단 최신 기록을 따릅니다.**

**현재 상태: C5 새 빌드의 실제 두피어 경기에서 20킬 TargetScore 종료, 실제300초 높은 점수 승리·Draw, 상대 정상 종료의 OpponentLeft 및 퇴장자 KD/이름 보존을 확인했습니다. 양쪽 결과·Finished 상태 고정·정상 로비 복귀·new Runner 초기화와 PC 짧은 Tab 입력을 아래 범위에서 검증했습니다. 런타임 pose/baseaim 보조와 사용자 추가 입력을 명시하고 순수 계산/아트 상태 주입 진단과 구분합니다. 제품 파일은 원상 복원했으며 문서만 변경합니다. Waiting blocked/취소·동tick 경합·진행중 reload 종료·실제held 입력·3peer/권한위조·Android·저FPS/재시뮬레이션 등은 미검증으로 유지합니다. C1–C4의 아래 기록과 미검증 항목은 각 당시 실행 범위입니다.**

### 접속 취소와 실제 두 피어 Host 종료 회귀 (2026-10-07)

**추가 상태: 최신 NRM 취소 수정에서 실제 Host 종료 후 Client의 자동 fresh Lobby 복귀가 양방향 모두 FAIL/미완료입니다. 정상 공개 복귀 API를 호출한 Host 자신의 새 로비 복귀와 Player 재실행 후 접속 성공은 별도 결과입니다. C5 경기 판정의 기존 통과를 이 자동 복귀 문제의 해결 근거로 확대하지 않습니다.**

기준 HEAD `32d65c28624a55151ba0fee23534b47002e88418`. 네트워크 담당 인계 근거는 `D:\meee\git\sudden-force-fps-validation\20261007-lobby-cancellation-01\HANDOFF.txt`, QA 실제 두 피어 근거는 `D:\meee\git\sudden-force-fps-validation\20261007-host-exit-01`입니다. 인계받은 검증과 이번 직접 실행을 구분합니다.

| 항목 | 결과·근거와 제한 |
| --- | --- |
| N01 접속 취소 3회 — 담당 인계 | Connected→정상 Return→새 Runner Connecting/JoinTask 미완료 중 Play 종료 3회. manager -6974/-9720/-12466의 종료 전 token=false/task WaitingForActivation, ExitingPlayMode에서 token=true/stopping=true/retired=true, 종료 후 static null/runners0. 다음 Play fresh join 완료. SDK OperationCanceled 오류 3건(seq50/64/79)을 보존하며 Console 오류0으로 서술하지 않습니다. captured 오류 3건에 RegionHandler/DontDestroyOnLoad outsidePlay0. domain reload로 소실된 첫 callback은 시험 제외. 이번 QA가 재실행한 3회가 아닙니다. |
| N02 단일 Host·Return 중 종료 — 담당 인계 | 실제 단일 Host Room -14570/StartGameTask 완료→정상 Return의 새 manager -15654/runner -15656 Connected 및 oldRoomPlayers0. Return pending stop은 첫 yield 경계 포착이며 SDK Shutdown-await 중 종료를 입증하지 않음. 후속 fresh join -18234 완료. region minimum ping 자체·Shutdown-await stop·독립 before-SDK-yield 반복·genuine failure·CTS dispose 완료·Android/OSquit는 NOT RUN 유지. |
| N03 새 Win64 빌드·게시 | Unity6000.3.25f1/PID55332, 최초 port7802 이후7800, 모든 명령에 project-path 명시. compile 중 첫 build 거절(NoTarget/Unknown/0bytes)은 성공 제외하고 원본 Console 오류 유지. 컴파일 완료 뒤 새 Development build Succeeded/398,735,092 bytes/258,746ms/오류0/경고5. stage player와 fixed active-player 전체380파일 SHA 차이0, 기존366파일은 active-player-before-backup으로 보존·차이0. build-all-files.json SHA256 978062BA2B68896C34F4D843983F30C7DBB22740D5ABF991255B606B26798A94. 임시 URP_COMPATIBILITY_MODE는 복원했습니다. |
| N04 최초 예상 밖 Client 복귀 — 의도한 시험 제외 | Editor Host -29286/runner -29288 공개방→Player PID72448 native Join/Ready→Host 공개 Start. frame5064 UTC09:06:22.720 Game/players2 뒤 frame5066 players1. 의도한 Host Return 전에 Player가 shutdown Ok/DisconnectByClientLogic→LobbyConnected로 복귀하고 Host는 Finished0–0/Client disconnected. 원인·호출 출처 미확정이며 Host 씬 로드의 약12초 공백을 timeout 원인으로 확정하지 않습니다. first-start-unexpected-client-lobby.json, Player-01.log, lifecycle.jsonl. 별도 정상 Host Return으로 정리한 뒤 새 방 재시도 1회를 수행했습니다. |
| N05 Editor Host 공개 Return — Host PASS, Client 자동 복귀 FAIL | 재시도 Host -32810/runner -32812의 실제 Running/양쪽 연결, score/KD/feed/result/respawn 세대0, HP100Ammo30, local Camera/Listener1·remote0 확인. Player 사용자 추가 사격1/Ammo29도 기록하며 자동화 사격으로 확대하지 않음. Host 공개 ReturnToLobbyAsync에서 old callback1→0/retired true→old manager·runner 파괴, task RanToCompletion, 새 -36312/runner -36314 LobbyConnected/join 완료/player0/events0/static/input null. Player Client는 실제 Code104/ServerLogic/OnShutdown DisconnectedByPluginLogic 뒤 게임 씬·비어 있는 HUD가 잔존했고, 창 복원 뒤에도 fresh Lobby 로그 없음. native Running 퇴장 UI는 없으므로 UI 퇴장 PASS가 아닙니다. public-return-02.jsonl, editorhost-before-intended-return.json, editorhost-return-fresh-lobby.json, client-after-resume-stale-game.png, Player-01.log. |
| N06 역할 교환·Player Host native 종료 — Client 자동 복귀 FAIL | 기존 Player native Alt+F4 정상 종료 후 같은 fixed exe PID90380 재실행/로비 연결. Player Host의 실제 UI 공개방 생성·Start와 Editor Client 공개 Join/Ready로 Running, 양쪽 score/KD/feed/result/respawn0, HP100Ammo30 확인. Player Host native 사격1/Ammo29가 Editor replica에 도착(hit0; 피해·lethal 회귀로 확대하지 않음). Player Host native Alt+F4/gone=true 뒤 Editor Client는 Code104 오류2건/OnShutdown DisconnectedByPluginLogic. frame64117까지 old owner 유지, frame64118 UTC09:22:56.879에 old manager -36312/runner -36314 파괴·stopping/retired true·callback 없음·returnTask RanToCompletion·currentInstance0·game scene 잔존·player0/input·match null. 새 owner/새 Lobby는 생성되지 않음. observer는 Editor update sample이므로 동일 frame 내부의 OnShutdown/OnDestroy/await 세부 순서는 직접 입증하지 않습니다. 태스크가 계속 기다리는 deadlock은 이 완료 관측과 구분합니다. editorclient-shutdown-frames.jsonl, editorclient-shutdown-transitions.json, editorclient-after-native-host-close.json, console-editorclient-hostclose.json, Player-close-02.json. |
| N07 SDK 계약과 재실행 접속 | 로드된 SDK Shutdown reflection에서 destroyGameObject 기본 true/forceShutdownProcedure 기본 false 확인(sdk-shutdown-defaults.json). 담당의 별도 Cecil/IL 읽기에서는 CloudServices.OnDisconnected가 Shutdown(true,reason,true)를 명시 호출한다는 인계가 있으나, 이는 실제 frame 내부 파괴 순서의 직접 관측과 구분합니다. 두 피어 모두 자동 복귀 실패, Editor의 old owner 소실/완료 task/새 owner 없음이 원인 분석 근거이며 제품 수정은 하지 않았습니다. Host 종료 후 세 번째 Player PID85356 재실행의 LobbyConnected/목록 수신도 확인했으나 자동 복귀 성공을 대체하지 않음(Player-03.log, player-relaunch-03-connected.png). |
| N08 정리·보존 | Player PID72448/90380/85356 모두 native Alt+F4 후 종료 확인. 외부 QA update observer2개 제거, Play=false/paused=false/빈 single clean scene/root0/Win64/autotick=false, manager·Match·presentation·local·input null/runners0/events0/QA callbacks[]. ProjectSettings 전체 byte 및 define 복원, 빌드가 자동 저장한 URP2개를 시작 dirty 내용·줄바꿈까지 복원. Assets/ProjectSettings/Packages3889개 시작·종료 SHA 차이0/누락0, active380파일 최종 차이0. recompile up_to_date(강제 전체 compile 아님). 최종 Console 현재 errors3/warnings2, cached errors10/warnings64이며 inherited SDK cancel3·compile중 build거절1·Pipeline timeout4·정상 종료 Code104 error2를 구분·원본 유지. 현재 outsidePlay 해결을 전체 미검증 범위의 해결로 확대하지 않음. cleanup-final.json, preservation-final.json, active-final-preservation.json, console-final-errors.json, console-status-final.json, compile-final.json. stage/commit은 총괄에 인계합니다. |

현재 FAIL/미완료: 양방향 실제 Host 종료 후 Client 자동 fresh Lobby. NOT RUN: 실패 수정 후 동일 경로 재검증, 실제 SDK Shutdown-await 경계 Playstop, CTS dispose completion, genuine network failure·Android/OSquit, 역할 교환 후 lethal/KD/feed 이벤트·실제 held movement/fire 전수. callback/HP/score/timer 주입이나 제품 patch는 사용하지 않았습니다.

### C5 서버 점수·시간·승패 실제 검증 (2026-10-07)

Refs #33, #19, #35, #1. 기준 HEAD `d88703307df2856cad0a331b15cd35777bfe75d5` (C5 코드 `70460a29040eb5b9cffbefbcb847ee59dcac2d8f` 포함). 증거 루트 `D:\meee\git\sudden-force-fps-validation\20261007-c5-actual-01`. 새 Windows64 Development 빌드 성공/오류0/경고2, 268,762,191 bytes, 32.7175866초. stage `player` 원본 및 기존 고정 C4 Player의 `active-player-c4-backup` 보존, 고정 경로 `D:\meee\git\sudden-force-fps-validation\active-player\SuddenForceFPS.Validation.exe`에 새366파일 게시/이름·SHA256 차이0. manifest SHA256 `7F925A26F70011BF9BF52B19E58A16586BE111C4CD9D0E22E85C4AF6EA3CE26B`. Player PID73500, 실제 Editor Host/Standalone Client의 kr 공개방 참가·Ready·정상 Start를 사용했습니다. 생산 상수20점/300초는 변경하지 않았습니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| C5-01 Waiting/Running·새 경기 | 실제 최초 Waiting의 점수0/timer없음, 이후 양쪽 game player 생성과 Running 관측. 빠른 전환 exact tick은 매번 직접 캡처하지 못해 TargetTick−19200(64Hz·300초 계약)을 derived start로 구분합니다. blocked initial spawn 대기/Waiting 중 필수 팀 이탈 취소는 미실행입니다. |
| C5-02 실제20킬 TargetScore | Runner-303738에서 runtime shooter pose/baseaim 보조와 실제 클릭·사용자 추가 입력으로 정상 사격→피해→사망20회→점수20. HP/Score/timer setter·synthetic kill record·SDK input은 사용하지 않았습니다. tick17892에 Finished/RedWin/TargetScore/ResultVersion1, 양쪽 VICTORY/DEFEAT와 RED20 BLUE0, HostKD20/0·Client0/20 일치. 총 HostShot35에는 miss/bodyshot·사용자 추가 입력/재장전이 포함돼20클릭=20발로 확대하지 않습니다. 15번째 lethal 당시 남은HP50 clamp damage50도 관측했습니다. |
| C5-03 실제 killfeed ring·UI | 초기 bind baseline0 뒤 actual KillObserved20개/sequence1..20/unique20/gap0. 최종 retained16개 seq5..20 순서 일치. Running HUD는 최신5개를 표시하고 Finished에서는 숨깁니다. actual Head 피해/HEAD 표시 및 nickname 양쪽 일치. 실제 저FPS/gap·동tick 빠른kill·rollback은 미실행입니다. `target19-editor.png`, `target20-summary.json`, `match-events.jsonl`, `target20-editor.png`, `target20-player.png`. |
| C5-04 Finished freeze·respawn | Host native click/R/w, Client native w/R 뒤 HP·Ammo·KD·score·deathseq·respawnversion/result 유지. Client click 시도는 user-input latch로 거절되어 실행으로 세지 않았고 재관측 후 계속했습니다. 마지막 ClientHP0/Death20/version19, respawnTarget18084 이후도 부활없음/RespawnsEnabled=false. 초기 Render sample Host z13.4974432→13.49744(~0.0000032), tick17956부터 pose 고정이므로 exact endTick transform bitwise freeze라고 서술하지 않습니다. 최종 pending timer 값이 남아 있어도 actual respawn은 차단되었습니다. 종료 때 진행 중 reload/동tick damage 경합은 미검증입니다. |
| C5-05 실제300초 높은 점수 | Runner-307336, 실제1킬 후 RED1 BLUE0. Waiting UTC07:21:00.460/첫Running07:21:00.510→Finished07:26:00.167, planned TargetTick20855=actual first Finished tick20855, RedWin/TimeExpired/ResultVersion1. 양쪽 VICTORY/DEFEAT/TIME EXPIRED·KD1/0·0/1 일치. 만료직전 nativeR는 발송했으나 accepted reload 관측이 없으므로 진행 중 reload 중단 PASS로 대체하지 않습니다. `timehigh-newmatch.json`, `timehigh-summary.json`, `timehigh-final.json`, `timehigh-editor.png`, `timehigh-player.png`. |
| C5-06 실제300초 Draw | Runner-310836, Waiting UTC07:27:24.334/첫Running07:27:24.384→Finished07:32:24.038, planned TargetTick21083=actual first Finished21083. Draw/TimeExpired/ResultVersion1/RED0 BLUE0/서버KD양쪽0, 양쪽 DRAW/TIME EXPIRED 결과 확인. 원본 `draw-editor.png`와 `draw-player.png`에서 roster 이름·KD0/0이 모두 패널 안에 표시됩니다. Editor 이미지의 행 KD는 헤더보다 오른쪽 정렬되어 있으나 글자 잘림은 확인되지 않았습니다. 앞선 클리핑 관측은 정정합니다. 경기 중 사용자 비치명 사격도 포함되나 score/KD0 유지, 상태 주입 없음. `draw-initial.json`, `draw-summary.json`, `draw-final.json`, `draw-editor.png`, `draw-player.png`. |
| C5-07 실제 상대퇴장·퇴장 roster | Runner-314326에서 실제1킬/리스폰 후 Client PID73500의 정확한 fixed exe 경로를 확인하고 정상 CloseMainWindow/WaitForExit true로 종료. tick3505/UTC07:34:02.334에 RedWin/OpponentLeft/ResultVersion1/RED1 BLUE0. 남은 Host VICTORY/OPPONENT LEFT 화면, 퇴장 C5Client의 Connected=false/name/KD0/1/LEFT 표시 보존, HostKD1/0. `opponent-left-before.json`, `Player-close.json`, `opponent-left-summary.json`, `opponent-left-final.json`, `opponent-left-editor.png`. Host 종료·3peer 중간 이탈/동시퇴장은 미실행입니다. |
| C5-08 정상 결과 복귀·new Runner | Client native RETURN TO LOBBY 및 Host MatchHudView.ReturnToLobby 공개 API. 로비 manager-307334/runner-307336 notretired, MatchState/MatchPresentation static null/player0/inputnull. 새 공개방/Ready/Start에서 score0/KD0/feedseq0/ResultVersion0/HP100Ammo30/epoch0, 새actualkill feed1; KillObserved HUD+QA각1, 옛20개 replay 없음. 두 번째 결과도 정상 복귀 후 runner-310836 새 경기0상태 확인. Draw 결과도 Client native 버튼/Host 공개 API로 정상 복귀했고 manager 새 Runner-314326 및 static null을 확인했습니다. `draw-subscriptions.json`에서 서버 KillConfirmed Match1/HUD Changed1/feed HUD+QA각1/static Bound·Unbound HUD각1. failure UI fixture는 아트 인계 진단이며 이번 actual PASS가 아닙니다. |
| C5-09 PC Tab·C2–C4 회귀 범위 | native 짧은 Tab keypress→scoreboard true/neutral snapshot→false/aim복원 런타임 관측. panel true를 읽을 때 key는 release후false였으며 지연PNG는 post-close 화면이라 점수판 시각 PASS로 사용하지 않습니다. 실제held 중 시각/이동·사격 조합은 미실행. C5 running의 actualHead 피해/탄약소비·KD·실제3초respawn 및 local hit/HUD/FX는 관측했으나 C4 부위·blocked·역할교환 전수 재검증은 아닙니다. |
| C5-10 정리·소스 비교 | 마지막 Host native 결과 버튼으로 정상 복귀. 로비 manager-317820/runner-317822 Connected/joinTask completed/notfaulted/operation pendingfalse, MatchState/MatchPresentation static null·Bound/Unbound null·player/presentation/events0·LocalInstance/InputInstance null·fixture[]. QA observers3개 해제 후 Play=false/빈 single scene dirtyfalse/root0/events0/QA callbacks[]/autotick disabled/StandaloneWindows64. 원 define live+disk 및 ProjectSettings 전체 텍스트·SHA256 backup 동일. Assets/ProjectSettings/Packages 전체3889파일 시작/종료 SHA256 차이0, active366파일 차이0, 기존 drift 보존. 최종 recompile up_to_date/failedfalse/errors[]/warnings[], 현재 Console errors0/warnings0, Player log Exception/Error/Assert 검색0. 반복 pending join 중 Playstop race 재현시험은 하지 않아 C4 과거race 해결 주장 없음. `lifecycle-final-lobby.json`, `prestop-lobby-static.json`, `cleanup-final.json`, `ProjectSettings-final-restore.json`, `preservation-final.json`, `compile-final.json`, `console-status-final.json`. |

원격 Player의 실제 복제 CombatState 로그에서도 HostKD20/0·Ammo11·Shot35·Hit22가 tick17895에 도착하고, ClientHP0/KD0/20/version19가 respawnTarget18084 이후 pending으로 유지됩니다. 이는 server 종료 tick17892와 replica 도착 시각을 구분한 근거입니다(`target20-player-replica.log`).

현재 미실행 범위: Waiting blocked all-spawn/필수팀 이탈 취소, 실제3peer/동시kill·respawn/권한위조, exact endTick damage 경합 및 진행중 reload 중단, 실제16명 결과 roster, 실제 late bind gap/저FPS·packet loss·rollback/resim, PC 실제held menu/physical-held fire release 경계, Android touch·OS pause, slope/air/physics corpse. 순수27검사/아트 reflection fixture와 이번 실제 match timeline을 구분합니다. C4 초기 Play 준비/종료 중 Fusion JoinSessionLobby outsidePlay race는 미해결 과거 관측으로 유지하고 현재 Console0만으로 해결을 주장하지 않습니다.

### C4 사망·KD·안전 리스폰 실제 검증 (2026-10-07)

Refs #33, #19, #35, #1. 기준 HEAD `78150f3b8f4d4dc91b6e9f85fb5cc4e609fb93fd`. 증거 루트 `D:\meee\git\sudden-force-fps-validation\20261007-c4-actual-01`. RespawnVersion 입력 struct와 최신 애니메이션/UI를 포함한 Windows64 Development 신규 빌드 성공/오류0/경고2, 268,706,104 bytes, 27.7754825초. stage `player` 보존, 이전 고정 경로 C3 Player는 `active-player-c3-backup`으로 백업한 뒤 `D:\meee\git\sudden-force-fps-validation\active-player\SuddenForceFPS.Validation.exe`에 게시했습니다. 전체366개 파일의 이름·SHA256 차이0, manifest SHA256 `03C82ADE8F3F77FA7111AFBC8DB53C8041CDBB5E21E4BA8CFBD049C522EC6354`. 실제 Player PID70428, kr 공개방01의 Editor Host/Player Client Runner-268350, 공개방02의 Player Host/Editor Client Runner-272634를 사용했습니다. 두 번째 UI 입력 방 이름에는 기존 TMP U+200B가 끝에 포함돼 실제 목록 이름을 그대로 참가했습니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| C4-01 몸통/머리/사지 lethal | 위치·base aim runtime fixture 후 Host 실제 몸통4발25씩: Client HP100→75→50→25→0, Host K1/Client D1/DeathSequence1. 새 life에서 Host 실제 Head100으로 K2/D2. 이후 실제 왼팔6발18×5+잔여10으로 HP100→82→64→46→28→10→0, Host K3/Client D3/DeathSequence3. 각 lethal LastHitKilled=true, 비치명타 false; 실제 ELIMINATED10 화면을 `editor-hit--270012-11.png`에서 확인했습니다. 사지 양쪽 모든 부위 반복 전수는 아닙니다. |
| C4-02 양쪽 공격 역할/KD once | 첫 경기 Player Client 실제 머리 사격2회가 각 life의 Editor Host HP100→0을 적용, 최종 Host KD3/2·Client2/3. 죽은 Client 방향 후속 실제3클릭은 Host Shot6→9/탄약24→21, Hit5/K2/Client D2 유지. Client 사망 중 실제 클릭+R도 HP0/탄약30/Shot0/reload0 유지. 실제 lethal 뒤 관측한 KillConfirmed1개에 공개 NotifyKillConfirmed로 같은 기록20회 재전달한 별도 진단은 이벤트1→1 유지했습니다. 첫 두 kill을 이 callback이 관측했다고 확대하지 않습니다. |
| C4-03 3초/identity/life 초기화 | 실제64Hz에서 Client death5043→respawn5235, 사지26346→26538, Host16742→16934 각각192tick. 대상 이전 version은 유지되고 복귀 때만 증가, HP100/탄약30/KD 보존, 같은 NetworkId1030/1031 유지. 첫 respawn 변화 sample의 transform은 Render 순서상 이전 pose일 수 있어 그 sample을 최종 teleport pose로 판정하지 않습니다. 후속 실제 화면·snapshot에서 팀 spawn/Red yaw0·Blue yaw180/pitch0, Idle_Aiming 복귀를 확인했습니다. |
| C4-04 blocked/pending/retry | actual lethal 직후 설치한 runtime occupancy fixture로 모든 해당 팀 spawn을 막았습니다. Client death10677/목표10869 이후 HP0·pending 유지, 장애 제거15013 후15030에만 복귀. Host death30203/목표30395도 제거33029 후33035에 복귀. Client retry target260개 간격259개 모두16tick, Host165개 간격164개 모두16tick. 강제 teleport/추가 KD 없음, Player와 Editor 실제 WAITING FOR A SAFE SPAWN 화면 확인. 정상 맵의 모든 spawn이 원래 막혀 있다는 의미가 아닙니다. |
| C4-05 collision/query/시체 표현 | 실제 dead state에서 HitboxRootActive=false/CC.detectCollisions=false/velocity0, local 무기 숨김·Camera1/Listener1 유지, remote Camera0/Listener0. 실제 dead head 방향12개 SDK ray는 해당 dead hitbox를 반환하지 않고 world/BG를 반환했습니다. helper는 자기CC/deadCC 제외true, 다른 aliveCC 제외false, blocked 후보4개 IsClear=false. 다른 corpse 뒤 실제 적 관통/아군 첫 hit/실제 충돌 이동 전수는 미실행입니다. |
| C4-06 root/ground/Idle | 실제 원격 사망을 blocked 상태로 유지한 두 sample에서 root(-5.625,0,13.5)/velocity0 불변, animation normalizedTime4.8585→9.1114로 재시작 없음. 왼손Y.0799997/발Y.13815 등 본 pose도 동일, root 기준+.08 최소 하강 표현을 확인했습니다. 복귀 후 양쪽 Idle_Aiming hash-1603497394/rootMotion=false/팀 spawn. 경사·공중 사망·물리 settling/ragdoll 합격으로 확대하지 않습니다. |
| C4-07 epoch 거절/타이머 | focused181poll의 own SDK 입력: serverVersion1에 old0/future2의 Move(1,1)/yaw90/pitch15/Fire+Reload 전달. Host 위치·yaw0/pitch0·Shot10 유지, 장전19770→19898/128tick은 무효 epoch 중에도 완료해 탄약30/index0. 유효version1 후속Fire는 Shot11/탄약29 정상 수락. 악성 remote 권한 위조·packet loss·native 입력 시험이 아닙니다. |
| C4-08 장전 사망 취소 | 별도 SDK cached own-input cycle로 장전을 진행하고 Player 실제 치명타를 적용했습니다. death 직전 tick30203의 이전 poll은 HP100/탄약29/reloadEnd30296, 다음 poll30204는 HP0/reload0; 실제 death state tick30203과 구분합니다. 사망 동안 preview ammo0/recoil0/index0, 복귀HP100/탄약30. 이 SDK는 창 포커스와 무관하게 유효 cached own payload를 전달하므로 정상 focus-loss 동작 증거가 아닙니다. 앞선 native Host R16503→16631은 lethal16742 전에 완료됐으므로 장전 중 사망 시험에서 제외합니다. |
| C4-09 HUD/터치/focus | 실제 local death/pending HUD와 K/D 표시, 무기 숨김/복귀를 확인했습니다. 실제 dead actor HUD disable/reenable에서 초기 dead snapshot을 다시 적용했습니다. runtime 강제 preview 버튼의 synthetic Down은 dead 상태에서 소유하지 못함. 실제 respawn 뒤 preview는 capture 초기false→freshDown true→oldUp 유지→matchingUp false, desktop 복원 후 비활성. 이 preview는 당시 비포커스여서 Snapshot bits0이며 실제 Fire 소비/Android PASS가 아닙니다. 실제 Editor Client focus true→false에서 input bits4→0/HasAim=false, private base yaw180 유지. 물리 Fire 지속 hold 후 release/freshDown 경계는 NOT RUN입니다. |
| C4-10 new Runner/역할 교환/C2C3 회귀 | 정상 Exit→Lobby player/presentation0·LocalInstance/InputInstance null. Player Host/Editor Client 새 경기 HP100/탄약30/KD0/deadfalse/version0/deathseq0/FX0·과거 overlay 없음. Editor Client 실제8클릭 Shot1~8/탄약30→22, 실제R128tick 후30/index0; Standalone Host 실제 클릭Shot1/탄약29/벽impact를 확인했습니다. 역할 교환 후 lethal/respawn 반복은 미실행입니다. native+SDK 전체30 ShotObserved에서 중복0/gap0, local confirmed hit11개/killed3개. C3 FX 음질·예측 지연·저FPS 전수 회귀는 아닙니다. |
| C4-11 정리/소스 | Player 정상 CloseMainWindow/WaitForExit true·종료 확인. 마지막 Lobby manager-276382/runner-276384/player·presentation·events0/LocalInstance·InputInstance null/fixture[]; main observer·kill observer 해제, SDK callback/component 제거. Editor Play=false/빈 single scene dirty=false/roots0/events0/QA callbacks[]/autotick=false/Win64. 원 define live+disk와 ProjectSettings 전체 텍스트 시작 backup 동일. prebuild 전체3873파일의 종료 SHA256 변경0, 최종 recompile up_to_date/failed=false/errors[]/warnings[], 현재 Console errors0/warnings0. |

fixture는 공개 SDK Teleport/PlayerInputSource.AddLookDelta·SeedAim으로 runtime 위치·조준만 준비하고 lethal은 실제 네이티브 클릭으로 적용했습니다. HP/Ammo/Team/Authority 직접 주입이나 scene/prefab 저장은 하지 않았습니다. blocked/saturation primitive는 runtime-only로 만들고 전부 제거했습니다. 별도 helper의 동일 tick 단일 후보 선택은 true→false, 다음 tick true;65개 tiny collider 중 NonAlloc64 포화에서 IsClear=false. 이는 실제 동시 리스폰 합격이 아니며 random state도 복원했습니다. SDK 입력 epoch 및 장전 진단은 정상 own input boundary에 임시 NetworkEvents를 연결한 것으로 실제 PC hold와 구분합니다.

미실행: 물리 held-fire death→respawn→release gate, Android touch/held-fire/OS pause, 실제3인 아군·권한 위조·동시 리스폰, 역할 교환 후 lethal 반복, 경사/공중 시체 및 physics ground, packet loss/실제 rollback/resimulation/저FPS, C5 팀 score/승패. Kill/Death 및 ELIMINATED 표시가 C5 match outcome 검증 근거는 아닙니다. 원본 clip/Controller를 포함한 전체 source hash가 동일합니다.

주요 증거: `active-publication.json`, `build-all-files.json`, `states.jsonl`, `events.jsonl`, `kills.jsonl`, `summary-final.json`, `retry-summary.json`, `player-visible.log`, `epoch-sdk.json`, `reload-death-sdk.json`, `pending-ground-01.json`/`02`, `dead-head-query.json`, `selector-dead-diagnostic.json`, `selector-reservation-saturation.json`, `dead-hud-pointer-diagnostic.json`, `respawn-touch-diagnostic.json`, `focus-before.json`/`after`, `game1-final.json`, `game2-initial.json`, `game2-regression.json`. 종료/원복은 `lifecycle-final-lobby.json`, `player-normal-close.json`, `cleanup-final.json`, `source-verification-final.json`, `recompile-final.json`, `console-final.json`을 우선합니다.

진단 준비 오류도 구분해 보존합니다. 빌드 define domain reload 중 연결 오류와 잘못된 eval 인수/멤버는 QA tooling 오류이며 수정 후 실제 빌드는 성공했습니다. 초기 Play 준비/종료 사이 UTC06:30:49의 Fusion JoinSessionLobby 비동기 작업은 RegionHandler DontDestroyOnLoad를 Play 밖에서 호출했다는 StartGame Failed 기록을 남겼습니다. 실제 경기 시작06:31:43 이전이며 현재 Console 오류0과 구분하고, 종료 경쟁 자체를 해결됐다고 주장하지 않습니다. 첫 occupancy fixture가 살아 있는 대상과 겹쳐 머리 사격이 miss했고, DontSave 오브젝트를 일반 FindObjectsByType로 찾지 못해 제거되지 않은 QA 준비 오류가 있었습니다. Resources.FindObjectsOfTypeAll로 제거한 뒤 actual lethal 직후 설치로 재시험했습니다. helper ray JSON의 초기 파일 이름은 after-respawn이나 마지막 내용은 dead이며 `dead-head-query.json` 복사본을 판정에 사용합니다. 일부 pose fixture JSON은 재실행으로 덮어써 최종 준비값만 보존되며 실제 발사/피해는 states/events stream으로 대조합니다. Player의 오류2는 첫 Host 정상 Exit에 따른 Fusion Code104 Server has disconnected 로그이며 후속 정상 로비/역할교환 연결과 함께 기록했습니다. OS 보안 설정은 변경하지 않았습니다.

### C3 반동·사격 효과·HUD 실제 검증 (2026-10-07)

Refs #33, #19, #35, #1. 기준 HEAD `00424fed6de792f4d7addbf00c0d1be38b7a86c2`(C3 코드 `635bf5853c215ea5d921c542510fb036c469f678` 포함). 증거 루트 `D:\meee\git\sudden-force-fps-validation\20261007-c3-actual-01`. 새 Windows64 Development 빌드 오류0/경고2, 268,688,921 bytes, 40.1704668초. stage `player`를 보존하고 고정 경로 `D:\meee\git\sudden-force-fps-validation\active-player\SuddenForceFPS.Validation.exe`에 게시했습니다. 기존 고정 폴더는 없었으며 build manifest 365개와 manifest 자체를 포함한 366개 파일의 이름·SHA256 차이0입니다. manifest SHA256 `30E0796E1799C767C9A044BFF09CCEAF340B061B57CA8D2F68C4F6FF1033A6B0`. 실제 Player PID38796이 kr 공개방 `SFMP-C3-20261007-01`~`03`에 목록 참가·Ready·정상 Start로 참여했습니다. Runner -213246/-216744/-220192는 서로 다른 경기입니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| C3-01 view/HUD 초기 상태 | Editor의 local은 view 활성/Camera1/Listener1/몸체15개 ShadowsOnly, remote는 view 비활성/Camera0/Listener0/몸체15개 On. Player 실제 화면에도 local 총기·HP100·30/30·PC 키 안내가 보이고 터치 버튼은 숨겨졌습니다. Player 프로세스 내부 Camera/Listener 수를 직접 조회한 결과로 확대하지 않습니다. |
| C3-02 반동과 방향 | actual shot sample의 Camera forward와 현재 RecoilOffset을 적용한 예상 방향 최대 차이3.31e-7. 첫 발 offset(0,1)/index1, 실제 후속 발 패턴 및 회복 뒤0을 관측했으며 input provider base aim에 반동이 누적되지 않았습니다. 사격 방향은 발사 시 postkick 방향과 일치했습니다. 다음 Render에서 회복된 카메라를 과거 LastShotDirection과 비교하지 않습니다. 최종 LateUpdate 이후 pose/지연 보정 정량 검증은 미실행입니다. |
| C3-03 실제 PC 연속 클릭/장전 | Player 실제 8회 클릭: Shot1~8/탄약30→22, accepted tick9243,9253,9262,9272,9281,9291,9301,9310(9tick 3회/10tick 4회). 실제 R은9733→9861/128tick 뒤30, 화면 RELOADING과 남은 시간 표시를 확인했습니다. Host 실제 R도22151→22279/128tick, 23→30/index0. 실제 연속 hold 시험으로 해석하지 않습니다. |
| C3-04 SDK held-fire 진단 | 별도 focused401poll SDK 입력에서 부분 탄창28발 소비, Shot3~30/28발 간격27개 모두7tick, 빈 탄창 시 추가 사격 없음. Fire+R tick16078에서 장전 시작/목표16206, 중복R에도 목표 유지/장전 중 Shot30 유지. 완료tick16206에 유지된 Fire가 Shot31을 허용하고 index1/offset(0,1), 이후 Shot36/탄약24, release 후 추가 사격 없음. 실제 PC hold·패킷 누락 시험과 구분합니다. |
| C3-05 피해와 HUD | runtime 위치·조준 fixture 후 Host 실제 클릭으로 몸통25, 양팔/양다리18 및 잔여HP3 finisher를 관측했습니다. Client HP100→75→57→39→21→3→0와 HIT25/18/3 표시가 대응합니다. 두 번째 경기 Host Head100→Client HP0/HEADSHOT100, 세 번째 경기 Client Head100→Host HP0/Player 실제 HEADSHOT100·탄약29·head impact를 확인했습니다. Host head 클릭에서는 서로 다른 tick의 Shot1·2가 수락됐고 피해는1회이며 중복 FX로 판정하지 않습니다. |
| C3-06 확정 hit point/실제 miss | 몸통 후 벽 사격은 새로운 LastShotHitPoint와 기존 LastHitPoint를 구분하고 HitSequence 유지/marker 만료 후 재활성 없음. Head 이후 실제 Host Shot37 miss는 LastShotHit=false/탄약23, Head100의 LastHitShot1·LastHitPoint·HitSequence1을 유지하며 HEADSHOT marker 비활성. `confirmed-point-after-nondamage.json`, `confirmed-head-after-miss.json` 참조. |
| C3-07 FX 이벤트/엔진 상태 | 전체 native+diagnostic ShotObserved56개에 Runner·actor·sequence 중복0/sequence gap0. effects audit55개에서 muzzle/audio isPlaying false0, FX counter와 sequence 불일치0, local spatialBlend0 46개/remote spatialBlend1 9개. 실제 화면의 muzzle/impact도 확인했습니다. 소리 청취 품질·음성 지연은 측정하지 않았습니다. RenderState20회 반복으로 FX7→7 유지. 모든56발을 native 입력으로 주장하지 않습니다. |
| C3-08 bind/unbind/new Runner | HUD disable 시 HP--/--/--, enable 직후 HP100/21/30. public Bind+Render20은 FX9→9, 과거 HIT3 marker 비활성. 정상 Exit에서 player/presentation/fx0·LocalInstance/InputInstance null; 새 Runner는 HP100/탄약30/Shot0/Hit0/FX0으로 시작하고 과거 marker가 재생되지 않았습니다. |
| C3-09 PC SafeArea/raycast | 실제 desktop SafeArea anchors0..1/1920×1080, touch Fire/Reload/Sprint 숨김. preview 복원 뒤 실제 EventSystem의 좌이동/우look/HP/탄약/crosshair5개 표본 모두 raycast hits[]로 장식 HUD 차단 없음. notch/동적 SafeArea 시험은 아닙니다. |
| C3-10 Editor 터치 preview | runtime preview fixture에서 synthetic pointer로 이동/Fire/Sprint 소유, 다른 ID release 무시, FireCancel의 개별 해제, 올바른 release/reset, Fire disable 해제를 확인했습니다. native Player 활성화로 실제 Editor focus 상실 후 capturedFire 해제도 확인했습니다. PC preview의 이동 벡터가 실제 Android 이동으로 소비됐다고 판정하지 않습니다. preview raycast의 좌이동 영역은 HP 표본도 포함하며 우look/탄약/crosshair는 비차단, 버튼 중심은 각 버튼 소유입니다. |
| C3-11 HP0와 정리 | 죽은 Client 및 Host의 실제 클릭+R 뒤 HP0/Shot·탄약·reload 유지. 최종 정상 Exit→Lobby manager-223620/runner-223622/player·presentation·fx·events0. Player CloseMainWindow/WaitForExit true, 종료 확인. Editor Play=false/빈 단일 씬 dirty=false/roots0/events0/QA callbacks[]/autotick=false/StandaloneWindows64. 원 Standalone define live+disk 및 ProjectSettings 전체 텍스트 시작 backup과 동일. 최종 recompile은 up_to_date/failed=false/errors[]/warnings[], 현재 consoleErrors0/Warnings1입니다. |

fixture는 공개 SDK NetworkCharacterController.Teleport와 PlayerInputSource.AddLookDelta로 runtime 위치·base aim만 준비했고 발사는 실제 네이티브 클릭으로 수행했습니다. HP/Ammo/Team/Authority를 직접 설정하거나 scene/prefab을 저장하지 않았습니다. SDK held-fire는 own LocalInputAuthority의 공개 SetFire/RequestReload→Snapshot→NetworkInput.Set 및 임시 NetworkEvents.OnInput으로 정상 서버 소비를 관측한 별도 진단입니다. preview focus 전환 중 수락된 Host Shot9도 synthetic 입력이며 native 사격에 포함하지 않습니다. 모든 임시 component와 QA callback은 제거했습니다.

Cursor helper Reset(8,6)→Consume11의 delta3/동일11 무시/rollback9 무시/12의 delta1은 별도 진단(`rebind-and-gap-diagnostic.json`)입니다. 실제 관측56개 이벤트는 모두 delta1이므로 실제 저FPS나 resimulation 합격을 주장하지 않습니다. 미실행: native 지속 hold·hold 중 focus 상실 경계, 실제 저FPS/sequence gap, 실제 rollback/resimulation, Client 예측·서버 보정 정량 측정, packet loss, 3인 아군 첫 hit/동시성/권한 위조, 네트워크 프로세스 역할 교환, Android 실기기/멀티터치 look/OS pause/cancel/notch SafeArea, C4 사망·리스폰·KD 및 C5 승패. HP0 몸체 잔존은 C4 범위입니다.

소스 검증은 prebuild 좁은 범위213파일의 종료 차이0, Assets/Scripts C#53개 변경0입니다. 전체3857파일 inventory는 **빌드 이후·후속 시험 이전**에 시작했으며 종료 차이는 임시 define을 원본으로 복원한 ProjectSettings.asset뿐입니다. 이 전체 inventory를 prebuild 증거로 부르지 않습니다. `source-full-scope.json`, `source-verification-final.json` 참조. 시작/종료 tracked patch 대조에서는 문서 외 `Assets/Fonts/DungGeunMo SDF.asset` 차이가 추가됐습니다(101줄 추가/73줄 삭제, Texture/TMP 직렬화 변경과 Unicode88 glyph/atlas 추가). 자동 import/동적 폰트 저장으로 추정하되 원인을 확정하지 않으며 보존하고 총괄 검토 대상으로 인계합니다. 기존3개 patch block은 동일합니다. 임시 build용 URP_COMPATIBILITY_MODE는 완전히 복원했습니다. stage/fixed build는 모두 보존했고 기존 사용자 변경은 rollback하지 않았습니다.

주요 증거: `active-publication.json`, `build-all-files.json`, `states.jsonl`, `events.jsonl`, `effects-audit.jsonl`, `rate-observer.jsonl`, `summary-final.json`, `player-visible.log`, `held-sdk.json`, `held-rate-summary.json`, `client-head-after-shot.json`, `editor-hit--218384-1.png`, `reload-audio.jsonl`, `desktop-rays.json`, `preview-pointer-rays.json`, `preview-focusloss.json`, `new-runner-initial.json`. 정리는 `lifecycle-final-lobby.json`, `player-normal-close.json`, `cleanup-final.json`, `file-verification-final.json`, `source-verification-final.json`, `recompile-final.json`, `console-final.json`을 우선합니다.

QA 준비 오류/중단은 제품 실패와 구분합니다. 임시 define 직후 compile 진행 중 첫 build 요청이 거절돼 완료 뒤 성공했고, domain reload 중 eval 연결 오류는 인프라 기록입니다. 첫 Editor nickname 팝업은 정상 공개 저장 handler로 처리했습니다. 잘못된 UI `button` 인수로 발생한 Host Shot1은 QA 입력 오류이며 이후 올바른 `mouse_button`을 사용했습니다. 사용자의 Escape 중단 요청에는 즉시 UI를 중지했고 명시적 재사용 승인 후 이어서 수행했습니다. C3에서 Windows 보안 팝업은 관측하지 않았으며 고정 경로의 향후 팝업 방지를 보장하거나 OS 설정 변경을 주장하지 않습니다. build 경고2는 ServicesCore project ID/Pipeline runtime config, 현재 Console 경고1은 Android adb 장치 목록 조회 실패입니다. pipeline의 과거 error1은 C3 QA 이전 잘못된 scene 경로 요청으로 현재 제품 compile/Console error와 구분합니다.

### C2 사격·피해·탄약/재장전 실제 검증 (2026-10-07)

Refs #33, #19, #35. 기준 HEAD `b13fe03b2eb12fd1e3057881b3f26450c0e25736`. 증거 루트 `D:\meee\git\sudden-force-fps-validation\20261007-c2-combat-01`. 새 Windows64 Development 빌드 성공/오류0/경고2, 268,096,463 bytes, 18.779초. 실제 Player PID59020과 Editor Host가 kr 공개방 `SFMP-C2-20261007-01`~`03`에 목록 참가·Ready·정상 Start로 3경기를 수행했습니다. Runner -179762/-182880/-185944는 서로 다른 경기이며 각 NetworkId1030/1031 재사용은 정상입니다. Host 로그 stateAuthority=true와 Player 복제 로그 false의 HP/탄약/Shot/Hit/부위/피해를 비교했습니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| C2-01 실제 사격/탄약 | 실제 Player 단발 클릭 Shot1/탄약30→29. 사용자 실제 입력으로 Shot2~31의 30발 소비, 탄약29→0을 관측했습니다. 인접 발사 간격은 7tick 27회, 36tick 1회, 637tick 1회(총862tick)입니다. 중단이 포함되므로 전체30발이 연속4초 hold였다고 주장하지 않습니다. 실제64Hz에서 최소 간격7tick이며 빈 탄창 시도5회는 empty-magazine 거절/Shot31·탄약0 유지. |
| C2-02 정상 재장전 | 실제 Player R: tick30237→30365 및 빈 탄창36052→36180, 각각128tick/2초 뒤 탄약30. 실제 Editor R도10306→10434/128tick. Editor 후속 클릭10612는 완료 뒤였으므로 네이티브 장전 중 사격 시험으로 판정하지 않습니다. |
| C2-03 몸통 피해 | runtime 위치/조준 fixture 후 Host 실제 클릭4회, Client HP100→75→50→25→0, Torso/피해25/Shot·Hit1~4/Host 탄약30→26. Player 복제 로그 동일. |
| C2-04 양쪽 팔/다리 | 새 정상 경기에서 Host 실제 클릭: 왼팔18→HP82, 오른팔18→64, 왼다리18→46, 오른다리18→28, 오른다리18→10, 마지막 오른다리 실제 적용10→0. 마지막 LastHitDamage10은 남은 HP만큼 적용된 결과이며 설정18과 구분합니다. Shot·Hit6/탄약24 및 Player 복제 일치. |
| C2-05 Client 머리 사격 | 세 번째 정상 경기에서 Client의 기존 실제 aim 방향에 Host를 배치한 fixture 후 Player 실제 클릭: Head/피해100, Host HP100→0, Client Shot3→4/탄약27→26/Hit1/TargetPlayer1. 양쪽 로그 일치. 모든 부위를 양쪽 역할에서3회 반복한 시험은 아닙니다. |
| C2-06 HP0 차단 | 실제 Client 사망 후 클릭+R: HP0/탄약10/Shot51/reload0 유지(tick44372→45238). 실제 Host 사망 후 클릭+R도 HP0/탄약27/Shot4/reload0 유지(16374→17415). 다음 피해 시험의 HP 복원은 정상 Exit→새 방·새 Runner 경기로 수행했습니다. |
| C2-07 입력 우선순위 진단 | 별도 승인된 SDK OnInput 경계의 focused181poll: 원 정상 입력0 뒤 진단 Buttons6(Fire+Reload)을 전달하고 다음 tick PreviousButtons6 소비 확인. 가득 찬 탄창의 동시R+Fire는 사격 없음, 부분 탄창의 동시 입력은 장전 시작9757/완료9885(128tick), 장전 중 Fire는 Shot1/탄약29 유지. 중복R에도 완료tick9885 유지, 정상 noFire 입력 중 완료 후 탄약30. 완료 후 동시R+Fire도 사격 없음, 다음 정상Fire 허용. **네이티브 PC 입력 시험과 구분합니다.** |
| C2-08 비정상 aim 진단 | 같은 SDK 진단에서 cooldown 이후 NaN yaw/pitch99/HasAimfalse는 Shot2/탄약29 유지 및 PreviousButtons0, 다음 유효Fire는 Shot3/탄약28로 회복. legacy boolFiretrue/Buttons.Firefalse도 사격 없었으나 cooldown과 겹쳐 독립 거절 증거로 확대하지 않습니다. read-only helper의 tick 수·범위/벡터 거절은 악성 remote 권한 시험이 아닙니다. |
| C2-09 벽/자기 제외 | 실제 diagnostic Shot3 hit 후 replay의 nearest는 PhysX Wall_04/layer8, HitSequence0/양쪽HP100으로 벽 차단 범위 확인. 자기 head/capsule 안의 발사 원점에서도 상대 피해와 자기HP100 유지, C1 자기 제외 대조를 함께 참조합니다. 벽 모서리/두께 전수 및 아군 첫 hit의 관통 차단은 미실행. |
| C2-10 수명/정리 | 정상 경기 Exit→새 로비 manager -189012/runner -189014/player0/events0. 실제 Player 정상 CloseMainWindow/WaitForExit true, 종료 확인. Editor Play=false/빈 단일 씬 dirty=false/roots0/events0/QA callbacks[]/autotick=false/Win64. 원 Standalone define live+disk 및 ProjectSettings 전체 텍스트 시작 backup 동일. C#46개 SHA256 변경0, 최종 compile failed=false/errors[] 및 consoleErrors0. |

피해 fixture는 공개 SDK NetworkCharacterController.Teleport와 PlayerInputSource.AddLookDelta로 runtime 위치·조준만 준비한 시험입니다. 실제 발사는 네이티브 클릭이며 정상 걷기/수동 조준 합격으로 확대하지 않습니다. HP/Ammo/Team/Authority를 직접 설정하지 않았고 prefab/scene을 저장하지 않았습니다. SDK 진단은 정상 input provider의 공개 SetFire/RequestReload→Snapshot→NetworkInput.Set 경로로 own LocalInputAuthority 입력을 전달했습니다. private combat 호출이나 임의 피해 주입 없이 실제 서버 소비 상태를 관측했으며 임시 NetworkEvents/callback을 모두 제거했습니다. 이후 정상 Host 네이티브 Shot4도 관측했습니다.

미실행: 3인 이상 아군 첫 hit 차단·같은 팀 사격, 임의 remote 피해/권한 위조, 네트워크 Host/Client 프로세스 역할 교환, held-fire 중 focus 상실, 패킷 누락 중 장전 완료, Android, 모든 부위 양방향 반복 전수. 장전 진단의 대기 구간은 HasAim=true/noButtons의 유효 입력이므로 패킷 누락으로 해석하지 않습니다. HUD는 실제 HP/탄약 변화에도 초기 표시가 남아 C3 연결 검증이 필요하며 C2 상태값 합격과 분리합니다. Kill/Death0·HP0 몸체 잔존/리스폰 부재는 C4 범위, 승패는 C5 범위입니다.

주요 증거: `editor-combat-observer.jsonl`, `player-visible.log`, `run-summary.json`, `torso-runtime-fixture.json`, 양쪽 팔/다리 fixture JSON, `client-head-runtime-fixture.json`, `client-head-after-shot.json`, 양쪽 `dead-*-input.json`, `poll-diagnostic-focused.json`, `readonly-contract.json`, `wall-after-poll-diag.json`, 새 build manifest. 정상 종료/복원은 `lifecycle-final-lobby.json`, `player-normal-close.json`, `cleanup-final.json`, `recompile-final.json`, `console-final.json`, `file-verification-final.json`을 우선합니다.

실패한 QA 진단도 보존합니다. 초기 Editor.update 입력 hook은 정상 PC Update에 덮여 원하는 동시 입력 근거가 아니었고, 첫 SDK probe는 GameView 비포커스로181poll 모두0이었습니다. 임시 NetworkEvents UnityEvent 초기화 누락/QA eval 타입 오타는 진단 오류이며 수정 후 focused probe 성공과 제품 compile 오류0을 구분합니다. 최초 Hidden Player PID58180은 창이 없어 실제 시험에서 제외하고 정확한 본인 실행 경로 확인 후 종료했습니다. 새 exe 보안 알림은 사용자 직접 처리 이후 시험을 진행했습니다. 제품 코드 변경이나 OS 보안 설정 조작을 하지 않았습니다. 최종 경고17은 기존 UniTask Editor CS0618 등, 빌드 경고2는 ServicesCore project ID/Pipeline runtime config입니다. 자동 저장 asset 차이는 시작/종료 patch로 보존하고 기존 사용자 변경을 rollback하지 않습니다.


### C1 Hitbox 기반 실제 검증 (2026-10-07)

Refs #33, #19, #35. 기준 HEAD `da43800b07db7281a18ec6d6483f40e4ef6e3e19`(코드 `d9a6713` + 아트 `da43800`). 증거 루트 `D:\meee\git\sudden-force-fps-validation\20261007-c1-hitbox-01`. 새 Windows64 Development 빌드는 오류0/경고2, 268,081,158 bytes, 32.894초입니다. 임시 Standalone URP_COMPATIBILITY_MODE를 사용했으며 종료 시 live/disk 원 define을 복원했습니다. Player PID71676의 실제 kr 로비→공개방 목록 참가→Ready→경기를 Editor Host와 수행했습니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| H01 등록/소유자 | 실제 두 player에 root각1/Hitboxes각12, 총24. 모든 metadata IsConfigured=true, index0~11/owner Player1·2/layer10 대응. 두 번째 새 Runner 경기에서도24/configured=true. prefab bake/등록표의 정적 근거는 C1 아트 인계와 구분합니다. |
| H02 부위 계약 | 실제 각 Head1/Torso3/Arm4/Leg4, 좌우 arm/leg 이름·소유자 대응을 확인했습니다. WeaponDefinition/CombatConfigured=true. 설정의 피해값은 사격 적용 합격이 아닙니다. |
| H03 animated broad bounds | QA의 실제 전 animation 상태 검증은 NOT RUN. 아트 담당의 broad radius2.3/offsetY.9 및 22상태×61표본 결과는 보조 인계 근거입니다. 이번 runtime child offset 실험을 정상 animation pose의 broad bounds 검증으로 확대하지 않습니다. |
| H04 부위 query | Editor Host에서 양팀12개 각각6축, 총144개 실제 SDK ray를 수행했습니다. Head/Torso/좌우 Arm·Leg 명중 분류를 확인했고 nearest가 다른 부위인 ray도 숨기지 않고 보존했습니다. spine_03은 6축 모두 다른 nearer 부위였으므로 이 bone 단독 노출 합격을 주장하지 않습니다. Host/Client 역할 교환 query·경계/겹침 전수·양쪽 시각적 일치는 NOT RUN. |
| H05 history/보간 | 실제 원격 PlayerRef2로 Host head query 정확한 hit, GetPlayerTickAndAlpha tick3123/tickTo=null/alpha=null. 원격 중간 alpha는 미관측. 별도 runtime-only child worldX .6m 변화 후 실제 저장된 인접 snapshot2472/2473의 head 위치를 RaycastAll로 확인하고 alpha.5/SubtickAccuracy 조회: head 반환 위치가 두 snapshot 중점과 오차0. 원 localPosition 복원true, callback해제. 이전 비인접 tick+수직 nearest 실험의 miss/torso 결과도 보존하며 제품 보간 오류로 판정하지 않습니다. 실제 네트워크 이동/animation capture timing·지연/손실 조건의 클라이언트 시각 보정은 NOT RUN. |
| H06 자기/CC 제외 | Player1 자기 head 쿼리는 IgnoreInputAuthority 전true/후false. 원격 PlayerRef2+IgnoreInputAuthority로 Player1 head는 hit하므로 상대까지 제외하지 않음. Physics layer9 대조에서 SoldierBlue CC hit, shot mask layer8+10 실제 결과 Wall(layer8)로 CC 제외 확인. Fusion IncludePhysX의 layer9-only query0은 동적 CC를 직접 포함하는 대조로 해석하지 않습니다. |
| H07 벽 우선 | layer8+10/IncludePhysX/IgnoreInputAuthority RaycastAll 결과를 거리 정렬하면 Wall_04 2.95755m가 enemy head27.89086m보다 앞선 실제 blocking geometry. SDK RaycastAll 배열 자체의 정렬을 가정하지 않았습니다. 벽 두께·모서리·관통 사격/피해 적용은 미검증입니다. |
| H08 수명 | 첫 Host 정상 Game Exit→새 manager -163834/runner -163836/Lobby/player0/LagCompensation=null→새 공개방 실제 Client 재참가·Ready·경기24/configured=true. 다음 Client 정상 window close 후 ActivePlayers1/GamePlayer1/root1/현재 child12, 떠난 Blue head 위치 query는 miss. 이때 history debug TotalHitboxes24가 남았으므로 이 필드를 활성 child 수와 동일시하지 않습니다. 마지막 Host 정상 Exit 및 전체 Editor 정리 완료. |

실험은 HP/Ammo/Ready/input 상태를 주입하지 않았습니다. query는 C1 진단이며 **C2 발사·피해·발사 권한·탄약/재장전·킬/사망·승패 합격이 아닙니다**. Animator나 RuntimeAnimatorController를 수정하지 않았고, 세 runtime child offset 진단은 각각 원 localPosition을 복원했습니다. 인접 pair 실험은 복원 여부를 JSON으로 직접 남겼습니다. 제품 C# 45개 SHA256 전후 변경0, 제품 prefab/scene/config을 직접 수정하지 않았습니다.

주요 근거: `runtime-queries.json/.cs.txt`, `history-query.json`, `history-query-small-offset.json`, `history-adjacent-query.json/.cs.txt`, `body-mask-control.json`, `lifecycle-lobby.json`, `lifecycle-second-game-playerref.json`, `lifecycle-client-exit-query.json`, `player-A.log`, 새 build manifest. Player의 Host 종료 Code104/Server has disconnected 로그는 의도한 종료 경로이며 다음 로비/경기 성공과 함께 기록합니다. 초기 두 history 실험의 잘못된 중간 기대값을 head 보간 오차나 제품 FAIL로 귀속하지 않습니다.

종료 정리: Player CloseMainWindow true/WaitForExit true 및 프로세스 소멸, Editor Play=false/autotick=false/빈 단일 씬 dirty=false/Win64, 원 Standalone define live+disk 복원. ProjectSettings 전체 텍스트는 시작 backup과 동일합니다. 빌드 자동 저장의 URP/GlobalSettings 등 차이는 `source-before.patch`와 `source-after.patch`로 보존하며 사용자 변경을 임의 rollback하지 않았습니다. 최종 compilation/import/console 결과는 `cleanup-final.json`, `recompile-final.json`, `console-final.json`을 우선합니다. recompile completed/failed=false/errors=[] 및 consoleErrors0이며, consoleWarnings17은 UniTask Editor TreeView API CS0618 등으로 별도 기록했습니다. 시작/종료 patch block 비교의 추가 tracked 차이는 이 문서뿐이며 설정/asset block은 동일합니다.

### 단계 2 이동·로컬 카메라 실제 검증 (2026-10-07)

Refs #33, #19, #35. 기준 HEAD `07cb3525bae273d525523345c4fed6b5c99fa751`의 전체 SHA는 증거 `head-before.txt`를 우선합니다(이동 코드 `999e1f0` + 프리팹 연결 `07cb352`). 신규 증거 루트는 `D:\meee\git\sudden-force-fps-validation\20261007-stage2-movement-01`입니다. 이전 exe는 사용하지 않았습니다. 새 Windows64 Development manifest: 성공/오류0/경고5, 268,076,937 bytes, 빌드 시간45.39초. Standalone URP_COMPATIBILITY_MODE는 승인된 임시 빌드 define이며 원복합니다.

| 표준 | 실제 결과와 제한 |
| --- | --- |
| P01 Camera/Listener/몸체 | PASS(직접 관측 범위). Editor Host Red와 역할 교환 Editor Client Blue 모두 local Camera·AudioListener 각1 enabled, body renderer8개 ShadowsOnly. remote는 Camera·Listener0 enabled/body On. Player는 실제 맵/HUD·시점 화면으로 확인했으며 내부 enabled 목록을 직접 열람하지 못했습니다. |
| P02 Blue seed | PASS. 첫 경기 remote Blue transform/AimYaw180, 역할 교환 local Blue transform/AimYaw180·stored input yaw180. 포커스 복귀 후 Snapshot HasAim=true/yaw180/pitch0/move0으로 첫 입력이 0도 방향을 덮지 않음을 확인했습니다. |
| P03 실제 입력/포커스 | PASS(범위 제한). Player Blue W와 마우스 drag→Host 복제 위치/aim 변경, Red 불변. Editor Red 실제 유지 WASD/마우스·ShiftW, 역할 교환 Editor Blue Game view 클릭 후 W/drag 위치·aim 변경, remote Red 불변. 실제 창 전환은 focused=false·move0/sprintfalse·CursorNone 및 다음 경기 input owner 해제까지 확인. **키를 누른 채 focus를 잃는 경계는 NOT RUN**(관측된 focus 전환 직전 키가 이미 해제됨). Android pause/touch도 NOT RUN. |
| P04 속도/아날로그/예측 | 지속 키 관측 PASS: 수동 실제 입력 90초 read-only frame observer에서 W524프레임, ShiftW160프레임. 10프레임 이상 동일 입력/상한 유지 구간은 walk402프레임/peak5.00054, sprint127프레임/peak7.50051. 전환 직후 이전 tick 값은 제외했습니다. 조이스틱 실제 drag와 후속 입력에서 moveX/Z가 비단위 값으로 유지되며 속도 변화 확인(23 active unique frames). **아날로그 고정 크기별 정상 속도비·대각선 과속·지연/손실 예측/보정 정량 비교는 NOT RUN**. Player 순간 W 변위 약0.055m는 속도 시험의 대체 근거로 쓰지 않습니다. |
| P05 벽/ground/동선 | 부분 확인: 실제 Red가 spawn→중앙 통로→Blue 측 통로를 이동, 양쪽 grounded 및 해당 표본 capsule overlaps[] 확인. CC height1.8/radius0.3/stepOffset0.3/slopeLimit45. **벽 밀기·모서리·낮은 천장·벽 관통 재현시험은 NOT RUN**. 유지 W 중 정지 접촉을 확인할 샘플은 없었으므로 통로 이동을 벽 충돌 합격으로 확장하지 않습니다. 아트의 정적 capsule sweep은 별도 보조 증거입니다. |
| P06 계단/경사 | NOT RUN. 이번 실제 이동 기록은 평면 통로이며 허용/초과 계단·경사 동선의 합격을 주장하지 않습니다. Jump는 이번 구현 범위 밖입니다. |
| P07 각1 spawn | PASS(정상 경로). 두 경기 각각 Red/Blue Id1030/1031 각1, 이후 ActivePlayers2/roster2/spawn dictionary2 유지. 같은 NetworkId는 **새 Runner 경기에서 재사용**될 수 있으며 경기 사이의 식별자를 전역 고유로 간주하지 않습니다. 반복 callback/동시 Start 강제 호출은 하지 않았습니다. |
| P08 안전 spawn | 부분 확인: 실제 1vs1 초기 pose 및 이동 표본에 static/player capsule 겹침 없음. **같은 팀 점유 spawn 회피는 NOT RUN**, 실제 추가 peer와 점유/fallback 정책 검증이 필요합니다. 소스는 현재 임의 후보 선택이며 점유 회피가 구현됐다고 단정하지 않습니다. |
| P09 이탈 회귀 | PASS. 정상 Editor Host Game Exit→새 manager -138396/LobbyConnected/roster0/inputBoundfalse, Player 자동 kr 로비/빈 목록. Player Host 새 공개방 생성→Editor 실제 수신 목록 참가→Ready→다음 경기 성공. Editor Client Game Exit→manager -141560/LobbyConnected/receivedtrue/roster0/inputBoundfalse/manager1. Player 정상 CloseMainWindow 종료 확인. |

입력은 실제 네이티브 키/클릭/drag 및 사용자 수동 입력을 관측했습니다. Ready/session/input 상태 직접 주입이나 HUD 숨김으로 합격을 만들지 않았습니다. 입력 observer는 transient Editor update callback으로 관측만 하고 종료 시 스스로 구독을 해제하며 증거 폴더에 JSONL을 남깁니다. 첫 observer 요청의 delegate 타입 오류와 초기 Login 전 manager=null 읽기 오류는 QA eval 오류이며 제품 컴파일/게임 예외와 구분합니다. 실제 제품 C# SHA256은 검증 전후 변경0입니다.

주요 증거: `initial-two-peer-presentation.json`, `player-blue-w-after.json`, `player-blue-mouse-after.json`, `manual-held-input-samples.jsonl`, `analog-editor-samples.jsonl`, `two-peer-capsule-physics.json`, `after-movement-counts.json`, `role-exchange-blue-presentation.json`, `blue-focused-aim-seed.json`, `role-exchange-editor-mouse.json`, `host-leave-lobby.json`, `client-lobby-connected.json`, `player-A.log`, `editor-cleanup.json`. native 화면은 Computer Use 출력에 관측했습니다. 신규 경로 Player의 Windows 방화벽 알림은 사용자 직접 처리 후 닫힘을 재관측했고 에이전트가 보안 설정을 조작하지 않았습니다.

후속 우선순위: PC 벽/계단/경사와 held-focuslost, 아날로그 고정 크기·대각선/예측 tick 비교, 추가 peer 점유 스폰 회피, Android 오른쪽 touch와 조이스틱 입력 분리. HUD 중앙 mouse drag는 실제 재현에서 회전이 가능했지만 다른 raycast 영역/touch 전체를 합격으로 확장하지 않습니다.

### 2026-10-07 실제 실행 결과

증거 루트: `D:\meee\git\sudden-force-fps-validation\20261007-stage12-01`. 빌드 산출물은 `player-retry\SuddenForceFPS.Validation.exe`, manifest는 `player-retry\validation-build.json`입니다. 성공 빌드: 오류 0, 경고 5, 268,516,831 bytes, manifest 파일 364개. 최초 빌드는 URP compatibility 사전 검사에서 실패했으며 성공 빌드와 구분합니다. 총괄 승인으로 Standalone에 URP_COMPATIBILITY_MODE를 빌드 중 임시 추가했습니다. Renderer/RenderGraph 설정은 변경하지 않았습니다.

| 범위 | 실제 결과와 관측 한계 |
| --- | --- |
| L01/L02 공개방 | PASS. Player Host `SFMP-20261007-1234-01` 생성, Editor는 수신된 AD.Room 공개 목록 handler로 참가. 역할 교환 시 Player는 화면의 Editor Host 공개방을 직접 클릭해 참가. 양쪽 로그 Region kr. Editor 실제 SDK client AppVersion 빈 문자열, ClientServer/Default 확인. Player 실제 AppVersion 직접 관측 수단은 없어 동일 namespace는 공개 발견·참가 성공과 동일 소스/설정에 따른 추론으로만 기록합니다. |
| L03 3회 재참가 | PASS. 정상 ExitButtonClick→Connected Lobby→실제 수신 목록 JoinRoom을 3회 반복. 새 manager -65808→-67228→-68648, roster 0, manager 1, 닉네임 보존을 snapshot으로 확인. 최종 재참가 2인 roster와 Host 화면 확인. |
| R01 Ready/Start | PASS(실행 범위). Client 미준비 상태의 Host 시작은 방 유지, Client OnStartButtonClicked도 방 유지. 정상 ReadyButtonClick→RPC 요청 후 Host 화면/복제 상태 모두 Ready. Host 정상 시작 성공. 팀은 Red/Blue 각 1명으로 일치. 불법 팀·초과 정원·동시 요청은 NOT RUN. Ready 상태 직접 주입은 하지 않았습니다. |
| R02 맵/스폰 | PASS(씬/스폰), FAIL(카메라). Player Host 경기에서 Editor 실제 scene DesertHouse/Game, SoldierRed Id1036/InputPlayer1·SoldierBlue Id1037/InputPlayer5 각각 1개. 역할 교환 Editor Host는 roster 2·spawn dictionary 2, Red Id1030/InputPlayer1·Blue Id1031/InputPlayer2. ClientServer StateAuthority PlayerNone은 서버 권한 표현입니다. Player의 경기 HUD/맵 화면은 관측했으나 Player 프로세스 내부 객체 수는 직접 열람하지 못했습니다. |
| 카메라/HUD/입력 | FAIL/NOT RUN. Editor 클라이언트에 활성 Camera 2개(Untagged), AudioListener 2개. 첫 Player Host 경기 화면은 회색 배경/HUD, 역할 교환 Player Client는 맵/HUD 표시. 이동·전투·로컬 카메라 분리 미구현 상태이며 각 spawn 합격과 분리합니다. |
| Client 게임 이탈 | PASS. 정상 ExitButtonClicked 후 새 manager -71838, Lobby/Connected, roster 0, 공개방 count 0. Host Player는 정상 CloseMainWindow 종료. |
| Host 방/게임 이탈 | PASS. Editor Host 방 Exit 후 새 Runner 로비, Player Client도 서버 종료 수신→kr 로비 재연결·빈 공개 목록. 새 Host 방 `hostleave-03`을 Player가 실제 목록 클릭 재참가. 역할 교환 게임 시작 후 Host 게임 Exit도 Editor manager -75854/Lobby/Connected/roster0/manager1과 Player 빈 목록 복귀 확인. |
| 전투/승패/Android | NOT RUN. 구현 통합 및 별도 기기 필요. 이번 결과로 전체 게임 합격을 주장하지 않습니다. |

주요 증거: `editor-public-list.json`, `editor-joined-room-final.json`, `client-lobby-after-exit-{1,2,3}.json`, `client-ready-request.json`, `two-peer-game.json`, `two-peer-game-editor-host.json`, `client-game-leave-lobby.json`, `host-leave-new-runner.json`, `hostleave-rejoin.json`, `host-game-leave-lobby.json`, Player B/C 로그. 네이티브 화면은 Computer Use 도구 출력에서 관측했습니다. Editor handler 호출은 실제 UI와 동일한 public handler/RPC 경로이며 직접 Ready 상태 변경이나 session 목록 주입을 하지 않았습니다. Editor Host 방 생성은 public CreateRoom 설정 요청을 사용했습니다.

Host 종료 시 Player Fusion `Code 104 / Server has disconnected / DisconnectedByPluginLogic`가 debug console error 카운트에 누적되지만 이후 재연결·재참가·새 경기까지 성공했습니다. 이 종료 로그를 미처리 제품 예외로 간주하지 않습니다. Editor 콘솔 수집에는 이전 Pipeline main thread timeout 이력이 1개 포함되며 최종 groundTruth compilationFailed=false/consoleErrors=0였습니다. 과거 빌드/QA 요청 오류와 실제 게임 오류를 구분합니다.

닉네임·방 이름의 TMP textComponent에서 trailing U+200B가 관측됐습니다. exact 문자열 매칭과 정규화 후보로 인계하며 이번에는 코드 수정하지 않았습니다. 빌드 시점 C# SHA256 inventory와 종료 후 비교: 변경 0개. 종료 정리: Player 정상 종료, Editor Play=false·컴파일/import=false·빈 단일 씬 dirty=false·autotick=false, target StandaloneWindows64 유지. 원 Standalone define은 live getter와 ProjectSettings 직렬화 양쪽에서 복원 확인합니다. 직접 변경 파일은 문서/신규 빌드 helper 및 자동 생성 meta입니다.

- 다음 시험: PC 벽·계단·경사/held-focuslost·아날로그 고정 크기와 Android 실제 입력, 이후 전투 구현 단계의 C02 이후를 검증합니다.
- 후속 선행 조건: 총괄 Editor 제어권 배정, 통합 snapshot 및 관측 가능한 전투 상태. Player 실제 AppVersion 직접 관측은 아직 필요합니다.
- 실행 수단: 신규 `MultiplayerValidationBuild.BuildWindows64(root, output)` 또는 전용 batch의 `BuildFromCommandLine`. 정확한 사용법은 마지막 부록에 있습니다.
- 이번 합격 범위: 공개 목록 발견·참가, roster 2명/서로 다른 PlayerRef, 퇴장·재참가 3회와 역할 교환, 양쪽 실제 로그·화면 증거.
- 전체 합격: 피해·권한·재장전·사망/리스폰·점수/승패 일치→로비→새 경기, Android 두 기기 및 Play 설치본 확인. 추가 peer 필요 항목은 별도 배정합니다.
- 직접 작성 범위: 이 문서와 신규 `Assets/Scripts/Editor/MultiplayerValidationBuild.cs`. 기존 BuildScript/설정/씬/패키지/Login은 해당 담당이 관리합니다.

아래 조사는 첫 작성 시점의 snapshot입니다. 다른 담당 수정이 들어오면 후속 실행 인계 시 현재값으로 다시 확인합니다.

2026-10-07(KST) 작성. 실제 저장소는 `D:\meee\git\sudden-force-fps`입니다. `PROJECT_PLAN.md`, `ANDROID_RELEASE_CHECKLIST.md` 및 현재 소스를 읽어 작성했습니다. 조사 시 HEAD는 `a2ad245fe5a4ef5f52bbf585070a67d452bcf65d`이며 작업 트리에 사용자 업그레이드와 여러 담당 변경이 있습니다. HEAD만으로 테스트 빌드를 식별하지 않습니다.

아래 초기 조사는 실행 전 snapshot이며 위 실제 실행 결과가 현재 상태입니다. 기존 단일 Lobby smoke는 다른 담당의 보조 근거로만 유지합니다. Photon Dashboard·계정 설정은 조회하거나 변경하지 않았습니다.

## 1. 읽기 조사 결과와 실행 수단

| 근거 | 현재 확인 내용 | 검증에 주는 영향 |
| --- | --- | --- |
| `Assets/Photon/Fusion/Resources/NetworkProjectConfig.fusion` | PeerMode 저장값 0, LagCompensation.Enabled=false, NetworkConditions.Enabled=false, ConnectionTimeout=10초 | Multi-Peer 활성 여부는 후속 Editor 배정 시 enum 이름으로 다시 확인합니다. 지연 보정 테스트는 해당 기능 구현·활성화 이후 진행합니다. |
| Fusion SDK `FusionBootstrap.cs`, `NetworkSceneManagerDefault.cs`, Runtime XML | Multiple 모드 다중 Runner와 개별 PhysicsScene/씬 지원 코드가 존재합니다. SDK XML은 Single을 프로세스별 한 peer로 설명합니다. | SDK 지원과 제품의 다중 Runner 호환은 별도입니다. 설정 숫자만 보고 제품 호환으로 판정하지 않습니다. |
| `Assets/Scenes`, `Assets/Prefabs`의 Bootstrap script GUID 검색 | SDK FusionBootstrap의 직렬화 참조를 찾지 못했습니다. | SDK 자동 다중 클라이언트 실행 흐름을 제품 씬이 사용한다고 가정하지 않습니다. |
| `NetworkRunnerManager`, `AD.Managers`, `RoomManager`, `SpawnPoints`, `UIManager` | static Instance가 존재하며 spawn·입력·UI가 전역 접근을 사용합니다. 일반 Unity SceneManager 호출도 존재합니다. | 동일 프로세스의 다중 Runner는 입력·팀 스폰·씬·UI를 잘못 공유할 위험이 있습니다. 현재 주 검증 수단으로 부적합하다는 소스 기반 판단이며 실제 재현 결과는 아닙니다. |
| `Packages/manifest.json` | Multiplayer Center는 있지만 `com.unity.multiplayer.playmode` 직접 의존성은 없습니다. | Unity Multiplayer Play Mode의 가상 Player를 지금 사용할 수 있다고 가정하지 않습니다. 설치하지 않았습니다. |
| `Assets/Scripts/UI/Login.cs` | Editor는 Lobby 진입, Android는 Google 로그인 성공 시 진입. Windows Standalone 분기가 없습니다. | 현재 Login 시작 PC 빌드는 로비로 진행하지 못할 것으로 예상됩니다. 네트워크/안정화 담당이 개발용 진입 경로를 구현·확인해야 합니다. Android 인증을 생략하는 배포본 변경은 요구하지 않습니다. |
| `ProjectSettings/ProjectSettings.asset` | runInBackground=1, forceSingleInstance=0 | 두 PC 프로세스 실행을 막는 저장 설정은 없습니다. 실제 비활성 창 tick·입력 해제는 후속 실행에서 확인합니다. |
| `ProjectSettings/EditorBuildSettings.asset` | Login → Lobby → Room → Game/DesertHouse, 모두 enabled | 각 클라이언트가 동일 목록·참조로 빌드돼야 합니다. 하드코딩 Room index 2 및 경기 씬 계산은 담당 수정 후 재확인합니다. |
| `PhotonAppSettings.asset` | AppVersion, FixedRegion이 빈 값입니다. | 이전 kr 연결은 런타임 관측이며 지역 고정의 증거는 아닙니다. 두 클라이언트의 실제 적용 AppVersion과 Region 일치가 필요합니다. |
| `NetworkRunnerManager.cs` | ClientServer 로비, CreateRoom=Host, JoinRoom=Client, NetworkSceneManagerDefault 전달 | Shared/Single 테스트로 대체하지 않습니다. 각 프로세스의 Host/Client 역할을 증거에 남깁니다. |
| `CanvasLobby.cs`, `NetworkRunnerManager.cs` | UI는 IsPrivateRoom 키에 `!toggle.isOn` 전달, manager는 이를 IsVisible로 사용합니다. 시작 시 IsVisible=false이며 현재 StartGame에 IsOpen=false 설정은 보이지 않습니다. | 키 이름만으로 공개/비공개 반전 버그로 단정하지 않습니다. 실제 공개 UI→IsVisible=true를 확인합니다. 목록 숨김과 경기 중 신규 참가 차단은 별도 검증합니다. |
| 저장소 추적 제외 포함 파일 검색 | 저장소 안 exe/APK/AAB를 찾지 못했습니다. | 외부 빌드가 없다는 뜻은 아닙니다. 총괄/빌드 담당으로부터 실행 파일 경로·빌드 결과를 받아야 합니다. |

[Photon Multi-Peer 공식 문서](https://doc.photonengine.com/fusion/v2/manual/testing-and-tooling/multipeer)는 Runner별 독립 simulation/physics/scene과 static·singleton 충돌 주의를 설명합니다. [씬 로딩 문서](https://doc.photonengine.com/fusion/v2/manual/scene-loading)는 기본 SceneManager의 Multi-Peer 지원을 설명합니다. 이를 근거로 SDK 가능성과 프로젝트 적합성을 구분했습니다.

| 방식 | 적합성·선행 조건 | 담당 배정 이후 용도 |
| --- | --- | --- |
| Editor + Windows Development build | 우선 권장. 단일 Editor 소유권, PC Login 진입 수정, PC 입력 경로·모듈·빌드 성공 확인이 필요합니다. | 초기 공개 방→대기실→경기 검증. Editor와 build의 다른 코드 시점 문제를 기록합니다. |
| 동일 Windows build의 두 별도 프로세스 | 동일 산출물로 버전 차이를 줄일 수 있습니다. PC Login·입력 지원이 필요합니다. 두 PID·서로 다른 로그 파일 필수입니다. | Editor 점유 없이 회귀 검증. 한 PC의 창 전환은 동시 조작 검증을 제한하므로 각자 정지 표적 시험 후 두 기기/두 운영자로 보완합니다. |
| PC/Editor + Android | APK·PGS 인증·지정 실기기·각 입력 지원이 필요합니다. | 플랫폼 간 동기화 및 모바일 조작. Host/Client 역할을 바꿔 재검증합니다. |
| Android 두 실기기 | 지정 기기·테스터 인증·APK 또는 Play 설치본이 필요합니다. | 최종 필수. 실제 두 사용자가 한 판 완료·로비 복귀·재참가, Play 설치본 검증까지 수행합니다. |
| 동일 프로세스 Multi-Peer | 현재 제약으로 바로 실행할 수 없습니다. 총괄이 필요성을 결정하고 담당이 Runner별 관리자·씬·입력·visibility·AudioListener/EventSystem을 분리한 후 배정합니다. | 보조 진단만 가능하며 두 실제 프로세스/실기기 합격을 대체하지 않습니다. 계획 문서의 Multi-Runner 선행 순서는 이 제약을 반영해 총괄이 확정합니다. |

두 번째 Editor나 다른 Unity 프로젝트를 열어 해결하지 않습니다. PC 빌드 담당이 없다면 가장 빠른 대체 경로는 안정화 담당의 APK + 현재 Editor이며, APK/인증이 준비되기 전에는 실행 선행 조건 미충족으로 기록합니다.

## 2. 후속 실행 전 인계 조건

총괄이 실행 수단, 단일 Editor 소유권, 테스트 시간대, 테스트 방 생성과 종료를 배정한 이후에만 실행합니다. 현재 파일 외 수정은 소유 담당에게 요청합니다.

1. 코드/씬/프리팹 통합 완료 시점을 정하고 그 시점의 commit + 작업 트리 diff 식별값 또는 보존 snapshot, Unity/Fusion 버전, 산출물 SHA-256, 플랫폼/Development 여부를 받습니다. 실행 도중 소스가 바뀌면 기존 빌드와 섞지 않습니다.
2. PC 방식이면 Login→Lobby와 두 클라이언트 입력 가능 여부, Windows 빌드 모듈·실제 build 성공을 먼저 확인합니다. 현재 Android 전용 BuildScript를 Windows 빌드 명령으로 사용하지 않습니다.
3. 같은 기존 Fusion App ID를 사용합니다. 기록에는 App ID 전체 대신 일치 판정/지문을 사용합니다. 앱·지역 allowlist·계정·요금제를 바꾸지 않습니다.
4. 두 peer의 실제 Region=kr, 실제 Photon AppVersion 동일, SessionLobby=ClientServer, custom lobby 사용 여부 동일을 기록합니다. FixedRegion/AppVersion이 빈 현재 구성에서는 자동 선택 결과를 관측해야 하며, 불일치 시 네트워크 담당의 기존 프로젝트 내 실행 설정을 먼저 받습니다.
5. 두 클라이언트 A/B의 구분 가능한 닉네임, PID 또는 기기 모델·OS, 서로 다른 로그 경로, 테스트 운영자·입력 방법을 지정합니다. 개인 계정 정보와 토큰은 남기지 않습니다.
6. 실행 시 적용 무기/경기 설정을 읽어 HP·부위 피해·탄창·발사 간격·재장전 시간·리스폰 시간·점수 제한·시간 제한을 확정합니다. 계획 초안 값으로 실제 동작을 합격 처리하지 않습니다.
7. 필요한 관측값이 로그/UI/Inspector에 없으면 담당에게 최소 진단 추가를 요청합니다. 실제 Runner state/authority/HP/tick/score를 확인할 수 없는 항목은 관측 부족으로 남깁니다.

[Photon 지역 문서](https://doc.photonengine.com/fusion/v2/manual/connection-and-matchmaking/regions)에 따르면 지역은 서로 분리되고 지역 미지정 시 Best Region을 선택합니다. [Matchmaking 문서](https://doc.photonengine.com/fusion/v2/manual/connection-and-matchmaking/matchmaking)는 Host/Server/Client의 ClientServer 로비와 IsVisible/IsOpen의 별도 역할을 설명합니다.

## 3. 공개 방 두 클라이언트 기본 절차

아래 절차는 후속 배정에서 실행합니다. 테스트 방 이름 예시는 `SFMP-20261007-01`이며 실행마다 충돌하지 않는 순번을 사용합니다. 타인의 방에는 참가하지 않습니다.

1. A/B를 Login에서 시작해 Lobby까지 진행합니다. 화면과 로그로 각각 연결 중→성공 및 Region/AppVersion/ClientServer를 확인합니다. 인터넷 reachability 값만으로 클라우드 성공이라 하지 않습니다.
2. A가 공개 방, DesertHouse, 최대 2명을 선택해 한 번 생성합니다. 실제 IsVisible=true/IsOpen=true, StartGameResult.Ok, Host/IsServer와 정확한 SessionName을 확인합니다. 요청 시각을 남깁니다.
3. B는 실제 OnSessionListUpdated에서 이름·MapName·인원·MaxPlayers를 확인하고 UI 목록의 해당 방을 눌러 참가합니다. 직접 SessionName 주입은 공개 목록 검증을 대체하지 않습니다. 생성 성공 후 30초 안에 목록이 안 나오면 이번 정상 경로는 실패/진단 대상으로 기록합니다. 30초는 시험 관찰 기준이며 Photon 보장 시간이 아닙니다.
4. B의 StartGameResult.Ok와 Client/LocalPlayer를 확인합니다. A/B 동일 SessionName, Room 씬, 서로 다른 PlayerRef, roster 2명 및 각 플레이어 Room 객체 1개를 양쪽에서 확인합니다.
5. B가 정상 퇴장합니다. A roster가 1명으로 줄고 유령 객체/준비 상태가 남지 않음을 확인합니다. B의 로비 복귀, 새 Runner 필요 여부와 중복 callback 여부를 기록합니다.
6. B가 목록으로 재참가해 2명으로 복원합니다. 반대 팀 선택 및 준비를 하고 Host만 경기 시작을 수행합니다. 시작 전에는 한 명/미준비 상태에서 시작 불가를 별도 확인합니다.
7. 두 클라이언트가 DesertHouse에 진입하고 각 player 1개, 해당 InputAuthority, Host StateAuthority, 자기 카메라·HUD·AudioListener만 활성화되어 있는지 확인합니다.
8. 아래 전투/경기 케이스를 수행하고 양쪽 결과 화면·로비 복귀를 확인합니다. 새 이름의 방에서 재참가합니다.
9. A/B의 Host/Client 역할을 바꿔 기본 절차를 반복합니다. 정상 경로는 최초 Host 역할별 1회 이상, 퇴장/재참가는 총 3회 이상 확인합니다.

## 4. 흐름별 합격 기준과 증거

### C1 Hitbox 기반 query 검증 준비 (NOT RUN)

Refs #33, #19, #35. C1은 프리팹 등록·부위 식별·지연 보정 query의 기반만 검증합니다. C2 실제 사격/피해가 연결되기 전에는 HP 감소·헤드샷 피해·발사 권한·탄약 검증 완료를 주장하지 않습니다. 아래 준비에서는 제품 파일/씬/Editor/컴파일을 조작하지 않았습니다. C1 담당의 코드·프리팹·설정 통합 및 제어권 반환 후 새 snapshot과 신규 Player 빌드로 실행합니다.

현재 읽기 snapshot의 NetworkProjectConfig는 LagCompensation.Enabled=false입니다. 담당이 history/query를 활성화하고 해당 플랫폼에서 실제 Runner가 history를 생성하는 것을 확인하기 전에는 animated history 시험을 BLOCKED/NOT RUN으로 유지합니다. 준비 중 파일은 담당 작업에 따라 바뀔 수 있으므로 실행 직전 적용값을 다시 읽습니다.

| ID | 확인 절차 | 합격 기준 / 증거 |
| --- | --- | --- |
| H01 프리팹 등록/bake | Red/Blue의 실제 prefab GUID·Fusion prefab table ID·NetworkObject baked NetworkedBehaviours·HitboxRoot와 child Hitbox 등록 배열을 읽고 실제 spawn과 대응 | 각각 root1/부위12, null·중복 참조·등록 누락 없음. Hitbox.Root/HitboxIndex가 배열 항목과 일치하고 해당 player NetworkObject/PlayerRef로 연결. import/compile 또는 prefab table 숫자만으로 실제 등록 성공을 대신하지 않음. |
| H02 Owner/분류 | 12개 각각 hierarchy/bone path·owner NetworkId/InputAuthority·부위 enum·좌우/상하 segment·shape/offset/size/layer를 inventory로 기록. 담당의 확정된 12개 구성 계약과 비교 | Head/Torso/Arm/Leg 및 좌우 구분이 명세와 일치, 원격 hitbox가 로컬 owner를 참조하지 않음. 12개 수만 맞고 모두 Torso로 매핑되는 경우 FAIL. 실제 12개 구성의 세부 부위 수는 계약 확인 전 임의 지정하지 않음. |
| H03 broad bounds | idle·이동·회전 및 실제 연결된 애니메이션 pose에서 각 hitbox의 world geometry 최대 범위와 root broad sphere 중심/반경을 비교. 끝부분에 향한 query도 수행 | 모든 활성 부위가 broad sphere 안에 포함되어 narrow phase 후보에서 누락되지 않음. 한 idle pose의 자동 반경 계산만으로 동적 pose 전체를 PASS 처리하지 않음. 미연결 애니메이션은 NOT RUN. |
| H04 부위 query | Host에서 표적과 ray pose/tick을 관측해 Head/Torso/좌우 Arm/Leg의 노출된 면을 각각 query. 각 결과의 HitboxIndex/Owner/부위/거리/point/normal을 기록. 두 팀 역할 교환 | 가까운 실제 부위가 올바른 분류로 반환되고 무관한 owner/부위가 선택되지 않음. 겹치는 팔·몸통/경계 ray는 nearest hit 또는 확정된 우선순위로 처리하며 배열 순서만으로 임의 선택하지 않음. ray는 실제 query API 경로를 사용하되 진단 query를 실제 발사로 표현하지 않음. |
| H05 애니메이션 history | Runner tick/history 버퍼 범위를 기록한 뒤 움직이거나 애니메이션 중인 표적의 과거 pose와 현재 pose가 다른 ray를 query. shooter PlayerRef·선택된 tick/alpha와 현재 geometry 대조 query를 함께 기록 | rewind 대상 부위가 해당 과거 pose와 맞고 현재 Physics hit만 반환한 것을 history 성공으로 간주하지 않음. Subtick 옵션 유무도 구분. 버퍼 밖·생성 전·despawn 뒤 요청의 실패/제한 정책 확인. 지연·애니메이션 연결이 없으면 NOT RUN. |
| H06 CC/자기 hitbox 제외 | query mask/layer와 IncludePhysX/IgnoreInputAuthority 실제 옵션을 기록. 표적 Head/Arm으로 향하는 ray, shooter 자기 capsule 통과 ray, CC만 포함하는 대조 query | 캐릭터 이동용 CC가 먼저 맞아 부위가 가려지거나 Torso로 대체되지 않음. shooter 자기 hitbox 제외는 실제 shooter PlayerRef 기준이며 원격 적군까지 제외하지 않음. IncludePhysX는 정적 월드 포함 범위와 분리해 평가. 일반 Physics.Raycast 성공으로 Fusion Hitbox 경로를 대신하지 않음. |
| H07 월드 벽 차폐 | 같은 표적 부위에 대해 노출/벽 뒤, 표적 앞/뒤 벽, 벽 모서리 ray를 query하고 world collider·hitbox의 최단 거리를 비교 | 표적 앞 벽이 더 가까우면 벽 차폐 결과, 표적 뒤 벽은 표적 query를 가리지 않음. player 부위 layer 제외/월드 mask 누락을 따로 확인. 정적 PhysX world의 현재 pose와 rewind hitbox의 과거 pose 사용을 명시하며 움직이는 문/엄폐물은 별도 history 정책 시험으로 남김. |
| H08 lifecycle/회귀 | actual2peer 정상 생성/Ready/Start→각12부위/root1 등록, Client/Host leave 및 새 Runner 경기 재참가에서 등록 수/owner/history 참조를 확인 | 현재 ActivePlayer별 등록 일치, 유령 root·despawn hitbox·중복 history 없음. 이동·로컬 카메라/Listener·Blue seed의 이전 gate 유지. query 수행 전후 HP/Ammo가 변하지 않으면 기반 query 상태로 기록하며 피해 완료라고 표현하지 않음. |

증거 최소 필드: build manifest/HEAD/플랫폼·config, query 종류와 호출 권한, shooter/target PlayerRef/NetworkId, input/authority tick 및 history tick/alpha(관측 가능한 경우), origin/direction/range/mask/options, 결과 종류(Hitbox/월드/없음)·HitboxIndex/Owner/부위/거리/point/normal, root broad bounds, 실제 pose·활성 부위 수. 부위12 inventory와 query 표본을 분리 보존합니다. 관측값이 노출되지 않으면 단일 작성 담당에게 최소 진단을 요청하며 QA는 제품을 임의 패치하지 않습니다.

SDK 근거는 설치된 Fusion.Runtime.xml의 Hitbox.Root/HitboxIndex, HitboxRoot.Hitboxes/BroadRadius/Offset 및 HitOptions.IncludePhysX/SubtickAccuracy/IgnoreInputAuthority입니다. BroadRadius 자동 계산은 현재 pose의 대략적인 범위이므로 animated bounds 전체 검증과 구분합니다.

이동 잔여 항목(Android touch/실기기, 계단·경사, held-focuslost, 같은 팀 occupied spawn 회피)은 기존 NOT RUN 상태로 계속 추적하며 C1 합격으로 대체하지 않습니다.

아래 표는 전체 시험의 합격 기준입니다. 이번에 실행한 범위의 실제 결과는 문서 첫 부분에 기록했습니다. 그 밖의 항목은 NOT RUN이며 코드 읽기·컴파일·단일 클라이언트 smoke를 PASS로 쓰지 않습니다.

| ID | 실행 케이스 | 합격 기준 | 필수 증거 |
| --- | --- | --- | --- |
| L01 | A/B 로비 접속, 빈 목록, 재시도 | 연결 성공/실패/방 없음 구분, 로딩 종료, 중복 요청·Runner 없음 | 양쪽 LobbyStatus·LastLobbyResult/Reason·SessionList callback·화면 |
| L02 | 공개 생성→목록→참가 | 동일 앱/kr/AppVersion/ClientServer와 실제 방 1개, B 목록 노출·참가 성공 | Host/Client StartGameResult, 목록 snapshot, SessionInfo |
| L03 | 퇴장→재참가 3회 | roster/객체 정확히 증감, 유령 플레이어·중복 이벤트·멈춘 로딩 없음 | PlayerJoined/Left/Shutdown 순서, Runner 수/PlayerRef 전후 |
| R01 | 1명·미준비·팀 불균형·Client 시작 시도 | 확정 규칙 위반 시작 차단, Host만 정상 시작, 준비/팀이 양쪽 일치 | 양쪽 roster/team/ready, 시작 요청과 호스트 판정 |
| R02 | 동시 경기 씬 진입 | 동일 맵·경기 instance, player 각 1개, 자기 입력·카메라, 올바른 팀 스폰 | NetworkId/PlayerRef/authority, 양쪽 씬·화면 |
| C01 | A 이동/회전/달리기/점프 후 B 반복 | 원격 이동·시점 반영, 자기 입력이 원격 player에 적용되지 않음, 엄폐/지면 통과 없음 | 양쪽 영상, 위치/회전 tick 표본, 입력 주체 |
| C02 | 정지 표적 몸통·머리·왼/오른팔·왼/오른다리 단발, 역할 교환 | 실제 적용 피해와 Host HP 감소가 정확히 일치, 양쪽 최종 HP 일치, 머리만 헤드샷 피드백 | shooter/target/shot 또는 tick/부위/전후 HP·피해량, HUD |
| C03 | 연속 발사·트리거 반복·빈 탄창 | Host가 발사 간격/탄약을 검증, 탄약 음수·허용량 초과 피해 없음, 반동이 확정 설정대로 발생·회복 | Host 승인 shot tick/탄약, 시점 영상, 설정 |
| C04 | 일부 탄약·빈 탄창 재장전 및 도중 발사 | 재장전 시간·탄약 확정 규칙 일치, 중복 재장전/도중 발사·피해 없음 | 양쪽 ammo/reload 상태 및 Host shot 승인 기록 |
| C05 | 벽 뒤 표적·벽 앞 표적 | 벽에 가린 표적 HP 변화 없음, 노출 표적만 유효 피해 | 표적 배치 양쪽 화면, ray/hit 판정·HP |
| C06 | 치명타·연속 중복 명중·사망 중 발사·리스폰 | 사망/점수는 1회, 사망 중 입력·발사 차단, 정해진 지연 후 팀 스폰, HP/탄약/상태 초기화 | Host death/kill/respawn tick·NetworkId, 양쪽 score·화면 |
| C07 | 아군 피해 | 팀킬 꺼짐일 때 아군 HP/점수 불변 | team·Host hit/HP/score. 정상 1대1 적군 테스트로 대체 불가 |
| M01 | 점수 제한 도달 | 확정 목표점수에서 한 번 종료, 양쪽 승패·최종 점수 동일, 종료 후 피해/점수 증가 없음 | Host end reason/tick, A/B 최종 scoreboard·result |
| M02 | 시간 종료·동점 | 실제 타이머 만료, 점수 우세 승패와 동점 각각 정확, 양쪽 최종 결과 동일 | 시작/종료 tick·시간·점수·result, 전체 경과 기록 |
| M03 | 결과→로비→새 방→새 경기 | 잔여 player/카메라/점수/타이머·Runner 없음, 초기 상태로 다시 플레이 가능 | 양쪽 씬·Runner/roster 전후, 새 SessionName·경기 ID |
| F01 | 방 생성/참가 버튼 연속 클릭, 방 종료와 참가 경쟁, 만원 | 요청 수렴 또는 명시 실패, 중복 방/객체·무한 로딩 없음 | 요청 순서/결과·ShutdownReason·UI |
| F02 | 경기 중 신규 참가 시도 | 목록 숨김과 별개로 실제 참가 거절, 기존 경기 상태 보존 | IsOpen/IsVisible, 추가 peer Join 결과·기존 roster |
| F03 | Client 정상 종료/프로세스 종료, Host 정상 이탈/종료 | 확정 정책대로 roster 정리, Host 이탈 시 종료 사유·로비 복귀, 호스트 이전을 기대하지 않음 | 살아있는 peer 로그·화면·ShutdownReason·복귀 시간 |
| F04 | 테스트 클라이언트만 연결 끊김·복구, Android 일시 정지/복귀 | 무한 로딩·입력 고착 없음, 실패 사유·재시도 또는 종료 정책 일치, 새 방 참가 가능 | 끊김/복귀 시각·tick·callback·입력 상태 |
| A01 | Android 두 기기 한 판·재참가·10분 | 실제 터치 이동+시점+발사/재장전 가능, 동기화·결과 일치, 크래시/ANR 없음 | 기기/OS/설치본 hash·양쪽 logcat·영상·성능 측정 |
| A02 | Play 테스트 설치/업데이트 두 기기 | Play 설치본 PGS 로그인, 두 명 경기·로비·재참가 및 지정 다음 versionCode 업데이트 성공 | 트랙/버전·설치 경로·로그인 결과·경기 증거 |

C02는 각 부위 최소 3회·Host/Client 공격 역할 양쪽 수행하며 비치명타 시험 전 표적 HP를 정상 리스폰으로 초기화합니다. HP 100/머리100/몸통25/팔다리18은 현재 계획의 초안입니다. 실제 설정이 같다면 단발 HP 0/75/82, 몸통 4발·팔다리 6발 치명타를 확인하며, 수치가 바뀌면 기대값을 실제 설정으로 계산합니다.

C07은 정상 팀전 최소 인원을 유지하는 4클라이언트 시험을 추가 배정하는 것이 권장됩니다. 제품 UI가 2명 같은 팀에서 경기 시작을 허용하지 않으면 임의 코드/팀 주입으로 우회하지 않고 추가 인원 필요로 기록합니다. F01 만원/F02 신규 참가도 세 번째 독립 peer가 필요합니다. 2 peer만 있으면 해당 케이스를 합격 처리하지 않습니다.

M02는 우세 점수로 시간 만료, 0:0 동점 만료의 별도 경기를 수행합니다. 실제 5분 설정이면 실제 5분 경과를 검증합니다. 짧게 바꾼 개발 설정 시험은 보조 결과이며 최종 규칙 한 판을 대체하지 않습니다. 타이머 비교는 같은 Host tick/end tick 기준으로 수행하고 비동기 화면 캡처의 숫자 차이를 곧바로 오류로 단정하지 않습니다.

지연·손실 시험은 기본 경로 통과 후 담당의 기존 프로젝트 내 지원 수단을 배정받아 진행합니다. 적용 방향, delay/jitter/loss, 실제 RTT와 tick을 기록하고 정지/이동 표적·가림·죽음 중복·결과 동기화를 반복합니다. 처음에는 정상 환경, 이후 예시 RTT 약 100/200ms·loss 1/3%를 별도 케이스로 확정합니다. 지연 보정 허용 범위가 정해지지 않거나 현재 비활성이면 Lag Compensation 합격을 선언하지 않습니다. 시스템 전역 네트워크/방화벽 변경이나 다른 앱 연결 차단은 사용하지 않습니다.

## 5. 증거 기록과 실패 분리

실행 담당이 프로젝트 밖 전용 evidence 디렉터리를 배정받아 A/B 원본 로그·화면/영상·build report·산출물 hash를 보존합니다. 이번 조사에서는 evidence 디렉터리를 생성하지 않았습니다. PC의 실제 실행 명령/PID/로그 경로, Editor Console 및 해당 구간 Editor.log, Android의 해당 앱 logcat을 각각 기록합니다. 두 peer 로그를 하나로 섞지 않습니다.

각 케이스 기록 형식:

- 실행 ID/케이스 ID/실행 시각(KST)/운영자, A/B/C 환경·PID 또는 기기, Host 역할.
- 소스 snapshot/작업 트리 식별값, build hash/Unity/Fusion/앱 버전, 실제 앱 지문·Region·Photon AppVersion·Lobby·SessionName.
- 사전 상태·조작 순서·기대값·실제값, Host tick/NetworkId/PlayerRef/권한·양쪽 관측값.
- PASS/FAIL/BLOCKED/NOT RUN, 증거 경로·영상 시각, 재현 횟수, 담당에게 필요한 수정 범위.

| 실패 구분 | 분리할 증거와 다음 확인 |
| --- | --- |
| 실행 환경/미구현 | PC Login 정지, 빌드 없음, 미구현 HUD/전투, 필요한 기기 없음은 BLOCKED로 구분합니다. 네트워크 실패로 기록하지 않습니다. |
| Google 로그인 | PGS 인증 결과·로그인 화면·설치본 종류를 먼저 확인합니다. Lobby 도달 전 실패를 Photon 실패로 오인하지 않습니다. |
| Photon 접속/매칭 | JoinSessionLobby/StartGame 결과·ShutdownReason, 실제 AppVersion/Region/Lobby, 방 visibility/open/capacity를 비교합니다. 계정 설정을 바꾸지 않습니다. |
| 목록 UI | 실제 SessionList callback에는 방이 있는데 UI만 없으면 UI 문제, callback도 없으면 매칭/연결/필터 문제부터 확인합니다. |
| Runner 수명 | 첫 참가 성공 후 두 번째만 실패하면 Shutdown 완료·Runner 재생성·callback 중복·로비 재접속을 확인합니다. 종료된 Runner의 재사용을 정상으로 가정하지 않습니다. |
| 씬/스폰 | 세션 참가 성공과 map 진입 실패를 분리합니다. build scene 목록·SceneRef·LoadDone·prefab NetworkId·roster·authority를 확인합니다. |
| 입력/포커스 | Host tick이 계속 증가하는데 창 전환 후 조작이 안 되면 local focus/input을 확인합니다. 원격 입력과 연결 실패를 구분합니다. |
| 피해/동기화 | Host 판정 자체 오류와 Host는 맞지만 Client state/HUD가 다른 경우를 분리합니다. shot/tick/부위/authority/HP 근거를 함께 제출합니다. |
| 경기/UI | Host score/end state 오류, replication 오류, 최종 state는 맞지만 결과 표시 오류를 각각 구분합니다. |
| 근거 부족 | 권한·피해·부위·종료 중복을 관측할 방법이 없으면 증거 부족입니다. 화면상 그럴듯함으로 PASS를 작성하지 않습니다. |

정상 경로의 새 Error/Exception/Assert는 원인 확인 전 통과하지 않습니다. 의도한 실패 케이스의 예상 종료 로그는 정상 케이스와 분리합니다. 재실행 전에 실패 로그를 보존하고, 수정 담당이 인계한 새 snapshot/build로 실패 케이스와 영향받는 기본 흐름만 다시 검증합니다.

## 6. 현재 상태와 다음 배정 제안

- [x] 기존 계획/Android checklist 및 제품·SDK 실행 경로 읽기 조사
- [x] 이 문서 작성
- [ ] 총괄의 실행 수단·Editor 제어권·방 생성/프로세스 실행 배정
- [ ] PC 로그인/입력 개발 경로와 첫 Windows build 또는 검증 가능한 APK 인계
- [ ] 두 peer의 실제 kr/AppVersion/ClientServer 일치 확인
- [ ] L01~L03 실제 두 클라이언트 공개 방 검증
- [ ] R/C/M/F 흐름 및 추가 peer가 필요한 케이스 검증
- [ ] Android 두 실기기 및 Play 설치본 최종 검증

### 추가 확인 기록 — 단일 호스트 방 수명주기

2026-10-07 12:22–12:23 KST, 네트워크 담당이 실제 Editor에서 고유 공개 진단 방 2개를 순차 생성해 Room 진입→퇴장→새 Runner/manager 로비 복귀를 확인했습니다. QA 담당은 전달받은 원본 `C:\Users\pc_17\Documents\ChatGPT\sudden-force-fps\network-room-retry-validation-20261007.jsonl`의 Room1/LobbyAfterRoom1/Room2/LobbyAfterRoom2 기록을 읽어 확인했습니다.

- 첫 방: 실제 Host/Room, IsVisible=true/IsOpen=true, MaxPlayers=2, team capacity=1, DesertHouse, RoomPlayers=1. 유효 팀 변경·팀7 거절·원래 팀 복귀·단독 호스트 시작 거절을 확인했습니다.
- 두 번째 방: StartGame 결과 Ok, LastRoomError 빈 값, 실제 local room NetworkObject와 GetPlayerObject(LocalPlayer) 일치, 단독 호스트 준비/시작 불가를 확인했습니다.
- 각 퇴장 후 manager -47036→-48118→-49190으로 새 instance, Lobby Connected/list received, manager 1개·roster 0·Room unload·닉네임 유지. 최종 목록에서 두 진단 방이 없음을 확인했습니다.
- 중복 create/return pending Task 동일, 새 runtime errors=[] 및 최종 Playing=false/Untitled dirty=false/autotick=false 복원은 네트워크 담당의 추가 보고입니다. QA는 Editor를 직접 조작하지 않았습니다.

**한 peer의 방 생성/복귀만 확인한 보조 결과입니다. L02의 두 클라이언트 공개 목록 참가, L03의 두 peer 재참가, 양팀 준비/경기 진입 및 전투 합격을 의미하지 않습니다.** 실제 Windows 빌드와 두 프로세스 시험은 총괄의 다음 배정을 기다립니다.

권장 첫 배정은 네트워크 담당의 로비/Runner 수명 수정 완료 후 **Editor + Windows build의 L01~L03**입니다. PC 준비가 Android보다 늦으면 **Editor + APK**로 대체합니다. 현재 SDK Multi-Peer 가능성을 확인한 사실은 실제 두 클라이언트 완료 증거가 아니며, 실행 전 선행 조건과 미실행 항목을 그대로 유지합니다.

## 부록: Windows64 개발 빌드와 두 프로세스 실행

### 빌드 함수 계약

신규 `Assets/Scripts/Editor/MultiplayerValidationBuild.cs`는 자동 실행 callback/MenuItem이 없는 명시적 함수입니다. 기존 BuildScript를 호출하지 않습니다. 자동 target 전환, PlayerSettings/define/버전/서명/Fusion setter, 씬 저장, Player 실행을 포함하지 않습니다.

- active target=StandaloneWindows64, PlayMode=false, import/compile/build 완료, Windows64 support 존재를 확인합니다. target 전환은 배정받은 Editor 담당이 먼저 수행합니다.
- `EditorBuildSettings.scenes`의 enabled 씬을 순서대로 사용하고 빈 목록/누락 파일을 거절합니다. Development + CompressWithLz4 + StrictMode이며 AutoRunPlayer/ConnectWithProfiler/AllowDebugging은 추가하지 않습니다.
- 허용 root와 새 output은 절대 로컬 Windows 경로입니다. root는 프로젝트 내부/상위/드라이브 루트를 거절하며 output은 root 자식만 허용합니다. 기존 output·junction/symlink·alternate stream은 거절합니다. 덮어쓰기/삭제는 하지 않습니다.
- 성공 조건: BuildResult.Succeeded + totalErrors=0 + exe 존재 + 전체 Player 파일 해시와 `validation-build.json` 기록 완료. manifest에는 UTC 시간, Unity/Player version, 씬 목록, 결과/에러·경고 수, 상대경로/SHA-256을 기록합니다. 소스 snapshot/diff 식별값은 증거에 따로 연결합니다.
- build/hash 실패 시 success=false와 안전한 예외 타입을 기록합니다. preflight 또는 디스크 쓰기 실패 시 manifest가 없을 수 있습니다. 강제 종료 후 Running 상태/manifest 없음은 성공이 아닙니다.
- CLI 함수는 batch만 허용하고 `-sfValidationRoot`, `-sfValidationOutput`, `-logFile`을 각각 한 번 요구합니다. log도 root 아래/output 밖이어야 합니다. 성공 Exit(0), 실패 Exit(1)입니다. 로그 경로 검사는 함수 진입 후이므로 launcher가 시작 전에 경로를 확인해야 합니다.
- live 함수는 실패를 throw하고 Editor를 닫지 않습니다. batch 함수를 live Editor에서 호출하지 않습니다. 기존 Editor 원본 로그는 기존 위치에 유지하고 시험 구간 사본을 증거 폴더에 보존합니다.
- App ID/키/서명/Photon 설정과 임의 SDK 예외 메시지를 직접 출력하지 않습니다. Unity/SDK의 자체 원본 로그는 로컬 보존하고 외부 전달 시 App ID·토큰·계정 식별값을 검토합니다.

네트워크 담당이 대상 Unity에서 실제 refresh/recompile을 수행해 `recompile_status=completed`, `failed=false`, `errors=[]`, `compilationFailed=false`를 확인하고 전달했습니다. 초기 manifest 오류·경고 필드의 `uint` 선언으로 발생했던 CS0266 두 건은 실제 API 반환값에 맞춰 `int`로 수정했습니다. QA 담당은 Editor를 직접 조작하지 않았습니다. Windows build 함수 실행·산출물·두 클라이언트 동작은 **미검증**입니다. 빌드/import/cache/target 전환 영향을 실행 담당이 diff로 확인하고 기존 변경을 보존합니다.

### 제어권 인계 후 사용 예시 — 현재 실행하지 않음

소유 중 Editor라면 target 전환·컴파일 완료 후 Pipeline의 실제 eval 명령을 확인해 다음 C#를 한 번 호출합니다. 동기 build이므로 CLI timeout만 보고 중복 호출하지 않고 실제 build 상태부터 확인합니다.

```csharp
MultiplayerValidationBuild.BuildWindows64(
    @"D:\meee\git\sudden-force-fps-validation",
    @"D:\meee\git\sudden-force-fps-validation\pc-20261007-01");
```

전용 batch는 기존 프로젝트 Editor를 정상 종료한 뒤 사용합니다. 같은 프로젝트의 Editor와 병행하지 않습니다. Unity CLI의 현재 버전·업데이트 확인·`run --help`를 실행 배정 후 확인하며 임의 설치/업데이트는 하지 않습니다. 허용 root가 프로젝트/상위와 겹치지 않고 reparse point가 없는지 먼저 확인하고 log의 부모 폴더를 준비합니다.

```powershell
unity run 'D:\meee\git\sudden-force-fps' --editor-version 6000.3.25f1 --timeout 1800 --no-tail -- -buildTarget StandaloneWindows64 -executeMethod MultiplayerValidationBuild.BuildFromCommandLine -sfValidationRoot 'D:\meee\git\sudden-force-fps-validation' -sfValidationOutput 'D:\meee\git\sudden-force-fps-validation\pc-20261007-01' -logFile 'D:\meee\git\sudden-force-fps-validation\build-20261007-01.log'
```

`unity run`이 batchmode/quit/projectPath를 관리하므로 `--` 뒤에 중복 전달하지 않습니다. timeout은 첫 측정 후 조정합니다. `-nographics`는 그래픽 의존 빌드 영향 확인 전 사용하지 않습니다. CLI exit code와 manifest.success/result/errors 및 파일 hash를 함께 확인합니다. CLI가 해당 기능을 지원하지 않으면 설치 대신 총괄에게 정확한 가용 실행 경로를 보고합니다. Unity.exe 직접 실행을 배정받았다면 같은 executeMethod/validation 인수와 명시적인 batchmode/projectPath/buildTarget/logFile을 사용합니다.

두 게임 프로세스는 성공한 **동일 build 폴더 전체**를 사용합니다. exe만 복사하지 않습니다. 아래는 실제 실행 배정 이후의 예시입니다. 로그 부모 폴더는 새 증거 디렉터리로 미리 준비하고 같은 경로가 있으면 중단해 새 순번을 배정합니다.

```powershell
$playerDirectory = 'D:\meee\git\sudden-force-fps-validation\pc-20261007-01'
$playerExe = "$playerDirectory\SuddenForceFPS.Validation.exe"
$playerA = Start-Process -FilePath $playerExe -WorkingDirectory $playerDirectory -ArgumentList '-screen-fullscreen 0 -screen-width 960 -screen-height 540 -logFile "D:\meee\git\sudden-force-fps-validation\run-20261007-01\player-A.log"' -WindowStyle Normal -PassThru
$playerB = Start-Process -FilePath $playerExe -WorkingDirectory $playerDirectory -ArgumentList '-screen-fullscreen 0 -screen-width 960 -screen-height 540 -logFile "D:\meee\git\sudden-force-fps-validation\run-20261007-01\player-B.log"' -WindowStyle Normal -PassThru
```

게임 창은 실제 사용자가 UI를 조작하는 도구이므로 표시합니다. 각 반환 객체의 PID·실행 경로·시작 시각·로그 경로·build manifest 지문을 증거에 저장하고 창을 A/B 닉네임과 연결합니다. A는 공개 생성, B는 목록 참가를 실제 UI로 수행합니다. 위 인수에는 자동 Host/Join/nickname 기능이 없으며 있다고 가정하지 않습니다. 별도 프로세스도 같은 Windows 계정의 PlayerPrefs를 공유할 수 있으므로 제품 UI에서 닉네임을 각각 지정하고 재시작 시 확인합니다.

한 PC에서는 각 창에 순서대로 포커스·입력을 전달하며 비활성 peer의 tick이 계속 증가하는지 확인합니다. 동시에 움직이며 사격하는 시험은 두 운영자/두 기기로 보완합니다. 해당 A/B 로그의 새 줄과 각 PID 종료 여부/최종 exit code를 수집합니다. 정상 종료는 제품 퇴장/닫기 흐름을 사용하며 강제 종료 케이스는 저장한 프로세스 객체/PID·경로·시작 시각을 다시 확인해 해당 peer만 종료합니다. 이름으로 모든 Unity/게임 프로세스를 종료하거나 증거 폴더를 삭제하지 않습니다.

[Unity 6.3 BuildPipeline.BuildPlayer](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/BuildPipeline.BuildPlayer.html), [BuildOptions](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/BuildOptions.html), [Player 명령행 인수](https://docs.unity3d.com/6000.3/Documentation/Manual/PlayerCommandLineArguments.html)를 확인해 설계했습니다. 특히 먼저 build target 전환·컴파일이 완료되어 플랫폼 define이 맞도록 제한했습니다.

실행 전 ProjectSettings 백업과 종료 파일의 차이는 Unity 빌드가 생성한 Standalone batching 직렬화 항목(m_StaticBatching=1, m_DynamicBatching=0)입니다. 원 define은 복원됐으며 해당 자동 직렬화 차이는 임의 덮어쓰기하지 않고 총괄에 인계합니다.

전체 tracked diff block 비교 추가 확인: 빌드 전후 차이 파일은 ProjectSettings.asset, BurstAotSettings_StandaloneWindows.json, URP-Performant.asset, URP-Balanced.asset, URP-HighFidelity.asset, UniversalRenderPipelineGlobalSettings.asset 총 6개입니다. URP shader prefilter/일부 SSAO prefilter 값, global settings의 build list/resource 직렬화 및 Burst schema Version3→5가 Unity 빌드 과정에서 저장됐습니다. 직접 Renderer/RenderGraph 변경은 하지 않았지만 자동 asset 변경은 존재합니다. 기존 사용자 변경을 보존하며 이 자동차이들을 임의 덮어쓰기하지 않고 총괄에 인계합니다. 자세한 비교 파일: tracked-diff-block-changes.json/source-before.patch/source-after.patch.

## 이동·로컬 카메라 검증 표준 (실제 실행 범위는 문서 첫 부분 참고)

Editor 제어권을 다시 배정받은 뒤 PC↔PC를 먼저 수행하고, PC↔Android에서 양쪽 Host 역할을 교환합니다. 같은 통합 소스 snapshot의 manifest·플랫폼 define·설정 차이를 기록합니다. 이번 helper/증거 inventory는 그대로 보존하며 제품 코드·씬·컴파일 영향 파일은 이 준비 단계에서 수정하지 않습니다. 각 항목은 실제 UI/입력으로 수행하고 로그·권한·위치/tick 관측과 양쪽 화면을 함께 남깁니다. Android 설치·실행과 생명주기 조작은 담당 배정 후 진행합니다.

| ID | 절차 | 합격 기준 / 필요한 증거 |
| --- | --- | --- |
| P01 로컬 카메라 | Red/Blue를 각각 로컬 역할로 시작, 양쪽 camera root·player·InputAuthority를 연결. 원격 플레이어가 움직이고 회전할 때 로컬 화면을 관찰 | peer당 gameplay Camera 1개와 AudioListener 1개만 활성. 원격 Camera/Listener는 inactive 또는 enabled=false. HUD/시점 대상은 로컬 PlayerRef이며 원격 입력으로 자기 시점이 전환되지 않음. network authority와 활성 객체 목록·양쪽 화면 필요. 별도 UI camera는 설계가 명시된 경우 분리 기록. |
| P02 Blue 초기 yaw | Blue spawn 직후 입력 전 pose.rotation, network yaw, camera yaw/forward와 선택된 spawn Transform의 forward를 표본. Red도 동일 비교 | Blue가 지정된 spawn 방향을 보며 첫 tick/첫 Render에서 yaw가 0으로 덮이지 않음. 양쪽 복제 rotation과 카메라 방향이 일치. 180도 고정은 규칙이 확인된 경우에만 기대값으로 사용. |
| P03 포커스 상실 | PC 이동·마우스 회전·달리기/점프 입력을 유지한 채 다른 창으로 전환, 비활성 중 키 해제 후 복귀. Android joystick/look pointer를 유지한 채 홈/화면 잠금·복귀 | focus/pause 경계에서 이동·look delta·버튼·pointer 상태 reset. 복귀 후 입력 없이 이동/회전/점프 반복 없음, 새로운 실제 입력에는 정상 반응. 비활성 Runner tick 진행/연결 유지 정책과 카메라 cursor lock 복원도 기록. 연결이 종료되면 로비 회복 경로를 별도 판정. |
| P04 아날로그/예측 | joystick 0·작은·중간·최대 입력 및 대각선, PC 동일 방향 입력을 각각 일정 tick 구간으로 반복. Host/Client 역할 교환 | dead zone 밖 입력 크기에 따른 속도 변화가 명세와 일치, magnitude가 의도치 않게 1로 정규화되지 않음. 대각선 과속 없음. 자기 입력은 자기 player에만 적용. Host 권위 위치와 Client 예측 위치/보정·remote 보간을 같은 tick으로 비교. 지연/손실 시험은 소유권 배정 및 합의한 조건으로만 수행. |
| P05 CharacterController 벽 | 정면/비스듬한 벽 접근, 모서리 이동, 좁은 통로, 머리 위 낮은 천장 아래 점프를 양쪽 역할로 수행 | 벽/바닥/천장 관통과 지속 침투 없음. 벽을 따라 이동 시 불필요한 정지·튕김·누적 보정 없음. capsule radius/height/skinWidth·collision flags와 충돌면 근처 위치 표본 필요. 단순 화면으로 충돌 판정을 확정하지 않음. |
| P06 계단/경사 | 실제 CC stepOffset/slopeLimit와 geometry를 읽은 뒤 허용/초과 계단·경사, 올라가기/내려가기·정지·점프 착지를 시험 | 허용 높이/경사 통과, 초과 면 차단. grounded 상태·중력/수직 속도 전환 정상, 계단 떨림/낙하·경사에서 무한 상승 없음. 해당 지형이 맵에 없으면 NOT RUN으로 남기고 제품 씬을 QA가 임의 추가하지 않음. |
| P07 스폰 중복 | 정상 Ready/Start, 경기 진입 callback 반복 가능 경계, leave/rejoin·새 경기에서 ActivePlayers와 network player 목록 대응을 확인 | 활성 PlayerRef별 valid game NetworkObject 1개, Host spawn dictionary/roster 일치, Client에도 같은 NetworkId. RoomPlayer 잔존·유령 player·중복 spawn 없음. 재참가 PlayerRef 변경은 새 참가자로 기록. 강제 SpawnGamePlayer 호출은 사용자 시작 경로 검증을 대체하지 않음. |
| P08 스폰 겹침 회피 | spawn 후보 pose와 capsule 범위를 관측. 같은 팀 2명 이상을 실제 추가 peer로 시작하거나 승인된 다인 시험에서 반복 | 살아 있는 player/벽과 capsule이 겹치지 않는 사용 가능한 pose 선택. 점유 spawn의 재선택·안전 fallback 정책 확인. 1vs1 두 팀의 최초 스폰 성공만으로 같은 팀 점유 회피를 PASS 처리하지 않음. 추가 peer 없으면 NOT RUN. |
| P09 종료 회귀 | 이동 중 Client leave, Host leave, focus 복귀 이후 정상 로비/재참가·다음 경기 | 새 Runner, 빈 roster/input cache, 중복 callback 없음. 다음 경기 카메라/Listener도 다시 각 1개. stage1 공개방/Ready/각1spawn gate가 유지됨. |

값이 확정되지 않은 속도·회전 감도·허용 오차는 첫 통합 snapshot에서 담당 명세와 실제 적용값을 기록한 뒤 판정합니다. 정상 입력 테스트로 서버 권한·cheat 검증까지 합격했다고 주장하지 않습니다. 진단 수단이 부족하면 담당에게 최소 관측 추가를 요청하고 상태를 관측 부족으로 남깁니다.

### 빌드 자동 저장 6파일의 분류 근거

동결 이전 사용자 업그레이드 변경은 `source-before.patch`, QA 종료 시점은 `source-after.patch`에 보존했습니다. HEAD 대비 전체 diff가 아니라 두 patch의 동일 파일 block 차이만 QA 중 추가차이로 분리했습니다. 현재 Android 담당이 Editor를 소유하므로 이 판정은 저장된 종료 snapshot 기준이며 후속 변경은 포함하지 않습니다.

- URP 3개 asset 추가차이는 `m_Prefilter*`만입니다. SDK `UniversalRenderPipelineAssetPrefiltering.cs`는 SSAO 필드에 `ShaderKeywordFilter.RemoveIf`를 지정하고 `UpdateShaderKeywordPrefiltering`에서 strip 플래그로 갱신합니다. `Editor/ShaderBuildPreprocessor.cs:499` 이후는 해당 값을 갱신·SetDirty·SaveAssetIfDirty합니다. 따라서 Balanced SSAO 차이는 RendererFeature의 SSAO 품질/활성 설정 변경이 아니라 **빌드 shader variant 제거 규칙** 변경입니다. 빌드 포함 variant에는 영향을 줄 수 있으므로 기능적으로 무의미한 차이라고 단정하지 않습니다.
- GlobalSettings 차이는 `m_Settings.m_RuntimeSettings.m_List`의 채움과 Renderer2DResources의 `m_FallOffLookup` 참조 채움입니다. Core `RenderPipelineGraphicsSettingsContainer.cs`의 OnBeforeSerialize/build stripping 경로가 runtime 목록을 채웁니다. FallOffLookup은 SDK에서 URP_COMPATIBILITY_MODE 조건부 필드로 선언한 2D lighting resource입니다. 임시 빌드 define 경로에서 리소스 참조가 채워진 것으로 해석하며 단순 목록 cache와 구분합니다. 저장된 두 patch 비교에 RenderGraph/compatibility flag·RendererDataList 변경은 없습니다. 이전 업그레이드 snapshot에 있던 `m_EnableRenderGraph=0`/`m_EnableRenderCompatibilityMode=1`을 QA가 바꾼 것으로 귀속하지 않습니다.
- ProjectSettings 추가차이는 Standalone batching 항목 직렬화이며 원 define의 임시 추가는 복원됐습니다. Burst 추가차이는 schema Version3→5와 신설/폐지 옵션 직렬화입니다. Burst 컴파일 결과 영향은 별도 담당 확인 대상이며 rendering/SSAO 설정 변경과 구분합니다.

자동차이 rollback은 하지 않았습니다. 원 사용자 변경과 검증 빌드 증거를 보존하며 플랫폼별 다음 빌드에서 prefilter가 다시 계산될 수 있음을 후속 비교 기준으로 남깁니다.

단계2 종료 정리 실제확인: Player PID2176 CloseMainWindow 정상 shutdown 후 프로세스/창 소멸(첫10초 wait는 timeout이었고 이후 정상종료확인, 강제kill없음). EditorPlayfalse/compilingfalse/updatingfalse/빈단일씬 path빈값 dirtyfalse/StandaloneWindows64/autotickfalse, 원Standalonedefine live+disk동일, recompile completed failedfalse errors[]. 실제게임 최종console groundTruth errors0/warnings2, 과거Pipeline timeout이력은 분리. 이번 빌드 자동저장추가차이는 URP-Balanced의 SSAO shader prefilter4값과 GlobalSettings runtime목록 rid4338206851167682583 제외입니다. RenderGraph/compatflag 직접변경없음. 기존importtrace 일부가Editor자동재생성/refresh후clean으로보이므로 시작status/patch와 종료patch를 함께 보존하며 임의rollback없음.


### persistent 복귀 수정 후 실제 두 피어 재검증 (2026-10-07)

이전 N01–N08 실패 기록은 당시 HEAD의 실제 결과로 보존합니다. 아래는 수정 HEAD 1b967a0ea19afe4e7dc0e06b1fd4b58a28d3b378에서 새로 실행한 후기이며, 이전 결과를 소급 변경하지 않습니다. 증거 경로: D:\meee\git\sudden-force-fps-validation\20261007-persistent-actual-01.

| 항목 | 결과 | 실제 관측과 한계 |
|---|---|---|
| P01 새 Windows 실행본 | PASS | Unity6000.3.25f1/Win64 Development build_81b26ae949b8 Succeeded/25.975초/398743220bytes/errors0/warnings2. 전체380파일 staged=active, 기존active380파일 전체백업 SHA동일. build-all-files.json SHA256 9D02CF99085C165AD15C3E34CAE18B5E948530BF3BD7FEFE1D8369AB6A354FE9. 빌드 전 임시Standalone URP_COMPATIBILITY_MODE는 빌드 후 원래define로 복원했습니다. 경고는 Unity projectID 미연결 및 Player Pipeline config없음이며 Player 내부 객체 관측 대신 실제화면·로그/Editor 복제를 사용했습니다. |
| P02 Editor Host 공개 복귀 → Player Client 자동 로비 | PASS | 공개방 PR-Editor-0944에서 두 피어 Running/connected2를 확인한 뒤 Editor public ReturnToLobbyAsync 1회. 같은 Player PID86640(재실행 없이) DisconnectedByPluginLogic→ClearingSession/LoadingLobby/CreatingRunner→generation2 Completed→fresh Lobby Connected Region kr/sessionlist0 로그와 'No public rooms available.' 실제화면 확인. Editor persistent -55090/gen1→2, oldNRM-58034/Runner-58036 callbacks1→0/둘다destroyed, fresh-61528/-61530 Runner1/Connected, static input/match/presentation/local null/player0. |
| P03 Player Host native Alt+F4 → Editor Client 자동 로비 | PASS | P02로 복귀한 같은 Player가 새 공개방 PR-PlayerHost-0945 생성(실제 SessionName은 TMP U+200B 포함), fresh Editor가 Join/Ready, Player nativeStart로 두피어 Running. PID86640 nativeAltF4 종료/gone true. 프레임8662까지 oldNRM/Runner 생존, 8663(09:45:48.188Z) old-61528/-61530 둘다destroyed/callback0/current0/Runner0/Managers1/persistent-55090 유지, gen2/LoadingLobby/RecoveryTask WaitingForActivation/input&matchnull/player0. 8665 fresh-65322/-65324/Runner1/gen3/Completed/taskRanToCompletion, 8666 Connecting/loadingtrue→8701 Connected/loadingfalse. 실제 Lobby화면 확인. WaitingForSdkRunnerDestruction은 프레임 샘플에 포착되지 않았고 동일프레임 내부 callback/파괴/await 순서는 직접 계측하지 않았습니다. old의 파괴 후 persistent task 지속과 후속 신규세션 완료를 구분합니다. |
| P04 복귀 후 다음 공개방/경기 초기화/기본 사격 | PASS(기본범위) | P02 이후 역할교환 경기와 P03 이후 gen3 Editor의 PR-AfterAuto-0946 새 공개방/Player PID38300 Join/Ready/Running 모두 실제 진행. 각 초기화 snapshot: 양쪽 rosterconnected/HP100/Ammo30/score·KD·feed·result·respawn·deathseq0, Editor localCamera/Listener1·remote0. 역할교환 경기 PlayerHost native클릭 1발 Ammo29/Shot1/Hit0가 EditorClient에 복제됐습니다(개발메시지 닫기 클릭이 게임발사도 발생, 패널은 잔존). P03 후 다음 경기 PlayerClient 중앙 native클릭 1발 Ammo29/Shot1/Hit0를 EditorHost 복제로 확인. 명중·피해·킬·리스폰·이동·Android 전체회귀 PASS는 주장하지 않습니다. PID38300 재실행은 다음 방 상대 준비이며 P02/P03 자동복귀 성공의 대체 근거가 아닙니다. |
| P05 담당 단일세션/실패 fixture | 별도 담당 증거 | 20261007-persistent-recovery-01 HANDOFF의 compile errors[]/sharedTask/duplicateAwake 서비스ID유지/Runner-firstAwake/Connecting 및 WaitingForExplicitShutdown PlayStop취소/syntheticTimeout 실패진단을 분리 참조합니다. synthetic OnShutdown은 실제 SDKdisconnect가 아니며 이번 actual PASS 근거로 사용하지 않았습니다. timeout 구체이유는 로그·필드만 보이고 사용자UI는 'Not connected to the lobby.'인 진단 한계가 남습니다. CTS Dispose완료 및 실제 이전세션 callback 지연도착 직접시험은 NOT RUN입니다. |
| P06 로그/정리/보존 | PASS(보존) | 이번 시작cursor378 이후 Console error2(seq438/439) 모두 SDK Code104 Server has disconnected, Player P02 로그도 Code104 2건을 보존. 전체Console error0으로 표현하지 않습니다. 최종Editor 오류2/경고1(누적buffer14/103)은 이번범위와 구분. 마지막 정상Lobby복귀후 Player 두PID nativeAltF4/gone, 관측delegate2개 해제, Playfalse/pausedfalse/compilingfalse/빈singleclean씬root0/autotickfalse/persistent·NRM·input·match·presentation staticnull/Runner0. ProjectSettings.asset 및 기존URP2 dirty 원본byte 정확복원. Assets/ProjectSettings/Packages 3889파일 SHA변경0/누락0/추가0, active380 최종SHA동일, 기존gitdirty 보존, 제품추가수정·stage·commit 없음. |
