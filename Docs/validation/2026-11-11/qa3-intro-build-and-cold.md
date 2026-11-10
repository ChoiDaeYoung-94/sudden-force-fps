# QA3 인트로 변경·빌드·콜드 검증 기록 초안

#369 공개 기록용 초안입니다. 시각은 **2026-11-10 UTC(한국 시각 11월 11일)**입니다. 마지막 전달 상태는 C04 buildIl2Cpp가 18:58:48 이후 실행 중인 시점이며, 이후 결과는 Root의 실제 근거로 갱신합니다.

**QA3 APK·정적 검증·서명·설치·콜드·경기 결과는 모두 PENDING입니다.** 기존 설치 QA2와 운영 code34/Play release12/원장 196,2,34를 유지합니다.

## 변경 출처와 입력 범위

P13의 D7 d7a3371296357a5ceb1b7b657d468415130e7047을 기준으로 승인한 두 파일만 임시 변경했습니다. main 전체를 D7에 병합한 빌드가 아닙니다.

- PR364(#362): Android 시작 배경색 한 항목을 검정으로 맞춘 변경입니다. 임시 PS black 후보는 29,741B/EC1EC396…352D6입니다.
- PR368(#367): 프로젝트 소유 intro adapter가 유효 video frame과 texture를 기다리는 guard입니다. 실제 후보는 CRLF 4,637B/9CC017A2…B580이며 PR의 LF 소스 본문과 줄바꿈 정규화 후 같습니다.
- 두 파일 이외 공개 4,093개 입력의 보존은 source2 감사로 구분합니다. 목표 localqa/code3/1.0.197-qa3와 실제 APK manifest·bytes 일치는 PENDING입니다.

export의 source53+임시 helper2=temporary55와 정적 검증의 staticSource55는 서로 다른 집합입니다. 각각의 manifest와 핀을 별도로 결속하며 단순히 “55개 동일”로 합산하지 않습니다.

## 완료: Unity export와 원본 복구

export 회차 6b4003…의 Native25628 정상 Exit0을 확인했습니다. 지정 메모리 설정 12개, 검정 배경의 export 전후 관측, 승인한 두 source 후보가 적용된 공개 4,095개 freeze를 남겼습니다.

반환 단계에서 발생한 생성 차이 26개는 변경으로 기록했습니다. 원본·생성본 52개를 외부 보존하고 분류·명시적 복구를 수행했습니다. 그 뒤 별도의 fresh 감사로 다음을 확인했습니다.

- 원래 D7 공개 4,095개 raw exact, private6 absent/indexS.
- P2의 기존 dirty5 바이트 보존.
- 최종 반환 18:16:20. 생성 차이 26개 분류와 원본 전체 일치 감사는 서로 다른 단계입니다.

F02(qa3-unsigned-candidate02/input-freeze)의 actual input IR(F883…)과 source2 freeze·반환 근거를 보존합니다. 이후 실행은 이 immutable cohort를 소비하며 새 실행 결과를 F02에 덮어쓰지 않습니다.

## 완료: C03 차단과 C04 수정·preflight

C03 첫 preflight는 F02 snapshot의 264자 경로 두 개에서 PINNED_PUBLIC_FILE_MISSING으로 차단됐습니다. NativeWorker/Job/출력 생성 전이며 Gradle 실행0입니다. 실패·진단·미사용 stage 3,040개를 보존하고 10개 승인을 회수해 18:46:29 반환했습니다.

별도 Java 진단은 같은 두 파일의 File/NIO/FileInputStream 읽기 508/179B에 성공했습니다. Groovy I/O는 변경하지 않았습니다.

C04는 Python 파일 메타데이터·읽기 I/O에 extended-path view를 적용하고 외부 manifest/pin은 정상 D:/ 표현을 유지합니다. 기존 파일·reparse·bytes/SHA 검사는 유지했습니다. Root의 실제 pin() 진단에서 두 경로의 크기·SHA와 정상 public path를 확인했습니다.

| C04 준비 근거 | 상태 |
| --- | --- |
| main source 78,372B/9AC4A99A…8283 | 동결 |
| core 3,027B/4041335E…7EE6 | 16멤버 핀 일치 |
| 독립 source IR 9DD200C8…5827 | source 준비 수용, 실제 빌드 자격과 구분 |
| Root의 long-path 두 파일 pin 읽기 | PASS, SDK·Job0 |

새 원창은 c4915b…/시작 18:54:15→신규 시작 제한 19:39:15→전체 종료 19:42:15입니다. 이전 f6c6… 원창을 연장하지 않았습니다. 새 context/stage/output으로 준비했으며 preflight는 18:56:38 실제 PASS입니다.

## 진행 중·PENDING

18:58:48 이후 Native assemble의 buildIl2Cpp가 실행 중입니다. assemble 완료·새 APK 생성·전체 반환 통과는 아직 미확보입니다.

| 후속 결과 | 상태와 추가할 실제 근거 |
| --- | --- |
| Assemble 및 resource 반환 | **PENDING** — receipt/Exit·Job·EOF·승인 회수·원창 |
| Unsigned QA3 APK | **PENDING** — 파일명/bytes/SHA/manifest code·name |
| 정적 검증 | **PENDING** — ZIP/CRC/AXML/Data229/ELF/native/notice/Source55 |
| 기존 키 서명 | **PENDING** — 서명·인증서·signed APK bytes/SHA |
| QA2→QA3 데이터 보존 갱신 설치 | **PENDING** — 설치본 readback·버전·기존 데이터 |
| 실제폰 콜드 시작 비교 | **PENDING** — 영상·시각 표본·회전/배경/인트로·메뉴 |
| QA3 기본 메뉴·일반 gesture | **PENDING** — 실제 화면과 정상 cleanup |
| QA3 경기 결과 | **PENDING** — 새 회차의 별도 결과 |

QA3 후속 기기 검증은 Native3 helper 실행을 금지하고 일반 tap/swipe와 screenshot/screenrecord 범위로 준비합니다. Unity state-recorder define 비활성의 빌드 근거와 Android screenrecord 사용을 구분합니다. 기존 QA2 경기 결과를 QA3 성공으로 합산하지 않습니다.

## 수용 한계와 후속 기록

QA3의 기기 흰 구간 개선·실청·실기기 16KB·발열/장기 FPS·PGS/Play 권한은 통과0입니다. source IR·helper 의미 컴파일·preflight만으로 새 APK나 런타임 결과를 수용하지 않습니다.

raw serial/방 token/키·암호 관련 비공개 속성/원시 로그는 공개 문서에 넣지 않습니다. 공개 소스·영수증·핀·원창과 제한된 결과만 연결합니다.

실제 APK/정적/서명/기기 결과와 최종 근거 핀은 후속 검증 뒤 추가합니다. 현재 필요한 사용자 조치·질문은 없습니다.
