# 재장전 손가락 자세의 기준 관측

#188의 다음 grip-pose 설계를 위해, PR282 제품 소스를 사용하는 별도 P11 검증 공간에서 양 팀 리그의 관절 데이터를 읽었습니다. 제품·프리팹·애니메이션을 변경한 작업이 아닙니다. P11은 기존 QA overlay를 보존한 `f055e4a` 공간에 제품 C# 2개를 복사한 상태이며, 최신 main 전체의 clean checkout 검증과 구분합니다.

## 실제 완료 범위

- 새 batchmode/Null 그래픽 Editor의 nonPlay·깨끗한 빈 scene1/root0/preview0에서 Blue·Red clone을 순차 한 개씩 사용했습니다. 원 scene의 이름은 빈 문자열이며 이름 변경이나 새 scene 생성은 하지 않았습니다.
- 각 팀의 Idle0 product support와 pull(.4375)에서 왼손15·오른손15관절, local TRS·무기 상대 pose·실제 distal 자식 이름·탄창 mesh bounds·palm proxy/cache를 읽었습니다. 네 표본에 관절 관측120행입니다.
- 실제 private Apply/Reset 경로의 phase 복원 확인4건, cleanup Reset2건, load/unload 각2건, 오류0·source9/scene 보존을 확인했습니다. 내부 보호 검사에서 손가락 local TRS와 허용하지 않은 변환을 지키도록 했습니다. 출력하지 않은 전체 보호 스냅샷을 별도로 재계산한 결과는 아닙니다.
- helper의 Render/BakeMesh/material 접근·game time·입력·네트워크 상태·프로젝트 저장·새 finger 회전 작성 경로는0입니다. SDK 내부 전체 호출을 계측한 profiler 결과는 아니며 실제 owner의 자연 lifecycle이나 피부 접촉 검증도 아닙니다.
- 후속 공개 입력3,984개 raw exact와 private6 부재/sparse를 확인하고, 동일 native process handle의 정상 종료 ExitCode0·강제 종료/저장0까지 확인했습니다.

미호출 compile-only와 disabled 경로는 각각 diagnostic0, 정의/Run 미호출 및 queries/clones/files0입니다. 이 진단을 Game 전체 컴파일 경고0으로 확대하지 않습니다. 초기 준비의 경로 표기 검사와 첫 후보의 scene 이름 검사에서 차단된 시도는 별도 보존하며 성공 횟수에 합산하지 않았습니다. scene guard는 관측된 빈 이름/root0에 맞춘 별도 후보02에서 다시 검토·컴파일·실행했습니다.

## 다음 설계에 사용하는 범위

양 팀은 같은 phase의 관절 수치가 같고, Idle→pull의 손가락 local TRS도 같습니다. 관측한 distal pivot은 손끝 피부나 접촉점이 아니며 직접 자식도 없습니다. 관절과 탄창 AABB 사이의 거리는 실제 mesh gap·관통·필요 curl 각도로 해석하지 않습니다. 현재 자료는 기존 pose의 기준이며 최종 finger target quaternion의 근거로는 부족합니다.

다음 최소 확인은 손 표면과 실제 탄창 표면의 간격·법선, 좌우 mesh 귀속과 같은 카메라의 가림입니다. 이를 확인한 뒤 왼손 grip-pose 한 개를 기존 phase window에 혼합하는 범위를 검토합니다. 오른손·손목 IK·입력/피해/탄약은 보호하고, .97/종료·자연 중단/리스폰·rig 교체에서 자기 출력을 복원하는 새 검증이 필요합니다. 임의 관절 각도나 출처 미확인 기존 Reloading clip은 적용하지 않았습니다.

## 보존한 근거

로컬 루트 `D:/codex/validation/sudden-force-fps/20261027-work/`의 다음 자료를 사용합니다. 원시 관절/기하 데이터는 로컬에 보존합니다.

- `reload-finger-measurement-candidate02/actual-8fd0b1089ece4bb9be6da9f4fcfc64e2/result.public.json`
- `reload-finger-measurement-independent-review01/`, `reload-finger-measurement-independent-review02/` — core/조건부 wrapper와 scene guard delta
- `reload-finger-measurement-actual-independent-review02/` — 실제 결과의 독립 파일 검토, 범위 내 차단0. 원문 result SHA256 `5262E9D9DA169F86033C44652C7EB4FA625A7408360BA5E491F3FC2B5183C705`
- `p11-finger-compile-only02.public.json`, `p11-finger-disabled02.public.json`, `finger-measurement-root-binding02.public.json`, `p11-finger-actual02.public.json`
- `p11-finger-headless-after-public01.public.json`, `p11-finger-after-normal-close01.public.json`, `p11-finger-headless-normal-close01.public.json`
- `reload-finger-measurement-next-design02/ANALYSIS.ko.md` — 작성자의 후속 수치 분석이며 독립 수용 검토와 구분

이번 관측으로 #188 전체를 닫지 않습니다. 코드34 QA APK/AAB에 새 grip-pose가 포함된 것도 아니며 새 Android 번호 예약·빌드·서명·업로드는0입니다. 현재 필요한 사용자 조치·질문은 없습니다.
