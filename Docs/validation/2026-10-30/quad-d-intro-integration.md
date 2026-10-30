# Quad D 인트로 통합 준비 (#302)

2026-10-30 P2, 변경 전 main `02a482c9e97d293077553d76884295ac1b776fa5` 기준입니다.
현재 단계는 패키지 설치·새 씬 저장·제품 코드 컴파일까지이며 **실제 재생·Login 전환·네트워크 회귀 전 Draft**입니다.

## 제품 구성

- 회사 저장소 [com.quadd.intro](https://github.com/ChoiDaeYoung-94/ChoiDaeYoung-94/tree/55bf9e4aa48c2f95942f3c2c995f1d55414e97c0/unity/com.quadd.intro) 0.1.0을 고정 커밋으로 설치했습니다. Unity UPM `Client.Add` 1회를 사용했고 manifest를 직접 편집하지 않았습니다. 기존 다른 manifest/lock 의존성은 같습니다.
- 새 `Assets/Scenes/Intro.unity`에는 Camera·AudioListener·AudioSource·VideoPlayer·QuadDIntro가 각각 1개입니다. 영상은 3.4초/1920×1080/30fps/음성 트랙1, FitInside, track0 enabled→자기 2D AudioSource, volume0.65/mute=false입니다. NextSceneName은 Login, persistent 완료 listener는0이며 별도 skip 입력은 연결하지 않았습니다.
- Intro 중 생성할 게임 Manager/Runner나 게임 UI는 없습니다. 완료 후 기존 Login 흐름으로 넘어가도록 구성했습니다. 패키지의 준비8초 제한·재생길이+3초 watchdog·decoder 오류 처리는 원본 소스에 있는 경로이며 아직 FPS에서 실행 검증하지 않았습니다.
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

실제 Intro 영상 frame/time 진행·오디오 신호, 정상 종료→Login/로비, 반복 실행, 모의 decoder 오류·지연 복구, 두 peer의 새 씬 목록/접속 버전 회귀를 확인해야 합니다. 소스 준비·설정값으로 실제 재생·실청·전체 Console 오류0·Android 포함을 대신하지 않습니다. 기존 제공34/source612와 분리하며 새35 예약·빌드·서명·업로드는0입니다. PR261 Draft/#259와 #42 전체 권리 범위는 유지합니다.

외부 근거: `D:/codex/validation/sudden-force-fps/20261030-work/`

- `intro-install-actual01/`: 사전 raw 사본, Add/등록/compile/scene 실제 결과·실패 보존·패키지 바이트 분류·자동 생성본 복구
- `p2-before-intro-open01.public.json`, `p2-intro-after-recovery01.public.json`: 사전/문서 전 최종 공개 입력 감사
- `native-exit-p2-candidate01/p2-intro-install01.public.json`, `intro-install-return01.public.json`: 보유 핸들 결과와 Root 도구/반환 관측 기록
- `intro-install-runtime-source-review01/`, `intro-scene-create02-independent-review01/`, `intro-scene-create03-independent-review01/`, `intro-product01-independent-review01/`: source 검토와02 부분 실패 관측 차단 이력
