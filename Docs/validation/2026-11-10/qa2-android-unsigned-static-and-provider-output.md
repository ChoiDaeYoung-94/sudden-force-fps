# QA2 Android 무서명 빌드·정적 검사·provider 출력

2026-11-10 Raw05 QA 소스 `d7a3371296357a5ceb1b7b657d468415130e7047`의 Android QA2 무서명 APK를 생성하고 정적 검토와 시험용 provider 제거 출력까지 확인했습니다. 아직 서명·설치·실제 Game 수용은 아닙니다. [이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)과 [Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)을 유지합니다. 같은 소스의 [Windows 빌드·복구](qa2-windows-player-and-restoration.md)와 향후 두 피어 검증을 연결합니다.

## C05 실패와 새 C06

C05는 assemble1500초 제한에 걸려 tool1·timeouttrue로 끝났습니다. worker0만으로 성공 처리하지 않았고, Job close 전 active13·자식 상태UNKNOWN과 무APK를 보존했습니다. 객체664개 등의 부분 출력과 실패 기록은 보존했으며 C06에 재사용하지 않았습니다.

C06은 원래 정상 Unity export03에서 새 nonce의 snapshot/stage/cmake 경로로 준비했습니다. export3,041개와 복사본2개, 공식 SDK counterpart287개·tool18·AGP jar4개·공개 입력 subset3,986개를 대조했습니다. 시작/종료의 전체 공개4,095개 감사와 subset 검사를 구분합니다. SDK 공개 의존 jar cache만 사용하고 이전 compiled native/partial cache는 사용하지 않았습니다.

자식에 `DOTNET_PROCESSOR_COUNT=2`/`BEE_BUILD_THREADS=2`를 적용하고 JVM processor1·Gradle worker1·heap4GB·offline·unsigned 조건을 유지했습니다. preflight120초/assemble2400초, 원창 UTC03:50:24→새 실행 중단04:35:24/전체 반환04:38:24입니다. 정상 수용 시간을 연장하거나 C05 원창을 재사용하지 않았습니다.

preflight는 실제 Exit0·자기 Job active0/close·EOF·cleanup0, 작업 실행0/무APK로 끝났습니다. QA2 identity5개와174개 작업의 unsigned graph, packageRelease의 signing-data provider 부재를 총괄이 읽고 별도 assemble 승인 후 실제 생성했습니다. assemble174/174·실패0, worker14204의 같은 Popen 핸들 Exit0·timeoutfalse·EOF·자기 Job active0 관측 후 close·cleanup0을 확인했습니다. Root tool30673/df8a6f/0는 총괄 관측으로 구분하며 독립 검토자의 도구 직접 관측으로 표현하지 않습니다.

SDK 실제 반환은 UTC04:09:19.326918입니다. 원 full 이전, 알려진 SDK/자기 prefix2회 부재 관측과 public4,095 rawexact·HEADd7·Gitclean/private6 absent/index S, Unity ADB prefs2 DWORD1·기존 daemon 관측 세대를 확인했습니다. 원 binding과 승인 바이트를 archive하고 binding9개/auth3개만 false로 회수했습니다. 별도 실제 검토에서 이 공개 기록·원본 archive·현재 Source4,095와 APK/native snapshot을 대조했습니다.

## 원 무서명 APK 정적 범위

원 APK는 `com.AeDeong.SuddenForceFPS.localqa`, code2/`1.0.197-qa2`, min25/target36입니다. 100,486,616B/SHA `B21BAD7A518D710204002B03A2A1EF3ABDA1C0130A5E36625D58DB920CA0AF34`이며 운영 앱과 구분한 시험 앱입니다.

Static07의 첫 실제 검증 및 별도 독립 재검토에서 다음을 확인했습니다.

- ZIP359개 전 stream의 크기/SHA/CRC, duplicate0·V1서명0·APK Signing Block 부재. res 경로의 casepair2그룹은 정확 경로의 stream으로 비교했고 Windows 파일 추출은 하지 않았습니다.
- 원 export Data229개/219,467,256B와 고지725,685B의 RAW_EXACT 일치, Source55 전후 일치와 씬 순서를 확인했습니다.
- normal graph의 실제 fresh `stripReleaseDebugSymbols` 최종 입력을 Root가 APK와 독립적으로 캡처했습니다. 전체 source/snapshot7개와 APK7개의 경로 집합·크기/SHA가 일치하며 ELF LOAD/RELRO 정적 검사를 통과했습니다. raw JNI/AAR 바이트나 관측 APK에서 역으로 고른 기대값을 사용하지 않았습니다. 실제16KB 기기 수용은 아닙니다.
- 원 source XML의 `unity.splash-enable="True"`와 APK의 typed booltrue는 finite encoding 정책에 맞습니다. 요청한 `PlayerSettings.SplashScreen.show=false`와의 등가나 실제 Unity splash 부재는 입증하지 않았습니다. 인트로 전체 재생·제품 수용도 별도입니다.

## 시험용 provider 제거 출력

Google Play Games 초기화 provider는 원 APK에 존재했습니다. 후보01의 다른 provider authority 세미콜론 목록 공유 검사가 빠진 결함은 반려·보존했습니다. 후보02에서 모든 provider authority를 literal token 목록으로 검사해 공유·빈/중복/공백/resource 참조를 거부하며, 새 독립 소스 검토를 통과한 뒤 실제 실행했습니다.

대상은 `com.google.android.gms.games.provider.PlayGamesInitProvider` leaf1개와 Root가 원 qualified 입력에서 확인한 `com.AeDeong.SuddenForceFPS.localqa.playgamesinitprovider`입니다. actual result와 독립 stdlib ZIP/AXML reader에서 start/end2개 chunk·140B 제거, manifest10,744→10,604B와 totalSize4바이트 외 원시 바이트 일치, 다른 typed tree/attribute·권한·string pool/resource map/namespace 보존을 확인했습니다. 사용하지 않는 문자열이 pool에 남는 것을 제거 범위 확대 근거로 사용하지 않았습니다.

출력은100,486,477B/SHA `9CFE7D5AD4F5468B53F443E0158F74D27C8D41E909352A3363C44730DCD7A056`입니다. ZIP359개 집합/순서/압축 방법과 나머지358개 payload의 bytes/SHA/CRC, Data229·native7이 일치했고 무서명 상태를 재확인했습니다. 압축 stream/offset·공식 zipalign·Android parse/시작 성공은 이 재포장 검토에서 수용하지 않았습니다.

## 서명 사전 거절과 다음 단계

기존 키 signer01의 첫 요청은 Root가 추가한 publicOnly 필드 때문에 정확3필드 pin guard에서 거절됐습니다. 수정한 별도 요청은 공개26핀 중 현재 번들 Python SHA1개가 역사 tool-pin과 달라 tools guard에서 거절됐습니다. 두 요청 모두 attempt 디렉터리·SDK child·키/PW metadata/contents 접근 전에 끝났고 원 입력/거절 기록과 원창을 보존했습니다. 현재 config7/auth3승인은 false이며 P2 기존 설정5개 post hash를 확인했습니다.

현재 Python은107,312B/3.12.14, SHA `1DA8F2782FD63CDFCA8B0DA1A46778F48C28AA9EDA3DDCD852957EC60813157B`, 유효한 OpenAI Authenticode이며 실제 C06가 사용한 Python pin과 같습니다. 기존 거절을 성공으로 바꾸거나 signer01의 동결 tool JSON을 편집하지 않았습니다. 새 signer02는 namespace와 Python1행만 변경했고 별도 독립 소스 검토를 통과했습니다. 키/PW·서명·종료·3필드 pin 계약은 그대로이며 이 문서 시점의 실제 서명은0입니다.

다음은 신규 승인·기존 키 서명/zipalign/공식 인증서 검증·P2설정5 post audit, LD2 설치와 같은 QA2 Windows–LD 경기입니다. 표시 버전과 실제 Photon 프로토콜을 혼동하지 않으며, Raw05 보행 분리 모드·최종 rifle/접지·native recorder·기기 수용은 실제 검증 전까지 미완료입니다. 이번 기기는 LD만 사용하고 휴대전화/장시간 성능·발열은 후속으로 남깁니다. 운영34/1.0.196/release12·원장196,2,34와 기존 QA1은 보존합니다.

## 공개 근거

외부 루트 `D:/codex/validation/sudden-force-fps/20261110-work`의 공개 자료입니다. 원시 로그·자격정보·dump를 저장소에 옮기지 않습니다.

| 자료 | SHA256 |
| --- | --- |
| `qa2-gradle06-actual-independent-review01/verification.public.json` | `B506F34DAA53DC65CDCDBFC6F5A10461551AF9709810120FE020481CBA6D219E` |
| `qa2-apk-static07-actual-independent-review01/verification.public.json` | `F4249764B7EEAF5A5CA3F8032F2AA2B84C4B99CE36562220B007933C3538EBBE` |
| `qa2-gpgs02-actual-independent-review01/verification.public.json` | `5BDC753EB4C70A1712A90D3997857D1DC7E2167B18098EF7512596CD4BF2D2A5` |
| `raw05-qa2-existing-key-sign-independent-review02/verification.public.json` | `EFF8929D5F9586FDE492E9C33AA820A64CD5C356487750754F4AC024858EA458` |
| `root-resume01/qa2-gradle06-actual-return01.public.json` | `ACBCAB17E1CAC7DFDBD9E60297B0533CA6F816C631729A737512B4F4E5D9997D` |

현재 필요한 사용자 조치 없음. 질문 없음.
