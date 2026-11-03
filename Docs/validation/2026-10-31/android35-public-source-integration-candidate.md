# Android35 공개 소스 통합 후보 검증

2026-10-31 source-only 후속 기록입니다. 제공34 QA와 최신 main의 차이를 조사하고, 실제 작업 파일을 바꾸지 않는 별도 Git 인덱스에서 QA 입력 두 파일을 main에 결합했습니다. 텍스트 패치 적용·blob 대조는 통과했으며 실제 통합 컴파일·게임·Android 빌드는 미실행입니다. `readyToBuild=false`이고 PR261은 Draft입니다.

## 비교 기준과 놓치기 쉬운 변경

제공34 source는 `61218033ff36a2da3770ad93b9cc6c10c105c92d`, 기준 main은 `3ca93617189ae46314d07744c083119baa12c671`, 공통 조상은 `ea74519dd466ec891430ba37b710c0a5e8cf88dd`입니다. 조사한 공개 제품 범위에서 공통 조상→QA는2파일, 공통 조상→main은10파일이며 교집합은0개입니다. 전체 저장소·산출물 차이의 전수 판정이 아닙니다.

QA 쪽은 `PlayerInputSource.cs`와 `CombatPointerControl.cs`입니다. FIRE Down에서 같은 pointer의 touch look을 캡처하고 해제 때 놓으며, owner/life/focus/menu guard가 있는 API를 추가합니다. 기준 main의 두 파일은 공통 조상 blob과 같으므로 **main만 빌드하면 이 QA 입력 변경이 빠집니다.** 현재 Draft261 head `f3eabfb86a1db7a2ba21cd8f07cf8c194e4eea47`의 두 공개 blob도 QA와 동일함을 Root가 확인했습니다. Draft branch는 수정하지 않았습니다.

반대로 제공34에는 main의 큰 FIRE(168→248, 내부 이미지156→232)·주변 control/KillFeed 배치, Intro scene/출력 adapter/초기 이동 예외, reload palm/view 및 첫 발걸음 clip preload, 채팅 surrogate pair 보호 등이 미포함입니다. Intro package/video 전체가 제공34에 없었다거나 APK 내부 에셋을 전수 검사했다고 판정한 것은 아닙니다.

main의 NetworkRunnerManager는 복사한 AppSettings의 AppVersion에 `-intro1`을 붙여 Lobby와 StartGame에 사용합니다. 원래 비밀 Photon 설정은 읽지 않았습니다. 따라서 제공34와 main 기반 peer의 방 호환성을 자동으로 가정하지 않습니다. 기존 P11/코드34 입력 검증과 Intro 통합 Windows 검증도 별도 소스·산출물 범위입니다.

## 작업 파일을 바꾸지 않은 결합 결과

별도 `GIT_INDEX_FILE`에서 기준 main을 `read-tree`한 뒤, 공통 조상→QA의 공개 두 파일 패치를 `apply --cached --check --whitespace=error`와 `apply --cached`로 적용했습니다. `write-tree` 결과는 `9af18a43edfcf8d77993e90a37b98c05e77c500d`입니다. 이는 커밋·브랜치가 아닌 후보 tree이며 현재 제품 main에 QA 코드를 병합한 것이 아닙니다.

main→후보의 변경 경로는 정확히 위 두 C#이고, 후보의 두 blob/export는 QA612와 바이트가 같으며 나머지 tree entry는 main과 같습니다. 패치 자체도 base→QA 및 main→후보 diff와 바이트가 같습니다. 패치 적용/확인·whitespace 검사는 종료0입니다. 독립 검토는 공개 blob·패치·export·tree metadata를 대조했으며 재적용이나 실제 게임 실행을 하지 않았습니다.

Root는 P2 HEAD/branch/공용 index/working status와 기존 dirty5 보존을 확인했습니다. 그 전후 보존 검사에서 공개 P2 ProjectSettings.asset을 각1회 해시했으며 본문 출력·저장은0입니다. 원본 후보 IR/README의 포괄 PS0 표현은 부정확하여 원본을 보존하고 `verification02`/`README02`에서 이 범위로 정정했습니다. Git PS 본문·P11 PS·private6 내용 조회/해시는 이 후보 작업에서0이며 보조 검토자의 실제 PS/private6 조회/해시도0입니다. checkout·작업 파일 변경·새 커밋/브랜치·Editor/컴파일·빌드·버전 예약·서명·업로드는 이 후보 생성에서0입니다.

Git tree의 영구 보관을 가정하지 않습니다. 기준 main/QA commit과 공개 패치·export 두 파일로 후보를 재구성할 수 있습니다. 임시 인덱스를 실제 Unity 작업 경로로 사용하지 않습니다.

## 근거와 다음 작업

외부 자료는 `D:/codex/validation/sudden-force-fps/20261031-work/resume-0124/`에 보존했습니다. source gap의 최신 권위는 `android35-integration-gap-source-audit01/REPORT02.ko.md`와 `verification02.public.json`(SHA256 `DA5FBA965B87452B82B55DF17392311B8002363FDA6526E34CFAAB686002D202`)입니다. 최초 보고서의 UI 명칭 정정 전 이력도 보존했습니다.

결합 후보는 `android35-public-integration-candidate01/verification02.public.json`(SHA256 `E1009411CCFC79474FEAC3125EC7BFB3B7A1DCB4109EAA3B645C8708CF5152E8`), `qa-public-input-overlay.patch`, `project-files/`의 공개 두 C#와 `README02.ko.md`입니다. 독립 검토 원본은 `android35-public-integration-independent-review01/verification.public.json`(SHA256 `327A5FD86A5929BAD14C33BF6AA2EA9DD7488F20E3D99D7DED3E0627A67045EB`)이며 PS 메타데이터 정정은 별도 addendum을 함께 읽습니다. 원본 검토·후보를 덮어쓰지 않았습니다.

다음은 새 통합 후보의 실제 입력/에셋/Intro·씬 프로토콜·Android 컴파일 검증입니다. 현재 B1/L2 미완료와 [작은 이동 결과](input261-small-public-joystick-navigation.md)·[다음 검증 계획](input261-next-b1-validation-plan.md)의 한계를 유지합니다. 텍스트 결합 통과는 기능 통합·실폰·native 입력·실제16KB·성능 통과가 아닙니다. 기존 제공34/1.0.196/release12와 원장196,2,34를 유지하고 새35 예약·게임 APK/AAB·서명·업로드는0입니다. 현재 필요한 사용자 조치와 질문은 없습니다.
