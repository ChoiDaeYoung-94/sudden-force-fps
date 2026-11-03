# Input261 native 경로 계약 수정과 검증

2026-11-03 재개 작업의 검증 기록입니다. 이 문서는 작업 중 초안이며 실제 컴파일·첫 publisher·종료/정리의 최종 영수증을 연결하기 전까지 완료 판정으로 사용하지 않습니다.

[#326](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/326)은 외부 L2 검증 도구의 경로 직렬화 정합 범위입니다. #259/PR261의 실제 Android 입력·L2/timing/packet 수용과 구분합니다. 기존 [C14 사전 중단](../2026-11-02/input261-l2-cohort02-preflight-stop.md)의 M6 false 판정과 실제 실패 자료는 보존합니다.

## 수정 범위

C15의 native7 JSON에서 projectPath/nativeExe 두 값만 검증을 마친 내부 Windows 상수의 forward-slash 표현으로 발행합니다. 내부 상수·canonical/noReparse/현재 PID·exe·project·start100ns 검증과 ownership/watcher의 기존 경로 표현은 유지합니다. core/mapper의 엄격 검사를 완화하지 않습니다.

새 검증 namespace는 cohort03/runtime-area03/phase03/family03이며 sourceDir은 revision15입니다. 공개 입력 루트의 날짜 경로 `20261102-work/resume-0452`는 보존한 저장 위치입니다. 새 관측의 날짜와 유효성은 실제 영수증의 시각·현재 scope/lease로 판단합니다. 새 source63의 manifest는 `98BDBA6369B6E4C78588E96DD8D14FFB13EA0681D8282C208EF45AD4D86F41EA`입니다.

새 native rawSHA가 runtime lease, normal helper receipt/admission, collector의 history/proof/prefix/mapper를 포함한10parent와 종료·정리까지 전파되어야 합니다. 과거 native 원문·SHA/PID/lease/IR를 새 성공 근거로 복사하지 않습니다.

## 실행 전 확인

현재 Source IR은 20,909B/SHA `6E7D977A93BFBD5C5AEADACCD41AD6272AF0C283C9D203EB5A302DA0B0B1C21A`, Control IR은 8,980B/SHA `D3E8E797B4D32D18D49DEFE075568416E3893E0DF20A0F2844561F1D9CDD97C3`입니다. 두 원본은 기존 공개 루트의 `l2-family15-gated-independent-review01/`과 `l2-family15-controlled-tests01/`에 CreateNew로 보존했습니다.

Source의 독립435검사와 IR 소비12, Control의 current Source IR 원문 재결속162검사·별도 정적69검사를 구분합니다. 중복·저자·과거 결과를 합산하지 않습니다. 실제 고정 공개 템플릿9를 사용한 native→runtime lease→normal helper/admission→collector history/proof/prefix→whole mapper/config2→Reader snapshot/window의 합성 양성과 단일 stale 핀 거부를 확인했습니다. source/control 수용은 이 통제 범위뿐이며 실제 Unity·SDK·기기 실행 수용이나 Root 권한을 뜻하지 않습니다. 각 하네스의 최초 실패와 fixture 정정도 보존합니다.

Root 실행 준비02는 원래 자원 창과 나중에 발행되는 lease의 기한이 어긋날 수 있다는 M1으로 수용하지 않았습니다. 미호출 준비03의 생성 오류는 PowerShell parser에서 차단됐으며 원 파일을 보존했습니다. 별도 준비04는 initial 직전 원 상한의 남은 시간을 계산하고, initial 직후 발행된 기한이 원 상한 안인지 검사한 뒤에만 stage/open을 진행하도록 했습니다. 이 검토는 실행 준비 코드 범위이며 실제 권한·실행 성공을 대신하지 않습니다.

## 실제 결과 — 최종 영수증 연결 전

| 범위 | 상태 |
| --- | --- |
| 새 실제 native 신원·forward native7 | 미실행 |
| compile4/defaultfalse4 | 미실행 |
| 첫 publisher prepare-directories | 미실행 |
| 동일 held native 종료·watcher tool/Dispose | 미실행 |
| own4 cleanup·public3984/dirty18·P2보호5 | 미실행 |
| Play·normal helper 실행·Queue·LD/ADB·Native 입력 | 이번 준비 검증 범위에서0 유지 |

## 현재 제품과 다음 범위

사용자 제공본은 코드34/1.0.196/내부 release12이며 새35 예약·빌드·서명·업로드는0입니다. 최신 FIRE 배치와 Quad D 인트로의 main 반영 및 PC 검증을 코드34 포함·Android Player 재생 수용으로 확대하지 않습니다. 실기기·장시간 성능 측정은 후속입니다. 현재 필요한 사용자 조치나 질문은 없습니다.
