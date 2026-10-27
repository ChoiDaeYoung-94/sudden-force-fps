# 재장전 손 기준과 총 구도 개선

관련 이슈는 #188·#166이며, 소스 기준은 `cf2aadcaf72c4e8ab4cef14612387f67646e7cc0`입니다. 재장전할 때 양손이 겹쳐 동작이 잘 보이지 않던 구도를 개선합니다. Idle 리그의 손가락 관절에서 손바닥 기준을 한 번 구해 왼손을 탄창에 맞추고, 재장전 중 총을 왼쪽으로 4cm 이동·yaw -25도·roll -12도 기울입니다. 새 구도는 공통 진행 곡선을 사용해 장전 끝부분에 사라집니다. 기존 탄창 이동량·상하 lift·조준/피해 판정은 유지합니다.

## 완료한 검증

| 검증 | 확인한 범위 |
| --- | --- |
| 소스·컴파일 | 제품 C# 2개 독립 검토와 P11 새 Game 어셈블리 컴파일 diagnostic 0. 물리 CRLF 소스와 커밋 LF blob은 정규화 본문 일치로 구분 |
| 정지 구도 | 별도 복제 리그의 4안 비교에서 왼쪽 구도를 선택. 오른쪽 구도는 개머리판이 시야를 가려 제외 |
| 복제 리그 회귀 | 양 팀 각 6진행률과 반복·직접 OnDisable 호출·NaN·잘못된 MCP의 복원 확인. 12진행률, 사례별 복원 확인 20건, 오류 0. 자연 사망 callback 검증은 아님 |
| 실제 owner | Windows05 host + 새 소스 P11 Editor client의 자연 경기 1회. 일반 UI로 방/팀/READY/START를 거쳐 사격으로 탄약 30→29, 정상 `RequestReload()` 합성 호출 1회 뒤 30 복귀 |
| 시간적 자세 | 167개 자연 프레임 기록과 idle/reach/pull/insert/settled PNG 5개 모두 해당 구간의 요청 전·렌더 begin/end·요청 후 상태 대조 통과. poseFrame은 모든 기록에서 현재 frame |
| 장전 끝 구도 | 진행률 .9765625 이상인 장전 중 기록 10개에서 새 X 이동·회전 0. 처음 관측된 장전 완료 프레임도 0. 기존 상하 lift blend는 그때 .842682958로 별도 감쇠 중 |
| 조준 보호 | 이 표본에서 카메라 위치/회전·Aim·simulation shot origin 변화 0. 오른손 grip 거리 최대 약 0.97µm, 각도 변화 0도 |
| 정리·보존 | observer callback/임시 렌더 자원 정리, GameView/테스트 닉네임 복원, Play 종료·clean Untitled 복귀. 전체 공개 입력 3,984개 전후 raw exact, private6 부재/index sparse 유지 |
| 정상 종료 표본 | 경기 시간 만료→Windows05 결과→로비→EXIT/OK 후 보유 process handle ExitCode 0·Crash literal 0. P11 Editor도 별도 정상 Close/ExitCode 0. 강제 종료·프로젝트 저장 없음 |

P11은 기존 QA overlay를 보존한 detached `f055e4a`의 검증 공간에 제품 C# 2개만 복사했습니다. 최신 main 전체의 clean checkout 빌드가 아닙니다. Windows05 host는 이전 바이너리이며 새 손 구도는 Editor client에서 확인했습니다. 첫 P11 컴파일 때 공개 입력 3,984개 중 의도한 두 C# 외 3,982개를 보존했고, 이후 동일 3,984개 baseline을 비교했습니다.

## 남은 수용 범위

- 손·탄창의 피부 접촉/비관통과 전체 재장전 애니메이션 완성은 #188에 남깁니다. 관절 proxy 정렬과 구도 개선을 mesh contact 수용으로 바꾸지 않습니다.
- 자연 사망/respawn·bind·중단 시점의 실제 owner, 장전 직후 첫 실제 사격, 물리 R 키·모바일 touch 수용은 이번 표본에 포함하지 않았습니다.
- PNG는 URP StandardRequest의 구간 대조 근거입니다. 실제 화면에 제시된 GPU 프레임과의 완전한 일치나 입력 지연 측정은 아닙니다.
- 정상 Quit 1회로 #154의 기존 native crash 원인을 해결했다고 하지 않습니다. 실제 Android·음향 청취·16KB 기기·장시간 성능도 미검증입니다.
- 로컬 코드34 QA APK/AAB와 마지막 확인 Play33/release11에는 이 변경이 포함되지 않습니다. 새 Android 번호 예약·빌드·서명·업로드는 하지 않았습니다.

## 로컬 근거

검증 루트는 `D:/codex/validation/sudden-force-fps/20261027-work/`입니다. 원시 로그·계정/서명 정보는 공개하지 않습니다.

- `reload-palm-product-fixture01/actual-90b12e553ac141b6969af5c372d6809f/`와 `reload-palm-product-fixture-actual-independent-review01/` — 복제 회귀와 독립 검토
- `reload-current-owner-observer-candidate02/actual-04f938d2fee040779d0543153608ac8c/` — 실제 owner 결과 SHA256 `F4988C2EDFCD008B52CA40F71DA0871EAC09B6063D96BE2842798F45E8CF6A5C`
- `product-reload-observer-root-binding02.public.json`, `p11-product-observer-start02.public.json`, `p11-product-normal-reload-request02.public.json` — 새 프로세스/owner 결합과 1회 호출
- `product-reload-owner-analysis02.public.json` — 구도 복귀/조준 보호의 좁은 계산 결과
- `p11-product-after-normal-close01.public.json`, `p11-product-gui-normal-close01.public.json`, `reload-product-gameplay01/host/process-result.public.json` — 전체 공개 입력 보존과 정상 종료 표본

현재 필요한 사용자 조치·질문은 없습니다.
