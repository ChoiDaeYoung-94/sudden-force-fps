# 코드34 모바일 입력 QA 산출물 (2026-10-27)

코드34/1.0.196의 Android 내보내기·패키징·기존 키 서명과 로컬 산출물 검증을 완료했습니다. **Play 업로드와 실제 Android 입력 수용은 미실행**이며 마지막 확인된 제공본은 코드33/내부 release11입니다. 이 문서는 완료된 빌드 기록 범위이고 #66/#43/#259 전체의 완료 선언이 아닙니다.

## 소스와 버전

- 기반은 main `ea74519dd466ec891430ba37b710c0a5e8cf88dd`입니다. Draft [PR261](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/261)의 입력 C#2개를 정상 병합한 `61218033ff36a2da3770ad93b9cc6c10c105c92d`를 별도 Android P12에서 빌드했습니다. 일반 main만의 출시 빌드가 아닙니다.
- QA recorder C#/meta2개를 추가한 공개 입력3997개를 고정했습니다. QA define은 기본 설정에서는 꺼져 있고 이번 Player의 extraScriptingDefines에만 추가했습니다. 로컬 숫자 계측은 실제 입력 성공을 대신하지 않습니다.
- Console 전체11개 업로드의 최대33을 확인한 뒤 권위원장을 `196,1,33 → 196,2,34`로 한 번 예약·저장·재확인했습니다. 예약 커밋은 `eb3a1f2`이며 실패·서명·전달 때문에34를 되돌리거나 다시 예약하지 않습니다. P12의 복사 원장33과 권위원장을 구분합니다.
- 버전1.0.196/code34, ARM64/IL2CPP, minSdk25/targetSdk36, exportProject·appBundle true, customKeystore false의 공개 설정을 저장하고 새 Editor에서 지속성을 확인한 뒤 내보냈습니다. 실제 서명은 기존 키를 사용하는 별도 단계였습니다.

## 내보내기와 패키징

Unity 내보내기는 GUID `d525a47bb57741dc8642bf73318c9a12`, result Succeeded, **오류2·경고68**, 369.7176초, 1,193,351,097B로 반환됐습니다. 동일 GUID의 오류2행은 CLI의 없는 명령 `editor status`와 `/api/exec` main-thread timeout4000ms로 독립 분류했습니다. 원래 오류 수를0으로 고치지 않았으며 개별 메시지 시각이 없어 둘 모두를 한 요청의 결과로 단정하지 않습니다. 이후 빌드 중 CLI status 요청은 피하고 별도 영수증을 확인합니다.

내보내기 후 입력3997개 중3974개는 바이트가 같았고 변화23개는 개행19개·CJK 빈 scalar 공백/개행1개·현재 ARM64/export/local Maven을 반영한 EDM3개로 분류했습니다. CJK glyph/atlas 값과 제품 C#/scene/prefab 변화는 없었습니다. before23은 Git blob과 기존 SHA에 맞춘 재구성이며 사전 물리 사본이라고 표현하지 않습니다. 패키징 후3997개 재해시도 분류 후 기준과 일치했습니다. 별도 QA 체크아웃의 private6개는 부재/indexS를 유지했습니다.

첫 Gradle 실제 패키징은 CMake prefab 배치 단계의 종료1로 실패했고 자료와 캐시를 보존했습니다. 공식 JDK ProcessBuilder의 네 가지 java-version 대조에서 자식 COMSPEC의 forward slash 경로는 실패하고 Windows native 경로는 성공했습니다. 고정 export 입력2819개만 새 출력 위치로 복사해 자식 COMSPEC 형식만 수정한 별도 recovery02를 검토·실행했습니다. Unity 프로젝트·키·기존 build/.gradle/.cxx 캐시와 전역 환경은 복사하거나 변경하지 않았습니다.

recovery02는 **2026-10-27 04:53:39 UTC에 종료0·176개 작업으로 완료**했고 실제 CMake 설정/빌드와 IL2CPP 컴파일을 수행했습니다. Unity가 이미 생성한 C++를 사용했으므로 Gradle에서 C#→C++ 변환까지 새로 수행했다고 주장하지 않습니다. 이 분리 export/Gradle 성공은 단일 Unity AAB 호출 안정화나 과거 실패 원인의 전체 해결이 아닙니다.

## 실제 산출물

로컬 폴더: `D:/codex/releases/sudden-force-fps/1.0.196-code34-input261-qa/`

| 파일 | 바이트 | SHA256 |
| --- | ---: | --- |
| SuddenForceFPS-1.0.196-code34-Input261-QA-unsigned.aab | 98,431,555 | `91E41873BF601BEB7EA6153B3958A7A052A02FDCD7D2FBA6A583B80FDEC5FFC6` |
| SuddenForceFPS-1.0.196-code34-Input261-QA-Signed.aab | 98,446,715 | `CDEE2D0F4A4C89EE98211341B3A4493926DDAC115DDBCD68AA31D86DFA72E13F` |
| SuddenForceFPS-1.0.196-code34-Input261-QA-UploadSigned.apk | 105,694,673 | `A5D0C34F8A0C1DB8BF7CDEB86BA67E604ECAF2F5B20F66174230D374B2CEDB75` |
| SuddenForceFPS-1.0.196-code34-Input261-QA-native-symbols.zip | 46,152,822 | `97DC867EC6BD49BEFF833BCD2F3F6F34E54D226EF4D40ECD1F3899F3785F78D6` |

로컬 복사본4개의 크기·SHA와 원본 일치를 독립 검토했습니다. APK는 사용자의 최신 지시에 따라 외부에 업로드하지 않았습니다. 기존 키를 유지했으며 키 생성·교체·이동·백업 업로드를 하지 않았습니다. 암호는 승인된 서명 도구의 stdin으로만 전달하고 argv/env/공개 출력·기록에는 넣지 않았습니다.

## 확인한 범위

| 검증 | 실제 결과와 경계 |
| --- | --- |
| 무서명 AAB | ZIP 전체 CRC150개, 공식 bundletool validate와 패키지 `com.AeDeong.SuddenForceFPS`·코드34·1.0.196·min25/target36·non-debug 확인 |
| 고지·광고/결제 제외 | 포함 TextAsset의 고지725,013B 본문 일치, 지정 DEX 네임스페이스17종0건·양성 클래스 대조, AD_ID/BILLING 권한 부재. 전체 외부 권리 판정과 구분 |
| native | ARM64 라이브러리7개의 LOAD·RELRO 정적16KB 검사. 실제16KB 기기 실행이 아님 |
| 아이콘·폰트 | 아이콘24개의 decoded alpha/visible RGB 대조, adaptive XML 존재, 원본 font7+meta7 보존과 자체 CJK 참조 검사. packed font 모든 glyph 렌더 수용은 아님 |
| 심볼 | native build ID/할당 payload 또는 debuglink CRC·함수 이름 대조6/7. `libnanosockets.so` 심볼은 누락 |
| 서명 AAB | 기존 업로드 인증서, jarsigner verify 종료0, 서명 항목을 제외한150개 payload 보존. trust chain 전체 판정은 아님 |
| 서명 APK | 공식 apksigner verify 종료0, V2/V3 true·V1 보고값 false, zipalign -P16 검사, 서명 항목 제외143개 payload 보존 |

실제 signed release 독립 검토는 `PASS_SCOPED_SIGNED_LOCAL_RELEASE34`, 차단0입니다. 근거는 외부 `D:/codex/validation/sudden-force-fps/20261027-work/android34-signed-release-actual-independent-review01/verification.public.json`(SHA `CA95A13D9201FB2E67139D0FE9879D51BA698C334B2E76E13F3E07BAAF165A4E`)과 core/aux/unsigned-signing-input 검토 자료에 있습니다. 원시 로그·키·암호는 공개 기록에 포함하지 않습니다.

## 보류와 다음 단계

허용된 LDPlayer FPS/index2의 기존 설치는 코드33이고 Play 앱 서명 인증서입니다. 이번 로컬 APK는 업로드 키 서명이므로 기존 앱을 그대로 업데이트할 수 없습니다. **기존 앱 삭제·데이터 초기화·서명 우회는 하지 않았습니다.** 기존33의 제한된 로비 관측을34의 로그인·경기·입력 통과로 합산하지 않습니다.

브라우저 제어 도구가 접근 승인 단계에서 거절돼 이번34의 Play 업로드는0입니다. 반환 메시지만으로 실제 Chrome 설정이 꺼졌거나 다른 프로젝트의 동시 접근이 원인이라고 확정하지 않습니다. 동일 설정을 반복 요청하거나 다른 Chrome 창·숨은 설정으로 우회하지 않습니다.

1. 도구 연결이 정상일 때 지정 작업용 Console의 Sudden Force 앱·현재 업로드 목록을 확인하고 서명 AAB34와 대응 심볼을 내부 테스트에 반영합니다.
2. Play 앱 서명 APK34를 로컬로 받아 무결성·패키지·버전·인증서를 검사하고 FPS의 기존 앱·데이터를 보존하며 업데이트합니다. APK 외부 전달은 하지 않습니다.
3. 기본 두 피어 경기와 같은 손가락 사격+LOOK·이동+사격의 서로 다른 해제 순서·MENU/사망/rebind 경계를 실제 Android 입력으로 검증합니다. Draft261은 수용 전까지 유지합니다.
4. 수용 후 일반 main 후속 빌드와 #66/#43/#259의 남은 범위를 별도 처리합니다. 실기기·실제 음향/발사 지연·전체 권리·16KB 실행·장시간 성능은 기존 후순위로 남습니다.

입력 recorder parser03의 합성41개와 독립 추가15개 검사는 통과했고 공개 SDK36 Java 컴파일도 별도 성공했습니다. 계측 도구 준비이며 실제 harness APK 설치·주입·기기 로그 판정·native 입력 수용은 아직0입니다. 현재 필요한 사용자 조치·질문은 없습니다.
