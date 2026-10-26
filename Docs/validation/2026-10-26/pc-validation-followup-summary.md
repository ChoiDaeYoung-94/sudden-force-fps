# PC 품질 검증 후속 요약 — 2026-10-26 19시 이후

현재 제품 main은 `439c833`입니다. 오늘 제품 변경은 PR239/240/241/243/245/247/253/255의8개이며, PR249/252는 문서·정책 변경으로 구분합니다. 기존6개 조작·HUD 개선에 UniRx 객체 검색 API 대체와 모바일 MENU 조작 안내를 더했습니다. **마지막 확인·전달본은 코드33/1.0.196/release11 그대로이며 이8개 후속 제품 PR은 제공 APK33에 미포함입니다.** 소비 원장 `196,1,33`과 P2 기존 dirty5를 보존했습니다.

이번 후속의 새 Android 예약·AAB/APK·서명·Play 업로드·Drive 전달은0입니다. 현재 Console 최대 코드를 새로 조회하지 않았습니다. 다음 Android 통합은 fresh 입력·출처·원장/Console 기준을 확인하는 별도 작업입니다.

## 이번에 확인한 범위

| 구분 | 완료한 좁은 범위 | 남는 제한 |
| --- | --- | --- |
| Windows03 | Succeeded/오류0/경고2, 실제 payload360개 SHA·크기 독립 일치, 공개3984개·strict13 보존 | f055 scratch+정확 소스2개/형식10개이며 canonical clean-main 빌드가 아님 |
| Windows03 native smoke | 자동 Login→Lobby(QAW03)→정상 EXIT/OK1회, 사전 보유 process handle ExitCode0·Crash literal0·강제종료0 | 명시적 종료 후 UI query0이나 backend UIA는 독립 측정하지 않음. #154 원인 해결·모든 종료/실제 경기 통과가 아님 |
| R7 MENU | C4 소스 일치,30pt4줄·glyph/raycast·전환10관측+finally 원복·정상 수명 정리 | 실제 GameView3040×1440의 단일 비율, Editor preview/public callback 경로이며 실제 멀티터치·Android Back 수용이 아님 |
| R8 CROUCH | 기존 LD33 host↔현재 P3 Editor의 정상840행/357프레임, crouch true73행/27프레임·서기→앉기→서기 |450ms는 요청값이며 넓은 명령 구간과의 겹침만 확인. 정밀 Down/Up·지연/FPS·렌더 자세·이동 중 앉기·실기기 통과가 아님 |
| Android Editor 준비 | UNITY_ANDROID+UNITY_EDITOR, Editor 재컴파일 성공·상태 조회 코드 진단0, Android 대상 지원·IL2CPP backend 설정·ARMv7/ARM64 관측 | 새 IL2CPP Player/AAB·ARM64 단독 대상 수용을 수행한 것이 아님 |

Windows Report27.664199초와 helper41.3210622초를 구분합니다. Report278,513,836B와 실제 합계278,711,383B의 차이197,547B는 Burst DoNotShip 텍스트1개입니다. Windows 표시 버전1.0.194는 Android 제공 버전이 아닙니다. 경고2개는 Dashboard 미연결과 RuntimePipelineConfig 부재 안내이며 해결·억제로 처리하지 않습니다. P2 보호6개와 기존 dirty5·원장을 보존했습니다.

R7의 P3 비대상8개는 현재 CRLF 정규화 후 같은 본문이고6개는 raw EOL 차이입니다. 전후 manifest가 없으므로 전체 rawexact나 전후 보존으로 쓰지 않습니다. R8 첫 Join 요청 뒤 Room/READY 검증 전 로비 복귀를 관측했고 재시도 성공도 함께 보존합니다. #198 원인은 미확정입니다. remote HP100/shot0/life0은 원격 actor 표본 값이며 양쪽 전체 카운터 불변으로 확대하지 않습니다.

## 다음 작업과 이슈 범위

#250/#254의 좁은 작업은 완료했습니다. 기존 열린 상위16개(#198/#188/#166/#162/#163/#132/#154/#66/#42/#41/#43/#140/#44/#35/#36/#1)는 각 잔여 범위를 유지합니다. #256은 이 후속 기록 묶음이며 상위 이슈를 닫는 근거가 아닙니다.

다음은 남은 기본 기능·모바일 UI 품질 경계를 구체화하고 새 Android 통합을 별도 준비하는 순서입니다. 실제 기기·Store·16KB 기기·전체 권리·음향 청취·발열·장시간 FPS/메모리·저사양 성능은 아직 미완료이며 합의한 후순위를 유지합니다. 수동 기기 연결을 반복 요청하지 않습니다.

상세 수치와 제한은 [Windows03 요약](windows-build03-summary.json), [모바일 MENU·CROUCH 요약](mobile-menu-crouch-summary.json)을 기준으로 합니다. [이전 같은 날 PC 기록](pc-validation-summary.md)과 [이전 Windows 빌드 기록](windows-build-summary.json)은 당시 범위로 보존합니다. 원본 APK·JPG·개인 로그·키/인증 자료는 이 문서 묶음에 포함하지 않습니다.

현재 필요한 사용자 조치와 질문은 없습니다.
