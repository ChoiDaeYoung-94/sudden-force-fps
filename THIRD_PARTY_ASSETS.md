# Third-party asset provenance

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
