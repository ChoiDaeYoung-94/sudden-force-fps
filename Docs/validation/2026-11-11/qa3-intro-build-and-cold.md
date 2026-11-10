# QA3 인트로 변경·빌드·콜드 표본 검증 기록

#369의 실제 결과입니다. 시각은 **2026-11-10 UTC(한국 시각 11월 11일)**입니다. 19:26:51에 폰·LD2 실행과 반환을 완료했습니다.

**QA3 APK 생성·정적 검증·기존 키 서명·폰/LD2 설치·로비 진입을 확인했습니다.** 콜드는 각 1회 녹화에서 선택한 표본만 수용하며 QA3 경기는 미실행입니다. 운영 code34/Play release12/원장 196,2,34는 변경하지 않았습니다.

## 변경 출처와 입력 범위

P13의 D7 d7a3371296357a5ceb1b7b657d468415130e7047을 기준으로 승인한 두 파일만 임시 변경했습니다. main 전체를 D7에 병합한 빌드가 아닙니다.

- PR364(#362): Android 시작 배경색 한 항목을 검정으로 맞춘 변경입니다. 임시 PS black 후보는 29,741B/EC1EC396…352D6입니다.
- PR368(#367): 프로젝트 소유 intro adapter가 유효 video frame과 texture를 기다리는 guard입니다. 실제 후보는 CRLF 4,637B/9CC017A2…B580이며 PR의 LF 소스 본문과 줄바꿈 정규화 후 같습니다.
- 두 파일 이외 공개 4,093개 입력의 보존은 source2 감사로 구분합니다. 실제 APK는 `com.AeDeong.SuddenForceFPS.localqa`/code3/1.0.197-qa3/min25/target36/ARM64입니다.

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

## 완료: assemble·정적 검증·기존 키 서명

실제 Native assemble은 Exit0/EOF2/소유 Job 종료·자식0으로 완료됐습니다. 새 native 최종 strip 출력 7개를 APK에서 기대값을 만들지 않고 별도 고정했습니다. 19:11:48에 승인 회수와 원본 공개 4,095개/개인 파일 sparse 제외/Git clean/P2 보호 상태를 확인해 반환했습니다.

| 결과 | 실제 근거와 한계 |
| --- | --- |
| 원본 unsigned | 100,485,116B/SHA256 `F52F0AB8CE99C18ADA4063B4DEA0D469ECC2B6CEF911278EAF19DE99F436DCAB` |
| 정적 검증 | ZIP359 전체 CRC/bytes/SHA, Data229/219,467,412B, native7 ELF64/AArch64 LOAD·RELRO·출처, 순서 있는 5scene, 내부 QA3 버전, 고지 725,685B 및 staticSource55 통과 |
| 독립 실제 검토 | IR 7,767B/SHA256 `635474C83A078FE14893D423BF3D35F1FB7F45A1F5F7BBD0EF8BF70ACB54D688`, P1/P2 없음. bounded native/unsigned static만 수용 |
| 로컬 QA provider 제거 | childless PlayGamesInitProvider 1개만 제거. 나머지 ZIP/Data/native payload exact. PGS/Play 인증 성공 근거가 아님 |
| 기존 키 서명 | v2 서명·zipalign·apksigner verify 통과, 인증서 SHA256 `4D072592228C991D3F6A0EDA768C8CBEE40895C3B1F4A6FB9379540F048965E0`. 19:18:22에 승인 회수·반환 |
| 최종 signed APK | 100,492,368B/SHA256 `DD663868131DB367E7866AEED6BF52F0762BEF84B09750FBE87EC8D21D953698` |
| 폰·LD2 설치본 | 각 install Success. 설치 APK 전체 재읽기 bytes/SHA가 최종 signed와 exact, package_info code3/1.0.197-qa3 확인 |
| 메뉴·일반 gesture | 각 닉네임 입력/확인 후 PUBLIC ROOMS 로비를 화면으로 확인. 방 생성·QA3 경기는 미실행 |
| 데이터 보존 범위 | 기존 앱에 갱신 설치했고 uninstall/clear-data0. 그러나 양쪽에 닉네임 창이 나타났으므로 기존 닉네임·모든 저장값 유지 성공으로 판정하지 않음 |

최종 파일은 `D:/codex/releases/sudden-force-fps/localqa-1.0.197-qa3-20261111/SuddenForceFPS-LocalQA-1.0.197-qa3-ARM64.apk`입니다. Drive 업로드·새 AAB·Play 배포는 하지 않았습니다. local QA3는 기존 QA2와 같은 Fusion QA suffix를 유지합니다.

## 완료: cold 영상의 제한된 표본 비교

공기계 SM-A175N과 LDPlayer index2/FPS에서 각 20초 screenrecord를 실행하고 전체 디코드 후 목표 0~19초 각 20장만 시각 검토했습니다. 영상 PTS는 녹화 기준이며 실제 cold 시작/완료 지연을 정밀 측정한 값이 아닙니다.

| 녹화 | 전체 디코드 | 선택 표본의 관측 |
| --- | --- | --- |
| 폰 678,793B/SHA256 `48FEBCDA0095D11F42C16AEC65B124152FC2B1693840DC06B6E9B57AB989A4E3` | 803frames/19.994067s | Quad D 목표3~5초, Loading7~10초, 닉네임11~19초 |
| LD2 1,034,637B/SHA256 `9EC7483AABB0DFC403D7B853DF3969CF353FDF29C89932F4DAFB7183A70C2BC9` | 542frames/19.5194s | Quad D 목표5~7초, Loading10~14초, 닉네임15~19초 |

두 기기의 선택 표본에서 흰 전체 화면과 Unity 로고를 관측하지 않았습니다. QA2의 폰 표본에서 흰 화면 목표3~4초/Quad D6~8초/닉네임14~19초였던 기록과 구분해 보존합니다. 전체 프레임 시각 검토·짧은 깜빡임 부재·반복 cold 안정화·시작 속도 개선 확정은 아닙니다. LD 목표1초는 첫 가용 PTS2.053초여서 정확1초 화면이 아닙니다.

폰 녹화에는 portrait canvas 안의 landscape 화면이 보입니다. 이후 직접 캡처는 2340×1080이므로 이 녹화만으로 앱 레터박스·실제 회전·인트로 크기를 판정하지 않습니다. 원본 녹화와 접촉시트는 비공개 외부 자료로 보존합니다.

manifest의 `unity.splash-enable` typed 값은 true/splash-mode0입니다. API 설정 요청과 구분하고, 선택 영상에 Unity 로고가 없었다는 사실을 manifest의 splash=false 또는 완전 제거로 표현하지 않습니다.

각 caller는 실패 호출0/활성0, source 호출 전 형식 거절1을 기록했습니다. 마지막 game force-stop+HOME 성공 뒤 config 승인5개·lease 승인3개 및 window를 회수했습니다. 19:26:51에 동일 ADB server38840의 connected exact4tuple peer를 재확인하고 P2 보호5개 바이트 exact 상태로 반환했습니다. 서버 lifecycle·native helper 실행0입니다.

## 수용 한계와 후속 기록

QA3의 반복 cold·모든 프레임의 흰 구간 부재·실청·실기기 16KB·발열/장기 FPS·PGS/Play 권한·기존 닉네임 유지·QA3 경기는 미검증입니다. Native3 helper는 실행하지 않았고 state-recorder define은 비활성입니다. 기존 QA2 경기/입력/포즈 결과를 QA3 성공으로 합산하지 않습니다.

raw serial/방 token/키·암호 관련 비공개 속성/원시 로그는 공개 문서에 넣지 않습니다. 공개 소스·영수증·핀·원창과 제한된 결과만 연결합니다.

실패 이력(C03 long path, Static 초기 두 준비/결속 거절, 최초 기기 scope의 경로 거절)은 보존했습니다. 실패·대기·미실행을 PASS로 합산하지 않습니다. 공개 결속 자료는 `D:/codex/validation/sudden-force-fps/20261111-work/`의 `qa3-native-static-actual-independent-review01/verification.public.json`, `root-resume01/qa3-game-manifest-cert.public.json`, `root-resume01/qa3-device-full-return.public.json`과 분리된 실제 결과에 보존합니다. 현재 필요한 사용자 조치·질문은 없습니다.
