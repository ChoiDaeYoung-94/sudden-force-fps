# Input261 native 경로 계약 수정과 검증

2026-11-03 C15/cohort03는 첫 컴파일 CLI 미수락을 보존하고 종료했습니다. 별도 C16/cohort04에서는 준비 상태 확인 후 compile4/default4 진단0을 실제 확인했지만 첫 publisher가 `REGISTERED_PUBLISHER_ONLY`로 중단됐습니다. 정상 종료·정리·보존은 두 회차에서 확인했으며, 첫 publisher 통과와 #326 전체 완료는 아직 미완료입니다.

[#326](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/326)은 외부 L2 검증 도구의 경로 직렬화 정합 범위입니다. #259/PR261의 실제 Android 입력·L2/timing/packet 수용과 구분합니다. 기존 [C14 사전 중단](../2026-11-02/input261-l2-cohort02-preflight-stop.md)의 M6 false 판정과 실제 실패 자료는 보존합니다.

## 수정 범위

C15의 native7 JSON에서 projectPath/nativeExe 두 값만 검증을 마친 내부 Windows 상수의 forward-slash 표현으로 발행합니다. 내부 상수·canonical/noReparse/현재 PID·exe·project·start100ns 검증과 ownership/watcher의 기존 경로 표현은 유지합니다. core/mapper의 엄격 검사를 완화하지 않습니다.

새 검증 namespace는 cohort03/runtime-area03/phase03/family03이며 sourceDir은 revision15입니다. 공개 입력 루트의 날짜 경로 `20261102-work/resume-0452`는 보존한 저장 위치입니다. 새 관측의 날짜와 유효성은 실제 영수증의 시각·현재 scope/lease로 판단합니다. 새 source63의 manifest는 `98BDBA6369B6E4C78588E96DD8D14FFB13EA0681D8282C208EF45AD4D86F41EA`입니다.

새 native rawSHA가 runtime lease, normal helper receipt/admission, collector의 history/proof/prefix/mapper를 포함한10parent와 종료·정리까지 전파되어야 합니다. 과거 native 원문·SHA/PID/lease/IR를 새 성공 근거로 복사하지 않습니다.

## 실행 전 확인

현재 Source IR은 20,909B/SHA `6E7D977A93BFBD5C5AEADACCD41AD6272AF0C283C9D203EB5A302DA0B0B1C21A`, Control IR은 8,980B/SHA `D3E8E797B4D32D18D49DEFE075568416E3893E0DF20A0F2844561F1D9CDD97C3`입니다. 두 원본은 기존 공개 루트의 `l2-family15-gated-independent-review01/`과 `l2-family15-controlled-tests01/`에 CreateNew로 보존했습니다.

Source의 독립435검사와 IR 소비12, Control의 current Source IR 원문 재결속162검사·별도 정적69검사를 구분합니다. 중복·저자·과거 결과를 합산하지 않습니다. 실제 고정 공개 템플릿9를 사용한 native→runtime lease→normal helper/admission→collector history/proof/prefix→whole mapper/config2→Reader snapshot/window의 합성 양성과 단일 stale 핀 거부를 확인했습니다. source/control 수용은 이 통제 범위뿐이며 실제 Unity·SDK·기기 실행 수용이나 Root 권한을 뜻하지 않습니다. 각 하네스의 최초 실패와 fixture 정정도 보존합니다.

Root 실행 준비02는 원래 자원 창과 나중에 발행되는 lease의 기한이 어긋날 수 있다는 M1으로 수용하지 않았습니다. 미호출 준비03의 생성 오류는 PowerShell parser에서 차단됐으며 원 파일을 보존했습니다. 별도 준비04는 initial 직전 원 상한의 남은 시간을 계산하고, initial 직후 발행된 기한이 원 상한 안인지 검사한 뒤에만 stage/open을 진행하도록 했습니다. 이 검토는 실행 준비 코드 범위이며 실제 권한·실행 성공을 대신하지 않습니다.

## C15/cohort03 실제 결과

| 범위 | 상태 |
| --- | --- |
| 자원 창 | 시작02:29:43.4188516Z, 실제 반환02:31:40.4338098Z; 원13분/15분과 더 이른 lease 상한 안에서 반환 |
| 새 실제 native 신원·forward native7 | PID31348/start02:30:08.6472854Z, 원100ns 보존·두 경로 forward 확인 |
| compile4/defaultfalse4 | 첫 bridge 컴파일 요청 exit12/EDITOR_NOT_RUNNING·result null·innerSuccess false; 나머지7건 미실행 |
| 첫 publisher prepare-directories | 미실행 |
| 동일 held native 종료·watcher tool/Dispose | native Exit0, 실제 watcher tool71563 완료0/Dispose0·강제 종료0 |
| own4 cleanup·public3984/dirty18·P2보호5 | 임시4개만 제거, 공개3984/dirty18 및 별도 P2보호5 exact 확인 |
| Play·normal helper 실행·Queue·LD/ADB·Native 입력 | 이번 준비 검증 범위에서0 유지 |

시각은 UTC입니다. 첫 CLI의 diagnosticCount0은 컴파일 진단을 받지 못한 미수락 결과이므로 컴파일 통과가 아닙니다. 뒤의 RETURN 목적 등록 editor_status에서는 ready/컴파일false/Play stopped를 관측했고, InspectEditor는 같은 native PID/start100ns·clean Untitled를 확인했습니다. 이 나중 관측으로 앞선 요청을 성공으로 고치지 않습니다. Editor 실제 부재나 C# 코드 결함을 확정하는 근거로도 사용하지 않습니다.

종료 CLI6/COMMAND_FAILED와 wrapper1은 원문대로 실패입니다. 별도 watcher가 종료 전 보유한 같은 native handle에서 waitSignaled/실제 exit 정수0을 확인했고 Root가 실제 tool 완료와 Dispose0을 관측했습니다. native 결과만으로 tool 성공을 추론하지 않았습니다. closed guard는 native/owned/unknown0·TempLock false였고, fresh audit→cleanup13→own4 제거→후속 audit의 순서를 유지했습니다. private6는 sparse/부재 메타데이터만 확인했고 내용·해시는0입니다. 공개 P2 ProjectSettings의 보호 해시 확인과 private6를 혼동하지 않습니다.

반환 원본은 새 보고 루트 `D:/codex/validation/sudden-force-fps/20261103-work/resume-0210/cohort03-return-checkpoint.public.json` SHA `25A985F2A2FE931E9C75942DA7E6EC5C1904CF2BCE65D880016AF55C8ED34596`입니다. 실제 native7 원문 SHA는 `20F5F03879C3BA6197F51EE978C61578DD3C8ABDE1BEE467D18D64BBCD35DED0`입니다. 해당 루트의 `l2-c15-cohort03-actual-independent-review01/verification.public.json` SHA `70188573B491978E34AE76700389EDAA255AB79B85BE21D1AE465DD6AF1EB8C6`는 native 형식·반환/보존의 공개 영수증 부분만 수용했습니다. 전체 compile/default/publisher/L2의 noBlockers는false입니다. 검토자는 실제 OS·프로젝트·private 파일을 재관측하지 않았습니다.

## C16 준비와 별도 실제 회차

C15 실패·출력·IR·lease를 보존한 채 C16/revision16/cohort04/runtime-area04/phase04/family04로 namespace와 DAG 핀만 새로 포트했습니다. source63의 manifest는 `46DF34A5BDDD7298269DFD9223D0D6E34963FE923AA7C6F57360964251C7D4BB`이며 동작 변경은0입니다. Source IR19,272B/SHA `3E58B48719A16CD7AD2964C7DF3E4DF8E1012583B34AC97C359283C43FF1F6C1`, Control IR10,114B/SHA `CAEFAC69FE9402658EC191D430467D120A5E32DBE86ED5AFF8856145D56D67CC`는 정확 C16 바이트로 새로 검증했습니다. C15의 통과·미수락을 C16 실제 실행에 승계하지 않습니다.

다음 별도 Root 절차는 heldready/runtime lease 이후 자연 대기45초→등록 editor_status1회가 ready/컴파일false/reloadfalse/stopped/현재프로젝트임을 확인→InspectEditor의 현재 native PID/원UTC7·failedfalse·clean 상태를 확인한 뒤에만 compile4/default4를 진행합니다. 대기 시간 자체는 증거가 아니며, 상태가 준비되지 않으면 delegate 컴파일0으로 RETURN합니다. Inspector 읽기 전용 eval과 검증 대상 compile delegate 호출도 구분합니다. 원13분/15분 상한·closed5 raw pin/부모 reparse 검사와 종료·정리 범위는 유지합니다.

C16은 다른 프로젝트의 실제 반환과 JDK 설정 복구 보고를 받은 뒤 새 차례·scope·closed guard/audit·stage·native heldhandle로 실행했습니다. Root는 다른 프로젝트의 복구 보고와 자기 P11 fresh guard를 구분하며, 다른 프로젝트의 원시 설정·키·백업을 읽지 않았습니다. cohort03의 권한·PID·lease를 재사용하지 않았습니다.

| C16/cohort04 범위 | 실제 결과와 한계 |
| --- | --- |
| 자원 창 | 시작02:54:01.0152999Z, 반환02:57:31.9106195Z; 원13분/15분 및 더 이른 lease 안에서 반환 |
| native | PID46988/start02:54:05.7981136Z/ticks639292712457981136, MVID a98efde5-eaba-4db9-bbbc-e88bfda282f4 |
| 준비 상태 | 자연 대기 후 등록 status ready/컴파일false/reloadfalse/stopped와 실제 Inspector의 같은 native·clean Untitled·failedfalse를 확인 |
| compile4/default4 | 각각 실제 CLI0/innerSuccess true/sourceStable true/diagnosticCount0; compile delegate 미호출과 기본 false 본문의 입력·등록·query0 구분 |
| normal helper | 고정 원본9개 렌더·현재 lease/epoch 결속·admission 준비, 실제 실행0 |
| 첫 publisher | prepare-directories exit1, ops._current58행 REGISTERED_PUBLISHER_ONLY; 전체 수용false |
| 종료·정리 | CLI6/wrapper1 보존, 동일 held native Exit0·실제 tool11526 완료0/Dispose0·own4만 제거·후속3984/dirty18·별도 P2보호5 exact |
| 미실행 | Play·Nav·Queue·normal helper 실행·LD/ADB·Native instrumentation0; 전체 L2/timing/packet 수용false |

시각은 UTC입니다. 준비 Inspector·beforeplay·beforeclose의 result는 동일합니다. 이를 모든 내부 메모리·캐시·제품 상태의 무변화로 확대하지 않습니다. 반환 원본 `cohort04-return-checkpoint.public.json` SHA는 `FC569B0B585D1E00CB5D02A92AA02CF89E81429E25ED56D15D047251955C17C4`, Root의 실패 관측 요약 `cohort04-first-publisher-failure.public.json` SHA는 `7414A8DAEF0DB727F7A6A050D0CBB28E8C1603E26B3DD3D4A39E31F7C6FBB82B`입니다. 두 파일은 위 새 보고 루트에 있습니다. 요약 작성시각은 실제 publisher 실패시각이 아닙니다.

독립 실제 검토 `l2-c16-cohort04-actual-independent-review01/verification.public.json` SHA `1F9B9275E886E02EAC6AAEF822AF7BD1D364D215E5A0469015C1974AF39F1079`는 공개48입력·62항목을 대조해 위 부분만 수용했습니다. Root의 실제 tool 완료 진술과 공개 영수증을 읽은 검토이며 실제 OS·프로젝트·private 파일 재관측이 아닙니다.

### M7 경로 반례와 현재 C16 판정

publisher는 자기 raw `__file__`를 세 preparation action에 전달했습니다. 공유 io.snapshot은 ctx.consumerPath를 forward로 발행하지만 ops 등록 guard는 raw consumer와 등록 forward 경로, ctx.consumerPath를 각각 exact 비교했습니다. Windows/mixed 자기 경로에서는 두 비교가 실패합니다. 실제 raw consumer 값을 별도로 로그하지는 않았으며, 실제 오류 코드·stack과 독립 전체 함수의 Windows 경로 통제 재현으로 이 경계를 확인했습니다.

현재 Source false addendum SHA `C221EC15934CC21CE508AB160E8171EDAF400D1F7314D5A8234AE4E31FA39F2A`와 Control false addendum SHA `17B67618F8C2FA7C1A934264A0ADC9C2C1B42B69DEA58C58B0E953D35D21A10E`를 기존 각 family16 검토 폴더의 addendum01에 보존했습니다. 원 true IR·source63·actual 출력·alias를 편집하지 않았으며, 현재 C16의 전체 runtime/noBlockers/controlledTestsAccepted는false입니다. 컴파일·정상 반환의 부분 성공으로 이를 대체하지 않습니다.

## C17 새 후보의 준비 범위

C17/revision17/cohort05/runtime-area05/phase05/family05는 publisher의 자기 `__file__`만 `Path.as_posix()`와 동등한 slash 투영으로 발행하고, alias는 거부하는 helper를 추가했습니다. 8개 self consumer 위치를 같은 표현으로 연결하며 io.C 상수로 caller를 대체하거나 ops/io 등록 검사를 완화하지 않습니다. namespace와 DAG 외 기능 변경은 이 자기 경로 정합뿐입니다.

source63 manifest SHA는 `D34556EF39BD31E055E6C2D4DF1A6F1C6CCDDFEA83D1394DF84848E462D93F76`, Source IR20,791B/SHA `90FA83658B9805A57FDDF1EB32D1D5256ECABF140462F7ACE273ADFDE71DC507`, Control IR12,020B/SHA `BF2B8DF02CB6AD7BD8D775F24C2B98F37F5131075E8043EC6128D65EC8E624D0`입니다. Windows/mixed 자기 경로의 실제 publisher 함수·공유 snapshot·3 action/config2·whole mapper/Reader/P1과 foreign/alias/stale/partial 거부를 새로 확인했습니다. source/control 범위만 수용하며 실제 enabled CLI·staging·컴파일·publisher·권한·ready·L2 수용은 아직0/false입니다. C16의 POSIX-only 모델 누락과 실제 실패를 보존합니다.

다음 실제 회차는 새 차례·원 기한·현재 관측부터 시작하며 cohort04의 권한·입력·출력을 승계하지 않습니다. 실제 검증 전에는 다른 프로젝트의 사용/복구 반환을 다시 확인합니다.

## 현재 제품과 다음 범위

사용자 제공본은 코드34/1.0.196/내부 release12이며 새35 예약·빌드·서명·업로드는0입니다. 최신 FIRE 배치와 Quad D 인트로의 main 반영 및 PC 검증을 코드34 포함·Android Player 재생 수용으로 확대하지 않습니다. 실기기·장시간 성능 측정은 후속입니다. 현재 필요한 사용자 조치나 질문은 없습니다.
