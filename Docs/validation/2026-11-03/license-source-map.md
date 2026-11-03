# 코드34 고지와 후속 공개 소스의 출처표 (#42)

2026-11-03, 공개 문서만 대조한 기록입니다. 코드34의 기존 정적 고지 포함 근거와 이후 main의 QuadD·FIRE 변경을 연결합니다. NOTICE/LICENSE 원문·제품·미디어·패키지·설정 변경, 새 빌드·서명·업로드는 없습니다. 법적 허용·고지 의무·전체 권리 완료를 판정하지 않으며 #42와 #302는 OPEN으로 유지합니다.

## 코드34의 기존 확인과 후속 소스 구분

| 대상 | 공개 기록으로 확인한 범위 | 포함·수용 한계 |
| --- | --- | --- |
| 코드34 QA | source `61218033ff36a2da3770ad93b9cc6c10c105c92d`의 AAB에 고지 TextAsset **725,013B 본문 일치**가 기록돼 있습니다. [산출물 검증](../2026-10-27/android34-input-qa-artifacts.md)의 고지 행을 따릅니다. | 이번 문서 작업에서 AAB/APK를 다시 읽거나 packed 고지 원문 SHA를 재검증하지 않았습니다. 전체 권리 판정이 아닙니다. |
| 실제 제공34 | [내부 테스트 제공 기록](../2026-10-28/play34-internal-delivery.md)은 코드34/1.0.196·release12와 PlaySigned APK 105,690,443B/SHA `09403527CFC04B74B85942BB7DFAFB0E727FA30BB0B8660AB2EB010E116E7F76`을 구분해 기록합니다. | AAB 고지 검증을 APK의 모든 assets/fonts/DEX 동등성이나 최신 main Android 통합 수용으로 확대하지 않습니다. |
| 이후 main | [Android35 공개 소스 후보](../2026-10-31/android35-public-source-integration-candidate.md)는 큰 FIRE 배치·Intro scene/출력 통합 등 제공34 이후 차이를 설명합니다. | 이 후보는 source-only·미실행이며 새35 예약·게임 APK/AAB·서명·업로드는0입니다. Intro package/video 전체의 기존34 payload 부재를 전수 판정한 자료도 아닙니다. |

## 후속 출처표

| 대상 | source·구성의 정확한 대응 | 고지·권리와의 경계 |
| --- | --- | --- |
| QuadD Intro | [통합 기록](../2026-10-30/quad-d-intro-integration.md)의 회사 저장소 `com.quadd.intro`0.1.0, 고정 commit `55bf9e4aa48c2f95942f3c2c995f1d55414e97c0`입니다. 영상88,032B는 원 Git blob `be65931578eb3803fa5fe4869c2d77d3c5346bb1` 대응으로 기록됐습니다. 3.4초·1920×1080·30fps·오디오 트랙1이며 프로젝트의 자기 AudioSource로 연결합니다. | 회사 패키지 도입과 영상/오디오 구성의 출처 연결입니다. 패키지·영상·오디오의 세부 저작자나 외부 재료의 라이선스를 임의로 만들지 않습니다. 현재 허용된 문서에는 패키지 LICENSE/NOTICE 본문의 정확 경로 대응이 없어 그 원문·세부 조건은 미검증입니다. |
| QuadD 후속 수용 | [Windows 표시·두 Player 회귀](../2026-10-31/quad-d-intro-windows-regression.md)와 [Android 대상 Editor 컴파일](../2026-10-31/quad-d-intro-android-editor-compile.md)을 별도 연결합니다. Intro scene·출력 adapter·접속 버전 분리는 제공34 이후 변경입니다. | Android 대상 Editor 컴파일은 Player 전용 분기·IL2CPP/Gradle·Android 인트로 재생·실청의 통과가 아닙니다. 회사 패키지라는 기록만으로 모든 구성물의 권리를 확정하지 않습니다. |
| FIRE 배치 | [배치 기록](../2026-10-30/mobile-fire-layout.md)은 기존 씬의 RectTransform8개 속성·8줄 교체입니다. FIRE 168→248, 내부 장식156→232와 주변 control/KillFeed 배치이며 기존 컴포넌트·이벤트·입력 연결을 유지합니다. | 문서에 새 자산 추가는 기록돼 있지 않습니다. 이 변경만으로 새 이미지·폰트·음원 고지 누락을 만들지 않습니다. 제공34/release12에는 이 후속 배치가 미포함이며 새35 생성 근거도 아닙니다. |
| 기존 HUD7 | [기존 출처 기록](../../../THIRD_PARTY_ASSETS.md)의 Crosshair·Fire·Gun·Health·HPBar·Jump·Sit PNG7개는 사용자가 직접 제작 또는 AI 생성이라고 확인한 상태를 유지합니다. | 개별 제작 방식·도구는 추정하지 않으며 출처 질문을 다시 열지 않습니다. 파일명만으로 버튼 기능을 판단하지 않습니다. |

## 현재 고지의 개행과 과거 packed 본문의 구분

현재 공개 [ThirdPartyNotices.txt](../../../Assets/Legal/ThirdPartyNotices.txt)는 **724,958B/SHA256 `14FC049C9D2C13ED374D92DC3B541A34F42D9B515C41A1D047292D878E79EDFF`**입니다. CRLF12,993개와 LF-only55개가 있고, 메모리에서 개행만 CRLF로 통일하면 **725,013B**가 됩니다. 파일에는 쓰지 않았습니다.

이는 코드34 문서의725,013B와 크기 차이55B를 설명할 수 있는 재현 가능한 관측입니다. 옛 packed 본문/SHA를 이번에 읽지 않았으므로 크기 일치만으로 본문 동일성을 다시 확정하거나, 원래55B가 누락됐다고 판정하지 않습니다. 다음 통합 빌드에서는 그때의 고지 원문과 개행 정책을 고정하고 packed 전문을 새로 결속해야 합니다. 기존 코드34 검증 결과와 현재 원문은 보존합니다.

## 남아 있는 #42 범위

- 기존 캐릭터·무기·맵 세 팩의2024년 취득 확인과 파일명 대응은 유지합니다. 옛 표의 취득 미확인은 뒤 확인 기록으로 보완됐습니다. 원 패키지 판본·정확 바이트·취득 당시 조건과 별도 구성물의 조건까지 확인한 것은 아닙니다.
- Animated Loading Icons는 기존 계정의 검색 범위에서 취득 항목을 찾지 못한 상태입니다. 취득 무효·사용 금지를 뜻하지 않으며 다른 기존 계정/원 패키지·당시 조건의 확인은 남습니다.
- 플레이어 원 애니메이션22개와 기존 음원61개의 정확한 원본·공급자·판본·조건 대응은 별도입니다. 코드33의 지정 exact 경로/prefix packed0은 해당33 기록으로 유지하고, 코드34 전수 부재나 저장소 원본 재배포 권리 완료로 바꾸지 않습니다.
- 코드33 packed 표와 Google6개 버전/104레코드·28본문 대응은 기존 근거입니다. 최신 배포별 BuildReport·resolved SDK/native/font/shader BOM·정확 고지 대응 전수는 남습니다. DOTween1.2.765 공식 ZIP/동봉 readme 대응은 유지하며 현재 웹 조건으로 과거 조건·Pro 여부·전체 권리를 대체하지 않습니다.
- QuadD 패키지·영상/오디오의 추가 공개 출처/고지 원문이 있으면 위 고정 source와 연결해야 합니다. 회사 패키지라는 이유로 외부 재료가 있다고도, 모두 자체 제작이라고도 추정하지 않습니다.

필요한 원자료·구매·권리자 사실은 남은 항목으로만 보존합니다. 이번에 새 구매·허락·계정 확인을 요청하지 않습니다. 현재 필요한 사용자 조치와 질문은 없습니다.

## 읽기 범위와 외부 검토 자료

근거는 Root의 #42 공개 snapshot과 지정된 공개 문서/고지 텍스트16개입니다. Git/GH/API/웹·OS/SDK·Unity·제품 source·설정·원본 폰트/미디어·바이너리·private6 조회·해시는0입니다. 위 고지 텍스트의 bytes/SHA 확인은 원본 폰트·바이너리 검사가 아닙니다. 과거 빌드·취득·배포 사실은 기존 문서 기록의 인용이며 이번의 재관측이 아닙니다.

- 읽기 전용 분류: `D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/license42-readonly-triage01/REPORT.ko.md` 및 `inputpins.public.json`.
- 문서 작성·원문 보존 검증: 같은 root의 `license42-doc-author01/REPORT.ko.md`와 `manifest.public.json`.

저장소의 기존 상세 [THIRD_PARTY_ASSETS](../../../THIRD_PARTY_ASSETS.md) 본문·표는 그대로 보존합니다. 이 출처표는 #42 전체 완료나 새 Android 빌드·실제 Android 인트로 수용의 근거가 아닙니다.
