# 보행 보정 07의 부분 개선과 남은 실패

관련 #341, #166, #41, Draft PR344. 제품 보행 클립·플레이어 프리팹에 연결하지 않았으며 제공 APK34에는 포함되지 않았습니다.

## 실제 관측

Unity 6000.3.25f1의 별도 Windows 검증 worktree에서 Grounded07을 한 번 실행했습니다. 같은 부모가 보유한 native PID34392의 시작은 `2026-11-07T06:53:01.6157307Z`였고 native/tool 종료는 모두1, 강제 종료 없음, Dispose 오류0입니다. managed 검증은17.3638507초였으며 최종 판정은 실패입니다.

|검사|관측|판정|
|---|---|---|
|양팀 cardinal skin 표본|66개, 실패0|통과|
|촘촘한 skin 표본|4,614개, 실패0|통과|
|방향·Idle 혼합 표본|2,048개, 실패510개|실패|
|최저 혼합 skin 정점|RunBR/phase0.75, Y−0.07415874m|지면 기준 실패|
|시간 연속성|WalkForward 최대 인접 변화13.2016916도(오른 종아리 key96→97), 최대 보정 변화10.361167도(위치 미특정)|기존12도/8도 기준 실패|
|원본 보호|Blue bake/Blue probe/Red probe의 보호 컴포넌트26개 및 상체49본 비교|해당3 fixture 범위 통과|

기존06의 WalkForward key3 오른발과 같은 thigh 입력에서 새07이 기록한 native `FromToRotation` 원 반환은 identity였습니다. 작은 acute 각도에 한정한 double-precision 회전 계산을 적용한 뒤 해당 발의 실제 잔차는 `1.90827976e−7m`로 기존0.2mm 기준을 만족했습니다. 06·07의 해당 thigh 입력15개 필드를 독립 비교했습니다. 이 관측을 Unity의 모든 회전 입력이나 내부 원인 확정으로 일반화하지 않습니다.

새74 PNG는 cardinal66개와 WalkFR 혼합8개입니다. 최악 RunBR 혼합 자세의 렌더는 포함되지 않았습니다. 일부 화면을 읽었지만 전체 모션의 시각 수용·새 소총 자세와 결합한 수용으로 처리하지 않습니다.

## 보존과 정리

07 소스 SHA256은 `B2E5DE4224D1992609F7C4F3B6BD7BA35E43BF3B5D7FD0A7AC4B666890860975`입니다. 독립 actual 검토에서233개 기존 출력의 불변, native 종료1, 혼합/연속성 실패, 같은 thigh 입력과 새 잔차를 대조했습니다. Root는 Editor 종료 후 임시 helper/metadata3개를 외부 보존·제거하고 해당 실행의 공개 추적 입력4,061개를 실행 전 바이트로 복원했습니다. 기본 P2의 기존 설정5개는 유지했습니다.

새 접지 허용 오차·다리 늘리기·root/hip 이동을 적용하지 않았습니다. 새 Android 생산 버전 예약·빌드·서명·업로드도 이 실행에서는0입니다. 07의 남은 두 실패를 해결하기 전 materialization과 제품 연결은 차단합니다.

## 다음 작업

WalkForward의 짧아진 knee pole 방향과 혼합 자세의 실제 full-weight skin을 별도 A/B 후보08에서 수집합니다. 새로운 pole 안정화는 기존 수용 기준을 유지하며, 혼합 후 추가 보정에 두 번째80mm 예산을 주지 않습니다. 원 raw blend 기준의 누적 보정량·전체 skin 접지·연속성·소총 자세·같은 시점의 부위별 판정과 실제 경기/기기를 차례로 검증해야 합니다.

외부 근거는 `D:/codex/validation/sudden-force-fps/20261107-work/ik-precision-candidate01/`, `ik-precision-actual-review01/`, `root-resume01/grounded-precision-native01/`, `root-resume01/editor-window01-return.public.json`입니다. 이전 실패와 입력/결과는 덮어쓰지 않았습니다.
