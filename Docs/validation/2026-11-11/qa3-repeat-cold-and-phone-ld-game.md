# QA3 반복 콜드와 폰–LD 2피어 경기·복귀 관측

이번 기록은 코드3/`1.0.197-qa3` 설치본의 새 반복 관측입니다. 두 기기의 설치 APK readback은 각각100,492,368바이트이며 이번 QA3 고정 APK와 whole SHA-256 일치를 확인했습니다. 이번 범위에서는 양쪽 재설치 옵션을 모두 끄고 helper 호출 없이 진행했습니다. QA2의 이전 성과와 source-only 준비 결과를 이번 실제 검증에 합산하지 않습니다.

## 반복 콜드

폰과 LD에서 두 번째 콜드 관측 뒤 닉네임 입력창이 빈 상태로 다시 나타나 테스트 닉네임을 각각 입력했습니다. 이는 화면 관측 사실이며, 앱 데이터 삭제나 닉네임 영속성 문제의 원인을 확정한 기록은 아닙니다.

공개 분석 `FPS_QA3_COLD_VIDEO_SELECTED_FRAME_OBSERVATION02_V1`(observation02, SHA-256 `78831105…751A5D`)을 확인했습니다. 폰 영상은 약19.98초/821프레임, LD 영상은 약19.50초/562프레임으로 디코드됐고, 각 영상에서 정수0–19초 목표의 선택 표본20개를 직접 검수했습니다.

| 선택 표본에서 처음 관측한 화면 | 폰 영상 내 상대 PTS | LD 영상 내 상대 PTS |
| --- | ---: | ---: |
| QuadD 인트로 | 약2.023초 | 약5.015초 |
| 닉네임 입력창 | 약11.022초 | 약15.025초 |

선택 표본에서는 흰 전체 화면과 Unity 로고를 보지 못했습니다. 전체 프레임의 부재 증명은 아니며, 위 수치는 OS 콜드 시작이나 앱 실행 지연 측정값이 아닙니다. 초기 어두운 화면의 앱 귀속도 확정하지 않았습니다. 세로 인코딩 캔버스 안의 가로 콘텐츠만으로 실제 앱의 레터박스·인트로 크기·회전 상태를 수용하지 않습니다.

## 2피어 경기와 복귀

폰 host와 LD client가 DesertHouse의 두 플레이어 구성으로 입장했고 LD의 READY를 확인했습니다. UTC19:38 무렵 첫 Game 화면에서 남은 시간은 폰04:57/LD04:56으로 보였습니다. 정확한 경기 시작 시각을 측정한 것은 아닙니다.

- 폰에서 발사 뒤 탄약30→23, reload 뒤30을 화면으로 관측했습니다.
- LD에서 이동·look에 따른 화면 변화를 관측했습니다.
- 메뉴→Resume 복귀와 roster의 두 플레이어 ONLINE 표시를 관측했습니다.
- 폰 FIRE 영역 드래그 뒤 탄약30→25와 시점 변화, LD 발사30→26과 재장전30을 확인했습니다. raw 입력 시간·패킷 지연·반동/예측 전체 수용은 아닙니다.

양쪽 화면에서 **TIME EXPIRED / DRAW / RED0·BLUE0 / 두 플레이어 DONE**을 확인했습니다. LD client의 RETURN 다음 폰 host의 RETURN을 눌렀고 두 기기 모두 PUBLIC ROOMS로 복귀했습니다.

이후 LD가 새 방 host, 폰이 client로 다시 참가했습니다. 같은 프로세스에서 두 테스트 닉네임이 보존되고 양쪽 READY가 표시됐습니다. 이 새 방의 Game은 시작하지 않았습니다. 폰 client 퇴장→LD host 퇴장 뒤 양쪽 최종 PUBLIC ROOMS/방 없음 화면을 확인했습니다. 따라서 같은 프로세스의 이름 보존과 설치 없는 cold 뒤 빈 입력창 관측을 구분합니다.

## 실행·반환 근거

신규 기기 원창은 UTC19:35:31.754577→정상 신규 작업 제한19:55:31.754577→전체 반환20:00:31.754577입니다. 이전 소비된 시각·성공·승인을 재사용하지 않았습니다. B3의 source/core는 계약 revision2를 유지한 namespace3 파생이며, 독립 source IR(3,789B/SHA256 `62C871BA28FB145ADB210221ED3D618425D910780C6D9F506A7EDA1C5EE58F8C`)을 실제 host 열거/Client 생성 전과 매 guard에서 소비했습니다.

각 caller는 실패 호출0·형식 거절0·활성 호출0·기록 thread join 및 game force-stop/HOME 성공으로 UTC19:44:46에 종료했습니다. config5·lease3·window 회수와 동일 ADB connected exact4tuple peer·P2 보호5개 바이트 대조를 마친 Root 전체 반환은 **UTC19:44:54.277792**입니다. 서버 lifecycle·SDK/Editor 실행·설치·helper 주입은0이며 다른 프로젝트에 현재 자원 사용·예약0을 알렸습니다.

Root의 실제 화면 관측 결속은 `qa3-regression-observations.public.json` 10,600B/SHA256 `00E3F46FDB579F2C0B23E538A318B320D0D6478D470C2F4DAF83E0A3642963A9`이며, `root-resume01/qa3-regression-full-return.public.json`과 `qa3-cold-video-analysis02/observation.public.json`을 분리해 읽습니다. 외부 근거 루트는 `D:/codex/validation/sudden-force-fps/20261111-work/`입니다.

사격 직후 LD 단일 프레임의 주황 효과는 frozen D7 소스상 muzzle과 wall impact가 함께 나올 수 있어 위치 버그로 확정하지 않았습니다. #162의 예측 muzzle/확정 impact 구분과 연결해 동일 프레임의 spawn·카메라 투영·particle bounds/age를 대조하는 후속 기준만 남겼습니다. 실제 코드·이펙트 수정은 없습니다.

## 측정 범위

이 기록은 반복 콜드의 제한된 영상 표본과 첫 QA3 경기·복귀 및 새 방 READY/퇴장 화면에 한정합니다. 전체 시작 화면·음향, kill/hit/사망·리스폰 판정, Native instrumentation, 입력 수용 지연·성능, PGS, 전신 gait/IK는 수용하지 않았습니다. 원시 방 토큰·방 이름·기기 serial·비공개 이미지/로그는 포함하지 않습니다. 운영34/Play release12/원장196,2,34는 유지하며 새 AAB·Play 배포0입니다.
