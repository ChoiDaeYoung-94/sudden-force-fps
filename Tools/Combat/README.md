# 발사 피드백 검증

## 원격 재장전 음향 (2026-10-26, #238)

원격 actor에게 발행되지 않는 `LocalVitalsChanged`만 기다리던 재장전 음향을 보완했습니다. 로컬 event 경로는 유지하고 원격은 표시 snapshot을 읽어 재장전 시작 edge를 한 번 재생합니다. 로컬2D/원격3D와 기존 clip·SFX·거리 설정을 유지하며, 만료·사망·리스폰 대기·비활성 AudioSource의 edge는 재생하지 않습니다. bind 때 이미 진행 중인 상태는 seed하여 과거 음향을 재생하지 않습니다. reload clip도 binding에서 미리 로드합니다. `PresentedReloadCount`는 PlayOneShot 요청수이며 실제 청취의 증거가 아닙니다.

fresh PC 두 Editor 피어에서 호스트4발/재장전의 탄약30→26→30, 클라이언트 proxy의 요청 증가1·3D/SFX/Loaded와 자체 관찰 콜백 정리를 확인했습니다. 자체 음량0 FX clone의59항목은 local/remote·중복·만료/NaN·사망/대기·비활성/재활성·실제 OnEnable/OnDisable 및 원본 delegate/scene 보존을 통과했습니다. clone 검사는 복사 snapshot이며 실제 player state를 쓰지 않습니다. 실제 bind-during-reload 표본2개는 미실행이고, isPlaying은 다른 소리도 포함하므로 개별 clip 출력·스피커 지연·Android 청취를 통과로 합산하지 않습니다. 제공 APK33에는 아직 미포함입니다.

## 새 owner 연결 시 사격 해제 경계 (2026-10-25, #228)

`PlayerInputSource.Bind`에서도 현재 FIRE held 상태를 release gate에 반영합니다. PC는 현재 왼쪽 마우스 버튼을 읽고, mobile은 이미 알고 있는 held를 보존하며 포커스가 없으면 해제를 요구합니다. Bind 전에 누르고 있던 왼쪽 버튼을 유지한 채 오른쪽 클릭으로 cursor를 잡는 것을 새 사격 누름으로 취급하지 않습니다. Bind 뒤 해제 상태를 샘플한 다음의 새 왼쪽 클릭과 mobile 새 PointerDown의 기존 false→true 동작은 유지합니다. 커서·탄약·네트워크 판정은 변경하지 않습니다.

구소스는 Bind에서 held/gate를 false로 지웠습니다. 보통 HUD의 Waiting→Running `SeedAim`이 이를 보완하지만, 이미 Running인 상태에서 새 input을 처음 관측하면 해당 호출이 보장되지 않습니다. 이 소스 경계를 독립 검토해 최소 초기화 블록만 수정했습니다. 실제 경기 자동사격 재현이나 발생 빈도를 확정한 변경은 아닙니다.

P8에서 새 게임 어셈블리의 실제 컴포넌트 release/cancel/focus/pause/pointer 회귀22조건은 실패0이며 자기 객체0·singleton 부재·preview 종료·원 Login 씬과 dirty 보존을 확인했습니다. 이 회귀는 Bind·spawned owner·물리 마우스/터치·실제 cursor 재획득을 실행하지 않으므로 해당 수용은 #162/#166에 남깁니다. 근거는 외부 `20261025-work/`의 `fire-bind-audit/`, `fire-bind-review01/`, `fire-bind-release-regression01.public.json`입니다. **제공 APK33에는 미포함**입니다.

## 포커스 상실·입력 취소 뒤 사격 해제 유지 (2026-10-24, #217)

입력 초기화는 사격을 멈추면서 이미 알고 있는 버튼 held 상태를 release gate에 보존합니다. 포커스 상실·pause/resume·Unbind·pointer cancel 뒤 계속 누른 값을 다시 받아도 발사가 재개되지 않으며, 실제 release를 알리는 `SetFire(false)` 뒤 새 press는 허용합니다. 메뉴·리스폰의 기존 SeedAim 경로와 발사/탄약/네트워크 규칙은 바꾸지 않습니다. 새 owner 바인딩의 모든 물리 입력 경계까지 검증한 것은 아닙니다.

Unity6000.3.25f1의 격리 PreviewScene에서 실제 PlayerInputSource와 CombatPointerControl을 호출한 동일22조건은 수정 전7실패, 수정 후0실패였습니다. callback과 held 재표본은 합성했으며 결과 필드는 읽기만 했습니다. 다른 pointer의 Up 무시·자기 Up 해제·새 press 허용·반복 reset·기존 crouch gate를 포함합니다. 두 실행 뒤 fixture 객체0·singleton 부재·preview 종료·원 scene/dirty 보존을 확인했습니다. 물리 마우스/터치·실제 권한자 입력·네트워크 경기 통과로 확대하지 않습니다.

공개 재현 source와 전후 결과는 외부 `D:/codex/validation/sudden-force-fps/20261024-resume/FireReleaseFixture.cs`, `issue217-before01.public.json`, `issue217-after01.public.json`에 보존합니다. 이 수정은 이미 제공한 코드33 APK에 미포함이며, 앞선 경기말 탄약0 관측의 원인으로 확정하지 않습니다.

## 로컬 예측 효과와 확정 명중 분리 (2026-10-22, #162)

기존 참가자 기기는 반동만 예측하고 총구 효과·총성은 방장의 ShotSequence가 도착한 뒤 재생했습니다. 이제 허용된 forward 발사에서 로컬 표시를 큐에 넣고 Render에서 재생합니다. 명중·피해·실제 탄약·재장전 권한은 방장에 유지하며 서버 hitscan에 탄환 비행 대기는 없습니다. 확정 impact·히트마커는 기존 권한자 관측을 따릅니다.

로컬 예측과 확정 발사의 총구·총성을 분리해 방장의 이중 재생을 막습니다. 재조정으로 예측 타이머가 되감겨도 마지막으로 허용한 표시 tick부터 기존 무기 간격을 지킵니다. 생명/바인딩 변경은 큐를 초기화하고, 이미 허용된 마지막 발사는 같은 생명에서 경기 종료 직후에도 표시합니다. 첫 발에 이펙트를 생성하던 비용은 바인딩 때 준비하며 사용 중인 PCM 총성도 미리 로드합니다.

최신 상태를 한 번 보여주는 기존 정책을 유지하므로, 렌더 전에 여러 발이 처리되면 효과를 한 번으로 묶고 SequenceDelta로 차이를 표시합니다. 예측 총성을 서버 교정 뒤 되돌릴 수 없으며 모든 패킷 손실/교정에서 발사와 효과의 일대일 대응을 보장하는 변경은 아닙니다.

## 실제 확인과 한계

Unity6000.3.25f1의 실제 두 피어 경기에서 새 Editor 참가자와 기존 Windows 검증 Player를 연결했습니다. 합성 NetworkEvents 입력으로 정상 연사11회/확정11회/탄약11 감소, 빈 탄창7회/확정7회/탄약7→0, 새 Editor 방장11회/확정11회, 재장전 뒤6회/확정6회/탄약30→24를 확인했습니다. 실제64Hz/무기0.1초의 최소7tick을 지켰고 방장의 중복 원격 효과는0입니다. 재장전과 동시 사격의 첫 효과는 약2.009초 뒤였습니다.

소프트웨어 이벤트 기준 로컬 효과가 확정 관측보다 먼저 발생했고 총성 AudioSource 출력도 확인했습니다. 물리 버튼·스피커 지연 또는 일반적인 지연 벤치마크는 아닙니다. 기존 원격 발사2회의3D/거리2–45m/Logarithmic/SFX 연결과 활성 AudioListener1개도 확인했습니다. 실제 기기·패킷 손실·거리별 청취 시험은 후속입니다.

초기v2의2tick/약9.8ms 중복 실패와 첫 fixture 실패는 보존했습니다. v3 첫 시험의11효과/Delta 합12는 렌더 전2발의 묶음으로 확정12·탄약12 감소와 맞았으며 발사 누락으로 판정하지 않습니다. 종료 경계 검사는 실제 호스트에서 합성 Finished 상태와 큐를 사용한 fixture이며 정상 승리 마지막 kill 시험은 아닙니다. 최종 재확인은 컴파일 up_to_date/오류0·경고0이며 처음v3 통합 때의 새 컴파일 완료와 구분합니다.

검증 자료는 외부 `D:/codex/validation/sudden-force-fps/20261022-quality/`의 fire-audit 및 fire-runtime-v3 계열입니다. 현재 Play/Drive 코드27과 무서명28에는 이 변경이 포함되지 않습니다.
