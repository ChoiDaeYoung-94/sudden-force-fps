# Android 조준점의 안전 영역 중앙 이동 · #401

QA9 소스 `d8abd32fee3e6a1357c8c9d22085ba9ecfdca556`의 실제 Android에서, 소유 카메라와 조준점의 같은 렌더 프레임 관측 8개로 오른쪽 50px 이동 원인을 확인했습니다. 이전 사진만의 추정을 실제 값과 대조했습니다.

| 관측 | 8개 행에서 동일한 값 |
| --- | --- |
| 화면/카메라 pixelRect | 2340×1080 / `(0,0,2340,1080)` |
| Screen.safeArea·캐시·조준점 부모 영역 | `(100,0,2240,1080)` |
| 조준점 screen centre | `(1220,540)` |
| 카메라 pixel centre | `(1170,540)` |
| 조준점−카메라 중심 | `(+50,0)`px |
| 프레임 / 샘플 | 6805,6814,6825,6836,6847,6858,6869,6880 / 1..8 |

조준점은 중앙 anchor와 위치0이지만 `CanvasUISafeArea`의 자식이었습니다. 왼쪽100px을 제외한 부모의 중심은1220px이고 전체 카메라의 중심은1170px입니다. 캐시와 실제 safeArea가 같아 초기 캐시 미반영으로 발생한 차이는 아닙니다. 위 값은 발사 tick 정렬이나 실제 피격 수용을 뜻하지 않습니다.

이번 수정은 `DesertHouse.unity`의 `ImgCrossHair`만 `CanvasUI`의 직접 자식으로 옮깁니다. 부모의 children 두 목록과 조준점의 father만 바꾸며 anchor·위치·크기·색·카메라·사격 코드는 유지합니다. 조준점을 SafeArea보다 앞에 그려 메뉴·사망·결과 UI가 위에 표시되는 순서를 유지합니다. 버튼과 상태 UI의 안전 영역은 유지합니다. SafeArea 전체 활성/비활성을 조준점이 더 이상 상속하지 않는 점은 후속 런타임 검수에 포함합니다.

별도 독립 소스 검토에서 Unity YAML1302개 문서 중 지정 RectTransform3개 외1299개 원형 일치, UI289노드 연결·중복·순환과 reticle의 비상호작용 설정을 확인했습니다. **현재 새 수정 APK·회전/다른 화면비·실제 발사 후 정렬 검증은 미완료**이며 Draft를 유지합니다.

원 strict capture는 Rect에 추가된 `serializedVersion` 필드를 거부했습니다. 숫자2로 잘못 가정한 reader02도 실제 입력에서 거부됐고, 실제32개 마커의 타입이 문자열 `"2"`임을 확인한 reader03을 독립 검토 후 같은 보존 입력에 적용했습니다. 정확한5개 Rect 필드·문자열2만 허용하며 다른30필드/벡터·부분 개수·바이트·SHA·run/frame/sample 검사 범위는 유지했습니다. 원 실패를 성공으로 수정하지 않았습니다.

공개 근거:

- APK9 `100,504,656B`, SHA256 `48CEDA394BBB84247D4B6B1604A41990436EF7AF7F06035F21638317A895228B`.
- run `0d82a481e9f34951b81d11c2a45cd45e`,32개 multipart, 최대783B. reader03 `7999B/646B08D4B1263C4C46772F6F76329C3F002BCB44BE71E11E84131D8ECDE95918`.
- 실제8행 결과 `21171B/4D5F2B6C70842EAD44F7807AFEB4F94E22CBB07CDC8DC213FF93661DD0F4672C`, 독립 수용 `70E646C3AE4BFD801ED60C62B7F58A1FF511C1948245DCA4C066068509252840`.
- 수정 Scene `1209231B/1608029B46D86B1B8E37AB0507BB5762D04A441B1D1F7D847D1A666B9EC30B2F`, 독립 소스 검토 `515C40625B5A6C515D59D62FFB2F27DA69AA560EE7FA5C80B9E4B4EF3CBF19A4`.
- 로컬 상세: `D:/codex/validation/sudden-force-fps/20261117-resume/qa9-geometry-reader03/`, `crosshair-minimal-candidate01/`, `qa9-phone-run01/`, `qa9-gui-return01/`.

정상 START 직후 사전 컴파일한 Editor RAM 타이머를10초로 줄여 양쪽 DRAW/TIME EXPIRED와 RETURN을 확인했습니다. 일반 APK에는 시간 변경 옵션을 추가하지 않았습니다. SDK·Editor·폰은 각각 원래 기한 안에 반환했고 Store34/release12·원장196,2,34·키·CI/CD 보류는 그대로입니다.
