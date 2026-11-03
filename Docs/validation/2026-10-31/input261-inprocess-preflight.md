# P11 InProcess 읽기 전용 사전 관측 (#259)

2026-10-31 새 회차의 선행조건만 관측했습니다. Intro PR307과 목적을 분리하며 B1/L2·PR261 수용 결과가 아닙니다.

대상은 `D:/codex/worktrees/sudden-force-fps-windows-20261026-01`, HEAD `f055e4a`와 기존 dirty18입니다. Root는 공개3,984개 baseline·비공개6개 actual absent/indexS를 확인했습니다. 비공개 내용·해시 조회0이며 최신 main clean checkout으로 표현하지 않습니다.

GUI Editor PID41124/start `2026-10-31T00:06:25.6944136Z`에 세션 한정 InProcess import 옵션을 적용했습니다. ready eval은 innerSuccess=true·진단0, Win64/nonbatch, Play·transition·compile·update·failed·build=false, DOTNET1/BEE1과 game MVID `db62160d-f13a-41b2-b222-f685dc73a32b`를 보고했습니다. 별도 엄격 가드는 native1/owned41124/unknown0이며 raw commandline 반환0입니다.

실제 activeImportMode=InProcess·session desiredWorkerCount=0입니다. configured standby2는 설정값이며 현재 worker2나 session override의 증명이 아닙니다. runningWorkerCountProven=false·isAssetImportWorkerProcess=false를 유지합니다. R29 후보의 schema 접두사만 변경한 관측 소스 SHA는 `DA49C835…A328D`이고 전체 값은 실행 영수증에 있습니다.

직후 공개3,984개는 사전 대비 변경·추가·누락0입니다. Play·Bridge staging·config/binding·Queue·입력·ADB·Save·Build·자격정보 사용0입니다. 이번 unknown0은 R29 unknown2의 역할·원인 해결이 아니며 B1 normal poll·bounded stop·native held 사망/리스폰·L2는 미실행입니다. PR261 Draft/#259 OPEN을 유지합니다.

정상 CLI close0과 사전 보유 native handle의 실제 정수 ExitCode0을 확인했습니다. 이후 fresh native0/unknown0·TempLock 부재, 공개3,984개 전체 raw exact/추가·삭제0 및 기존 dirty18을 확인했습니다. 개인6은 실제 부재/indexS를 유지하며 내용·해시는 조회하지 않았습니다. 이 종료는 당시 세션의 선행 관측 마감이며 실제 B1 입력의 성공 근거가 아닙니다.

R31 근거: `p11-ready-attempt01.public.json`, `p11-ready-strict-guard01.public.json`, `p11-import-mode01.public.json`, `p11-ready-public01.public.json`, `native-exit-p11/p11-readiness01.public.json`, `p11-after-close-guard01.public.json`, `p11-after-close01.public.json`. R31은 `D:/codex/validation/sudden-force-fps/20261031-work`입니다.
