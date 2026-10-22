# Sudden Force 앱 아이콘

## 코드29 Android 정적 포함 확인 (2026-10-22, #164/#185)

실제 새 AAB의 manifest가 일반/원형 `@mipmap/app_icon`·`@mipmap/app_icon_round`를 참조하고,6density의 일반/원형/배경/전경 PNG24개가 존재합니다. export 원본24개와 크기·알파·비영 알파 RGB가 모두 같았습니다.18개는 decodedRGBAexact,6개 전경은 완전 투명 RGB13,294픽셀만 달랐습니다. encoded PNG 바이트24개는 모두 다르므로 원 strictRGBA 완전 일치 실패를 지우거나 전체 bytesexact로 표현하지 않습니다.

비교한 색공간 chunk5종은 원본/AAB 양쪽 모두 부재입니다. adaptive XML의 정확 두 entry 존재는 확인했지만 binary XML 의미를 재파싱하거나 실제 launcher·기기·Play 업데이트를 검증하지 않았습니다. source/이미지를 수정하지 않은 제한 정적 비교이며, 새AAB SHA·현재 export·검사 결과와 독립 검토는 외부 `20261022-evening/android29-icon-visible-pixels-05.public.json` 및 `android29-artifact-binding-review/ICON164_FINAL_REPORT.ko.md`에 있습니다. 아래는 제작/PC 적용 당시의 기록입니다.

2026-10-22 #164. 이 프로젝트를 위해 AI로 새로 만든 전술 헬멧 이미지입니다. 외부 에셋·상표 이미지를 가져오지 않았으며 앱에 이름 글자나 모서리 마스크를 미리 굽지 않았습니다.

- `Assets/Art/AppIcon/SuddenForceIcon.png`: 일반·원형·기본 아이콘용 불투명 원본, 1254×1254 RGB, SHA256 `d9debc08822523026a6560ffc3f526c1ee5e613ec1cc6a5ff288e8b403f2ed0b`.
- `SuddenForceIconForeground.png`: 적응형 전경용 RGBA, 1254×1254, SHA256 `ac3abc25b7247ed07f5d9046c71d8637b30351a7ebf9f8558cd54d5bd4f4a165`. alpha16 이상 본체 영역은617×613/전체약49%이며 alpha1~15의 희미한 픽셀9536개가 바깥에도 있어 완전히 빈 여백이라고 주장하지 않습니다.
- `SuddenForceIconBackground.png`: Unity에서 생성한1024×1024 단색 남색 RGBA(1,13,39,255). SHA256 `fe598798529b12ee1d5853b8c3f8731bf85ecbda6666fc3df1be12b1c8500704`. 전 픽셀을 독립 색상 조건으로 검사했습니다.

Android Adaptive·Round·Legacy는 공식 `AndroidPlatformIconKind` 식별자로 구분했습니다. Unity6000.3.25f1의 실제 API에서 각6개, 총18개 크기 슬롯과 기본128px1개를 채우고 readback했습니다. Adaptive 레이어 순서는 실제 메타데이터 Background/Foreground를 읽어 정했고 일반·원형은 불투명 아이콘을 사용합니다. importer는 최대1024/no mipmap/uncompressed이며 전경만 alphaIsTransparency=true입니다. [Android 적응형 아이콘 지침](https://developer.android.com/develop/ui/compose/system/icon_design_adaptive)의 두 레이어 구조를 따릅니다.

실제 Editor의 PrepareImport는 PlayerSettings 접근 없이 PNG3개와 meta를 만들었으며 ApplyIcons는 메모리 슬롯을 설정·검사한 뒤 root가 File/Save Project를 한 번 실행했습니다. 전체 공개 입력3960개를 사전에 별도 보존했고 저장 전후 차이는 ProjectSettings.asset 한 파일입니다. index에는 m_BuildTargetIcons/m_BuildTargetPlatformIcons 두 필드만 HEAD 위에 반영해 기존 버전·서명·다른 설정 변경을 섞지 않았습니다. 첫 긴 백업 경로의 Windows 경로 길이 오류는 보존하고 짧은 새 경로에서3960개 전체 SHA 일치 백업 뒤 저장했습니다. 불완전한 첫 백업을 성공으로 합산하지 않습니다.

기존 APK27/AAB28에는 이 아이콘이 없습니다. 이 소스 적용 시점에는 Android mipmap 출력이 후속이었으며, 이후 코드29 정적 포함은 위 최신 기록을 따릅니다. launcher의 실제 원형/적응형 마스크·Play 설치 업데이트 검증은 여전히 후속입니다. 키·암호 getter/서명/버전 예약은 수행하지 않았습니다. 외부 검증 근거는 `20261022-quality/icon-candidate/revision-03/`입니다.

추가로 실제 import된 텍스처를 Unity UI의 사각·원형·적응형 원형·적응형 squircle 마스크로 합성 렌더해 확인했습니다. 저장된 preview는 외부 icon-ui-mask-preview.png이며 Android launcher 실행 화면이 아닙니다. 이 렌더는 제품 이미지/에셋을 저장하지 않습니다.
