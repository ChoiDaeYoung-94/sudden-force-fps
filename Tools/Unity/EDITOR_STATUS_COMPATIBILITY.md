# Pipeline0.8의 Unity CLI 상태 조회 (#275)

현재 프로젝트의 `com.unity.pipeline 0.8.0-exp.1`은 상태 조회 명령을 `editor_status`로 등록합니다. CLI1.0.0-beta.14의 일반 `status`는 정상 상태를 반환하면서도 Editor Console에 `No command named 'editor status'` 오류를 남길 수 있습니다. 이 조합의 후속 조회는 등록된 이름을 직접 사용합니다.

```powershell
& '<설치된 Unity CLI>' command editor_status --project-path '<실제 활성 검증 프로젝트>' --format json
```

CLI 종료 코드와 성공 envelope를 먼저 확인하고 `data.result.projectPath`가 의도한 프로젝트인지 대조합니다. 실패하면 반복 호출하거나 다른 Editor로 대상을 바꾸지 않습니다. 설치 패키지의 명령 등록 계약이 바뀌면 이 문서의0.8 가정을 다시 확인합니다. 현재 대응을 위해 UPM·Unity 엔진·CLI를 일괄 업그레이드하거나 frozen helper를 수정하지 않습니다.

## 원인과 검증

P2와 P11의 manifest/lock/실제 PackageCache는0.8.0-exp.1로 같았습니다. `Editor/Commands/EditorStatusCommand.cs`는 `editor_status`를 등록하며, `Runtime/Common/BasePipelineServer.cs`는 정확한 요청명을 찾지 못하면 Console에 오류를 남깁니다. CLI의 공개 호환 계약에는 새 이름과 이전 이름의 대응이 있습니다. [공식 CLI 호환 설명](https://github.com/Unity-Technologies/unity-agent-plugin/blob/main/skills/unity-cli/references/integration-advanced.md)을 함께 참고합니다.

이 계약은 새 이름의 조회 오류와 최종 ready 응답이 함께 나타나는 것을 설명합니다. 실제 일반 status의 HTTP 요청 전체나 어떤 fallback이 최종 응답했는지를 추적한 것은 아닙니다. Console 오류를 게임 컴파일 실패로 바꾸거나, 반대로 기존 BuildReport의 오류 수를0으로 고치지 않습니다.

2026-10-27 실제 P11에서 다음 대조를 수행했습니다.

| 항목 | 결과 |
| --- | --- |
| 별도 소유·준비 관측 | PID19952/시작05:52:01.6252214Z, Windows64·D3D11, nonplay/noncompiling/nonupdating/nonbuilding, compilationFailed=false |
| 등록명 직접 조회 | `editor_status`, CLI 종료0·success=true, ready/stopped·컴파일/업데이트 false, 대상 projectPath 일치 |
| 알려진 오류 prefix 관측 | 06:01:04.8085815Z~06:01:06.904746Z, 60초 만료 전, `ExecuteCommandByName: No command named` 신규0건 |
| 정리 | 자신의 log/update delegate 해제와 임시 AppDomain 데이터 삭제 확인. 원문 메시지·stack 저장0, Console clear0 |

위 시각은 UTC이고 PID는 당시 증거이지 다음 호출의 고정 대상이 아닙니다. 오류0은 해당 짧은 구간의 알려진 prefix 계수이며 모든 Console 오류·모든 동시 요청·장시간 안정화의 증거가 아닙니다. 제품·UPM·설정 저장과 새 빌드는 수행하지 않았습니다.

외부 근거: `D:/codex/validation/sudden-force-fps/20261027-work/`의 `unity-cli-status-contract-audit01/`, `p11-reload-editor-readiness01.public.json`, `p11-legacy-editor-status-actual01.public.json`, `p11-status-compatibility-probe-install01.public.json`, `p11-status-compatibility-probe-finish01.public.json`입니다.

## 상태 응답의 한계

등록명 응답은 `status`, `compiling`, `domainReloadInProgress`, `playMode`, `lastHeartbeat`, `projectPath`, `unityVersion`을 제공합니다. blocked 응답은 별도 dialog 정보를 가질 수 있습니다.0.8에서 `domainReloadInProgress`의 실제 값은 `EditorApplication.isUpdating`이며 CLR 도메인 재로드 전체 계측이 아닙니다.

이 응답에는 `compilationFailed`, PID/프로세스 시작 시각, 게임 assembly MVID가 없습니다. ready만으로 게임 컴파일 성공·프레임 진행·빌드 성공을 판정하지 않으며 필요한 작업에서는 별도의 fresh 소유·어셈블리·컴파일·씬 검사를 유지합니다. 빌드 중에는 상태 명령을 폴링하지 않고 소유한 빌드 영수증을 확인합니다.

이 변경은 현재 고정 패키지의 조회 경로에 한합니다. 코드34의 보존된 오류2·경고68, 단일 Unity AAB 빌드 안정화, Chrome 접근 승인이나 네이티브 창 활성화 문제를 해결 처리하지 않습니다. 부모 #66은 유지합니다.
