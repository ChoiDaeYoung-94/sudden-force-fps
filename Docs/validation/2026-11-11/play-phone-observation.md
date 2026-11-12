# #43 폰 Google Play 상세 화면 관측과 미완료 범위

2026-11-11의 저장 증거를 독립 검토한 결과입니다. 운영 패키지 `com.AeDeong.SuddenForceFPS`의 상세 URI를 한 번 열었을 때 화면에 **“항목을 찾을 수 없습니다.” / “다시 시도”**가 표시됐습니다. 최종 typed probe는 **`UI_XML_MISSING` / `FAILED_OR_UNKNOWN`**으로 실패했습니다. 이 제한된 화면 관측과 실패·반환 기록을 남기며 **#43은 열린 상태로 유지합니다.**

## 실행·확인 결과

| 항목 | 확인한 범위 |
|---|---|
| 상세 조회 | intent 1회 정상 반환. 저장 화면 전후 focus는 같은 정확한 허용 Play component와 일치했습니다. |
| 실제 화면 | 저장 PNG에 항목 없음/다시 시도 문구가 보였습니다. FPS 제목·제공 버전·트랙·설치 가능 여부는 확인하지 못했습니다. |
| typed UI | UI 읽기 명령은 반환했지만 payload 33바이트는 dump 완료 표식뿐이고 XML/hierarchy가 없었습니다. 고정 parser의 `UI_XML_MISSING` 재현을 확인했습니다. |
| 판정 | 원 summary의 `exactPlayFocusObserved=false`, `fpsCorrespondenceObserved=false`, UI 상태 5개 unknown을 유지합니다. 사후 focus 확인으로 probe 전체를 PASS로 바꾸지 않습니다. |
| 정리 | 자체 HOME 1회가 exit0으로 반환했고 Root는 10:12:27.692856 UTC에 원 full 이내 반환을 기록했습니다. HOME 뒤 새 focus·런처 화면은 확인하지 않았습니다. |

원창은 UTC 10:11:47.049989 시작, new 10:15:47.049989, full 10:16:47.049989였습니다. 설치·opt-in·계정 선택/로그인·동의/나이·게임 실행·운영 package metadata 읽기는 0입니다. 공개 config 승인 6개 false는 독립 검토자가 읽었고, private lease 승인 4개 false·퇴역은 Root 반환 증거로 구분했습니다. 저장된 같은 server 세대·exact peer 반환 증거는 확인했으며 현재 자원을 다시 조회한 결과는 아닙니다.

**HOME exit0는 실제 HOME 화면 확인이 아닙니다.** Root 반환 파일의 final-focus 표식은 마지막 저장 Play focus의 주석/상수이며, HOME 이후의 새 측정이 아닙니다. 원격 UI 읽기 프로세스의 완전 종료도 미입증입니다.

## 이전 시도와 설치본 구분

Play04 첫 준비는 optional metadata 권한의 명시 false 누락으로 constructor에서 거절돼 wire0·UI0이었습니다. 다음 Play04는 intent 정상 반환 후 닫힌 component 목록 불일치로 실패했고 자체 HOME 반환을 보존했습니다. Play05는 실제 관측된 동일 Play 패키지의 정확한 component 하나만 별도 허용한 후속이며, 최종 결과는 위 XML 수집 실패입니다. 이력과 source-only 검토를 추가 성공 횟수로 합산하지 않습니다.

선행 preflight의 policy 한정 `safe`와 Root의 별도 화면 확인은 당시 새 창의 전제였습니다. 이를 현재 폰의 잠금 상태나 자동 실행 승인으로 재사용하지 않습니다. 운영 code34/Play release12는 기존 제공 기록이며 이번에 현재 Console 트랙이나 폰의 운영 설치 버전을 조회하지 않았습니다. 별도 패키지의 QA3 code3 직접 설치·경기 결과도 Google Play 설치 성공과 구분합니다.

## 남은 검증과 완료 기준

항목 없음의 원인은 계정·테스터 등록/참여·제공 상태·기기 중 어느 것으로도 확정하지 않았습니다. UI XML 수집 실패의 원인도 별도 미확인입니다. 다음은 새 현재 자원 범위에서 수집 형식을 제한적으로 확인하고, 지정 테스터의 실제 참여 상태·대상 운영 앱/제공 트랙을 확인하는 작업입니다. 기존 증거를 지우거나 같은 nonce로 자동 재시도하지 않습니다.

#43 완료에는 **등록 테스터 자격/참여 상태 확인과 별도 지정 기기에서의 실제 Google Play 설치 또는 갱신**, 설치된 운영 패키지·서명·버전 및 기본 실행의 확인이 필요합니다. 이번 화면·사후 focus·HOME 명령 반환으로 이를 완료 처리하지 않습니다. 계정/결제/테스터 설정 변경, opt-in, 설치·로그인·PGS는 이번 관측으로 수행하거나 수용하지 않았습니다.

공개 요약은 [play-phone-observation.public.json](play-phone-observation.public.json)에 결속합니다. 원시 focus·serial·계정·private PNG/로그는 공개 문서에 포함하지 않습니다. 현재 필요한 사용자 조치와 질문은 없습니다.
