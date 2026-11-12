# QA7 결합 소스 준비 · Draft 범위

2026-11-13 07시 마감 전 QA7 결합 소스와 Draft를 준비했습니다. 이후 SDK 예약은 없으며 **QA7 build/export/APK/signing/기기 실행은 모두0**입니다. 운영 코드34와 원장196,2,34는 유지합니다. 새 Android 실행은 후속 검증 범위입니다.

## 지정 기준과 최소 소스 범위

기준은 Draft PR386의 QA6 source `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`(Networked43)입니다. 여기에 main `bea14`의 검증된 다음 prefab 두 개를 **정확한 Git blob 그대로** 반영하고 protocol literal 한 곳만 바꾸는 QA7 Draft를 준비했습니다.

- `Assets/ThirdPartyAssets/Objects/LowPolyFPSLite/Prefabs/Wall_01.prefab`
- `Assets/ThirdPartyAssets/Objects/LowPolyFPSLite/Prefabs/Wall_04.prefab`
- `Assets/Scripts/Managers/NetworkRunnerManager.cs`의 SceneProtocolSuffix: `-intro1-riflecore2-gaitraw1` → `-intro1-riflecore2-gaitraw1-window1`

예상 QA AppVersion은 `sff-q20261022-ladder-v1-intro1-riflecore2-gaitraw1-window1-localqa`입니다. LOCAL_QA 및 설정 복제본의 `-localqa` 추가 로직은 그대로입니다. GamePlayerNetworkData의43개 Networked property, HP 보완·gait/pitch·Raw9 clip/source, Login adapter와 나머지 세션 동작도 이번 준비에서 바꾸지 않습니다. 기존 core39/QA6 protocol과 섞지 않고 실제 양 peer의 cached AppVersion으로 동일 namespace를 확인해야 합니다.

관련 이슈는 #395이며 Draft는 기존 QA6 후보 브랜치를 기준으로 검토합니다.

검토용 D:/codex/worktrees/sudden-force-fps-rights-20261113은 sparse worktree이며 지정 공개 파일만 물리화했습니다. Unity 기동·빌드 대상이나 새 전체 checkout 준비 완료로 보지 않습니다. 실제 빌드는 기존 QA6 입력과 캐시를 보존한 별도 준비 및 새 전체 source manifest를 확보한 뒤 진행합니다. 새 checkout의 raw bytes/source freeze는 새로 확보하며 과거 저장 후 raw SHA나 QA6 freeze를 QA7의 actual pin으로 복사하지 않습니다.

## 이미 있는 근거와 이번 결합의 미검증 범위

두 map prefab는 stopped GUI DesertHouse6곳에서 Before/After 각각84개 query, 수정 후 슬롯36개 root/child hit0·frame48개 차단 유지, 같은 target-head query의 앞 차단1→0을 검증했습니다. Unity 저장에 따른 직렬화 migration과 원 GUID/mesh/material/child 보존 범위는 기존 #392 검증 문서에 따릅니다. 이 근거는 asset 수정과 고정 정적 query의 근거이며, **QA6 base와 결합한 이번 소스의 compile/Game 또는 새 APK 통과가 아닙니다. 기존 QA6 APK(code6)에는 이 map 수정이 없습니다.** 실제 사격 피해와 창문 주변 이동·충돌 품질도 별도입니다.

최근 QA6 순수 proxy 관측은 약4초·230행 ROOT_STOP으로 PoseReady230/230·fault0, context available/source-aligned229/230이었습니다. Ready는 결속 가용성이며 230회 IK 적용 완료를 뜻하지 않습니다. 한 행의 큰 snapshot gap source fallback은 다음 프레임 복귀했지만 시각 pop·적용 hitbox 동시성·HP 피격 품질까지 확정하지 못했습니다. LD3는 설치/UI 범위만 관측했고 black 상태·manager의 C0000005 한계가 남아 정상 QA6 경기/안정된 VM 실행의 근거가 아닙니다. 이 두 자료를 QA7 검증 준비의 제한으로 유지하며 QA7 성공으로 승격하지 않습니다.

## 후속 순서

1. Root가 지정 세 경로의 최소 diff/Git blob과 protocol 한 literal을 확인해 Draft source를 만들고, 정확 head에서 fresh compile을 수행합니다. 현재 결합 compile은0입니다.
2. 새 외부 export 준비에서 **code7 / version `1.0.197-qa7`을 후보로 검토**한 뒤 실제 version 설정·새 공개 source freeze·export를 별도로 결속합니다. 현재 버전 예약/설정/export는 없습니다. 운영 원장을 QA 후보 표시 때문에 갱신하지 않습니다.
3. 실제 산출물이 생긴 뒤 signed 검증과 전체 payload·package/version/protocol/source 결속을 확인합니다. 과거 code6의 APK/인증/검증 결과를 code7 actual로 복제하지 않습니다.
4. Phone/LD 실행 전 필수 ADB transport·설치 대상 세대·각 앱 version/code를 실제로 준비합니다. **native helper7·phone gate7은 아직 없으며 기존6 helper의 version gate를 자동 완화하지 않습니다.** 필요하면 별도 최소 파생·검토·실제 도구 검증을 거칩니다. LD3의 기존 한계도 이 단계에서 분리해 다룹니다.
5. 같은 QA7 cached protocol의 새 peers로 정상 Lobby/Ready/Game을 검증합니다. 창문을 통한 실제 정상 입력 사격·피해, frame 차단 유지와 필요한 이동/시각 품질을 확인하고 정상 결과·복귀·자원/공개 source 반환까지 기록합니다. 미실행·실패는 통과에 합산하지 않습니다.

기존 map 정적 근거는 재사용하되 새 결합에서만 필요한 실제 compile·산출물·동일 peer 검증을 남깁니다. 이번 작업은 소스 결합과 정적 검토까지이며 추가 SDK/기기 작업을 시작하지 않습니다.
