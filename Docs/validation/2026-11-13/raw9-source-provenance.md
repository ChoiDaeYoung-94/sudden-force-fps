# Raw9 lower-body 소스 provenance

Raw9 9클립의 원 take·파생 설정·공급자 선언과 보존된 sample/serialized asset 연결을 기록합니다. 대조 기준은 QA6 통합 소스 `9bee2e6169fba87b94b6cf1e5f70b6af0694798e`의 `Assets/Resources/WorldBodyGaitContact03/Raw9Contract.json`입니다. 이 문서는 소스·기록 대응을 보완하며 main 제품 활성화, 현재 산출물 전체 고지, 권리·배포 수용은 미완료로 유지합니다. Refs #42, #341.

## 공급 원본과 선언

공급자는 Quaternius이며 보존된 다운로드 inventory는 [Ultimate Modular Men 공식 페이지](https://quaternius.com/packs/ultimatemodularcharacters.html)와 당시 공식 연결 다운로드 위치, 개별 공급 파일의 bytes/SHA를 기록합니다. 동봉 선언에는 `Ultimate Modular Males by @Quaternius`와 CC0 1.0 Universal/Public Domain Dedication 표기가 있습니다. 아래는 그 공개 원본의 고정 핀입니다.

| 보존 파일 | bytes | SHA256 |
| --- | ---: | --- |
| `modular-men-Animations.fbx` | 7,695,388 | `259ED6F6E5F1BB2A15215A66A9ED02E8956083A281A8098103490D1A00292B7F` |
| `modular-men-Humanoid-Swat.fbx` | 481,820 | `3C5EDA6FE78350350363CB1E2FF8BE0C7D8A02D58FC60584992B471C04357FA0` |
| `modular-men-License.txt` | 372 | `E8DBF915A2B82229913E301A0787696611241BDEFEC4832BC084F54161DB1EFE` |

원본과 `modular-men-download-inventory.json`, take 목록 sidecar `modular-men-Animations.fbx.metadata.json`은 이 PC의 `D:/codex/validation/sudden-force-fps/20261017-animation-replacement-candidates-01/`에 보존돼 있습니다. inventory는 개별 FBX·선언 다운로드를 기록하고 있으며, 별도 원 ZIP 파일·판본명은 이번 추적에서 확인하지 않았습니다. 원 FBX 다운로드 기록과 고정 핀은 그대로 보존합니다. `Humanoid-Swat.fbx`는 별도의 FK reference로 사용한 파일입니다.

기존 `Assets/Animations/Player/CC0Rifle/License.txt`는369바이트이며, 공급자372바이트 선언과 행끝 공백·마지막 개행을 정규화한 문구가 일치했습니다. 두 파일의 SHA는 서로 다릅니다. 기존 선언의 정리·고지와 별도 Idle/Death 두 클립의 리타깃은 [Tools/AnimationBaking/README.md](../../../Tools/AnimationBaking/README.md)에 기록돼 있습니다. 공급자 선언의 존재를 이번 문서에서 새로운 권리 해석으로 확장하지 않습니다.

## Raw9의 take와 파생 설정

`20261106-work/resume-0330/locomotion-author01/BakeCC0LegClips.cs`는 위 두 FBX의 핀을 확인하고, Generic source의 world pose를 FK reference와 `HumanPoseHandler`를 통해 당시 Military Avatar의 다리 rotation으로 옮기는 코드를 보존합니다. source take의 실제 이름은 아래 접미사 앞에 `CharacterArmature|`를 붙인 값입니다.

| Raw9 clip | 기록된 sourceTake | 파생 duration | legMuscleGain |
| --- | --- | ---: | ---: |
| LegIdle | 빈 문자열 | 1.0s | 0 |
| WalkForward | Walk | 1.33333337s | 1 |
| WalkBackward | Run_Back | 1.33333337s | .72 |
| WalkLeft | Run_Left | 1.33333337s | .72 |
| WalkRight | Run_Right | 1.33333337s | .72 |
| RunForward | Run | .8s | 1 |
| RunBackward | Run_Back | .8333334s | 1 |
| RunLeft | Run_Left | .8s | 1 |
| RunRight | Run_Right | .8s | 1 |

WalkBackward/Left/Right는 해당 Run take의 속도를 늦추고 leg muscle gain .72를 적용한 파생입니다. 방향별 원 Walk take가 있다는 주장은 하지 않습니다. 위 take 목록과 길이는 보존된 import metadata 및 sample 기록이며, 이번에 Unity로 새로 import하거나 평가한 결과가 아닙니다.

LegIdle의 sourceTake는 빈 문자열이고 sourceDuration도0입니다. baker는 당시 SoldierBlue Animator에서 `Base Layer.Idle_Aiming`을 평가한 기준 자세를 사용하여 정적 lower-body rotation을 기록합니다. author 보고서는 이를 기존 Quaternius 기반 Idle_Aiming의 다리로 설명합니다. 당시 prefab/controller·기존 CC0Rifle 소스 핀과 연결된 기록이며, 별도 원 Idle take를 직접 sample한 것으로 표시하지 않습니다. 이번 추적에서 당시 prefab→controller→clip의 전체 Unity dependency나 pose를 새로 재구성하지 않았습니다.

## sourceInput SHA9와 현재 contract 연결

보존 sample의 연결은 아래와 같습니다. 모든 경로는 `D:/codex/validation/sudden-force-fps/`를 기준으로 합니다.

1. 최초 author sample: `20261106-work/resume-0330/locomotion-author01/leg-bake-01/{name}.samples.json`.
2. seam-curated sample: `20261107-work/locomotion-contact-candidate11/leg-bake-11/raw-seam-curated-{name}.samples.json`. 원 sample의 metadata를 유지하고 `sourceInputSha256`에 최초 author sample의 SHA를 기록합니다. seam curation 이후 contact IK를 적용하기 전에 raw를 저장한 경로입니다.
3. 현재 contract의 `sourceSampleSha256`는 위 seam-curated sample을 가리킵니다. `serializedAssetSha256`/`serializedMetaSha256`는 별도의 serialized clip/meta 핀입니다.

아래 sourceInputSha256 9개는 최초 author sample의 직접 재해시와 모두 일치했고, sourceTake도 두 sample 사이에 모두 일치했습니다.

| Raw9 clip | sourceTake | seam-curated sample의 sourceInputSha256 |
| --- | --- | --- |
| LegIdle | 빈 문자열 | `E2193A8B3FF90FC604405B803D2A2769470B2B478F97D11A2F12BDD9B71CA5E6` |
| WalkForward | Walk | `F53158853E87F1EC2A571C488D9EC36E14008150C6999B342D70ACF23DDCEB3F` |
| WalkBackward | Run_Back | `25BBD3460BD8F2ED129958B089B9DF736D439EA362E73C53829A3B68D592153B` |
| WalkLeft | Run_Left | `40F4580E5CC080013D0C46D29FA62F6A20A5A3EB566E8331ACFFFFBFF594878D` |
| WalkRight | Run_Right | `812B5768592C1A23CD69FDB814F50798AC6B3DB477BD543878A1A443F540B738` |
| RunForward | Run | `0B2386285E3F9DDCA84E34B9FB85BAA3C68E0C7F26379285AA47AD859358BF47` |
| RunBackward | Run_Back | `EF98AA3BFA295A7C23A483057902FBCB64443DBF45A0F6C165857145349C0840` |
| RunLeft | Run_Left | `502F99A37C101AA246D46949358C3BBE1A4EC9344D3DD0E69C0AEA90196DCFDF` |
| RunRight | Run_Right | `19E56363ADFAEDAD4EE3234E37A6554C34A717B86AB9BE56824141ED50EBF0FF` |

현재 contract의 sourceSampleSha256 9개도 보존 seam-curated sample의 직접 재해시와 모두 일치했습니다.

| Raw9 clip | 현재 contract의 sourceSampleSha256 |
| --- | --- |
| LegIdle | `EB4D46808FFA754A6DE88F8E10FE884A3ADD0D852AC6F9BE95642942C7F13EA9` |
| WalkForward | `B7E1E365E174B23B0CDCC3494F3088CF8D22BB6135FBF88A0A2ACBB4AF4F4A71` |
| WalkBackward | `9F2DB690AC967AB92B876C0FA6712C4D4E5EBA61605ABB989E0C3643E9A7CF16` |
| WalkLeft | `75B05B01C674BBB2D7BE16DBD1FADA579DE4C6215013AF1F94C760FBFB9CA91B` |
| WalkRight | `FF33EBA51D7AFA617CD292C8C52AD5587B1579EADA2BD6FD169A32370707DA7C` |
| RunForward | `D65DDDDA852D6A76EF8EEE2C487F643A35D443F5657303E066F0ED21B9D25D07` |
| RunBackward | `13EEB2858FAEED8C3AD05114BE69792EA8066E52F592567B338626C243C213A9` |
| RunLeft | `7EF669D8BB336813E8CFBD12D5CFCA24DA23710C55FA1CBD754485D821883CF0` |
| RunRight | `5223BD91E385255521ADC444060AA1E37741FE220B9734734C29AF6B5320BC6D` |

이 비교는 JSON 파일의 바이트·명시 take·입력 연결 확인입니다. 원 FBX→retarget→곡선 생성 수학 전체의 새로운 재실행이나 현재 Game의 실제 동작 수용을 의미하지 않습니다.

## serialized clip과 기존 실행 근거

Raw9 contract 자체는8,898바이트/SHA `FC4DA351905E1C74452591E6194C26D7C6FEA6387F877D1CA1EE859E68ADF7F7`입니다. 그 `materializationReviewSha256`는 아래 기존 materialization 독립 기록의 핀과 일치합니다. Raw9 9anim/9meta, 폴더meta2와 contract쌍을 포함한 Resource22 집합은 기존 Raw05 연결 문서의 범위로 구분합니다.

| 공개 검증자료 상대경로 | bytes | SHA256 | 기록 범위 |
| --- | ---: | --- | --- |
| `20261106-work/resume-0330/locomotion-author01/input-pins.public.json` | 3266 | `51EB82CD7834334D9BA8CDCBA27F67A9B1DDE7EAEB3E09055AB45730C7BBCEC5` | 공급 원본3·당시 prefab/controller·기존 문서 입력 핀 |
| `20261107-work/gait-contact03-materializer-candidate02/input-manifest.public.json` | 14188 | `0710C6E39F3C74C7CDA3B3E566E1F0EE34772D2A4FD51FEAA88009A2F51ED114` | author/curated sample·공급 원본·변환 소스 연결 |
| `20261107-work/gait-materialized-assets-actual-independent-review02/verification.public.json` | 11535 | `DED75CE3B35FF299536E632A4C2A6C10D96B035D22597A1E26BE7A01E42DE590` | 기존 materialization actual의 독립 대조 |
| `20261108-work/gait-product-raw-qa-candidate04/raw-asset-scope.public.json` | 14379 | `D9CAEC8859942E52BB95DFCF9FCA244EBF57FAD6B7457BDBA4E0E733B4AE5732` | clip별 serialized anim/meta/sample 대응 |
| `20261112-night/rights42-raw9-source-trace01/trace.public.json` | 16811 | `188258EE268AD2E6A64F21ED0A074283328EF83A77C543C7DD53C30D35B2946D` | 이번 로컬 공개 sample SHA·take 연결 추적 |

`Docs/validation/2026-11-09/raw05-source-and-editor-preview.md`는 Draft PR353의 소스 연결과 기존 격리 Editor 하체 preview·종료·원복의 범위를 기록합니다. source provenance, serialized asset 대조, 실제 Editor preview는 각각 별도 근거입니다. 초안을 작성한 보조의 추적에서는 새 Editor/SDK/기기 실행이나 serialized anim·FBX 본문 재해시를 하지 않았습니다. 총괄의 별도 원본 재해시는 아래 절과 구분합니다. 현재 실제 Game·폰·순수 proxy·접지·전체 외형 수용은 해당 새 실행의 근거로 판단해야 합니다.

## 남은 범위

현재 contract는 `qaOnly:true`, `contactCorrectionEnabled:false`, `runtimeOrProductAccepted:false`를 유지합니다. 이번 문서만으로 main 제품 활성화나 계약 변경을 승인하지 않습니다. 최신 QA6 및 최종 production에서 실제 포함된 clip·SDK·전체 고지 전문과 앱 표시를 연결하는 검증도 남습니다.

기존 CC0Rifle의 Idle_Aiming/Death_Front 두 클립, 이번 Raw9, 출처 미확인 legacy Player22는 별도 집합입니다. Raw9는 별도 author/baker/sample 체인에서 위 공급 FBX로 연결된 기록을 확인했습니다. 이 연결을 legacy22의 원공급자·조건 해결이나 저장소 보존·재배포 권리 수용으로 적용하지 않습니다. Raw9 baker는 원22곡선을 읽거나 복사하지 않는 범위를 명시합니다.

완료 범위는 보존된 공개 source/take/선언과 SHA 연결의 문서화입니다. 새 권리 해석·배포 수용·차단 결정, 제품 활성화, 현재 packed 전체 고지 수용은 미완료입니다. 원 공급자 다운로드 근거와 기존 실패·preview·materialization 기록은 그대로 보존합니다. 현재 필요한 사용자 조치나 질문은 없습니다.

## 총괄의 공급 원본 재확인

2026-11-13 KST 총괄은 위 공급 FBX 두 개와 372바이트 선언의 현재 바이트를 다시 해시하여 보존 download inventory 및 최초 author 입력 핀과 모두 일치함을 확인했습니다. 별도 공개 기록은 `20261112-night/rights42-raw9-source-trace01/root-source-public-rehash01.public.json`이며 SHA256은 `AD13F2FF7196FFC1F574971274BBE78F3F87CC378DC64CFF34311A1AC1CF646D`입니다. 이 확인은 새 import·bake·게임 실행이나 배포 전체 권리 수용이 아닙니다.
