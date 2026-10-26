# 메뉴 미리보기 이미지

## 방 목록 맨 위 이동의 취소 수명 (2026-10-26, #237)

`MoveToTop`은 실행마다 새 CTS를 소유하고, 취소·비활성·종료에서 스크롤 차단 패널을 즉시 해제합니다. 취소된 이전 실행의 `finally`는 새 실행의 패널을 끄지 않으며, 각 CTS는 자기 비동기 작업이 끝날 때 정리합니다. 이동 시작 시 기존 ScrollRect 관성을 멈춥니다.

실제 `RoomManage` 공개 메서드와 자연 Editor update를 사용하는 격리 회귀22항목을 통과했습니다. 정상 완료·취소 후 재실행·연속10회·이전 finally 경계·빈 목록 전환과 명시적 disable/destroy 정리를 확인하고 기존 scene/root/preview·singleton을 보존했습니다. 초기 검사 helper의 delegate 형식 컴파일 실패는 보존한 뒤 새 revision에서 컴파일/회귀를 통과했습니다. 자동 Play lifecycle·물리 드래그 이벤트·Android 터치는 별도이며 제공 APK33에는 아직 미포함입니다.

## 이동 조이스틱 좌표와 해제 경계 (2026-10-26, #236)

조이스틱의 포인터 화면 좌표를 실제 `pressEventCamera`와 안정된 부모 RectTransform의 local 좌표로 변환합니다. 이동 반경·deadzone·손잡이 표시를 같은 UI 단위로 계산하며, 기존 고정/자유 모드와 반경 계수1.3·pointer 소유권을 유지합니다. 레이아웃 이동 뒤 새 누름에서 중심을 다시 읽고, 해제 시 원래 손잡이 local 위치를 복구합니다. 비활성 컴포넌트의 직접 Down/Drag 호출은 새 입력을 잡지 않습니다.

실제 Canvas·RectTransform·PointerEventData를 만든 격리 Editor 회귀195항목을 통과했습니다. Overlay와 eventCamera, 부모 배율0.5/1/2, 고정/자유 모드·대각선·최대 이동·다른 pointer 거부·배치 이동과 명시적 focus/pause/disable/cancel을 확인했습니다. 기존 scene/root/preview와 singleton을 보존했습니다. 이는 화면 해상도 자체 변경·물리 멀티터치·Android 자동 lifecycle dispatch의 통과가 아니며 제공 APK33에는 아직 미포함입니다.

## 팀 변경 거절 안내 (2026-10-22, #179)

대기실에서 선택한 팀에 빈자리가 없으면 기존 공통 팝업으로 `RED TEAM is full` 또는 `BLUE TEAM is full` 안내를 표시합니다. 자기 플레이어/방 정보가 아직 준비되지 않은 경우에도 재시도 안내를 표시합니다. 로컬 버튼 경로와 기존 서버 `CanChangeTeam` 검사는 같은 사유 함수를 사용하며, 서버 sender·정원 검증과 같은 팀 선택 시 READY 유지, 다른 팀으로 실제 이동할 때 READY 해제 정책은 유지합니다.

안내는 현재 복제된 방 인원에 대한 사전 검사입니다. 로컬 검사 이후 서버에서 발생한 슬롯 경쟁의 거절 응답을 보장하지 않으며, 일시적으로 오래된 roster에서는 요청이 로컬에서 차단될 수도 있습니다. 새 RPC·Networked 필드·프리팹 변경은 없습니다. 제공 APK27 및 무서명 AAB28에는 포함되지 않습니다.

실제 PC 두 피어/팀당1명/양쪽 READY에서 호스트의 합성 Canvas callback으로 BLUE 만석 문구·공통 팝업 등록, 정상 닫기·stack/owner 해제, 같은 RED 선택 시 팝업 없음과 양쪽 Team·READY 보존을 확인했습니다. 이후 정상 START의 두 경기 객체와 클라이언트의 Game 진입, 정상 로비 복귀·클라이언트/Editor 열린 process handle ExitCode0을 확인했습니다. 이는 headless 실행의 고정 검사 코드와 최종 집계 결과이며, 단계별 표본은 직렬화되지 않아 표시 시간·픽셀 렌더·원격 클라이언트 버튼·모바일 터치까지 검증한 것은 아닙니다. 새 Windows 검증 빌드는 오류0/경고2·360파일 SHA 일치이며 임시 검사 소스는 최종 보존 감사 후 제품에서 제거했습니다. 근거는 외부 `20261022-quality/extension-1500/team-switch-ux-runtime-01/`과 대응 Room/Game·종료·산출물 기록입니다.

## 자체 안전 영역 컴포넌트 통일 (2026-10-21, #157)

`CanvasLobby`·`CanvasRoom`의 안전 영역을 기존 자체 `ScreenSafeArea`로 통일했습니다. 두 대상은 full-stretch·offset0, ConformX/Y=true·Logging=false였고 외부 컴포넌트의 코드 참조·씬 override는 없었습니다. Unity `LoadPrefabContents`·`AddComponent`·대상 컴포넌트 제거·프리팹별 저장을 사용했습니다. 컴포넌트 ID 두 개는 새로 생성됐으며 프리팹 GUID·나머지 계층/이벤트·Canvas 직렬화 모드·카메라 바인딩·씬·자체 스크립트 meta는 유지합니다.

자체 코드는 NaN/Infinity·0/음수·화면 밖·float 정밀도로 사라지는 영역을 적용하지 않고 이전 배치/캐시를 보존합니다. 유효한 영역은 기존 anchor 계산과 offset0을 유지하고, 적용한 뒤에만 입력 캐시를 갱신합니다.

- 계산 31종: 유효7(전체·좌/우 노치·세로 상하 여백·양축 여백·1픽셀·와이드), 거부24(화면/영역 크기·음수/화면 밖·NaN/Infinity·float 퇴화/정규화 underflow).
- 실제 두 프리팹의 격리 contents와 DesertHouse preview의 기존 HUD에서 각각 같은31종을 적용했습니다. 잘못된 입력의 anchor/offset/cache 변경0, 정상 값 복귀, 현재 Screen 값의 Update/OnEnable 직접 호출을 확인했습니다. 하니스는 자산을 저장하지 않고 preview를 닫습니다.
- 벡터 비교 허용 오차는 거리 `1e-4`입니다. 실제 터치·렌더·기기 노치·화면 회전 시험을 뜻하지 않습니다. 이번 변경은 제공 APK27에 미포함입니다.
- 최종 C# 컴파일 오류/경고0. 프리팹441/203객체에서 대상 컴포넌트 교체와 부모의 component 참조 외 직렬화 내용을 보존했습니다. Unity 저장본과 원본을 외부 보존하고 empty scalar 표현만 정리했습니다.

첫 비활성 부모 조회의 NullReference, 존재하지 않는 CLI refresh 명령, filtered CRLF와 원래 native LF 바이트를 혼동한 첫 formatter 중단은 실패 이력으로 보존합니다. 첫 formatter는 제품 바이트 쓰기 전에 중단됐고, 원래 hash에 일치하는 Git blob으로 재검증했습니다. 초기29종 검증도 최종31종과 구분해 보존합니다. 근거는 외부 `20261021-safearea-source-audit-01/`의 최종 하니스·구조 감사·원본/저장본입니다.

## 실제 맵 미리보기

`Assets/Art/UI/MapPreviews/DesertHouse.png`는 외부 사진이나 AI 그림이 아니라 제품의 실제 `Assets/Scenes/Game/DesertHouse.unity`를 Unity 6000.3.25f1/URP에서 촬영한 이미지입니다. 원본 씬·모델·재질·조명은 변경하지 않았습니다.

- 카메라 위치 `(8,13,-11)`, 시선 대상 `(0,2,3)`, 수직 FOV52도,16:9, near0.1/far250, HDR 및 FXAA.
- 출력1536×864 PNG, 원본 이미지 SHA256 `CBDD3C507715D9B5045D3EBFA94037E7DE58D6AD72B2FFBB980F5AE8BEE2CEB3`.
- 촬영 시 씬 SHA256 `1556B5689150A9845964A421A037022718365E042F21E57D0B34E657028C6CB1`.
- `Maps` 아래 실제 Renderer123개의 bounds는 크기 `(20.36417,10.529734,30.36417)`입니다. 외벽만 보이던 초기 구도는 채택하지 않았습니다.
- UI Sprite 단일 모드, 최대1024, Bilinear/Clamp, mipmap·CPU readable 비활성, Android ASTC6×6. 로비/대기실 카드와 현재 맵에 같은 Sprite를 공유합니다.

원본 맵은 기존 JustCreate의 Low Poly FPS Map Lite 자산으로 구성되어 있으며, 취득·파일 대응 기록은 [에셋 목록](../../THIRD_PARTY_ASSETS.md)의 해당 항목을 따릅니다. 이 촬영으로 원본 자산의 권리를 새로 소유하거나 CC0로 전환한 것으로 보지 않습니다. #42의 전체 권리 검증 범위도 유지합니다.

맵의 화면 표시명 `Desert House`와 Fusion/씬 식별자 `DesertHouse`는 다릅니다. CanvasLobby는 내부 선택 키를 보관하고 생성 요청에 전달합니다. 표시 라벨을 내부 키로 다시 읽지 않습니다. 저장된 기존 버튼 인자와 호스트 전용 변경 흐름은 유지합니다.

2026-10-18 PC 검증은 실제 버튼의 합성 포인터 선택·상태 유지/초기화 및1920×1080,1280×720,2400×1080 격리 렌더를 포함합니다. 실제 Android 터치·새 AAB·최신 기기 경기는 별도 검증입니다.

## 메뉴 폰트와 맵 촬영 배경 (2026-10-18, #107)

`Assets/Art/UI/Fonts/SuddenForceMenuFont.asset`는 기존 LiberationSans SDF의 static atlas·material을 공유하면서 메뉴 전용 fallback chain을 분리한 폰트 자산입니다. 원본 LiberationSans SDF와 TTF·아틀라스·공용 material은 변경하지 않았습니다. `SuddenForceMenuCjk.asset`는 기존 DungGeunMo.ttf에서 만든 별도 dynamic fallback으로,1024×1024/point48/padding5/scale1/multiAtlas·ClearDynamicDataOnBuild 활성 상태입니다. 새 Texture/Material은 CJK 자산의 subasset으로 저장합니다. 초기 texture는 glyph 생성 전1×1이며1024는 페이지 설정값입니다. multiAtlas는 전체 페이지 수 제한을 뜻하지 않습니다.

Liberation Sans2.00.1의 SIL OFL1.1 및 DungGeunMo1.301의 Public Domain 표기는 기존 TTF·동봉 자료와 앱의 `Assets/Legal/ThirdPartyNotices.txt` 고지에 대응합니다. 새 글꼴을 다운로드하지 않았습니다. DungGeunMo는 이번 한국어 UI용 fallback이며 모든 CJK·사용자 이름·기호 지원을 보장하지 않습니다. 공용 원본 material의 shader 속성은 직접 수정하지 않습니다.

메뉴 배경 `Assets/Art/UI/MapPreviews/DesertHouseBackdrop.png`는 같은 기존 DesertHouse 촬영의 alley 시점 PNG입니다. 카메라는 `(0,2.4,-8)`에서 `(0,1.5,5)`를 보며 FOV52/1536×864입니다. 원본 PNG SHA256은 `7AE53834FFF98BA294B3232BE3D9038E8E60DBDCC610709CD38946D45D4D71C4`입니다. 외부 사진이나 AI 이미지가 아니며 기존 Low Poly FPS Map Lite의 출처·조건을 유지합니다. 폰트 연결 후 가독성·혼합 언어 baseline·줄바꿈과 실제 기기 검증은 별도 확인합니다.

최종 연결 후1920×1080/1280×720/2400×1080의7종 화면(총21장)과 한국어·긴 방 이름 표본을 확인했습니다. 영문·한국어 UI 표본의 HasCharacters 누락0, 동적 글리프45개/1024 한 페이지를 확인한 뒤 새 fallback만0글리프/1×1로 비워 저장했습니다. 실제 Play 합성 포인터의 메뉴·고지·맵 선택 검증은 README의 #107 기록을 따릅니다. 실기기와 전체 CJK·전체 고지 문자 지원은 별도입니다.

## 방 카드의 가변 폭 (2026-10-18, #108)

목록은 PanelRooms와 실제 Viewport 중 좁은 폭에서 padding·열 간격을 뺀 뒤 열 수로 나눕니다. 폭 변경 때 기존 풀의 가로폭/X만 갱신하며 Y·행 인덱스·세션·객체 ID를 유지합니다. 방 이름은 일반 굵기/Ellipsis, 인원은18–30 자동 크기로 둡니다. 별도 bold typeface가 없는 TMP의 Bold700 ellipsis 검색은 Truncate로 바뀔 수 있어 새 atlas나 원본 material 변경 대신 제목 설정만 조정했습니다. 실제 메뉴 해상도 검증과640 UI 단위의 인위적 좁은 마스크 표본을 구분합니다. 기존 풀 생성 높이의 sizeDelta 사용은 이번에 바꾸지 않았습니다.

## 초기 카드 풀의 실제 높이 (2026-10-18, #116)

후속 #116에서는 초기 높이만 부모와 실제 마스크의 rect.height 중 작은 값에서 top padding을 뺀 값으로 바꿨습니다. stretch 배치의 sizeDelta.y=-160은 표시 높이가 아니므로 초기 풀을 8개로 과소 생성했습니다. 같은 실제 프리팹·CanvasScaler·12개 합성 목록의 1024×768 렌더에서 풀은16개, 1920×1080/2400×1080에서는12개가 생성됐으며 세 화면 모두 보이는 행 누락이 없었습니다. 최초 높이0은 최신 snapshot을 보관한 채 대기하고 복구 후 표시합니다. 기존 생성 후 풀 성장·SafeArea·실기기·실제 SDK 세션 검증을 추가한 것은 아닙니다.

## 대기실 동적 표시와 입력 글꼴 (2026-10-18, #115)

RoomPlayer의 이름/READY와 Message, CanvasRoom·CanvasLobby의 TMP_InputField global/text/placeholder는 같은 MenuFont와 한국어 fallback을 사용합니다. 이름은18–30 자동 크기/NoWrap/Ellipsis이며 오른쪽170 UI 단위를 예약합니다. READY는150×44의 오른쪽 중앙 배지,18–28 글꼴, 금색 배경과 어두운 글자입니다. 닉네임 부모의 회색 배경을 투명하게 하고 기존15 UI 단위 안쪽 여백을 유지합니다. 기존 팀 번호0/1과 READY 활성 상태·참조는 유지합니다.

RoomPlayerUI·CanvasRoom은 single-line 표시 사본만 만들고 ChatManager는 실제 줄바꿈을 유지합니다. richText/parseCtrlCharacters를 false로 적용하므로 `<b>`/`<size>`와 literal backslash 제어 문자가 레이아웃 명령이 되지 않습니다. 이는 네트워크 원문 제한을 바꾸거나 모든 유니코드 글리프를 지원한다는 뜻이 아닙니다. 저장 프리팹의 보호 필드·persistent ID와 Fusion의 숨겨진 importer 데이터도 구분해 검사했습니다. 1024×768/1920×1080/2400×1080의 실제 프리팹 렌더312검사·기존 로비 실제 풀 AddMessage31검사는 README의 범위와 한계를 따릅니다.

## 로비 방 카드의 일반 텍스트 표시 (2026-10-18, #118)

AD.Room.SetRoom은 재사용할 때마다 방 이름·맵의 richText/parseCtrlCharacters를 false로 적용합니다. 실제 CR/LF/tab만 UI 사본에서 공백으로 바꾸며 원문·SessionInfo 참조·MapName 유효성·세션 인덱스·인원과 JoinRoom 본문을 보존합니다. null/알 수 없는 맵의 기존 안내도 유지합니다. 프리팹이나 네트워크 입력 규칙을 바꾸지 않습니다.

실제 저장된 로비 프리팹의 풀 카드에10가지 합성 사례를 적용해307검사와3해상도 PNG9장을 확인했습니다. 기존2668개 목록 검사는 변경하지 않은 하니스로 재실행했습니다. 합성 setter 표시와 실제 참가·SDK 세션 수신·기기 검증은 구분합니다.

## 경기 결과·점수판의 열 제목 (2026-10-21, #148)

공백으로 정렬하던 단일 Columns 텍스트를 PLAYER/K / D/STATUS의 독립 제목으로 나눕니다. 기존 Columns2개는 PLAYER로 재사용하고 나머지 장식 제목4개만 추가합니다. 제목은 기존 행의 이름380·KD120·상태85 폭과 동일한 X/정렬을 사용하고 raycast는0입니다. 기존32행/96개 TMP·RectTransform과 MatchHudView의 배열/네트워크 데이터·입력·버튼은 보존합니다.

1024×768·1920×1080·2560×1080의 Results/Scoreboard와1920×1080의 UI최대32행 합성 표본, 총7렌더/672열 좌표 비교의 최대 오차는0px입니다. 긴 이름의 기존 생략·literal 태그/백슬래시 표시·LEFT와 여러 자리 KD를 확인했습니다. 합성32행은 실제32인 네트워크 경기가 아니며, 실제 새 Windows Player·Android 포함/배포는 별도입니다. 기존 결과 패널의 넓은 화면 테두리 여유는 #150으로 분리합니다. 근거는 외부 `20261021-result-columns-01/`입니다.

## 초광폭 결과 패널의 여백 (2026-10-21, #150)

ResultsPanel의 높이를940에서900으로 줄이고 결과 제목·사유·점수의 Y좌표만20씩 내렸습니다. 중앙 anchor/pivot·가로폭·폰트·32행 표와 복귀 버튼·점수판·HUD/네트워크 바인딩은 그대로입니다. 상단 장식선은 기존 top anchor를 따라가며 새 객체는 추가하지 않았습니다. 기존1097개 Scene ID를 보존하고 의미 변경은 RectTransform4필드뿐입니다.

1024×768·1920×1080·2560×1080의8/32행 Results6개와8행 Scoreboard3개, 초광폭32행의 패배/무승부/취소3개를 합쳐12개 격리 렌더를 확인했습니다. 전체1152개 Rect 열 좌표 비교의 최대 오차는0px이며 비활성 행도 포함합니다. 결과의 상하 여유는 초광폭에서 약20.4px, 제목/사유/점수·마지막 행/복귀 버튼은 겹치지 않았고 점수판 PNG3개는 수정 전과 바이트까지 같습니다. 이는 현재 full safe-area의 합성 UI 검사이며32인 경기·실제 notch/32:9·새 Windows/Android 배포 통과가 아닙니다.

미리보기 글리프7개가 메뉴용 dynamic CJK에 저장된 첫 전체 입력 감사 실패는 보존했습니다. 해당 폰트만 초기화·개별 저장 후 표현 차이만 확인해 이번 원본 바이트로 복원하고 동기 import했습니다. 이 문서 추가 전의 전체 공개3927입력 감사에서 의도한 Scene만 달랐고 원본 폰트·기존 로컬 설정5개는 그대로였습니다. 이후 변경은 이 기록뿐이며 전체 폰트 메모리 객체의 dirty0을 뜻하지 않습니다. 근거는 외부 `20261021-results-panel-fit-01/`입니다.

## 채팅 전송과 시작 조건 안내 (2026-10-22, #163)

방 채팅은 포커스 해제 대신 Enter/모바일 Done의 Submit 또는 새 SEND 버튼으로 전송합니다. 기본 안내는 `Type a message...`이며 입력과 안내를 왼쪽 정렬했습니다. 빈 입력·160 UTF-16 단위 초과·내부 제어 문자·깨진 surrogate는 보내지 않고, 연결 복구 중에는 초안을 보존하고 안내합니다. 메시지는 기존과 같이 태그를 해석하지 않습니다.

방장의 START 조건이 부족하면 기존 인원·양 팀·닉네임·READY 검사와 같은 경로에서 구체적인 팝업을 표시합니다. 조건 부족 상태에서는 다시 누를 수 있고 실제 경기 전환 중에만 잠깁니다. 권한과 중복 경기 전환 보호는 유지합니다.

Unity 컴파일 오류0/경고0, 실제 Photon 호스트 한 명의 방에서 인원 부족 팝업·반복 START·blur 초안 보존·Submit/SEND 콜백의 단일 전송·빈 입력 무전송과 정규화8사례를 확인했습니다. 실제 프리팹을1920×1080/2400×1080/1280×720에서 합성 렌더해 겹침과 텍스트 넘침을 검사했습니다. 실제 키보드/IME·Android Done/터치·원격 채팅과 양 팀 READY 성공 경기는 별도이며 새 APK 포함 검증은 아닙니다. 검증 자료는 외부 `20261022-quality/`입니다.

## 전투 HUD와 조작 피드백 (2026-10-22, #166)

전투 HUD의 FIRE·RELOAD·SPRINT·조이스틱과 HP·탄약·MENU를 로비와 같은 남색·청록색·호박색으로 정리했습니다. FIRE 입력 영역은 140×140에서 168×168로 넓히고 RELOAD·SPRINT는 140×140을 유지했습니다. 버튼을 누르면 장식 부분만 0.94배로 줄고 테두리 색이 바뀝니다. `CombatControlFeedback`는 기존 포인터 소유 상태를 읽어 시각 요소만 갱신하며 입력·탄약·네트워크 상태를 쓰지 않습니다. 기존 `ImgCrossHair`의 중앙 좌표와 sprite 참조를 유지하면서 흰 막대·짙은 외곽선·청록색 점으로 조준 표시를 보완했습니다. 별도 HitMarker의 바인딩과 명중 표시 구현은 보존했으며 반동 확산 기능은 추가하지 않았습니다.

저장된 DesertHouse 씬의 독립 직렬화 감사에서 원본 1,097개 레코드는 삭제 없이 남았고 변경 50개·신규 148개는 `UIManager/CanvasUI` 아래에 한정됐습니다. UI 밖 199개 레코드와 기존 HUD·조이스틱·포인터·이벤트·Button 연결 레코드 18개는 원시 바이트가 동일합니다. 새 Graphic 34개는 모두 raycast 비차단입니다. 버튼 위치와 FIRE 입력 영역은 의도적으로 바뀌었으므로 입력 기하 전체가 불변이라는 뜻은 아닙니다. 최종 명시적 저장은 해당 씬의 SaveScene 1회이며 전역 저장은 하지 않았습니다. 근거는 외부 `20261022-quality/hud-apply-01/independent-source-review.md`와 대응 JSON·record diff입니다.

격리 Canvas 렌더는 1280×720·1920×1080·2340×1080·2560×1080의 4해상도, 3화면비에서 확인했습니다. 최종 표본은 `HP 100`, `30 / 30`이며 활성 TMP의 overflow는 모두 false입니다. 2340×1080에만 좌우 3.5%·상하 2.5%의 합성 안전 여백을 적용했습니다. 이는 실제 OS safe area나 노치 검증이 아닙니다. 실제 경기의 3040×1440 Editor ScreenCapture는 `hud-runtime-02/actual-screen-touch.png`에 보존했습니다. 화면의 `EDITOR TOUCH PREVIEW`는 Editor에서 터치 UI 표시를 켠 상태이며 실제 기기 터치를 뜻하지 않습니다. 격리 렌더 근거는 `hud-render-03/`입니다.

실제 Photon 2피어 경기의 로컬 호스트와 EventSystem 1개에서 수행한 `hud-runtime-03/state.json`은 통과했습니다. FIRE·RELOAD·SPRINT·MovementArea 중심 raycast가 원래 입력 대상에 도달했고, 첫 포인터 소유·두 번째 포인터 거절·다른 포인터의 Up 무시·소유자의 Up/Cancel 해제, 눌림/해제 색과 버튼 장식 크기, 기존 조이스틱 이벤트 연결을 확인했습니다. 메뉴 열기에서 네 조작의 캡처가 해제되고 메뉴 중 입력 차단·Resume 복귀도 확인했습니다. RELOAD의 Up/Cancel은 기존처럼 대기 중인 재장전 요청을 남기며, 검사 종료의 명시적 입력 초기화로 정리했습니다.

위 검사는 실제 런타임 객체에 합성 포인터 콜백과 조이스틱 ExecuteEvents를 전달한 검사입니다. 눌림 시각 확인도 단일 동기 콜백 안에서 private Update를 140회 반복한 것이므로 실제 프레임 응답 시간·물리 터치·네이티브 입력 모듈 전달·기기 멀티터치의 통과 근거가 아닙니다. 같은 콜백 전후 보호 객체 25개와 RectTransform 35개의 차이 0, 검사 대상 소스 6개 일치, 탄약 30→30·ShotSequence 0→0·Fusion tick 21774→21774를 확인했으며 발사·이동·재장전의 시뮬레이션 성공을 시험한 것은 아닙니다. 검사 중 씬 저장·콜라이더/시뮬레이션 Transform/네트워크 상태 쓰기는 0입니다. 임시 터치 미리보기는 별도 복구 호출로 원래 false로 되돌렸습니다.

최초 `hud-runtime-01`은 QA 설정에 남은 닉네임 팝업이 첫 raycast를 받아 소유권 검사 전에 중단됐습니다. 팝업 처리 뒤 `02`도 즉시 검사에서 raycast 결과가 비어 중단됐으며, 터치 미리보기를 먼저 활성화하고 프레임이 지난 뒤 실행한 `03`에서 통과했습니다. 두 실패와 정리 결과를 보존하며 최종 통과에 합산하지 않습니다. 렌더 최초 표본의 잘못된 HP/탄약 fixture와 이전 FIRE 라벨 overflow도 각 이전 자료에 남겼습니다. 이번 HUD는 기존 Play/Drive APK27과 앞서 생성한 AAB28에 미포함이며, 추가 APK 생성·Android 포함 검증은 수행하지 않았습니다.


씬 저장으로 자동 반영된 메뉴 CJK 글리프와 기존 PlayerSettings alias 변경은 외부 보존 후 Editor 정상 종료 요청·프로세스/Temp 잠금 부재를 확인하고 CAS로 원래 바이트를 복원했습니다. 원래 로컬 설정 5개는 초기 SHA와 모두 일치하고 공개 입력 3,945→3,947개에서 의도한 씬·새 C#·meta 외 변경이 없는 것을 확인했습니다. 이 문단 전 감사이며 이후 문서 변경은 별도입니다. 종료 handle 보고의 ExitCode가 null이라 최초 엄격 복원은 쓰기 전 중단됐고, 새 프로세스 부재 근거로 후속 복원했습니다. ExitCode0·native 종료 안정화 통과로 합산하지 않습니다.
