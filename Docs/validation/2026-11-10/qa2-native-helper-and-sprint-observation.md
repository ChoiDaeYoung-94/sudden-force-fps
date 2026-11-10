# QA2 입력 도우미 갱신과 Sprint 실제 관측

[Case05에서 확인한 QA1 전용 helper](qa2-continuous-gait-and-bootsole-observation.md)의 target 버전 검사 한 줄만 code2/`1.0.197-qa2`로 바꿨습니다. 새 무서명 빌드·독립 DEX/AXML 검토·기존 키 서명·독립 V2/RSA 검토를 거쳐 LD2에 갱신했고, 실제 두 피어 Game에서 Sprint+이동과 이동+사격 해제 순서 두 가지의 native profile을 실행했습니다. **세 주입은 정상 반환했고 Sprint/속도·위상 및 사격 상태 변화를 관측했습니다. 전체 달리기·접지·양손 IK·입력 생명주기 수용은 후속**입니다.

게임은 같은 D7 소스/별도 localqa/code2/`1.0.197-qa2`이며 APK100,492,368B/SHA `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33`가 그대로입니다. [Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)·[이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)과 운영34/release12·원장196,2,34를 유지합니다. 휴대전화·Play/Drive 업로드는0입니다.

## 도우미 산출물과 설치

helper Java는 한 줄, 무서명 builder는 경로/SHA/IR/DEX literal 다섯 곳만 바뀌었고 역변환 바이트 일치를 확인했습니다. 기존 same-Root/same-core 서명 cohort는 별도 helper-only signer에서 이미 검증한 QA2 Game02의 결과·core·signed APK 고정 pin 계약으로 명시 교체했습니다. game-only signer 권한이나 소비된 game 승인을 재활성화하지 않았습니다.

새 무서명 helper11,180B/SHA `41542109068A62F2FFBA878933B93996F47A1E4D25E59E638C57C26D32F1BA53`는 정확 ZIP2개·CRC/SHA·무서명·DEX4정렬을 통과했습니다. 독립 typed AXML은 min28/target34/compile36·testOnly·자체 instrumentation/Game query·권한/Activity0을, DEX는 실제 code2/QA2 문자열의 버전 거절 분기를 확인했습니다. SDK API36과 helper 선언 target34를 구분합니다.

기존 키로 서명한 helper는16,528B/SHA `8A0D7A48065EB4962433268676CB539D21209FD54151146A2091052D5D4ADB80`, 인증서SHA `4D072592228C991D3F6A0EDA768C8CBEE40895C3B1F4A6FB9379540F048965E0`입니다. SDK sign/verify/zipalign Exit0와 별도 RSA2048/V2 content digest·인증서·ZIP payload/순서/method 보존을 독립 확인했습니다. Root precheck의 state/status 키 기대 오류는 서명0으로 보존한 뒤 정확 schema로 재검증했습니다. 암호/키 내용의 parent 읽기·해시·복사·출력과 새 키/백업/이동은0입니다.

SDK 단위는 원 UTC06:52:16.485514/새 작업 중지07:04:16.485514/전체 반환07:07:16.485514 안에07:00:21.107418 실제 반환했습니다. builder worker0·owned Job active0·EOF2·cleanupUnknown[]와 signer Native0, P2 보호5개 사후 일치·build7/sign7/auth3 승인 회수를 구분해 기록했습니다.

Case06의 별도 원창은07:05:31/새 작업 중지07:25:31/전체 반환07:30:31, nonce `30392fdeeb0640bfa3af1c76a1770b63`입니다. helper만 -r로 갱신하고 전체 installed APK SHA가 새16,528B와 일치함을 확인했습니다. game의 전체 installed readback도 위100,492,368B와 같았습니다. game install/clear/uninstall은0입니다.

## 실제 동시 입력과 한계

정상 방 참가·READY/START 뒤 Game/CanCombat·양 actor HP100, LD2의 현재 Game PID/foreground/HUD를 확인했습니다. 일반 Client 호출을 모두 반환한 뒤 같은 원 lease/server/device/clock의 새 config/evidence로 한 번만 단계 전환했습니다. 기존 C19 Source와 일반 Client 메모리는 변경하지 않았고 예기치 않은 단계 전환 실패 시 별도 정리가 필요하다는 한계를 인계했습니다. 실제 단계 전환은 성공·실패0입니다.

첫 `two-fire-up-first`는 오른쪽 첫 보조 포인터를 Sprint 버튼에 배치한 명시적 Sprint+이동 시험입니다. 뒤의 두 profile은 FIRE 버튼을 사용해 Fire 먼저/이동 먼저 해제했습니다. 세 instrumentation 요청 모두 정상 반환했으며 주입 완료와 게임의 모든 중간 해제 효과 수용을 구분합니다.

실제 네트워크 상대를 따라간15초/5Hz 진단75 PNG·metadata에서 Sprint4표본·최고속도약7.411m/s·보행 위상 변화를 관측했습니다. 소스의 run blend 속도 조건은 충족하지만 모든 방향/전체 달리기 주기·자연스러움·접지까지 통과한 결과는 아닙니다. 같은 표본에서 ShotSequence0→18/Ammo30→12와 마지막 이동0·Sprint=false/Shot18을 관측했습니다. 두 해제 순서의 정확한 중간 타이밍·모든 입력 패킷·피해/킬·사망/리스폰은 미수용입니다. 진단 카메라/관측은 actor transform·health·Networked 상태를 쓰지 않았고 camera cleanupErrors0·가림 pause0이었습니다.

## 종료·보존·다음 작업

정상 메뉴 퇴장 뒤 Editor는 Lobby/actor0/camera0·clean scene을 확인했습니다. LD 마지막 화면은 로비 전환 중이었으므로 최종 LD Lobby는 수용하지 않고 force-stop/HOME으로 정리했습니다. caller Native0·23개 source 호출 모두 반환·활성0·실패0이며 별도 helper 설치 단계6호출과 구분합니다. 호출 수는 기능 시험 통과 수가 아닙니다.

clean Untitled와 기존 runInBackground=true 복원 뒤 Exit 요청 CLI6은 보존했고 보유 Editor16052의07:13:19.4492789 Native Exit0/not forced·EOF2·Dispose0·prefs 두 값1 복원을 확인했습니다. owned rsp/meta2 회수 당시 EDM1차이를 외부 보존하고 baseline으로 되돌린 뒤 **07:13:59.492702 Root 감사에서 공개4,095개 raw exact·private6 absent/indexS·D7 Git clean·임시2 부재**를 확인했습니다. binding/config/lease 회수와 원 new/full 이전 자원 반환을 기록했습니다. 전체 unknown child 종료와 Root 파일 감사의 독립 재현은 별도 미증명입니다.

외부 근거는 `D:/codex/validation/sudden-force-fps/20261110-work`의 `qa2-helper-unsigned-actual-independent-review01`, `localqa-qa2-helper-existing-key-sign-actual-independent-review01`, `root-resume01/qa2-helper-sdk-return01`, `root-resume01/case06-helper-install01`, Case06 caller/holder와 `editor-case06-final-return01`에 보존합니다. 원시 로그·개인/서명 입력은 공개하지 않습니다.

한정 독립 실제 검토는 helper 갱신 전후 UID/dataDir/firstInstallTime 기록 일치, source23콜·3profile/각28주입 이벤트·75 PNG metadata와 선택4시각 표본, 종료/회수 기록을 대조했습니다. 주입 성공을 release 순서별 게임 효과 임계나 전체 자세 수용으로 확대하지 않습니다.

| 자료(위 루트 기준) | bytes | SHA256 |
| --- | ---: | --- |
| `localqa-qa2-helper-existing-key-sign-actual-independent-review01/verification.public.json` | 3,593 | `D8F9D613015A1B856D64C3D71024DA4666E4506DC8FD064648BF55819E334856` |
| `qa2-case06-actual-independent-review01/verification.public.json` | 8,321 | `F46EBB9DE53AD81A95D831CD9C787FCC7282E11A8ABB85F09A56499BA8C55C0A` |
| `root-resume01/editor-case06-final-return01/final-audit.public.json` | 1,145 | `8229170A31C0D2820F1CFF87CD5DE9795F7AC501B2F2D870EEE75940E8DBE99C` |

다음은 동시 입력 해제의 패킷/화면 시간 결속, 반대 역할·남은 방향/조준·사망/리스폰과 접지/소총 품질 검토입니다. LD1의 흰 화면 원인·실제16KB·음향/장시간 성능·전체 권리는 남습니다. 현재 필요한 사용자 조치·질문은 없습니다.
