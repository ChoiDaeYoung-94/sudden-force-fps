# 2026-11-07 보행 후처리 진단과 통합 준비

Refs #341. 새 보행은 아직 제품에서 활성화하지 않았습니다. 이 기록은 별도 CPU 진단·Android 대상 컴파일 관측과 진단용 에셋의 실제 생성 기록이며, PR344의 보행 품질 완료나 새 APK 포함을 뜻하지 않습니다. 기존 실패와 미완료 범위는 유지합니다.

## C11의 별도 1mm 진단

고정 source `31789E517B1C94D597C2B84D5AD5742686A5F4629CD4A386012E6311E39A19C3`의 Blue/Red·32개 위상·32개 blend 조합에서 새 `runtimeVerticalContact1mm` 2,048개가 수치 조건을 만족했습니다. 1,222개는 추가 solve 없이, 826개는 한 iteration에서 총 1,068회 leg solve로 조건을 만족했습니다.

| 직접 집계한 값 | 결과 |
| --- | --- |
| 원 raw mixer 대비 최대 최종 foot 변위 | 79.22352998 mm |
| 최소 양발 sole / 전체 CPU skin Y | 0.9995997 / 0.9995997 mm |
| 최대 보고 target residual | 0.0004571039 mm |
| 보고 foot world orientation error 최대 | 0° |

Boots 704개는 총집합이며 calf 영향 453개는 그 부분집합입니다. 61개 bone·1,428개 influence의 전체 가중치를 사용했습니다. 제한된 12개 witness의 CPU LBS 대조는 전체 GPU skin 수용으로 확대하지 않습니다. 위 수치는 고정 rig/clip과 유한 표본의 오프라인 평면 진단이며, 실제 지면 접촉이나 시간 연속성·자연스러운 footplant의 증거가 아닙니다.

원 blend 494/2,048 실패와 두 8mm trial의 종합 판정 실패(`all=false`), `allQuantitative=false`는 그대로입니다. 두 trial의 개별 outcome success 2,044/2,048·1,910/2,048을 전부 실패했다고 표현하지 않습니다. 실제 C11의 managed/native/tool 종료도 1이므로 정상 실행 전체 통과로 세지 않습니다. 기존 cardinal 66·dense 4,614·continuity 9와 새 post-IK 연속성 검증은 별개입니다.

독립 실제 검토: `D:/codex/validation/sudden-force-fps/20261107-work/contact11-actual-independent-review01/verification.public.json`, SHA256 `A4571962453AA9367B4954A15C4B246052BA1E2DD652FCB07C001723480F2B67`. 실제 출력 281개/57,197,889B와 이전 입력 보존을 대조한 범위입니다.

## 별도 Android 대상 컴파일 관측

제품 후보03의 정확한 제안 소스 7개를 P13의 source `98d191c806bec27c9ee532b80bd804e1115bd26a`에 임시 적용했습니다. 원본 4개와 새 파일은 전체 4,067개 공개 입력 백업 후 다뤘으며, 프로젝트 파일로 적용하기 어려운 기존 malformed diff는 사용하지 않았습니다. 이 기록 커밋에는 그 제안 소스 7개를 추가하지 않습니다.

첫 Native는 원 시간창 부족으로 강제 종료되어 결과가 없습니다. 다음 Native는 검사 entry까지 실행했으나 `ACTUAL_DEFINE_MODE`에서 중단했습니다. 동일 보유 프로세스 42516은 2026-11-07T13:13:52.8851223Z에 시작해 13:14:39.2249163Z에 ExitCode1/forcedfalse로 끝났고 Dispose 오류는0입니다. 전체 closed-compile 검사는 실패 상태를 유지합니다.

이후 실행 없는 Cecil 메타데이터 관측에서는 helper의 QA 상수 true, 게임의 QA 접속 버전 분기 문자열 1개, 신규 네트워크 속성 4개의 위빙 정보를 확인했습니다. `WorldGaitPhase`, `WorldGaitLocalVelocity`, `WorldGaitEligible`, `WorldPoseClimbing`의 offset/word는 각각 `(1,1)`, `(2,2)`, `(4,1)`, `(5,1)`입니다. 합계 5word 확인을 전체 클래스 위빙 검사·정상 경기나 실제 snapshot 수용으로 확대하지 않습니다.

근거는 `root-resume01/gait03-native02-offline-metadata.public.json` SHA256 `17EBFA2CFEFA6EE42DDC91836783DF2FFB8E237F647CE01EF5C17B4B4A4F6293`입니다. 대상 게임 assembly를 load하거나 제품 메서드를 호출하지 않았습니다. 당시 API defines 배열과 실제 응답 파일 정의를 구분하는 후속 검사는 미실행이었으며, 아래 C4에서 별도로 관측했습니다.

복구 첫 시도는 EDM 설정의 자동 서식 차이를 만나 중단했습니다. 원본과 생성본을 보존한 뒤 CRLF와 줄 끝 공백만 다름을 확인하고 같은 exclusive 파일 핸들에서 비교·복원했습니다. 두 번째 복구는 전체 4,067개 raw exact·임시 11개 부재·Git clean을 확인했습니다. Editor0·공용 ADB 설정 DWORD1 복귀·기존 서버 동일 세대 보존 후 13:19:25.2548763Z에 실제 반환했습니다. 이 P13 복구와 P2의 기존 dirty5는 구분합니다.

복구 근거는 `D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-editor-window08-return.public.json`, 같은 폴더의 `gait03-native02-edm-format-restore.public.json`과 `D:/codex/validation/sudden-force-fps/20261107-work/root-gait03-stage-restore-candidate02/actual-4e9ffa8f47444f0f87f174596ef17113/restore-02/restore-result.public.json`입니다. 첫 중단 기록은 별도 restore-01에 보존합니다.

## C4의 실제 closed compile 후속

앞선 실패 출력은 그대로 보존하고, 별도 C4 helper와 새 nonce `b5e8577bebb84ddcaad0a30ba71349d4`로 다시 관측했습니다. Native20764는 14:50:49.9095447Z에 시작해 정상 Exit0/forcedfalse/Dispose0으로 끝났고 managed 결과는 `CLOSED_COMPILE_METADATA_OBSERVED`입니다. 실제 tool session95848/chunkbf66aa도 종료0입니다.

Game/Editor API defines의 QA 값은 여전히 false인 관측입니다. C4는 자기 `csc.rsp` 정확30B, 두 compiler 목록의 자기 응답 파일 각1개, parser의 QA=true/errors0, 컴파일된 helper QA 상수 true와 같은 게임 모듈의 정확한 `-localqa1` IL 문자열1개를 결합해 QA 모드를 확인했습니다. 다른 응답 파일 본문은 열지 않았으며 게임 메서드/getter도 호출하지 않았습니다. 이 복합 판정은 완전 CLR IL 검증이나 전체 source-to-IL 등가 증명이 아닙니다.

실제 메타데이터에서 Networked 속성43개·전체75워드와 신규4개/5워드 배치를 확인했습니다. 승인 SHA3개는 빈 문자열이고 Play/Resources/graph/제품 활성화는0입니다. 같은 내용의 게임 DLL은 이전 관측과 동일 SHA/MVID이며, 모든 소스의 cold rebuild를 새로 수행했다고 주장하지 않습니다.

첫 restore는 EDM의 줄바꿈·후행 공백 변경 하나에서 중단됐습니다. 양쪽 바이트 보존과 별도 Root CAS 후 restore02는 전체4,067개/751,146,413B의 원 경로·길이·SHA 행 일치 및 임시11개·폴더 부재를 확인했습니다. Editor0·두 prefs 원래1·공용 서버 동일 세대 확인 후 14:55:08.4161379Z에 원 창 안에서 실제 반납했습니다.

실제 결과/후처리 검토는 `gait03-c4-actual-independent-review01/verification.public.json`, SHA256 `CC0110A32542A010C189B0C0428449E8E18C71A22D7DCAD364EB47DCD7203E6C`입니다. 이 결과 검토자가 C4 소스 작성자라는 점과, 별도 작성자의 source IR `0134107166D70B254A823BE1FFAD08A67147EFD710F33AF944A1324239E2DAEE`를 구분합니다. 최종 반환 근거는 `root-resume01/gait-editor-window09-return.public.json`입니다.

당시 별도 에셋 생성기 자체의 metadata-only compile도 정상0으로 끝났으나, 그 창에서는 실제45개 파일을 생성하지 않았습니다. 생성 준비 writer01이 고정 manifest의 외부 공개14개 경로를 거절해 설정 쓰기 전에 중단됐습니다. exact 경로 수정02는 독립 검토했지만 원 시간창의 최소 여유 조건을 지나 실제 실행하지 않았고, 기존 창을 늘리지 않았습니다. 다음 시도는 새 원창을 쓰는 별도 revision입니다.

## Body01 실패와 Body02의 별도 실제 자산 생성

후속 Body01의 Native48524는 15:27:33.4278928Z부터 15:28:21.7913101Z까지 실행해 Exit1/forcedfalse/Dispose0, tool63528/chunkb2cc53/1로 끝났습니다. Body는1회 호출됐으나 InvalidOperationException/OPAQUE_FAILURE였고 실제 materialized 파일은0개였습니다. 이 실패를 아래 Body02 성공으로 소급 승격하지 않습니다.

공개 소스 조사에서는 같은 Boots3 mesh를 참조하는 비활성 ViewModel renderer와 월드 renderer가 양 팀에 각각 존재하고, Body01의 inactive-inclusive 검색 뒤 Single 선택이 두 매치를 받는 결함을 확인했습니다. C11의 월드 renderer 한정을 빠뜨린 작성자 소스 결함이며 mesh 이름 불일치나 SDK/기기 결함으로 바꾸지 않습니다. 다만 당시 실제 실패에는 phase/throw 위치가 없어, 그 예외가 바로 이 줄에서 발생했다고 확정하지 않습니다.

Body02는 authored activeSelf 체인·enabled·ViewModel ancestry 제외·정확 월드 경로/renderer 이름으로 후보를 한정하고 count1을 검사하며, 기존 mesh GUID/localID·704/61/1428와 BuildClip 검증은 보존했습니다. 별도 source IR은 `gait-contact03-materializer-independent-review02/verification.public.json`, SHA256 `B4B804FDF284F2ABD81D5C7CDC003F4FFDA8EA71EF140D804A9F20A59E319615`입니다. 이 source IR은 실제45나 런타임을 승인한 것이 아닙니다.

새 nonce `484c6a2c363141b8bb8b1673e159dbdf`의 Native39732는 16:13:24.0626381Z에 시작해 16:13:45.4561628Z에 정상 Exit0/forcedfalse/Dispose0으로 끝났고 tool50821/chunke3434f도0입니다. Entry02는 같은 Native에서 현재 source·signature/module MVID·compiled Editor DLL pin을 확인한 뒤 `compile-metadata-before-body.public.json`을 CreateNew했습니다. 관측 MVID는 `3f61cfbe-4049-4f61-ad21-5a621cb682a6`, DLL은110,592B/SHA256 `D5E384023DDE11D9570665D1C64A121C693D927593123FC401FC7E3DC2F5719A`이며 앞선 Body01 compile의 MVID와 다릅니다. witness 시점 materializerCalls0 뒤에 Body를 정확1회 호출했고 최종 Entry receipt는 `CLOSED_MATERIALIZATION_RESULT_VERIFIED`입니다. 이전 Body01 D386 compile qualification을 새 Body02 승인으로 재사용하지 않았으며, 전체 source-to-IL 등가도 주장하지 않습니다.

Root 결과는 원 raw9+corrected9의 AnimationClip18개·TextAsset3개·meta를 포함한 정확45파일 생성을 기록합니다. geometry·diagnostic evidence·postblend contract의 positive 승인값과 runtime 활성화는 false이고, owner-origin plane offset은 여전히 null/pending입니다. clip identity의 loadedCurveKeysMatchInput=true는 Body의 API readback 자기보고이며, 별도 직렬화 바이트 검토와 구분합니다. 외부 snapshot02는45개 공개 파일의 byte 보존을 기록하며 snapshot SHA256은 `548DD0915E6923D3C299C7535385A4C9A5F41BC4628C2F0C7E4C4E3E36AA529E`입니다.

독립 실제45 IR은 [verification.public.json](D:/codex/validation/sudden-force-fps/20261107-work/gait-materialized-assets-actual-independent-review02/verification.public.json)11,535B/SHA256 **DED75CE3B35FF299536E632A4C2A6C10D96B035D22597A1E26BE7A01E42DE590**이며 `materialization45Accepted=true`입니다. 수용한 결과 SHA는 `FABDE789CFEFC914B375EA430F98EE5C50E0F40CD243404D98A7823EBE34AA37`입니다. 정확45개/57,827,494B·24개 고유 GUID와 실제 외부 snapshot의 경로·길이·SHA, JSON cross-pin을 대조한 범위입니다.

독립 검토는18 clip의 Editor quaternion48곡선 전체 key 시간·값222,336 scalar와 runtime Quaternion12곡선 값111,168개를 입력 sample의 float32 bit와 대조해 불일치0을 확인했습니다. 별도 EulerEditor36은 leg12의 localEulerAngles.xyz이며48곡선과 이중 합산하지 않습니다. geometry의704 vertices/61 bone paths·976 bindpose scalar/1,428 influences와 calf453/single-foot46을 C11 registry와 대조했고, JSON의12 positive flags=false·plane offset=null/pending을 확인했습니다. 새 GPU나 CPU2048 trial을 실행한 결과는 아닙니다.

Imported Generic/Resources API 성공은 Body의 자기보고이며 해당 API를 독립 재실행하지 않았습니다. DLL은 해시만 대조했고 MVID를 DLL에서 새로 해석하거나 source-to-IL 등가를 증명하지 않았습니다. Native/tool/restore/반납은 Root 공개 기록 범위로 분리 수용했습니다. 현재 프로젝트49개 입력 전체 재감사·프로젝트 전체 GUID 충돌·새 imported mesh/GPU/물리·NCC/두 peer·제품/새 APK 포함을 이 IR로 승인하지 않습니다. 자세한 한계는 [독립 실제45 보고서](D:/codex/validation/sudden-force-fps/20261107-work/gait-materialized-assets-actual-independent-review02/REPORT.ko.md)6,185B/SHA256 `A1D29703E3C2ACC02F18B1C68185E186B60F81F47AEDEAEB60B535436237A759`에 있습니다.

복원01은 EDM 자동 서식 변경에서 차단돼 원본·생성본과 실패를 보존했습니다. Root는 EOL/후행 공백 동치를 확인한 같은 exclusive handle CAS 후 restore02에서 공개4,067개 raw exact·own5/helper 폴더 부재·materialized root/meta 부재와 기존 Resources parent 보존을 기록했습니다. 45개는 외부 snapshot에 남겼고 실제 프로젝트에 계속 적용한 상태가 아닙니다. 두 config의 active 원문을 보존한 뒤 Body7개/Entry9개 bool을 모두 false로 퇴역했고 stage도 retired했습니다. 원 source/nonce/3시각을 변경하거나 실패 출력에 덮어쓰지 않았습니다.

원 start16:10:20/new16:22:20/full16:25:20Z 안인 **16:17:03.9112502Z**에 실제 반환했습니다. Root proof는 Editor0·두 prefs 원래1·공유49852 동일 세대·Git clean·root reservation0을 기록합니다. 이 문서 작성자가 프로세스/Registry/프로젝트 전체를 새로 조사한 것은 아닙니다. 최종 반환 proof의 독립 실제45 검토 pending 표시는 작성 당시 상태를 보존합니다.

근거: [Body01 slice 반환](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-materializer-assets-window02-slice-return.public.json), [Body01 소스 조사](D:/codex/validation/sudden-force-fps/20261107-work/gait-contact03-materializer-candidate02/failure01-source-investigation.public.json), [같은 Native의 호출 전 witness](D:/codex/validation/sudden-force-fps/20261107-work/root-gait-materializer-entry-candidate02/run-actual-484c6a2c363141b8bb8b1673e159dbdf/compile-metadata-before-body.public.json), [Body02 생성 결과](D:/codex/validation/sudden-force-fps/20261107-work/gait-contact03-materializer-candidate02/materialized-actual-484c6a2c363141b8bb8b1673e159dbdf/result.public.json), [snapshot02](D:/codex/validation/sudden-force-fps/20261107-work/gait-materialized-assets-snapshot02/actual-484c6a2c363141b8bb8b1673e159dbdf/snapshot.public.json), [EDM CAS](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/body02-materialization-edm-format-restore.public.json), [restore02](D:/codex/validation/sudden-force-fps/20261107-work/root-gait-materializer-stage-restore-candidate03/actual-484c6a2c363141b8bb8b1673e159dbdf/restore-02/restore-result.public.json), [config 퇴역](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/materializer-body02-configuration-retired.public.json), [최종 반환](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-editor-window10-return.public.json) SHA256 `814CA5590F16EC9E8D72C950C5787A1FFD63EA47E4A7324C6384D3CE70684BBD`.

| 자료 | 관측·수용 범위 |
| --- | --- |
| C11 CPU2048 | 고정 표본의1mm 수치 진단; 기존8mm 실패·전체 Native1 유지 |
| C4 metadata | QA 복합 근거·43속성/75word·신규4/5word; 런타임과 별개 |
| Body02 assets | 독립 exact45/바이트·GUID·곡선/JSON 수용; Imported/Resources API는 Body 자기보고 |
| Owned Rest61/GPU preview | 실제 실행 전; 자연모션·704 GPU skin·접지·물리 수용 아님 |
| NCC/네트워크/기기/새 APK | 실제 보행·두 peer·life/history와 gait 포함 APK는 미검증/미포함 |

## 남은 작업

- 독립 수용한 raw9+corrected9/JSON3의45파일 snapshot을 후속 staging/import에서 별도 검증. 제품 Resources 소비·Owned Rest61/post-idle 관측과 실제 GPU preview는 아직 별도 단계입니다. Rest01은 inactive 포함 Boots3 이름 선택이 world/ViewModel 두 renderer와 일치하는 소스 차단으로 수정·독립 IR을 기다리며, 이는 해당 줄 도달시의 소스 반례이고 실제 Rest 실행 실패를 관측한 것은 아닙니다.
- Builtin Plane의 실제 identity/topology와 고정 평면 지원 범위 확인. geometry의 owner-origin plane offset은 null/pending이며 0으로 승인하지 않습니다.
- 원 SkinnedMeshRenderer를 사용하는 실제 GPU·rifle 후처리·704개 전체 가중치 검증과 post-IK 시간 연속성.
- 실제 NCC 이동·resimulation·snapshot 보간·새 life·두 peer와 기기에서의 보행 검증.

제품의 승인 SHA3개는 비어 있고 positive flags는 false라는 기존 source/C4 관측을 유지합니다. 실제45 snapshot으로 현행 제품3개 SHA를 새로 독립 증명한 것은 아니며, 별도로 자산 JSON의12 flags=false를 확인한 범위입니다. 새 로컬 Android QA의 source98에는 input261·main HUD/Intro·rifle345가 포함되며 gait344는 미포함입니다. 제공34/release12와도 구분하며, 현재 자료만으로 PR344를 병합하거나 #341을 닫지 않습니다.
