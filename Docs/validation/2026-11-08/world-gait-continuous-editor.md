# 연속 Editor 보행 표본 검증 — 2026-11-08

[이전 보행 기록](../2026-11-07/world-gait-postblend-readiness.md)에 이어 continuous04의 실제 Editor update 샘플과 독립 검토를 기록합니다. Game/제품 보행 완료는 아닙니다. Draft PR #344 / #341의 후속 근거입니다.

고정 계획을 callback당 1회 평가하여 568 frame JSON·52 PNG·result 1개, 총621파일/11,822,970B를 생성했습니다. 독립 IR은 전후 전체 pin·집합, PNG CRC/IDAT와 44종의 해제 픽셀을 확인했습니다. 원본 source61 보호·소총 보정 전 visual49 보호, 원 skin renderer8, 3개 MVID 안정 및 cleanup 오류0은 공개 결과에 기록돼 있습니다. 역사 Rest61은 불변 입력으로 재사용했으며 새 Rest 관측이 아닙니다.

평면상대 fullskin 최소 +0.999745mm, sole 최소 +0.9996928mm, raw 대비 foot 보정 최대67.2569mm입니다. 평면을 다시 빼지 않았습니다. Editor 표본 span16.0916061초와 logical1/30초는 Game FPS가 아닙니다. 저장 수치 검토이며 독립 Unity geometry 재실행은 없습니다.

일반 Stop243의 왼쪽 ball 인접 이동은476.024mm(raw foot323.500mm), steady Run 최대는321.737mm(raw261.646mm)입니다. 최대 quaternion 변화45.813°는 Run161입니다. 고정 clone에서 같은 관절의 인접 표본 간 3D거리이므로 NCC 이동·80mm 보정량과 다릅니다. 원인·새 정지 버그·자연성 통과를 확정하지 않았고 해당243 PNG도 없습니다. Idle0과 Stopped256의 leg12 world 위치/local 회전 및 PNG 바이트는 정확히 같습니다.

Native25312는 UTC10:44:13.5927515→10:45:01.8891356, exit0/forced=false/Dispose 오류0·outer tool0입니다. 원창10:39:31/new10:51:31/full10:54:31을 유지하고10:50:38.670758에 반납했습니다. Root 공개 proof상 prefs 두 값1, Editor0, 서버760의 현재 연결 peer 확인·ADB payload0입니다. listener 소유권 증명은 아닙니다.

첫 stage는 PowerShell hash 변경으로 프로젝트 쓰기 전에 차단됐습니다. Valid Microsoft Windows 서명과 새 D8F182… 핀을 결속한 운영 revision2를 사용하고 원 binding/window를 보존했습니다. Restore01은 EDM 개행·줄끝 공백1개로 BLOCKED를 남겼습니다. 양쪽 보존·정규화 동등 확인·exclusive CAS 뒤 Restore02에서 공개4067 rawexact/own62+4폴더 부재를 확인했습니다. Root proof상 P13 Git clean/P2 보호5 불변입니다. 소비한 운영4binding은 원바이트 archive 후 승인 회수했습니다.

full source-to-IL, 하드웨어 skinning/GPU frame, 동영상·자연성, 물리·NCC·Game·2peer·기기·제품은 미수용입니다. offset=null, 제품3SHA empty/12 positive=false를 유지합니다. QA APK17FC 및 운영 code34에는 이번 실험이 없으며 새 예약·서명·업로드0입니다. 다음은 동일 baseline Editor+phone의 정상 Room/Game, live NCC·실제 support plane·속도 전환 관측입니다.

외부 근거 기준은 `D:/codex/validation/sudden-force-fps/20261108-work/`입니다. 아래 SHA-256으로 원본을 식별합니다.

- `gait-continuous04-actual-independent-review01/verification.public.json` (2999B): `1CE0B0EF6E34759BE5AEB8445E6D54280F8A401ECECD98C224C42E69EAC7AF0E`. 상세 REPORT(6167B): `6B5DA127215A9EE3F37CA693DD0AD0F1C385AD5AFBD33670FF4ADD680C2B02AB`.
- `root-resume01/continuous04-actual-return01.public.json`: `FF2ECE5CCDB8B750DA2AA4DEEFCE7533019069FBA1A769E11264B25883496FD1`.
- `root-resume01/continuous04-operational-binding-retirement01.public.json`: `50573A93EB18E231124078E44DA8B8AD27C6E2DBED24930C10BB77A4737C688B`.
- `gait-continuous-stage06/actual-e5c1fff890cd4b7997a658990e9a92d8/restore-02/restore-result.public.json`: `01DB07BDFCDF923A53069E6725FDE3E1EAD07DADED52C2F66C905C90D73B3859`.
