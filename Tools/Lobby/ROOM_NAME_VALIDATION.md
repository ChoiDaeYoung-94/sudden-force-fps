# 방 이름 입력 검증 · 2026-10-23 (#194)

## 문제와 변경

source15196 Windows 개발 Player의 정상 GUI에서 입력한 방 이름 끝에 입력하지 않은 U+200B가 실제 Photon 세션 목록으로 전달됐습니다. TMP_InputField는 캐럿 처리를 위해 표시 라벨에 이 문자를 추가합니다. CanvasLobby.Confirm이 그 TMP_Text.text를 직접 사용하던 경로를 수정했습니다.

기존 serialized12필드·public9콜백·프리팹·map/capacity/visibility 옵션을 유지합니다. 기존 라벨의 ancestor TMP_InputField와 textComponent 소유 관계를 확인하고 input.text만 읽습니다. Trim 후 빈 값·64 UTF-16 한계·제어문자·잘못된 surrogate를 거부하며 입력 연결이 없으면 명시적으로 안내합니다. 입력자가 직접 넣은 Format 문자까지 일괄 삭제하는 정책은 추가하지 않았습니다.

## 실제 확인

검증 소스 CanvasLobby.cs의 SHA256은 `FF969241B3439593EAB2AC6A7B878FD60720E8E3BD78974F2D5E69509147A6F0`입니다. Unity6000.3.25f1의 P2 GUI에서 재컴파일 completed/failed=false/errors0을 확인했습니다.

| 구분 | 실제 확인 범위 |
| --- | --- |
| 정규화25개 | null/blank/trim·ASCII64/65·한글/combining·emoji32/33·control·불완전 surrogate 등 고정 기대값 |
| 실제 TMP5개 | 비활성 input 복제본의 SetTextWithoutNotify/ForceLabelUpdate가 만든 표시용 sentinel과 입력값 차이; suffix를 테스트에서 주입하지 않음 |
| helper read9개 | 정상5·empty·null label·input ancestor 없음·다른 textComponent 소유 관계 |
| 보존 | 총39행 passed/allMatched=true, 복제본 제거·원래 입력/label/selection/focus 상태·helper/request/source 핀 유지 |
| 실제 빈 Confirm | 정확한 안내 팝업·동일 Runner/기존 Lobby·작업 pending=false·원래 메시지 복원, 방 생성 없음 |
| 실제 정상 Confirm | 입력 `  SFF-QA-NAME194  ` → Photon 이름14 UTF-16 `SFF-QA-NAME194`와 Room 헤더 exact, U+200B 없음 |
| 다른 피어 | 별도 Windows Player의 목록 카드 수신·정상 GUI 참가 후 호스트 관측 인원1→2 |
| 복귀 | 정상 호스트 퇴장 뒤 Connected Lobby·닉네임 복구·게임/QA 객체0·clean Untitled roots2/save0 |

문서 기록 전 기존 공개 manifest3968개 감사에서 변경은 CanvasLobby.cs 하나뿐이고 기존 dirty5 바이트는 유지됐습니다. 전체 P2 경로 전수 탐색이나 메모리/캐시 무변화 증거는 아닙니다. 수정 범위와 staged diff 검사는 통과했으며 기존 EDM 파일의 공백 경고를 없애기 위해 사용자 변경을 수정하지 않았습니다.

## 근거와 한계

외부 자료 루트는 `D:/codex/validation/sudden-force-fps/20261023-work`입니다. `room-name-input-validation01/actual-01`, `room-name-setup-invalid194.public.json`, `room-name-network-observed194.public.json`, `room-name-twopeer-observed194.public.json`, `room-name-restored194.public.json`, `room-name-clean-untitled194.public.json`과 `room-name-actual-independent-review01`을 함께 읽습니다. 실제 clone/정상 생성 검증과 미호출 컴파일을 구분합니다.

clone은 readonly/비활성 상태의 검사이며 실제 IME·Android 키보드 전체 검증이 아닙니다. 실제 정상/빈 Confirm은 총괄이 별도로 합성 UI 콜백을 호출했습니다. 기존 Windows Player는 일반 GUI를 사용했고 새 Windows 바이너리 빌드는 하지 않았습니다. 일부 복구 helper는 시작 ticks 핀이 없으므로 연속 소유 Editor 관측 맥락에 한정합니다.

호스트 퇴장 시 client의 SDK104/LeaveRequest error2는 원본 그대로 보존하며 모든 런타임 오류0으로 합산하지 않습니다. Windows 정상 EXIT/OK 뒤 종료 대기는 #154의 별도 관측이며 이 수정으로 해결됐다고 주장하지 않습니다. private room 기능과 실기기·서명·Play/Drive 전달은 검증 범위 밖입니다.

코드30 AAB/source15196은 불변이며 이번 수정은 새31 이상 빌드에 포함해야 합니다. 현재 실제 제공은27/release8입니다.
