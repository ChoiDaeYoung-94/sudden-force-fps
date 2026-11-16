# Android 조준점 중앙 정렬 수정

비대칭 안전 영역이 있는 화면에서 조준점이 카메라 중앙보다 오른쪽으로
이동했습니다. 실제 2340×1080 폰에서 안전 영역은 `(100,0,2240,1080)`,
카메라는 전체 화면이었고, QA9의8개 프레임 모두 조준점 `(1220,540)`과
카메라 중심 `(1170,540)` 사이에 `(+50,0)` 차이가 있었습니다.

`ImgCrossHair`만 `CanvasUI`의 직접 자식으로 옮깁니다. 부모 연결과 양쪽
자식 목록3곳만 바꾸며, SafeArea보다 먼저 그려 기존 메뉴·결과 UI 순서를
유지합니다. anchor/offset·크기·색·raycast와 카메라·사격 코드는 바꾸지 않습니다.
버튼과 상태 UI는 안전 영역에 둡니다. 고정 픽셀 보정은 사용하지 않습니다.

## 실제 확인

- QA10/source `05f949116cd33512a622670eb059a90ab345f4a4`의 서명 APK를
  SM-A175N/API37/4KB 페이지 폰에 보존 업데이트했습니다. 기존 UID·최초 설치
  시각과 전체 설치 APK 해시가 유지/일치했습니다.
- 같은 화면과 안전 영역에서8개 프레임 모두 조준점과 카메라 중심이
  `(1170,540)`으로 일치하고 차이는 `(0,0)`이었습니다. 조준점 부모는 전체
  Canvas이며 SafeArea 조상과 캐시는 없는 상태로 관측됐습니다.
- START 전 RAM 타이머를 컴파일·비활성 확인한 뒤 정상 두 피어 경기에서
  10초 타이머 적용 → DRAW/TIME EXPIRED → 양쪽 Lobby 복귀와 타이머 객체0을
  확인했습니다. 결과 화면은 별도로 확인했습니다.
- 독립 검토는 이전/새 typed8행의 재판독·APK 해시·영수증 연결과+50→0을
  확인했습니다. 수집기는32part를 검증했지만 원시 multipart를 보존하지 않아
  독립 원시 SHA 재조립을 수행한 것으로 기록하지 않습니다.

이 PR의 Scene은 QA10에서 확인한 Scene과 원바이트가 같습니다.
main의 수정 전 Scene도 QA9의 수정 전 Scene과 원바이트가 같았습니다.
YAML1,302문서 중 지정3문서 외1,299문서와 UI289노드 연결을 독립 검토했습니다.
진단 코드·APK·서명 자료는 이 PR에 포함하지 않습니다.

## 한계와 후속

실제 사격 tick·회전/다른 화면비·메뉴/사망 경계·자연 collector 해제·독립
PID birth는 별도 검증입니다. #401은 남은 완료 조건 때문에 유지합니다.
제품43/QA 진단 브랜치 전체나 실제16KB·전체 권리·Play 배포 수용으로 확대하지 않습니다.

QA10 APK SHA256: `7B202F495C81BB21EE4D2F97960EF0115B193751E86CB4848C6164133CC169B4`

외부 근거 루트: `D:/codex/validation/sudden-force-fps/20261117-resume/`.
`qa10-geometry-actual-review01/completion.public.json`,
`qa10-phone-run01/full-return.public.json`, `qa10-gui-return01/full-return.public.json`,
`crosshair-minimal-candidate01/independent-review.public.json`을 참조합니다.
