# Quad D 인트로 통합 준비 (#302)

2026-10-30 P2, 변경 전 main `02a482c9e97d293077553d76884295ac1b776fa5` 기준입니다.
패키지 설치·새 씬 저장·제품 코드 컴파일 후 Editor Play를 실행했습니다. 최초 검은 출력은 프로젝트 측 UI adapter로 보완해 **Simulator에서 로고 표시와 반복·로비 복귀를 확인했습니다.** 별도 GameView의 표시 문제·새 두 peer 네트워크 회귀·Windows/Android 포함 검증이 남아 있어 Draft를 유지합니다.

## 제품 구성

- 회사 저장소 [com.quadd.intro](https://github.com/ChoiDaeYoung-94/ChoiDaeYoung-94/tree/55bf9e4aa48c2f95942f3c2c995f1d55414e97c0/unity/com.quadd.intro) 0.1.0을 고정 커밋으로 설치했습니다. Unity UPM `Client.Add` 1회를 사용했고 manifest를 직접 편집하지 않았습니다. 기존 다른 manifest/lock 의존성은 같습니다.
- 새 `Assets/Scenes/Intro.unity`에는 Camera·AudioListener·AudioSource·VideoPlayer·QuadDIntro·SuddenForceIntroCanvasOutput이 각각 1개입니다. 영상은 3.4초/1920×1080/30fps/음성 트랙1, track0 enabled→자기 2D AudioSource, volume0.65/mute=false입니다. NextSceneName은 Login, persistent 완료 listener는0이며 별도 skip 입력은 연결하지 않았습니다.
- Intro 중 생성할 게임 Manager/Runner는 없습니다. 출력 adapter가 실행 중 자기 자식 Overlay Canvas·배경·RawImage/FitInParent를 생성합니다. 패키지 Start의 CameraNearPlane 설정 후 LateUpdate에서 APIOnly texture를 UI에 연결하며, 패키지의 영상·오디오·종료·장면 전환은 유지합니다. 자세한 실제 범위와 한계는 아래 후속 검증을 따릅니다.
- Editor 시작 redirect는 정확한 Intro 경로만 허용합니다. 기존 Test/Login/다른 개발 씬 정책과 Player 분기는 유지합니다.

| enabled 씬 | 이전 index | 새 index |
| --- | ---: | ---: |
| Intro | 없음 | 0 |
| Login | 0 | 1 |
| Lobby | 1 | 2 |
| Room | 2 | 3 |
| DesertHouse | 3 | 4 |

Fusion SceneRef index 변화 때문에 NetworkRunnerManager는 원래 AppSettings의 복사본에 `-intro1`을 한 번 붙이고 manager별로 재사용합니다. 첫 Lobby `customAppSettings`와 같은 runner의 StartGame `CustomPhotonAppSettings`에 같은 복사본을 전달합니다. Global/asset AppVersion을 쓰지 않습니다. 이전34와의 정상 매칭 공간을 분리하는 설계이며 실제 혼용 차단·새 양쪽 peer 접속은 미검증입니다.

BuildSettings는 Intro3줄과 Unity가 함께 직렬화한 `m_UseUCBPForAssetBundles: 0` 한 줄을 추가했고 CRLF→LF로 저장됐습니다. 기존4개 씬의 경로·GUID·enabled·순서와 나머지 YAML 내용은 보존했으며 원바이트 일치는 아닙니다.

## 실제 확인과 보존

1. installer의 default false C# 컴파일/비활성 결과를 확인한 뒤 Add1을 요청했습니다. marker만으로 성공 처리하지 않았습니다. 설치 직후 package0 관측과 reload 중 CLI6 조회 실패3건을 보존했고, 이후 등록 package1/version0.1.0/Git/런타임 타입 로드를 확인했습니다.
2. manifest와 lock의 full commit, 검토한 텍스트8개와 영상 Git blob을 대조했습니다. 최초 raw exact 검사는 줄바꿈 차이로 중단했습니다. 후속 분류에서는 텍스트7개의 CRLF→LF만 변환하면 원본과 같고, package.json은 UPM의 `_fingerprint` 필드1개 추가 외 구조가 같습니다. 원바이트 전체 일치로 표현하지 않습니다. 영상88,032B는 원본 Git blob `be65931578eb3803fa5fe4869c2d77d3c5346bb1`과 일치하며 패키지 meta GUID12개와 기존 공개 meta 사이 충돌은0입니다.
3. 제품 source2개를 적용한 직후의 첫 getter는 이전 assembly라 helper/cache/suffix가 모두false였습니다. 해당 미통과를 보존하고 고정 source2개를 ImportAsset한 뒤 새 MVID `6ec5928b-131a-4b59-ad44-461b7a97ff55`에서 helper signature·cache type·suffix를 확인했습니다. compile/update/failed=false, getter C# 진단0이며 설정 복사 helper를 호출하거나 네트워크에 연결하지 않았습니다.
4. scene creator03의 default false 컴파일 후 실제 `SaveScene`1/BuildSettings setter1을 실행했고 inner success=true를 확인했습니다. 새 GUID는 `c6f332b72d03f174ba236701215f96dd`입니다. 후속 getter로 저장된 clean Intro·root1·구성요소와 오디오 연결·build5를 재확인했습니다. Edit Mode의 `CanStreamedLevelBeLoaded("Login")`는 false였으므로 실제 target 유효성·전환 통과로 처리하지 않았습니다.
5. normal close CLI는 exit6을 반환했습니다. 추가 close·force·종료 중 UIAutomation 없이 기존 보유 native handle에서 wait signaled/hasExited/정수 ExitCode0을 확인했습니다. native 종료 기록12:31:48.822Z와 CLI 실패 기록12:31:48.911Z를 모두 보존했고, 12:32:31Z guard의 native0/unknown0/TempLock 없음으로 공유 자원을 반환했습니다.
6. 자동 직렬화된 폰트와 ProjectSettings 생성본을 보존한 뒤 종료 상태에서 이번 사전 사본과 정확히 일치하도록 복구했습니다. 문서 전 공개 입력4,014개는 사전4,012개 대비 의도한 기존5개(source2/packages2/BuildSettings1) 변경+새 씬/meta2개 추가이며 나머지4,007개는 원바이트 일치, 삭제0입니다. 이후 새 씬/meta의 자동 생성된 빈 값 끝공백5개는 원본 보존 후 제거해 staged diff check를 통과했습니다. 기존 dirty5는 보존했습니다. 공개 감사는 private6을 제외하며 해당 내용 조회·해시를 요청하지 않았고 일반 Editor import/직렬화 콜백은 억제하지 않았습니다.

## 후속 수용과 근거

### 출력 보완 전 Editor Play 관측 (13:06~13:19 UTC)

- P2에서 새 native Editor PID64220으로 Play 5회를 실행했습니다. 5회 모두 Intro 영상 frame/time 진행·prepare/end 이벤트·AudioListener mixed 출력 RMS와 targetAudioPlayingUnmuted, Intro→Login→Lobby 각 1회 전환을 관측했습니다. 전용 오디오 귀속은 증명하지 않았으며 영상 픽셀 표시나 실청 증거가 아닙니다. 실제 Play getter에서 Login loadable=true였고 Root는 로비 Photon 연결 화면도 관측했습니다. 닉네임 제출·방 생성·경기 입력은 하지 않았습니다.
- 관측기02의 `normalObservedCriteriaMet`는 false입니다. 첫 Intro sceneLoaded의 mode 원값은 `4`로 공개 enum 의미가 확인되지 않았고, 기존 Login 흐름은 Lobby를 Additive로 엽니다. 모든 mode를 Single로 요구한 기존 결과는 그대로 보존하며 통과로 재작성하지 않습니다.
- 3회차는 PAUSE_WATCHDOG라는 caseLabel을 사용했으나 첫 개입 호출은 준비된 영상 조건 불충족으로 called=false/videoCalls0이었습니다. 두 번째 호출은 innerSuccess=false/결과 미수용이며 영수증 시각은 config 만료 후이지만 상세 원인은 공개 결과로 확정할 수 없습니다. 개입 intent/result 파일도 없고 실제 Pause·모의 decoder 오류 실행은 확인되지 않아 둘 다0으로 구분합니다. watchdog·오류 복구 통과로 합산하지 않습니다. 이후 4·5회차는 정상 흐름을 다시 관측했습니다.
- Root가 확인한 실제 Simulator와 별도로 연 GameView 화면 모두 Intro 동안 검게 표시됐고 로비 UI는 표시됐습니다. 관측기 JSON이 영상 픽셀을 측정한 결과는 아닙니다. 디코딩 frame/time와 오디오 신호가 있어도 출력 경로 수용은 실패입니다. URP/camera-plane의 원인 확정은 하지 않았으며 패키지 원본을 유지하는 프로젝트 측 UI 출력 보완을 후속 검토합니다.
- Play 중단과 observer 비활성을 확인한 뒤 normal close 1회를 요청했습니다. 별도 GameView Close의 inner success=false는 보존했습니다. 보유 native handle에서 13:19:08.911Z 정수 ExitCode0, close CLI0, 후속 native0/unknown0/TempLock 없음으로 종료를 확인했습니다. 종료 중 UIAutomation·추가 close·force kill은0입니다.
- 임시 observer 소스/meta/폴더 meta 총3파일과 신규 폴더를 정확한 해시로 외부 보존한 뒤 제거했습니다. Play 중 자동 직렬화된 ProjectSettings는 변경 키가 AndroidKeyaliasName 1개인 것을 값 출력 없이 분류하고 생성본 보존 후 실행 전 dirty 원본으로 복구했습니다. 최종 공개 입력4,015개는 실행 전과 모두 원바이트 일치·추가/삭제0, 기존 dirty5는 유지합니다. 최초 guard 뒤 stale LASTEXITCODE를 검사해 후속 명령이 중단된 orchestration 실패도 보존했고 JSON guard 값을 확인한 뒤 계속했습니다.

### 프로젝트 출력 보완과 새 관측 (13:32~13:42 UTC)

`SuddenForceIntroCanvasOutput.cs`는 프로젝트 소유 코드입니다. 비활성 컴포넌트의 Awake에서도 UI가 만들어질 수 있는01 후보를 보존하고, `!isActiveAndEnabled` 조건1개를 더한02를 독립 검토했습니다. 패키지 원본은 변경하지 않았습니다. borrowed texture를 해제하지 않으며 raycast2false·입력/raycaster/EventSystem0, 완료·disable 시 texture 비우기/출력 숨기기, destroy 시 자기 Canvas만 정리합니다. 일반적인 disable→enable 재초기화와 CameraNearPlane 복구는 지원 범위가 아닙니다.

새 PID9404/MVID `1af46a12-4eb2-401c-8897-4f0453136573`에서 idle/failed=false를 확인했습니다. attach helper의 default false 컴파일 뒤 실제 AddComponent1/SaveScene1을 실행했고 payload success·componentAdded·saveReturned·sceneClean=true/adapterCount1을 확인했습니다. BuildSettings·전역 저장은0입니다.

| 새 회차 | 실제 개입과 관측 | 수용 한계 |
| --- | --- | --- |
| 06 정상 | prepare1/end1/error0·frame/time 진행·mixed RMS>0, Simulator 로고→Login/로비 | 전용 오디오 귀속·실청·별도 GameView 출력 미검증 |
| 07 Pause | 같은 Video에 Pause 반환1, 0.0667초 정지 샘플123개/end0/error0, 약6.707초 뒤 Login/로비 | 원본 play watchdog와 일치하는 복귀이며 인과 직접 계측은 아님 |
| 08 로컬 오류 | 고정된 부재 로컬 파일로 Source/URL/Prepare 각각 반환, decoder 이벤트1 후 약0.393초에 Login | 인위적인 로컬 소스 오류이며 자연 오류·모든 decoder 장애 통과는 아님 |
| 09 반복 정상 | prepare1/end1/error0·frame/time·mixed RMS>0, Simulator 로고 재표시→Login/로비 | Simulator 정적 화면과 JSON 신호 범위 |

개입 계획·raw config·nonce·PID/ticks·observer/개입 소스와 시각 결속을 독립 대조했습니다. 관측기02의 normal/order/fault/watchdog/Android false 플래그는 그대로 유지합니다. 새로운 개입 영수증과 복귀를 별도로 설명하며 과거01~05를 재분류하지 않습니다. 08/09 로비에서 loaded scene1/소유 Canvas0/adapter0/Intro Video0을 getter로 확인했습니다. 준비8초 timeout 자체·실청·전체 Console 오류0은 통과가 아닙니다. 별도 GameView는 Intro 로고가 없고 로비 일부가 잘려 보였으며 원인을 확정하지 않았습니다.

첫 auto bridge 컴파일은 inner=false/진단3으로 미수용됐고 당시 meta 부재를 관측했습니다. 해당 실패를 보존하고 정확한 임시 소스 ImportAsset1 후 default02를 inner=true/진단0·쓰기/영상호출0으로 확인했습니다. 최초 준비한 native binding01은 잘못된 schema로 실행하지 않았고, 별도02의 실제 PID/start를 보유 핸들에 연결했습니다. normal close1/CLI0과 native 정수 ExitCode0(13:42:43.704Z), fresh native0/unknown0/TempLock 없음으로 종료·반환했습니다. 종료 중 UIAutomation·추가 close·force는0입니다.

임시 observer·fault 소스/meta와 폴더 meta 총5파일+신규 폴더를 보존 후 제거하고 active config도 원본 보존 후 제거했습니다. 자동 PS alias 변화는 생성본을 보존하고 실행 전 dirty 원본으로 복구했습니다. 저장된 Scene의 빈값 공백3개(기존2+새 컴포넌트1)를 원본 보존 후 정리했으며, 새 컴포넌트 참조/블록을 역치환하면 원9471B exact입니다. 문서 전 공개 입력4,017개는 사전4,015개 대비 의도한 Scene1 변경·제품 CS/meta2 추가 외4,014개 raw exact/삭제0이며 기존 dirty5를 보존했습니다. 이후 이 문서 변경은 별도입니다.

새 버전 두 peer의 씬/연결 회귀와 단일 Windows Player 출력·GameView 진단, Android 포함 검증이 남았습니다. 기존 제공34/source612와 분리하며 새35 예약·Windows/Android 빌드·서명·업로드는0입니다. PR261 Draft/#259와 #42 전체 권리 범위는 유지합니다.

외부 근거: `D:/codex/validation/sudden-force-fps/20261030-work/`

- `intro-install-actual01/`: 사전 raw 사본, Add/등록/compile/scene 실제 결과·실패 보존·패키지 바이트 분류·자동 생성본 복구
- `p2-before-intro-open01.public.json`, `p2-intro-after-recovery01.public.json`: 사전/문서 전 최종 공개 입력 감사
- `native-exit-p2-candidate01/p2-intro-install01.public.json`, `intro-install-return01.public.json`: 보유 핸들 결과와 Root 도구/반환 관측 기록
- `intro-install-runtime-source-review01/`, `intro-scene-create02-independent-review01/`, `intro-scene-create03-independent-review01/`, `intro-product01-independent-review01/`: source 검토와02 부분 실패 관측 차단 이력
- `intro-play-actual01/`, `intro-runtime-observer-attempt01/`~`attempt05/`: 각 실제 호출·거절·결과와 원본 화면
- `native-exit-p2-candidate01/p2-intro-play01.public.json`, `p2-after-intro-play-closed01.public.json`: 이번 Play의 보유 핸들 종료와 후속 guard
- `intro-play-cleanup01.public.json`, `p2-after-play-cleanup01.public.json`: 임시 파일 보존·제거와 공개4,015개 원바이트 복구 감사
- `intro-output-adapter02-independent-review01/`, `intro-output-attach01-independent-review01/`, `intro-output-runtime02-independent-review01/`, `intro-auto-fault01-independent-review01/`: 새 출력/부착/실행기/개입 source 검토
- `intro-output-actual01/`, `intro-runtime-observer-attempt06/`~`attempt09/`, `intro-output-results-independent-review01/`: 새 실제 결과·영수증·공개 화면과 독립 대조
- `intro-output-scene-delta-independent-review01/`, `intro-output-cleanup01.public.json`, `p2-after-output-products-staged01.public.json`: 의도한 Scene 구조 변화·끝공백 분류·임시 파일 정리와 공개 입력 보존
- `native-exit-p2-candidate01/p2-intro-output01.public.json`, `p2-after-output-closed01.public.json`: 이번 실행의 실제 native 종료와 후속 guard
