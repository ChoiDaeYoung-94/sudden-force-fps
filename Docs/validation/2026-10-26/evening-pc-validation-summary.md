# 저녁 PC 검증과 남은 작업 · 2026-10-26 (#262)

이 문서의 적용 준비 기준 main은 `e19cb55eb78cee238e5dd8582243f0c0dfd628bc`입니다. main의 제품8PR과 [PR261](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/261)의 FIRE/LOOK 후보 `f3ee5451fa24a22d419233da274d31984494276c`를 구분합니다. **PR261은 Draft·미병합이며 #259도 OPEN입니다.** #262는 실제 검증과 잔여 범위를 묶는 문서 작업입니다. 새 중복 Loading [#263](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/263)은 [PR264](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/264)의 좁은 PC 수정·독립 검토 뒤22:15:13 KST main `e19cb55`에 병합해 닫았습니다. 후보 head `910365c584d808ff922e3a8f2046cec2cb560b44`와 병합 main을 구분합니다. 최종 적용 시 main/head·검토·필수 조건은 총괄이 다시 확인합니다.

마지막 알려진 제공본은 **Play 코드33/1.0.196/release11**, 소비 원장은 `196,1,33` 그대로입니다. 이번 후속의 fresh Console 제공·최대코드 조회, 새 Android 예약·AAB/APK 생성·서명·Play 업로드·Drive 전달은0입니다. 다음 코드를 예약했다고 가정하지 않습니다.

## main·Windows03·Draft Windows04의 출처

| 구분 | 포함·출처 | 검증 범위 |
| --- | --- | --- |
| main30d5 제품 | PR239/240/241/243/245/247/253/255의8개 제품 변경 | 기존 Android33에 미포함. 문서·정책 PR과 구분 |
| Windows03 | f055 scratch·보존 형식10개에 UniRx/C4 MENU 정확2소스를 복사해 제품8개 내용을 대응 | clean-main checkout 빌드가 아님. 빌드/산출물 검토와 단독 Lobby 종료1회 Exit0/Crash0 |
| Windows04 | 같은 f055 scratch/형식10 이력에 UniRx/C4/PlayerInputSource/CombatPointerControl 정확4소스를 복사 | main 제품8개에 Draft PR261의2C#를 덧댄 실제 검증본. main 병합·Android 수용과 구분 |

형식10은 초기 scratch 분류 이력입니다. Windows04 공개 입력3,984개의 불변은 **해당 새 빌드 baseline과의 전후 비교**이며 canonical main과 모든 바이트가 같다는 뜻이 아닙니다. P2 보호6개·기존 dirty5와 원장을 보존했습니다. #263의 후속2C#는 이미 생성한 Windows04 binary에 포함되지 않습니다.

## Windows04 실제 빌드·산출물·compiled metadata

Windows64/Mono Development 빌드는 **Succeeded/오류0/경고2**입니다. helper57.1740496초와 BuildReport41.6547637초를 구분합니다. 실제 payload360개는278,712,107B, Report는278,514,560B이며 차이197,547B는 함께 보존한 Burst DoNotShip 텍스트1개입니다. 자체 manifest는 payload360개와 별도입니다. Windows 표시 버전1.0.194는 Android 제공 버전1.0.196과 구분합니다.

산출물 독립 검토에서360개 payload와 공개 입력3,984개를 실제 재해시해 불일치0, strict15의 빌드 후 크기/SHA 일치를 확인했습니다. private6 부재/indexS·generated5 부재·P2 보호6개 exact는 총괄 공개 metadata/전후 receipt 범위입니다. 원장 `196,1,33`은 불변입니다.120초 시작 창·단1회 callback·자기 해제·승인 소비false를 확인했으며 opaque SDK의 모든 부작용을 계측한 검증은 아닙니다.

실제 Game DLL의 SHA와 연결한 Cecil 검토에서 새 Capture/Release/Touch overload·guard API와 Control 호출, 중앙 Touch sampler의 AddLookDelta1회 호출 구조를 확인했습니다.03 reference와4type의 field declaration 배열도 같습니다. 이는 **compiled metadata 확인**이며 reader가 해당 DLL을 로드·실행하거나 native Touch/게임플레이를 검증한 것이 아닙니다.

경고2개는 Unity Dashboard 미연결과 RuntimePipelineConfig 부재로 인한 Pipeline 비활성 안내입니다. warnings01의 `OWN_REPORT03`은 명명 흔적이며 warnings02는 status 문자열1개만 `OWN_REPORT04`로 정정했습니다. 실제 guard는04 outputPath를 대상으로 했고 경고2개는 그대로입니다. 원본01을 보존했으며 경고 해결·억제가 아닙니다.

## Native Windows04 실제 두 피어 표본

총괄이 새 Windows04의 정상 마우스·키보드/UI 경로와 P10 호스트로 기본 경기1라운드를 확인했습니다. 실제 Native04 근거와 별도 독립 검토를 사용했으며 Windows03의 단독 종료 결과를 재사용하지 않았습니다.

| 항목 | 실제 확인 | 한계 |
| --- | --- | --- |
| Login/Lobby·방 진입 | Windows Login 우회→Lobby, 닉네임QAW04, 일반 방 목록 Join·BLUE READY·호스트 START | 인증/Store·물리 Android Touch 수용이 아님 |
| 사격·재장전·메뉴 | LMB1발로30→29, R 뒤30·authoritative shot1/reload1, Escape MENU·RESUME | 실제 청취·발사 지연·전체 연사/재장전 접촉 수용은 별도 |
| 짧은 이동 | W 입력 뒤 같은 authoritative actor 위치 변화·life0/HP100 유지 | 별도 정지/속도 측정0. 걷기·달리기·앉기 전체 수용이 아님 |
| 자연 경기 종료·복귀 | 5분 자연 시간만료의 DRAW/TIME EXPIRED·RED0/BLUE0, Native RETURN 후 Lobby 화면 | 두 피어 기본 표본이며 새 방 재참가·death/respawn 전수는 미실행 |
| 정상 프로세스 종료 | EXIT→OK, 사전 보유 process handle signaled·ExitCode0, 강제 종료false | 정확 종료 지연·모든 종료 안정화·#154 원인 해결이 아님 |

JPG10개는 독립 검토에서 실제 header/1282×752/bytes/SHA를 공개 metadata와 대조했습니다. 결과·Lobby·종료창·중복 Loading 원본4장은 직접 시각 검토했습니다. 같은 binary/프로세스 식별과 사전 보유 handle을 연결한 종료 표본이며 `Crash!!!` literal 집계0·LobbyConnected2·SessionList3은 총괄의 고정 비공개 로그 집계 범위입니다. 원시 로그·dump·로그 해시를 공개하지 않았습니다.

최종 OK 뒤 명시적인 창 상태 요청은0이며 backend 내부 UIA 동작을 독립 계측한 것은 아닙니다. 정확한 클릭 시각/종료 지연을 계산하지 않았습니다. 초기 CUA 화면 오인에서는 입력0을 보존하고 정확한 자체 앱 식별·활성화 뒤 진행했으며 도구 전역 오류를 수정했다고 판단하지 않습니다. **#154 Windows native crash 원인은 계속 미확정**입니다.

RETURN 도중 `RETURNING TO LOBBY`와 별도 Loading/spinner가 동시에 보인 실제 UI 결함을 #263으로 분리했습니다. 정확한 노출 지속시간은 측정하지 않았습니다. 아래 수정 후보는 이 Windows04 원본의 성공으로 소급하지 않습니다.

## 후속 Windows05 · Draft261 제외, #263 후보 포함

후속05는 f055 scratch·보존 형식10 이력에 main Source/Control을 복사하여 **Draft PR261의2C#를 제외**하고 #263/PR264 후보 `910365c584d808ff922e3a8f2046cec2cb560b44`를 포함한 별도 검증본입니다.04의 Draft FIRE/LOOK source와05의 source를 구분하며05도 canonical clean-main checkout 빌드가 아닙니다.

실제05 빌드는 Succeeded/오류0/경고2이며 BuildReport48.4012262초와 helper56.695821초를 구분합니다.360개 payload의 실제 합계278,712,139B·Report278,514,592B·차197,547B를 관측했습니다. 차이는 함께 보존한 Burst DoNotShip 텍스트1개이며 새 공개3,984개 baseline 전후 rawexact입니다. 이 수치를04의278,712,107B와 혼합하지 않습니다. 별도 산출물 독립 검토에서 실제360개와 공개3,984개·strict16·P2 보호6개를 직접 재해시하여 mismatch0을 확인했습니다. Core/CompileOnly·시작120초 창·단1회 job·자기 callback 해제·소비false·AutoRun0도 확인했습니다. private6/indexS·generated5 부재는 내용 읽기 없이 별도로 대조했습니다. 아래 실제05 Player 관측과 별도 Native05 독립 검토의 원본14장 고정 범위를 사용합니다. 복귀·재참가·종료 미실행은 양성 표본으로 합산하지 않습니다.

05 compiled metadata 검사 최초값은 `oldDirectReturningShowWindowExists`1개가 실패했습니다.8instruction 근접 휴리스틱이 다음 menu-root Show를 returning-root Show로 함께 센 오탐을 원본으로 보존했고, 정확한 field/Show 패턴 검토로 old direct returning 패턴1개→05의0개를 분리했습니다. 나머지17검사와 Popup getter/helper·HUD2메서드 추가, Draft261의3개 API 부재를 별도 확인했으며 **소스·binary 변경/빌드 재실행0**입니다. compiled metadata를 실제 화면 중복0이나 새 Android 수용으로 확대하지 않습니다.

## Native Windows05 · 같은 binary 두 Player의 기본 경기/DRAW

총괄이 같은 Windows05 binary의 자기 Host PID23604·Client PID21948로 방 `q26w05a`를 정상 UI에서 생성·목록 Join·READY·START했습니다. 호스트1명일 때 최소2명 안내와 미READY 안내를 확인했고 양쪽 실제 Running 화면을 보존했습니다. 원래 유일 창과 새 두 번째 창으로 역할을 대응했으며 창/PID mapping을 독립 조회한 것은 아닙니다. 한글 `W05 정상 복귀 확인`을 type_text로 입력한 뒤 native Return으로 제출하여 Client의 원격 메시지 표시와 입력 placeholder 복귀를 확인했습니다. Unicode 입력/Enter 표본이며 **IME composition·물리 키보드 입력 전수 시험이 아닙니다.**

호스트 LMB1발은 탄약30→29, R 재장전은 표시1.4초와 완료30을 확인했습니다. Client Escape의 MENU→RESUME를 확인했고 양쪽5분 자연 시간만료의 DRAW/TIME EXPIRED·0:0 결과 원본 JPG를 보존했습니다.300초 전패킷/전체 프레임 연속 관측은 아닙니다. 이동·Ctrl 앉기·피해·death/respawn·실제 청취·성능 수용은 미실행입니다. 실제 Android33나 Editor와의 조합이 아닌 같은 native binary 두 Player 표본입니다.

**결과 RETURN·새 방 재참가·EXIT는0입니다.** Sky의 Client 활성화가 실패했고 inventory에서 같은 두 창을 확인한 뒤 재바인딩/재시도1회도 같은 실패였습니다. 별도 CUA getApp의 AX 읽기는 성공했지만 노출된 Raise 역시 같은 활성화 실패여서 후속 입력을 진행하지 못했습니다. 이를 제품의 RETURN 실패나 OS/도구 전역 오류 해결로 판정하지 않습니다.

22:28 KST 총괄 공개 assessment 기준 두 native PID는 생존하며 ExitCode는 미확보입니다. 보유 handle monitor는30분 대기 뒤 timeout하더라도 kill하지 않는 범위입니다. 대기/timeout을 정상 종료나 cleanup 통과로 합산하지 않고 #154 원인도 미확정으로 유지합니다. Windows04의 별도 held-handle Exit0를05 종료 결과로 재사용하지 않습니다. 추가 연속 Native 시험을 강요하지 않고 사용자 필요 단계·질문은00시로 모읍니다.

일부 헤더 글자 누락이라는 초기 시각 추정은 총괄/보조의 해석이 달랐습니다. 원본3개 ROI에서 prefix65/65/65·K22/22/22를 대조했지만 실제 누락을 확정하지 못했습니다. **새 제품 결함 확정·신규 이슈·수정0**이며 기존 #132를 열린 조사로 유지합니다. 이를 cold TMP 원인 해결이나 실제 글자 표시 전체 통과로 확대하지 않습니다.

Native05의 위 실행은 총괄 실제 관측·원본 보존 범위입니다. 총괄은 실제14 JPG의 bytes/SHA와 공개 metadata·owner의 동일 EXE/Game DLL 대응을 대조했으며 문서 작성자는 해당 공개 영수증만 고정했습니다. 별도 Native05 독립 검토는 원본14장과 metadata·동일 DLL·56핀을 확인하고, 좁은 UI/결과 양성 표본만 인정했습니다. 후속15번째 사진과 미수락 RETURN 시도는 그14장 고정 검토 밖이며 여전히 정상 복귀·종료 근거가 아닙니다. 빌드05 산출물 독립 검토와도 구분합니다. 이후 독립 문서 검토도 실제 Player 종료·기기·픽셀 시험을 대신하지 않습니다.

후속 창 제어 연결을 초기화한 뒤에도 활성화가 실패했습니다. 자기 호스트의 추가 RETURN 입력 시도 후 읽기 전용 화면은 여전히 DRAW였으므로 정상 복귀를 확인하지 못했습니다. 저장 원본을 보존했고 강제 종료·앱 데이터 삭제는 하지 않았습니다. 이는 로딩 수정의 Editor 렌더 콜백41표본과 별도이며 native pixel 중복0의 근거로 사용하지 않습니다.

## 입력 QA와 R11의 완료 경계

| 검사 | 확인한 좁은 범위 | 미검증 경계 |
| --- | --- | --- |
| R9 합성38개 | Running 문맥의 양수 Fire pointer→중앙 Touch sampler·delta1회·Snapshot 반복·LOOK 선점/Up/Cancel/menu/reset·원복. 후속 필드 동일·shot0 | 로컬 합성 Touch/Pointer. 실제 UI Fire raycast·하드웨어 polling·원격 NetworkOnInput 전수와 구분 |
| R10 공개 Bind 순서 합성10개 | Unbind 거절/같은 owner Bind 복원·임시 Source Bind 뒤 구 Source 거절·구 Fire Up의 새 LOOK 보존. 필드21 exact·Source1→1/Control4→4/Unity14,901→14,901·후속 shot0 | native Source 교체·실제 사망/리스폰 전환 검증이 아님 |
| P10 현재 focusfalse guard | 실제 isFocused=false·CanProvideInput=true에서 capture=false·Source 필드 동일·shot0 | 현재 false 상태의 거절만 확인. 실제 포커스 상실/복귀 전환을 통과 처리하지 않음 |
| R11 정상 종료·복귀 | 정상5분 경기 종료 뒤 양쪽 정상 RETURN 요청, 별도 P3의 Lobby/Connected·새 Runner·닉네임QAC26 관측 | P3 복귀 관측을 두 피어 전체 복구 전수나 focus/death 수용으로 확대하지 않음 |

P3/P10 실제 읽기 전용 관측은 `mobilePlatform=false`, `WindowsEditor`, touchCount0입니다. Android target·Simulator 화면과 합성 Touch를 Android 물리 입력 수용으로 계산하지 않습니다. 최초 관측 초안의 존재하지 않는 `MatchHudView.PreviewMobileHud` 접근은 CS0117오류1개와 자동 tail의 CS0162경고1개를 냈습니다. V2 관측은 진단0이며 이 초안 실패를 제품 C# 컴파일 실패로 합산하지 않습니다.

R9/R10/R11과 Native04는 서로 다른 라운드입니다. 위48개는 로컬 합성 검사 수이며 물리 입력 성공 횟수가 아닙니다. 실제 focus 전환·death/respawn·native Source 교체·원격 native OnInput·물리 멀티터치 수용은 미실행으로 남겨 **PR261 Draft/#259 OPEN**을 유지합니다.

## #263 중복 Loading의 좁은 PC 수정 · PR264 병합

PopupManager/MatchHudView의2C# 후보는 전역 Loading 시각물이 보일 때 HUD의 RETURNING 문구를 숨기고, 전역 시각물이 없거나 returnFailed이면 기존 fallback을 유지하는 범위입니다. 새 입력 차단·Back 소유권·SDK recovery·직렬화 필드 변경과 구분합니다. 정적 독립 검토 뒤 실제 P3/P10에서 새 MVID와 `compilationFailed=false`, 새 getter/refresh 존재를 관측했습니다.

P10의 해당 CLI recompile 명령은 **exit6/COMMAND_FAILED**였고 성공으로 바꾸지 않습니다. 이어진 actual assembly 관측의 새 MVID·compilationFailed=false는 별도 근거로 기록합니다. coarse ready 또는 앞선 recompile 성공을 이 실패 명령의 성공으로 재사용하지 않습니다.

P3의 격리 UI12항목은 모두 통과했습니다. 자기 객체 정리·Popup 필드/Back/input owner 복원·RectTransform/Canvas/Graphic 수226/3/202 원복을 확인했으며 같은 동기 프레임의 owner fixture입니다. 별도 정상 Editor 복귀는 **Running 경기에서 MENU→나가기→확인의3개 public handler**를 거친 경로입니다. 연속 render callback43개 중 전역 Loading41개·local RETURNING0·중복0, 이전 HUD 파괴·새 Manager·LobbyConnected·Loading 닫힘·observer 해제를 관측했습니다. 결과패널 RETURN 버튼 관측이 아니며 그 버튼을 사용한 수정 전 Native04 원본과 구분합니다. 강제 scene/network field setter0이며 **실제 Native pixel 비교/Android 수용은 아닙니다.**

후속 독립 COMMENT는 blocker0이며 총괄이 최신 head/base·필수 조건을 확인한 뒤 PR264를 main e19cb55에 병합하고 #263을 닫았습니다. 이 좁은 PC 수정 완료를 모든 프레임·기기·render 순서나 Android 전체 수용으로 확대하지 않습니다. 사용하지 않는 완료263 브랜치만 tip·Squash tree·사용 여부 확인 후 정리했고 기존 P2 dirty5·보호6개와 미반영 작업을 보존했습니다.

21:48 KST 새 관측에서 기존 P3/P10/P11 Editor PID57772/45496/70368은 모두 부재, native Unity0·CLI STATUS_NO_INSTANCES였습니다. 정확한 ExitCode·종료 원인을 확보하지 못했으므로 **Editor 정상 종료/cleanup 통과가 아닙니다.** 앞선 정상 복귀의 저장된 양성 결과는 해당 시점 근거로 보존하며, 별도 Windows04 Player의 보유 handle Exit0와 혼동하지 않습니다. 이후 P11은 새 PID13136/시작UTC12:50:31.0871514Z의 별도 Editor로 열렸고 batchmode·Graphics Null·StandaloneWindows64/Mono·compilationFailed=false·새 MVID를 실제 확인했습니다. Windows05 빌드와 그 후속은 위 별도 근거를 따르며04 성공을05 근거로 복제하지 않습니다.

## 열린17개와 별도 기록·신규 UI 이슈

20:40 OPEN snapshot의 제품·상위17개를 빠짐없이 한 번씩 재사용합니다. **#259는 이17개에 이미 포함됩니다.** 상위/하위 관계가 겹치므로 독립 구현17건으로 합산하지 않습니다. #262는 기록 묶음, #263은 이후 발견해 PR264로 좁게 완료한 별도 UI 이슈이며 fresh 전체 OPEN 목록의 총수를 추정하지 않습니다.

| 번호 | 남은 범위 |
| --- | --- |
| #259 | FIRE/LOOK Draft 검토·미실행 입력/생명 경계와 새 Android 수용 |
| #198 | 초기 Join/10초 Timeout 원인 분리; 재시도/경기 성공과 구분 |
| #188 | 재장전 손·탄창 접촉/중단·생명 경계와 실제 청취·기기 |
| #166 | 이동·앉기·맵 사다리·팔·조작 품질의 상위 잔여 경계 |
| #163 | 채팅/IME·START 안내·실패/재시도·물리 gesture 경계 |
| #162 | 로컬/원격 발사 효과·연사/빈탄창·생명/메뉴·실제 청취/지연 |
| #154 | Windows 종료 native crash 원인; 새 Exit0 비재현 표본과 분리 |
| #140 | 정적 RELRO 오탐 정정 이후 실제16KB 환경 검증 |
| #132 | cold TMP 최초 경고와 새 Player 첫 표시 원인 |
| #66 | 새 Android 통합·입력/보존·단일 Unity AAB 안정화 상위 범위 |
| #44 | 검증기 CI를 넘어 Unity6 빌드·서명·배포 CI의 운영 후속 |
| #43 | fresh Console/새 빌드·검사·서명·제공·새 APK 전달·설치 |
| #42 | 남은 자산/SDK의 출처·취득/재배포 조건·새 산출물 고지/포함 대응 |
| #41 | 새 APK 기본 경기/동시입력·복귀와 실제 Store/기기·청취·성능 |
| #36 | PlayFab 통합 장기 후속; 기존 PGS→Photon 표본과 구분 |
| #35 | MVC/popup/Canvas/TMP/폴더 문서 등의 장기 리팩토링 |
| #1 | 위 작업들의 전체 로드맵·증거/배포 현황 추적 |

PC에서는 도구 제어가 가능한 시점에 Windows05의 미실행 후속을 분리하고 PR261/#259 잔여 수용 경계를 정리하며 필요한 품질·원인 조사를 작은 목적별로 이어갑니다. 새 Android 통합은 fresh source·원장/Console 최대코드·공개입력/출처·저장/재시작·산출물/고지/native/폰트/심볼·서명/제공 근거를 새로 연결합니다. 기존33의 번호나 proof를 새 성공 근거로 복사하지 않습니다.

실제 기기·Store/PGS·물리 멀티터치·청취·실제16KB·발열·장시간 FPS/메모리·저사양은 합의대로 후순위입니다. 키·USB·발열 질문을 추가하지 않습니다. 정확한 **Sudden Force Console 작업 탭 식별과 기존 테스트 폴더에 접근 가능한 Drive 연결 복구** 두 항목만 후속 사용자 할 일로 모으고 현재 반복 질문하지 않고00시 마감에 모읍니다. 다른 계정 연결에서 폴더404라는 관측은 기존 폴더 삭제나 과거33 APK 전달 실패의 증거가 아닙니다.

총괄의21:44 KST 실제 사용량 API 관측은1%이며 이전16% 표시는 과거 관측입니다. 리셋 사용0이고 전체 게임 완성률을 뜻하지 않습니다. 당일 최신 마감은 **24:00 KST(2026-10-27 00:00)**입니다. 사용량·마감은 해당 시각의 운영 기록이며 자동으로 새 제한이나 사용자 질문을 만들지 않습니다.

## 별도 Android main 준비 · #265

후속 [#265](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/265)에서 main e19cb55의 공개 파일3,990개를 새 키 제외 Android worktree로 준비했습니다. 개인 키·env·credential Editor helper6개는 no-checkout 뒤 sparse 규칙을 설정한 시점부터 제외했고 내용 읽기·해시는0입니다. 작성 시점에는 새 Android Editor의 최초 가져오기를 시작한 상태이며 실제 컴파일·AAB 성공으로 합산하지 않습니다. 기본 Unity/Fork P2·기존 dirty5·소비 원장은 그대로이고 새 버전 예약·서명·업로드는0입니다. 이 준비 이슈는 위20:40의17개 목록 이후에 생긴 별도 작업이며 #66 전체를 닫은 것이 아닙니다.

## 근거와 기록 범위

외부 `20261026-work/`의 공개 근거 이름만 연결합니다. 빌드/산출물은 `windows-build04-artifact-audit.public.json`과 `windows-build04-independent-artifact-review-xhigh01/`, compiled metadata·경고 정정은 `windows-build04-compiled-fire-look-assessment01.public.json`과 `p11-windows04-warnings02.public.json`입니다. Native04는 `player04-native-smoke01/`의 assessment/process-result와 `player04-native-independent-review-xhigh01/`를 따릅니다. Windows05의 실제 audit/metadata 정정과 `windows-build05-independent-artifact-review-xhigh01/`는 별도05 산출물 근거입니다.

합성 입력은 `fire-look-runtime01/`·`fire-look-lifecycle02/`의 root-assessment/result, 현재 focusfalse는 `focus-guard03/result.public.json`과 `p10-focus-guard03-action01.public.json`, R11은 `p3-after-r11-returned-lobby01.public.json`입니다. #263은 `double-loading-source-review-xhigh01/`와 `double-loading-runtime-independent-review-xhigh02/`, `double-loading-runtime01/result.public.json`, `double-loading-normal-return01/result.public.json`, P3/P10 actual assembly 및 P10 recompile 공개 receipt의 범위입니다. Editor 부재/새 P11은 `editors-absent-and-p11-reopen05.public.json`, Native05는 `player05-native-two-peer01/assessment.public.json`과 `player05-native-independent-review-xhigh01/`의 고정14장 양성 범위입니다. 헤더의 초기 시각 추정 정정은 `native05-result-glyph-audit-xhigh01/`의 원본 픽셀 마스크 비교와 독립 재계산을 따릅니다. 남은17개는 `remaining-work-2040-xhigh01/`의 mapping과 대응합니다. 각 근거의 실제 bytes/SHA와 원문 보존은 외부 후보 manifest에 별도 고정합니다.

총괄의 실제 실행, 산출물/Native 독립 검토, 이 문서 작성의 역할을 구분합니다. 문서 작성자가 빌드·런타임·기기 시험을 반복한 것은 아니며 원시 로그·키·APK/JPG·DLL을 저장소 문서로 복사하지 않았습니다. [앞선19시 후속](pc-validation-followup-summary.md), [현재 계획](../../../PROJECT_PLAN.md), [추적 문서](../../../ISSUE_TRACKING.md)의 기존 원문은 각 시점 기록으로 보존합니다.
