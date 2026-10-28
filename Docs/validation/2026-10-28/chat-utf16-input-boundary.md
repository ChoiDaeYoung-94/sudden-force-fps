# 채팅 160 UTF-16 경계의 surrogate 보존 (#289, Refs #163)

방 채팅의 기존160단위 hard cap은 그대로 유지합니다. ASCII159개 뒤에 😀를 붙이면 TMP가 high surrogate만160번째에 남기던 경계를 수정합니다.
실제 `TMP_InputField.Append(string)`은 UTF-16 char를 순서대로 넣고, `Insert`는 매 char의 삽입 전에 현재 길이를 검사합니다.

`ChatManager`는 기존 validator가 없는 현재 입력 정책에서만 자기 instance validator를 연결합니다.
선택 영역을 제거한 문자열에 두 단위 공간이 있을 때 high를 허용하고, low는 삽입 위치 직전 high와 짝을 이룰 때 허용합니다.
BMP·한글/자모 입력과 기존 Submit160단위 검사는 유지하며, 종료 시 자기 delegate만 제거해 교체된 handler를 보존합니다.

P11의 batchmode/Graphics Null·clean empty scene1/root0/preview0/EventSystem 부재에서 실제 방 프리팹 clone을 사용했습니다.
같은 PID41448에서 원본·수정 후의 새 Game MVID를 각각 고정했으며 TMP MVID는 같았습니다. 제품 입력은 실제 TMP protected Append(string)으로 합성했습니다.

| 실제 입력 경계 | 원본 | 수정 후 |
|---|---|---|
|159 ASCII + 😀|고립 high/160단위·invalid|기존159단위 prefix 보존·valid|
|158 ASCII + 😀|정상 pair/160단위|동일|
|ASCII160 + ASCII1|기존160단위 유지|동일|
|전체 선택 양방향 + 😀|정상 pair2단위|동일|
|full160 중2단위 선택 + 😀|정상160단위|동일|
|full160 중1단위 선택 + 😀|고립 high/160단위·invalid|원초안160·공개 선택 보존|
|정상 중간 Unicode·한글/자모 BMP|각 합성 Append 예상값|동일|

두 실행은 각각9행·기존 delegate/validation·후교체/체인 정리4역할·clone5개 load/unload5개·errors0입니다.
원본의 complete는 위 두 invalid 행을 포함한 결함 재현 완료이며 기능 통과가 아닙니다. 수정 후9행은 예상 본문·valid pair·공개 선택 정합이 모두 일치했습니다.
각 Append 직후 UTF-16 escape와 원 char의 수동 LE 해시를 담은 관측9개를 먼저 저장해 후속 mesh 검사와 구분했습니다.
실제 textInfo의 index/stringLength와 공개 string/caret 위치를 대조했으며 ForceLabelUpdate 호출 사실만으로 통과 처리하지 않았습니다.

실제 C# 재컴파일은 completed/compilationFailed=false였고, 양쪽 CompileOnly·Disabled·본문은 diagnostics0입니다.
소스/fixture 정적 독립 검토는 완료했으며, 한정된 실제 전후 결과를 독립 COMMENT로 검토했습니다. 정확한 커밋의 PR 최종 검토와 최신 병합 조건은 별도입니다.

P11 최초3984개 기준에서 의도한 ChatManager.cs1개만 달라 나머지3983개 바이트가 일치했습니다. 수정 후 새 baseline 대비 종료 뒤3984개가 일치했습니다.
P2/P11의 C#2개는 raw exact였고 CanvasRoom.prefab은 줄바꿈 정규화 후 같았습니다. 최초 binder01의 raw LF/CRLF 차이 차단은 출력 전 실패로 보존했습니다.
baseline/after 소비 binding과 marker를 따로 보존한 뒤 active binding은 원래 false template로 되돌렸습니다. 기존 후보/실패/비활성 자료를 삭제하지 않았습니다.
동일 held process handle의 정상 종료는03:06:24.812210Z에 ExitCode0이었고 강제 종료·저장 요청은 없었습니다.
이 바이트 감사는 공유 CJK font atlas/cache·전체 메모리·모든 dirty 상태 불변의 증명이 아닙니다. 감사 대상 공개 font 파일의 새 바이트 변화는 없었습니다.

이 fixture는 SubmitChat/TryNormalizeMessage/RPC를 호출하지 않았습니다. native IME·실제 키보드/클립보드·원격 수신·엔진 자동 Awake는 미검증입니다.
현재 Play 코드34/release12에는 이 채팅 수정이 미포함입니다. #289의 좁은 입력 경계와 #163 전체 채팅 사용성 수용을 구분합니다.

외부 근거: `D:/codex/validation/sudden-force-fps/20261028-work/chat-input-bounded-fixture-candidate02/`의 두 actual 결과와 Append 관측입니다.
원본 `actual-6d6ebc6e2e8e4101be3c51bab2102d84/result.public.json` SHA256: `C3C2B0160FF80B5F99E22085A9C47A3687A16E26768FDC8C0FE23E412045EA11`.
수정 후 `actual-f8054927684740329188f5c2193a1971/result.public.json` SHA256: `4CBF3254BD7B4FA9AAFCCE167CF1184A8FFA09517D6CCE4304A2601FDCCC216B`.
종료/파일 감사는 같은 상위 폴더의 `p11-chat-normal-close01.public.json`·`p11-chat-after-close01.public.json`입니다.
실제 독립 검토 `chat-input-bounded-actual-independent-review01/verification.public.json` SHA256: `0D31058F022659208C4652162FCCDF8A9A6CEB575175C190058866A97E380849`.
