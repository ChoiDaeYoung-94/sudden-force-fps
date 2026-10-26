# Third-party asset provenance

## DOTween 1.2.765 선택 파일의 공식 배포본 대응 (2026-10-26, #257/#42)

[원저자 목록](https://dotween.demigiant.com/download.php)의 2024-02-04판 [1.2.765 ZIP](https://dotween.demigiant.com/downloads/DOTween_1_2_765.zip)을 외부 검증 폴더에서 확인했습니다. ZIP215,500B/SHA `B359376326F333D6A132E0CBE0DC7A9CF18EE6C8DE89739193E5438E27DBB6A0`의27항목 CRC를 검사했고 runtime DLL·Editor DLL·readme 세 파일은 저장소 원본과 바이트가 같습니다. runtime DLL은 새 Windows03 내장본과도 같습니다.

공개 Cecil metadata에서 `DG.Tweening.DOTween.Version`의 정적 대입값1.2.765를 읽었으며 assemblyVersion1.0.0.0과 구분했습니다. 검사한 DLL을 metadata reader가 로드·호출한 것은 아닙니다. [선택 파일 대응표](Tools/AssetLicenses/dotween-1.2.765-binary-provenance.json)를 따릅니다.

SDK 업그레이드·DLL/원래 readme·앱 고지 변경은 없습니다. ZIP에 별도 license 파일은 없고 readme가 원저자 URL을 가리킵니다. 현재 [라이선스 페이지](https://dotween.demigiant.com/license.php)는 참고로만 연결하며 2024년 조건·모든 module/생성 파일·과거 취득·Pro 여부·전체 권리 완료로 확대하지 않습니다. #42는 남은 범위를 유지하고 아래 이전 기록을 보존합니다.

## Kenney 콘크리트 발소리5종 (2026-10-25, #166/#42)

[공식 Impact Sounds1.0](https://kenney.nl/assets/impact-sounds)의 `footstep_concrete_000–004.ogg`를 mono WAV5개로 파생했습니다. 원 ZIP800,850B/SHA `029D734AF1582474EDF3A694D1B0CEBC97C1C152F2F39FA34D4C2BAFC5DE77F8`와 동봉 CC0 원문을 확인했습니다. 원 License608B/SHA `B49AA9C56B04528B95913DE13E506A0F7C5E807B9925DB9BFEF86AF1F91120DB`에서 줄바꿈만 LF로 바꾼586B/SHA `48D20ED2F4FD9146E76B7F558D09A1C27E62554D9897BEB93DEE57D269006D02`를 새asset 폴더와 앱 고지에 보존합니다. [공식 사용 안내](https://kenney.nl/support)와 원문은 상업적 사용과 선택적 credit을 설명합니다.

선택5멤버 SHA/CRC·파생 PCM5개/License·변환 수치의 독립 검토를 수행했고, 재생성 도구·manifest는 [자료](Tools/AudioAssets/KenneyFootsteps/README.md)에 있습니다. 원 손실 Vorbis의 stereo 평균/DC 보정/fade/하향 gain 파생이며 자체 녹음·원래 손실의 복구·전체 OS에서 바이트 재현으로 분류하지 않습니다. 변환용 Python decoder는 앱에 포함한 새 런타임 SDK가 아닙니다.

두 팀의 발소리 참조만4→5로 연결하고 기존 자체4WAV·meta/원 script/mixer를 유지했습니다. 앱 고지의 기존724,028B prefix는 그대로이며 새 출처·원문930B를 덧붙였습니다. 이5개 선택파일의 권리/출처 대응과 후속 소스 도입이며 제공33의 packed 결과를 바꾸거나, 전체 에셋 권리·새 Android 포함·실기기 청감을 완료한 것은 아닙니다. 아래 코드33의 자체 발소리4개 표는 당시 제공본의 기록입니다.

## 코드33 원 export 보고서의 포함 경로 대조 (2026-10-24, #42)

코드33 Native export의 원 BuildReport 파일(SHA256 `04E3759C2A17D4BABCA92E31942A80176CED8787DF1A50A0F9CBC5FD539B7869`)을 별도 편집 중인 P8에서 메모리로만 읽었습니다. 보고서 GUID `70c60f2a1f6c42f98b7cdf8e8814afab`·Android/Succeeded를 원 export 반환과 대조했고, 원 오류0/경고71·출력E33 조건은 공개 reader의 identity guard로 연결했습니다. 새 빌드나 최신 P8 소스의 보고서로 대체한 것이 아닙니다. 실제 제공본은 계속 코드33/release11입니다.

11 packed 그룹의6,831행에서 Assets/Packages 공개 상대 경로 **2,788개(Assets585+Packages2203)**를 수집했습니다. 공개 상대 경로 조건을 만족하지 않은32행은 출력하지 않았으며 민감 basename 후보는0행입니다. 이32행은 예전 보고서의 unique/empty 수와 단위가 달라 증감 비교하지 않습니다.

| 대상 | 현재33 공개 경로 수 | 확인 범위 |
| --- | ---: | --- |
| Military 본체·FPS 리그 자료 | 74 | Basemesh1·URP Materials18·Texture55 |
| LowPolyFPSLite 맵 | 17 | Materials2·Models13·Scenes1·Textures1 |
| LowPolyWeapons_LITE 소총 | 3 | Material1·Model1·Texture1 |
| 사용자 제작 확인 HUD | 7 | 기존 지정 PNG7개 |
| 자체 발소리 / 총성·재장전 | 4 / 2 | AuthoredStep01–04 / RifleShot·ReloadTick |
| 새 CC0 기반 Rifle clip/controller | 3 | Idle_Aiming·Death_Front·controller; 파생 Avatar는 별도 |
| 지정 옛 clip/controller·Loading Icons·EmojiOne·Font2·구61음원 | 0 | 기존 공개 목록의 exact 경로와 해당 prefix 한정 |

공개 코드29 목록2,787개와 직접 비교해 모두 공통이고 추가는 `ChatScrollDragObserver.cs`1개, 삭제는0개였습니다. 경로 집합 비교이며 에셋 객체/서브에셋·음원 원본·APK payload의 바이트 동일성은 아닙니다. 위 분류는 겹치는 집합이 있어 표의 숫자를 전체 수로 단순 합산하지 않습니다. 지정 옛 경로0도 다른 이름·복사·내장 객체의 전수 부재로 확대하지 않습니다.

보고서로 새로 로드한 객체18개를 모두 정리했고 원 Login scene clean·원 report 바이트 불변을 확인했습니다. SourceIR 뒤 미호출 컴파일과 단1회 actual 읽기, 공개 결과·분류의 독립 검토를 구분합니다. 근거는 외부 `20261024-resume/code33-packed-report03.public.json`, `report-read-actual-independent-review03/`, `code33-packed-classification01/`입니다. V1/V2 후보의 필터·dirty 비교 보완 이력과 미실행 상태는 보존했습니다.

이 결과는 세 공급자 팩의 실제33 포함 근거를 보강합니다. 기존 취득 확인을 되돌리지 않으며, 원 패키지 판본·정확 파일·동봉 조건 대응과 SDK 전체 BOM/고지·APK 객체 바이트는 여전히 후속입니다. 새로운 위반·배포 금지·전체 권리 완료를 판정하지 않았고 자산·license·앱 고지·계정·키를 바꾸거나 새 다운로드/동의를 수행하지 않았습니다.

## DOTween 동봉 readme의 출처 대응 (2026-10-24, #212/#42)

DOTween 기본 컴포넌트의 출처는 [동봉 readme](Assets/Plugins/Demigiant/DOTween/readme.txt)를 우선 연결합니다. 원문은 Daniele Giardini - Demigiant의 2014–2018 저작권과 원저자 license URL을 담고 있으며, 2,190B/SHA256 `37B99FFDBB8B01B159D807B1FE77DB664AE5B3B201734A5AD602C7C7F83941F0`입니다. 코드32의 실제 source manifest에 포함된 같은 파일과 바이트가 일치합니다. readme의 Pro 언급만으로 Pro 도입·취득을 추정하지 않습니다.

2026-10-24에 [원저자 license 페이지](https://dotween.demigiant.com/license.php)와 이를 가리키는 [공식 저장소 LICENSE](https://github.com/Demigiant/dotween/blob/develop/LICENSE)를 확인했습니다. 현재 웹 조건을 과거 동봉 DLL의 정확한 라이선스로 대체하지 않으며, 원 배포 패키지 버전과 당시 조건은 미확정입니다. 별도 공개 metadata 관측의 assemblyVersion `1.0.0.0`도 DOTween 패키지 버전이 아닙니다. readme의 업그레이드 설명에 있는 버전 번호 역시 설치 버전 근거로 쓰지 않습니다.

코드32 빌드 소스 `bce341b6ef387f29cbf5bff6d3491e866a3f375e`의 생성 입력 목록에서 `unityLibrary/src/main/Il2CppOutputProject/Source/il2cppOutput/` 아래 `DOTween.cpp`, `DOTween__1.cpp`, `DOTween__2.cpp`, `DOTween_CodeGen.c` 네 경로의 metadata를 확인했습니다. 이는 생성 입력의 경로·바이트·SHA 대응이며, DLL 본문·최종 네이티브 함수 생존·전체 배포 권리를 증명하지 않습니다. 별도 `UniTask.DOTween` 생성 항목까지 같은 조건으로 묶지 않습니다.

이번에는 출처 대응 기록만 추가했고 앱 고지 본문·DLL·에셋·패키지와 기존 원본 readme는 변경하지 않았습니다. 앱의 고지 의무 위반이나 전체 권리 완료를 판정하지 않으며 #42의 남은 범위를 유지합니다. 근거는 외부 `20261024-work/assets42-remaining-pc-gaps01/`의 원문 핀·공개 생성 목록·버전 metadata 한계 기록입니다.

## 코드30 고지 전문과 정적 검사 범위 (2026-10-23, #192/#42)

재장전·소매 후속 포함 코드30 AAB에서 고지724,052B 전문이 source와 exact이며 지정 광고/결제 DEX17prefix 참조/정의0·양성 대조·native7 LOAD/RELRO와 앱 아이콘24개의 알파/보이는 RGB를 확인했습니다. [산출물과 검사 한계](ANDROID_RELEASE_CHECKLIST.md)를 따릅니다. 기존 제공27과 구분하며 키·원시 로그·산출물은 Git에 넣지 않습니다.

새 BuildReport packed 실관측은 Editor 부재로 미실행이고29의 known/unknown 수를30 결과로 복사하지 않습니다. 개별 에셋/AAB 객체 대응·취득/공개 재배포 조건·전체 권리는 미완료라 #42를 유지합니다. 기존 사용자 제작/취득 확인을 미확인으로 되돌리지 않습니다. 아래는 각 시점의 이전 기록입니다.

## 코드29 packed 관측과 아이콘 정적 포함 (2026-10-22, #185/#42)

실제 코드29 BuildReport의 known-public unique2787개를 이전28과 같은 Assets/Packages 범위로 대조했습니다.2768→2787의 추가19/삭제0은 Military7개·자체 발소리4개·전용 팔Avatar1개·파생재질1개·자체C#6개입니다. Military는67→74이며 앞서 정적 참조 후보였던7개가 실제 목록에 포함됐습니다. 원문을 내보내지 않은 unknown unique3개와 empty entry5개는 별도이며 전체 경로/권리 확인으로 합산하지 않습니다.

BlueNoise7·무기3·맵17·사용자 제작 확인 HUD7은 목록에 남고, 옛23클립/controller·Loading Icons·EmojiOne·기존61음원·문제 Font2·외부SafeArea의 정확 경로는0입니다. 경로 부재가 이름 변경/내장 payload 전수 부재나 기존 취득 권리의 해결이라는 뜻은 아닙니다. 자체 아이콘3PNG의 packed 경로0은 native Android res와 별개이며, 실제 AAB의 PNG24/일반·원형 참조 및 투명 RGB만의 차이 분류는 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)를 따릅니다.

고지724,052B 전문 포함과 지정 DEX 검사는 확인했으나 전체 에셋 취득·조건·객체 대응은 미완료이므로 #42는 유지합니다. 근거는 외부 `20261022-evening/code29-packed-rights-review/`와 새 AAB 정적 검사이며, 기존 제공27에 소급 반영되지 않습니다. 아래는 각 시점의 기록입니다.

## URP BlueNoise256 7개의 원저자 대응 보강 (2026-10-22, #42)

코드28 known-public packed 목록의 추가7경로인 `Packages/com.unity.render-pipelines.universal/Textures/BlueNoise256/LDR_LLL1_[0..6].png`에 대해, 현재 설치 URP17.3.0의7개 파일과 [Christoph Peters의 공식 원본 ZIP](https://momentsingraphics.de/Media/BlueNoise/FreeBlueNoiseTextures.zip) 내 `Data/256_256/` 동명7개의 바이트·SHA256 일치를 확인했습니다. [원저자 페이지](https://momentsingraphics.de/BlueNoise.html)는 해당 texture database를 CC0로 공개한다고 명시합니다. 자세한 경로·해시·조건 범위는 [에셋 원본 대응 기록](Tools/AssetLicenses/README.md)과 [공개 대응표](Tools/AssetLicenses/urp-blue-noise-256-provenance.json)에 있습니다.

전체42,288,737B ZIP을 받지 않고 exact HTTP206범위로 중앙1743항목과 선택7개만947,003B 읽었으며, 파일명·복원 크기·CRC32를 검사했습니다. 전체 ZIP SHA·전체 아카이브 무결성, 코드28 AAB의 개별 image payload identity·모든 URP texture의 권리를 확인한 것은 아닙니다. 새 필수 고지 누락을 발견한 판정이 아니므로 제품 고지는 변경하지 않았습니다. 이 대응을 정리한 시점의 Military7개는 정적 참조 후보였으며, 이후 코드29 실제 packed 관측은 위 최신 기록을 따릅니다. 전체 #42는 남은 범위를 유지합니다.

## 로비·대기실 안전 영역을 자체 구현으로 통일 (2026-10-21, #157/#42)

로비·대기실 프리팹의 외부 `Crystal.SafeArea` 두 컴포넌트를 프로젝트 자체 `ScreenSafeArea`로 교체하고, 참조가 없어진 `Assets/ThirdPartyAssets/SafeArea.cs`와 meta를 제거했습니다. 기존 자체 스크립트의 GUID·경기 HUD 참조는 유지합니다. 새 의존성이나 외부 구현을 추가하지 않았습니다.

기존 파일은 공식 Safe Area Helper 상품과 동일한 지원 글 identifier를 갖지만, 정확한 원본 버전·조건 대응까지 확인된 것은 아닙니다. 기존 사용의 위반을 단정하거나 사용자님의 무료 취득·배포 가능 기억을 번복한 조치가 아닙니다. 이미 제공한 Android27의 packed source에는 해당 파일이 1개 포함돼 있으며 이번 소스 변경이 소급 반영되지 않습니다. 다음 Android 산출물에서 제외를 별도 확인해야 합니다.

유효 영역 7종과 잘못된 입력 24종, 두 프리팹·기존 경기 HUD의 격리 Editor 검증을 수행했습니다. 실제 노치·화면 회전·최신 Android 실행이나 전체 #42의 권리 확인 완료는 아닙니다. 구체적 범위는 [UI 검증 기록](Tools/UI/README.md)의 안전 영역 항목과 외부 `20261021-safearea-source-audit-01/`을 따릅니다. 이하 항목은 각 작업 시점의 기록입니다.

## Perfect DOS 글꼴의 원본 허용문·고지 보강 (2026-10-21, #155/#42)

실제 코드27 packed source에 포함된 Core17.3.0 Debug UI의 `PerfectDOSVGA437.ttf`에 대응하는 저자·출처와 허용·조건 관련 절을 앱 고지에 추가했습니다. 기존 722,942B 고지 prefix와 폰트·GUID·프리팹·씬은 유지합니다. 이 고지 변경은 이미 제공한 코드27/Drive APK에 소급 반영되지 않으며 다음 Android 산출물에서 전문 포함을 확인해야 합니다.

현재 작업 폴더의 고지는 724,028B/SHA256 `45C8F5A84BBB3B89EB2BB1509E97D3F1C6B0C379868EA7FC0211D59D9F0A5018`입니다. 실제 Unity TextAsset 바이트 일치와 기존 pager의 200페이지 원문 재조합, 최대 표시 4,089 UTF16자·93줄·FF27 보존을 확인했습니다. 폰트/UI 렌더·새 Android 포함 검증은 별도입니다.

Core의 81,192B/SHA256 `AB5F7798CE9D05AA09380FCE2142B11690BA3216A73208954F87C6C63A035102`는 [저자 표시 배포 페이지](https://www.dafont.com/perfect-dos-vga-437.font)가 연결한 원본 ZIP의 비 Win `Perfect DOS VGA 437.ttf`와 바이트가 일치합니다. 동봉 `dos437.txt`는 게임 사용·배포 허용과 폰트 판매 금지, release-form 관련 안내를 함께 담고 있습니다. [저자 직접 답변](https://www.dafont.com/font-comment.php?file=perfect_dos_vga_437)도 무료 사용 취지를 확인합니다. CC0/OFL·공용영역으로 재분류하거나 폰트 상품화 허용으로 확대하지 않습니다.

원본 ZIP50,530B/SHA256 `7B4AB0E257309EF6B2D4643DAFFDB7525F5B78A1E817A34F998A24DC8D865E24`, `dos437.txt`4,957B/SHA256 `2965098E9A46569D6B7E51E5C72F94D3FCC9DFF202965AA8F2791684EC7425B9`를 외부 감사 자료와 대조했습니다. Core DebugUI prefab26개의149필드는 legacy Font 참조이며 이번 작업은 폰트 교체가 아닙니다.

전체 #42 권리 확인은 유지합니다. `Crystal.SafeArea.cs`의 정확한 원 제공본·조건 대응과 기존 팩의 남은 원본/조건 범위는 별도입니다. 이전 사용자 제작/취득 확인을 미확인으로 되돌리거나 현재 packed에 없는 옛 자산을 새 앱 고지 누락으로 집계하지 않습니다. 근거는 외부 `20261021-remaining-license-gaps-01/`의 원본 배포 ZIP/readme, 실제 packed 경로와 폰트/GUID 대조, 적용·pager 검증 기록입니다.


## 코드27 고지의 실제 내부 배포·APK 전달 (2026-10-21, #42/#43)

고지722,942B/SHA256 `C5DE97CE407C5104B62869CA5090A076608FD26E9B6DB125A6267DBBA7D919CA` 전문을 실제 코드27 서명 AAB에서 재파싱해 원문과 대조했습니다. 해당 AAB를 내부 테스트 release8에 제공하고 Play 서명 APK도 기존 비공개 테스트 Drive 폴더에 전달했습니다. APK의 assets13개가 원래 AAB와 집합·payload SHA 모두 같으므로 동일 고지가 연결됩니다. APK TextAsset 전문을 새로 재파싱하거나 실제 기기 고지 화면을 확인한 결과로 확대하지 않습니다.

코드27의 packed source2,765개에서 새CC0클립2/메뉴폰트2/맵이미지2 포함, 옛22클립/controller·Loading Icons·EmojiOne 정확 경로0이라는 기존 검증을 유지합니다. SDK/고지 포함과 전체 취득·재배포 권리는 별도이며 #42는 닫지 않습니다. 소스 자산 삭제나 키/계정/공유 권한 변경은 없고 [실제 산출물·한계](ANDROID_RELEASE_CHECKLIST.md)를 따릅니다. 아래는 각 시점의 기록입니다.

## 코드26의 기존 오디오·Font2 포함 경로 재분류 (2026-10-18, #42)

실제 코드26 packed source path 2,768개와 현재 파일 목록을 대조했습니다. Assets/Audios의 현재 음원63개 중 자체 합성 Authored2개는 모두 packed에 있고, 나머지 기존 후보61개는 정확한 경로 기준0개입니다. mixer1개는 별도 포함됩니다. 과거2026-10-07의 개별61목록은 이번 범위에서 복원하지 못했으므로 현재61개가 당시와 같은 개별 원본이라고 단정하지 않습니다.

SimplePixelUI의 정확한 Font2 TTF 경로 Assets/ThirdPartyAssets/UI/SimplePixelUI/fonts/Font2/pixelfont2.ttf와 SimplePixelUI 전체 prefix도 packed0입니다. 기존 Font2의 동봉OFL/내부표기 문제를 해결한 것은 아니며 현재 메뉴용 폰트2개라는 수량과 구분합니다. 옛22클립/controller·Loading Icons·EmojiOne packed0, 새CC0클립2 포함이라는 기존 코드26 결과도 유지합니다.

캐릭터·무기·맵 경로와 사용자 제작 확인 HUD7개는 packed 목록에 남습니다. 기존 취득/제작 확인을 미확인으로 되돌리지 않습니다. 경로 부재는 복사·이름 변경·내장 payload 전수 부재나 원본 취득/저장소 공개 재배포 권리 확인이 아니며, 이번에는 미디어 binary·TTF내부·AAB객체를 다시 읽지 않았습니다. 제품·고지·원본 파일과 AAB는 변경하지 않았고 #42 전체는 열림 유지합니다.

기계 목록·근거 SHA·분류 한계는 외부 D:/codex/validation/sudden-force-fps/20261018-code26-provenance-reconciliation-01/의 reconciliation.json/REPORT.md에 보존합니다. 아래 기록은 각각 당시 상태입니다.


## 코드 26의 UI·고지·제외 경로 확인 (2026-10-18, #42)

기존 맵에서 촬영한 메뉴배경/맵카드2개와메뉴font2개를 포함한 코드 26 무서명AAB에서 고지 722,942B 전문일치·fresh packed2,768경로·새 CC0 플레이어 클립 2개·옛22클립/controller·Loading Icons·EmojiOne0을 확인했습니다. 새 외부 이미지 팩은 반입하지 않았고 기존 Map Lite와 글꼴의 취득/조건·원본 고지를 유지합니다. [UI자료](Tools/UI/README.md)와[코드 26검증](ANDROID_RELEASE_CHECKLIST.md)을따릅니다.

지정 광고/결제 17개 네임스페이스의 DEX 참조/정의는 0이며 전체 권리·객체 GUID 전수·모든 SDK 동작 검증 완료는 아닙니다. 현재 Play 마지막 확인 제공본은 **1.0.195/코드 21**이며 코드 26은 미서명·미업로드입니다. 최신 Android UI·Store 설치·Fusion 전체 경기·실제 16KB 기기는 별도 미검증입니다. 실기기 검증 후순위·장시간 성능 후속 합의를 유지하고 수동 시험이나 USB 연결을 지금 요청하지 않습니다. 따라서 #42는 유지하고 이전 문서는 당시 근거로 보존합니다.


## 기존 맵의 UI 촬영 이미지 (2026-10-18, #106)

`Assets/Art/UI/MapPreviews/DesertHouse.png`는 기존 DesertHouse 씬을 Unity에서 촬영한 파생 UI 이미지입니다. 새 외부 이미지 팩이나 AI 생성 이미지를 반입하지 않았고, 기존 Low Poly FPS Map Lite 취득·원본 대응 기록과 조건을 유지합니다. 원본 씬·모델·재질은 변경하지 않았습니다. 촬영 좌표·해시·import 설정은 [UI 자료](Tools/UI/README.md)에 기록했습니다. 맵 원본을 새 CC0 자산으로 분류하거나 #42 전체 권리 검증을 완료한 것은 아닙니다.

## 코드25 무서명 AAB 검증 완료 (2026-10-17, #66/#42/#43)

PR102의 CC0 소총 조준·사망 클립 2개와 PR103의 런타임 고지를 포함한 1.0.195/코드25 AAB를 생성하고 정적 검증했습니다. 공식 증분 export는 오류 0·경고 2·55.573초로 완료됐습니다. 버전 설정을 메모리와 디스크에 저장한 뒤 다음 Editor update에서 다시 확인하고 코드를 예약했으며, 생성된 Gradle의 실제 버전도 25/1.0.195로 확인했습니다. 별도 offline C1 Gradle의 새 IL2CPP 작업·종료 0·최종 native payload 일치를 확인했습니다. Unity 단일 AAB 호출 안정화나 충돌 근본 해결을 뜻하지 않습니다.

무서명 AAB 92,276,193B/SHA256 `AB1D91B08740B501BFBEF1BD9A5F2CD72799DD051541D9477CCE2E550B72F4A7`의 ZIP/bundletool·패키지·min25/target36·비개발·ARM64 SO 7개의 LOAD 16KB 정렬·고지 722,942B 전문 일치를 확인했습니다. 지정한 광고/결제 네임스페이스 17개의 DEX 참조/정의는 0입니다. fresh packed 경로 2,763개에는 새 클립 2개만 포함되며, 옛 클립 22개/controller·Loading Icons·EmojiOne 경로와 활성 씬 4개의 두 팩 의존성은 0입니다. 객체/GUID 전수 판독이나 전체 권리 대응 완료는 아닙니다.

대응 심볼 ZIP 46,085,333B/SHA256 `219A32C9D9B9B285C525A783077925BA4D363F94773D317C11FF7E5874F7437D`는 실제 AAB의 BuildID/ALLOC·GNU debuglink CRC 또는 전체 allocated bytes를 대조한 6/7개를 포함합니다. 누락: libnanosockets.so. libunity는 debug_line과 전체 ALLOC 일치를 확인한 unstripped 원본을 사용합니다. 파일 검증과 Console 업로드 여부는 구분합니다.

코드24는 export 성공 보고와 달리 생성 Gradle이 기존 코드10/1.0.194여서 dry gate에서 실패했습니다. 실제 패키징·AAB·서명·업로드는 모두 0이며 실패 기록을 보존했습니다. raw 복원 후 설정 재로드 시점은 원인 후보일 뿐 확정하지 않았습니다. 코드25에서는 2단계 저장과 실제 출력 검증으로 검사 공백을 보완했습니다. 코드23은 새 고지가 미포함된 검증용 무서명본으로 보존하고 배포하지 않습니다. 코드23 입력 감사의 최초 경로 판정 실패도 보존했으며, 정상 Source/il2cppOutput/compile-data.json을 독립 대조한 뒤 정확한 경로 1개만 허용해 재검증했습니다.

빌드 후 입력 감사 시점에 시작 파일 3,907개는 예약 원장 외 원래 바이트를 보존했고, 기존 dirty·원본·이전 release AAB 4개의 해시도 보존했습니다. 이후 변경은 이번 검증 기록입니다. 원장 `195,6,25`·다음 새 코드26 이상을 유지하며 실패/예약 코드는 재사용하지 않습니다. 키·암호·원시 로그·AAB는 커밋하지 않습니다.

**현재 Play 제공본은1.0.195/코드21이며, 코드25는 미서명·미업로드입니다.** 기존 업로드 키 서명 입력 후 서명/제공 검증이 남습니다. RELRO 정적 경고 5건·최신 Store 설치/기기/UI/전체 경기·실제 16KB 환경·전체 에셋 권리 검증은 별도입니다. 기기 검증 후순위·장시간 성능 후속 및 전체 약92% 작업 기준 추정은 유지합니다. 아래 기록은 각각 당시 상태입니다.


## 런타임 공급자 고지 최소 보완 (2026-10-17, #42)

코드23 생성·등록 코드와 로컬 공급자 원문을 대조해 Burst의 .NET 숫자서식 차용 MIT, Unity Companion License v1.4 전문과 Burst1.8.30/Collections2.6.8/Mathematics1.3.3 저작권, Collections xxHash 파생 BSD2 및 Mathematics Noise의 Ashima Arts/Stefan Gustavson MIT 고지를 추가했습니다. 설치 전용 패키지를 일괄 포함하지 않았으며 정확한 .NET/xxHash 원차용 리비전과 최종 네이티브 링크 생존 전수는 확정하지 않았습니다.

기존 고지710,172B prefix와 제안 원문12,770B를 그대로 보존해 새 고지는722,942B/SHA256 `C5DE97CE407C5104B62869CA5090A076608FD26E9B6DB125A6267DBBA7D919CA`입니다. 실제 Unity TextAsset 동일성·기존 pager의 전체 페이지 원문 재조합·최대 표시4,089 UTF16자/93줄·FF27 보존·컴파일 실패false를 확인했습니다. 새 UI 렌더링이나 기기 통과를 뜻하지 않습니다.

코드23 공식 export의 fresh packed 경로2,770개에는 새 CC0 플레이어2클립만 포함되고 옛22클립/controller·Loading Icons·EmojiOne 경로와 활성4씬의 두 팩 의존성은0입니다. 이는 BuildReport/씬 의존성 근거이며 AAB 객체/GUID 전수 판독이 아닙니다. 원본은 보존했습니다.

**현재 Play 제공본은1.0.195/코드21입니다. 이번 새 고지는 코드23 export에도 미포함이며 후속 코드24 이상 빌드가 필요합니다.** 원장195,4,23·실패/예약 코드 재사용 금지, 코드23은 검증용 보존·서명/배포 보류입니다. #42 전체 완료와 Unity 단일 AAB 호출 안정화·실제16KB·실기기/성능 통과로 확대하지 않습니다. 전체 약92% 작업 기준 추정과 기기 검증 후순위 합의를 유지합니다. 현재 사용자 조치·질문은 없습니다. 아래 기록은 각각 당시 상태입니다.

### 이번 고지의 출처와 적용 범위

- .NET: 설치된 BurstString.Float.cs의 고정 [dotnet/runtime75036ffe LICENSE](https://github.com/dotnet/runtime/blob/75036ffec9473dd1d948c052c041fdedd7784ac9/LICENSE.TXT) 차용 선언과 코드23 Unity.Burst.cpp/CodeGen 등록에 대응합니다. 전체 .NET 패키지 버전을 주장하지 않습니다.
- Unity: 설치된3패키지 LICENSE.md의 저작권·문구를 그대로 보존하고 [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license) v1.4 전문을1회 추가했습니다. 기존 uGUI/URP/Core 공급자 안내도 보존합니다. 별도 third-party 구성요소의 권리를 이 라이선스로 덮지 않습니다.
- xxHash: Collections xxHash3.cs의 명시적 upstream·192B 기본 secret 전체 일치·대응 함수와 생성/등록을 근거로 [공식 v0.8.2 참조 LICENSE](https://github.com/Cyan4973/xxHash/blob/v0.8.2/LICENSE)의 Yann Collet2012–2021/BSD2 전문을 보존했습니다. 참조 태그를 실제 로컬 차용 버전으로 확정하지 않습니다.
- Noise: Mathematics Noise12소스의 명시적 MIT/upstream·실제 생성/등록을 근거로 [stegu/webgl-noise 공식 LICENSE](https://github.com/stegu/webgl-noise/blob/master/LICENSE)의 Ashima Arts2011 및 Stefan Gustavson2011–2016/MIT 전문을 보존했습니다. 정확한 로컬 원차용 리비전은 미확정입니다.
- VisualScripting/2D 설치·packed MonoScript만으로 최종 런타임을 단정하지 않았고, LLVM 도구 전용·Ryan Juckett 소스 보존 조건·미확정 셰이더 variant를 일괄 필수 고지로 추가하지 않았습니다. 전체 권리 검증 완료 선언은 아닙니다.

근거는 외부 `D:/codex/validation/sudden-force-fps/20261017-remaining-packed-assets-audit-01/`의 CODE23-AND-ISSUE42-REPORT, minimum-notice-proposal-sources/xxhash-evidence/notice-application/notice-editor-validation JSON과 fresh 코드23 packed 보고서입니다. 키·암호·원시 로그·AAB는 커밋하지 않습니다.


## CC0 소총 생명 애니메이션 소스 전환 (2026-10-17, #42)

Quaternius 공식 CC0 자료를 기존 Military Avatar에 리타깃한 조준·사망2클립과2상태 controller를 추가하고 SoldierBlue/Red에 연결했습니다. 양손 소총 그립·호흡 루프·뒤로 넘어지는 사망·마지막 자세 유지가 포함됩니다. 기존 상태 이름과 Avatar/네트워크/히트박스/무기 바인딩·루트모션 비활성·독립3초 리스폰은 보존했습니다. 기존22클립/컨트롤러는 삭제하지 않았습니다.

저장된 제품 프리팹의 실제 Editor Animator를 양 팀·원점/이동 및 yaw137에서 Idle→Death→Idle로 평가했습니다. 키 사이·비정규 시각 포함3,144회/12경우가 같은 기준을 통과했고 최소 표면 여유5.003mm·최대 사망 그립 오차3.773mm·총구 편차0.757도·루트 이동/회전0입니다. 원본 Generic Humanoid 직접 임포트 실패와 초기 침하/보간 실패는 보존했습니다. 전완 회전 경계는 동일 뼈 자세의 주기4를 실측한 뒤 연속화했고, 새 높이 내장 클립만 legacy 바닥 보정을 끕니다. 기존 보정 기본값true/실행 경로도 확인했습니다. 이는 분리된 Editor 재생/guard 검증이며 실제 Fusion snapshot 전환·전체 경기·기기 통과가 아닙니다.

공급자 선언은 원본372B의 끝 공백·빈 줄만 정리해 369B로 보존했고 문구 변경은0입니다. 기존 고지709,379B prefix를 바이트 그대로 보존해 고지는710,172B/SHA256 `6EAC1F6A808BE68E30E3BA25E6BAAD1D628F58427219471116FD8D0FB35DFD53`입니다. 두 클립의 재생성 입력/명령·원본 SHA·수정 내역은 [애니메이션 자료](Tools/AnimationBaking/README.md)에 기록했습니다. 두 Soldier의 정적 의존성과 실제 packed 제외는 구분하며 후속 빌드로 기존22클립 포함 여부를 확인해야 합니다. #42 전체 권리 대응 완료로 닫지 않습니다.

추가 분리 Play 검증에서는 프레임 증가·양 팀 조준 루프·사망 마지막 자세 유지·조준 복귀를 확인하고 화면을 보존했습니다. 네트워크 Behaviour는0개 활성/실제 spawn0이며 Animator 상태를 직접 전환했으므로 Fusion snapshot·실제 리스폰 검증으로 확대하지 않습니다. 임시 오브젝트0·기존 clean Untitled 복귀·기존22원본 보존·두 Soldier 정적 옛 클립 의존성0·컴파일 실패false/현재 Console 오류0을 확인했습니다. SDK 구식 API/URP 호환 모드와 검증용 body 위치 조회 경고는 남겨 두었으며 경고0으로 보고하지 않습니다. Play 중 재직렬화된 EDM3설정은 사전 기준값으로 저장 복원하고 이 변경에 포함하지 않았습니다.

**현재 Play 제공본은1.0.195/코드21이며 이 애니메이션·고지는 아직 미포함입니다.** 이번 소스 작업의 새 AAB/서명/업로드/예약은0, 원장 `195,3,22`·다음 새예약23 이상입니다. 기기 검증 후순위·성능 후속과 전체 약92% 작업 기준 추정은 유지합니다. 다음 PC 우선순위는 새 소스 빌드·packed 의존성/고지 검증입니다. 현재 사용자 조치·질문은 없습니다. 아래 기록은 각각 당시 상태입니다.

## Editor 도구 변경: 공식 Unity EDM2.1.1 (2026-10-17, #98)

공식 Unity Registry `com.unity.external-dependency-manager@2.1.1`를 Editor 도구로 추가했습니다. 공급자 패키지의 Unity Companion License와 포함 Google resolver의 Apache2/MiniJSON MIT 고지는 패키지 원문으로 보존합니다. 새 런타임 에셋이나 광고·결제 SDK 도입이 아니며 플레이어 고지 본문709,379B는 변경하지 않았습니다. 구형 EDM4U1.2.182 DLL 원본도 삭제·변조하지 않고 importer만 비활성화했습니다.

현재 Play 코드21은 전환 전 빌드이며 코드22는 공식 export·별도 dry-run까지만 검증했습니다. 이 도구 변경을 기존 플레이어22애니메이션의 권리 대응 완료나 새 AAB 배포로 확대하지 않습니다. 실제 앱·기기 검증은 별도이며 아래 출처 기록은 각 자산의 기존 근거를 유지합니다.

## 코드21 서명·Play 내부 테스트 제공 완료 (2026-10-17, #43/#66/#42)

2026-10-17 **18:23**, 총괄이 기존 사용자 지정 작업용 Chrome 탭에서 코드21(1.0.195)을 직접 업로드·출시했고 release7의 **내부 테스터에게 제공됨**을 확인했습니다. 자체 로딩·원형 HUD(PR96/PR97)가 포함된 새 제공본이며 코드20과 이전 산출물은 보존했습니다. 참여 링크는 https://play.google.com/apps/internaltest/4701524132186452960 입니다.

서명 AAB92,645,187B/SHA256 `9494682DCC907916D3AD90D9882DAB191FF0FC8F76BFE43D760E9CC25644866E`의 기존 업로드 인증서·jarsigner 검증·129개 payload 전체 서명과 무서명본 동일성·ZIP/bundletool·패키지/코드21·min25/target36·ARM64 LOAD16KB·고지709,379B 전문 일치를 확인했습니다. 대응 심볼 ZIP34,234,538B/SHA256 `8DA3F71458A24A20157EDA0A66F6AB4E34A93D6F99D5473F298E672D3FE821E4`를 실제 코드21 행에 첨부했습니다. 커버리지는5/7개이며 libunity/nanosockets는 누락, debug_line은 libmain/Swappy만 있습니다.

서명 후3,883개 소스/설정은 코드21 빌드 복원 기준과 모두 같고 기존 dirty·이전 AAB6개를 보존했습니다. 암호는 서명 자식 환경에만 전달한 뒤 Editor 메모리에서 비웠고 정상 종료했습니다. 원장 `195,2,21`·다음 새코드22이상을 유지합니다. 키·암호·원시 로그·AAB는 커밋하지 않습니다.

공식 증분 export와 새 IL2CPP 컴파일·별도 offline C1 패키징을 거친 산출물입니다. Unity 단일 AAB 호출 안정화·충돌 근본 해결·최신 Store 설치/기기/UI/전체경기·실제16KB·전체 에셋 권리는 별도입니다. RELRO 정적 경고5건과 Console의 가독화 매핑 파일 경고1개도 남습니다. 실기기 후순위·성능 측정 후속 합의와 전체 약92% 작업 기준 추정을 유지하며 현재 사용자 조치는 없습니다.

다음 PC 우선순위는 #98 공식 EDM 전환과 CC0 애니메이션 후보 리타깃/소총 자세 검증입니다. 아래 무서명 대기·코드20 제공·이전 기록은 각각 당시 상태입니다.

## 코드21 무서명 AAB 검증 완료 (2026-10-17, #66/#42)

자체 로딩·원형 HUD(PR96/PR97)를 반영한 **1.0.195 / 코드21 무서명 AAB**를 생성·정적 검증했습니다. 공식 증분 export는 오류0·경고2·183.062초, 별도 offline C1 Gradle은 exit0·1335.0초입니다. 이번에는 변경 소스의 새 IL2CPP 작업을 실행했고, 코드20 네이티브 재사용 없이 새 라이브러리와 최종 stripped payload 일치를 확인했습니다. Unity 단일 AAB 호출 안정화·충돌 근본 해결·기기 실행을 의미하지 않습니다.

AAB는92,631,531B/SHA256 `DC81AE7FA302CC4C05DA92BE5247A7A7A940C9F672D302007258F3E0F4EAB781`입니다. ZIP/bundletool·패키지·min25/target36·비개발·ARM64 SO7개 LOAD16KB·고지709,379B 전체 본문 일치와 지정 광고/결제 SDK17네임스페이스의 DEX 참조·정의0을 확인했습니다. 패킹 보고서의 Loading Icons 경로는0건이며 플레이어22클립은 모두 포함돼 있습니다. 실제16KB·RELRO 경고5건·기기/UI/전체 경기·전체 에셋 권리 확인은 별도입니다.

**현재 Play 제공본은 코드20입니다. 코드21 서명·업로드는 아직 대기입니다.** 원장 `195,2,21`·다음 새코드22 이상을 유지하며 실패 코드도 재사용하지 않습니다. 시작3,883개 파일은 원장 외 동일하도록 복원했고 새 Gradle backup2/meta만 외부 보존했습니다. 기존 dirty와 이전 AAB6개 해시를 보존했고 키는 읽거나 해시하지 않았습니다. 실기기 후순위·성능 측정 후속 합의와 전체 약92% 작업 기준 추정을 유지합니다.

다음 PC 우선순위는 코드21 서명·내부 테스트 반영, 반복 Gradle 안내창의 공식 EDM 전환 검증([이슈98](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/98)), CC0 애니메이션 후보의 리타깃/소총 자세 검증입니다. 후보 확보만으로 현재22클립 출처 대응을 완료 처리하지 않습니다. 아래 자체 HUD·코드20 기록은 각각 당시 범위입니다.

### 애니메이션 원본 대응 후속

확인한 새 PC 기본 캐시·Downloads·검증 경로에서 당시 원 패키지를 찾지 못했습니다. 기존 캐릭터 취득 확인과 기존22클립의 미확인 상태는 유지하며 위반으로 단정하지 않습니다. [Quaternius 공식 CC0 배포](https://quaternius.com/packs/ultimatemodularcharacters.html)의 Animations.fbx7,695,388B/SHA256 `259ED6F6E5F1BB2A15215A66A9ED02E8956083A281A8098103490D1A00292B7F` 및 공급자 라이선스372B를 외부 확보했습니다. 실제 Idle_Gun_Pointing1.667초·Death1.067초의 본/곡선은 기술 후보입니다. 조준은 한손 총기 계열·사망은 정적 추정상 후방이며 원본 Avatar/T-pose·소총 양손 자세·GroundDeathPose·실행 품질은 미검증입니다. 현22개/컨트롤러/프리팹을 바꾸지 않았고 이번 코드21에도 포함돼 있습니다. 상세 원본·SHA·구체적 제안은 외부 `20261017-animation-replacement-candidates-01/REPORT.txt`에 보존했습니다.

## 후속 소스: 자체 원형 HUD 검증 (2026-10-17, #42)

CanvasJoyStick의 테두리 1개와 DesertHouse HUD의 테두리·손잡이 2개를 자체 `CircleGraphic` 메시로 대체했습니다. 기존 RectTransform·색상·활성 상태·레이캐스트·입력 바인딩을 보존했고, 두 자산의 Animated Loading Icons 의존성은 0건입니다. 이전 자체 로딩 표시와 함께 남은 직접 spritesheet 참조 3개를 제거한 소스 변경입니다.

Unity 6 컴파일·저장 자산의 누락 스크립트 0건·분리된 Play 렌더링과 조이스틱 두 입력 경로의 합성 포인터 콜백을 확인했습니다. 실기기 터치·전체 경기·새 AAB 검증으로 확대하지 않습니다. 현재 Play 제공본은 **1.0.195 / 코드20**이며 이번 HUD와 이전 자체 로딩 변경은 포함되지 않았습니다. 새 빌드·서명·업로드·예약은 0건, 원장 `195,1,20`·다음 코드21 이상을 유지합니다.

외부 원본 팩·고지는 보존했고 팩 전체의 배포 제외나 에셋 권리 검증 완료를 주장하지 않습니다. 다음 PC 우선순위는 플레이어 애니메이션 22클립의 당시 패키지 대응과 후속 소스 빌드입니다. 실기기 후순위·전체 약92% 작업 기준 추정은 유지하며 현재 사용자 조치는 없습니다. 아래 자체 로딩·코드20 기록은 각각 당시 범위입니다.

### 검증 범위와 보존 근거

- 자체 링은 64분할·내측 반지름 비율0.88, 손잡이는 채운 원이며 외부 sprite/texture/clip을 사용하지 않습니다. 링256정점/384인덱스와 원66정점/192인덱스의 실제 CanvasRenderer 메시·흰 기본 텍스처·350/200/100 크기 및 렌더 화면을 확인했습니다. 색상·마스킹·원래 raycast true/false를 보존했습니다.
- 씬의 나머지1,139컴포넌트는 적용 전후 Editor JSON이 같고, 프리팹의 나머지16컴포넌트와 저장 YAML 블록도 보존했습니다. 프리팹 저장 중 현재 JoyStick 코드에 없는 과거 필드4개만 제거됐으며 현재 핸들·영역 참조와6개 EventTrigger 바인딩은 그대로입니다. 제거된 Image3개의 외부 컴포넌트 참조는 없습니다.
- 실제 저장 프리팹을 격리 Play에 인스턴스화해 JoyStickSafeArea·Img_touchableArea 각각의 저장 PointerDown/Drag/PointerUp 콜백으로 캡처·이동·다른 포인터 무시·해제를 확인했습니다. public Cancel 및 비활성화 초기화도 확인했습니다. 물리적 터치·GraphicRaycaster 전달·Android·Fusion 경기의 직접 검증은 아닙니다.
- 첫 적용 helper의 Editor JSON 루트 해석 오류는 저장 전 임시 프리팹에서 실패했고, preview scene 저장은 Unity가 거부해 일반 씬으로 적용했습니다. QA helper의 두 입력 경로를 하나라고 가정한 검사와 렌더러가 없는 임시 복사본도 실패 후 수정·재검증했습니다. 제품 원본 세 객체의 CanvasRenderer는 보존됐고 새 타입에는 RequireComponent도 명시했습니다. 실패를 통과 건수로 합산하지 않습니다.
- 원래 깨끗한 빈 씬의 루트·컴포넌트 JSON 동일성·timeScale1·playModeStartScene 복원을 확인하고 임시 자산은 외부로 이동했습니다. Editor는 정상 종료했으며 자동 저장된 폰트·Player/SceneTemplate/Time 설정4개는 시작 사본으로 복원했습니다. 시작3,881파일 중 변경은 두 자산뿐, 추가는 자체 코드/meta2개, 누락0입니다. 기존 dirty·원본 팩·고지·예약 원장과 키 파일은 보존했고 키는 읽거나 해시하지 않았습니다.

근거는 외부 `D:/codex/validation/sudden-force-fps/20261017-authored-hud-01/`의 inspection/apply-scene-result/saved-validation/render-validation/joystick-callback-result/serialized-preservation/preview-restored/preservation-result JSON 및 hud-preview.png입니다. 저장 시 생성된 새 YAML 이름 필드2줄은 공백만 정리해 `git diff --check`를 통과했습니다. 이전22클립 감사의 원본 대응 한계는 그대로이며 #42를 닫지 않습니다.

## 후속 소스: 자체 로딩 표시 검증 (2026-10-17, #42)

LoginCanvas·Manager의 로딩 표시3개를 자체 `LoadingSpinnerGraphic`으로 대체하고 외부 로딩 Animation7개 참조를 제거했습니다. 외부 sprite/clip을 쓰지 않는270도 링이며 시간 정지에도 회전하고 raycast를 막지 않습니다. 두 프리팹의 나머지224컴포넌트·61오브젝트 바인딩/레이아웃/활성 상태를 보존했습니다.

Unity6.3 실제 컴파일 타입 로드/실패 플래그false와 저장 프리팹의 외부 LoadingIcons 의존성0을 확인했습니다. 격리한 Editor Play 미리보기에서 세 링의192정점/288인덱스·렌더 화면·프레임165→1494 진행·timeScale0회전·숨김각도 유지와 재표시 후 회전 재개를 확인했습니다. 제품 로그인/경기 전체 흐름·기기/AAB 검증을 뜻하지 않습니다. 초기 미저장 씬의 추가 생성·이름 변경만을 통한 격리는 실패했고 저장 사본/공식 playModeStartScene으로 수정했습니다. 임시 관측 helper의 GetMesh 인자 오류도 수정 후 검사했으며 제품 컴파일 오류와 구분합니다.

미리보기 임시 씬/meta를 외부 보존하고 원래 깨끗한 빈 씬·기본2오브젝트·timeScale1을 복원했습니다. 자동 저장된 폰트와 Player/SceneTemplate/Time 설정4개는 시작 해시의 바이트로 복원했습니다. 시작3,879파일 중 기존 변경은 두 프리팹뿐이며 신규는 자체 코드/meta2개, 누락0·기존 dirty·원장·원본 팩 보존을 확인했습니다. LoginCanvas의 TMP 현행 직렬화 필드 전환은 기존 로드 시점 값과 같으며 텍스트·폰트 스타일 재설계가 아닙니다.

플레이어22클립 추가 읽기 감사: 최초 반입1de985d(2024-11-14)과 개행 정규화 본문/GUID·컨트롤러가 같고 반입 FBX도 바이트가 같습니다. FBX의 Take001/5곡선은 본5개의 Visibility이며366키 값은 모두1.0이라22동작의 원본 대응 근거가 아닙니다. 현재 직접 재생 상태는 Idle_Aiming·Death_Front지만 컨트롤러22참조를 나머지20개 출하 제외로 판단하지 않습니다. 기존 캐릭터 취득 확인은 유지하며 당시 패키지의 별도 애니메이션/제공 버전 대응은 미확인입니다.

근거: 외부 `D:/codex/validation/sudden-force-fps/20261017-authored-loading-01/`의 inspection/apply-result/preview-sample-a·b·c/preview-reenabled/preview-restored/preservation-result JSON·spinner-preview-all.png와 독립 코드/애니메이션 감사. 외부 원본 팩과 고지 본문은 변경하지 않았습니다.

현재 Play 제공본은코드20이며 **이 소스 변경은 코드20에 포함되지 않았습니다.** 새 빌드/서명/업로드/예약0, 원장195,1,20·다음21 이상입니다. 단종 팩 원본은 삭제하지 않았고 DesertHouse의spritesheet참조2개와 CanvasJoyStick.prefab의1개는 남습니다. 팩 전체 배포 제외·전체 권리 완료로 처리하지 않고 #42를 유지합니다. 실기기 검증 후순위와 전체 약92% 작업 기준 추정을 유지합니다.

다음 PC 우선순위는 남은 원형 HUD 이미지3참조의 자체 표시 대체와22클립 원본 대응입니다. 아래 코드20 제공 완료·이전 고지 기록은 각각 당시 범위입니다.

## 코드20 서명·Play 내부 테스트 제공 완료 (2026-10-17, #66/#42/#43)

2026-10-17 16:33, 총괄이 사용자 지정 작업용 Chrome의 Sudden Force/패키지·내부 테스트 트랙을 확인한 뒤 코드20(1.0.195)을 직접 업로드·출시했습니다. Console의 release6 **내부 테스터에게 제공됨** 상태를 확인했습니다. 현재 제공본은코드20이며 코드18/19 산출물과 이전 검증은 보존했습니다.

서명 AAB92,843,051B/SHA256 `7C051D98CB68E70878F4EFC18E50CFA67E5D192910DEA58802AE8EF908F9DA61`의 기존 업로드 인증서·전체129payload 무결성/무서명본 동일성·ZIP/bundletool·min25/target36·ARM64 SO7개 LOAD16KB·고지709,379B 본문 일치를 확인했습니다. 서명 후 시작 기준3,879개 소스/설정은 예약 원장 외 정확히 같으며 기존 dirty를 보존했습니다. 원장195,1,20·다음 새예약21 이상입니다.

추가 MIT 고지의 실제 서명 AAB 포함과 배포 반영을 확인했습니다. 전체 에셋 취득/원본 대응·앱 UI/실기기 고지 표시 완료로 확대하지 않습니다.

증분 export·동일 입력2,734개/NDK7,935개 대조를 통한 코드19 IL2CPP 산출물 재사용·코드20 새 CMake/Swappy 패키징 결과입니다. 코드20 새 IL2CPP 컴파일·Unity 단일 AAB 호출 안정화·충돌 근본 해결이 아닙니다. RELRO4건·코드20 Store 설치/로그인/Fusion 경기·실제16KB는 후순위 미검증입니다. 실기기/장시간 성능 합의와 전체 약92% 작업 기준 추정을 유지하며 지금 추가 수동 시험·USB 연결을 요청하지 않습니다.

아래 항목의 원장·다음 코드·미완료 표현은 각 시점의 이전 기록입니다.


## UnsafeCollections 메모리 보조 부분 MIT 고지 보완 (2026-10-17, #42)

2026-10-17 추가: Photon Fusion 공급자 선언과 실제 코드19 생성 입력에 대응하는 UnsafeCollections 메모리 보조 부분의 MIT 고지1건을 소스에 추가했습니다. 새 소스709,379B/SHA256 `F35330D344D7209A617A8CFC919E2A2FDC26E6BC30090E3EAA26B6D76EBB9790`의 기존707,770B prefix를 바이트 그대로 보존했고, 공식 고정 LICENSE 전문·pager197페이지 원문 round-trip·Editor TextAsset 바이트와 에셋 GUID 보존을 확인했습니다. 앱 UI·후속 AAB 포함·기기 표시는 미검증입니다. 코드19은 이전707,770B 고지의 검증용 무서명 AAB로 보존하며 서명/업로드0, 현재 Play 제공본은 코드18입니다. 원장195,0,19·다음 새코드20이상·전체 약92% 추정과 기기 검증 후순위 합의를 유지합니다.

공식 [Photon OSS PDF](https://doc.photonengine.com/docs/content/OSS-Photon_Fusion.pdf)3쪽의 MIT/Fredrik Holmstrom2019 선언과 코드19 `Fusion.Common.cpp`의 `Expand`(15546)·`MemCpyFast`(15584) 구현/`Fusion.Common_CodeGen.c`(891/892) 등록을 [고정 upstream Native.cs](https://raw.githubusercontent.com/fholm/UnsafeCollections/aeff810906e78efe1b5184c5216495dddb328b8b/UnsafeCollections/Assets/UnsafeUtils/Native.cs)에 대조했습니다. 메모리 보조 부분의 직접 대응이며 전체 UnsafeCollections 패키지 포함·정확 로컬 버전/BOM·최종 네이티브 개별 함수의 링크 생존을 주장하지 않습니다.

[공식 LICENSE](https://raw.githubusercontent.com/fholm/UnsafeCollections/aeff810906e78efe1b5184c5216495dddb328b8b/LICENSE)1083B/SHA256 `F28D78D6FB64034ABDC9174226915DDC93813EF53F64E99CBC446D34A4A49001`의 저작권·허가·면책 전문을 CRLF 개행만 정규화해1103B로 복사했습니다. 기존 전체 raw prefix와 문구를 보존했으며 저작권/고지 중복은 없습니다. 현재 upstream 트리에서 별도 NOTICE가 없던 확인을 과거 공급자 배포본 전체의 부재로 확대하지 않습니다.

조건부 항목도 대조했습니다. Fody는 빈 `ProcessedByFody` 표식이고 InlineIL은 공식 설명상 IL을 삽입한 뒤 참조를 제거하는 빌드 도구이므로 앱 고지 필수 누락을 확정하지 않았습니다. Oswald는 Fusion EditorResources 경로이며 Android 포함 근거를 발견하지 못했습니다. WebSocketSharp는 WebGL 한정 asmdef와 별도 DLL의 Android 허용 importer를 구분했고 코드19의 생성/등록·독립 export 파일에서는 발견하지 못했습니다. 설정만으로 전체 제외를 단정하지 않으며 최종 packed/resource 범위는 별도입니다.

검증 근거는 외부 `D:/codex/validation/sudden-force-fps/20261017-unsafe-native-notice-01/`의 sources.json·원문 사본·pager-validation.jsonl·editor-apply-result.json과 독립 검토입니다. 제품 pager 수정0, 현재 추가 빌드/서명/업로드/예약0입니다. 에셋22애니메이션·단종 LoadingIcons 원본/취득 대응과 남은 기기 확인 때문에 #42는 열림 유지합니다.

## 코드19 무서명 AAB의 새 고지 포함 (2026-10-17, #42)

2026-10-17 코드19: Unity6000.3.25f1의 새 공식 Gradle export가 오류0·경고71·448.111초로 완료됐습니다. 이어 별도 C1 JVM·명시4GiB·processor1/worker1·parallelfalse 및 IL2CPP/Bee count1 조건의 offline Gradle bundleRelease가 exit0으로 끝나, 버전1.0.195/code19의 무서명 AAB 92,829,022B/SHA256 `EE26786D896B590FE5CDA48597BA240708EBBB7D2E18241991BE3A89C4C46693`를 생성했습니다. ZIP/bundletool·패키지·min25/target36·비개발·ARM64 SO7개 LOAD16KB와 새 고지707,770B의 본문 바이트 일치를 확인했습니다. 원장 `195,0,19`는 유지하며 다음 새 코드는20 이상입니다. **코드19 서명·업로드·기기 실행은 아직 미검증이고 현재 Play 제공본은 코드18**입니다. export와 별도 패키징의 성공이며 Unity의 단일 AAB 호출 안정화나 과거 충돌의 근본 해결로 확대하지 않습니다.

후속 SDK 조사에서 UnsafeCollections 메모리 보조 부분의 공급자 MIT 고지1건을 추가로 확인했습니다. 따라서 코드19은 검증용으로 보존하고 서명·Play 업로드를 진행하지 않습니다. 공식 원문을 포함한709,379B 후보의 prefix 보존·pager197페이지 검사를 완료했으며, 소스 보완 후 새코드20이상에서 실제 고지 포함과 배포를 이어갑니다. 코드19의 생성 성공과 배포 준비 완료를 구분합니다.

새 AAB의 UnityFS 데이터에서 정렬된 TextAsset 이름·길이·전체 본문을 확인했습니다. 새 고지 SHA256은 `1532E3839B916793EB7B746D331E6CB66FC1CDAB44549B6D8858783E1714C6D9`입니다. 이 검사는 전체 권리 출처·직접 UI 표시나 serialized object-table GUID 증명으로 확대하지 않습니다. 현재 Play 코드18의 고지694,428B는 그대로이며 나머지 에셋 원본/조건부 SDK 대응 때문에 #42는 열림 유지합니다. 아래 소스 보완 당시 추가 빌드0 기록과 이번 코드19 생성을 구분합니다.

## Rx.NET 차용 코드·Fusion BVH 고지 보완 (2026-10-17, #42)

`Assets/Legal/ThirdPartyNotices.txt`에 확인된 런타임 고지2항목을 추가했습니다. 기존 고지 전체 문구와 Git 정규화 원본681,938B prefix를 보존한 새 소스는707,770B/SHA256 `1532E3839B916793EB7B746D331E6CB66FC1CDAB44549B6D8858783E1714C6D9`입니다. 현재 배포 코드18의 고지는694,428B 그대로이며 이번 소스 변경의 새 AAB·서명·업로드·버전 예약은0입니다. 원장194,10,18·다음 새 코드19 이상을 유지합니다.

| 보완 항목 | 확인 근거와 적용 범위 |
| --- | --- |
| UniRx의 Rx.NET 차용 부분 | Microsoft Open Technologies 헤더가 있는 EventPattern·Notification·TimeInterval·Timestamped·CancellationDisposable·RefCountDisposable·ScheduledItem·CurrentThreadScheduler 8파일이 공식 UniRx7.1.0 대응 파일과 BOM/개행 정규화 텍스트 일치. 전체 패키지 버전 확정은 아님. 코드15 IL2CPP 생성 출력에 관련 구현·UniRx 모듈 등록을 확인했으며 개별 최종 네이티브 링크 생존 전수 검사는 아님 |
| Simple Scene BVH — Photon Fusion 공급자 선언 | [Photon OSS PDF](https://doc.photonengine.com/docs/content/OSS-Photon_Fusion.pdf) 3쪽이 v1.0·upstream URL·Apache2.0을 명시. 코드15 Fusion.Runtime의 BVH/BVHNode 구현·메서드 등록과 대조. v1.0은 공급자 문서 표기로 한정하며 로컬 정확 버전·upstream master 바이트 동일성·Fusion2.0.13 전용 BOM을 확정하지 않음 |

Rx.NET은 [고정 역사 리비전의 Source/license.txt](https://raw.githubusercontent.com/dotnet/reactive/c9cefc1b5606e4d2e3908fe68f62e9b5553144c3/Rx.NET/Source/license.txt) 전체730B/SHA256 `D6E3B2775B63D800D10B369D9FA1D9F7147AE61E32A0003CFE318A948BAA87EE`를 추가했습니다. Microsoft 저작권뿐 아니라 기여자 감사·CodePlex 링크·Apache2.0 안내·면책을 보존합니다. [2020년 MIT 전환](https://github.com/dotnet/reactive/commit/1a87246a90872a1046e4b8be347746862cbc528e)을 더 오래된 차용 코드에 소급 적용하지 않습니다. 정확한 최초 차용 리비전·mono 차용 코드 전체는 미확인 범위로 남깁니다.

두 항목의 적용 범위를 명시하고 [Apache 공식 전문](https://www.apache.org/licenses/LICENSE-2.0.txt) 원문11,358B를 문구 변경 없이 함께 제공합니다. 추가한 두 원문은 기존 파일 형식에 맞춰 CRLF 개행만 정규화했습니다(Rx.NET 복사744B·Apache 복사11,560B). 기존 혼재 개행81개도 CRLF로 정리했으며 원문 문구와 Git 정규화 원본681,938B prefix는 그대로입니다. 원래 작업 사본694,428B는 외부 근거에 보존했습니다. Windows CRLF 작업 사본707,770B와 Git의 LF 정규화 blob694,963B를 구분하며, Git checkout 필터를 적용한 결과와 작업 사본의 바이트 일치도 검증합니다. 현재 SimpleScene upstream master의 David W. Jeske2014/public domain 헤더와 Photon 선언의 차이는 근거에 보존하며, 이를 공급자 오류·로컬 라이선스 변경으로 단정하거나 새 저작권/NOTICE를 만들지 않습니다. UnsafeCollections·Fody·InlineIL·Oswald와 WebSocket 등 공급자 목록 전체를 일괄 추가하지 않았습니다.

변경 없는 실제 제품 pager 소스를 Unity 번들 Roslyn/Mono로 별도 컴파일해 최종 CRLF 고지196페이지의 전체 원문 round-trip·최대4089 UTF-16 units/93줄바꿈을 확인했습니다. 처음 `latest` 언어 옵션은 번들 구형 컴파일러의 CS8652로 실패했고 `preview`로 컴파일해 통과했습니다. 제품 pager 수정0이며 이 독립 검사를 Editor TextAsset/UI·Android 실행으로 확대하지 않습니다. 현재 Editor 미연결 상태로 새 UI 표시·새 배포 포함은 미검증입니다. 에셋 원본/취득 대응·조건부 구성요소·정확한 공급자 배포 NOTICE와 기기 확인 때문에 #42는 열림 유지합니다.

근거: 외부 `D:/codex/validation/sudden-force-fps/20261017-runtime-oss-notices-01/`의 `sources.json`, `prepare-stable-result.json`, `upstream-unirx/`, `pager-stable-validation.jsonl`, `pager-compiler-scope.json`과 공급자 PDF3쪽 렌더·독립 읽기 검토입니다.

## 코드18 서명 AAB의 보완 고지 포함 확인 (2026-10-16, #42)

기존 업로드 키로 서명한 코드18 AAB92,840,760B/SHA256 `83ADB6D168F787CF0419DD5141A6675B1105D99D4407FE6E4FF115A073DD7FD9`에서 고지 본문694,428B/SHA256 `D326402384AE8EA89BDFA8034F0B017BB8A198E8E9449616B9CAE60A1836267D`가 코드18 빌드 시점의 소스와 정확히 일치함을 확인했습니다. 아래 MiniJSON·Newtonsoft.Json 보완이 실제 새 산출물에 포함됐으며 서명 전후 payload129개 바이트 차이0입니다. DEX1개도 코드12와 같아 기존 지정17개 광고·결제 SDK 조사 범위에 연결됩니다. 총괄이 직접 업로드·출시한 Play 내부 테스트 release5/code18의16:37 내부 테스터 제공을 확인했습니다. 앱 고지의 실제 기기 표시와 나머지 애니메이션·단종 로딩 팩의 원본 대응 등 #42의 남은 범위는 유지하며 새 Store 설치·실행 성공으로 확대하지 않습니다.

근거: 외부 `20261016-export-aab-code18-c1-01/`의 `signed-notice-body-validation.json`, `signed-static-validation.json`, `signed-release-summary.json`과 독립 서명 검토입니다.

## MiniJSON·Newtonsoft.Json 앱 고지 보완 (2026-10-16, #42)

코드12가 사용한 Gradle export의 IL2CPP 출력에서 MiniJSON 직렬화/역직렬화 함수의 구현·등록과 Newtonsoft.Json 모듈 등록을 확인했습니다. 코드12 AAB의 `global-metadata.dat`에도 두 이름이 존재하고, 코드12에 내장된 기존 앱 고지와 현재 수정 전 파일의 SHA256은 `5EECFD5DAA79ECD94CBBDA5F2CED0FE4289D6ED4FF05C1BA5BAF51188DB48FCD`로 일치하지만 두 라이브러리의 저작권·MIT 허가/면책 고지가 빠져 있었습니다.

`Assets/Legal/ThirdPartyNotices.txt`의 기존687,423B를 그대로 보존하고 아래 원문을 덧붙였습니다.

- MiniJSON: 로컬 `Assets/ThirdPartyAssets/MiniJSON.cs` 헤더의 Calvin Rien(2013) 저작권, Patrick van Bergen 기반 안내와 MIT 허가·면책 전체. 주석 구분자만 제거했으며 전체 파일이 현재 UnityCsReference와 동일하다고 주장하지 않습니다. [공식 저장소의 같은 고지 헤더](https://github.com/Unity-Technologies/UnityCsReference/blob/master/External/JsonParsers/MiniJson/MiniJSON.cs)를 대조했습니다.
- Newtonsoft Json for Unity: 설치 패키지 `com.unity.nuget.newtonsoft-json`3.2.2(설명상 upstream13.0.2)의 `LICENSE.md`와 `Third Party Notices.md` 전체. James Newton-King·SaladLab·Kalle Jillheden·Mike Wuetherick의 공급자 MIT 고지를 생략하지 않았으며 공급자 원문의 중복 `Copyright (c)` 표기도 보존했습니다. [upstream13.0.2 MIT 원문](https://github.com/JamesNK/Newtonsoft.Json/blob/13.0.2/LICENSE.md)과 패키지 전체의 적용 범위를 구분합니다.

수정 후 작업 파일은694,428B/SHA256 `D326402384AE8EA89BDFA8034F0B017BB8A198E8E9449616B9CAE60A1836267D`입니다. 실제 `ThirdPartyNoticePager.cs`를 Unity6000.3.25f1 번들 Mono에서 실행해 수정 전191/수정 후193페이지의 source 재결합 일치와 페이지 예산(최대4089 UTF-16단위/93LF)을 확인했습니다. 외부 PowerShell의 .NET10에서는 수정 전 원문도 `StringInfo.ElementIndex` 동작 차이로 실패했으므로 그 시도를 제품 실패나 통과 결과로 취급하지 않고, 제품 코드는 변경하지 않았습니다. Unity UI·글리프·Android 실기기 표시는 이번에 실행하지 않았습니다.

**배포된 코드12 AAB는 수정하지 않았습니다.** 이 보완은 다음 새 배포 코드13 이상에서 실제 고지 본문 포함과 표시를 확인해야 합니다. 두 구체적 누락을 고친 것이며 과거 문서의 모든 미확인 항목을 새 누락으로 간주하거나 #42 전체를 닫지 않습니다. 근거: `D:/codex/validation/sudden-force-fps/20261016-json-notices-01/`의 원문 사본·`prepare-result.json`·`pager-validation.jsonl`, 코드12 export의 `Assembly-CSharp__2.cpp`/`Assembly-CSharp_CodeGen.c`/`Il2CppCodeRegistration.cpp`입니다.

## 기존 계정의 세 팩 취득 기록 확인 (2026-10-16, #42)

사용자가 지정한 작업용 Chrome의 기존 탭에서 로그인된 공식 Asset Store의 구매 배너와 My Assets 항목을 직접 대조했습니다. 아래는 새 구매가 아니라 기존 취득 기록입니다. 현재 상품의 무료 표시만으로 취득을 추정했던 범위를 보완했습니다.

| 공식 상품 | 기존 취득일 표시 | 현재 로컬 파일과 공식 패키지 목록의 대응 |
| --- | --- | --- |
| Slayver [Survivalist character,181470](https://assetstore.unity.com/packages/3d/characters/survivalist-character-181470) | 2024-11-13 | `Military/Basemesh/SK_Military_Survivalist.fbx`와 공식 `Survivalist/Basemesh/SK_Military_Survivalist.fbx` 이름 대응1개 |
| JustCreate [Low Poly FPS Weapons Lite,245929](https://assetstore.unity.com/packages/3d/props/guns/low-poly-fps-weapons-lite-245929) | 2024-11-14 | `Weapon_02.fbx`를 포함한 모델5개와 `WeaponsLite_Texture_01.png`, 총6개 이름 대응 |
| JustCreate [Low Poly FPS Map Lite,258453](https://assetstore.unity.com/packages/3d/environments/low-poly-fps-map-lite-258453) | 2024-11-14 | 건물·상자·벽·계단 등 FBX13개와 `LowPolyFPSLite_Texture_01.png`, 총14개 이름 대응 |

세 상품 모두 기존 소유 상태의 **Unity에서 열기**와 현재 **Standard Unity Asset Store EULA** 표시를 확인했습니다. 캐릭터의 현재 제공 버전은1.2, 무기·맵은1.0입니다. 원 패키지를 다운로드·반입하거나 제품 파일을 변경하지 않았으며, 공식 파일명과 로컬 경로/해시 목록을 연결한 검사입니다. 공식 바이너리와의 바이트 동일성, 취득 당시 약관 snapshot이나 전체 에셋·SDK 조건을 증명한 것으로 확대하지 않습니다. 아래10월14일 표의 세 팩 취득 미확인은 이 기록으로 보완됐고 원본 바이트 대조는 별도입니다.

현재 캐릭터 패키지의 펼쳐 본 `StarterAssets/ThirdPersonController/Character/Animations`에는 `Jump--InAir.anim.fbx`, `Locomotion--Run_N.anim.fbx` 등 기본 클립이 표시됩니다. 이 목록으로2024년 반입된 프로젝트의 플레이어22개 `.anim` 출처를 확정하지 않았습니다. 캐릭터 취득 확인과 별도 애니메이션 원본 대응을 구분하며, 현재 버전에 없는 이름이 당시에도 없었다고 판단하지 않습니다.

[Animated Loading Icons,47844](https://assetstore.unity.com/packages/2d/gui/icons/animated-loading-icons-47844)의 공식 단종 페이지는 기존 소유자의 신규·기존 프로젝트 계속 사용을 안내합니다. 현재 로그인 계정 My Assets에서 정확한 제목과 `Loading` 검색 및 기본/숨김/단종 상태를 확인했지만 대상 취득 항목을 찾지 못했습니다. 이는 해당 계정·검색 범위의 미확인이며 과거 취득 무효나 사용 금지 근거가 아닙니다. 다른 기존 계정·원 패키지의 확인은 남아 있습니다. HUD7개는 사용자 제작 확인을 그대로 유지합니다.

근거: `D:/codex/validation/sudden-force-fps/20261016-asset-entitlements-01/`의 `asset-entitlements.json`, `local-name-correspondence.json`, 팩별 `*-acquired.png`/`*-package-files.txt`, `loading-deprecated-search.txt`입니다. 개인 계정 화면 원본은 외부 검증 자료에만 보관합니다. 이번에 계정 인증 정보 추출·새 구매/약관 동의·패키지 다운로드/교체·Editor 시작/제품 빌드·서명은 수행하지 않았습니다. #42는 남은 원본/고지 대응 때문에 열림 유지합니다.

## 코드12 광고·결제 네임스페이스와 남은 출처 점검 (2026-10-16, #42)

서명 AAB SHA256 `21AD89A76079243D2C53AB7B7F26C29B34019D6CE815D5462FE8A2CF01906E9A`의 DEX1개 전체에서 타입 참조와 클래스 정의3696개를 읽었습니다. [AOSP DEX 형식](https://source.android.com/docs/core/runtime/dex-format)의 헤더·테이블 경계·SHA1/Adler32를 검증했고 Google Mobile Ads·Play Billing·Unity Ads/Purchasing·ironSource 등 지정17개 네임스페이스 참조/정의0을 확인했습니다. GPGS238·Unity bridge36·Unity Player207 클래스 정의를 양성 대조로 확인했습니다. 프로젝트 UPM·제품 스크립트·Android 의존성의 명시적 광고/결제 API·좌표와 실제 manifest의 AD_ID/BILLING도 발견되지 않았습니다. AAB 해시를 보존했고 제품 변경·재빌드는 없습니다.

이는 기존 광고·결제 제외 작업을 보완하는 출하 DEX 점검이며, 알 수 없는 난독화·이름 변경·네이티브·동적/원격 동작 전체의 부재 증명이나 전체 SDK 권리 검증은 아닙니다. Console 광고 없음 선언은 저장했고 검토 전송 전입니다. 근거: 외부 `20261016-play-code12-followup-01/shipped-dex-audit.json` 및 `ads-none-saved.png`.

독립 읽기 조사 당시 기존 캐릭터·무기·맵·단종 로딩 팩의 공식 출처 후보와 참조 체인을 재확인했고 새 권리 문제나 배포 불가 근거는 확인하지 못했습니다. 이후 세 팩의 기존 취득 기록과 파일명 대응을 위 추가 기록으로 보완했습니다. 애니메이션22개·단종 로딩 팩의 원본 대응과 조건부 SDK 고지는 별도로 유지하며 HUD7개의 사용자 제작 확인을 되묻거나 미확인으로 되돌리지 않습니다. #42는 열린 상태입니다.

## 코드12 서명 AAB 고지 본문 확인 (2026-10-15, #42/#66)

기존 업로드 키로 서명한 `1.0.194/code12` AAB의 `base/assets/bin/Data/data.unity3d`에서 고지 TextAsset 본문687,423B를 디코딩해 소스 전문 SHA256 `5EECFD5DAA79ECD94CBBDA5F2CED0FE4289D6ED4FF05C1BA5BAF51188DB48FCD`와 일치를 확인했습니다. 서명 전후 payload127개 바이트 차이0이며 JAR 서명 메타데이터3개만 추가됐습니다. 새 object-table GUID 증명, 전체 에셋 권리 조건 또는 고지 UI 실기기 검증을 완료했다는 뜻은 아닙니다.

근거는 `D:/codex/validation/sudden-force-fps/20261015-export-aab-code12-c1-01/`의 `signed-notice-body-validation.json`, `signed-static-validation.json`, `signed-cryptographic-validation.json`입니다. 서명 AAB는 배포 후보이며 Play 업로드·Store 설치 검증은 아직 없습니다. 남은 출처 범위 때문에 #42는 유지합니다.

## HUD 제작 확인과 남은 원본 대조 (2026-10-14, #42)

사용자는 `Assets/Art/Game/UI`의 `Crosshair.png`, `Fire.png`, `Gun.png`, `Health.png`, `HPBar.png`, `Jump.png`, `Sit.png` 7개를 **직접 제작 또는 AI 생성**한 이미지라고 확인했습니다. 이 답변을 프로젝트 자체 제작 HUD의 출처 기록으로 반영하고 기존 파일과 GUID를 유지합니다. 개별 파일의 제작 방식·생성 도구는 추정하지 않으며, PNG 생성 시각 메타데이터를 저작자 증명으로 취급하지 않습니다. 이전의 HUD 출처 질문은 해소됐습니다.

직렬화 참조를 대조하면 `Sit.png`는 달리기 버튼, `Jump.png`는 재장전 버튼에 연결돼 있습니다. 파일명만으로 웅크림·점프 구현을 주장하지 않습니다. 이번 작업은 출처 기록이며 이미지·입력·씬 변경이나 런타임 UI 검증은 수행하지 않았습니다.

| 기존 자산 | 확인한 공식 출처 후보 | 남은 확인 |
| --- | --- | --- |
| Military/Survivalist 캐릭터 | Slayver의 [Survivalist character, 181470](https://marketplace.unity.com/packages/3d/characters/survivalist-character-181470), 현재 무료·Standard Unity Asset Store EULA. 공식 제작자 목록의 모델명과 로컬 파일명이 대응 | 기존 계정 취득 내역과 원 패키지의 로컬 파일 대응 |
| Weapon_02 | JustCreate의 [Low Poly FPS Weapons Lite, 245929](https://assetstore.unity.com/packages/3d/props/guns/low-poly-fps-weapons-lite-245929), 현재 무료·Standard EULA | 기존 계정 취득 내역과 원 모델·텍스처 대응 |
| DesertHouse 맵 자산 | JustCreate의 [Low Poly FPS Map Lite, 258453](https://marketplace.unity.com/packages/3d/environments/low-poly-fps-map-lite-258453), 현재 무료·Standard EULA | 기존 취득 기록과 로컬 원 패키지 대응 |
| 로딩 이미지·애니메이션 | [Animated Loading Icons, 47844](https://assetstore.unity.com/packages/2d/gui/icons/animated-loading-icons-47844), 현재 단종. 공식 페이지는 기존 취득자의 새·기존 프로젝트 이용을 안내 | 기존 취득 내역, spritesheet·animation 대응 및 당시 적용 조건 |
| 플레이어 애니메이션 22개 | 캐릭터와 같은 반입 커밋 및 제작 메타데이터는 확인 | 정확한 원 클립·제공 패키지 대응. 별도 ArtStation 판매 파일의 존재를 무료 Unity판에 포함됐다는 증거로 사용하지 않음 |

현재 무료 표시와 비슷한 파일명은 기존 취득 권한·원 패키지 동일성을 모두 증명하지 않습니다. 단종이나 기록 미확인 역시 배포 불가 확정으로 해석하지 않습니다. 새 PC의 미디어 726개 해시와 활성 4개 씬·Resources의 보수적 참조 체인을 읽기 대조했으며, 이는 최종 산출물의 실제 포함 목록을 대체하지 않습니다. 이번에 다운로드·에셋 교체는 하지 않았고 #42 전체는 열린 상태입니다. 외부 근거: `D:/codex/validation/sudden-force-fps/20261014-remaining-assets-readonly`.

2026-10-11 서명 AAB code5 추가 확인: 실제 ThirdPartyNotices TextAsset675,225 bytes/SHA256 `a58eb4b964c268847a85773f96449f7f24daaba85ef734f6032b8d30476a15d6`가 원본과 바이트 일치했고 EmojiOne 경로·GUID packed0을 독립 확인했습니다. 콘솔 packed12항목은 모두 MonoScript이고 비MonoScript0입니다. UI·프리팹·텍스처 제외의 검증이며 script·AAR 전체 부재나 남은 Military/Weapons/HUD 등 에셋 출처·배포 권리 확인 완료는 아닙니다. signed 산출물과 상세 근거는 ANDROID_RELEASE_CHECKLIST.md 상단을 따릅니다.

2026-10-11 빌드 범위 추가 확인: 서명 없는 Android export의 동일 BuildReport packed6,409개에서 IngameDebugConsole 기존93항목/29경로는12항목/12경로(모두 MonoScript)로 감소했습니다. 기존 GO/UI/Texture/Sprite 항목은0이며 script·AAR 전체 부재나 vendor 권리 검증 완료를 뜻하지 않습니다. 최종 signed AAB의 고지 본문·EmojiOne 및 새 패키지 범위는 별도 확인 대상입니다. 내보내기와 보존 근거는 ANDROID_RELEASE_CHECKLIST.md 상단을 따릅니다.

확인·취득일: **2026-10-07 (Asia/Seoul)**. 이 문서는 확인된 출처와 확인이 필요한 항목을 구분합니다. 저장소에 파일이 있다는 사실만으로 취득 권한이나 배포 라이선스를 확정하지 않습니다.

## Kenney Particle Pack — 신규 선별 텍스처

- 제작자: **Kenney Vleugels (Kenney.nl)**. 필터 템플릿 기여자 크레딧은 원본 `License.txt`에 보존했습니다.
- 공식 배포 페이지: https://kenney.nl/assets/particle-pack
- 공식 페이지의 “Continue without donating...” 링크에서 확인한 ZIP: https://kenney.nl/media/pages/assets/particle-pack/f8fe0f8cb8-1677578741/kenney_particle-pack.zip
- 라이선스: **Creative Commons Zero (CC0 1.0)**. 원본 ZIP의 `License.txt`를 변경 없이 `Assets/Art/VFX/KenneyParticlePack/License.txt`에 보존했습니다.
- 라이선스 원문: https://creativecommons.org/publicdomain/zero/1.0/legalcode
- 공식 이용 안내: https://kenney.nl/support — 상업 프로젝트 이용 허용, 출처 표기 의무 없음. Kenney 로고는 공식 프로젝트용으로 별도 취급되므로 가져오지 않았습니다.
- CC0는 상업 게임에 포함한 이용·수정·배포 및 원본 재배포를 허용합니다. 선택적 크레딧 예: `Particle textures by Kenney (kenney.nl), CC0`.
- 버전 증거: 웹 페이지는 1.0 / 2018 / 80 files로 표시되지만, 취득한 공식 ZIP의 라이선스는 **Particle Pack (1.1)**로 기재되어 있습니다. ZIP에는 투명/검정 배경 및 회전 변형 이미지가 더 들어 있습니다. 아래 목록은 실제 취득 ZIP 기준입니다.
- 원본 ZIP SHA256: `b631d4b07f7002549fdcf155f01141ad482f79f3440e4e301eed49ce5f1d8958` (15,001,764 bytes). ZIP은 프로젝트 밖의 임시 작업 폴더에서 검사했으며 저장소에 추가하지 않았습니다.

### 가져온 파일

모든 PNG는 ZIP 내부 `PNG (Transparent)/`에서 가져왔습니다. 파일 이름과 바이트를 변경하지 않았으며, 총 5개 PNG와 라이선스만 선별했습니다. 용도는 후속 VFX 제작을 위한 제안이며 현재 씬·프리팹에 연결되어 있지 않습니다.

| 프로젝트 경로 | 제안 용도 | bytes | SHA256 |
| --- | --- | ---: | --- |
| `Assets/Art/VFX/KenneyParticlePack/muzzle_01.png` | 길쭉한 총구 화염 | 81811 | `08f650f6a615b65024b48d8da32178158d74307d7c235f214f0c075b7aa688b9` |
| `Assets/Art/VFX/KenneyParticlePack/muzzle_03.png` | 짧은 총구 화염 변형 | 57240 | `579983452d263ebeca78080714b6dd55a0f95fca7452f309e91a9d466de6c058` |
| `Assets/Art/VFX/KenneyParticlePack/spark_02.png` | 작은 피격 불꽃 | 101613 | `f7c5f1f849657eed79fd7b663be5a57829450a1e4fdc4a062049a05b9a406e89` |
| `Assets/Art/VFX/KenneyParticlePack/dirt_01.png` | 벽·지면 피격 파편 | 47313 | `6827a0a32a293ec9570fb98d63963b8dd6e6aaba9e1096afad932e93a378bea9` |
| `Assets/Art/VFX/KenneyParticlePack/smoke_03.png` | 소량의 피격 분진 | 39737 | `a71f8abcac64f8d73a94625cc9a10033dbeafa7eaea750560cba0daa73fe8752` |
| `Assets/Art/VFX/KenneyParticlePack/License.txt` | ZIP 원본 라이선스·크레딧 | 651 | `f9e70b81d8cc07c4e07c9f2eff1d94fd371a06070b18cb67c651c41158ec2975` |

### 검증 및 후속 적용 조건

- 다운로드 응답이 ZIP signature `PK 03 04`이며 정상 ZIP으로 파싱되는지 확인했습니다.
- PNG 5개 모두 signature `89 50 4E 47 0D 0A 1A 0A`, Pillow 디코딩 및 무결성 검사 통과, **512 × 512** 이미지입니다. HTML을 PNG 확장자로 저장한 파일은 없습니다.
- PNG 원본은 팔레트 형식(`P`)이며 투명도 정보가 있습니다. RGBA 디코딩으로 완전히 투명한 픽셀과 보이는 픽셀을 모두 확인했습니다. 원본 변환·리사이즈는 하지 않았습니다.
- 후보 이미지 시각 검토로 총구 2개, 불꽃 1개, 파편/분진 2개를 선정했습니다. 각 저장 파일은 ZIP entry와 바이트가 동일합니다.
- Unity 6/URP 런타임 표시 및 모바일 비용은 **미검증**입니다. 이 팩은 이미지이며 Unity VFX prefab이나 동작 스크립트를 제공하는 것으로 취급하지 않습니다.
- 후속 단계에서 URP `Universal Render Pipeline/Particles/Unlit`, 짧은 수명, 작은 입자 수, 풀링을 검토합니다. 텍스처 크기·플랫폼 압축은 실기 확인 후 정하며 이번 단계에서는 Import 설정과 `.meta`를 직접 변경하지 않습니다.

## Low Poly FPS Map Lite — 기존 맵

- 저장소 경로: `Assets/ThirdPartyAssets/Objects/LowPolyFPSLite/`.
- 사용 증거: `Assets/Scenes/Game/DesertHouse.unity`가 이 패키지의 건물·벽·상자·판자·항아리·계단 프리팹을 참조합니다.
- 제작자: **JustCreate**.
- 공식 배포 페이지: https://marketplace.unity.com/packages/3d/environments/low-poly-fps-map-lite-258453
- 공식 페이지 확인 내용: FREE, 버전 1.0, 2023-07-05, 원래 Unity 2021.3.0, **Standard Unity Asset Store EULA**. 페이지의 라이선스 유형 표시는 Extension Asset입니다.
- 공식 약관: https://unity.com/legal/as-terms (확인한 페이지의 개정일 2024-12-04).
- EULA 2.2.1은 상당한 독창적 콘텐츠와 결합한 Licensed Product의 내장 구성요소로 사용·배포·판매하는 범위를 허용합니다. CC0와 달리 원본 자산을 단독으로 공개 재배포할 수 있는 권한으로 해석해서는 안 됩니다. 크레딧 의무는 Standard EULA 자체에서 확인되지 않았으며 별도 제공 조건은 확인해야 합니다.
- **미확인:** 이 저장소에 반입된 파일의 최초 취득일, 실제 취득 계정/팀 권한, 원본 패키지 해시, 별도 제공자 조건. 현재 공식 상품 정보를 확인한 것이며 과거 취득 이력을 증명한 것은 아닙니다. 이번 단계에서 재다운로드·수정하지 않았습니다.
- Unity 6 공식 호환 보증은 해당 페이지에서 확인하지 못했습니다. 기존 머티리얼의 URP/Lit 참조와 실제 Unity 6 표시 검증은 구분합니다.

## 기존 오디오 — 출처·라이선스 미확인

대상은 `Assets/Audios/`의 기존 총성·발소리·재장전·수류탄·UI 효과음과 음악입니다. 예시: `FootStep.mp3`, `fire.mp3`, `Handling_Gun_01_Reload_Sq_SFX.wav`, `Pistol_01_Fire_01_SFX.wav`, `Aggressive FPS Game Music/`, `FPS Menu Music Themes Vol. 1/`, `audio/`.

파일과 폴더 이름으로 제작자·배포 사이트·상업 이용 조건을 추정하지 않습니다. 조사에서 이 파일들의 라이선스와 취득 출처를 증명하는 문서를 찾지 못했으며, **상업 이용·게임 배포·원본 재배포·출처 표기 조건은 모두 미확인**입니다. 신규 Kenney CC0 라이선스는 이 기존 오디오에 적용되지 않습니다.

배포 전 각 팩의 공식 source URL, 취득 내역, 제공 라이선스, 크레딧 요구 사항을 복원해야 합니다. 출처를 복원할 수 없는 파일은 확인 가능한 자산으로 교체할 대상으로 분류합니다. 이번 단계에서는 기존 오디오를 수정·삭제·대체하지 않았습니다.


## C3 전투 표시 적용 (2026-10-07)

- 기존 `Low Poly Weapons LITE/Weapon_02`에서 프로젝트용 `Assets/Prefabs/Weapons/RiflePresentation.prefab`을 만들었습니다. 원본은 수정하지 않았으며, 시각 모델의 축과 총구/왼손 위치만 별도 프리팹에서 구성했습니다. 기존 패키지의 취득 이력과 배포 권한 증빙은 미확인입니다.
- Kenney 총구/불꽃 PNG는 `Assets/Art/VFX/Combat/`의 URP Particles/Unlit 머티리얼과 입자 프리팹에 연결했습니다. 총구는 1개, 명중은 최대 6개 입자를 재사용하며, 새 발사 sequence에 대해서만 표시합니다. 기존 텍스처 원본은 수정하지 않았습니다.
- `Assets/Audios/Authored/RifleShot.wav`와 `ReloadTick.wav`는 이 프로젝트용으로 수학적 노이즈·사인파·감쇠 포락선을 합성한 효과음입니다. 외부 음원이나 샘플을 사용하지 않았으며, 기존 출처 미확인 오디오와 구분합니다. 생성 시드 7301/7302, 22050 Hz, 16-bit PCM, mono이며, Unity는 PCM/DecompressOnLoad/PreserveSampleRate 설정을 사용합니다. SFX 믹서 그룹에 연결했습니다.
- RifleShot: 0.16초, 7100 bytes, SHA-256 `d9c53a349d120543fa7ef1d8d7c3ad9395cf3532313a775c81475cf398ec5bcc`.
- ReloadTick: 0.22초, 9746 bytes, SHA-256 `b23486b56265debdd3aab7c6c56b6a2551e0808eee79183104219d04f3128eb7`.
- 생성 코드 및 검증 증거는 작업 백업 `sudden-force-fps-backups/20261007-c3-presentation-ui/`에 보관했습니다. 별도의 외부 라이선스 음원을 다운로드하지 않았습니다. Android 실기 비용 및 실제 다중 터치 검증은 별도 QA 대상입니다.


## 배포 전 포함 에셋 점검 (2026-10-07)

사용자는 기존 에셋을 무료이며 배포 가능한 조건으로 취득했던 것으로 기억합니다. 이 배경을 반영하되, 현재 판매 페이지의 같은 파일명만으로 로컬 파일과 상품의 동일성이나 적용 조건을 단정하지 않습니다.

읽기 전용 조사에서 미디어 794개와 활성 씬·Resources·직렬화 참조를 추적했습니다. 정적 참조는 실제 Android AAB 포함 여부를 확정하는 근거가 아니며 배포 직전 BuildReport로 확인합니다. 근거 보고서는 `D:\meee\git\sudden-force-fps-backups\20261007-license-audit\HANDOFF.txt` 및 같은 폴더의 JSON에 보존했습니다. 이번 조사에서 에셋 변경·다운로드·교체는 하지 않았습니다.

| 항목 | 확인 상태와 후속 작업 |
| --- | --- |
| Kenney 텍스처·직접 합성 효과음 2개 | 문서와 실제 파일 일치. C3 전투 프리팹 연결 확인 |
| Military 캐릭터·플레이어 애니메이션 22개·무기·기존 HUD 이미지 7개·Loading Icons | 취득 당시 출처와 조건이 현재 파일에서 확인되지 않음. 기존 취득 자료나 제작자 정보를 추가 대조 |
| Liberation Sans | 동봉 OFL 파일 확인. 배포 앱에 필요한 고지를 포함하는 경로 확인 필요 |
| EmojiOne 샘플 | 최신 Android BuildReport에서 EmojiOne.asset·EmojiOne.png 실제 포함 확인. 정확한 artwork 버전·라이선스·앱 attribution은 미확정이며 별도 조건 복원 대상. 이번 SDK 고지 변경에서 제거·교체하지 않음 |
| 기존 오디오 61개 | 정적 참조 0, Resources 밖. 실제 빌드 포함 여부를 확인하고 사용 에셋과 분리 |
| SimplePixelUI Font2 | 현재 미참조 후보. 동봉 OFL과 TTF 내부 표기 불일치가 있어 사용 전에 출처 조건 확인 |

출처가 미확인이라는 사실을 배포 불가 확정으로 해석하지 않습니다. 취득 조건을 확인한 에셋은 유지하며, 확인되지 않는 배포 포함 항목의 대응은 실제 포함 목록과 이식 영향을 검토한 뒤 결정합니다.

## SDK 고지 및 최신 Android 빌드 포함 확인 (2026-10-08)

앱의 `Assets/Legal/ThirdPartyNotices.txt`에 아래 5개 SDK의 원문 고지를 제공합니다. 기존 폰트·파티클 고지를 보존하며, 이 추가가 모든 SDK와 에셋의 배포 고지 검증 완료를 의미하지는 않습니다.

| SDK | 식별·사용 근거 | 고지 원문 출처 |
| --- | --- | --- |
| UniRx | `UpdateManager`의 `EveryUpdate` 사용. 정확한 전체 패키지 버전은 미확정이므로 버전 번호를 표시하지 않음 | [공식 MIT 원문](https://github.com/neuecc/UniRx/blob/7.1.0/LICENSE). 이 태그의 `Observable.cs`와 로컬 파일 텍스트 일치만 확인했으며 전체 패키지 버전의 증거로 취급하지 않음 |
| UniTask 2.5.10 | 로컬 `package.json` 및 씬 전환·로그인 코드 사용 | [2.5.10 MIT 원문](https://github.com/Cysharp/UniTask/blob/2.5.10/LICENSE) |
| Google Play Games plugin for Unity 2.0.0 | 로컬 package·PluginVersion 및 Android 로그인 코드 사용 | [v2.0.0 copyright 및 Apache 2.0 전문](https://github.com/playgameservices/play-games-plugin-for-unity/blob/v2.0.0/LICENSE) |
| In-game Debug Console 1.6.8 | 로컬 README, Login 씬의 prefab 참조, 최신 BuildReport의 관련 source paths 29개 확인 | [v1.6.8 MIT 원문](https://github.com/yasirkula/UnityIngameDebugConsole/blob/v1.6.8/LICENSE.txt) |
| NanoSockets | APK의 ARM64·ARMv7 `libnanosockets.so` 확인. 각 uncompressed entry SHA256가 로컬 Fusion 번들 라이브러리와 동일 | 로컬 `Assets/Photon/Fusion/Plugins/NanoSockets/libnanosockets_LICENSE.txt`의 Stanislav Denisov copyright 및 MIT 전문 |

UniRx에 포함된 Microsoft 차용 코드의 `Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.` 문구도 앱 고지에서 원문대로 보존합니다. 해당 source header의 원문은 `See License.txt in the project root for license information.`까지이며 저장소의 해당 코드 파일에 남아 있습니다. 로컬에서 그 `License.txt`는 확인하지 못했습니다. 공식 UniRx README는 Rx.NET·mono 코드 차용을 설명하므로, 원본 시대별 라이선스와 저작권 범위가 모두 해소됐다고 판단하지 않습니다.

### 실제 포함 증거 및 남은 범위

- 대조 APK: `SuddenForceFPS.UploadKeyValidation.apk`, 238,310,918 bytes, SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`.
- 해당 검증 폴더의 `packed-assets.json`은 Unity `BuildReport.GetLatestReport().packedAssets.contents.sourceAssetPath`를 모은 목록으로, source paths 2,889개를 기록합니다. 이는 최종 IL2CPP 타입·메서드 단위 사용 여부나 모든 전이 의존성의 권리 조건을 증명하는 자료는 아닙니다.
- `apk-native-inspection.json`은 native `.so` 14개를 기록합니다. ARM64의 7개는 `libc++_shared.so`, `libmain.so`, `libnanosockets.so`, `libswappywrapper.so`, `libunity.so`, `lib_burst_generated.so`, `libil2cpp.so`입니다. 이름과 포함 사실만으로 각 라이브러리 내부 OSS 목록이나 라이선스 전체를 확정하지 않습니다.
- EmojiOne의 `Assets/ThirdPartyAssets/TextMesh Pro/Resources/Sprite Assets/EmojiOne.asset` 및 `Assets/ThirdPartyAssets/TextMesh Pro/Sprites/EmojiOne.png`가 실제 packed source 목록에 있습니다. 이전 문서의 'Resources 포함 후보'에서 실제 포함 확인으로 갱신합니다. 동봉 Attribution 파일은 제작자 사이트 안내만 제공하므로 정확한 artwork 버전·라이선스·앱 attribution은 여전히 미확정입니다. 화면에서 emoji를 직접 사용하지 않는다는 이유로 미포함 처리하지 않으며, 이번 변경에서 제거·교체하거나 특정 버전의 조건을 임의 적용하지 않습니다.
- Fusion 실제 DLL/build 정보는 `2.0.13.2379`이며 `package.json`의 `1.1.0` 표기와 구분합니다. 공식 Photon OSS 문서의 runtime 내장 라이브러리, codegen 도구, 번들 폰트 범위를 별도로 대조해야 합니다. 별도 KCC 애드온은 발견되지 않았고 현재 게임은 Fusion 기본 `NetworkCharacterController`를 사용합니다.
- Unity 패키지 및 Google Play services의 최종 전이 의존성·고지, EmojiOne 조건 복원, UniRx 차용 코드의 원 권리 범위는 #42 후속 확인 대상입니다. 61-section SDK 합본을 전부 앱에 적용하지 않습니다.

근거와 원문 SHA256는 외부 작업 기록 `D:\meee\git\sudden-force-fps-backups\20261008-sdk-notices-audit\` 및 `20261008-sdk-notices-prepare\`에 보존했습니다. 고지 텍스트 제공과 실제 포함 권리 범위 확인을 구분하며 #42는 후속 확인이 끝나기 전 닫지 않습니다.

## Unity 패키지·Photon .NET 추가 고지 (2026-10-08)

앱의 `Assets/Legal/ThirdPartyNotices.txt`에 다음 5개 섹션의 고지를 추가합니다. 기존 고지 22,019바이트 전체와 기존 SDK 5개의 저작권·라이선스 원문을 보존하며, 추가 범위를 아래 항목으로 한정합니다.

| 고지 항목 | 설치·사용 근거 | 고지 출처와 적용 범위 |
| --- | --- | --- |
| Unity uGUI 2.0.0 (TextMesh Pro 포함) | 설치본 `package.json` 버전. 대조 Android BuildReport에 `Packages/com.unity.ugui/` source paths 141개 | 설치본 `LICENSE.md`의 저작권·Unity Companion License 링크·면책 원문 전체. [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license)와 구분하며, 동봉 파일 전체를 복사한 것이 UCL 웹 전문을 복사했다는 뜻은 아님 |
| Unity Universal Render Pipeline 17.3.0 | 설치본 버전. 해당 BuildReport의 URP source paths 309개 | 설치본 `LICENSE.md` 원문 전체. Unity 패키지 본체를 MIT/Apache 라이선스로 판단하지 않음 |
| Unity Render Pipeline Core 17.3.0 | 설치본 버전. 해당 BuildReport의 Core source paths 358개 | 설치본 `LICENSE.md` 원문 전체. 패키지 안의 모든 별도 third-party 구성요소 권리 확인을 대신하지 않음 |
| Photon .NET Client SDK — BigInteger | packed `Fusion.Realtime.dll`의 AssemblyRef에 `Photon3Unity3D`. 로컬 Photon DLL에 `Photon.SocketServer.Numeric.BigInteger` 및 `DiffieHellmanCryptoProvider` 타입 확인 | [Photon 공식 .NET OSS PDF](https://doc.photonengine.com/docs/content/oss-.net_client_sdks.pdf) 2쪽의 Authors, 2003 Ben Maurer·2002 Chew Keong TAN·2004/2007 Novell 저작권, MIT 허가·면책 전문. PDF 레이아웃 공백만 정리하며 문구를 생략하지 않음 |
| Photon .NET Client SDK — ZigZag Encoding | 같은 로컬 DLL의 `Protocol18.EncodeZigZag32/64`·`DecodeZigZag32/64` 메서드 확인 | 같은 공급자 PDF 2쪽의 Apache 2.0 선언에 따라 [Apache 공식 전문](https://www.apache.org/licenses/LICENSE-2.0.txt)을 제공. 공급자 PDF의 레이아웃과 공식 전문의 바이트가 같다는 주장은 하지 않으며, GPGS Google 저작권을 이 구성요소에 적용하지 않음 |

설치된 `Fusion.Common`·`Fusion.Realtime`·`Fusion.Runtime`의 실제 DLL ProductVersion은 `2.0.13.2379+4f8b2d70`, `Photon3Unity3D.dll`은 `4.1.8.21+be37a12aeee85c41f8b2eb28c44cfff8b29e5668`입니다. 이는 로컬 DLL 식별값이며 공식 OSS PDF가 해당 버전만을 대상으로 한 BOM이라는 증거는 아닙니다. Photon SDK 본체의 상용 라이선스 또는 계정·플랜 권리를 OSS 고지로 대체하지 않습니다.

포함 근거는 기존 검증 APK SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`에 연결된 source paths 2,889개의 BuildReport 스냅샷입니다. `PhotonLibs` 경로가 그 목록에 없다는 사실만으로 Photon DLL이 불포함이라고 단정하지 않습니다. 어셈블리 참조와 내부 타입 존재도 최종 IL2CPP에서 개별 타입·메서드가 생존했다는 확증은 아닙니다. 이후 새 APK에는 해당 APK의 포함 근거를 다시 연결해야 합니다.

정확한 버전별 공급자 BOM, 조건부 Fusion·셰이더·WebSocket 구성요소, Unity Player/Burst·네이티브 라이브러리 및 Android 전이 의존성, EmojiOne artwork 조건은 여전히 후속 확인 대상입니다. 이번 5개 추가는 전체 SDK·에셋의 권리 검증 완료 또는 #42 종료를 의미하지 않으며, 61개 섹션 합본을 적용하지 않습니다.

원문 사본·SHA256·공백 정리 검증·추가 섹션별 byte offset 및 보존 비교는 외부 작업 기록 `D:\meee\git\sudden-force-fps-backups\20261008-unity-photon-notices-prepare\`에 있습니다. 조건부 항목과 EmojiOne 제외 검토는 `20261008-sdk-runtime-scope\REPORT.txt` 및 `EMOJIONE_EXCLUSION_REVIEW.txt`에 보존합니다.

### Google Play services 공급자 OSS 고지 및 Android 버전 메타데이터 (2026-10-08)

아래 6개 버전의 vendor properties가 `6a8b203` 기준 `SuddenForceFPS.UploadKeyValidation.apk`에 실제 들어 있는 것을 확인했습니다. APK의 groupId/artifactId 필드는 미존재이며, entry 이름과 `version` 값으로 식별합니다. 생성된 `launcher-debug.apk`와 외부 검증 APK의 SHA-256이 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`으로 같아 두 APK의 바이트 동일성을 확인했습니다. output metadata는 `debug`, versionCode `3`, versionName `1.0.0`, 패키지 `com.AeDeong.SuddenForceFPS`입니다. 이후 다른 소스·APK에 이 결과를 확대하지 않습니다.

| 모듈 | 실제 APK properties의 version | 공식 공급자 OSS JSON 레코드 수 | 원문 배포본 |
| --- | --- | --- | --- |
| play-services-base | 18.5.0 | 16 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-base/18.5.0/play-services-base-18.5.0.aar) |
| play-services-basement | 18.4.0 | 16 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-basement/18.4.0/play-services-basement-18.4.0.aar) |
| play-services-drive | 17.0.0 | 9 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-drive/17.0.0/play-services-drive-17.0.0.aar) |
| play-services-games-v2 | 20.1.2 | 23 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-games-v2/20.1.2/play-services-games-v2-20.1.2.aar) |
| play-services-nearby | 18.5.0 | 24 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-nearby/18.5.0/play-services-nearby-18.5.0.aar) |
| play-services-tasks | 18.2.0 | 16 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-tasks/18.2.0/play-services-tasks-18.2.0.aar) |

Google 공식 Maven에서 정확한 버전의 POM·AAR을 별도로 취득했으며, 6개 공식 AAR 각각의 SHA-256이 기존 로컬 캐시 AAR과 같습니다. 캐시 존재나 수정 시각만으로 포함을 판단하지 않았습니다. APK와 생성된 merged Java resources의 6개 properties entry/version도 일치합니다. manifest blame에서 지원 wrapper `2.0.0`과 base·basement·games-v2·nearby 좌표를 보조 확인했으나, 이전 시각의 incremental 파일이므로 이것만으로 전체 선택 그래프를 확정하지 않습니다.

각 AAR의 `third_party_licenses.json`과 `.txt`에서 공급자 고지 원문을 확보했습니다. 총 104개 레코드의 바이트 offset/length를 검증하고, 33개 공급자 이름과 28개의 서로 다른 원문 본문을 앱 고지에 매핑합니다. 같은 본문 SHA만 중복 제거하며, 같은 이름의 다른 원문 및 원문 내부 서브고지는 보존합니다. 기존 폰트·파티클·Core5 및 Unity·Photon 고지는 유지합니다. 각 공급자 이름은 고지 메타데이터의 표기이며 개별 컴포넌트가 모두 최종 앱 클래스에 남아 있다는 뜻이 아닙니다. 원문에 없는 라이브러리 버전이나 SPDX 식별자를 추정해서 추가하지 않습니다.

6개 POM은 모두 `Android Software Development Kit License`와 [Google SDK terms URL](https://developer.android.com/studio/terms.html)을 지정합니다. Unity용 GPGS wrapper의 Apache 2.0 조건은 해당 wrapper 고지이며 Google Play services 상용 SDK 전체의 라이선스로 확대하지 않습니다. 공급자 OSS 고지와 상용 SDK 약관을 구분하고, 현재 웹 약관이 각 SDK 배포 당시 snapshot과 같거나 권리 조건 전체를 충족했다고 주장하지 않습니다. [Google 공식 OSS 안내](https://developers.google.com/android/guides/opensource)는 POM 및 공급자 embedded OSS 고지의 수집·표시를 설명합니다.

이번 근거는 6개 버전 메타데이터의 실제 출하, 공식·캐시 AAR 동일성, 공급자 고지 원문과 앱 고지 후보의 대응입니다. 완전한 Gradle resolved transitive closure, AndroidX 등 추가 의존성의 최종 선택과 고지, R8/DEX 클래스·메서드 생존, 각 기능의 런타임 사용, 상용 SDK 약관 권리 완료를 증명하지 않습니다. 선언된 POM 요청 버전을 최종 선택 버전으로 간주하지 않으며, #42 전체 검증 완료로 표시하지 않습니다.

외부 증빙은 `D:\meee\git\sudden-force-fps-validation\20261008-android-transitive-readonly\`의 `bounded-shipped-metadata.json`, `official-pom-map.json`, `official-aar-oss-map.json`, `vendor-license-fragments.json`, `combined-apply-verification.json`에 보존했습니다. 제품에 적용하는 고지는 검토용 머리말 348 bytes를 제외한 Google 원문 블록 624,205 bytes와 기존·Unity·Photon 고지를 바이트 그대로 연결한 후보를 기준으로 합니다. 28개 본문 전체와 104개 source mapping의 대응 검증은 고지 텍스트의 보존 검사이며 전체 앱 라이선스 완료 검사가 아닙니다.

### 고지 페이지 UI 검증 (2026-10-08)

최종 고지 파일은 660,971 bytes이며 SHA256은 `7bc6cbcff09412812115ab699b4215a1336b4ef6587a314a7a2f47b36299df2e`입니다. 기존 22,019 bytes prefix와 추가 라이선스 전문을 보존하고, 검토용 Google 머리말만 앱 표시용 제목과 분리했습니다.

`ThirdPartyNoticePager`는 원문 범위를 보존하며 현재 페이지의 문자열만 TMP에 전달합니다. 기본 한도는 4,096 UTF-16 단위/96 LF이며 실제 합본은 183페이지, 최대 4,092 단위였습니다. 폼피드 27개는 canonical 원문에 유지하고 표시에서는 페이지 구분자로 처리합니다. 페이지 source를 이어 붙인 UTF-8 bytes는 제품 파일과 정확히 같았습니다.

Unity 6000.3.25f1의 Play=false 임시 prefab/deep font·atlas·material 복사본에서 실제 Button pointer/EventSystem 경로로 183페이지 순회와 역순 이동을 확인했습니다. 표시 코드포인트 172개의 TMP glyph 대조 불일치·글리프 경고 0, 공급자 고지 28개 시작의 viewport 접근, 마지막 문장·마침표 접근, 닫기·다시 열기·빠른 페이지 이동·focus/interactable 복원·빈/누락 고지 처리를 확인했습니다. 대표 native 화면 6개를 육안 확인했으며, 모든 28개 시작을 각각 육안 검토했다는 뜻은 아닙니다.

이는 Editor UI 검증입니다. Android 물리 입력·기기 glyph·메뉴 성능이나 최종 AAB 포함 검증을 대신하지 않습니다. 임시 QA 객체를 정리했고 원본 폰트·아틀라스·기존 변경사항을 보존했습니다. 근거는 외부 `D:\meee\git\sudden-force-fps-backups\20261008-notices-full-ui-qa\HANDOFF.md` 및 같은 폴더의 source-byte-proof/glyph-and-mapping/vendor-header-access/last-bottom-access/post-qa-preservation JSON과 PNG에 있습니다. #42 전체 완료는 아닙니다.
### 광고·결제 제외 및 iOS 광고 잔재 정리 (2026-10-08)

광고·인앱 결제 기능을 제외하는 프로젝트 범위에 따라 기존 `Assets/Plugins/iOS/`의 Google Mobile Ads Unity 브리지 잔재를 제거했습니다. `GADUAdNetworkExtras.h`, `GADTSmallTemplateView.xib`, `GADTMediumTemplateView.xib`, `unity-plugin-library.a`와 각 meta 8개, 비어진 NativeTemplates/iOS 폴더 meta 2개가 대상입니다. 제거 전 10개 파일을 프로젝트 밖에 백업하고 해시를 대조했습니다.

정적 라이브러리는 x86_64/arm64 ar의 object 54개가 GADU/GADT/GAMU 이름이며, GADU 광고 생성·초기화와 GADT 템플릿 정의 및 GADMobileAds 외부 심볼로 광고 브리지임을 확인했습니다. importer의 `gvh_version-9.1.1`은 메타데이터 label이며 공식 해당 release의 바이트 동일성을 검증한 것은 아닙니다. 제거 파일의 importer는 Android 비활성/iOS 활성이고 해당 GUID·클래스의 다른 제품 파일 참조를 찾지 못했습니다.

조사 범위의 UPM manifest에 ads/purchasing/mediation 패키지가 없고 제품 스크립트·EDM/XML의 명시적 광고·결제 API/SDK 좌표도 발견되지 않았습니다. 이름에 ads가 들어간 headshot 애니메이션·ThreadScheduler 및 Google Play Games/EDM/라이선스 원문은 제거하지 않았습니다. 이는 전체 최종 DEX/모든 전이 의존성의 광고·결제 부재 증명이 아니며 최종 Android AAB 포함 검증은 별도입니다. iOS export/build도 검증하지 않았습니다.

출처·심볼·범위 근거는 외부 `D:\meee\git\sudden-force-fps-validation\20261008-ads-iap-residue-readonly\REPORT.txt` 및 archive-ownership-summary.json에 있고, 제거 전 백업은 `D:\meee\git\sudden-force-fps-backups\20261008-ads-ios-residue-removal\before-manifest.json`에 있습니다.

## AndroidX — 기준 APK 버전 metadata 및 공식 POM 조사

기준은 외부 6a8b203 UploadKeyValidation APK(SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`)입니다. 현재 main 또는 후속 Android QA APK로 조사 결과를 확장하지 않습니다.

APK에서 13개 명시적 AndroidX `.version` entry(각 6B, 총 78B)를 관찰했습니다. entry 이름에 따른 Maven 좌표 추정과 공식 POM의 groupId/artifactId/version 확인을 구분하며, 아래 13개 좌표는 공식 POM identity와 모두 일치합니다. 이는 전체 Gradle resolved graph나 최종 클래스 생존을 확정하는 증거가 아닙니다.

| 정확한 좌표 및 버전 | 공식 POM |
| --- | --- |
| `androidx.activity:activity:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/activity/activity/1.0.0/activity-1.0.0.pom) |
| `androidx.arch.core:core-runtime:2.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/arch/core/core-runtime/2.0.0/core-runtime-2.0.0.pom) |
| `androidx.core:core:1.2.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/core/core/1.2.0/core-1.2.0.pom) |
| `androidx.customview:customview:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/customview/customview/1.0.0/customview-1.0.0.pom) |
| `androidx.fragment:fragment:1.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/fragment/fragment/1.1.0/fragment-1.1.0.pom) |
| `androidx.lifecycle:lifecycle-livedata-core:2.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-livedata-core/2.0.0/lifecycle-livedata-core-2.0.0.pom) |
| `androidx.lifecycle:lifecycle-livedata:2.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-livedata/2.0.0/lifecycle-livedata-2.0.0.pom) |
| `androidx.lifecycle:lifecycle-runtime:2.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-runtime/2.1.0/lifecycle-runtime-2.1.0.pom) |
| `androidx.lifecycle:lifecycle-viewmodel:2.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-viewmodel/2.1.0/lifecycle-viewmodel-2.1.0.pom) |
| `androidx.loader:loader:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/loader/loader/1.0.0/loader-1.0.0.pom) |
| `androidx.savedstate:savedstate:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/savedstate/savedstate/1.0.0/savedstate-1.0.0.pom) |
| `androidx.versionedparcelable:versionedparcelable:1.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/versionedparcelable/versionedparcelable/1.1.0/versionedparcelable-1.1.0.pom) |
| `androidx.viewpager:viewpager:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/viewpager/viewpager/1.0.0/viewpager-1.0.0.pom) |

공식 POM 13개는 모두 The Apache Software License, Version 2.0을 선언합니다. 공식 AAR 총 1,088,562B를 승인된 각 2MiB/합 10MiB 한도 안에서 외부 폴더로 취득했고, 13개 모두 기존 exact cache AAR와 SHA256이 일치했습니다. POM의 dependency는 선언된 요청이며 최종 선택 그래프로 취급하지 않습니다.

AAR 및 메모리 전용 classes.jar의 승인된 LICENSE/NOTICE/COPYING 및 third_party_licenses.json/txt entry는 13개 모두 `NOTFOUND_IN_APPROVED_WHITELIST`입니다. 이는 해당 범위의 관찰이며 다른 위치의 원문이나 추가 의무가 없다는 뜻이 아닙니다. 새 AndroidX 공급자 copyright는 미확인입니다. 클래스/bytecode/DEX/general strings를 읽거나 classes.jar를 저장하지 않았습니다.

앱 고지에 적용한 AndroidX 섹션은 기존 660,971B prefix를 바이트 그대로 보존하고, 위 좌표/POM 출처와 공식 Apache 약관 전문을 독립 AndroidX 섹션으로 추가합니다. 약관 출처는 [Apache 공식 원문](https://www.apache.org/licenses/LICENSE-2.0.txt)이며 기존 외부 `Apache-2.0.txt` 11,358B를 exact 복사했습니다(SHA256 `cfc7749b96f63bd31c3c42b5c471bf756814053e847c10f3eb003417bc523d30`). 이 본문을 AAR embedded NOTICE 추출 결과로 주장하지 않습니다. 공급자 copyright를 만들지 않았고, 기존 GPGS 라이선스의 Google copyright를 AndroidX에 옮기거나 귀속 범위를 확대하지 않았습니다.

기존 GPGS 본문 재사용 여부 대조는 기존 Apache 전문 포함 확인에만 사용했습니다. 이번 독립 AndroidX 섹션은 해당 GPGS copyright 본문을 복사하지 않고 지정된 공식 Apache 약관 파일만 복사합니다. 전체 전이 closure, 최종 클래스 생존, 광고 부재 또는 전체 제품 라이선스 완료는 미확정입니다. 원문 공급자 copyright와 추가 의무는 후속 공식 릴리스 근거 확인 범위로 남깁니다.

독립 검토를 통과한 후보를 앱 고지에 적용했습니다. 적용 직전 기존 660,971B prefix와 후보 SHA256을 확인했으며 최종 파일은 675,225B, SHA256 `a58eb4b964c268847a85773f96449f7f24daaba85ef734f6032b8d30476a15d6`입니다. 위 183페이지 UI 검증은 적용 전 고지의 결과입니다. 추가된 AndroidX 섹션의 Editor 표시와 최종 Android 빌드 포함 검증은 별도로 진행합니다. 외부 조사·독립 검토 근거는 `D:\meee\git\sudden-force-fps-validation\20261008-androidx-metadata-readonly\`에 보존했습니다.

### AndroidX 추가 고지의 Editor 표시 검증 (2026-10-08)

적용 후 Unity 6000.3.25f1에서 고지 TextAsset 로딩과 실제 C# pager의 총 187페이지를 확인했습니다. source slices를 UTF-8로 재결합한 bytes는 적용 파일과 정확히 같고, 기존 660,971B prefix 및 1~182페이지 source는 보존됐습니다. 연결 경계인 183페이지부터 마지막 187페이지까지 실제 Button pointer/EventSystem 경로로 이동·왕복·끝 clamp·스크롤·닫기/재열기를 확인했습니다.

추가 구간의 표시 코드포인트 154개와 새 한글 43개를 TMP glyph와 대조했으며 불일치와 관련 경고는 0입니다. 마지막 마침표와 약관 마지막 문장이 viewport에서 접근 가능했습니다. 기존 전체 183페이지의 글리프 검증을 반복한 결과로 확대하지 않습니다. 원본 폰트·아틀라스의 깊은 복사본과 임시 prefab을 사용한 Play=false Editor 검사이며 Android 기기 표시·성능·최종 AAB 포함 검증은 별도입니다.

QA 전후 19개 보존 대상의 존재·해시가 정확히 같고 임시 객체·폰트·material·atlas·EventSystem을 정리했습니다. 외부 근거는 `D:\meee\git\sudden-force-fps-backups\20261008-androidx-notices-ui-qa\HANDOFF.md`, tail-verification.json, post-qa-preservation.json 및 Boundary/TailStart/LastBottom PNG에 있습니다. #42는 추가 에셋 출처 및 최종 빌드 검증이 남아 열린 상태로 유지합니다.
### EmojiOne 자동 포함 경로 제외 (2026-10-08)

후속 AAB 확인: 제품 HEAD0f46c28/1.0.193/code3/SHA256 `E8C216521CD68420F29C44F28A06E24B88B5C94B06FF67AF9314A03ED035949B`의 packed6,492개에서 EmojiOne 옛·새 sprite 경로와 sprite/PNG GUID 일치0을 독립 확인했습니다. 최신 ThirdPartyNotices TextAsset도 실제 AAB에서675,225 bytes를 추출해 원본 SHA256 `a58eb4b964c268847a85773f96449f7f24daaba85ef734f6032b8d30476a15d6` 및 바이트 일치를 확인했습니다. 아래 미검증 설명은 변경 직후 시점이며 원본 artwork 권리나 다른 에셋 전체 라이선스가 확정된 뜻은 아닙니다. AAB 전체 판정은 RELRO3건·개발 콘솔 잔존 때문에 PARTIAL입니다.

배포 범위를 줄이기 위해 `Resources/Sprite Assets/EmojiOne.asset`을 기존 `Sprites/EmojiOne.asset`으로 AssetDatabase.MoveAsset 이동하고 TMP Settings의 기본 sprite 참조만 해제했습니다. 원본 asset/meta bytes와 GUID `c41005c129ba4d66911b75229fd70b45`, 내부 material 및 PNG 참조는 그대로입니다. PNG·JSON·Attribution 원본은 보존했습니다. defaultSpriteAssetPath/enableEmojiSupport/font/fallback 설정도 유지하며 저장 중 발생한 kerning migration은 시작 값으로 복원했습니다.

정적 참조 검토와 Editor에서 Resources의 EmojiOne 및 기본 sprite 경로 로드가 null임을 확인했습니다. 이는 Resources/default 참조 경로를 끊은 결과이며 최종 새 AAB에서 sprite/material/PNG가 불포함된 증명이 아닙니다. 다른 포함 경로가 없는지는 새 빌드의 packed asset 근거로 확인합니다. 기존 APK에는 이 변경이 반영되지 않았습니다. EmojiOne 원본의 artwork 배포 조건이 확인된 것으로 확대하지 않습니다.

Play=false 임시 prefab/deep font 복사본에서 로비 일반 label과 고지 마지막 187페이지 slice, 채팅의 TMP 대입과 동등한 일반 한글·영문·기호 문자열 표시를 확인했습니다. ChatManager.AddMessage 자체나 실제 네트워크/사용자 채팅은 실행하지 않았습니다. 명시적 sprite 태그 3종은 문자 그대로 표시됐고, 폰트에 없는 😀는 □와 글리프 경고1개로 대체됐습니다. 이모지·sprite 표시가 기존과 완전히 같거나 채팅에서 쓰이지 않는다고 주장하지 않습니다. 일반 문자 검사의 경고0과 이모지 테스트의 예상 경고를 구분합니다.

원본 폰트·아틀라스·고지·prefab·기존 dirty 등 보존 대상을 대조하고 임시 객체를 정리했습니다. 외부 근거는 `D:\meee\git\sudden-force-fps-backups\20261008-emojione-resource-exclusion\`의 apply-result.json, ui-verification.json, preservation.json 및 HANDOFF.md입니다. 롤백은 AssetDatabase.MoveAsset로 기존 Resources 경로로 되돌리고 TMP 기본 참조를 원래 GUID로 복원합니다. 새 GUID를 생성하거나 원본 PNG를 재임포트하지 않습니다. #42는 최종 빌드 포함 여부와 남은 출처 확인까지 유지합니다.

## 기존 폰트 기반 메뉴 자산과 촬영 배경 (2026-10-18, #107)

메뉴용 `SuddenForceMenuFont.asset`는 기존 LiberationSans SDF의 static atlas·material을 공유하고, `SuddenForceMenuCjk.asset`는 기존 DungGeunMo.ttf에서 만든 별도 dynamic fallback입니다. 새 글꼴 다운로드 없이 원본 두 SDF·두 TTF와 각 meta의8개 SHA256을 생성 전후 동일하게 보존했습니다. Liberation Sans2.00.1의 copyright·SIL OFL1.1 전문과 DungGeunMo1.301의 Public Domain·저자·출처 고지는 기존 `Assets/Legal/ThirdPartyNotices.txt`에 포함되어 있습니다. 원본 폰트 조건을 유지하며 전체 CJK 지원을 뜻하지 않습니다. 설정·보존 범위는 [UI 자료](Tools/UI/README.md)에 기록합니다.

`DesertHouseBackdrop.png`는 기존 DesertHouse 씬 촬영의 alley 시점을 메뉴 배경으로 재사용한 파생 이미지입니다. 새 외부 이미지·AI 생성 자산이나 CC0 전환으로 분류하지 않으며 Low Poly FPS Map Lite의 기존 취득·원본 대응 근거를 유지합니다. 이 추가가 #42 전체 권리 검증 완료나 새 Android 배포 검증을 뜻하지는 않습니다.
