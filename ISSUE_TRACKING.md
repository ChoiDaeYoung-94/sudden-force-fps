# GitHub 이슈와 PR 작업 관리

## 현재 운영 기준 (2026-10-16)

사용자가 연구의 운영 규칙을 이 프로젝트에도 적용하도록 요청했습니다. 관련 이슈→작업별 codex 브랜치→의미 있는 한글 커밋·푸시→목적 하나의 PR→독립 검토→필수 검사 통과 후 별도 승인 없는 main Squash merge로 마무리합니다. 본인 APPROVE·보호 규칙 우회는 하지 않습니다. 구체적인 실행·검증·세션·보고 규칙은 [AGENTS.md](AGENTS.md)를 따릅니다. 광고/결제 제외·GPT-6.1 Sol High·실기기 검증 연기 합의는 유지합니다.

브랜치 정리: 최신 origin/main17e7d73의 조상이며 고유 미반영 커밋0인 완료 브랜치만 독립 확인해 로컬18개·원격39개를 정리했습니다. 삭제 직전 원격 SHA를 대조하고 해당 SHA 조건의 원자적 삭제를 수행했으며 main을 쓰거나 이력을 재작성하지 않았습니다. 코드18 미병합 브랜치, main·origin/HEAD와 미반영 과거 개발 origin/issue/35·origin/issue/36은 보존했습니다. 사용 중 Unity 워크트리·기존 dirty·현재 HEAD는 정리 전후 동일합니다. 현재 운영 규칙 기록은 별도 codex/issue-1-workflow-cleanup 브랜치에서 진행하며 코드18 배포 기록 PR과 목적을 구분합니다.

최신 #66 후속은 코드15 네이티브 완료/Gradle 옵션 게이트 실패, 코드16 재부팅 중 결과 미확정, 코드17 CMake 경로 게이트 실패와 코드18 별도 Gradle 생성·기존 키 서명·정적 검증입니다. 원장194,10,18·다음 새 코드19 이상이며 전체 payload129개 서명 검증과 독립 검토를 마친 뒤 총괄이 Play에 직접 업로드·출시해 내부 테스트 release5/code18·16:37 게시·내부 테스터 제공을 확인했습니다. #42의 보완 고지는 새 배포 AAB에 정확히 포함됐지만 남은 원본 대응·기기 표시는 미완료입니다. 실기기 검증 전반은 사용자 결정으로 PC 연결 가능 시점까지 후순위로 두며 #41/#43을 완료 처리하지 않습니다. GitHub 연결 도구가 현재 제공되지 않아 이번 후속 이슈·PR 갱신은 연결 복구 후 수행하며 로컬 기록·한글 커밋·푸시와 구분합니다.

아래는 코드13·14 마감 당시의 기록입니다.

코드13·14의 고지 반영 AAB 시도는 각각 Linker·APIUpdater 접근 위반으로 실패해 #66에서 추적합니다. 원장194,6,14와 다음 코드15 이상을 유지하며 AAB·새 서명·업로드0입니다. 고지 보완 PR83의 소스 반영과 코드12 실제 배포를 구분하고 #42/#43의 후속 배포·검증 기준은 열림 유지합니다. 10월16일 사용자 결정으로 #41의 발열·장시간 FPS/메모리·저사양 성능 검증은 후속으로 미루고 이번 내부 테스트 완료 조건에서 제외합니다. 최신 측정 미실행과 과거 측정 결과를 구분하며 성능 측정용 USB 연결은 지금 요청하지 않습니다.

사용자 지시에 따라 이슈의 완료 기준을 먼저 정하고 `codex/issue-번호-설명` 브랜치에서 작업합니다. 한글 커밋과 PR에 이슈를 연결하고, 변경 내용·검증 결과·남은 제한을 리뷰한 뒤 저장소 규칙을 준수하여 병합합니다. 일부 요구만 해결했으면 `Refs #번호`, 이슈 전체 범위가 완료된 경우만 `Closes #번호`를 사용합니다. 진행 중/미검증 이슈를 소급 완료하지 않습니다.

총괄은 이슈·브랜치·커밋·PR 생성과 병합을 담당합니다. 작업 종료 때 필요한 인계를 남기고 역할이 끝난 보조 세션은 아카이브하며, 다음 작업에 필요한 세션만 새로 생성합니다. Unity Editor 제어는 지정한 한 세션만 수행하며, 다른 세션의 파일과 기존 dirty 변경을 함께 커밋하지 않습니다. 브랜치를 바꿀 때에는 Editor 작업 중 소스가 달라지지 않도록 조정합니다. 코드와 문서의 검증은 실제 실행·외부 진단·미실행을 구분합니다.

GitHub 연결은 PR 작성자와 같은 계정이므로 정식 자기 승인을 만들 수 없습니다. 별도 세션의 코드 검토 및 검증 근거를 PR 리뷰에 기록하고, 보호/필수 리뷰 규칙을 바꾸거나 우회하지 않은 상태에서 병합합니다. 독립 계정 승인이 필수라면 해당 승인을 실제로 받아야 합니다. 아직 전환하지 않은 Unity2022/App Center CI는 #44에서 추적하며 로컬 검증을 GitHub CI 통과로 표시하지 않습니다.

| 이슈 | 작업과 현재 연결 |
| --- | --- |
| [#1 로드맵](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/1) | 기존 완료 목록을 보존하고 아래 단계와 전체 진행률을 관리 |
| [#35 리팩토링](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/35) | Unity6 등 부분 완료, MVC/팝업 책임/폴더 문서 등 나머지는 열림 유지 |
| [#36 Login](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/36) | PlayFab 통합 미완료; PGS 로그인 경로 확인과 별개 |
| [#37 로비 안내·고지 UI](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/37) | [PR #45](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/45), 기존 에셋 전체 라이선스 완료와 구분 |
| [#38 서명 APK·모바일 경기](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/38) | [PR #46](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/46), 실제300초/복귀/초기화와 미검증 전투·성능 구분 |
| [#39 peer timeout 복귀](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/39) | [PR #47](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/47), 수정 EditorClient 실제 강제 종료 재시험/재참가 통과 |
| [#40 정상 퇴장 메뉴](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/40) | 구현·기존 검증 완료로 닫힘. 최신 APK 사용자 방 퇴장 보고와 실제 두 피어 경기 전체 회귀는 구분 |
| [#41 Android 최종 검증](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/41) | Note20 Ultra·S25 Ultra 사용자 수동 경기·동시 이동/시점/사격·입력 해제·재장전/사망/리스폰·종료/재참가 정상 보고. 입력 전수·비정상 종료 등 남음. 발열·장시간 FPS/메모리·저사양 성능 측정은 사용자 결정으로 후속 연기 |
| [#42 라이선스·고지](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/42) | 배포 포함 자산·SDK 출처 및 필수 고지 점검 |
| [#43 Play 내부 테스트](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/43) | code12 내부 release4 제공과 실제 참여 링크 직접 확인. 두 기기 Store 업데이트·로그인·로비·방 참가 사용자 정상 보고. 최신 한 경기/재참가·남은 품질/출처 기준은 열림 |
| [#44 CI 전환](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/44) | Unity6/Google Play 작업에 맞게 기존 자동 빌드·배포 경로 정리 |
| [#66 새 PC 빌드 환경](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/66) | 오류0 Gradle 생성(PR74)과 실제 ARM64 IL2CPP·무서명 APK 패키징 성공. 간헐 안정성·서명 배포·실제16KB 등 남음 |
| [#75 빌드 성공 판정](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/75) | 오류0·새 nonempty APK/AAB 및 원장 예약 전 출력 충돌 보호 구현, Win64 Editor 컴파일·경계8사례 통과. [PR #76](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/76) 병합 완료·이슈 닫힘 |

2026-10-16 최신 결과는 Console 직접 관찰과 사용자 수동 보고를 구분합니다. 코드12 Store 기본 흐름과 MENU/리스폰 후 held FIRE의 새 입력 요구는 사용자 확인이며 직접 기기 계측이 아닙니다. 광고 없음 선언 저장·출하 DEX의 지정 네임스페이스 점검을 보완했으며 #41–43·66은 남은 기준 때문에 열림 유지합니다. #37–40·75는 기존 완료 상태이고 새 자동 CI·#35 잔여 리팩토링·#36 PlayFab·공개 배포 설정 전체는 이번 내부 테스트 완료의 선행 조건으로 확대하지 않습니다. 아래 이전 날짜의 조사·완료 체크는 당시 기록입니다.

## 최초 조사 기록 (2026-10-07)

2026-10-07 GitHub 공개 API에서 열린 이슈 3개와 닫힌 이슈 목록을 확인했습니다. 이 문서는 현재 구현과 검증을 연결하며, GitHub 이슈 상태를 변경하지 않습니다. 완료로 닫기 전에는 해당 이슈의 전체 요구사항과 실제 검증 결과를 다시 확인합니다.

| 기존 이슈 | 현재 연결과 남은 작업 |
| --- | --- |
| [#35 프로젝트 리팩토링](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/35) | Unity 6 전환·패키지·TMP 복구, null 검사 및 Runner 수명주기 정리를 진행했습니다. MVC, 공용 팝업과 씬 Canvas 책임 분리, Canvas 기준 해상도 1920, 폴더 설명 문서는 아직 전체 완료가 아닙니다. 기존 TMP→Text 요청은 모바일 HUD·한글·실제 사용 목적을 검토한 뒤 필요한 범위에 적용하며 일괄 교체하지 않습니다. |
| [#36 Login](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/36) | 요구사항은 PlayFab 적용입니다. Android Google 로그인 코드의 플랫폼 가드와 PC 검증 진입을 수정했지만 PlayFab 로그인 완료를 의미하지 않습니다. origin/issue/36의 초석·로그인 테스트 커밋을 참고해 기존 자격 증명·초기화·실패·재시도 경로를 감사하고 필요한 부분만 이식합니다. 실제 Android 및 Play 설치본 인증 검증은 남아 있습니다. |
| [#1 SuddenForce-FPS Roadmap](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/1) | UI·사운드 적용과 최적화, #35를 추적합니다. PROJECT_PLAN.md의 화면·조작, Android 성능, 라이선스 및 배포 단계에 연결합니다. 무료 에셋 확보만으로 실제 VFX·사운드 연결이나 최적화를 완료 처리하지 않습니다. |

기존 origin/issue/35에는 Unity 6 전환과 GPGS 2.1.0 변경이 있으며, origin/issue/36에는 PlayFab 초석과 빌드 후 로그인 테스트가 있습니다. 현재 검증된 SDK·패키지와 중복 또는 충돌 여부를 확인하며 전체 브랜치를 무조건 병합하지 않습니다.

닫힌 이슈도 회귀 기준으로 유지합니다. #33 플레이어·게임 흐름, #30·#27·#21 방, #15 로비, #11 Fusion 설정, #6 Google 로그인·PlayFab, #19 QA, #18 경고·버그, #5 CI/CD의 현재 Unity 6 동작을 다시 검증합니다. 특히 #33이 닫혔어도 현재 이동·전투·로컬 카메라는 미완성이므로 완료 근거로 사용하지 않습니다.

현재 검증과 커밋 연결:

- #35 관련: Unity 의존성·결제 제거 `7bd49e8`, 에셋·TMP `fd30b80`, 플랫폼 가드 `4ece9e6`. 전체 #35 완료는 아닙니다.
- #11 및 방·로비 회귀: Fusion SDK `c40ca32`, 네트워크 수명주기 `87a7fbe`, 상태 UI `ba81b18`.
- #1 관련: 라이선스 확인 Kenney 에셋 `4c865ac`; 실제 효과 연결은 후속 작업입니다.
- #19·#30·#33 회귀 검증: Windows 빌드 도구와 실제 두 클라이언트 검증 `fb8ae16`. 카메라 실패·전투 및 Android 미검증을 문서에 구분합니다.

새 기능 커밋과 PR에는 관련 이슈 번호를 연결합니다. 부분 해결에는 Refs #번호를 사용하고, 전체 요구사항과 검증이 끝난 경우에만 완료를 나타내는 표현을 사용합니다. 기존 이슈를 이번 프로젝트 범위와 별개로 누락하거나 자동 종료하지 않습니다.
