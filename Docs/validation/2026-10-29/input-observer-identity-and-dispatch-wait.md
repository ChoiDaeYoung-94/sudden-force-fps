# 입력 관측 시각과 dispatch 대기 종료의 실제 범위

2026-10-29의 실제04/05는 입력 검증을 준비하고 안전하게 종료한 부분 기록입니다. 실제05에서는 시각 전달용 새 runtime의 관측9개와 설정 발행까지 진행했으나, probe dispatcher가30초 대기 뒤 본문을 호출하지 않고 끝났습니다. pulse·enabled witness Arm·paired 발행·collector packet은0이며 PR261 Draft/#259 OPEN을 유지합니다.

제공 중인 게임은1.0.196/code34/source612 QA·내부 release12이고 권위원장은 `196,2,34`입니다. 최신 main과 같은 산출물로 보지 않으며 새35 예약·게임 빌드·서명·업로드는0입니다.

## 실제04: 같은 시각의 문자열 비교 차단

새 Editor69776/start `2026-10-29T02:13:06.6722200Z`에서 자연 witness 타입·GUID/동일 MVID와 compile/default-disabled20개를 확인했습니다. 정상 helper9개로 새 두 피어 경기와 readonly 관측1회를 진행했고, readonly는 handler32→32/해시와 자기 callback 부재를 복구했습니다.

이후 `bind-joystick06.py` line63이 InspectEditor의 `.67222Z`와 deferred 원문의 `.6722200Z`를 문자열로 비교해 차단됐습니다. UTC100ns tick은 정확히 같았으며 다른 line63 조건은 일치했습니다. 어느 직렬화 단계가 축약했는지는 확정하지 않았습니다. frozen config 생성·probe 본문·pulse·Arm·paired·packet은0입니다.

기존 actual JSON·alias·소스·marker를 고치거나 실패를 재시도하지 않았습니다. 정상 DRAW/결과 복귀 뒤 같은 held handle Exit0, 공개3,984개 보존·소유2파일 제거와 readonly 퇴역·FPS VM 종료를 확인했습니다. 실제 자원 반환은02:24:43.0083764Z입니다.

## 새 출력 처리와 실행 전 차단 보존

runtime09의 시각 정규화는 P11 eval과 정확히 핀된 관측기3개에만 적용합니다. generic 다른 project/open/recompile 분기는 이 검토로 승인하지 않았습니다. InspectEditor.processStartedUtc, InspectWitnessType01.startedUtc, InspectJoystickContext01.editorStartedUtc 중 해당하는 한 필드의 UTC-Z 소수초0~7자리를 오른쪽0 패딩으로7자리에 맞춥니다. 날짜 유효성·정수100ns 동일성을 검사하며 반올림·절삭·다른 결과 필드 변경은 하지 않습니다.

새 CLI stdout은 같은 phase의 private/quarantine에 CreateNew로 보존하고 공개 결과에는 원문 SHA/길이, 신원 전후 값·100ns, 규칙과 source 전후 SHA를 남깁니다. 알 수 없는 응답 metadata의 값과 raw 전체는 공개하지 않습니다. 이 보관 구분이 암호화·ACL 보장을 뜻하지는 않습니다.

첫 runtime08 후보는 정상 CLI command wrapper에 data.success=true를 추가로 요구하고 parameters를 거부하는 오류가 있어 실제 실행 전에 독립 검토에서 BLOCKED로 보존했습니다. 수정09는 CLI.success와 Evalinner.success를 검사하고 정상 wrapper/양 계층 parameters의 값을 공개 payload로 복사하지 않습니다. 명시 실패·오류/경고/진단·알 수 없는 result 모양은 거절합니다.

source09 독립 순수111검사와 핀35개, Root의 저장04 payload+가상 wrapper3개 검사는 source 범위입니다. 실제 beta14 원문 실행이나 새 경기 수용으로 합산하지 않습니다. stage/open/driver와 consumer·제품 소스의 핀은 각각 구분합니다.

## 실제05: 정상 전달과 본문 미호출

새 Editor2204/start `2026-10-29T02:50:41.0931393Z`에서 before/type/after와20개 비활성 경계를 새로 수집했습니다. P11은 detached `f055e4a84391bd9d8741c4e12855887b1ef72166`와 기존18개 QA overlay를 유지한 작업본입니다. 최신 일반 main의 clean checkout 또는 새 Windows/Android 빌드 검증은 아닙니다. 타입/GUID/MVID 관측·alias6 원바이트·정상 helper9·공개3,984개 보존을 확인했습니다. MVID `e94bd0ea-00f6-41d7-a474-e9eec784d802`는 재관측 값이며 새 모듈 생성의 증명은 아닙니다.

관측기 응답9개는 실제 ACCEPTED였습니다. 이번 raw의 신원은 이미7자리여서 변환 전후 값과 payload SHA가 같았습니다. 따라서 실제 짧은 소수초가 패딩되는 live 표본을 관측한 것은 아닙니다. 이전04의 실패, source 검사와 이번 실제 응답을 구분합니다. 독립 실제 검토자는 private/quarantine 원문에 접근하거나 해시하지 않았습니다.

정상 nickname/room/READY/START로 두 피어가 Game에 진입했습니다. readonly actual `a11b58d5a1ea4237a8661f1ea92e8a36`은 handler31→31/해시와 자기 callback 부재를 복구했습니다. callback 등록은 실제 있었으므로 모든 등록0이라고 표현하지 않습니다.

frozen probe config `f082ccfa6e9c4c24ac8a135091e0db21`과 probe request를 새로 만들고 false proposal 검토 뒤 D 설정1개를 실제 발행했습니다. 이 성공은 paired2개 발행이나 입력 소비의 성공이 아닙니다.

D actual `b5b84fadba5c4dccb54b6b4b47e8f135`은 약30.004초 뒤 `WAIT_OR_TTL_OR_LEASE_EXPIRED`로 끝났습니다. bodyInvoked=false, ownDispatchAbsent=true와 handler29→29/동일 해시 복구를 확인했습니다. allowed pulse0이며 F actual 디렉터리와 consumed marker는 없습니다. D의 복구 플래그를 미호출 F 본문의6복구 성공으로 확대하지 않습니다. 본문·pulse·enabled Arm·paired·packet은0이고 재시도하지 않았습니다.

대기 전 직접 관측은 focus=true/crouchReleaseRequired=false였고, 대기 뒤에는 false/true였습니다. Game 탭 클릭만 한 후에도 false/true였으며, 명시적인 정상 창 활성화→새 화면 확인→Game 탭 클릭 뒤에는 true/false로 복귀했습니다. HP100/탄약30/shot0과 관측기의 writes0을 확인했고 private flag를 직접 변경하지 않았습니다. 이 샘플은 대기 전후 관측으로, 전체30초 동안 어떤 Idle 조건이 실패했는지는 증명하지 못합니다.

## 종료·정리와 통과로 합산하지 않은 기록

정상5분 DRAW/TimeExpired 뒤 Android의 양팀0·DONE2행과 RETURN/public rooms→EXIT/OK/home은 Root의 화면 관측입니다. 별도 PNG0이며 독립 픽셀 검증으로 확대하지 않습니다. Host 결과 RETURN의 첫 관측은 Lobby/Connecting/list=false였고, 후속02에서 Connected/list=true/retired=false/witness0을 확인했습니다.

StopPlay→clean Untitled→framecap -1 뒤 Close eval은 exit6/COMMAND_FAILED로 실패 응답을 보존했습니다. 별도 같은 held handle은03:01:55.2126507Z에 ExitCode0을 기록했습니다.

Root가 held 종료 확인 전에 cleanup06 검사를 시작해 held-exit 파일 부재에서 멈췄습니다. 당시 first final guard는 native1/TempLock=false였고 제거0입니다. 그 guard/audit/실패를 보존했으며, 이를 post-exit 통과로 쓰지 않습니다. 실제 종료 뒤 새 final02 guard의 native0/unknown0/TempLock=false, final02 감사와 cleanup07로 소유2파일 제거/잔존0/untracked0을 확인했습니다. after-cleanup의 공개3,984개 path/bytes/SHA는 원 baseline과 같고 개인6개는 부재/indexS입니다. 보호된 개인6개 내용 조회·해시0이며 캐시·전체 메모리 원복의 증명은 아닙니다.

readonly와 D, 미호출 F의 승인 사본·false 퇴역 사본을 보존해 enabled/rootApproved 두 JSON bool만 의미상 true→false로 바꿨습니다. raw2바이트만 바뀌었다는 주장은 하지 않습니다. F는 actual/consumed 부재와 caller 본문 미호출을 확인한 뒤 퇴역 사본을 남기고 active leaf만 제거했습니다. W/C 초기 false와 모든 actual/consumed marker·실패 자료는 보존했습니다.

05 퇴역 직후의 D false baseline SHA는 `1AA1E7FA0C20AB982FFD4EDFA5E063FA6C77475DC4445B60E44F29AF02858EBA`이며 과거4956 baseline으로 되돌리지 않습니다. Q false SHA는 `1BBAFFDBA575DDEAB64D5B1E1A8671CD81C0E02DBA22F3B103DBE8E5DF502AF6`입니다. 실제 전체 자원 반환은03:04:46.2758487Z로 한도03:05:27.1879067Z보다 이릅니다.

비활성 FPS 창 capture1회는 FPS metadata와 다른 앱 화면이 불일치했습니다. 그 capture에 입력하지 않고 동일 FPS id/app을 재식별·활성화한 뒤 올바른 결과 화면을 확인했습니다. 다른 앱에 대한 UI 조작이나 내용의 추가 파일 저장·전재는0입니다. 반환된 이미지의 일반 도구 응답 보존까지 없었다고 주장하지 않습니다. 캡처 불일치의 원인은 미확정입니다.

## 다음 단계와 남은 수용

다음 새 회차에서는 실제 공용 반환·fresh 양측 동의·UTC-Z lease·새 PID와 전체20/epoch를 다시 수집합니다. Root 검토/발행 뒤 실행 직전에 정상 창 활성화와 Game 탭 선택을 하고 같은 orchestration 안에서 queue/terminal 관측을 이어가는 계획입니다. 이 계획이 성공이나 포커스 유지의 보장은 아니며 기존 gate/flag를 우회하지 않습니다. 기존 actual/alias/marker를 고쳐 성공으로 만들지 않습니다.

실제 packet 수집, 원래 NullReferenceException 해결, Android 사망/리스폰(L2), native 전체·PR261 전체 수용은 미검증입니다. 현재 L2 controller의 NativeDown과 Host 공격 연결·공통 시각 오차 상한도 준비 과제이며 실행0입니다. 실폰·청취·실제16KB·장시간 성능/발열은 합의대로 후순위입니다. #259/#166/#41/#43/#66 및 다른 미완료 부모를 이 기록으로 닫지 않습니다.

## 근거

외부 기준은 `D:/codex/validation/sudden-force-fps/20261029-work/`와 `20261028-work/resumed-1448/`입니다. private raw 전체를 공개 저장소로 옮기지 않았습니다.

| 근거 | 경로 | SHA256 |
|---|---|---|
|04 실제 독립 검토|`20261029-work/packet04-actual-partial-review01/verification.public.json`|`B37B6D2ECD7EBF128D2C3ED5B5816BFB60D6E1FDA71EE25064B8E70C0106065E`|
|runtime08 차단 검토|`20261029-work/packet-attempt05-preparation-source-review01/verification.public.json`|`D545606BC71DE886F56CD6DEA631788A313E2D8FD3ABBF7B6F45F1F243678957`|
|runtime09 source 검토|`20261029-work/packet-attempt05-preparation-source-review02/verification.public.json`|`407473D46F50B3F53ED991B8A02781F200316F5CF7F2B512E6105724D6B3755D`|
|05 실제 독립 검토|`20261029-work/packet05-actual-partial-review01/verification.public.json`|`8600E5906EA7DDCF7D10EC770D943A2C20F10B97193623CEBD42B6FE9D3DCA17`|
|05 실제 반환|`20261028-work/resumed-1448/simulation-witness-attempt05-20261029/actual-return.public.json`|`E14632CF61D4A70E453B93688E76072A70B73638FCB77A47345A3711DCA0709F`|
|05 최종 보존 감사|`20261029-work/p11-witness-after-packet05-cleanup.public.json`|`DE2FB6BD746DEB218907437D953516E6EC08AC13367AB6FF31994921643757CF`|