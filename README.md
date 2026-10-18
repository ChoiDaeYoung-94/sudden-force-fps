# Sudden Force FPS

## 메뉴 화면 정리 (2026-10-18, #107)

로그인·로비·방 대기 화면에 실제 맵 촬영 배경과 어두운 색상·금색 강조를 적용했습니다. 기존 Latin 정적 글꼴과 별도1024 한국어 동적 fallback을 연결하고, 로비의 현재 메뉴 제목·선택 표시, 방 카드의 긴 이름 생략·가변 폭 배치, 팀 색상·주요 버튼 간격을 정리했습니다. 맵 키·기존 버튼 이벤트·방 생성과 팀 선택 흐름은 유지하며 미구현 비공개 메뉴는 계속 비활성입니다.

1920×1080/1280×720/2400×1080 격리 화면과 한국어·긴 이름 표본을 확인했습니다. 실제 Play 모드에서 맵 선택8검사, 메뉴 이동·고지 열기/닫기·선택 표시·뒤쪽 입력 복구를 합성 포인터로 확인했고, 한국어/영문 표본 누락0과 원본 폰트8파일 SHA 보존을 확인했습니다. 모달 활성 직후 같은 eval에서 다음 버튼까지 누르던 첫 하니스는 프레임 분리 후 재검증했으며 실패 기록을 보존했습니다. 이 결과는 실제 Android 터치·신규 빌드·전체 CJK 지원 검증을 뜻하지 않습니다. 현재 Play 제공본은 코드21이며 새 빌드는 후속입니다.

## 실제 맵 미리보기와 선택 표시 (2026-10-18, #106)

로비와 대기실의 흰 맵 이미지3곳을 실제 DesertHouse 촬영본으로 교체하고,480×270 카드에 맵 이름·선택 테두리·현재 맵 표시를 추가했습니다. 대기실의 빈 변경 버튼에는 CHANGE 라벨을 넣었습니다. 표시명과 내부 `DesertHouse` 키를 분리해 읽기 쉬운 이름을 표시해도 방 생성 식별자는 유지됩니다.

실제 재생 중 저장된 버튼을 합성 포인터로 선택한8개 검사와1920×1080/1280×720/2400×1080 화면 검토를 완료했습니다. 검사 전 Editor 닉네임 모달을 닫았으며 닉네임 저장·방 생성·실제 터치 검증은 하지 않았습니다. 새 텍스처의 출처·카메라·import 설정은 [UI 자료](Tools/UI/README.md)에 기록했습니다. 메뉴 테마와 새 빌드는 후속이며 Play 제공본은 코드21입니다.

## 로딩 표시 통합 (2026-10-18, #105)

로그인 안내를 씬 전환 시작 시 닫고, 일반 작업과 씬 전환이 겹치면 씬 로딩 하나만 표시합니다. 일반 로딩 배경도 불투명하게 바꿔 뒤쪽 로비 연결 안내가 겹쳐 보이지 않도록 했습니다. 전환 중 중복 요청을 거부하고 실패 시 로그인 재시도를 복구하며, 씬 로딩 배경이 뒤쪽 터치와 Back을 막습니다. 인증·Fusion 접속 흐름은 유지했습니다.

Unity 6000.3.25f1에서 격리 논리 검사18개와 실제 Manager 프리팹의 상태·콜백·합성 포인터 검사10개를 통과했습니다. Editor 로그인 재진입은7프레임 동안 최대 로딩 표시1개, Login 언로드·Lobby 활성·전환 플래그 해제·Runner1개를 확인했습니다. 실제 Google 로그인·터치·Android 빌드 검증은 아닙니다. 재생 종료 전 Console 오류0, 컴파일 실패false이며 경고는 남습니다.

기존 프로젝트의 종료 후 잠금이 남아 별도 Git 워크트리 `D:/codex/worktrees/sudden-force-fps-ui-20261018`에서 검증했습니다. 기존 소스·미커밋 변경·Library와 서명 키는 보존하고 공유하지 않았습니다. 현재 Play 제공본은 코드21, 기존 코드25는 미서명·미업로드이며 이번 변경의 새 빌드는 아직 없습니다. 맵 미리보기·메뉴 개선은 #106/#107, 방 목록 갱신은 #108에서 진행합니다.

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


## CC0 소총 생명 애니메이션 소스 전환 (2026-10-17, #42)

Quaternius 공식 CC0 자료를 기존 Military Avatar에 리타깃한 조준·사망2클립과2상태 controller를 추가하고 SoldierBlue/Red에 연결했습니다. 양손 소총 그립·호흡 루프·뒤로 넘어지는 사망·마지막 자세 유지가 포함됩니다. 기존 상태 이름과 Avatar/네트워크/히트박스/무기 바인딩·루트모션 비활성·독립3초 리스폰은 보존했습니다. 기존22클립/컨트롤러는 삭제하지 않았습니다.

저장된 제품 프리팹의 실제 Editor Animator를 양 팀·원점/이동 및 yaw137에서 Idle→Death→Idle로 평가했습니다. 키 사이·비정규 시각 포함3,144회/12경우가 같은 기준을 통과했고 최소 표면 여유5.003mm·최대 사망 그립 오차3.773mm·총구 편차0.757도·루트 이동/회전0입니다. 원본 Generic Humanoid 직접 임포트 실패와 초기 침하/보간 실패는 보존했습니다. 전완 회전 경계는 동일 뼈 자세의 주기4를 실측한 뒤 연속화했고, 새 높이 내장 클립만 legacy 바닥 보정을 끕니다. 기존 보정 기본값true/실행 경로도 확인했습니다. 이는 분리된 Editor 재생/guard 검증이며 실제 Fusion snapshot 전환·전체 경기·기기 통과가 아닙니다.

공급자 선언은 원본372B의 끝 공백·빈 줄만 정리해 369B로 보존했고 문구 변경은0입니다. 기존 고지709,379B prefix를 바이트 그대로 보존해 고지는710,172B/SHA256 `6EAC1F6A808BE68E30E3BA25E6BAAD1D628F58427219471116FD8D0FB35DFD53`입니다. 두 클립의 재생성 입력/명령·원본 SHA·수정 내역은 [애니메이션 자료](Tools/AnimationBaking/README.md)에 기록했습니다. 두 Soldier의 정적 의존성과 실제 packed 제외는 구분하며 후속 빌드로 기존22클립 포함 여부를 확인해야 합니다. #42 전체 권리 대응 완료로 닫지 않습니다.

추가 분리 Play 검증에서는 프레임 증가·양 팀 조준 루프·사망 마지막 자세 유지·조준 복귀를 확인하고 화면을 보존했습니다. 네트워크 Behaviour는0개 활성/실제 spawn0이며 Animator 상태를 직접 전환했으므로 Fusion snapshot·실제 리스폰 검증으로 확대하지 않습니다. 임시 오브젝트0·기존 clean Untitled 복귀·기존22원본 보존·두 Soldier 정적 옛 클립 의존성0·컴파일 실패false/현재 Console 오류0을 확인했습니다. SDK 구식 API/URP 호환 모드와 검증용 body 위치 조회 경고는 남겨 두었으며 경고0으로 보고하지 않습니다. Play 중 재직렬화된 EDM3설정은 사전 기준값으로 저장 복원하고 이 변경에 포함하지 않았습니다.

**현재 Play 제공본은1.0.195/코드21이며 이 애니메이션·고지는 아직 미포함입니다.** 이번 소스 작업의 새 AAB/서명/업로드/예약은0, 원장 `195,3,22`·다음 새예약23 이상입니다. 기기 검증 후순위·성능 후속과 전체 약92% 작업 기준 추정은 유지합니다. 다음 PC 우선순위는 새 소스 빌드·packed 의존성/고지 검증입니다. 현재 사용자 조치·질문은 없습니다. 아래 기록은 각각 당시 상태입니다.

## 공식 Unity EDM 전환 검증 (2026-10-17, #98/#66)

공식 EDM2.1.1로 전환하고 구형5DLL 원본을 보존한 채 실행만 비활성화했습니다. 강제 Resolve2회 후 현대 `packaging` 문법·GPGS2.0.0 의존성 유지와 코드22 공식 증분 export 오류0/경고71/160.177초, 별도 offline C1 Gradle dry-run exit0을 확인했습니다. 템플릿 갱신 안내·총괄 Yes 조작0, dry 실행태스크0/AAB0/입력 보존입니다. 전환 초기 메뉴 대기는 소유 Editor 재기동과 닫힌 상태의 importer 설정으로 복구했으며 공식 메뉴 완료로 주장하지 않습니다.

**현재 Play 제공본은 코드21**이며 코드22는 export 검증만 수행했습니다. 새 AAB 생성·서명·업로드0, 원장 `195,3,22`·다음 새예약23 이상입니다. 기존 dirty/자산·키·출력은 보존했고 실기기·성능 측정 후순위, 전체 약92% 작업 기준 추정을 유지합니다. 깨끗한 캐시/새 checkout·iOS·Unity 단일 AAB 호출 안정화와 충돌 근본 해결은 미검증입니다. 현재 사용자 조치·질문은 없습니다.

다음 PC 우선순위는 CC0 애니메이션 후보의 Humanoid 리타깃/양손 소총 자세 검증과 남은 에셋 대응입니다. 자세한 근거는 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)의 공식 EDM 전환 기록을 따릅니다. 아래 코드21·이전 기록은 각각 당시 상태입니다.

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

## 후속 소스: 자체 원형 HUD 검증 (2026-10-17, #42)

CanvasJoyStick의 테두리 1개와 DesertHouse HUD의 테두리·손잡이 2개를 자체 `CircleGraphic` 메시로 대체했습니다. 기존 RectTransform·색상·활성 상태·레이캐스트·입력 바인딩을 보존했고, 두 자산의 Animated Loading Icons 의존성은 0건입니다. 이전 자체 로딩 표시와 함께 남은 직접 spritesheet 참조 3개를 제거한 소스 변경입니다.

Unity 6 컴파일·저장 자산의 누락 스크립트 0건·분리된 Play 렌더링과 조이스틱 두 입력 경로의 합성 포인터 콜백을 확인했습니다. 실기기 터치·전체 경기·새 AAB 검증으로 확대하지 않습니다. 현재 Play 제공본은 **1.0.195 / 코드20**이며 이번 HUD와 이전 자체 로딩 변경은 포함되지 않았습니다. 새 빌드·서명·업로드·예약은 0건, 원장 `195,1,20`·다음 코드21 이상을 유지합니다.

외부 원본 팩·고지는 보존했고 팩 전체의 배포 제외나 에셋 권리 검증 완료를 주장하지 않습니다. 다음 PC 우선순위는 플레이어 애니메이션 22클립의 당시 패키지 대응과 후속 소스 빌드입니다. 실기기 후순위·전체 약92% 작업 기준 추정은 유지하며 현재 사용자 조치는 없습니다. 아래 자체 로딩·코드20 기록은 각각 당시 범위입니다.

## 후속 소스: 자체 로딩 표시 검증 (2026-10-17, #42)

LoginCanvas·Manager의 로딩 표시3개를 자체 `LoadingSpinnerGraphic`으로 대체하고 외부 로딩 Animation7개 참조를 제거했습니다. 외부 sprite/clip을 쓰지 않는270도 링이며 시간 정지에도 회전하고 raycast를 막지 않습니다. 두 프리팹의 나머지224컴포넌트·61오브젝트 바인딩/레이아웃/활성 상태를 보존했습니다.

현재 Play 제공본은코드20이며 **이 소스 변경은 코드20에 포함되지 않았습니다.** 새 빌드/서명/업로드/예약0, 원장195,1,20·다음21 이상입니다. 단종 팩 원본은 삭제하지 않았고 DesertHouse의spritesheet참조2개와 CanvasJoyStick.prefab의1개는 남습니다. 팩 전체 배포 제외·전체 권리 완료로 처리하지 않고 #42를 유지합니다. 실기기 검증 후순위와 전체 약92% 작업 기준 추정을 유지합니다.

다음 PC 우선순위는 남은 원형 HUD 이미지3참조의 자체 표시 대체와22클립 원본 대응입니다. 아래 코드20 제공 완료·이전 고지 기록은 각각 당시 범위입니다.

## 코드20 서명·Play 내부 테스트 제공 완료 (2026-10-17, #66/#42/#43)

2026-10-17 16:33, 총괄이 사용자 지정 작업용 Chrome의 Sudden Force/패키지·내부 테스트 트랙을 확인한 뒤 코드20(1.0.195)을 직접 업로드·출시했습니다. Console의 release6 **내부 테스터에게 제공됨** 상태를 확인했습니다. 현재 제공본은코드20이며 코드18/19 산출물과 이전 검증은 보존했습니다.

서명 AAB92,843,051B/SHA256 `7C051D98CB68E70878F4EFC18E50CFA67E5D192910DEA58802AE8EF908F9DA61`의 기존 업로드 인증서·전체129payload 무결성/무서명본 동일성·ZIP/bundletool·min25/target36·ARM64 SO7개 LOAD16KB·고지709,379B 본문 일치를 확인했습니다. 서명 후 시작 기준3,879개 소스/설정은 예약 원장 외 정확히 같으며 기존 dirty를 보존했습니다. 원장195,1,20·다음 새예약21 이상입니다.

증분 export·동일 입력2,734개/NDK7,935개 대조를 통한 코드19 IL2CPP 산출물 재사용·코드20 새 CMake/Swappy 패키징 결과입니다. 코드20 새 IL2CPP 컴파일·Unity 단일 AAB 호출 안정화·충돌 근본 해결이 아닙니다. RELRO4건·코드20 Store 설치/로그인/Fusion 경기·실제16KB는 후순위 미검증입니다. 실기기/장시간 성능 합의와 전체 약92% 작업 기준 추정을 유지하며 지금 추가 수동 시험·USB 연결을 요청하지 않습니다.

아래 항목의 원장·다음 코드·미완료 표현은 각 시점의 이전 기록입니다.


## 후속 배포 고지 준비 (2026-10-17)

2026-10-17 추가: Photon Fusion 공급자 선언과 실제 코드19 생성 입력에 대응하는 UnsafeCollections 메모리 보조 부분의 MIT 고지1건을 소스에 추가했습니다. 새 소스709,379B/SHA256 `F35330D344D7209A617A8CFC919E2A2FDC26E6BC30090E3EAA26B6D76EBB9790`의 기존707,770B prefix를 바이트 그대로 보존했고, 공식 고정 LICENSE 전문·pager197페이지 원문 round-trip·Editor TextAsset 바이트와 에셋 GUID 보존을 확인했습니다. 앱 UI·후속 AAB 포함·기기 표시는 미검증입니다. 코드19은 이전707,770B 고지의 검증용 무서명 AAB로 보존하며 서명/업로드0, 현재 Play 제공본은 코드18입니다. 원장195,0,19·다음 새코드20이상·전체 약92% 추정과 기기 검증 후순위 합의를 유지합니다.

상세 근거는 [에셋·고지 기록](THIRD_PARTY_ASSETS.md)을 따릅니다. 아래 코드19 빌드 결과와 이전 배포 이력은 당시 고지에 대한 검증입니다.

## 코드19 빌드 진행 (2026-10-17)

2026-10-17 코드19: Unity6000.3.25f1의 새 공식 Gradle export가 오류0·경고71·448.111초로 완료됐습니다. 이어 별도 C1 JVM·명시4GiB·processor1/worker1·parallelfalse 및 IL2CPP/Bee count1 조건의 offline Gradle bundleRelease가 exit0으로 끝나, 버전1.0.195/code19의 무서명 AAB 92,829,022B/SHA256 `EE26786D896B590FE5CDA48597BA240708EBBB7D2E18241991BE3A89C4C46693`를 생성했습니다. ZIP/bundletool·패키지·min25/target36·비개발·ARM64 SO7개 LOAD16KB와 새 고지707,770B의 본문 바이트 일치를 확인했습니다. 원장 `195,0,19`는 유지하며 다음 새 코드는20 이상입니다. **코드19 서명·업로드·기기 실행은 아직 미검증이고 현재 Play 제공본은 코드18**입니다. export와 별도 패키징의 성공이며 Unity의 단일 AAB 호출 안정화나 과거 충돌의 근본 해결로 확대하지 않습니다.

후속 SDK 조사에서 UnsafeCollections 메모리 보조 부분의 공급자 MIT 고지1건을 추가로 확인했습니다. 따라서 코드19은 검증용으로 보존하고 서명·Play 업로드를 진행하지 않습니다. 공식 원문을 포함한709,379B 후보의 prefix 보존·pager197페이지 검사를 완료했으며, 소스 보완 후 새코드20이상에서 실제 고지 포함과 배포를 이어갑니다. 코드19의 생성 성공과 배포 준비 완료를 구분합니다.

상세 검증과 제한은 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)를 따릅니다. 아래 코드18과 이전 기록은 각 시점의 결과입니다.

> Photon Fusion 2 기반의 호스트·클라이언트 멀티플레이 Unity 3D FPS 프로젝트입니다. 로비에서 방을 만들고 팀을 선택해 DesertHouse에서 한 판을 진행합니다.

## 코드18 제공 당시 상태 (2026-10-17 정리, 이전 기록)

최신 결과는 **코드18 서명 AAB 생성·검증 및 Google Play 내부 테스트 release5 제공 완료**입니다. 총괄이 지정된 Console 탭에서 직접 업로드·출시했고 10월16일16:37의 내부 테스터 제공 상태를 확인했습니다. 코드15에서 Bee 작업 수를1로 제한해 새 고지 포함 데이터·ARM64 네이티브 컴파일/링크를 완료했으나 Unity의 실제 Gradle JVM 옵션 게이트가 차단해 전체 빌드는 Failed였습니다. 재부팅으로 결과를 확인할 수 없는 코드16과 CMake 경로 검사로 중단된 코드17의 예약을 보존하고, 실제 `.utmp` 경로 검사와 자식 JVM C1/worker1 조건을 통과한 별도 Gradle 실행으로 코드18을 생성·서명했습니다. 서명본92,840,760B의 전체 payload129개가 기존 업로드 인증서로 검증됐고 서명 전후 payload129개 바이트 차이0입니다. 원장은 `194,10,18`이고 다음 새 코드는19 이상입니다. clean Unity 전체 빌드 성공이나 재부팅 원인 해결로 확대하지 않습니다.

코드18의 패키지·버전18·비개발·ARM64·ZIP·bundletool 및 SO7개 LOAD16KB를 확인했고, 보완 고지694,428B가 소스와 정확히 일치합니다. 지정 광고·결제 SDK를 조사한 코드12의 DEX와도 바이트가 같습니다. 무서명 검증 파일은 Play 업로드 대상이 아니며 실제 업로드한 것은 서명본입니다. 출시 당시 Console의 가독화 파일·네이티브 디버그 기호 미업로드 권고2건 중 네이티브 심볼은 후속으로6/7개 대응 ZIP을 직접 업로드해 코드18의 첨부 완료 표시를 확인했습니다. nanosockets 심볼·핵심 Unity3개 소스 행번호·매핑 파일과 Console 권고 전체 해소는 미확인 범위로 유지합니다. GNU_RELRO 정적 경고4건과 실제16KB 실행 등 미검증 범위도 보존합니다. 사용자 결정에 따라 실기기 검증 전반은 기기를 작업 PC에 연결해 직접 확인할 수 있을 때까지 후순위로 두고, 코드18 Store 설치·실행은 아직 검증하지 않았습니다.

2026-10-17 소스 고지 보완: UniRx의 역사적 Rx.NET 차용 코드와 Photon 공급자 선언에 따른 Simple Scene BVH 안내·Apache2.0 전문을 추가했습니다. 기존 고지 전체 문구와 Git 정규화 원본 prefix를 보존한 새 소스707,770B의 pager196페이지 원문 보존 검사를 통과했습니다. 현재 배포 코드18은 기존 고지 그대로이며 이번 추가의 새 빌드·서명·업로드·예약은0입니다. 새 고지의 Editor/Android 표시와 후속 AAB 포함은 미검증입니다. 원장194,10,18·다음 코드19 이상을 유지하며 상세는 [에셋·고지 기록](THIRD_PARTY_ASSETS.md)을 따릅니다.

아래 코드13·14 및 이전 배포 기록은 당시 결과입니다.

고지 보완 후 새 AAB 시도는 코드13·14 모두 실패했고 배포된 코드12는 그대로입니다. 코드13은 UnityLinker/.NET8 접근 위반, 코드14는 프로세서 수1 조건에서도 APIUpdater의 MovedFromExtractor 접근 위반으로 실패했습니다(각 오류2·경고71, AAB0). 원장 `194,6,14`를 보존하며 다음 새 코드는15 이상입니다. 개별 Linker 단독 검사의 성공을 전체 빌드 안정화로 확대하지 않습니다. 상세는 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)의 코드13·14 기록을 따릅니다.

`1.0.194 / versionCode 12`는 사용자 업로드 후 Google Play 내부 테스트 release4로 제공 중입니다. 10월16일 총괄이 Console의 패키지·코드12·내부 테스터 제공 상태를 직접 확인했습니다. 실제 참여 링크는 [Sudden Force 내부 테스트](https://play.google.com/apps/internaltest/4701524132186452960)입니다. 기존 앱·데이터를 보존하고 등록된 테스터 계정으로 참여한 뒤 Play 스토어에서 업데이트합니다.

Galaxy Note20 Ultra·S25 Ultra의 Store 업데이트→로그인→로비→방 참가를 요청한 질문에 사용자는 위 링크로 테스트가 잘 됐다고 답했습니다. 이어 MENU 열기/닫기와 사망/리스폰 중 사격을 계속 누른 두 상황에 대해서도 새로 눌러야 사격된다고 답해 의도한 입력 차단을 확인했습니다. 사용자 수동 보고이며 기기별 화면·설치 코드/인증서·Android 버전·최신 한 경기 종료/재참가·FPS를 직접 계측한 결과는 아닙니다.

코드12는 기존 검증 export와 프로세스 한정 C1 JVM 조건으로 생성·서명했습니다. AAB92,835,970B의 인증서·payload127개 전체 서명과 원본 바이트 일치, manifest·ARM64·비개발·ZIP·bundletool·고지 본문을 확인했습니다. 당시 원장194,4,12와 이후 코드13·14 실패 예약을 구분합니다. clean Unity 재빌드와 이전 실패의 근본 원인 해결을 확정하지 않습니다.

이전 코드11 시도는 dry-run 통과 후 실제 Gradle JVM의 C2 스레드 충돌로 실패했습니다. 당시 AAB·서명0과 원장194,3,11 예약을 보존했고 이후 코드12의 C1 조건 생성·서명으로 이어졌습니다.

10월15일 새 PC의 비개발 ARM64 AAB 빌드는 코드10을 예약한 뒤 Player 스크립트 컴파일의 APIUpdater/CLR 오류로 실패했습니다(오류3·경고71, AAB 미생성). 원장 `194,2,10`을 보존하며 다음 코드는11 이상입니다. 사용자 지시로10부터 사용했으며 Play 최대 코드를 직접 재조회한 결과는 아닙니다. 이전 오류0 Gradle export 기반의 배포 경로를 별도 검토 중입니다. 아래 기존 Play APK·무서명 APK 성과와 이번 새 서명 빌드 실패를 구분합니다.

`1.0.194 / versionCode 5`는 2026-10-11 Google Play 내부 테스트에 출시했습니다. 전달한 Play 서명 APK는 10월 14일 사용자 수동 테스트에서 Galaxy Note20 Ultra·S25 Ultra의 두 피어 경기 진입, 동시 이동·시점·사격과 입력 해제, 재장전·사망·리스폰, 경기 종료·로비 복귀·새 방 참가가 정상이라고 보고됐습니다. 직접 APK 설치 결과이며 Play 스토어 설치/업데이트 결과와 구분합니다.

새 PC에서는 10월 14일 실제 ARM64 IL2CPP 컴파일과 검증용 무서명 APK 패키징을 완료했습니다. 이 APK는 설치·Drive 전달·Play 업로드 대상이 아닙니다. SO7개 LOAD16KB 정렬을 확인했지만 RELRO 정적 경고4건과 실제16KB 기기 검증은 남았습니다. SO는 압축 저장돼 zipalign 통과를 비압축 SO의16KB 직접 배치 검증으로 확대하지 않습니다. 오류·기존/누락/빈 산출물의 빌드 성공 판정은 [PR #76](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/76)으로 보강했고 [#75](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/75)는 닫혔습니다. 수정 Editor 컴파일과8개 함수 경계 사례가 통과했으며 서명 빌드·batch 통합 실행과는 구분합니다.

남은 핵심은 clean 빌드 충돌 원인·재현성, 배포 포함 에셋의 원본 대응·조건부 SDK 고지 확인과 코드18 Store 설치·경기 종료/재참가·비정상 연결 종료·동시 입력 경계·실제16KB 검증입니다. 새 고지 포함 AAB의 내부 테스트 배포는 완료했으며 실기기 항목은 PC 연결 가능 시점까지 후순위로 유지합니다. 10월16일 사용자 결정으로 발열·장시간 FPS/메모리·저사양 성능 측정은 후속으로 미루며 이번 내부 테스트 마무리의 완료 조건에서 제외합니다. 기존60 FPS 목표·저사양30 FPS 허용 기준과 과거 측정 기록은 보존하고 최신 측정 통과로 처리하지 않습니다. Console의 광고 없음 선언은 저장했고 검토 전송 전입니다. 광고·인앱 결제는 제외하며 공개 프로덕션 출시·새 자동 CI·PlayFab 통합·스킬/장비 확장은 별도 범위입니다. 전체 **약92%는 작업 기준 추정**이며 시간 비율이나 최종 품질 점수가 아닙니다. 시험 연기만으로 진행률을 올리지 않습니다.

이전 APIUpdater·shader·shutdown 진단의 실패와 복원 이력은 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)에 보존합니다. 일부 빌드 성공으로 간헐 충돌 원인 해결·재발 방지를 확정하지 않습니다. 과거 외부 백업·미커밋 변경의 복구나 서명 비밀번호 이전을 가정하지 않습니다. HUD 이미지7개는 사용자가 직접 제작 또는 AI 생성했다고 확인했으며 남은 출처는 [에셋 기록](THIRD_PARTY_ASSETS.md)을 따릅니다. 최신 검증 범위는 [멀티플레이 기록](MULTIPLAYER_TEST_PLAN.md)과 배포 점검표를 우선합니다.

## 주요 구현

| 기능 | 내용 및 관련 코드 |
| --- | --- |
| 멀티플레이 | Fusion의 ClientServer 로비, Host/Client 방 접속, 네트워크 입력·상태 동기화, 호스트 권한의 사격·피해 판정 · [NetworkRunnerManager.cs](Assets/Scripts/Managers/NetworkRunnerManager.cs) |
| 방 목록 UI | 항목을 재사용하는 스크롤 뷰 · [RoomManage.cs](Assets/Scripts/UI/RecyclableScrollView/RoomManage.cs) |
| 전투 | 이동·시점·달리기, 총기 반동, 부위별 피해, 탄약·재장전, 사망·리스폰 · [GamePlayerNetworkData.cs](Assets/Scripts/Creature/Player/GamePlayerNetworkData.cs) |
| 경기·HUD | 팀 점수·타이머·KD·킬 피드·스코어보드·결과 화면 · [NetworkMatchState.cs](Assets/Scripts/Game/Match/NetworkMatchState.cs) |
| 씬·복귀 | 일반 씬 전환과 Fusion 경기 씬 로드, persistent Managers가 종료된 Runner를 정리하고 새 로비 Runner를 생성 · [Managers.cs](Assets/Scripts/Managers/Managers.cs) |
| 로그인 | Android Google Play Games Services v2 인증·취소/실패/30초 시간 초과·재시도 처리. 기존 업로드 키 APK의 실기기 인증과 전달 Play 서명 APK의 사용자 로그인 보고 확인. 스토어 설치 경로는 별도 · [Login.cs](Assets/Scripts/UI/Login.cs) |

Fusion은 방 접속뿐 아니라 입력 전달, 권한에 따른 상태 복제, Hitbox 지연 보정 쿼리와 경기 씬 전환에 사용합니다. 지연·손실 환경의 전체 품질 검증은 별도이며, 상세 통과 범위와 미검증 항목은 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)에 구분합니다.

## 개발 목표

- [x] Photon Fusion 기반 멀티플레이
- [x] Google Play Games Services v2 로그인 경로 구현
- [x] 기존 업로드 키 APK의 Android 실기기 인증·두 피어 모바일 경기 확인(최종 품질 검증과 구분)
- [ ] 최신 버전 Google Play 테스트 배포·테스터 설치 검증
- [ ] 스킬 시스템 및 장비 시스템 구현
- [ ] 프리팹 구성과 재사용 방식 개선

시연 영상과 Google Play 공개 링크는 준비 중입니다.

스킬·장비는 기본 경기 이후의 별도 단계입니다. 광고·인앱 결제는 현재 및 향후 작업 범위에서 제외합니다. PlayFab 로그인 통합도 완료된 기능으로 표시하지 않습니다.

## 개발 환경

| 항목 | 내용 |
| --- | --- |
| 엔진 | Unity 6000.3.25f1 LTS |
| 언어 | C# |
| 네트워크 | Photon Fusion 2.0.13 |
| 빌드 대상 | Android APK·AAB, Windows x64 개발/QA 클라이언트 |
| Android 빌드 확인 | IL2CPP, API 25 이상·target/compile 36. 기존 개발 APK와 별도로 내부 테스트 code5는 ARM64 |

## 프로젝트 열기

1. 저장소를 복제합니다.

   ```bash
   git clone https://github.com/ChoiDaeYoung-94/sudden-force-fps.git
   ```

2. Unity Hub에서 복제한 `sudden-force-fps` 폴더를 프로젝트로 추가합니다.
3. **Unity 6000.3.25f1**로 프로젝트를 엽니다. Android 빌드에는 Android Build Support와 SDK/NDK/OpenJDK가 필요합니다.
4. `Assets/Scenes/Login.unity`를 열고 로그인 및 네트워크 설정을 확인합니다.

Photon 서비스 설정이 유효해야 실제 로비·방에 연결됩니다. Android 인증에는 기존 PGS 프로젝트의 패키지·서명 인증서 연결과 테스트 계정 권한이 필요합니다. 저장소만 복제한 새 계정에서 인증 성공을 보장하지 않습니다.

Editor와 Windows Standalone은 개발/QA용으로 Google 인증을 생략하고 로비로 진입합니다. 이 경로의 성공은 Android PGS 인증 성공을 의미하지 않으며, Android 빌드는 실제 Google 인증 경로를 사용합니다.

### 두 피어 실행

1. Editor에서 `Login.unity`를 열고 Play를 실행합니다. 두 번째 피어는 별도의 Windows 개발 Player 또는 인증 가능한 Android 기기로 준비합니다.
2. 각각 닉네임을 설정합니다. 한 피어가 `Create Room`에서 방 이름과 `DesertHouse`를 선택해 공개방을 만듭니다.
3. 다른 피어가 방 목록에서 참가하고, 서로 다른 팀을 선택합니다. Client가 `READY`를 누른 뒤 Host가 `START`를 누릅니다.
4. 결과 화면의 로비 복귀 버튼으로 돌아가 새 방에 재참가할 수 있습니다. 경기 중 Host 연결이 종료되면 Client는 새 Runner로 로비에 자동 복귀합니다. 실제 PC 양방향 검증 근거는 검증 기록의 P01–P06에 있습니다.

Windows 빌드는 활성 대상을 Windows x64로 바꾸고 4개 씬을 포함한 Development Build로 만듭니다. 기존 PC QA는 빌드 동안 Standalone define에 `URP_COMPATIBILITY_MODE`를 추가하고 완료 후 원래 define을 복원했습니다. [검증 빌드 도구](Assets/Scripts/Editor/MultiplayerValidationBuild.cs)는 대상 전환이나 Player 실행을 자동으로 하지 않습니다. 상세 빌드·설정 보존 절차는 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)을 따릅니다.

### 경기 규칙과 조작

현재 기본 규칙은 팀별 20점 선취 또는 300초 제한입니다. 적 처치로 팀 점수가 1 올라가며, 시간이 끝나면 높은 점수 팀이 승리하고 동점은 무승부입니다. 상대 팀 참가자가 모두 이탈하면 남은 팀이 승리합니다. 경기 종료 후에는 전투·리스폰이 중단됩니다.

기본 소총은 탄창 30발, 재장전 2초이며 피해는 머리 100·몸통 25·팔/다리 18입니다. 호스트가 명중·피해를 판정하며 아군 피해는 적용하지 않습니다. 사망 후 3초부터 안전한 팀 스폰을 찾고, 가능할 때 HP 100·탄약 30으로 리스폰합니다. 안전한 스폰이 없으면 대기하므로 항상 정확히 3초에 부활하는 것은 아닙니다.

| 동작 | PC | 모바일 구현 |
| --- | --- | --- |
| 이동 | WASD 또는 방향키 | 왼쪽 조이스틱 |
| 시점 | 게임 영역 클릭으로 커서 캡처 후 마우스 이동 | 오른쪽 빈 영역 터치 드래그 |
| 사격 | 마우스 왼쪽 버튼 누르기/유지 | 사격 버튼 누르기/유지 |
| 달리기 | Shift 유지 | 달리기 버튼 유지 |
| 재장전 | R | 재장전 버튼 |
| 스코어보드 | Tab 유지 | 점수 버튼으로 열고 닫기 |
| 커서 해제 | Esc, 게임 영역 클릭으로 재캡처 | 해당 없음 |

스코어보드를 열면 전투 입력이 차단됩니다. Editor의 `Preview Touch Controls`는 터치 UI 표시·포인터 동작을 점검하는 기능입니다. Editor는 PC 입력 경로를 유지하므로 이 미리보기로 실제 다중 터치, 기기 포커스·백그라운드 복귀, 성능을 검증했다고 할 수 없습니다. 모바일 조작 표는 구현 설명이며 실기기 통과 결과는 아닙니다.

### 씬 구성

| 씬 | 용도 |
| --- | --- |
| `Assets/Scenes/Login.unity` | 로그인 |
| `Assets/Scenes/Lobby.unity` | 로비 |
| `Assets/Scenes/Room.unity` | 방 |
| `Assets/Scenes/Game/DesertHouse.unity` | 게임 맵 |

## 다운로드

[과거 APK 산출물](https://drive.google.com/file/d/1SQojyQafq9IdmNvTONF80G8ri3QPqeXk/view?usp=sharing) — 기존 링크이며 최신 전투·로그인·복귀 수정이 포함된 빌드로 검증하지 않았습니다.

현재 전달용 파일은 Play Console의 기존 내부 테스트 버전5에서 다운로드한 `SuddenForceFPS-1.0.194-code5-PlaySigned.apk`입니다. 크기는99,434,034 bytes, SHA256은 `FC64BA8EFF0521A8CD12081293C15B84019D1B481E5B69B0E844DFBCCFFAAABF`이며 Play 앱 서명 인증서를 사용합니다. 사용자 지정 Drive에 전달한 파일로 공개 다운로드 URL은 이 문서에 게시하지 않습니다. 새 PC에서 새로 빌드한 APK가 아니며 Drive 직접 설치 성공도 Play 스토어 설치 검증을 대체하지 않습니다.

이전 2026-10-07 소스 `ea29fe6`의 `SuddenForceFPS.DebugValidation.apk`는 Android Debug 서명 개발본입니다. 위 Play 서명 APK와 구분하며 당시 파일·검증 범위는 [Android 릴리스 점검표의 APK 기록](ANDROID_RELEASE_CHECKLIST.md#최신-전투로그인복귀-포함-개발-apk-2026-10-07)에 보존합니다.

## 외부 라이브러리

- [Photon Fusion](https://doc.photonengine.com/fusion/current/getting-started/sdk-download)
- [play games plugin](https://github.com/playgameservices/play-games-plugin-for-unity/releases)
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
- [MiniJSON](https://github.com/Unity-Technologies/UnityCsReference/blob/master/External/JsonParsers/MiniJson/MiniJSON.cs)
- [Keystore Helper](https://assetstore.unity.com/packages/tools/utilities/keystore-helper-58627)
- [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068)
- [Safe Area Helper](https://assetstore.unity.com/packages/tools/gui/safe-area-helper-130488)

Google 로그인 참고: [Google Play Games Services 로그인](https://developer.android.com/games/pgs/android/android-signin?hl=ko)

## 빌드

Android 배포용 빌드 진입점은 [BuildScript.cs](Assets/Scripts/Editor/BuildScript.cs)입니다. Android를 활성 대상으로 선택하고 Play·컴파일·임포트가 끝난 상태에서 실행합니다. 활성 씬 4개와 기존 앱 패키지 `com.AeDeong.SuddenForceFPS`, 기존 서명 키·alias 및 `BuildInfo/buildinfo.txt`가 필요합니다.

| 방식 | 방법 |
| --- | --- |
| Unity 에디터 | `Build > AOS > APK` 또는 `Build > AOS > AAB`. `Build/AOS`는 스크립트가 생성 |
| 전용 배치 빌드 | Android 대상으로 `BuildScript.BuildAOSAPK` 또는 `BuildScript.BuildAOSAAB` 실행. 성공/실패에 따라 전용 배치 Editor exit 0/1 |
| 개발 APK 검사 | 별도 [AndroidValidationBuild.cs](Assets/Scripts/Editor/AndroidValidationBuild.cs)의 `Queue` 경로. 기존 Android debug key로 외부 새 디렉터리에 생성하며 릴리스 경로와 구분 |

배포용 빌드는 기존 서명 비밀번호를 Editor에 안전하게 입력하거나 `SUDDEN_FORCE_KEYSTORE_PASSWORD`와 `SUDDEN_FORCE_KEYALIAS_PASSWORD` 환경변수로 전달해야 합니다. 값은 소스·README·로그에 기록하지 않습니다. 새 키 생성으로 기존 앱 서명을 대체하지 않습니다.

기본 versionCode는 로컬 ledger와 PlayerSettings 중 큰 값에 1을 더합니다. `SUDDEN_FORCE_VERSION_CODE`를 지정하면 두 로컬 값보다 크고 2,100,000,000 이하인 정수여야 합니다. Play에 올리기 전 Console의 기존 업로드 코드도 확인해야 합니다. 실패한 빌드도 예약된 코드에 빈 번호를 남길 수 있습니다. 최신 debug code3은 Play 업로드 코드 예약을 뜻하지 않습니다.

APK 메뉴는 Development/LZ4, AAB 메뉴는 non-Development/LZ4HC/CleanBuildCache이며 둘 다 상세 빌드 보고서를 생성합니다. APK 메뉴를 non-Development 릴리스 APK로 표현하지 않습니다. 도구체인·서명·Resolver 재실행 시 템플릿 점검과 배포 선행 조건은 [Android 릴리스 점검표](ANDROID_RELEASE_CHECKLIST.md)에 있습니다.

### 기존 CI/CD 구성의 현재 상태

- Unity2022/macOS/self-hosted `buildpc` → App Center 구성은 [원문 보관 파일](.github/legacy/cicd.yml.disabled)로 이동했습니다. 활성 `.github/workflows`에 실행 정의가 없으므로 이 소스에는 자동 Android 빌드·배포나 PR 검증 CI가 없습니다. 과거 [unity-cicd](https://github.com/ChoiDaeYoung-94/unity-cicd) 안내는 현재 배포 절차가 아닙니다.
- [App Center 배포 기능은 종료](https://learn.microsoft.com/en-us/appcenter/retirement)되었습니다. 로컬 빌드는 Unity6000.3.25f1 절차를 사용하며 기존 Sudden Force 앱의 Google Play 내부 테스트 code5 업로드·출시는 완료했습니다. Play 스토어 설치본의 설치·로그인·경기 검증은 남아 있습니다.
- 2026-10-08 확인한 과거 Actions 2건은 completed/cancelled였고 저장소에 등록된 self-hosted runner는 없었습니다. 해당 시점의 확인이며 현재 runner 가용성이나 Unity6 라이선스·Android 도구체인·secrets 구성을 보장하지 않습니다. 새 CI 환경은 미구성입니다.
- 후속 CI는 확인된 runner에서 수동 `workflow_dispatch` 단일 빌드 작업으로 검증한 뒤 추가합니다. 로컬 빌드·실기기 검증과 GitHub CI 결과를 구분하며, PR 병합으로 빌드·배포를 자동 실행하지 않습니다. 진행 범위는 [이슈 #44](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/44)에 기록합니다.

## 계획·검증·라이선스

- [프로젝트 계획과 범위](PROJECT_PLAN.md)
- [멀티플레이 검증 결과·미검증 항목](MULTIPLAYER_TEST_PLAN.md)
- [Android 빌드·서명·Google Play 테스트 배포 점검](ANDROID_RELEASE_CHECKLIST.md)
- [외부 에셋과 라이선스](THIRD_PARTY_ASSETS.md)
- [기존 이슈와 현재 작업 연결](ISSUE_TRACKING.md)
