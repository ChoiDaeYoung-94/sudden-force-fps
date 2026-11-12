# Android 시작 배경을 검정 인트로에 맞추기

`ProjectSettings/ProjectSettings.asset`의 `m_SplashScreenBackgroundColor` 한 항목을 불투명 흰색에서 불투명 검정으로 변경했습니다. 자체 `SuddenForceIntroCanvasOutput`의 준비 배경과 같은 색입니다. 인트로 재생·음향·장면 전환이나 첫 frame 대기 코드는 변경하지 않았습니다.

Unity6000.3 공식 문서는 이 색이 Android 정적 시작 화면과 Android12 이상 자동 시작 화면을 가리는 배경에 사용된다고 설명합니다. [SplashScreen.backgroundColor](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityeditor/playersettings/splashscreen/backgroundcolor)

첫 공기계의 QA2 콜드 영상에서는 로고 전 흰 화면 표본을 보았습니다. QA2 생성·Gradle 입력의 `colors.xml`은 `staticSplashScreenBackgroundColor=#FFFFFFFF`이고 Android12 이상 스타일이 이 색을 참조했습니다. 이 설정 차이를 정리한 것이며, 실제 기기의 두 흰 전환 구간이 모두 이 설정 때문이라고 확정하지 않습니다.

검증은 설정 파일29,741바이트에서 색 한 항목의1행만 바뀌었고 다른 원본 바이트는 같다는 대조와 `git diff --check`입니다. 기존 Unity 로고·버전·서명 설정은 그대로이며, 사용 중인 P2/P13 프로젝트를 저장하거나 SDK·Editor·기기를 실행하지 않았습니다. 문서 전용 worktree에서 이 공개 설정 파일 하나만 sparse 포함해 수정했습니다.

이 변경은 현재 설치 QA2나 운영34/release12에 들어 있지 않습니다. 다음 정상 Unity export에서 생성 색/스타일·최종 APK와 실제 공기계의 콜드 시작을 확인해야 합니다. 같은 원본의 단순 후처리만으로 Unity 내부 버전까지 새 QA3가 됐다고 판정하지 않으며 새 산출물 검증을 별도로 진행합니다. #362는 기기 비교가 남아 열린 상태로 유지합니다.

근거: [생성 입력·adapter 조사](D:/codex/validation/sudden-force-fps/20261111-work/phone-startup-white-investigation01/REPORT.ko.md), [색 한 항목과 원본 바이트 대조](D:/codex/validation/sudden-force-fps/20261111-work/root-resume01/issue362-color-change.public.json), [QA2 첫 공기계 검증](qa2-first-phone-and-ld2.md).
