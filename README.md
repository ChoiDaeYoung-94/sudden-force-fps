# Sudden Force FPS

## 코드20 무서명 AAB 검증 완료 (2026-10-17, #66/#42/#43)

코드20(1.0.195)의 공식 증분 export는 오류0·경고2·86.786초, 별도 offline Gradle 패키징은 exit0·59초로 완료했습니다. 무서명 AAB는92,829,388B/SHA256 `D1C7ED2D5A13940E6AD0CBB804A0C4C75B7738298C735475B163CF89C9EF7F79`이며 ZIP/bundletool·패키지·min25/target36·비개발·ARM64 SO7개 LOAD16KB·새 고지709,379B/F35330D344D7209A617A8CFC919E2A2FDC26E6BC30090E3EAA26B6D76EBB9790 본문 일치를 확인했습니다.

시작 기준3,879개 소스/설정은 예약 원장195,1,20만 달라지도록 복원했고 신규 backup2/meta는 외부 보존했습니다. 코드19 이전 임시 성능파일4개 복구를 뜻하지 않습니다. 다음 새 예약은21 이상입니다. 현재 코드20 서명·Play 업로드는 대기, 현재 제공본은코드18입니다. 기존 코드18/19 산출물은 보존합니다. RELRO 경고4건·실제16KB/기기 확인은 미검증으로 유지하고 실기기 후순위·성능측정 후속 합의와 전체 약92% 작업 기준 추정을 유지합니다.

아래 항목의 원장·다음 코드·미완료 표현은 각 시점의 이전 기록입니다.

## 후속 배포 고지 준비 (2026-10-17)

2026-10-17 추가: Photon Fusion 공급자 선언과 실제 코드19 생성 입력에 대응하는 UnsafeCollections 메모리 보조 부분의 MIT 고지1건을 소스에 추가했습니다. 새 소스709,379B/SHA256 `F35330D344D7209A617A8CFC919E2A2FDC26E6BC30090E3EAA26B6D76EBB9790`의 기존707,770B prefix를 바이트 그대로 보존했고, 공식 고정 LICENSE 전문·pager197페이지 원문 round-trip·Editor TextAsset 바이트와 에셋 GUID 보존을 확인했습니다. 앱 UI·후속 AAB 포함·기기 표시는 미검증입니다. 코드19은 이전707,770B 고지의 검증용 무서명 AAB로 보존하며 서명/업로드0, 현재 Play 제공본은 코드18입니다. 원장195,0,19·다음 새코드20이상·전체 약92% 추정과 기기 검증 후순위 합의를 유지합니다.

상세 근거는 [에셋·고지 기록](THIRD_PARTY_ASSETS.md)을 따릅니다. 아래 코드19 빌드 결과와 이전 배포 이력은 당시 고지에 대한 검증입니다.

## 코드19 빌드 진행 (2026-10-17)

2026-10-17 코드19: Unity6000.3.25f1의 새 공식 Gradle export가 오류0·경고71·448.111초로 완료됐습니다. 이어 별도 C1 JVM·명시4GiB·processor1/worker1·parallelfalse 및 IL2CPP/Bee count1 조건의 offline Gradle bundleRelease가 exit0으로 끝나, 버전1.0.195/code19의 무서명 AAB 92,829,022B/SHA256 `EE26786D896B590FE5CDA48597BA240708EBBB7D2E18241991BE3A89C4C46693`를 생성했습니다. ZIP/bundletool·패키지·min25/target36·비개발·ARM64 SO7개 LOAD16KB와 새 고지707,770B의 본문 바이트 일치를 확인했습니다. 원장 `195,0,19`는 유지하며 다음 새 코드는20 이상입니다. **코드19 서명·업로드·기기 실행은 아직 미검증이고 현재 Play 제공본은 코드18**입니다. export와 별도 패키징의 성공이며 Unity의 단일 AAB 호출 안정화나 과거 충돌의 근본 해결로 확대하지 않습니다.

후속 SDK 조사에서 UnsafeCollections 메모리 보조 부분의 공급자 MIT 고지1건을 추가로 확인했습니다. 따라서 코드19은 검증용으로 보존하고 서명·Play 업로드를 진행하지 않습니다. 공식 원문을 포함한709,379B 후보의 prefix 보존·pager197페이지 검사를 완료했으며, 소스 보완 후 새코드20이상에서 실제 고지 포함과 배포를 이어갑니다. 코드19의 생성 성공과 배포 준비 완료를 구분합니다.

상세 검증과 제한은 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)를 따릅니다. 아래 코드18과 이전 기록은 각 시점의 결과입니다.

> Photon Fusion 2 기반의 호스트·클라이언트 멀티플레이 Unity 3D FPS 프로젝트입니다. 로비에서 방을 만들고 팀을 선택해 DesertHouse에서 한 판을 진행합니다.

## 현재 상태 (2026-10-17 정리)

최신 결과는 **코드18 서명 AAB 생성·검증 및 Google Play 내부 테스트 release5 제공 완료**입니다. 총괄이 지정된 Console 탭에서 직접 업로드·출시했고 10월16일16:37의 내부 테스터 제공 상태를 확인했습니다. 코드15에서 Bee 작업 수를1로 제한해 새 고지 포함 데이터·ARM64 네이티브 컴파일/링크를 완료했으나 Unity의 실제 Gradle JVM 옵션 게이트가 차단해 전체 빌드는 Failed였습니다. 재부팅으로 결과를 확인할 수 없는 코드16과 CMake 경로 검사로 중단된 코드17의 예약을 보존하고, 실제 `.utmp` 경로 검사와 자식 JVM C1/worker1 조건을 통과한 별도 Gradle 실행으로 코드18을 생성·서명했습니다. 서명본92,840,760B의 전체 payload129개가 기존 업로드 인증서로 검증됐고 서명 전후 payload129개 바이트 차이0입니다. 원장은 `194,10,18`이고 다음 새 코드는19 이상입니다. clean Unity 전체 빌드 성공이나 재부팅 원인 해결로 확대하지 않습니다.

코드18의 패키지·버전18·비개발·ARM64·ZIP·bundletool 및 SO7개 LOAD16KB를 확인했고, 보완 고지694,428B가 소스와 정확히 일치합니다. 지정 광고·결제 SDK를 조사한 코드12의 DEX와도 바이트가 같습니다. 무서명 검증 파일은 Play 업로드 대상이 아니며 실제 업로드한 것은 서명본입니다. 출시 당시 Console의 가독화 파일·네이티브 디버그 기호 미업로드 권고2건 중 네이티브 심볼은 후속으로6/7개 대응 ZIP을 직접 업로드해 코드18의 첨부 완료 표시를 확인했습니다. nanosockets 심볼·핵심 Unity3개 소스 행번호·매핑 파일과 Console 권고 전체 해소는 미확인 범위로 유지합니다. GNU_RELRO 정적 경고4건과 실제16KB 실행 등 미검증 범위도 보존합니다. 사용자 결정에 따라 실기기 검증 전반은 기기를 작업 PC에 연결해 직접 확인할 수 있을 때까지 후순위로 두고, 코드18 Store 설치·실행은 아직 검증하지 않았습니다.

2026-10-17 소스 고지 보완: UniRx의 역사적 Rx.NET 차용 코드와 Photon 공급자 선언에 따른 Simple Scene BVH 안내·Apache2.0 전문을 추가했습니다. 기존 고지 전체 문구와 Git 정규화 원본 prefix를 보존한 새 소스707,770B의 pager196페이지 원문 보존 검사를 통과했습니다. 현재 배포 코드18은 기존 고지 그대로이며 이번 추가의 새 빌드·서명·업로드·예약은0입니다. 새 고지의 Editor/Android 표시와 후속 AAB 포함은 미검증입니다. 원장194,10,18·다음 코드19 이상을 유지하며 상세는 [에셋·고지 기록](THIRD_PARTY_ASSETS.md)을 따릅니다.

아래 코드13·14 및 이전 배포 기록은 당시 결과입니다.

고지 보완 후 새 AAB 시도는 코드13·14 모두 실패했고 배포된 코드12는 그대로입니다. 코드13은 UnityLinker/.NET8 접근 위반, 코드14는 프로세서 수1 조건에서도 APIUpdater의 MovedFromExtractor 접근 위반으로 실패했습니다(각 오류2·경고71, AAB0). 원장 `194,6,14`를 보존하며 다음 새 코드는15 이상입니다. 개별 Linker 단독 검사의 성공을 전체 빌드 안정화로 확대하지 않습니다. 상세는 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)의 코드13·14 기록을 따릅니다.

`1.0.194 / versionCode 12`는 사용자 업로드 후 Google Play 내부 테스트 release4로 제공 중입니다. 10월16일 총괄이 Console의 패키지·코드12·내부 테스터 제공 상태를 직접 확인했습니다. 실제 참여 링크는 [Sudden Force 내부 테스트](https://play.google.com/apps/internaltest/4701524132186452960)입니다. 기존 앱·데이터를 보존하고 등록된 테스터 계정으로 참여한 뒤 Play 스토어에서 업데이트합니다.

Galaxy Note20 Ultra·S25 Ultra의 Store 업데이트→로그인→로비→방 참가를 요청한 질문에 사용자는 위 링크로 테스트가 잘 됐다고 답했습니다. 이어 MENU 열기/닫기와 사망/리스폰 중 사격을 계속 누른 두 상황에 대해서도 새로 눌러야 사격된다고 답해 의도한 입력 차단을 확인했습니다. 사용자 수동 보고이며 기기별 화면·설치 코드/인증서·Android 버전·최신 한 경기 종료/재참가·FPS를 직접 계측한 결과는 아닙니다.

코드12는 기존 검증 export와 프로세스 한정 C1 JVM 조건으로 생성·서명했습니다. AAB92,835,970B의 인증서·payload127개 전체 서명과 원본 바이트 일치, manifest·ARM64·비개발·ZIP·bundletool·고지 본문을 확인했습니다. 당시 원장194,4,12와 이후 코드13·14 실패 예약을 구분합니다. clean Unity 재빌드와 이전 실패의 근본 원인 해결을 확정하지 않습니다.

이전 코드11 시도는 dry-run 통과 후 실제 Gradle JVM의 C2 스레드 충돌로 실패했습니다. 당시 AAB·서명0과 원장194,3,11 예약을 보존했고 이후 코드12의 C1 조건 생성·서명으로 이어졌습니다.

10월15일 새 PC의 비개발 ARM64 AAB 빌드는 코드10을 예약한 뒤 Player 스크립트 컴파일의 APIUpdater/CLR 오류로 실패했습니다(오류3·경고71, AAB 미생성). 원장 `194,2,10`을 보존하며 다음 코드는11 이상입니다. 사용자 지시로10부터 사용했으며 Play 최대 코드를 직접 재조회한 결과는 아닙니다. 이전 오류0 Gradle export 기반의 배포 경로를 별도 검토 중입니다. 아래 기존 Play APK·무서명 APK 성과와 이번 새 서명 빌드 실패를 구분합니다.

`1.0.194 / versionCode 5`는 2026-10-11 Google Play 내부 테스트에 출시했습니다. 전달한 Play 서명 APK는 10월 14일 사용자 수동 테스트에서 Galaxy Note20 Ultra·S25 Ultra의 두 피어 경기 진입, 동시 이동·시점·사격과 입력 해제, 재장전·사망·리스폰, 경기 종료·로비 복귀·새 방 참가가 정상이라고 보고됐습니다. 직접 APK 설치 결과이며 Play 스토어 설치/업데이트 결과와 구분합니다.

새 PC에서는 10월 14일 실제 ARM64 IL2CPP 컴파일과 검증용 무서명 APK 패키징을 완료했습니다. 이 APK는 설치·Drive 전달·Play 업로드 대상이 아닙니다. SO7개 LOAD16KB 정렬을 확인했지만 RELRO 정적 경고4건과 실제16KB 기기 검증은 남았습니다. SO는 압축 저장돼 zipalign 통과를 비압축 SO의16KB 직접 배치 검증으로 확대하지 않습니다. 오류·기존/누락/빈 산출물의 빌드 성공 판정은 [PR #76](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/76)으로 보강했고 [#75](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/75)는 닫혔습니다. 수정 Editor 컴파일과8개 함수 경계 사례가 통과했으며 서명 빌드·batch 통합 실행과는 구분합니다.

남은 핵심은 clean 빌드 충돌 원인·재현성, 배포 포함 에셋의 원본 대응·조건부 SDK 고지 확인과 코드18 Store 설치·경기 종료/재참가·비정상 연결 종료·동시 입력 경계·실제16KB 검증입니다. 새 고지 포함 AAB의 내부 테스트 배포는 완료했으며 실기기 항목은 PC 연결 가능 시점까지 후순위로 유지합니다. 10월16일 사용자 결정으로 발열·장시간 FPS/메모리·저사양 성능 측정은 후속으로 미루며 이번 내부 테스트 마무리의 완료 조건에서 제외합니다. 기존60 FPS 목표·저사양30 FPS 허용 기준과 과거 측정 기록은 보존하고 최신 측정 통과로 처리하지 않습니다. Console의 광고 없음 선언은 저장했고 검토 전송 전입니다. 광고·인앱 결제는 제외하며 공개 프로덕션 출시·새 자동 CI·PlayFab 통합·스킬/장비 확장은 별도 범위입니다. 전체 **약92%는 작업 기준 추정**이며 시간 비율이나 최종 품질 점수가 아닙니다. 시험 연기만으로 진행률을 올리지 않습니다.

이전 APIUpdater·shader·shutdown 진단의 실패와 복원 이력은 [배포 점검표](ANDROID_RELEASE_CHECKLIST.md)에 보존합니다. 일부 빌드 성공으로 간헐 충돌 원인 해결·재발 방지를 확정하지 않습니다. 과거 외부 백업·미커밋 변경의 복구나 서명 비밀번호 이전을 가정하지 않습니다. HUD 이미지7개는 사용자가 직접 제작 또는 AI 생성했다고 확인했으며 남은 출처는 [에셋 기록](THIRD_PARTY_ASSETS.md)을 따릅니다. 최신 검증 범위는 [멀티플레이 기록](MULTIPLAYER_TEST_PLAN.md)과 배포 점검표를 우선합니다.

## 주요 구현

| 기능 | 내용 및 관련 코드 |
| --- | --- |
| 멀티플레이 | Fusion의 ClientServer 로비, Host/Client 방 접속, 네트워크 입력·상태 동기화, 호스트 권한의 사격·피해 판정 · [NetworkRunnerManager.cs](Assets/Scripts/Managers/NetworkRunnerManager.cs) |
| 방 목록 UI | 항목을 재사용하는 스크롤 뷰 · [RoomManage.cs](Assets/Scripts/UI/RecyclableScrollView/RoomManage.cs) |
| 전투 | 이동·시점·달리기, 총기 반동, 부위별 피해, 탄약·재장전, 사망·리스폰 · [GamePlayerNetworkData.cs](Assets/Scripts/Creature/Player/GamePlayerNetworkData.cs) |
| 경기·HUD | 팀 점수·타이머·KD·킬 피드·스코어보드·결과 화면 · [NetworkMatchState.cs](Assets/Scripts/Game/Match/NetworkMatchState.cs) |
| 씬·복귀 | 일반 씬 전환과 Fusion 경기 씬 로드, persistent Managers가 종료된 Runner를 정리하고 새 로비 Runner를 생성 · [Managers.cs](Assets/Scripts/Managers/Managers.cs) |
| 로그인 | Android Google Play Games Services v2 인증·취소/실패/30초 시간 초과·재시도 처리. 기존 업로드 키 APK의 실기기 인증과 전달 Play 서명 APK의 사용자 로그인 보고 확인. 스토어 설치 경로는 별도 · [Login.cs](Assets/Scripts/UI/Login.cs) |

Fusion은 방 접속뿐 아니라 입력 전달, 권한에 따른 상태 복제, Hitbox 지연 보정 쿼리와 경기 씬 전환에 사용합니다. 지연·손실 환경의 전체 품질 검증은 별도이며, 상세 통과 범위와 미검증 항목은 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)에 구분합니다.

## 개발 목표

- [x] Photon Fusion 기반 멀티플레이
- [x] Google Play Games Services v2 로그인 경로 구현
- [x] 기존 업로드 키 APK의 Android 실기기 인증·두 피어 모바일 경기 확인(최종 품질 검증과 구분)
- [ ] 최신 버전 Google Play 테스트 배포·테스터 설치 검증
- [ ] 스킬 시스템 및 장비 시스템 구현
- [ ] 프리팹 구성과 재사용 방식 개선

시연 영상과 Google Play 공개 링크는 준비 중입니다.

스킬·장비는 기본 경기 이후의 별도 단계입니다. 광고·인앱 결제는 현재 및 향후 작업 범위에서 제외합니다. PlayFab 로그인 통합도 완료된 기능으로 표시하지 않습니다.

## 개발 환경

| 항목 | 내용 |
| --- | --- |
| 엔진 | Unity 6000.3.25f1 LTS |
| 언어 | C# |
| 네트워크 | Photon Fusion 2.0.13 |
| 빌드 대상 | Android APK·AAB, Windows x64 개발/QA 클라이언트 |
| Android 빌드 확인 | IL2CPP, API 25 이상·target/compile 36. 기존 개발 APK와 별도로 내부 테스트 code5는 ARM64 |

## 프로젝트 열기

1. 저장소를 복제합니다.

   ```bash
   git clone https://github.com/ChoiDaeYoung-94/sudden-force-fps.git
   ```

2. Unity Hub에서 복제한 `sudden-force-fps` 폴더를 프로젝트로 추가합니다.
3. **Unity 6000.3.25f1**로 프로젝트를 엽니다. Android 빌드에는 Android Build Support와 SDK/NDK/OpenJDK가 필요합니다.
4. `Assets/Scenes/Login.unity`를 열고 로그인 및 네트워크 설정을 확인합니다.

Photon 서비스 설정이 유효해야 실제 로비·방에 연결됩니다. Android 인증에는 기존 PGS 프로젝트의 패키지·서명 인증서 연결과 테스트 계정 권한이 필요합니다. 저장소만 복제한 새 계정에서 인증 성공을 보장하지 않습니다.

Editor와 Windows Standalone은 개발/QA용으로 Google 인증을 생략하고 로비로 진입합니다. 이 경로의 성공은 Android PGS 인증 성공을 의미하지 않으며, Android 빌드는 실제 Google 인증 경로를 사용합니다.

### 두 피어 실행

1. Editor에서 `Login.unity`를 열고 Play를 실행합니다. 두 번째 피어는 별도의 Windows 개발 Player 또는 인증 가능한 Android 기기로 준비합니다.
2. 각각 닉네임을 설정합니다. 한 피어가 `Create Room`에서 방 이름과 `DesertHouse`를 선택해 공개방을 만듭니다.
3. 다른 피어가 방 목록에서 참가하고, 서로 다른 팀을 선택합니다. Client가 `READY`를 누른 뒤 Host가 `START`를 누릅니다.
4. 결과 화면의 로비 복귀 버튼으로 돌아가 새 방에 재참가할 수 있습니다. 경기 중 Host 연결이 종료되면 Client는 새 Runner로 로비에 자동 복귀합니다. 실제 PC 양방향 검증 근거는 검증 기록의 P01–P06에 있습니다.

Windows 빌드는 활성 대상을 Windows x64로 바꾸고 4개 씬을 포함한 Development Build로 만듭니다. 기존 PC QA는 빌드 동안 Standalone define에 `URP_COMPATIBILITY_MODE`를 추가하고 완료 후 원래 define을 복원했습니다. [검증 빌드 도구](Assets/Scripts/Editor/MultiplayerValidationBuild.cs)는 대상 전환이나 Player 실행을 자동으로 하지 않습니다. 상세 빌드·설정 보존 절차는 [멀티플레이 검증 기록](MULTIPLAYER_TEST_PLAN.md)을 따릅니다.

### 경기 규칙과 조작

현재 기본 규칙은 팀별 20점 선취 또는 300초 제한입니다. 적 처치로 팀 점수가 1 올라가며, 시간이 끝나면 높은 점수 팀이 승리하고 동점은 무승부입니다. 상대 팀 참가자가 모두 이탈하면 남은 팀이 승리합니다. 경기 종료 후에는 전투·리스폰이 중단됩니다.

기본 소총은 탄창 30발, 재장전 2초이며 피해는 머리 100·몸통 25·팔/다리 18입니다. 호스트가 명중·피해를 판정하며 아군 피해는 적용하지 않습니다. 사망 후 3초부터 안전한 팀 스폰을 찾고, 가능할 때 HP 100·탄약 30으로 리스폰합니다. 안전한 스폰이 없으면 대기하므로 항상 정확히 3초에 부활하는 것은 아닙니다.

| 동작 | PC | 모바일 구현 |
| --- | --- | --- |
| 이동 | WASD 또는 방향키 | 왼쪽 조이스틱 |
| 시점 | 게임 영역 클릭으로 커서 캡처 후 마우스 이동 | 오른쪽 빈 영역 터치 드래그 |
| 사격 | 마우스 왼쪽 버튼 누르기/유지 | 사격 버튼 누르기/유지 |
| 달리기 | Shift 유지 | 달리기 버튼 유지 |
| 재장전 | R | 재장전 버튼 |
| 스코어보드 | Tab 유지 | 점수 버튼으로 열고 닫기 |
| 커서 해제 | Esc, 게임 영역 클릭으로 재캡처 | 해당 없음 |

스코어보드를 열면 전투 입력이 차단됩니다. Editor의 `Preview Touch Controls`는 터치 UI 표시·포인터 동작을 점검하는 기능입니다. Editor는 PC 입력 경로를 유지하므로 이 미리보기로 실제 다중 터치, 기기 포커스·백그라운드 복귀, 성능을 검증했다고 할 수 없습니다. 모바일 조작 표는 구현 설명이며 실기기 통과 결과는 아닙니다.

### 씬 구성

| 씬 | 용도 |
| --- | --- |
| `Assets/Scenes/Login.unity` | 로그인 |
| `Assets/Scenes/Lobby.unity` | 로비 |
| `Assets/Scenes/Room.unity` | 방 |
| `Assets/Scenes/Game/DesertHouse.unity` | 게임 맵 |

## 다운로드

[과거 APK 산출물](https://drive.google.com/file/d/1SQojyQafq9IdmNvTONF80G8ri3QPqeXk/view?usp=sharing) — 기존 링크이며 최신 전투·로그인·복귀 수정이 포함된 빌드로 검증하지 않았습니다.

현재 전달용 파일은 Play Console의 기존 내부 테스트 버전5에서 다운로드한 `SuddenForceFPS-1.0.194-code5-PlaySigned.apk`입니다. 크기는99,434,034 bytes, SHA256은 `FC64BA8EFF0521A8CD12081293C15B84019D1B481E5B69B0E844DFBCCFFAAABF`이며 Play 앱 서명 인증서를 사용합니다. 사용자 지정 Drive에 전달한 파일로 공개 다운로드 URL은 이 문서에 게시하지 않습니다. 새 PC에서 새로 빌드한 APK가 아니며 Drive 직접 설치 성공도 Play 스토어 설치 검증을 대체하지 않습니다.

이전 2026-10-07 소스 `ea29fe6`의 `SuddenForceFPS.DebugValidation.apk`는 Android Debug 서명 개발본입니다. 위 Play 서명 APK와 구분하며 당시 파일·검증 범위는 [Android 릴리스 점검표의 APK 기록](ANDROID_RELEASE_CHECKLIST.md#최신-전투로그인복귀-포함-개발-apk-2026-10-07)에 보존합니다.

## 외부 라이브러리

- [Photon Fusion](https://doc.photonengine.com/fusion/current/getting-started/sdk-download)
- [play games plugin](https://github.com/playgameservices/play-games-plugin-for-unity/releases)
- [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
- [MiniJSON](https://github.com/Unity-Technologies/UnityCsReference/blob/master/External/JsonParsers/MiniJson/MiniJSON.cs)
- [Keystore Helper](https://assetstore.unity.com/packages/tools/utilities/keystore-helper-58627)
- [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068)
- [Safe Area Helper](https://assetstore.unity.com/packages/tools/gui/safe-area-helper-130488)

Google 로그인 참고: [Google Play Games Services 로그인](https://developer.android.com/games/pgs/android/android-signin?hl=ko)

## 빌드

Android 배포용 빌드 진입점은 [BuildScript.cs](Assets/Scripts/Editor/BuildScript.cs)입니다. Android를 활성 대상으로 선택하고 Play·컴파일·임포트가 끝난 상태에서 실행합니다. 활성 씬 4개와 기존 앱 패키지 `com.AeDeong.SuddenForceFPS`, 기존 서명 키·alias 및 `BuildInfo/buildinfo.txt`가 필요합니다.

| 방식 | 방법 |
| --- | --- |
| Unity 에디터 | `Build > AOS > APK` 또는 `Build > AOS > AAB`. `Build/AOS`는 스크립트가 생성 |
| 전용 배치 빌드 | Android 대상으로 `BuildScript.BuildAOSAPK` 또는 `BuildScript.BuildAOSAAB` 실행. 성공/실패에 따라 전용 배치 Editor exit 0/1 |
| 개발 APK 검사 | 별도 [AndroidValidationBuild.cs](Assets/Scripts/Editor/AndroidValidationBuild.cs)의 `Queue` 경로. 기존 Android debug key로 외부 새 디렉터리에 생성하며 릴리스 경로와 구분 |

배포용 빌드는 기존 서명 비밀번호를 Editor에 안전하게 입력하거나 `SUDDEN_FORCE_KEYSTORE_PASSWORD`와 `SUDDEN_FORCE_KEYALIAS_PASSWORD` 환경변수로 전달해야 합니다. 값은 소스·README·로그에 기록하지 않습니다. 새 키 생성으로 기존 앱 서명을 대체하지 않습니다.

기본 versionCode는 로컬 ledger와 PlayerSettings 중 큰 값에 1을 더합니다. `SUDDEN_FORCE_VERSION_CODE`를 지정하면 두 로컬 값보다 크고 2,100,000,000 이하인 정수여야 합니다. Play에 올리기 전 Console의 기존 업로드 코드도 확인해야 합니다. 실패한 빌드도 예약된 코드에 빈 번호를 남길 수 있습니다. 최신 debug code3은 Play 업로드 코드 예약을 뜻하지 않습니다.

APK 메뉴는 Development/LZ4, AAB 메뉴는 non-Development/LZ4HC/CleanBuildCache이며 둘 다 상세 빌드 보고서를 생성합니다. APK 메뉴를 non-Development 릴리스 APK로 표현하지 않습니다. 도구체인·서명·Resolver 재실행 시 템플릿 점검과 배포 선행 조건은 [Android 릴리스 점검표](ANDROID_RELEASE_CHECKLIST.md)에 있습니다.

### 기존 CI/CD 구성의 현재 상태

- Unity2022/macOS/self-hosted `buildpc` → App Center 구성은 [원문 보관 파일](.github/legacy/cicd.yml.disabled)로 이동했습니다. 활성 `.github/workflows`에 실행 정의가 없으므로 이 소스에는 자동 Android 빌드·배포나 PR 검증 CI가 없습니다. 과거 [unity-cicd](https://github.com/ChoiDaeYoung-94/unity-cicd) 안내는 현재 배포 절차가 아닙니다.
- [App Center 배포 기능은 종료](https://learn.microsoft.com/en-us/appcenter/retirement)되었습니다. 로컬 빌드는 Unity6000.3.25f1 절차를 사용하며 기존 Sudden Force 앱의 Google Play 내부 테스트 code5 업로드·출시는 완료했습니다. Play 스토어 설치본의 설치·로그인·경기 검증은 남아 있습니다.
- 2026-10-08 확인한 과거 Actions 2건은 completed/cancelled였고 저장소에 등록된 self-hosted runner는 없었습니다. 해당 시점의 확인이며 현재 runner 가용성이나 Unity6 라이선스·Android 도구체인·secrets 구성을 보장하지 않습니다. 새 CI 환경은 미구성입니다.
- 후속 CI는 확인된 runner에서 수동 `workflow_dispatch` 단일 빌드 작업으로 검증한 뒤 추가합니다. 로컬 빌드·실기기 검증과 GitHub CI 결과를 구분하며, PR 병합으로 빌드·배포를 자동 실행하지 않습니다. 진행 범위는 [이슈 #44](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/44)에 기록합니다.

## 계획·검증·라이선스

- [프로젝트 계획과 범위](PROJECT_PLAN.md)
- [멀티플레이 검증 결과·미검증 항목](MULTIPLAYER_TEST_PLAN.md)
- [Android 빌드·서명·Google Play 테스트 배포 점검](ANDROID_RELEASE_CHECKLIST.md)
- [외부 에셋과 라이선스](THIRD_PARTY_ASSETS.md)
- [기존 이슈와 현재 작업 연결](ISSUE_TRACKING.md)
