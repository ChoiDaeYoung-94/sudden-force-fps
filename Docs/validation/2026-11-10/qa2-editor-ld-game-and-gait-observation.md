# QA2 Editor–LD2 실제 경기와 자세 관측 한계

같은 D7 소스(`d7a3371296357a5ceb1b7b657d468415130e7047`)의 Editor와 LD2 QA2가 정상 방 참가·READY/START를 거쳐 DesertHouse 경기에서 5분 DRAW/TIME EXPIRED·0:0을 마쳤습니다. 양쪽 정상 RETURN 뒤 Lobby를 확인했습니다. **실제 경기 흐름과 이동 반영은 확인했으나 연속 보행·발 접지·소총 IK 수용은 미완료**입니다. [이전 Case02·03](qa2-runtime-blocks-and-lobby-check.md)의 Windows 보안창 차단과 LD1 흰 화면은 별도 실패로 보존합니다.

LD2 설치본은 [기존 키 서명 QA2](qa2-signed-ld-install-and-cold-intro.md), `com.AeDeong.SuddenForceFPS.localqa`/code2/`1.0.197-qa2`, 100,492,368B/SHA `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33`입니다. 운영34/`1.0.196`/release12와 원장196,2,34·[Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)은 유지합니다. 휴대전화·Play/Drive 업로드는0입니다.

## 실제 경기와 화면

Case04 원창은 UTC05:50:58 시작/06:10:58 새 작업 중지/06:15:58 전체 반환, nonce `3aa4117a17d54a6985ae57a6ac234d28`입니다. 최초 시각 생성 거절은 별도 보존했고 그 거절로 stage/SDK/device를 호출하지 않았습니다. 이후 이 원창은 연장하지 않았습니다.

Editor는 GPU Direct3D11의 실제 Play로 진행했으며 프레임 대기1978→1984와 met=true를 관측했습니다. QA define 컴파일의 Networked43/Weaved75와 `-localqa1` 프로토콜 literal을 조회했습니다. 이 조회는 전체 동기화 정확성·모든 생명주기 검증이 아닙니다.

정상 닉네임·CreateRoom/팀 선택 API와 LD2의 실제 터치로 서로 다른 두 PlayerRef와 Red/Blue를 구성했습니다. 정상 시작 차단 사유가null인 뒤 START를 요청했습니다. LD2 조이스틱 오른쪽 swipe 뒤 서버 측 원격 위치가 초기XZ(1.875,-13.5)에서(5.8572464,-13.66543), gait phase가0→1.1964로 변했습니다. 뒤의 일부 이동은 벽에 막혔습니다. 이 수치는 총괄의 도구 관측 요약이며 원시 SDK 응답 전체의 별도 보존이나 독립 재실행은 아닙니다.

실제 플레이어 카메라99프레임은 중앙 건물에 상대가 가려 자세 판정에 사용할 수 없었습니다. 별도 진단 카메라는 실제 네트워크 상대만 따라100프레임을 촬영했습니다. 진단 카메라가 배우의 transform·health·Networked 상태나 애니메이션 위상을 변경하지는 않았습니다. 다만 offset(-3,1.5,-1.2) 때문에 일부 카메라 위치가 벽 밖으로 나가 연속 가시성을 확보하지 못했습니다. 독립 시각검토의 f010~f014 짧은 표본에서는 병사의 다리 자세 변화와 소총이 보였지만 f015 이후에는 다시 가렸습니다. 이것으로 완전한 보행 주기·미끄러짐·발 접지·자연스러운 총기 자세·12 hitbox를 통과 처리하지 않습니다. 진단 카메라 화면을 정상 플레이어 시점의 가시성으로 표현하지 않습니다.

LD2 결과 화면은 양팀 각1명의 DRAW/TIME EXPIRED·0:0을 표시했습니다. 양쪽 RETURN 뒤 Editor의 Lobby/단일 NetworkRunnerManager·clean scene과 LD2 Lobby 화면을 확인하고 Play를 종료했습니다. QA2 sprint/fire/reload/death/respawn·두 손가락 입력과 반대 authority/proxy 역할은 이번 단위에서 검증하지 않았습니다.

## 종료·원본 보존

clean Untitled와 기존 runInBackground=true를 되돌린 뒤 정상 Editor Exit(0)을 요청했습니다. CLI는 연결 종료 중 invalid response로 Exit6을 반환했으므로 CLI 성공으로 기록하지 않습니다. 별도 보유 native 핸들은06:09:21.2097564 UTC Exit0/not forced, stdout/stderr EOF·disposeErrors0을 확인했습니다. 전체 unknown child 종료의 독립 증명은false입니다.

LD caller는33개 일반 호출 뒤 force-stop/HOME 정리를 반환했고 caller native Exit0·sourceActiveCalls0을 기록했습니다. 이 호출 수를 기능 시험33개 통과로 세지 않습니다. 두 Editor ADB 설정은0→원래1로 복원했고 공유 ADB46652의 연결 peer·birth·exe SHA가 유지됐습니다. listener ownership 전체 증명은false이며 daemon lifecycle 명령은0입니다.

소유 csc.rsp/meta 두 파일은 외부 바이트 보존 후 같은 Windows 파일 핸들로 회수했습니다. 그 시점 공개4,095개 감사에는 EDM 설정1개가 달라 publicExpectedRowsExact=false였습니다. 총괄이 자동 생성된 세 값과 서식 변경을 외부에 보존·검토하고 정확 baseline으로 복원한 뒤, **06:10:30.998378 UTC 최종 감사에서 공개4,095개 raw exact·private6 absent/indexS·D7 Git clean·임시2 부재**를 확인했습니다. 승인 binding/config/lease는 원 full 전에 회수했습니다. 소유2 회수 성공과 후속 전체 감사 결과를 분리합니다.

## 다음 검증

벽 안쪽에서 연속 가시성을 확보하는 카메라로 짧은 새 정상 경기를 진행하고, idle→walk/run·전후좌우·정지 전환과 소총/양손 자세를 확인합니다. 이어 실제 사격·장전·사망/리스폰·입력 해제 회귀와 반대 역할을 검증해야 합니다. [이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341), Draft353 및 의존 입력/소총 변경은 이 부분 결과만으로 닫거나 병합하지 않습니다. 실폰·실제16KB·전체 권리·음향/장시간 성능은 후속입니다. 현재 필요한 사용자 조치·질문은 없습니다.

공개 근거는 외부 루트 `D:/codex/validation/sudden-force-fps/20261110-work`에 보존했습니다. `root-resume01/editor-case04-root-observations.public.json`은 총괄 도구 관측 요약이고, camera01/02의 summary와 PNG, LD2 030/033 PNG, holder/prefs 결과, 소유2 회수 결과, `editor-case04-final-return01/final-audit.public.json`은 각 단계의 실제 기록입니다. 독립 검토는38개 공개 입력 핀·199 PNG 메타데이터를 대조하고 선택13개 이미지를 직접 보았습니다. 전체199개 이미지의 시각검토나 P13의4,095개 파일/Git/private6 독립 재조회는 아닙니다. 원시 로그·개인/서명 입력은 저장소로 옮기지 않습니다.

| 자료(위 루트 기준) | bytes | SHA256 |
| --- | ---: | --- |
| `root-resume01/editor-case04-root-observations.public.json` | 2,417 | `CE8B439A0FEFAF5C5442D5062AC274D264491B63046DB2A168B6C39C5E55D657` |
| `root-resume01/editor-case04-camera01/summary.public.json` | 26,995 | `5305366924DB971755B8F778E4E0B51BC6333BFA3C35214E757E207AC723497B` |
| `root-resume01/editor-case04-camera02-diagnostic/summary.public.json` | 27,297 | `C2A6266C15204DFED0BBB004CC77C9797685A0C1B05AD11067C51A4945176481` |
| `root-resume01/ld2-case03-interactive-9efd585de6f94aaab19d09d04b244936/finish.public.json` | 432 | `B01456A37E9CF1CACBD9D9C553715EC964BFCF2331190C62572C8D7E5624E452` |
| `root-resume01/shared-prefs05-final-3aa4117a17d54a6985ae57a6ac234d28.public.json` | 1,779 | `2088FCDB952A8820FE4FA4172561A3FF9E6D9499D15004D58EBCA3E68D749B08` |
| `root-resume01/editor-case04-final-return01/final-audit.public.json` | 1,145 | `BEB3D57327756B0EDC2DD05CBC868E7041470ADFC3EB2FF7A5DCD97E0C1BD125` |
| `qa2-editor-ld-case04-actual-independent-review01/verification.public.json` | 2,807 | `CC2A836EFF538FC69C6DE83F11D39649D41F96B38765E0D582E99F7923F74CE6` |
