# Raw05 QA 연결과 Editor 하체 렌더 검증

Raw05 QA 소스를 Draft PR353에 연결하고, 같은 소스의 정상 Editor 격리 preview에서 하체 자세 변화와 공개 산출물·종료·원복 기록을 확인했습니다. 실제 Game·소총 자세·접지·GPU skinning·FPS·연속 자연성·제품 수용은 후속입니다. 이 문서의 완료 범위는 2026-11-09 05:23 KST에 반환한 Editor 검증이며 QA2 Android 산출물·서명·설치는 별도 기록으로 판단합니다. Refs #341.

## 소스와 앱의 구분

[PR353](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/353)은 Draft이며 base는 PR348의 `98d191c806bec27c9ee532b80bd804e1115bd26a`, head는 `d7a3371296357a5ceb1b7b657d468415130e7047`입니다. 변경은 제품 payload32와 `.gitattributes`1의 정확33파일, 기존5수정·28신규입니다. 최종 공개 파일 수는4095이며 다른 기존4,062개 파일의 바이트 보존은 Root의 최종 감사 기록입니다. 초기32적용 직후의 다른4,063개 파일 보존과 혼동하지 않습니다.

승인 Raw05 payload32와 실제 작업 파일이 일치했고, Resource22의 보존된 selected checkout도 SHA·bytes가 모두 같았습니다. attributes는 기존 규칙을 보존하고 해당 Resource의 `-text`와 generated meta whitespace 정책만3개 pattern 행으로 추가했습니다. CS 공백 규칙은 바꾸지 않았습니다. 기존 CS4와 attributes의 Git blob은 작업 파일과 CRLF→LF만 다르고 Resource22의 바이트 계약은 유지됩니다. 전체 새 checkout을 검토자가 실행한 것은 아닙니다.

QA define이 꺼지면 gait graph는 Resources 전에 닫힙니다. QA 분기는 Raw9·기존 visual Animator의 수동 graph만 사용하고 contact03은 false·승인SHA3 빈값을 유지합니다. 다만 새 Networked4개·43/75 예상 레이아웃과 `-intro1-riflepose2-gaitraw04` protocol은 define 밖의 소스 변경이므로, 비QA에서 기존39/70 프로토콜로 되돌아가는 것은 아닙니다. QA의 `-localqa1` 격리는 유지되며 기존 QA1 peer를 새 branch와 호환된다고 수용하지 않습니다.

기존 설치 검증 기록의 앱은 `1.0.197-qa1`/code1/SHA `17FC62AD…`이며 gait가 포함되지 않은 source98 앱입니다. 이 Editor preview를 그 APK에 포함됐다고 보고하지 않습니다. production code34와 기존 원장·서명 키는 이번 QA 소스/preview 작업의 변경 대상이 아닙니다. 후속 QA2는 같은 localqa package의 code2/name `1.0.197-qa2` 계획이며 이 문서는 그 산출물·배포 증거가 아닙니다.

## 실제 격리 preview

실행 nonce는 `fef3e13ecc3a4aadb014a0891362fa79`입니다. 아래 증거에서 `A`는 `raw05-minimal-preview-bridge02/actual-fef3e13ecc3a4aadb014a0891362fa79/`이며 모든 상대경로의 기준은 `D:/codex/validation/sudden-force-fps/20261108-work/`입니다.

정상 Graphics Editor PID50316의 공개 metadata eval은7types·43 Networked properties·75word·Game MVID `163db092-aec1-41c3-aa29-dd8e609f98df`와 Raw05 계약 literal을 관측했습니다. snapshot은 메타데이터 관측이며 전체 source-to-IL 등가나 private readonly mode 값의 독립 판정이 아닙니다. 실제 정상 factory 반환과 이어진 graph 평가 결과를 별도로 확인했습니다. Play·Runner·네트워크는 사용하지 않았습니다.

| 항목 | 실행 기록과 독립 확인 범위 |
| --- | --- |
| 표본·시간 | Blue315/Red315, 총630행·Editor span23.630687초입니다. 한 update당 최대1표본이며 논리30Hz입니다. editor delta 최대 약2.137초도 기록돼 있어 Game FPS나 실제 일정 간격 캡처로 해석하지 않습니다. |
| 이미지 | 영상용640×360 PNG630개와 portrait512×768 PNG44개입니다. 실제674개 파일의 bytes/SHA·크기·PNG CRC·filtered scanline을 독립 대조했습니다. 고유 PNG SHA는 각각551/38종이며 매 frame이 모두 고유하다고 주장하지 않습니다. |
| graph | 양팀 start/end의10 playable·출력1·root1·mixer1(input9)·clip9가 일치합니다. 로드9의 경로/GUID·Generic/loop·48 rotation bindings는 실행 producer의 API 관측입니다. Dispose 뒤 graphValid=false·팀별 cleanup0·callback detached가 기록됐습니다. |
| 수치 보호 | initial source61 두 집합과630×leg12의 7,560개 관절표본에서 localPosition/localScale을 직접 대조했습니다. 다리 회전 표본은 팀별275종입니다. Source61·상체49·leg parent·skin8 원참조 보호는 실행 guard의 보고 범위이며, 전 표본의 전체 상체/parent/renderer 상태를 독립 재구성한 것은 아닙니다. |
| 시각 범위 | 대표 portrait4장을 직접 보아 팀 색상과 다리 자세 차이를 확인했습니다. 상체는 authored pose이고 rifle 적용 전이므로 소총 hold나 연속 자연성 통과가 아닙니다. |

원본 PNG는 변경하지 않았습니다. Root가 별도로 만든 legs-crop GIF와 sheet는 `raw05-leg-diagnostic-derivatives01/provenance.public.json`에 source result/journal pin과 제작 범위를 기록했습니다. GIF는3표본마다 선택한210장·100ms 논리 재생이며 Game/capture FPS가 아닙니다. 상체·제품 화면을 대표하는 영상으로 사용하지 않으며, 원본 전체나 실제 Android/Game 화면 검증을 대신하지 않습니다. 이 초안에서는 provenance를 참조했고 파생 GIF 전체를 새로 재생·평가하지 않았습니다.

## 종료와 원복

첫 Exit eval은 C# character literal의 컴파일 오류였고 실행 성공으로 합산하지 않았습니다. 수정된 exit02의 `OWN_EXIT0_SCHEDULED_NOT_OBSERVED` 문자열도 종료 증명이 아닙니다. 별도 controller의 같은 held Native50316 기록에서 실제 normal Exit0/forcedfalse·EOF·Dispose0를 확인했고, prefs 두 DWORD 복원과 동일760 세대의 새 positive peer 검증 기록을 대조했습니다. listener 소유나 과거 전체 자식 프로세스 closure는 증명하지 않았습니다.

Editor 종료 후 자동 변경17개를 원문 보존했습니다. GeneratedLocalRepo13·공개 Gradle2·Resolver2 중15개만 EOL/끝공백 정규화로 같고, `settingsTemplate.gradle`1502→859와 `AndroidResolverDependencies.xml`1240→1239는 의미 차이가 있는2개입니다. 전체를 format-only로 재분류하지 않았습니다. Root가 보존한 before17 checkout의 SHA가 baseline과 같고 after17도 외부에 보존됐음을 대조한 뒤, Root가 정확한17개만 같은 handle CAS로 복원했습니다. 최종 audit는 public4095 rawexact·temp4 제거·git clean·private6 제외를 보고합니다. 검토자가 현재 프로젝트/OS/Registry/socket/Git를 다시 조회한 것은 아닙니다.

실제 전체 반환은 `2026-11-08T20:23:20.2433689Z`이며 원 시작20:14:34/new20:26:34/full20:29:34 안입니다. 현재 부재·서버/prefs·철회·원복은 Root의 공개 proof 범위로 수용하며, 원 Native·CLI 실패·복원 전 자동 변경은 그대로 보존합니다.

## 대표 공개 근거

전체 SHA와 입력 목록은 각 verification에 있습니다. 아래는 대표 pin의 앞16자리이며 source/actual/Root 관측의 역할을 구분합니다.

| 공개 상대경로 | 대표 SHA256 | 범위 |
| --- | --- | --- |
| `pr353-raw05-independent-review01/verification.public.json` | `63F759A4769D9ECE…` | 고정 HEAD/base·33파일 코드/범위 |
| `raw05-minimal-preview-bridge-independent-review02/verification.public.json` | `404B78F433DADF0A…` | bridge02 소스 delta·준비 검토 |
| `A/result.public.json` | `E5014114FE2A8A48…` | 실행 producer630/674·cleanup·MVID |
| `raw05-preview02-actual-independent-review01/verification.public.json` | `2E454E4C515878EB…` | 공개709파일·수치/PNG·종료/원복 기록 독립 대조 |
| `root-resume01/raw05-normal-preview-actual-return01.public.json` | `F1EA47F72C45C6EC…` | Root 전체 반환·원시각·기한 |
| `raw05-preview-return-audit02/verification.public.json` | `6604FD46EAAA6B09…` | Root public4095·own4·private6 제외 |
| `raw05-leg-diagnostic-derivatives01/provenance.public.json` | `818FDDE28E510FD3…` | 원본 미변경 legs-crop 파생물 provenance |

추가 실제 관측은 `A/plan.public.json`, `A/samples.public.jsonl`, `A/{Blue,Red}-graph-{start,end}.public.json`, `A/{Blue,Red}-initial-source61.public.json`, `root-resume01/raw05-normal-preview-cli01/{metadata-eval,begin,exit,exit02}.cli.public.json`, `raw05-normal-editor-controller02/actual-fef3e13ecc3a4aadb014a0891362fa79/`의 공개 receipts에 남겼습니다. private 로그·기기 식별자·자격정보 경로는 이 문서에 옮기지 않습니다.

## 현재 상태와 다음 작업

현재 수용은 Draft 소스 연결, 격리된 Editor 하체 렌더 산출물과 공개 종료·원복 기록의 정합성입니다. Game/rifle/Hitbox/contact/704/offset·physics/NCC·양피어·하드웨어 GPU skinning·FPS·연속 자연성·전체 source-to-IL·권리·제품 수용은 false입니다. #341 전체 완료나 PR353 merge 조건 충족으로 처리하지 않습니다.

다음은 독립 source 검토를 거친 QA2 batch export 구성·새 unsigned APK/static 검증입니다. export helper02·기존 키 signer의 source IR은 별도 source-only 자료이며 실제 준비 권한·산출물 핀은 아닙니다. controller와 Gradle의 QA2 입력·경로·시각·독립 IR도 새 실제 범위에서 결속해야 합니다. 기존 GPGS provider-only 처리와 같은 키 서명은 새 QA2 산출물/qualification과 Root fresh 권한 뒤 별도로 판단합니다. Editor 검증 반환 시점의 QA2 actual export/APK/sign/install은0이며, 이후 시도와 결과는 이 문서의 통과에 합산하지 않습니다.

현재 필요한 사용자 조치나 질문은 없습니다.
