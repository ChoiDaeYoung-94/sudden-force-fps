# Native basic controller 초안과 소스 검토 기록 (#328)

기존 helper의 single 기본 기능을 별도 controller에서 검토하기 위한 **source-only 초안·검토 기록**입니다. 실제 controller 완성·#328 종결·C# 컴파일·SDK/게임 수용은 아닙니다. 제공 기준 코드34/release12를 유지하며 새35 예약·빌드·서명·업로드는0입니다.

R=`D:/codex/validation/sudden-force-fps/20261103-work/resume-0210`, C=`D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/native-basic-candidate01/revision02`입니다. 현재 source3는 외부 C에 보존되어 있으며 프로젝트에 등록하지 않았습니다.

## single14와49 관측 경계

기존 Java single은 pointer0 DOWN1+MOVE12+UP1의 정상14 event·6초 event profile입니다. 초기 query·전체wall의6초 상한이 아니며 held-life32 판정과 혼용하지 않습니다. parser는 exact helper result19·instrumentation code0 정확1개·PC child 정수exit0, attempted14/elapsed14 비감소·stage6=0·정상 선언을 검사합니다.

Recorder49는 행마다의 정수 필드 수입니다. 같은 상태의 행은 생략하므로14 event=14행이 아닙니다. 모델은 whole-prefix seq1·현재 identity/life/focus/Game·mask·positive touch-polled, launch 뒤 capture→최종 neutral을 검사합니다. 실제 UP/neutral 수용이 아니며 look fingerID로 회전을 판정하지 않습니다.

기본 비활성 C# observer는 PC 상대 actor의 공개 AimYaw/AimPitch·CameraRoot와 identity/life를 읽습니다. PC replicated Aim과 Android 실제 local view는 다릅니다. Snapshot/GetInput/SeedAim/Reset/SourceState·SetFire 등 관측용 호출·writer는 없습니다. getter 무부작용·paired 수신·C# compile/runtime은 미검증입니다.

## M1과 검사군

revision01은 child timeout에18초·원UTCstop 잔여만 넣어 원Recorder/Root mono·geometry 경계를 넘길 수 있었습니다. 원source·false IR·반례를 보존하고 revision02 driver 한 hunk만 수정했습니다. core·observer는 부모 rawexact입니다.

argv 계산→최종UTC/mono 표본→같은 표본의 기존 guard→`min(18s, 원UTCstop 잔여, 원Rootmono780s 잔여, 원Recorderanchor100s−정책2s 잔여, 원geometryUTC/mono10s 잔여)`→단일 injected child 순서입니다. 원시각/anchor 리셋·threshold 완화는 없습니다. 순차 표본의 원자성·SDK latency를 보장하지 않습니다. geometry 잔여≤10초이며18초는 보존된 상한입니다.

| 검사군 | 결과·범위 |
| --- | --- |
| 저자 기존 회귀 |161/실패0, 새source·default business IO trap0·strictJSON/14·identity·partial/poison |
| 저자 신규 M1 |63/실패0, 잔여 min·1ns/UTC1tick·경계 도달/초과·시계 어긋남·argv 순서·원M1 재현 |
| 독립 Source02 |50개, 한hunk 역변환·core/observer rawexact·원기한/잠금·경계 |
| 독립 Control02 |63개/실패0, 그중20 경계case·timeout 전달/반례 해결·default/poison |

네 그룹과 이전 회차를 합산하지 않습니다. fake child의 근접0 elapsed는 산술 경계 시험이며 실제 instrumentation 속도·운영 가능성의 실측이 아닙니다.

## 현재 판정·핵심 핀

Source02의 sourceAccepted/noBlockers=true, Control02의 controlledTestsAccepted/noBlockers=true는 **DRAFT source·메모리 모델만** 수용합니다. manifest의 저자 시점 false·과거 IR은 수정하지 않았습니다. DRAFT_SOURCE_ONLY_LOCK=true·observer Enabled=false, CLI --enable도 잠금입니다. 실제 실행/ready/권한·nativeBasic/gameFireCaptureReleased/replicatedAim/Androidview/helperExit/resourceReturn/timing/packet/L2는 모두false입니다.

| 파일(별도 표시 없으면 C 아래) | bytes / SHA256 |
| --- | --- |
| basic_contract01.py |16359 / 1B9137C8444E7231C6D0F8B79625562EBFC6428048A2AD02B5B3D6F4745CE40E |
| bounded_sdk_driver01.py |6235 / 39D37E2E9663BC1A3217F38FA56917594BAEE1761925EB892E3D27A8E36A809E |
| NativeBasicReadOnlyObserver01.cs |4568 / FDF753D3C3F5B3306AD1F4E85B5785AA5D731A4F315DFDEE13DA837CEB0A731B |
| manifest.public.json |6526 / DE70FB5A34E66F7B3BC241ED771AF77753A88C23B54C63300CC6D3CF1F9DF41F |
| R/native-basic-source-independent-review02/verification.public.json |6936 / F0EFE71473D425EF355F5DAA2EE03224E4601B0729A620840E91B1FFD8CDE4EB |
| R/native-basic-controlled-review02/verification.public.json |8978 / BC3C60EE566CFC9F8F3185F562D7E96774568CD4CAD857A081DCD51D47ED6B16 |

부모 Source false IR5AE0AE3A25BFBCB9A9E6A432F5C44D8CD309B581547E018BB411EBB865E403AD·Control false IR CBB39708D7B7BF49CC613F913886B5EC74BEAA1F64949854B25E9E05124FDE75는 보존했습니다. C/REPORT.ko.md·parent-diff.patch·final-pins.public.json에서 delta·저자 결과를 추적합니다. 기준은 R/native-basic-contract01/CONTRACT.ko.md(SHA17BA2F8C3AFFE80150BCA84FAE133E2286A182C9DCE821144C38D7324B849C4E)입니다.

## 미구현·다음 최소 단위

Basic Root/Source17/device admission, production bounded SDK/process/files transport, paired readonly receiver는 미구현입니다. hash 형식 검사는 실제 원문 body·설치·현재Source17 audit를 대신하지 않습니다. 과거 L2 권한/IR/lease를 Basic으로 변환하지 않습니다.

다음은 evidence 원문bytes/rawpins와 injected clock/native facts를 받는 default-locked Basic admission verifier입니다. current Root/native·fresh Source07 before·전체Source17 audit·Android birth/boot↔PC actor association·prearm/geometry exactshape를 정하고 누락/foreign/stale/crossSHA를 거부합니다. 승인파일 발행·SDK transport 개방은 별도입니다. mapping은 R/native-basic-production-adapter-map01/REPORT.ko.md(SHA ECC776C89C44D4F6CD04AB6B09EEE4833C0B04AF31A46BF2ABE7B119EA92E224)입니다.

helper14 완료·PC child 종료/timeout으로 remote helper 종료·Android UP/neutral·게임/view·반환을 단정하지 않습니다. 미확인은 UNKNOWN/별도 Root 복구·반환 근거 필요로 남기며 자동 force-stop/재시도0입니다. 위 외부 초안 작성·소스/통제 검토에서는 제품/PS/private/환경/SDK/Unity/API/Git·저장소 변경이0입니다. 이 저장소 기록은 검증 문서1개만 추가하며 제품·실행 권한을 변경하지 않습니다. 현재 필요한 사용자 조치나 질문은 없습니다.
