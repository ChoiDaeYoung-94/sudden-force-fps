# QA2 Windows Player 생성과 공개 입력 복구

2026-11-10에는 Raw05 QA 소스 `d7a3371296357a5ceb1b7b657d468415130e7047`에서 새 Windows Player를 생성했습니다. 기존 QA1과 구분한 같은 QA2 소스의 두 피어 검증을 준비하는 작업이며, 실제 Game·프로토콜·보행·소총·Android 수용은 별도입니다. [Draft PR353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)과 [열린 이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)을 유지합니다.

## 실행과 산출물

- Unity `6000.3.25f1`, Windows64/Mono, Development·DetailedBuildReport, Intro/Login/Lobby/Room/DesertHouse 순서의 5개 씬입니다.
- 직접 Native Editor를 새로 실행하고 보유 Process 핸들의 PID·생성 시각·실행 파일을 확인했습니다. `DOTNET_PROCESSOR_COUNT=1`, `BEE_BUILD_THREADS=1`, job worker1을 해당 자식에만 적용했습니다.
- BuildPlayer 1회, `Succeeded`, errors0/warnings2, 빌드 보고서 시간185.9981374초입니다. 경고 원문은 비공개 로그에 남겼으며 원인은 이 문서에서 판정하지 않았습니다.
- Native PID38920의 시작은 UTC03:24:26.8709683, 실제 종료는03:30:51.3960563입니다. 실제 ExitCode0·강제 종료 없음·stdout/stderr EOF·Dispose 오류0을 확인했습니다. 관리 코드의 종료 요청과 실제 종료를 구분했습니다.
- 실제 381파일의 전체 경로 집합·크기·SHA를 독립 재계산했고 모두 일치했습니다. 합계404,419,837B입니다. BuildReport의404,222,251B와 차이197,586B는 DoNotShip의 `lib_burst_generated.txt` 한 파일로 설명됩니다.

핵심 실행 파일 `WindowsQA2.exe`는667,136B/SHA `9985CEA53B68F2D8787AAB4F496A228C335A139B39703A3915BEF6676716662A`이며, Player의 `Assembly-CSharp.dll`은326,144B/SHA `37F65F4541A9CC49A869AC841B31DB60087A14B8A3B09F9F94A6C5EC8B2C3704`입니다. Editor에서 읽은 MVID를 Player DLL의 등가 증명으로 사용하지 않았습니다. 전체 inventory를 실제 파일과 대조한 범위와 소스의 명령어 수준 등가·런타임 판정을 구분합니다.

표시 버전은 `1.0.197-qa2`입니다. 표시 버전이 Photon AppVersion이라는 주장은 하지 않습니다. `SUDDEN_FORCE_LOCAL_QA`와 native-input QA 정의를 요청했지만 정의 추가만으로 recorder 코드·`SFF_INPUT_QA` 로그의 실제 포함·작동을 수용하지 않았습니다. 실제 태그 계약을 확인하기 전에는 화면·HUD·게임 상태로 검사합니다.

## 보존과 복구

빌드 전 공개4,095개와 백업4,095개를 새로 비교했습니다. 별도 임시 C#/meta2개만 적용했고, source55·임시 helper2·메모리3필드 복원은 helper와 controller 결과를 교차 확인했습니다. 전역 저장·dirty-clear·Android setter·자격정보 getter는 helper에서0입니다.

종료 뒤 실제 바이트가 달라진 공개 파일은9개였습니다. URP3·URP Global·GraphicsSettings·PlayerSettings·ShaderGraphSettings의7개는 줄바꿈 변화이며, CJK font asset과 EDM Resolver 설정2개에는 직렬화 변화가 있었습니다. 정확한 변경 목록·현재 SHA·원본 백업 SHA를 고정한 읽기 전용 scope를 총괄이 검토한 뒤, 별도 독립 검토를 통과한 복구 도구를 적용했습니다.

각 원본·생성본9쌍과 임시 helper2의 바이트를 외부 증거에 보존하고 readback한 뒤 CAS로9개를 복원했습니다. 임시2개는 정확한 소유 경로·SHA일 때만 제거했습니다. controller 승인4개/helper 승인2개만 false로 회수했으며 나머지 JSON 필드와 원래 승인 바이트를 보존했습니다. 이후 독립 검토에서 공개4,095개 전체의 현재 크기/SHA 일치, HEAD d7·Git clean·private6 실제 부재/index S를 확인했습니다.

실행 자원 반환은 UTC03:37:06.1683213이며 두 Unity ADB 설정은 DWORD1, 공유 daemon의 관측 세대와 실행 파일 SHA를 유지했습니다. 후속 zero-payload probe의 첫1회 실패·사유 미확인과 별도 새 관측 성공을 분리 보존했습니다. daemon 재시작이나 기기 명령은 수행하지 않았습니다. 알려진 SDK 부재 표본과 전체 미확인 자식 트리 종료 증명을 동일하게 취급하지 않았습니다.

파일 복구 완료는 UTC03:43:43.949731로 원래 full03:42:12 이후입니다. `beforeOriginalFull=false`를 유지합니다. 실행 자원의 기한 내 반환과 이후 파일 복구를 분리하며 전체 작업이 원 full 전에 끝났다고 소급하지 않습니다.

## 수용 범위와 다음 검사

이번 통과는 새 Windows 빌드·산출물·공개 입력 복구 범위입니다. Player를 실행한 실제 QA2 Game, 두 피어 연결, Raw05 발 접촉/방향·속도 전환, 최종 rifle 자세, native recorder, 전체 unknown 자식 종료, Android APK·서명·설치·Play 제공은 이 결과로 통과 처리하지 않습니다. Raw05는 보행을 분리해 보는 모드이며 최종 소총 동작과 정량 접지는 후속입니다.

다음은 QA2 Android 산출물의 독립 정적 검증·기존 키 서명과 LD플레이어 설치, 같은 QA2 Windows–LD 두 피어 경기입니다. 평지 방향/걷기↔뛰기·이동 사격/장전·사망/재생성을 먼저 보고 crouch/ladder·가능한 낙하/착지·결과/로비 복귀를 이어갑니다. ordinary jump 모드는 현재 제품에 없으므로 새 점프 기능을 전제로 검증하지 않습니다. 사용자 지시에 따라 이번 회차는 LD를 사용하며 휴대전화 검증은 새 기기 후속으로 남깁니다. 운영34/1.0.196/release12·원장196,2,34와 기존 QA1을 보존합니다.

## 공개 근거

외부 검증 루트는 `D:/codex/validation/sudden-force-fps/20261110-work`입니다. 원시 로그·dump·자격정보를 저장소에 옮기지 않았습니다.

| 자료 | SHA256 |
| --- | --- |
| `root-resume01/windows-player01-actual-return.public.json` | `19E527C0BC1F0EE338711B870739E2860661737E4691F6DA59C9F513DC5BD039` |
| `root-resume01/windows-player01-full-inventory.public.json` | `184281407ED593B207A52B65923F4ECCCC0F3CE1165A77092121E2D21371D627` |
| `windows-player01-actual-review01/verification.public.json` | `0DF3A303E91D70FE657E2620473CDD46626569BC749298B6F13B04BCB296CF95` |
| `windows-player01-root-return-actual-review01/verification.public.json` | `DB561CDAB0CF1A54BB1E1077F829E04C8D0FEEB0484A88D69654F515EFF0819B` |

현재 필요한 사용자 조치 없음. 질문 없음.
