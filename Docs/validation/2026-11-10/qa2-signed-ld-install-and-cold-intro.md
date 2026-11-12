# QA2 기존 키 서명·LD 갱신 설치·기본 cold 인트로

2026-11-10 Raw05 QA 소스 `d7a3371296357a5ceb1b7b657d468415130e7047`의 QA2 APK를 기존 키로 서명하고 LD의 기존 QA1 위에 갱신 설치했습니다. 설치본 전체 SHA와 서명 산출물 SHA가 일치하며, 기본 renderer의 cold 시작 영상에서 QuadD 조립→wordmark/hold→fade→닉네임 입력/Lobby를 총괄이 관측했습니다. 실제 Game·보행/소총 수용은 미완료로 [이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)과 [Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)을 유지합니다.

이전 [무서명 빌드·Static07·provider 제거 기록](qa2-android-unsigned-static-and-provider-output.md)은 당시의 서명 사전 거절2개와 미실행 상태를 그대로 보존합니다. 이번 기록은 그 뒤 새 signer02와 새 LD Case01에서 확인한 범위입니다.

## 서명 APK와 시험 앱 구분

| 항목 | 실제 공개 기록 |
| --- | --- |
| 패키지 | `com.AeDeong.SuddenForceFPS.localqa` |
| 버전 | code2 / `1.0.197-qa2` |
| 플랫폼 | minSdk25 / targetSdk36 / `arm64-v8a` |
| APK 크기 | 100,492,368B |
| APK SHA256 | `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33` |
| 인증서 SHA256 | `4D072592228C991D3F6A0EDA768C8CBEE40895C3B1F4A6FB9379540F048965E0` |
| 보관 APK | `D:/codex/releases/sudden-force-fps/localqa-1.0.197-qa2-20261110/SuddenForceFPS-LocalQA-1.0.197-qa2-ARM64.apk` |

총괄의 공개 snapshot 기록에서 signer 원본·LD 입력 사본·위 release 사본의 크기/SHA 일치를 확인했습니다. 문서 작성 중 APK를 새로 열거나 SDK를 재실행하지 않았습니다. 운영 앱 `com.AeDeong.SuddenForceFPS`의 code34/`1.0.196`/release12와 원장196,2,34는 유지합니다. 이번 QA2는 별도 localqa 시험 앱이며 새 Play 업로드나 운영 버전 배포가 아닙니다.

## 기존 키 서명과 독립 검토

signer02의 실제 서명·공식 verify·zipalign check가 각각 같은 보유 Popen 핸들에서 Exit0, EOF/close·cleanup unknown0으로 끝났습니다. 결과는 signer1개·V2 true·V1/V3/V4 false입니다. 독립 실제 검토는 V2 인증서/SPKI·RSA 서명·chunk content digest를 직접 검증했고 V1 entry와 V3 block 부재, 저장 entry 정렬을 확인했습니다. V4 false는 공식 도구 기록 범위로 구분했습니다.

provider 제거 후 qualified 무서명 입력은100,486,477B/SHA `9CFE7D5AD4F5468B53F443E0158F74D27C8D41E909352A3363C44730DCD7A056`입니다. 서명 전후 ZIP359개 전부를 META-INF 제외 없이 비교해 entry payload의 bytes/SHA/CRC·순서·압축 방법과 Data229/native7 보존을 확인했습니다. 서명 블록과 ZIP 컨테이너 변화가 있으므로 APK 전체 바이트 동일로 표현하지 않습니다. 압축 native7과 zipalign 통과는 실제16KB 기기 수용 근거가 아닙니다.

두 사전 거절과 signer01 동결본은 보존했습니다. signer02는 새 namespace와 현재 Python 공개 pin1행만 바꾼 후보이며, 실제 서명 후 config7/auth3 승인을 false로 회수하고 원 승인 입력을 보존했습니다. 총괄의 post audit은 기존 P2 설정5개 hash 일치와 기존 dirty 보존을 보고했습니다. 독립 검토는 승인 회수·원본 archive 연결을 대조했으며 P2/자격정보 metadata를 재조회하지 않았습니다. SDK 반환 관측은 UTC04:49:18.716430으로 원래의 새 실행 중단/전체 반환 시각 전입니다. 새 키 생성·키 이동은 없습니다.

## SDK manifest 첫 실패와 읽기 전용 교정

첫 aapt2 manifest 조회에서 SDK native 프로세스는 Exit0이었으나 바깥 parser/tool은 Exit1·RuntimeError로 끝났습니다. 당시 result의 manifest identity 수용값은 false이며 이 실패를 성공으로 덮어쓰지 않았습니다. SDK36 출력의 `minSdkVersion`을 기존 parser가 기대한 `sdkVersion`과 다르게 읽은 것이 원인이었습니다.

총괄은 같은 보존 capture를 읽기 전용으로 다시 해석해 localqa/code2/`1.0.197-qa2`·min25/target36·ARM64를 확인했습니다. SDK 재실행은 없었습니다. manifest/cert 공개 receipt와 signed-static qualification은 이 교정 기록·서명 result·payload comparison·정확 signed APK pin을 연결합니다. 인증서와 manifest 정적 확인은 인트로 전체 재생·Game·제품 수용을 대신하지 않습니다.

## LD Case01 갱신 설치

기존 localqa QA1 code1 위에 QA2 code2를 설치했고 clear/uninstall은 수행하지 않았습니다. 공개 결과는 일반 작업17개와 정리2개의 Exit0·failure null을 기록합니다. 독립 검토는 전후 package 기록에서 UID/dataDir/firstInstallTime 동일, code1→2와 단일 대상 패키지를 확인했으며 원시 식별값은 공개하지 않았습니다. 이는 해당 식별자 보존 확인이며 앱 데이터 모든 파일의 바이트 감사를 뜻하지 않습니다.

설치 APK readback의 실제 기록은100,492,368B/SHA `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33`으로 정확 target manifest/cert receipt 및 release 사본과 같습니다. 독립 LD 검토는 이 기록의 일관성을 수용했으며 설치 APK 바이트를 별도로 다시 열어 해시하지 않았습니다.

## 기본 cold 인트로 영상 관측

force-stop 후 기본 renderer로 시작했습니다. 특수 GLES30 실행은 사용하지 않았습니다. 원격/로컬 MP4는704,235B/SHA `0BA6964A92DFA60AE9D85FBB7D1F6FCE0E05BE6B8C23A88EEF7CBEB06C251764`으로 일치합니다. 총괄은2fps로 추출한900×1600 표본39개 중 game 영역을 관찰해 QuadD 조립, wordmark와 hold, fade 후 Lobby·닉네임 prompt를 확인했습니다. 초기 uncropped sheet와 원격 영상은 보존했습니다.

표본 프레임에서 Made with Unity는 관측되지 않았습니다. 모든 프레임·모든 기기의 splash 부재를 수용한 것은 아니며 오디오 실청과 frame 단위 정확 시각 판정도 하지 않았습니다. 독립 LD 검토의 영상/인트로 수용값은 false로 남아 있습니다. 이 문단은 이후 총괄이 발급한 별도 영상 관측 기록을 근거로 하며 독립 검토자가 영상을 수용했다고 표현하지 않습니다.

## 정리·반환과 남은 범위

Case01 종료에서 force-stop/home Exit0, source client inactive, Root config 승인 회수와 lease 회수, 원 full 시각 전 반환을 기록했습니다. LD 범위의 phone/SDK/Editor 호출은 각각0입니다. 독립 검토는 정리 명령의 정상 기록을 확인했으며 PID 부재나 전체 자식 종료까지 수용하지 않았습니다.

다음은 같은 QA2 Windows–LD의 실제 연결/경기와 Raw05 보행 분리 모드·소총/접지·최종 동작 관측입니다. 기본 cold 시작과 Lobby 도달을 Game 플레이 통과로 합산하지 않습니다. 휴대전화·실제16KB 기기·장시간 FPS/메모리/발열·전체 권리/제품 수용은 미완료이며 이번 회차 휴대전화 사용은0입니다. Draft353을 유지하고 운영34와 과거 QA1 자료를 보존합니다.

## 공개 근거

외부 루트 `D:/codex/validation/sudden-force-fps/20261110-work`의 공개 JSON을 실제 bytes로 읽어 아래 pin을 대조했습니다. 원시 로그·개인 식별값·자격정보·키 경로는 저장소에 옮기지 않습니다.

| 자료(위 루트 기준) | bytes | SHA256 |
| --- | ---: | --- |
| `raw05-qa2-existing-key-sign-candidate02/actual-game01/result.public.json` | 2,400 | `0FE278292A25AE9BBAB9118CA1F3A9FCC1C9B5BE95CDB13372AD949381B76C8F` |
| `raw05-qa2-existing-key-sign-candidate02/actual-game01/payload-comparison.public.json` | 72,819 | `33C0B91C93B2878EC98247A2BEFCE71E774E150E209501F74FD3C6A5FB19293A` |
| `qa2-signer02-actual-independent-review01/verification.public.json` | 4,527 | `9ADFC08105A82B076E5B9392F2B99E55E6AF457BA7A22F7098A1306905BF1228` |
| `root-resume01/signed-manifest-sdk01/result.public.json` | 1,208 | `F2D614A7374A62304235CF62D907A5968C43317B30CFFEBBB3445BA0D9875944` |
| `root-resume01/signed-manifest-sdk02-readonly-review.public.json` | 1,180 | `1DD5BF0DDEB73E8BD4DE3330B533CCCFFD9356893A2CF970F7712A7D458C2E9E` |
| `root-resume01/signer02-root-postaudit01.public.json` | 2,113 | `CE08E05120846BF5A5743A4E096AF3D6248E9A3DBDA0799E7AFF052F5BF4BC5C` |
| `ld-artifacts-game02/game-manifest-cert.public.json` | 1,418 | `5E53C9454084DB65E98AE64B54FABCE2D74A24AAB41302D5EF8B4E1EE997E3E5` |
| `root-resume01/qa2-signed-static-qualification01.public.json` | 2,031 | `6BED276739B73260E89CFA8C55CD693101F36CC7821D7D7CF720D53B22135243` |
| `root-resume01/signed-apk-public-snapshot01.public.json` | 1,808 | `600AF35D9488274788E3EA0F105A05114A00D5251879FBD261803233FB0CC5B2` |
| `root-resume01/ld2-qa2-case01-observation/result.public.json` | 4,021 | `D3FE6BCFAAADB2F1919E05B480FBB0762402E56B42EFEBDC01FAA99CC6FC2D6B` |
| `qa2-ld2-case01-independent-review01/verification.public.json` | 1,728 | `C026853DA2E3D311789FE6CB2CE4503599DFE76896A278173F604E92C33B0919` |
| `root-resume01/ld2-qa2-case01-observation/video-observation.public.json` | 898 | `96FDE6AEF5836069E655A3D9759B0510576B09FC5BE8B08C11E416AFAB93A55D` |
| `root-resume01/ld2-qa2-case01-actual-return.public.json` | 575 | `DBD496075E7279F516D3F46247BCC1ECC8148E12C81E92DD88317536D983F67C` |
