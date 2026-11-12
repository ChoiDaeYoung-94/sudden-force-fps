# 모바일 FIRE+LOOK 입력 수용 범위 · #259 / PR261

FIRE를 누른 손가락으로 시점을 움직이면서 다른 손가락으로 조이스틱을 사용할 수 있도록 `PlayerInputSource`의 중앙 touch sampler에 FIRE 포인터 소유권을 연결했습니다. 버튼이 raw delta를 별도로 전송하지 않으며 Up/Cancel/비활성화 때 자신이 얻은 소유권을 해제합니다.

## 소스와 최신 main 결합

입력 후보 `b1a5c6e905ad17784813a2a7608688a572600c25`는 main `4471888ac3bcd2848be6bc78bcd097f0a9f9a307`에 입력 두 파일만 재결합한 head입니다. 기존 head와 main을 두 부모로 보존했고 나머지 main 4,087 entry와 mode는 일치합니다. 이 문서와 README의 후속 기록은 제품 C# 변경을 추가하지 않습니다.

- `PlayerInputSource.cs`: Git blob `70170ae8ce1e7a699ff9d5338272d581ffd421e4`, 실제 Windows 파일 SHA-256 `F8788B018F41066597D0441B9FE3CBCD7C3E7388C5784062DB1FAC4222A66777`.
- `CombatPointerControl.cs`: Git blob `684673c98ceb900ff389dcfa4768f42fc61b41bf`, 실제 Windows 파일 SHA-256 `E72E7C0265847CFFB6C4309C42DBAE228E2DAFF1E26FE177F55BF8DF955B3B67`.

Git LF blob과 Windows CRLF 파일 핀을 구분합니다. 두 파일은 실제 시험 APK QA5와 같으며 입력 struct, 메뉴/생명주기 소비 계약, Scene·프리팹 및 Unity/Fusion 의존을 별도로 대조했습니다. 변경 diff는 touch LOOK 허용·capture/release API·기존 sampler 연결과 FIRE 버튼 연결입니다. 기존 Snapshot/Reset/menu/focus/pause/life held gate의 본문은 유지합니다.

새 P14 체크아웃에서 실제 제품 타입·7개 메서드 IL을 읽었습니다. 새 native52000의 `Assembly-CSharp` MVID는 `45ede068-f8c6-46f8-acd3-20b62fe8cf5a`이며 stopped/noncompiling/nonupdating·QA define false·StandaloneWindows64/Null graphics에서 eval 진단0입니다. 이는 실제 제품 컴파일 근거이며 전체 Console 이력0이나 게임 실행 근거가 아닙니다. 정상 Exit0 뒤 공개4,083개 기준으로 생성된 설정2개를 보존·복원했고 전체 raw exact/Git clean을 확인했습니다.

## 같은 입력 코드의 실제 Android 검증

공기계와 LD의 QA5 설치본, 같은 접속 버전의 Editor를 사용했습니다. QA5는 `.localqa`/code5/`1.0.197-qa5`이며 APK100,492,368B의 SHA-256은 `2B8D9048337E4D32A11991DA9E40D3044CDF14C8CC5FD25B6092808177B10AFB`입니다. QA5와 P14는 전체 Networked layout/protocol이 다르므로 두 후보를 서로 접속시키지 않았습니다.

| 조건 | 실제 관측 | 범위 |
| --- | --- | --- |
| FIRE+LOOK+이동, 두 해제 순서 | 정상 두 손가락 이벤트 각28개·pointer 정리, 서버의 발사/탄약·위치·시점 변화 | QA5 실제 Android 입력 |
| 메뉴 진입·복귀 | prefire13/17 → MENU22/8 → 원 UP 전 두 표본22/8 고정 → 새 Down25/5 | MENU snapshot 이후의 한정 구간. prefire→MENU 차이를 정확한 메뉴 후 오발로 해석하지 않음 |
| held 사망·리스폰 | 정상 LD 명중으로 HP0/life0 → HP100/life1, 기존 held 두 표본shot17/ammo30 고정 → 새 Down20/27 | Editor client 복제 관측. 시계 정밀 동기화·정확 적용 프레임은 미확인 |
| HOME·복귀 FIRE | 서버 관측838행, 보수적 원 UP 전169행/2.8초에서shot23/ammo7·시점 고정 → 새 Down 후26/4·시점 재개 | 실제 HOME key pair·비게임 전경·동일 게임 프로세스 복귀 |
| HOME·복귀 조이스틱 | 서버 관측853행. 여유 구간149행/2.467초에서 이동 직경2.342mm·속도0.001381m/s → 새 Down 후0.833m 이동·최대4.498m/s | 같은 프로필의 옆 이동 좌표. 지형 장애물과 입력 차단을 구분 |

메뉴·사망 시험의 먼저 발생한 실패는 덮어쓰지 않았습니다. 이전 메뉴 시도는 사격 선행이 불명확했고, 종료 경기의 사망 시도와 오래된 LD 세대 영수증 시도는 실제 목표 입력 전에 차단됐습니다. 위 표는 별도의 성공한 실제 자료만 사용합니다.

HOME 프로필은 기존 포인터의 Down/이동 뒤 HOME 1회, 게임 Activity 복귀, 새 Down 없는 기존 Move/Up, 새 Down/이동/Up 순서입니다. MotionEvent19개와 HOME key2개·포인터 정리·자식 Exit0·동일 게임 PID를 확인했습니다. Android wall clock과 호스트는 사전·사후 ADB date의 왕복 구간을 이용해 보수적으로 대조했으며 동일 시계나 동기 dispatch를 주장하지 않습니다. 관측기 Stop과 다음 Editor frame에서 소유 registry 두 개의 부재도 확인했습니다.

## 종료와 독립 검토

두 HOME 시험 뒤 host의 남은44.34375초를 다음 정상 simulation tick에서10초로 줄였습니다. timer 외 HP/점수/phase를 쓰지 않았고 정상 `TimeExpired`/DRAW0:0, RETURN 입력 뒤 Lobby 복귀를 확인했습니다. 시험용 timer의 global 제거·객체 부재, native39448의 정상 Exit0, 공유 설정 원복과 폰 font1.0/QA5 PID 부재를 확인했습니다.

소유 시험 파일2개와 생성 Resolver를 정리한 후 새408b QA 소스 기준 공개4,096개 raw exact/Git clean을 확인했습니다. Resolver에는 형식 차이 외 실제 설정3개 차이도 있어 원본·생성본·차이를 보존한 뒤 복원했습니다. 형식만 바뀐 것으로 합산하지 않았습니다. 이 QA 소스 복원은 P14의 별도4,083개 감사와 구분합니다.

독립 검토는 exact head/base의 입력 diff·기존 계약 및 P14 컴파일, 같은 두 blob의 실제 양손/메뉴/사망/HOME 원자료를 함께 검토했습니다. HOME FIRE와 조이스틱은 각 원자료에서도 구간·수치를 독립 재계산했습니다. 입력 두 파일에 한정한 추가 차단 사항은 없다는 판단입니다. 외부 자동 수치 분석기의 첫 세 오판정은 별도 보존·차단했고, 수정본은 합성 경계 검토 후 실제 분석에 사용했습니다.

## 별도로 남는 범위

새 P14 Android APK·실제 Game 통합, P14와 QA5의 wire/protocol 상호운용, 정확한 Unity focus/pause callback 종류·순서와 입력 지연, 전체 모바일 환경·실제16KB·전체 보행/소총 품질은 이 기록의 통과 대상이 아닙니다. 이 검증은 운영 Play34/release12·원장196,2,34나 설치 QA5를 새 버전으로 바꾸지 않습니다. 새35 예약·운영 서명/업로드와 CI/CD 작업은 수행하지 않았습니다.

공개 수치 근거의 로컬 보관 위치는 `D:/codex/validation/sudden-force-fps/20261112-night/`의 `pr261-final-independent-review/`, `input261-main-compile01/`, `hp1-input-lifecycle/`, `qa5-focus-root01/`입니다. 원시 로그·개인키·자격정보·기기 식별자는 공개 기록에 반입하지 않습니다.
