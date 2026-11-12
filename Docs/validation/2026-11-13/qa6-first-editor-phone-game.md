# QA6 첫 Editor·공기계 경기와 반환 기록

2026-11-13 KST, issue #347/#341의 첫 QA6 통합 두 피어 표본입니다. 빌드·runtime 입력은 P13 HEAD `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`이고, main `7c4…`는 문서 기록 기준입니다. PR345·PR386은 Draft, 운영 code34/release12·원장196,2,34는 그대로입니다. APK·설치·readback은 [앞선 산출물·공기계 기록](qa6-artifact-and-phone-install.md)을 참조하며 여기서 반복하지 않습니다.

## 준비와 첫 경기

새 native51492의 원창은 KST02:30:04~new02:55:04/full03:00:04입니다. 원 stopped MVID `643d29f7-a2c9-4f03-8844-e32f450e93fb`/QAflagfalse에서 임시 QA define 적용 후 Play MVID `eb8a07bf-0c5a-4aa0-b1fa-26996fde19ed`/43 Networked properties가 정착했습니다. 실제 Lobby manager의 cached AppVersion은 `sff-q20261022-ladder-v1-intro1-riflecore2-gaitraw1-localqa`로 일치했습니다. source18·원 native/window 핀에 결속한 RAM compile은 Load1·Arm(false)3·Reader(false)1·enabledArm0·fixture0 및 scene/roots/dirty/frame 보존을 기록했습니다.

정상 Lobby→Room Ready/START 뒤 `Q6N13H01`에서 Editor Host와 공기계 client, 상반 팀 actor1031/1032의 Running 경기·roster2를 확인했습니다. 시작/종료 Reader 표본은 양 actor poseReady·12 configured/root-aligned hitbox, pose failure/gaitFaults/lifeHolds0입니다. 두 actor 모두 Host의 StateAuthority 관측이므로 순수 proxy 렌더 검증은 아닙니다.

## 위치 준비 거부와 정상 입력의 한계

setup은 `REJECT_NO_CLEAR_FLAT_POSITION_5_TO_8M`으로 종료했습니다. TeleportAttempted/Returned=false이며 양 actor Before/After DTO 전체가 같고 NonSetupFieldsUnchanged=true입니다. GLOBAL_REMOVED 뒤 늦은 frame82064의 fixture0을 확인했으며, 위치 준비 성공이나 강제 배치로 합산하지 않았습니다.

공기계의 정상 joystick+SPRINT 좌표 두 pointer 프로필은 **code6 gate 실제 실행·28event·errorCode0·terminal code0·injectedAll=true·activePointersCleared=true**를 남겼습니다. 호출자가 CRLF 포함 문자열을 trim하지 못한 첫 실패와 원문은 보존했고, 같은 출력의 공개 파싱만 정정했습니다. 입력 재주입/프로필 재실행은0입니다.

후속 Reader에서 공기계 Blue actor1032의 평면 위치 변화 약7.25m와 gaitPhase0→1.8794173을 확인했습니다. 이것은 시작·끝 표본입니다. 중간 Sprint flag/velocity·실제 질주 속도·연속 이동 외형·자연스러운 파지 전체·적용 pose frame은 저장하지 못했으며 미수용입니다.

## Timer·결과·로비 반환

Host timer는 `APPLIED tick=21662 oldTarget=26912 target=22302 remaining=10`을 기록했습니다. 적용 tick의640tick 뒤 target과 cleanup/늦은 frame98920 fixture0을 확인했습니다. 뒤 Reader는 양 팀0점의 Draw/TimeExpired이고 폰 결과 화면도 일치했습니다. 다만 첫 종료 표본 tick28721은 새 target과 원 target 모두 지난 시점이므로, 정확한10초 벽시계·만료 callback 시각 측정으로 확대하지 않습니다.

정상 RETURN 후 Editor는 새 Lobby Runner/actors0/fixtures0까지 관측했으나 마지막 상태는 **Connecting**입니다. 공기계는 PUBLIC ROOMS 로비 UI를 확인했습니다. 재연결 완료·새 경기 재참가·Connected callback 완료는 미수용입니다.

## 물리 반환과 full 이후 CPU 원복

Play Stop 후 원 symbols 복원을 요청했고 정착한 stopped MVID643d…/QAflagfalse/clean Intro를 확인했습니다. 종료 eval RPC의 Invalid response format 실패는 성공으로 고치지 않았습니다. 별도 소유 native handle 영수증이 **ExitCode0·forcedfalse·Prefs2 원복·cleanupFailures0·globalUnity0**을 기록합니다. 폰도 정상 EXIT 후 PID 부재, 강제종료 fallbackfalse/helper 부재, 설정1.0/0/0·같은 ADB 세대를 확인했습니다.

물리 자원 반환 기록은 UTC17:56:33(KST02:56:33)으로 원 full18:00:04(KST03:00:04) 전입니다. 종료 직후 source 감사는 changed3/restoredfalse였고 이 상태를 보존했습니다. 세 파일의 원본·생성본을 모두 보존한 뒤 CPU에서 분류·CAS3 복원을 수행했습니다. ProjectSettings는 CRLF1023, ShaderGraph는 CRLF19이고 Resolver는 CRLF81·빈 scalar 끝공백2 외에 **실제 bool3 변화**가 있어 전부 형식 변화로 취급하지 않았습니다.

최종 Root 영수증은 UTC18:01:38(KST03:01:38)의 public4122 fresh exact·Gitclean·private6 absent-indexS 및 개인 파일 내용/해시/복사0을 기록합니다. **CPU 소스 원복 완료는 원 full 이후**이며, 물리 반환과 혼합해 full 내 전체 원복 성공으로 소급하지 않습니다. native 종료 뒤 native 호출은0입니다.

## 근거와 남은 범위

공개 근거 폴더는 `D:/codex/validation/sudden-force-fps/20261112-night/qa6-runtime-launch01/actual-0f3f31c7e5604f8a8125d8c50b467dc1/`입니다.

- `independent-round1-review.public.json`: 9,580B/SHA256 `B7F847D70160FCFEC7440B0BD178D1B4E780331DA4EC862D014A02ACD62F1C3C`. 제한된 경기·반환 및 실제 result 핀입니다.
- `independent-round1-source-return-addendum.public.json`: 2,501B/`342DD82A15EC4F5C888602D1BA6FD2048BEFAC31A939CF696409A4E4CF158347`.
- `source-return01/final-source-return.public.json`: 1,157B/`2CC3FBBF35C87E040256BB164450E83E45AAF4585B20645CD19447B6689B0C98`. 추가 검토는 Root 영수증 대조이며 reviewer의4122 재해시/Git 재검사는0입니다.

순수 proxy·HP 피해/명중·kill/respawn·life 경계, 시각 보행/질주와 이동 중 pose, 실청·실제16KB·전체 권리는 미완료입니다. QA6 전체 Game/제품 raw gait·PR345 단독 core39·PR386 전체 수용이나 운영 배포로 확대하지 않습니다. 작성자의 새 실행·재검증은0입니다. 현재 필요한 사용자 조치나 질문은 없습니다.
