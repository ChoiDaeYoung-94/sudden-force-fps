# 2026-10-26 PC 검증 체크포인트

최신 제품 기준은 main `f055e4a84391bd9d8741c4e12855887b1ef72166`이며 PR239/240/241/243/245/247의 좁은 개선을 포함합니다. 실제 제공은 코드33/1.0.196/release11이고 이 여섯 PR은 APK33에 미포함입니다. 이 문서는 Windows 산출물 검토와 기존 Android33·Editor 상호운용1라운드의 좁은 결과를 담습니다. 새 Windows EXE 실행이나 최신 여섯 PR의 새 Android 통합 완료 보고가 아닙니다.

## Windows #248

별도 Windows64 Development 빌드는 Succeeded/오류0/경고5, BuildReport210.746초·helper225.388초로 완료했습니다. 실제360개 크기·SHA를 manifest와 독립 대조했고, Report278,510,396B와 실제합계278,707,943B의 차이197,547B는 함께 보존한 Burst DoNotShip 디버그 파일1개와 일치합니다. Windows 검증 메타데이터는1.0.194이며 새 Android 버전이 아닙니다.

공개입력3,984개는 누락0/C#변경0, raw변경9개는7개 EOL만·자체 CJK와 EDM2개의 EOL/빈 필드 공백으로 분류되어 설정값 변화는 발견되지 않았습니다. 변경본을 외부 보존했고 P2 보호6개·기존 dirty5·소비 원장을 유지했습니다. private6 부재/indexS와 credential helper 선제 sparse 제외는 Root 공개 영수증 범위입니다.

경고5개는 Unity Dashboard 미연결1, RuntimePipelineConfig 부재로 Player Pipeline 비활성1, UniRx의 CS0618 3개입니다. 경고를 해결·억제한 것은 아니며 새 Windows EXE의 실행·두 피어 경기·종료 검증은0회입니다. 초기 loose globalgamemanagers 기대 감사 실패와 data.unity3d 구성에 맞춘 정정을 보존했고 빌드 재실행은0회입니다.

EXE/UnityPlayer/CrashHandler/Managed/Mono 및 packed UnityFS data.unity3d 동반파일을 확인했습니다. Windows playerVersion1.0.194는 검증 JSON 메타데이터이며 EXE PE 버전은 Unity6000.3.25f1입니다. 원격 계정 연결·런타임 Pipeline 활성화·경고 억제를 하지 않았습니다. [compact 공개 빌드 요약](windows-build-summary.json)에 수치·원본 공개 영수증 SHA를 남겼으며 전체360개 manifest, 원시 로그, 키·계정 자료는 싣지 않았습니다.

독립 artifact review는 현재 파일 무결성과 보존 범위에서 차단 불일치를 발견하지 못했습니다. 이는 패키지 내부 모든 코드/리소스의 동작, cold font/최초 표시, 두 피어 정상 UI 경기, 물리 입력, 실제 청취·발사/재장전 지연이나 안정 종료의 통과가 아닙니다. private/credential helper 제외는 Windows 준비의 명시적인 source exception이므로 원본 main 전체가 그대로 컴파일되었다고 표현하지 않습니다.

## 기존 Android33 호스트·P3 Editor 상호운용1라운드

별도 LDPlayer(index2)의 기존 Android33 호스트와 오늘 여섯 PR의 제품 복사본을 담은 P3 Editor 클라이언트로 상호운용1라운드를 확인했습니다. 로비·빈 제목/맵 미선택 안내·최소2명/Ready 안내를 거쳐 DesertHouse actors2로 진입했고, 호스트 사격의 탄약30→26/shot4, P3 원격 proxy의 reload=true·표시카운터0→1, 장전완료30/shot4 유지를 관측했습니다. 양쪽 Draw/TimeExpired와 P3 Finished/remaining0/CanCombat=false·DONE2행, 정상 RETURN 뒤 양쪽 연결 로비 복귀 및 P3 닉네임QAC26·새 Runner를 확인했습니다.

이 라운드는 기존33 Android와 최신 제품 복사본 Editor의 좁은 상호운용 근거입니다. 새 Windows EXE 실행0·새 Android 생성/여섯 PR 포함0을 유지합니다. crouch tap은 UP 뒤에 관측했고 held는 동시 MatchPhase를 기록하지 않아 성공을 판정할 수 없으며, 이 라운드의 이동·피해·지연·성능·새 방 재참가 검증도0입니다. 원격 재장전 관측은 실제 청취·물리 입력·출력 프레임 지연 측정이 아닙니다.

LD 기본 안내와 양쪽 결과 PNG, P3의 공개 상태 영수증을 대조했습니다. P3의 최신 제품 복사본 구성과 양쪽 Connected 판단은 총괄 인계 범위입니다. 문서 작성 보조는 기기·Editor·프로세스를 제어하지 않았습니다. 새 Android 예약·AAB/APK 생성·서명·업로드는0건이며, 기존 코드33 결과를 최신 소스·새 제공본의 proof로 복제하지 않습니다.

## 기존 PC 근거와 남은 범위

격리 컴포넌트 회귀(조이스틱195/스크롤22/재장전59/응답255항목 실패0), 정상 조이스틱 경로의 실제 두 Editor 피어 보행 표본, 별도 합성 입력의 원격 재장전 요청과 경기 HUD 표본은 [프로젝트 계획](../../../PROJECT_PLAN.md)의 각각의 범위로 보존합니다. fixture 수치와 실제 게임 표본을 기기·물리 입력 통과 수로 합산하지 않습니다. 이전 보행 중단·경로가 다른 전후 비교·Editor preview와 cold 첫 표시/LEFT 렌더의 한계도 소급 통과시키지 않습니다.

다음 검증은 총괄의 별도 새 Windows Player 실행·두 피어 경기·종료와 이번 라운드에서 미검증인 crouch·이동·피해·새 방 재참가입니다. 실제 맵 사다리·전체 이동/재장전 수용·IME/물리 입력·cold UI·Windows 종료 원인과 기존 열린 이슈는 유지합니다. 실기기·실제16KB·발열·장시간 FPS/메모리·저사양 검증은 합의된 후순위이며 현재 PC 검증·문서 정리에는 사용자 조치가 필요하지 않습니다. 이후 Chrome 작업 창 식별과 Drive 접근 확인은 별도 후속입니다.

선택 PNG3개는 아직 복사·링크하지 않았습니다. 총괄이 실제 저장소에 복사할 때 필요한 링크를 추가할 수 있습니다.
