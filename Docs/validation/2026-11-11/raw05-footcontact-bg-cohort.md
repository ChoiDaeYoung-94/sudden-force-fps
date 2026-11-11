# Raw05 밑창 높이: 같은 BG의 실제 299표본 (#373)

**실행.** [Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)의 게임 소스 `d7a3371`을 연 Editor를 호스트로, QA3 폰을 client로 연결해 Idle→일반 UI 왼쪽 이동 요청 2초→release 한 번을 관측했습니다. 폰의 설치 APK 전체 읽기는 code3/`1.0.197-qa3`, 100,492,368B, SHA-256 `DD663868…D953698`와 일치했습니다. Editor D7에는 QA3의 인트로 관련 두 파일 변경이 미반영되어 있으므로 두 실행 소스의 동일성을 주장하지 않습니다. 처음 비공개 방(`visible=false`) 시도는 정상 복귀했고, 이어 공개 방(`visible=true`)을 새로 만들어 두 피어가 진입했습니다. 이를 두 번의 유효 측정으로 합산하지 않습니다. 별도 input_profile/도우미 없이 일반 Android tap/text/swipe를 사용했으며 제품 수정·새 APK 빌드·배포는0입니다.

**확인.** 같은 actor/life와 같은 거의 수평인 BG support의 한 cohort에서 약 10초 설정의 299표본을 채택했고 거절은 0이었습니다. 독립 검토는 이 수치와 source/runplan/compile/start/입력 결속을 수용했습니다. 아래 값은 같은 표본에서 구한 BG에 대한 Y 간격의 중앙값이며 단위는 cm입니다. `lower sole`은 좌·우 boot 정점(각 352개)의 현재 baked 최저값 중 낮은 값입니다.

| 기록된 gait speed 구간 | 표본 | CC 바닥 | lower sole | lower sole−CC |
|---|---:|---:|---:|---:|
| 이동 속도 발생 전 | 107 | ≈0 | 0.558879 | 0.558879 |
| 속도 >0.01m/s | 55 | 2.999997 | 1.220545 | −1.779452 |
| 기록 속도 0 복귀 뒤 | 137 | 2.999991 | 3.558869 | 0.558879 |

앞뒤 기록된 gait 속도0 구간의 lower sole−CC는 약 **0.558879cm로 같았고**, CC와 밑창의 월드 높이는 함께 약 3cm 상승했습니다. 이동 중 lower sole의 월드 최저값은 **−1.106cm**, 음수는 **15표본**이며 lower sole−CC 범위는 **−4.106~2.669cm**였습니다. 이동 구간의 lower sole 월드 최댓값은 5.669cm였습니다. 이 관측만으로 정지 후 월드 높이 약 3.56cm를 고정 리그 오프셋이나 일괄 보정량으로 정하지 않습니다.

**한계.** lower sole은 stance·실제 접촉 상태의 정의가 아닙니다. `HasStateAuthority`, 실제 적용된 `pose.LastContext`, 같은 pass의 frame 시각이 없어 기록된 actor gait 값과 실제 렌더 pose context의 일치를 확정할 수 없습니다. 기록 속도 0 뒤에도 actor 위치가 약 9mm 변했으므로 완전한 물리 정지로 부르지 않습니다. SkinWidth 0.03m와 상승량의 근접성은 물리 원인의 증명이 아닙니다. `summary.startedUtc`는 원래 자원 창 시각이고 observer나 입력 시작 시각이 아닙니다. 2초는 일반 swipe의 요청값이며 host call 약 2.716초도 실제 주입 지속·정확 latency가 아닙니다. 접촉·전체 보행/달리기·IK·GPU 픽셀·성능의 통과로 확대하지 않습니다.

**반환과 다음.** probe는 callback 해제·자체 Mesh 회수·cleanup 오류 0을 기록했고 호스트는 일반 메뉴 경로로 퇴장했습니다. Root는 Editor 정상 Exit0, 폰 caller Exit0/EOF·join·자체 GAME 종료/HOME, 승인 퇴역 및 Prefs 복원을 확인했습니다. 자동 형식 변경 두 파일은 생성본을 보존한 뒤 ShaderGraph의 LF 19개를 원래 CRLF 바이트로, EDM을 정확한 원본 바이트로 각각 복원했습니다. Root의 공개 4095개 raw exact/private6 부재·index S/Git clean 수용 후 전체 반환 시각은 **10:37:55.847536 UTC**, 원래 full **10:38:03 UTC** 이내였습니다. 원창은 연장하지 않았습니다. 별도 독립 반환 검토는 저장된 종료·회수 근거를 수용하고 공개4,095개/780,045,101B를 직접 다시 해시해 차이0을 확인했습니다. private6은 부재만 직접 확인했고 index S·Git clean·registry 상태는 Root 실제 receipt 범위로 구분합니다. 반환 IR SHA-256은 `05F226AA7040021BADD6DAC18708F7F3A63DE1B75B9912EC01607BDF0111B73C`이며, 이 반환 수용도 gait 품질 통과와 구분합니다.

다음은 같은 actor/평면에서 실제 사용 pose context·authority·기존 NCC 상태를 함께 기록하는 작은 읽기 전용 재현입니다. 원인을 분리하기 전 offset·scale·SkinWidth·contact flag나 제품 코드는 바꾸지 않습니다. 근거는 `footcontact-cohort-actual-independent-review01`의 statistics/verification과 원 runplan·probe summary·일반 퇴장·전체 반환 기록입니다. **현재 필요한 사용자 조치 없음입니다.**

운영 제공34/`1.0.196`·Play release12와 Draft353은 유지합니다. 이번 수용은 #373의 한 cohort 관측 범위이며, #341/보행·소총·접지 전체와 #43/Play 설치를 닫는 근거가 아닙니다. [수치와 근거 요약](raw05-footcontact-bg-cohort.public.json)을 함께 읽습니다.
