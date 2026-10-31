# 단독 작업용 B1 검증 도구 준비와 미실행 범위 (#259)

2026-10-31, 사용자의 단독 작업·다른 프로젝트와 소통하지 않는 조건에 맞춰 외부 검증 도구를 준비했습니다. 제품 입력 수정 PR261은 계속 Draft이고 #259는 OPEN입니다. 이 단독 도구의 소스 준비에서 실제 staging·새 Editor·Play·B1/L2·lease/config 발행·Queue/Enable은0입니다. 기존 제공34/release12/source612 QA와 원장196,2,34를 유지하며 Android 새35 예약·빌드·서명·업로드는0입니다.

검증 대상 P11은 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`의 기존 QA overlay입니다. 최신 main clean checkout이나 새 인트로 Windows payload가 아닙니다. P11 dirty18/공개3984와 기본 P2 dirty5·기존 실패/marker를 보존하며, 이번 소스 준비는 외부 R31 디렉터리에서만 진행했습니다. 기존 native Exit0·MVID를 새 실행의 신원으로 복사하지 않습니다.

## 확인한 좁은 범위

- 단독 Root/session/project/source 결속과 stage(native0 전제)/runtime(새 PID·100ns 시작 시각)/종료 후 cleanup의 역할을 분리했습니다. 기존 NoHardware·unknown0·source17·공격1.2초/12발·새 입력13분/정상 복귀15분·소유4파일 exact 정리 조건을 유지하는 후보입니다. 옛 다른 프로젝트 동의값을 true로 복사하지 않았고 실제 다른 프로젝트 통신도0입니다.
- guard03 최초 통제150개 중4개 거부 부족을 발견했습니다. 경로 탈출·bytes의 bool/float·PID의 bool/int 별명 문제를 수정한 guard04는 revision03 경로의 통제175개를 통과했습니다. 실제 filesystem·프로세스·Root lease를 검증한 수가 아닙니다.
- 최초 publisher fixture가 실물 Host에 없는 phase 키를 정상값으로 둔 오류를 별도 정정했습니다. 실물 Writer/Host 템플릿 키와 epoch17을 사용하는 후속에서 추가/누락 키·잘못된 양쪽 bridge root의13개 반례를 보존했고, revision03의 수정 publisher는 통제144개를 통과했습니다. 실제 Host는 phase를 Queue 인자/고정 경로로 결속하며 원래 ExactKeys로 알 수 없는 키를 거부합니다. 이를 실제 게임의 버전/phase 혼용 성공으로 일반화하지 않습니다.
- revision02의22개 source 핀은 일치했지만 Python 구문은7개 통과/Runtime16 들여쓰기1개 실패였습니다. 원본을 보존하고 revision03 새 사본에서 **Python8개 구문 컴파일만 통과**했습니다. 모듈 실행·C# 컴파일이 아닙니다.
- 기본 비활성 발행기, false proposal→검토한 Writer raw SHA→Host raw SHA의 비순환 순서, Writer metadata request→단발 typed Queue→기존 Host READY 순서를 준비했습니다. enabled caller는 baseline 첫줄만 바꾼 바이트를 별도로 검증하는 설계이며 실제 파생 파일을 발행하지 않았습니다.

revision03 전체 family source IR는 `noBlockers=false`/`runtimeSourceReviewed=false`입니다. dispatch 전에 manifest의 baseline caller와 정확 첫줄 파생 바이트를 직접 대조하는 필수 보완과 미완료 검토가 남은 원본 판정을 보존합니다. 새 불변 revision04는 이 경계를 pure 함수와 실제 dispatch 전 검사로 보완했고, 한정 정적 후속에서 해당 소스 지적의 해소를 확인했습니다. 전체 family 검토·C# 컴파일·실제 입력 수용은 여전히 미완료이며 좁은 후속 결과를 전체 실행 준비로 합산하지 않습니다.

다음은 최신 불변 후보의 전체 source 검토와 실제 P11 신원/보존 감사→staging·새 C# 컴파일/기본 비활성 확인→정상 두 피어의 B1 normal poll입니다. L2 사망/리스폰·native held lifecycle은 B1 이후 별도 scope와 새 증거로 진행합니다. 현재 `readyToRun=false`이며 사용자님이 하실 일이나 답변하실 질문은 없습니다.

근거 R31=`D:/codex/validation/sudden-force-fps/20261031-work`: `input261-next-b1-readiness01/`, `input261-solo-candidate01/revision02/`, `input261-solo-candidate01/revision03/`, `solo-family02-root-syntax-pins02.public.json`, `solo-family03-root-syntax01.public.json`, `input261-solo-guard-tests01/`, `input261-publisher-offline-tests01/`, `input261-solo-family03-source-review01/`입니다. 최신 후속 후보·수용 여부는 마감 인계 기록과 함께 확인합니다.
