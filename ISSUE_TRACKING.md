# GitHub 이슈와 PR 작업 관리

## 현재 운영 기준 (2026-10-08)

사용자 지시에 따라 이슈의 완료 기준을 먼저 정하고 `codex/issue-번호-설명` 브랜치에서 작업합니다. 한글 커밋과 PR에 이슈를 연결하고, 변경 내용·검증 결과·남은 제한을 리뷰한 뒤 저장소 규칙을 준수하여 병합합니다. 일부 요구만 해결했으면 `Refs #번호`, 이슈 전체 범위가 완료된 경우만 `Closes #번호`를 사용합니다. 진행 중/미검증 이슈를 소급 완료하지 않습니다.

총괄은 이슈·브랜치·커밋·PR 생성과 병합을 담당합니다. Unity Editor 제어는 지정한 한 세션만 수행하며, 다른 세션의 파일과 기존 dirty 변경을 함께 커밋하지 않습니다. 브랜치를 바꿀 때에는 Editor 작업 중 소스가 달라지지 않도록 조정합니다. 코드와 문서의 검증은 실제 실행·외부 진단·미실행을 구분합니다.

GitHub 연결은 PR 작성자와 같은 계정이므로 정식 자기 승인을 만들 수 없습니다. 별도 세션의 코드 검토 및 검증 근거를 PR 리뷰에 기록하고, 보호/필수 리뷰 규칙을 바꾸거나 우회하지 않은 상태에서 병합합니다. 독립 계정 승인이 필수라면 해당 승인을 실제로 받아야 합니다. 아직 전환하지 않은 Unity2022/App Center CI는 #44에서 추적하며 로컬 검증을 GitHub CI 통과로 표시하지 않습니다.

| 이슈 | 작업과 현재 연결 |
| --- | --- |
| [#1 로드맵](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/1) | 기존 완료 목록을 보존하고 아래 단계와 전체 진행률을 관리 |
| [#35 리팩토링](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/35) | Unity6 등 부분 완료, MVC/팝업 책임/폴더 문서 등 나머지는 열림 유지 |
| [#36 Login](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/36) | PlayFab 통합 미완료; PGS 로그인 경로 확인과 별개 |
| [#37 로비 안내·고지 UI](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/37) | [PR #45](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/45), 기존 에셋 전체 라이선스 완료와 구분 |
| [#38 서명 APK·모바일 경기](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/38) | [PR #46](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/46), 실제300초/복귀/초기화와 미검증 전투·성능 구분 |
| [#39 peer timeout 복귀](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/39) | [PR #47](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/47), 수정 EditorClient 실제 강제 종료 재시험/재참가 통과 |
| [#40 정상 퇴장 메뉴](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/40) | MENU·확인·입력 차단 구현 중, 실제 Android 정상 퇴장까지 열림 유지 |
| [#41 Android 최종 검증](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/41) | 최신 APK·전투·물리 동시 입력·10분 성능·종료 회귀 |
| [#42 라이선스·고지](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/42) | 배포 포함 자산·SDK 출처 및 필수 고지 점검 |
| [#43 Play 내부 테스트](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/43) | release AAB·필수 Console 항목·테스터 설치/로그인/경기 |
| [#44 CI 전환](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/44) | Unity6/Google Play 작업에 맞게 기존 자동 빌드·배포 경로 정리 |

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
