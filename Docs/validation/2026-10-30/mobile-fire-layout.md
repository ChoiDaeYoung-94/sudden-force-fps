# 모바일 FIRE 중심 버튼 배치 (#303)

2026-10-30, 변경 전 소스 `153a775c2b218cc666c10c4f680b11ad69cd1059`의 P2에서 검증했습니다.
사용자 참고 이미지에 맞춰 오른쪽 아래 FIRE를 가장 크게 하고 보조 버튼 세 개를 주변에 배치했습니다.

## 제품 변경

`Assets/Scenes/Game/DesertHouse.unity`의 RectTransform 8개 속성만 바꿨습니다.
위치·크기 단위는 기존 1920×1080 기준 Canvas 단위입니다.

| 대상 | 변경 전 | 변경 후 |
| --- | --- | --- |
| FIRE 위치 / 크기 | (-120, 286) / 168×168 | (-160, 150) / 248×248 |
| RELOAD 위치 | (-292, 110) | (-390, 125) |
| SPRINT 위치 | (-120, 110) | (-175, 362) |
| CROUCH 위치 | (-292, 286) | (-390, 305) |
| FIRE ControlFace / AccentRing 크기 | 각각 156×156 | 각각 232×232 |
| KillFeed 위치 | (-410, -370) | (-410, -330) |

보조 버튼은 140×140을 유지합니다. 킬피드는 하단 FeedGap까지 보조 버튼과 떨어지도록 위로 옮겼습니다.
씬 diff는 8개 속성의 8줄 교체이며 기존 앵커·피벗·버튼 기능·컴포넌트·이벤트 연결·조이스틱·입력 코드는 같습니다.

## 실제 PC 검증

- Inspector helper로 실제 씬 GUID·로컬 ID·계층·기존 값·CanvasScaler·레이캐스트 구조를 확인한 뒤 8개 setter와 `SaveScene` 1회를 실행했습니다. `SaveProject`/`SaveAssets` 호출은 0입니다.
- 변경 전 6개, 변경 후 6개의 격리된 Edit Mode preview 렌더를 완료했습니다. 해상도는 1280×720, 1920×1080, 2400×1080, 2560×1080, 2560×1080의 좌우96/상하24픽셀 가상 안전 여백, 1440×1080입니다.
- 양쪽 모두 6/6 렌더·cleanup·레이아웃 검사가 통과했습니다. 버튼 4개 경계·상호 겹침·루트 레이캐스트와 장식의 비레이캐스트, MENU 및 활성 킬피드 TMP 13개(4행+FeedGap)의 경계·상호 겹침을 검사했습니다.
- 기존 맵 이미지 위에 합성 경기 HUD를 표시했습니다. 실제 경기·실기기 안전 영역·포인터 입력 시험이 아닙니다. 1920×1080 전후와 2560×1080 안전 여백 결과를 실제 이미지로 확인했습니다.
- 모든 preview를 닫고 원래 열린 씬의 path/handle/dirty/root 서명, 렌더 입력 8개와 설정 원바이트 보존을 각 case에서 검사했습니다. 동적 폰트·엔진 캐시·전체 메모리 불변을 증명한 것은 아닙니다.

저장 과정에서 폰트와 ProjectSettings가 자동 직렬화됐습니다. 생성본을 외부에 보존한 뒤 Editor가 종료된 상태에서 정확한 실행 전 SHA와 일치하는 기존 공개 원본으로 복구했습니다. 처음의 단일 숫자 역변환 가설은 SHA 불일치로 쓰기 전에 중단됐고, 이후 보관 원본과의 실제 일치를 확인해 복구했습니다. 문서 추가 전 최종 공개 4,011개 감사에서 의도한 씬 1개 외 4,010개가 실행 전 원바이트와 같고 추가·삭제는 0입니다. 기존 dirty 설정 5개를 보존했습니다. 총괄의 공개 파일 감사는 private 6개를 제외하며 해당 내용 조회·해시를 요청하지 않았습니다. Editor의 일반 가져오기·직렬화 콜백은 억제하지 않았습니다.

정상 close 뒤 보유한 native process handle로 Editor PID5616의 wait signaled/hasExited/정수 ExitCode0을 확인했고, native Editor0·unknown0·TempLock 없음으로 공유 자원을 반환했습니다. 최초 numeric ticks 직렬화의 정밀도 손실 때문에 잘못 만든 binding은 START_INSTANT_MISMATCH로 중단한 기록을 보존했습니다. 새 안전한 native 시작 시각 문자열 바인딩으로 확인한 종료 결과이며, 최초 실패를 성공으로 바꾸지 않았습니다.

## 근거와 남은 범위

외부 근거 루트: `D:/codex/validation/sudden-force-fps/20261030-work/`

- `ui-layout-actual01/`: 실제 inspect/apply/전후 렌더 호출·close·시작 시각 및 두 binding
- `mobile-ui-render-candidate02/renders/`: 전후 PNG 각 6개와 case/summary JSON
- `native-exit-p2-candidate01/p2-ui-layout01.public.json`, `p2-ui-layout02.public.json`: 첫 차단과 후속 보유 핸들 종료 결과
- `ui-auto-change-preservation01/`, `ui-auto-change-preservation02/`: 자동 생성본·첫 사전 차단·원바이트 복구 기록
- `p2-ui-after-recovery01.public.json`: 최종 공개 입력 4,011개 감사

이 변경은 PC에서 검토한 UI 배치 소스 반영입니다. 제공 중인 코드34 APK/Play release12에는 포함되지 않았고, 새35 예약·Android 빌드·서명·업로드는 0입니다. 실제 휴대전화 사용성은 후속입니다. PR261의 사격 중 같은 손가락 시점 이동·사망/리스폰·OS 전이는 별도이며 Draft PR261/#259를 완료 처리하지 않습니다.
