# 메뉴 미리보기 이미지

`Assets/Art/UI/MapPreviews/DesertHouse.png`는 외부 사진이나 AI 그림이 아니라 제품의 실제 `Assets/Scenes/Game/DesertHouse.unity`를 Unity 6000.3.25f1/URP에서 촬영한 이미지입니다. 원본 씬·모델·재질·조명은 변경하지 않았습니다.

- 카메라 위치 `(8,13,-11)`, 시선 대상 `(0,2,3)`, 수직 FOV52도,16:9, near0.1/far250, HDR 및 FXAA.
- 출력1536×864 PNG, 원본 이미지 SHA256 `CBDD3C507715D9B5045D3EBFA94037E7DE58D6AD72B2FFBB980F5AE8BEE2CEB3`.
- 촬영 시 씬 SHA256 `1556B5689150A9845964A421A037022718365E042F21E57D0B34E657028C6CB1`.
- `Maps` 아래 실제 Renderer123개의 bounds는 크기 `(20.36417,10.529734,30.36417)`입니다. 외벽만 보이던 초기 구도는 채택하지 않았습니다.
- UI Sprite 단일 모드, 최대1024, Bilinear/Clamp, mipmap·CPU readable 비활성, Android ASTC6×6. 로비/대기실 카드와 현재 맵에 같은 Sprite를 공유합니다.

원본 맵은 기존 JustCreate의 Low Poly FPS Map Lite 자산으로 구성되어 있으며, 취득·파일 대응 기록은 [에셋 목록](../../THIRD_PARTY_ASSETS.md)의 해당 항목을 따릅니다. 이 촬영으로 원본 자산의 권리를 새로 소유하거나 CC0로 전환한 것으로 보지 않습니다. #42의 전체 권리 검증 범위도 유지합니다.

맵의 화면 표시명 `Desert House`와 Fusion/씬 식별자 `DesertHouse`는 다릅니다. CanvasLobby는 내부 선택 키를 보관하고 생성 요청에 전달합니다. 표시 라벨을 내부 키로 다시 읽지 않습니다. 저장된 기존 버튼 인자와 호스트 전용 변경 흐름은 유지합니다.

2026-10-18 PC 검증은 실제 버튼의 합성 포인터 선택·상태 유지/초기화 및1920×1080,1280×720,2400×1080 격리 렌더를 포함합니다. 실제 Android 터치·새 AAB·최신 기기 경기는 별도 검증입니다.
