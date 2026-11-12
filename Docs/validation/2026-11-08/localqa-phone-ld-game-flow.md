# QA1 휴대전화·LD플레이어 기본 경기 검증 — 2026-11-08

관련 #347 / Draft PR348. 설치된 LocalQA `1.0.197-qa1`의 정상 화면 흐름을 실제 두 기기에서 확인했습니다. 운영 Play 코드34/release12와 분리된 테스트 앱이며, 이번 기록은 전체 입력 검증이나 제품 출시 승인이 아닙니다.

기기 구성은 Samsung SM-N986N(Android13/ARM64)과 LD플레이어 index2(Android14/x86_64)입니다. 게임 APK는 99,942,564B/SHA256 `17FC62AD4A73E6B3221BD470E2D97AEB84AFCEB29AB2F20ADEAF56DA359C188F`, 입력 도우미 APK는16,528B/SHA256 `31B7971E51B49C2814CCEACA7F77C98529E05E732DC372890AB331995B7C520E`입니다. 앞선 case01에서 두 기기의 설치 APK 네 개를 다시 내려받아 각각 일치시켰습니다. 아래 case02에서 설치 APK를 다시 읽거나 새로 설치한 것은 아닙니다.

case02의 LD 호스트 `SFQAld8`와 휴대전화 클라이언트 `SFQAphone8`은 `SFFQA1108G2` 방의 반대 팀으로 참가했습니다. 닉네임 확인→DesertHouse 방 생성/공개 목록 참가→READY/START→양쪽 Game HUD→5분 종료→양쪽 결과→양쪽 로비 복귀 순서입니다. 결과 화면은 모두 DRAW/0:0/TIME EXPIRED, 명단 두 명 DONE이며, 복귀 뒤 PUBLIC ROOMS의 빈 목록을 확인했습니다. 로비에서 Android Back은 입력 취소로 동작하므로 닉네임은 화면의 확인 버튼으로 확정했습니다.

양쪽 탄약 표시는 사격 전30→사격 후20→재장전 후30이었습니다. 일반 조이스틱 입력 후 화면 변화와 휴대전화 시점 스와이프를 관찰했고, LD의 ROSTER에서 두 명 ONLINE을 확인했습니다. 상대 명중·피해·처치·사망, 실제 좌표/NCC, 달리기/앉기 held 상태와 동시 터치, 소리·성능은 이 결과에 포함하지 않습니다. 걷기·달리기 애니메이션 후보도 이 APK에는 없습니다.

호출76개 중 wrapper 종료0은72개, guard 실패는4개입니다. 실패3개는 primary exit=null인 사전 차단이고, LD 캡처1개는 primary exit0 뒤 후속 guard 실패입니다. 네 실패를 성공으로 합산하거나 자동 재실행하지 않았습니다. 양쪽 앱 중지/HOME의 정리4개는 종료0이지만, 정리 뒤 PID 부재는 별도로 조회하지 않았습니다.

원창은 UTC11:42:27.342235/new11:54:27.342235/full11:57:27.342235였고, 실제 전체 반환은11:54:44.399947입니다. 소비한 공개 제어는 원문 보존 후 승인 회수했고, private lease 회수·서버760 연결 peer·공유 자원0은 Root 공개 관측입니다. 독립 검토자의 새 OS/기기 조회나 TCP listener 소유권 증명으로 표현하지 않습니다.

독립 검토는 성공 PNG31개의 CRC/IDAT와 길이/SHA, 공개 복사본11개의 바이트 일치, 직접 본 고유 화면15개와 원 호출/반환 근거를 대조해 제한된 두 기기 Room→Game→Result→Lobby UI 흐름을 수용했습니다. 실제 Ready 내부 상태, 설치 바이트의 case02 재검사, 하드웨어 동시 입력과 전체 source-to-IL 등가성은 수용하지 않았습니다.

외부 근거의 기준 폴더는 `D:/codex/validation/sudden-force-fps/20261108-work/`입니다.

- `device-case02-independent-review01/verification.public.json` —4,521B/SHA256 `2FD79B05AF92C2A8E3D1E5C0BB6600E7ACE52F4B07CDFD7C09B77B837588B46B`.
- `root-resume01/device-case02-root-visual-observation01.public.json` —7,820B/SHA256 `8A6590B63539EDC5FA8B156B5C3FC930E2B2281DE71DE224CF130A618B0F29EA`.
- `root-resume01/device-case02-actual-return01.public.json` —78,830B/SHA256 `4C0C27E7BE7B00BF65BA9E9F4C47ED41B2553A48040B0392B367963E45877A03`.

이 기본 경기 뒤 진행한 LD 인트로 표본·입력 준비·PC 연동 실패는 [같은 날의 후속 기록](localqa-followup-bounded-results.md)에 구분했습니다. 실제 Game에서 보행 후보·동시 터치/해제 순서/lifecycle 검증은 계속 남아 있습니다. 기존 실패와 Draft 상태를 유지하며 현재 필요한 사용자 조치와 질문은 없습니다.
