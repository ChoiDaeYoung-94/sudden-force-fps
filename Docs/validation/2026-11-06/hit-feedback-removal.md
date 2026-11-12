# 중앙 명중 표시 제거

사용자 피드백에 따라 상대 명중 시 중앙 표식과 `HIT`/`HEADSHOT`/`ELIMINATED` 및 피해량 문구를 표시하지 않습니다. 체력·탄약·재장전·K/D와 명중·피해·처치 판정, 킬 피드는 기존 경로를 유지합니다. 본인 피격 방향이나 혈흔을 새로 구현한 변경이 아닙니다.

`CombatHudView`의 명중 이벤트 구독·표시 함수·타이머만 제거하고, 재바인딩 때 기존 직렬화된 표식을 비웁니다. 참조 필드는 유지하므로 씬·프리팹의 재저장은 없습니다.

## 확인한 범위

Unity 6000.3.25f1의 별도 Windows batch Editor에서 실제 DesertHouse `CanvasUI`의 비활성 PreviewScene 복제본을 검사했습니다. lifecycle callback을 명시 호출하고 실제 `CombatPresentation` 이벤트 backing delegate에 합성 데이터를 전달했습니다. 자동 lifecycle·물리 입력·Fusion 경기 시험이 아닙니다.

- 명중 fixture 12개: 살아 있음·메뉴·사망·새 대상으로 재바인딩의 네 그룹에서 각각 일반 명중·헤드샷·처치를 발행했습니다. sentinel 전달과 HUD 명중 구독 부재, 표식/텍스트 숨김을 확인했습니다.
- 체력·바·탄약·재장전·빈 탄창·K/D·죽음 안내·리스폰, 메뉴 열기/퇴장 확인/취소/복귀와 재바인딩을 확인했습니다. vitals 발행 호출 7개는 현재 HUD 전달 6개와 해제된 이전 대상의 무변화 1개입니다.
- assertion 295행이 모두 참입니다. 배열 참조 등의 반복 검사가 포함되며 295개의 독립 시나리오라는 뜻은 아닙니다.
- 공개 입력 6개의 실행 전·후·정리 후 핀, 보통 씬의 root/dirty/active 상태와 정적 이벤트 구독 복원을 대조했습니다. preview 개수와 모든 객체의 부재를 별도로 전수 검사한 것은 아닙니다.
- 원래 보유한 native process handle의 ExitCode 0, 실제 도구 종료 0, 강제 종료 없음, Dispose 오류 0을 별도로 확인했습니다. helper의 종료 요청만으로 판정하지 않았습니다.

첫 회차는 함께 staging한 정지 자세 검증 helper의 `Light.renderingLayerMask` 형식 오류 `CS0266`으로 컴파일 단계에서 중단됐습니다. HUD 본문 실행은 0이었으며 원 로그·종료 1을 보존했습니다. 해당 helper를 명시적 int 변환으로 수정하고 새 출력 회차에서 위 결과를 얻었습니다. 첫 시도를 통과로 합산하지 않습니다.

Editor 종료 뒤 EDM 설정 한 파일의 개행/빈 값 끝공백 변화를 발견했습니다. 생성본과 Git blob·실제 사전 SHA에 맞춰 재구성한 원본을 보존한 뒤, 닫힌 Editor에서 그 경로만 복원했습니다. 재구성본을 사전 물리 백업이라고 표현하지 않습니다. 준비한 공개 추적 입력 4,055개는 복원 후 모두 일치했고, 이 준비 기준에는 의도한 HUD 수정이 포함됩니다. 임시 helper/metadata 5개와 새 빈 폴더를 보존·정리했으며 P2의 기존 설정 변경 5개도 유지했습니다.

## 남은 범위

실제 경기의 명중·사망/리스폰 및 Android 포함·기기 검증은 후속입니다. 제공 APK는 코드34/1.0.196/release12 그대로이며 이 변경은 미포함입니다. 새 Android 버전 예약·빌드·서명·업로드는 하지 않았습니다. #162/#41 전체 완료와 구분합니다.

외부 근거: `D:/codex/validation/sudden-force-fps/20261106-work/resume-0330/`의 `hud-actual02/`, `hud-actual-independent-review01/`, `hud-edm-serialization-restore01/`, `hud-helper-cleanup.public.json`, `p12-after-hud-restoration.public.json`, `fps-resource-return01.public.json`입니다. 원시 로그·키·자격정보는 공개 문서에 포함하지 않습니다.
