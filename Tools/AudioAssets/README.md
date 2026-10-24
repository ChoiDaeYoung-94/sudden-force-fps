# 이동·씬 전환 음향 검증

## CC0 콘크리트 발소리5종 도입 (2026-10-25, #166/#42)

SoldierBlue/Red의 PlayerLocomotionPresentation._stepClips만 기존 자체4종에서 Kenney 콘크리트5종으로 연결했습니다. 자체4WAV와 원meta·원script·SFX mixer/그룹·보행거리/volume/거리 감쇠는 보존합니다. 모든 바닥을 같은 콘크리트 음원으로 표현하며 지형별 소재나 차폐를 추가한 것은 아닙니다.

원파일은 mono44,100Hz/PCM16·약0.106–0.114초입니다. 새 Unity importer는 기본값 그대로 **Vorbis/DecompressOnLoad/PreserveSampleRate/preload=false**를 관측했으며 기존 importer나 DSP·믹서 값을 바꾸지 않았습니다. 원파일 PCM과 Unity codec을 구분합니다. 첫 재생 로딩·청감/입력 지연·실기기 검증은 아직 통과가 아닙니다. [출처·변환·재생성](KenneyFootsteps/README.md)과 고정 manifest를 따릅니다.

같은 preview의 전체 GameObject/Component JSON에서 MonoBehaviour._stepClips 하나만 제외한 비교 후 두 prefab을 저장했고, 별도 read-only inspector에서 양쪽5참조·순서·GUID/AudioClipID·SFX를 확인했습니다. 원meta와 문서 class/fileID/order·배열 밖 전체 YAML은 LF만 정규화해 일치했으며, P8 공개3,947파일 전후에서 변경은 두 prefab뿐이었습니다. P2에는 원래 LF 형식으로 이 저장본과 Unity 생성 새13asset/meta만 승격하고 고지의 기존724,028B prefix를 유지해930B를 덧붙였습니다.

초기 import03의 전체 Resource dirty flag 비교는 실패했고 원인을 소급 식별하지 못했습니다. 그 false/부분 실행과 실패한 도구 초안은 보존했습니다. 후속05는 총괄의 명시된 제한 판단에 따라 기존3934파일 바이트·새13개를 연결하고, 공개asset/현재normalScene 객체의 identity·dirty flags 및 원씬/preview1·render구독 보존을 새로 검증했습니다. 이를 원래 전체 메모리 보존 성공·복원으로 바꾸지 않습니다.

근거는 외부 `20261024-resume/kenney-footstep-adoption-candidate05/`의 Verify/Apply/Inspect actual, `kenney-prefab-block-audit01/actual-37848b01d5f344e9a42579b537944cd7/`, `kenney-p8-after-apply05.public.json` 및 독립 검토입니다. 제공 코드33 APK에는 이 후속 소스가 미포함입니다.

## Additive 씬 전환의 리스너 중첩 제한 (2026-10-24, #166)

AD.SceneManager는 target을 additive로 읽기 전에 현재 source scene의 원래 active/enabled AudioListener만 잠시 끕니다. source unload 요청 이전에 끝나면 그 목록만 복원하며, 원래 disabled/inactive·다른 씬·이동된 리스너와 전역 pause/믹서는 바꾸지 않습니다. 부분 suspend 실패도 복원 목록을 보존하고, 복원이 기존 로딩/BGM/전환 flag의 finally 정리를 막지 않도록 했습니다.

같은 읽기 전용 프로브의 정상 Login→Lobby 전후에서 수정 전16표본 중 두 표본이 Login/Lobby Main Camera2개였고, 수정 후15표본의 최대는1개였습니다. Lobby가 로드된 사건에서도 수정 후 Lobby 리스너만1개였으며 두 실행 모두 이후 단일 로비 표본 .5초와 정상 로비 접속·Play 종료/Login clean을 확인했습니다. 수정 후에는 target 활성 전 리스너0인 두 표본도 있습니다(표본 시각 차267.621ms). 기존 코드상 BGM pause 이후 source 리스너를 먼저 끄는 방식이며, 이 수치를 전체 무리스너/청취 무음 길이로 해석하지 않습니다. 첫 로딩 구간의 큰 표본 간격이 있어 전체 frame·정확한 중첩 지속 시간·청취/총성 지연을 측정한 것은 아닙니다.

별도 격리된 실제 AudioListener20조건은 모두 일치했습니다. 중첩 자식·원래 disabled/inactive·타깃/기존 리스너 보존·반복 복원·다른 씬으로 이동·파괴/null·invalid scene과 preview/원 scene 정리를 포함합니다. 실제 native load의 지연 취소·target 정리 실패 전수 검증은 아니며, 취소 뒤 native load가 계속되는 기존 경계까지 해결했다고 처리하지 않습니다.

외부 근거는 `20261024-resume/audio-transition-observer-candidate02/actual-dbb333645a324b569f64df1fa4737bd6.public.json`, `actual-507879e323d14c9b9d1d4507c9330141.public.json`, `scene-audio-scope-fixture01.public.json`과 source/독립 검토입니다. 임시 Editor 관측 source/meta는 백업한 뒤 자기2파일만 제거했습니다. 새 소스 수정은 제공33 APK에 미포함이며 아래 자체 발소리·과거 PC 기록과 구분합니다.

## 자체 발소리 (2026-10-22)

`Assets/Audios/Authored/Footsteps/AuthoredStep01–04.wav`는 외부 샘플을 사용하지 않고 결정론적 잡음·사인파 합성으로 제작했습니다. 기존 출처 미확인 FootStep.mp3와 별개입니다.

4개는 각각 약0.135–0.153초, 모노/22,050Hz/16bit PCM입니다. Unity에서는 PCM/DecompressOnLoad/preload/forceToMono로 가져오며 기존 SFX mixer group을 공유합니다. 상대 발소리는 3D·선형 감쇠(1.5–20m), 자신의 발소리는 낮은 볼륨의2D입니다. 지형별 소재·벽 차폐·HRTF를 구현한 것은 아닙니다.

재생 간격은 관측한 수평 이동 거리와 Grounded에서 구하며 멈춤·공중·큰 보정·생명 변경에서 누적을 초기화합니다. 권위 이동/피해/네트워크 필드에는 쓰지 않습니다. 렌더 위치를 관찰하는 표시 기능으로 모든 서버 교정에서 완벽한 보행 판정을 보장하지 않습니다.

재생성은 Python 표준 라이브러리만 사용합니다. 새 외부 출력 폴더를 지정하고 원본 WAV 해시를 대조한 뒤 별도 검토합니다.

```powershell
python Tools/AudioAssets/generate_authored_footsteps.py --output D:/codex/validation/footstep-regeneration-new
```

기존 폴더나 제품 자산을 자동 덮어쓰지 않습니다. Android 실기기 청취·지연·성능은 후속 검증이며 기존 코드27/28에는 미포함입니다.

## PC 검증 (2026-10-22, #166)

새 Windows 개발 Player와 Editor의 실제 두 Fusion 피어에서 합성 입력으로 걷기·후진 달리기·정지·시선 회전·재장전을 실행했습니다. 두 번째 시험은 캡슐 경로를 조회한 복도에서24.17m 이동/발걸음12회/HP100 유지/탄약19→30/실제 재장전을 관측했습니다. 첫 시험은 벽에 막힌2.52m·발걸음1회로 남기며 충분한 달리기 검증으로 합산하지 않습니다. 두 번째 시험도 모든 경사·보정·원격 이동 조건을 보장하지 않습니다.

총성 연사11발/효과11회/탄약30→19와 중복0을 재확인했습니다. 실제5분 DRAW 결과에서 발소리 정지와 총기 offset 초기화, 로비 복귀·새 방 재참가도 관측했습니다. 정지 중 같은 프레임의 disable/직접 호출/re-enable에서 offset0·추가 step0을 확인했으며 이동 중 전체 생명주기 시험과 구분합니다.

별도 합성 원격 표시 경로에서 발소리 AudioSource를1.67m/30m/1.67m+SFX−80dB에 놓았습니다. AudioListener 믹스 출력 peak는 각각0.1132/0/0이었고 기존 mixer 값과 source 설정을 복원했습니다. 실제 상대 이동 입력이나 헤드폰 방향감 시험이 아니며, source 전체 JSON 바이트 동일성·mixer snapshot 소유 상태까지 검증한 것은 아닙니다.

Windows 빌드는 오류0·경고8·출력360파일 해시 일치를 확인했습니다. 공개 입력3,939개 중 자동 저장4개(폰트 표현 공백·URP prefilter4bool·기존 URP resource 참조·PlayerSettings 별칭 필드)는 생성본을 보존하고 Editor 종료 뒤 실제 전image로 복원해3,939개 rawexact를 확인했습니다. 나머지3,935개는 독립 검토에서도 일치했습니다. Editor 종료 요청 후 부재는 확인했으나 종료코드는 관측하지 못했으므로 ExitCode0으로 기록하지 않습니다. 기존 서명 키·비밀번호는 사용하지 않았습니다.

임시 적용 도우미의 첫 raw NetworkObject 검사 중단은 CRLF/LF 차이였습니다. 기존233개 record와 네트워크 목록이 줄바꿈 정규화 후 일치함을 확인한 뒤 별도 후속으로 Red만 적용했습니다. 처음 PowerShell 통합 오류와 해시 대소문자 비교 오류도 실패 자료로 보존하고 정정본과 구분했습니다. 최종 도우미 컴파일은 오류0·경고0입니다.

외부 근거: `D:/codex/validation/sudden-force-fps/20261022-quality/`의 movement-audit, locomotion-runtime-01/02, locomotion-audio-01/02, locomotion-windows-01입니다. 오디오01은 경기 종료 후 사전 조건에서 거절되어 믹서·source 변경0이며 새 경기의02와 구분합니다. 실제 기기·client 역할 이동·자연 원격 발소리·손실 네트워크·움직이는 생명주기는 후속입니다.
