# 2026-11-07 보행 후처리 진단과 통합 준비

Refs #341. 새 보행은 아직 제품에서 활성화하지 않았습니다. 이 기록은 별도 CPU 진단·Android 대상 컴파일 관측·진단용 에셋 생성·Rest pose 관측과 오프라인 정지 이미지 생성 기록이며, PR344의 보행 품질 완료나 새 APK 포함을 뜻하지 않습니다. 기존 실패와 미완료 범위는 유지합니다.

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

## Rest03 관측과 Preview02의 정지 이미지 생성

Rest01은 inactive 포함 Boots3 이름 선택이 world/ViewModel 두 renderer와 일치하는 소스 차단이었습니다. 해당 줄 도달시의 반례이며 실제 Rest 실행 실패를 관측한 것은 아닙니다. 뒤이은 Rest02는 staging 계약의 source 길이37438이 실제38113과 달라 적용 전에 차단됐고, 두 후보와 당시 검토 이력은 보존했습니다. Rest03은 월드 renderer 한정과 정확한 중첩 pin을 별도 source IR `68D39050A1F3F675A26FCA8C19D80B302CED935515F3291C4715BABB3239345F`로 검토한 새 후보입니다.

새 nonce `8934bb21a0ba4902bfd86773e6bf5665`에서 Rest Native54748은 17:36:15.3060088Z부터 17:37:08.9215417Z까지 정상 Exit0/forcedfalse/Dispose0, outer tool4455/chunk2deb33/0으로 끝났습니다. 독립 실제 Rest IR은 [rest61-actual01-independent-review.public.json](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/rest61-actual01-independent-review.public.json)10,890B/SHA256 **51D5FC2B4940F90DCE5EB03BE09AE7BE31D4EAFCA89F067ADB89182F36AADF91**입니다. Blue/Red 각61행, upper49×2/raw-leg12×2, JSON roundtrip122행과 source 사전후122행을 대조해 offline Preview 입력으로 수용했습니다. clone renderer 복원은 실제22×2=44행이며 초기15개 요약을 정정했습니다. 전체 C11 post-idle61 pose 동등성·source-to-IL·물리/자연보행/제품 수용은 여전히 false입니다.

이어 Preview02 Native12488은 17:40:43.8609384Z부터 17:41:04.1865596Z까지 정상 Exit0/forcedfalse/Dispose0, outer tool28919/chunk5c66d7/0으로 끝났습니다. 실제 결과는98,830B/SHA256 `6A383CD93B638CEC70D6BAB032F0CB0559067BA8EFB6B3F7F23441AD13FE6CB5`, status `OFFLINE_GPU_IMAGES_CREATED_REVIEW_PENDING`이며64행과128개 PNG를 생성했습니다. D3D11에서 자기 clone의 원8개 skin renderer를 사용하고 CPU 대체 renderer·제품 생성/승인 쓰기는0으로 보고했습니다. observer/game/binding 세 MVID 관측값은 각각 실행 사전후 같았지만 전체 source-to-IL 등가나 hardware skinning 실행 경로를 증명한 것은 아닙니다. 연속 재생이나 Runtime/NCC 실행도 아닙니다.

후속 [Preview02 독립 actual IR](D:/codex/validation/sudden-force-fps/20261107-work/gait03-preview02-actual-independent-review01/verification.public.json)5,030B/SHA256 **8343EAE6EF7DAEDD9DAC5434FE6712207956CC6B1590CB9BA2D7CE6220253896**은 정상 종료의 공개 기록과128 PNG의 실제 길이·SHA·CRC·512×768·IDAT, 보고 수치64행의 재집계를 수용했습니다. 생성 당시 result의 review-pending status는 덮어쓰지 않고 이 후속 IR과 구분합니다. Root의 [128파일 해시 대조](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-preview02-image-uniqueness.public.json) 및 독립 IR 모두 실제 이미지가4종뿐이며 팀×front/side마다32장이 완전히 같은 바이트임을 확인했습니다. [대표4종 contact sheet](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-preview02-four-unique-contactsheet.png)는 정지 화면이며 Walk/Run·방향·위상의 시각 변화와 자연보행 수용은 false입니다. 원인은 미확정입니다.

`fullMinimum`은 이미 평면 상대값입니다. 올바른64행 집계는 +1.00003555~+2.76454538mm, −2mm 미만0행이며 최대 raw→foot 변위46.3492423mm입니다. Blue11,604/Red11,222개 witness는 helper가 원8 renderer에서 보고한 BakeMesh 수치를 재집계한 범위로, 독립 정점 solver 재실행이나 C11의2,048개/전체 위상 수용이 아닙니다. 평면을 두 번 차감해 침투로 해석했던 검토자의 초기 해석은 철회·정정됐고 유효 finding으로 남기지 않습니다. 자세한 범위는 [독립 actual 보고서](D:/codex/validation/sudden-force-fps/20261107-work/gait03-preview02-actual-independent-review01/REPORT.ko.md)에 있습니다. 자기 clone의 skin 행렬 갱신을 분리하는 Preview03은 source-only 후속 후보이며 새 실행이나 원인 확정은 아닙니다.

복원01은 EDM 자동 서식 변경1개를 만나 중단했고 양쪽 바이트와 실패를 보존했습니다. Root는 [EOL/후행 공백 동치와 같은 exclusive handle CAS](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/rest-preview02-edm-format-restore.public.json)를 별도로 기록한 뒤 [Unified04 restore02](D:/codex/validation/sudden-force-fps/20261107-work/root-gait03-unified-preview-stage-candidate04/actual-8934bb21a0ba4902bfd86773e6bf5665/restore-02/restore-result.public.json)에서 공개4,067개 raw exact·own62개와4폴더 부재를 확인했습니다. Rest12개/Preview10개 config bool은 모두 false로 퇴역하고 active 원문 archive를 보존했습니다. [최종 반환](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-rest-preview-window02-return.public.json)1,486B/SHA256 **AA5E791233DF47286462863C8EE8CD71132DACAF7A9E9F905D62026A72928431**은 원 start17:33:59/new17:45:59/full17:48:59Z 안인17:43:24.088668Z에 Editor0·두 prefs1·공유 ADB49852 동일 세대·Root 공유/예약0을 기록합니다. 반환 당시 독립 Preview 검토 pending 표시는 당시 상태를 보존하며, 이 문서 작성자가 OS/Registry/Git/전체 프로젝트를 새 감사한 결과는 아닙니다.

## Preview03의 같은 프레임 행렬 갱신 비교

Preview02의 원본·실패·실제 IR은 그대로 보존하고 별도 Preview03 후보를 만들었습니다. Unity 공식 [forceMatrixRecalculationPerRender](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityengine/skinnedmeshrenderer/forcematrixrecalculationperrender)의 동일 update 다회 수동 렌더 용도를 참고해 검증 clone의 원8 SkinnedMeshRenderer에서 원래 값을 캡처한 뒤 true로 설정하고, 매 렌더 getter8개와 cleanup 원래값 복원을 검사했습니다. Graph·수학·clip·원본 pose·제품 가드는 보존했고 source IR `744B97184AF51E585F66A1C041B3E202140FBFAA287566A7815E94B2C41E829F`는 core13/7종 역변환 범위만 수용했습니다. 이 옵션을 제품 캐릭터나 원본 prefab에 적용한 것은 아닙니다.

새 nonce `48a99d62f8d54acf9d422ea721249e2c`의 Rest Native27388은17:57:18.1857225Z→17:57:34.7759039Z 정상0/forcedfalse/Dispose0, tool69485/chunk6f48e5/0입니다. 새 Rest436,403B/SHA256 `519AEF0E3DDB0350EB30DA37012B301075B358B542C57F266D3D60130666B7F1`과 독립 IR `285EF40663DAFD9D9AF8AE89267674D823A49A87C8315FBD002230C86D56E677`로61×2/source122/renderer44/roundtrip을 다시 대조했습니다. 현재 observer MVID는 `0868bfcd-fbea-45e8-9fb4-3c4e982057a2`, binding은 기존 `54356e39-7aac-447e-a179-71e8b11a5c71`입니다. 이전 nonce의 Rest 결과를 새 실행 성공으로 복사하지 않았습니다.

Preview03 Native46268은18:00:26.1738893Z→18:00:44.4066132Z 정상0/forcedfalse/Dispose0, tool74980/chunk5a8d27/0입니다. 결과106,638B/SHA256 `16B7E371F0EA1217D1DA2E8C94059261CC2E539882F4C54342C31248B4D9039F`의64자세/128 PNG를 실제 재해시했고 모두 서로 달랐습니다. [독립 actual IR](D:/codex/validation/sudden-force-fps/20261107-work/gait03-preview03-actual-independent-review01/verification.public.json)5,439B/SHA256 **000DC5414A0B189374DF2FAA835A95062DB03D38CE4B6456C70162B53A70330A**도 전파일 길이/SHA/CRC·512×768·PNG 바이트·압축 해제 filtered stream128종과 팀×시점별32종을 직접 확인해 `offlineStillPoseVariationAccepted=true`로 판정했습니다. 대표8장과 [Root의16장 비교 화면](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-preview03-forward-phase-comparison.png)에서 걷기/달리기 phase0/.5의 leg/knee 자세 차이를 관측했습니다.

보고 수치64행은 동일 tuple의 Preview02 값과 같으며 평면 상대 fullMinimum +1.00003555~+2.76454538mm·최대 foot 변위46.3492423mm입니다. 전128개 getter count8/원8 skins/D3D11·실행 전후3 MVID 안정성을 확인했지만 실제 hardware skinning 경로·전체 source-to-IL·연속 동작/자연스러움·물리/NCC·두 peer·기기·APK 포함·제품 수용은 모두 false입니다. 이 비교는 같은 프레임 반복 이미지 문제를 해당 fixture에서 해결한 결과이며 실제 게임 보행 구현 완료가 아닙니다.

EDM 서식 변경1개로 막힌 restore01과 양쪽 바이트를 보존한 뒤 EOL/후행공백 동치·exclusive CAS를 별도 기록했습니다. Unified05 restore02는 공개4,067개 raw exact·own62/4폴더 부재를 확인했습니다. 활성 config 원문 archive를 남기고 bool12+10을 false로 회수했습니다. [실제 반환](D:/codex/validation/sudden-force-fps/20261107-work/root-resume01/gait-rest-preview-window03-return.public.json)1,502B/SHA256 **C709F9D130DAA687A59269C7E1BB06E71C4891953CF10F78261EF23724DDAB98**은 원17:55:04/new18:07:04/full18:10:04Z 안인18:02:46.980270Z에 Editor0/prefs1/공유49852 원세대·5037/Root공유·예약0을 기록합니다. 독립 검토자는 이 OS 상태를 직접 재조회한 것이 아니라 Root 공개 proof와 결속했습니다.

| 자료 | 관측·수용 범위 |
| --- | --- |
| C11 CPU2048 | 고정 표본의1mm 수치 진단; 기존8mm 실패·전체 Native1 유지 |
| C4 metadata | QA 복합 근거·43속성/75word·신규4/5word; 런타임과 별개 |
| Body02 assets | 독립 exact45/바이트·GUID·곡선/JSON 수용; Imported/Resources API는 Body 자기보고 |
| Owned Rest03 | Blue/Red61×2·upper49/raw-leg12/roundtrip/source 보존 대조; offline Preview 입력만 수용 |
| Preview02 | 정상 종료·128 PNG 무결성/64수치 집계 수용; 이미지4종×32동일로 보행 시각 변화·자연모션 수용false |
| Preview03 | 정지자세128종/대표 leg·knee 변화 수용; 같은 프레임 반복 이미지 해결, 연속·자연·게임 보행 수용false |
| NCC/네트워크/기기/새 APK | 실제 보행·두 peer·life/history와 gait 포함 APK는 미검증/미포함 |

## 남은 작업

- Rest03의 offline 입력과 Preview03의128종 정지 자세 변화를 생성·검토했습니다. 다음은 연속 주기와 전환/실제 게임 이동의 시각 검증이며, 자연보행 수용은 아직 false입니다.
- Builtin Plane의 실제 identity/topology와 고정 평면 지원 범위 확인. geometry의 owner-origin plane offset은 null/pending이며 0으로 승인하지 않습니다.
- 원 SkinnedMeshRenderer의 hardware skinning 경로·rifle 후처리·704개 전체 가중치와 post-IK 시간 연속성 검증. 128개 정지 이미지 생성과 별개입니다.
- 실제 NCC 이동·resimulation·snapshot 보간·새 life·두 peer와 기기에서의 보행 검증.

제품의 승인 SHA3개는 비어 있고 positive flags는 false라는 기존 source/C4 관측을 유지합니다. 실제45 snapshot으로 현행 제품3개 SHA를 새로 독립 증명한 것은 아니며, 별도로 자산 JSON의12 flags=false를 확인한 범위입니다. 새 로컬 Android QA의 source98에는 input261·main HUD/Intro·rifle345가 포함되며 gait344는 미포함입니다. 제공34/release12와도 구분하며, PR344는 Draft, #341은 OPEN을 유지하며 현재 자료만으로 병합하거나 닫지 않습니다.
