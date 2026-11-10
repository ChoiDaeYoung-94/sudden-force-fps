# QA2 공기계의 입력 해제 순서와 자세 관측

새 공기계 SM-A175N과 GPU Editor의 정상 두 피어 경기에서 두 손가락 입력 세 종류를 시험했습니다. **사격을 먼저 놓고 이동이 이어지는 상태, 이동을 먼저 놓고 사격이 이어지는 상태와 마지막 중립 복귀를 호스트 저장 상태에서 확인**했습니다. 별도 측면 촬영에서는 보폭 변화와 총구 섬광도 관측했습니다. 이는 [이슈341](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/341)/[Draft353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)의 부분 근거이며 전체 입력·보행·양손 IK·접지 수용은 후속입니다.

게임은 D7 `d7a3371296357a5ceb1b7b657d468415130e7047` 기반 localqa/code2/`1.0.197-qa2`입니다. 공기계의 게임 APK100,492,368B/SHA `3A4CD98CDE7C42C60CA0A37CFEC00324254980CB7CCB09B4E59EC780A2BD8A33`와 helper16,528B/SHA `8A0D7A48065EB4962433268676CB539D21209FD54151146A2091052D5D4ADB80`를 이 회차에서 전체 readback하여 고정 산출물과 대조했습니다. 운영34/release12·원장196,2,34와 설치 QA2는 그대로이며 새 빌드·서명·Play/Drive 업로드는0입니다.

## 실제 입력과 호스트 상태

원창은 UTC16:44:44/신규 중단17:04:44/전체 반환17:09:44, nonce `98e48a553c5f4b3f9e178914f5d4d0d2`이며 연장하지 않았습니다. Editor의 실제 Play 프레임 증가·로비 연결·compile/update 비활성과 같은 ADB generation/exact connected peer를 확인한 뒤에만 공기계 protocol을 열었습니다. 정상 닉네임·방 생성·팀 선택·READY/START로 DesertHouse의 양 actor HP100/생명0·Game/CanCombat에 도달했습니다. 구체적인 입력 해제 시험은 모두 Editor host가 관측한 원격 phone actor입니다. 반대 authority/proxy 역할은 이 회차에서 실행하지 않았습니다.

일반 Client 호출을 반환한 뒤 같은 원 lease/server/device/세 시각에 묶인 새 config/evidence로 한 번만 native 단계를 열었습니다. 이전 Client 메모리와 source/artifact binding은 유지했고 실제 rebind는 성공했습니다. 준비된 별도 실패 정리 도구는 사용하지 않았습니다.

| 시험 | 오른쪽 포인터 | helper 결과 | host 상태 표본 |
| --- | --- | --- | ---: |
| Sprint+좌측 이동 | Sprint 버튼, 먼저 해제 | 28이벤트·모두 주입·포인터 해제·session finish·종료0 | 359 |
| 사격 먼저 해제 | FIRE 버튼 | 같은 조건 충족 | 359 |
| 이동 먼저 해제 | FIRE 버튼 | 같은 조건 충족 | 360 |

첫 시험의 모드 문자열은 `two-fire-up-first`지만 오른쪽 좌표를 Sprint 버튼에 둔 명시적 Sprint 시험입니다. 호스트에 Sprint가 켜진22표본과 이동44표본을 기록했습니다. 나머지 두 시험에서 helper의 실제 이벤트 순서와 host 저장 상태를 별도로 대조했습니다. 사격 먼저 해제에서는 Fire=true+이동21표본 뒤 Fire=false+이동10표본이 있었고, 이동 먼저 해제에서는 Fire=true+속도0인6표본에서 탄약14→13·ShotSequence16→17을 관측했습니다. 정지 기준은 평면 속도0.05m/s 이하입니다. 이동 먼저 해제의6표본 사이 actor 위치는 약3.209mm 변했으므로 완전한 위치 고정을 뜻하지 않습니다. 세 기록의 마지막 표본은 저장 버튼 중립·속도0이며, 전부 HP100/같은 생명0·관측 전후 같은 runner tick입니다.

각 state sampler는 최대12초·목표30Hz로 실행했고 총1,078표본을 확보했습니다. 이는 `PreviousButtons`와 보행 속도 등 **호스트에 저장된 상태**이며 raw `GetInput`/MoveX·MoveZ 패킷이나 입력 소비 tick의 기록이 아닙니다. Python 호출 전후 UTC/Stopwatch bracket은 guard·APK/PID 확인·실제 호출·사후 확인까지 포함합니다. 서로 다른 clock의 helper 이벤트/Editor 표본을 정확한 native UP 시각에 결속하거나 입력 지연·FPS로 해석하지 않습니다. 같은 tick이라는 관찰도 원자적 스냅샷 보장은 아닙니다.

## 별도 보행·소총 화면

위 세12초 기록이 끝난 뒤 별도로 body/Sprint+이동, rifle/사격+이동의15초 촬영을 수행했습니다. 두 촬영은 각75개, 합계150 PNG/1280×720·목표5Hz의 진단 카메라 출력입니다. 첫 body 표본 간격32.926ms를 포함하므로 일정한 실제200ms 간격을 보장하지 않습니다. native helper는 추가 두 회 모두28이벤트·정상 포인터 해제/종료를 반환했습니다. 각 camera는 pause0·cleanupErrors0·callback 회수와 소유 camera 부재를 확인했습니다. actor/Networked 상태는 쓰지 않았습니다.

선택 body 프레임에서 보폭과 다리 자세 변화·정지 자세 복귀, 선택 rifle 프레임에서 소총과 받침손·총구 섬광이 보입니다. 측면 일부 표본으로 모든 조준 각도·반대쪽 방아쇠손·정확한 어깨 접촉·발 미끄러짐/접지·사망/리스폰을 수용하지 않습니다. 별도 진단 카메라 위치와 생성 비용을 정상 플레이어 화면·게임 FPS로 간주하지 않습니다.

camera의 원본 claim에는 이전 회차의 `actualNativeTouch=LD2_SEPARATE_ROOT_CONTROL` 고정 문구가 남아 있습니다. 원본을 고치지 않고 이 기록으로 정정합니다. 이 회차 입력은 새 공기계의 phone03/PID29289와 같은 lease/evidence의022/023 profile이며 LD 입력은0입니다. 두 모드 문자열은 모두 `two-fire-up-first`이고 body의 Sprint 의미는 실제 버튼 좌표와 Root 조작 분류입니다.

## 실패와 정리 범위

최초 `Assets/Scenes/LoginPlay.unity` 열기는 존재하지 않는 경로로 실패했습니다. 해당 eval의 내부 실패를 검사하지 않고 clean scene에서 Play에 진입한 실수를 보존하며, 즉시 Play를 끝내 실제 `Assets/Scenes/Login.unity`로 열어 새 Play와 로비 연결을 확인했습니다. 모두 공기계 protocol 전에 발생했습니다. 이후 `HasFinalResult`를 사용한 읽기 eval은 멤버 부재 컴파일 진단으로 실행0이었고, 실제 HUD의 final 관찰과 정상 `ReturnToLobby`로 정리했습니다. 프로젝트 소스 재컴파일·domain reload·import·빌드·SDK/Prefs 변경을 phone 활성 중 수행한 것은 아닙니다.

`tag` 요청 한 번은 `TAG_ROWS`로 거절돼 진단 표식 결과를 수용하지 않았습니다. 재시도하지 않았고 helper 원본·host 상태·camera 출력과 구분합니다. 시작32요청+최종 정리2개로 source34call 모두 반환, active0입니다. 기능 시험34개 통과라는 의미는 아닙니다. 공기계 메뉴의 정상 LEAVE MATCH→PUBLIC ROOMS와 host의 final→정상 Lobby/Connected를 확인한 뒤 force-stop/HOME·phone caller 실제 Exit0(UTC16:54:56.5331644)을 확인했습니다.

그 다음에 Editor Play를 끝내 닉네임 메모리를 원래 빈 값으로 돌리고 clean empty scene에서 정상 Exit를 요청했습니다. 보유 Editor30856은 UTC16:55:20.7183383에 Native Exit0/not forced·EOF2·disposeErrors0으로 종료했습니다. 시작 시 runInBackground=true를 썼으나 최초 실패 eval에서 이전 값을 저장하지 못했으므로 이전 메모리값 복원을 주장하지 않습니다. 이 Editor는 종료했으며 전역 Save0입니다.

관측 중인 자손8개는 동일 generation이 모두 사라졌고 direct-parent 후보0/all native Editor0, 두 ADB 종료 Prefs의 원래DWORD1 복원과 같은 server generation/exact connected peer를 확인했습니다. 전체 birth history나 unknown child 전수 종료 증명은false입니다. 공용 ADB 시작/종료/재시작은0입니다.

소유 rsp/meta2를 외부 보존 후 회수할 때 EDM1개 변화가 남았습니다. 생성1904B/SHA `3F5496C530EACDCBF09AFF9A5F2A3836D5321DD975606503EB8FE2BA684BD193`와 원본1983B/SHA `01E2562923C891ACD39BCBD35BCA486695032950F41637A0FF4B8A410C2B1B6F`를 둘 다 보존하고 원본 pin과 exact 일치하는 Git blob/EOL 후보로 그 경로만 복원했습니다. 최종 Root 감사는 공개4,095개 raw exact·private6 absent/indexS·D7 Git clean·임시2 부재였습니다. 생성본의 의미 동등성이나 전체 메모리/캐시 원복 주장은 아닙니다.

모든 holder/phone 승인·lease를 회수하고 **UTC16:56:20.3454333에 실제 전체 반환**했습니다. 원래 신규 중단/전체 반환 전이며 현재 공유 Editor/기기 호출·예약은0입니다. 권위 P2의 기존 dirty5·다른 프로젝트 앱/자료·서명 키·운영 원장은 보존합니다.

## 검토와 다음 작업

외부 근거 루트는 `D:/codex/validation/sudden-force-fps/20261111-work`입니다. `release07-actual-review01`, `camera03-phone-actual-review01`, `root-resume01/case07-full-return.public.json`, 각 sampler/camera 원본·phone caller/helper/bracket과 native holder/Prefs/원복 결과를 함께 따릅니다. 원시 로그·기기 식별자·private lease·자격정보는 공개하지 않습니다.

다음은 반대 역할의 phone host Game·남은 방향과 조준·생명주기/입력 취소·12 hitbox 및 접지/총기 품질입니다. 시작 배경 #362는 별도 source 수정만 main에 있으며 현재 QA2에 미포함입니다. 새 Android 산출물·실제 cold A/B·음향 청취·16KB·장시간 성능·전체 권리 수용은 후속입니다. 현재 필요한 사용자 조치·질문은 없습니다.