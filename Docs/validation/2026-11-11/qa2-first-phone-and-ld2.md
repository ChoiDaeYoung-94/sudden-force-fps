# QA2 첫 휴대전화·LD2 실제 검증 기록

첫 휴대전화의 QA2 설치와 콜드 시작을 확인했고, phone–LD2의 첫 경기에서 양쪽 **TIME EXPIRED · DRAW 0:0** 결과와 정상 로비 복귀를 확인했습니다. 이어 휴대전화를 host로 새 방을 만들어 LD2가 참가하는 것과 순서대로 퇴장한 뒤 양쪽 로비로 돌아오는 흐름도 확인했습니다. 자기 작업 범위의 호출·정리·반환은 원래 기한 전에 완료했습니다.

총괄의 직접 화면 검수와 공개 결과 기록을 반영했습니다. 설치·콜드 시작과 경기·복귀의 독립 실제 검토를 마쳤습니다. 이전 중간 기록은 보존했습니다. QA 소스 D7·Draft #353·운영34·Play12의 기존 상태를 유지하며 새 빌드·서명은 수행하지 않았습니다.

| 실제 확인한 항목 | 결과 |
|---|---|
| 첫 휴대전화 설치 | QA2 게임·helper 설치 성공, 설치 APK 전체 파일 일치 |
| LD2 설치물 | 새 APK 읽기 결과가 휴대전화의 게임·helper와 각각 전체 해시 일치 |
| 휴대전화 환경 | native API37 · ARM64(`arm64-v8a`) · page size4096B |
| 콜드 영상 | 약19.984초,699 frame, 정수초 요청에 대응한 표본20개 보존 |
| 시작 화면 선택 표본 | 요청3–4초에서 흰 화면,6–8초에서 QuadD,14–19초에서 닉네임 창을 총괄이 확인 |

첫 경기에서는 이동·시점 조작 전후의 화면 변화, 메뉴/Resume과 두 참가자가 ONLINE인 roster를 관측했습니다. 휴대전화에서 약1.2초 Fire 유지 뒤 탄약30→19, Reload tap 뒤30 회복을 확인했습니다. 짧은 Fire tap에서는 탄약 변화를 관측하지 못했으며, 유지 Fire의 결과와 구분해 남깁니다.

| 경기·복귀 순서 | 양쪽 화면에서 확인한 결과 |
|---|---|
| 첫5분 타이머 경기 종료 | 양쪽 TIME EXPIRED · DRAW 0:0, 결과의 양팀 DONE |
| 양쪽 정상 RETURN | 첫 로비 복귀 |
| 새 방B 생성·재참가 | 휴대전화 host1명→LD2 client 참가로2명, 양쪽 참가자 표시와 READY 상태 |
| LD2 먼저 Exit | 휴대전화 host만 방에 남음 |
| 휴대전화 Exit | 양쪽 최종 로비 복귀 |

총괄의 후속 화면 검수 위치는 phone seq36(결과),39(첫 로비),48–50(새 방 참가·LD2 퇴장),52(최종 로비), LD2 seq33(결과),36(첫 로비),37·40·42(새 방 참가·READY),45(최종 로비)입니다. 새 방B에서는 Game을 시작하지 않았습니다.

| 자기 작업 범위 정리 | 최종 결과 |
|---|---|
| 호출 종료 | phone 일반52회+cleanup2회, LD2 일반45회+cleanup2회; 각각 실패0·활성 호출0·caller 종료 코드0 |
| 정리 | 양쪽 force-stop/home 모두 성공, config·lease 비활성화 및 반환 확인 |
| 실제 반환 | UTC2026-11-10 15:51:55.699 / KST2026-11-11 00:51:55.699 |
| 원래 기한 | 신규 작업 마감15:56:24.278UTC, 전체 반환16:01:24.278UTC; 원창을 보존하고 두 기한 전에 반환 |
| 실행 범위 | SDK·Editor·GUI 호출0, 새 빌드·서명0 |

측정 범위에는 한계가 있습니다. 첫 경기의 시간 만료·결과·복귀와 화면상의 이동/Fire 유지/Reload 반응을 확인했으며, 새 방B의 휴대전화 host 실제 경기는 검증하지 않았습니다. native helper instrumentation은0이고 raw 입력·정확한 release latency·전체 gait/IK·실제16KB·전체 음향 검증도 하지 않았습니다. 시작 영상은 선택 표본만 보았으므로 첫 frame이나 모든 frame의 Made with Unity 표시 유무·전체 인트로를 확정하지 않으며, 영상 timestamp를 실행 지연으로 해석하지 않습니다. 정수초 요청1초/2초의 실제 표본은2.480초/2.509초여서 균등1초 표본도 아닙니다.

근거: [최종 전체 반환 기록](D:/codex/validation/sudden-force-fps/20261111-work/root-resume01/first-phone-full-return.public.json), [경기 중간 관측 기록](D:/codex/validation/sudden-force-fps/20261111-work/root-resume01/first-phone-game-observations.partial.public.json), [설치 후 qualify summary](D:/codex/validation/sudden-force-fps/20261111-work/phone-coldstart-preparation01/actual-qualify-36e34a06d4154ac5b0dd719f83a345a8/summary.public.json), [콜드 영상 summary](D:/codex/validation/sudden-force-fps/20261111-work/phone-coldstart-preparation01/actual-coldvideo-627d9c1c76d248b39c6c72a064a351ea/summary.public.json). 중간 관측 파일의 당시 미완료 상태는 보존하고, 최종 경기·복귀·반환은 총괄의 후속 화면 검수와 최종 반환 기록으로 갱신했습니다. 전체 파일 해시·시각 메타데이터는 [기존 근거 자료](D:/codex/validation/sudden-force-fps/20261111-work/first-phone-evidence-draft01/evidence-metadata.public.json)에 분리했습니다.

현재 완료한 범위는 첫 휴대전화 설치·콜드 시작의 선택 표본, 첫 경기 종료·양쪽 복귀, 새 방 참가·순차 퇴장과 자기 범위 반환입니다. 현재 필요한 사용자 조치나 질문은 없습니다.

독립 실제 검토: [설치·콜드 시작](D:/codex/validation/sudden-force-fps/20261111-work/first-phone-install-cold-actual-review01/verification.public.json), [경기·복귀·반환](D:/codex/validation/sudden-force-fps/20261111-work/first-phone-game-actual-review01/verification.public.json). 경기 검토는 source101콜·화면51개 바이트 대조/선택23개 시각 검수 범위이며 전체 보행·입력 수용으로 확대하지 않았습니다.
