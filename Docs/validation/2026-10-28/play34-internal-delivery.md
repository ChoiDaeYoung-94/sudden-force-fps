# 코드34 내부 테스트 제공과 공식 Play APK 검증

2026-10-28 코드34/1.0.196을 Play 내부 테스트 release12에 제공했습니다. Console에 표시된 게시 시각은11:58 KST입니다.
빌드 source는 `61218033ff36a2da3770ad93b9cc6c10c105c92d`이며 Draft261 입력계측 QA 후보입니다.
제품 소스 기준5775에 반영한 PR282/287/288/290의 손·발소리·채팅 후속은 이 APK에 미포함입니다. #289는 좁은 채팅 수정으로 닫았고 #163 전체는 유지합니다.

## 제공과 로컬 APK

Root가 기존 지정 작업 창의 유일한 대상 Console 탭에서 `서명됨, 범용 APK`/106MB 행을 공식 다운로드했습니다.
제공·다운로드·창 map 영수증과 공개 JPG를 교차 검토했으며, 독립 검토자가 브라우저로 직접 다시 확인한 결과는 아닙니다.
패키지는 `com.AeDeong.SuddenForceFPS`입니다. 모든 테스터에게 Store 전파·설치가 끝났다는 뜻은 아닙니다.
로컬 파일: `D:/codex/releases/sudden-force-fps/1.0.196-code34-input261-qa/SuddenForceFPS-1.0.196-code34-Input261-QA-PlaySigned.apk`.
파일명: `SuddenForceFPS-1.0.196-code34-Input261-QA-PlaySigned.apk`.
크기105,690,443B / SHA256 `09403527CFC04B74B85942BB7DFAFB0E727FA30BB0B8660AB2EB010E116E7F76`입니다.
Play 인증서 SHA256은 `01B2846B34835CD145264048DD5C642B6F112D35AA8F0B601DBA11F49E100D86`로 기존33과 같습니다.
기존 로컬 UploadSigned의 업로드 인증서 `4D072592228C991D3F6A0EDA768C8CBEE40895C3B1F4A6FB9379540F048965E0`와 구분합니다.
원본 `C:/Users/user/Downloads/34.apk`와 기존 UploadSigned APK는 보존했습니다. APK의 Drive·원격 업로드는0입니다.

## 실제 정적 검증과 제한

Root의 공식 apksigner/zipalign/aapt2 검증 child3개는 각각 exit0입니다. 별도 certificate precheck의 공개 결과도 같은 단일 Play signer/v2를 확인합니다.
별도 precheck는 독립 PID/exit JSON이 없어 Root 공개 결과와 실행 source assertion으로 교차했으며 main3개 영수증과 구분합니다.
APK/AAB 전체 ZIP CRC, 중복/경로/암호화/크기 경계, 정확 ARM64 native7의 전체 uncompressed stream·각 static SHA가 일치했습니다.
비교한 frozen unsigned AAB는98,431,555B/SHA `91E41873BF601BEB7EA6153B3958A7A052A02FDCD7D2FBA6A583B80FDEC5FFC6`입니다.
APK binary manifest 직접 해석은 code34/name1.0.196/min25/target36/ARM64/nondebug와 AD_ID/BILLING 부재에 일치했습니다.
독립 검토는 APK v2 block의 실제 단일 certificate DER SHA도 대조했으며, SDK 도구를 다시 실행하지 않았습니다.
실제 APK의 native7은 모두 ZIP compression method8(Deflate)입니다. `zipalign -c -P 16 4` exit0을 압축 SO의 직접 mmap offset 정렬이나 실제16KB 기기 실행으로 확대하지 않습니다.
Console의16KB 호환 label도 기기 실행 수용의 대체 근거로 쓰지 않습니다. ELF LOAD/RELRO 전체를 이번 APK 검토에서 다시 파싱한 것은 아닙니다.
공개 증거8개·tool9개 bytes/SHA와 원본 다운로드/복사 APK를 다시 대조했습니다. source3997은 공개 manifest metadata만 읽었고3997개 원본 파일을 재해시하지 않았습니다.
선택 R8/proguard mapping 부재 경고1건은 남겼으며 frozen Gradle은 minifyEnabled=false입니다. 필수 검사 실패를 무시한 것으로 처리하지 않습니다.
심볼은6/7이고 nanosockets 심볼은 없습니다. Console 부착 표시는 전체7개 심볼 존재의 증명이 아닙니다.
실제 기기·16KB 기기·native 입력 시나리오·기본 기능·권리 전체·whole assets/fonts/DEX 동등성·glyph/render·성능은 미검증입니다.
검증기의 키/암호/환경 내용 접근·서명·설치/삭제·기기 제어는0이며 원래 업로드 키를 계속 사용한다는 합의는 유지합니다.

## 보존과 다음 단계

실행 config BFF16…27C6는 `run-config.consumed01.public.json`으로 보존하고 active `run-config.public.json`은 원래 false template(B8C84…25C3)로 retire했습니다.
단일 validation-attempt/실제 결과/독립 검토/실패 자료를 보존했습니다. 재시도 허용이나 새 실행 승인을 남긴 상태가 아닙니다.
release kit에 `play34-official-download.public.json` sidecar만 추가했으며 원래 frozen `release-manifest.public.json`은 변경하지 않았습니다.
원장 `196,2,34` 소비를 유지하고 새35 예약은0입니다. 이번 검증 때문에 키를 교체·이동하거나 앱/데이터를 삭제하지 않았습니다.
기존 Play33 앱·데이터를 보존하는 업데이트 설치는 후속입니다. 같은 Play 인증서라는 사실만으로 업데이트·로그인 성공을 처리하지 않습니다.
다음 순서는 QA34 native 입력 시나리오 수용 → Draft261 반영 판단 → 최신 일반 main의 새 Android 통합입니다. 다음 새 예약은35이상이며 아직 예약·빌드하지 않았습니다.

## 근거

외부 기준 폴더는 `D:/codex/validation/sudden-force-fps/20261028-work/`입니다. 아래 SHA는 실제 파일을 고정합니다.

| 근거 | 파일 | SHA256 |
|---|---|---|
|실제 APK 검증|`play34-apk-validation-prepare01/actual-30396095b01c429e8bfced14f7fba1a1/play-apk-verification.public.json`|`F01366539813D2CCD184014E8C1708D917903B69DE03A78E0CB4C832BC4FF8D9`|
|실제 독립 검토|`play34-apk-actual-independent-review01/verification.public.json`|`5FAF3791DA7BD305E6C3665FB8215D74CAF3DC7BEEF8BBEFA303A55BF0DBE55A`|
|Root 제공 관측|`play34-provided01.public.json`|`D1087127332DE105A73C58BE8B0FB8217C7E410D76FF964C694616CF0B373373`|

다운로드 출처는 `play34-download-origin01.public.json`, 창 식별은 `chrome-window-target-map01.public.json`, retire/sidecar 연결은 `play34-retired-validation01.public.json`을 함께 읽습니다.
검토 완료 범위는 정적 산출물과 Root 관측 근거의 교차 검토입니다. #43/#259/#41의 남은 수용을 닫는 자료로 확대하지 않습니다.