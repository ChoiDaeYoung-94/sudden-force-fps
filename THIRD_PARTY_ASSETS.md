# Third-party asset provenance

확인·취득일: **2026-10-07 (Asia/Seoul)**. 이 문서는 확인된 출처와 확인이 필요한 항목을 구분합니다. 저장소에 파일이 있다는 사실만으로 취득 권한이나 배포 라이선스를 확정하지 않습니다.

## Kenney Particle Pack — 신규 선별 텍스처

- 제작자: **Kenney Vleugels (Kenney.nl)**. 필터 템플릿 기여자 크레딧은 원본 `License.txt`에 보존했습니다.
- 공식 배포 페이지: https://kenney.nl/assets/particle-pack
- 공식 페이지의 “Continue without donating...” 링크에서 확인한 ZIP: https://kenney.nl/media/pages/assets/particle-pack/f8fe0f8cb8-1677578741/kenney_particle-pack.zip
- 라이선스: **Creative Commons Zero (CC0 1.0)**. 원본 ZIP의 `License.txt`를 변경 없이 `Assets/Art/VFX/KenneyParticlePack/License.txt`에 보존했습니다.
- 라이선스 원문: https://creativecommons.org/publicdomain/zero/1.0/legalcode
- 공식 이용 안내: https://kenney.nl/support — 상업 프로젝트 이용 허용, 출처 표기 의무 없음. Kenney 로고는 공식 프로젝트용으로 별도 취급되므로 가져오지 않았습니다.
- CC0는 상업 게임에 포함한 이용·수정·배포 및 원본 재배포를 허용합니다. 선택적 크레딧 예: `Particle textures by Kenney (kenney.nl), CC0`.
- 버전 증거: 웹 페이지는 1.0 / 2018 / 80 files로 표시되지만, 취득한 공식 ZIP의 라이선스는 **Particle Pack (1.1)**로 기재되어 있습니다. ZIP에는 투명/검정 배경 및 회전 변형 이미지가 더 들어 있습니다. 아래 목록은 실제 취득 ZIP 기준입니다.
- 원본 ZIP SHA256: `b631d4b07f7002549fdcf155f01141ad482f79f3440e4e301eed49ce5f1d8958` (15,001,764 bytes). ZIP은 프로젝트 밖의 임시 작업 폴더에서 검사했으며 저장소에 추가하지 않았습니다.

### 가져온 파일

모든 PNG는 ZIP 내부 `PNG (Transparent)/`에서 가져왔습니다. 파일 이름과 바이트를 변경하지 않았으며, 총 5개 PNG와 라이선스만 선별했습니다. 용도는 후속 VFX 제작을 위한 제안이며 현재 씬·프리팹에 연결되어 있지 않습니다.

| 프로젝트 경로 | 제안 용도 | bytes | SHA256 |
| --- | --- | ---: | --- |
| `Assets/Art/VFX/KenneyParticlePack/muzzle_01.png` | 길쭉한 총구 화염 | 81811 | `08f650f6a615b65024b48d8da32178158d74307d7c235f214f0c075b7aa688b9` |
| `Assets/Art/VFX/KenneyParticlePack/muzzle_03.png` | 짧은 총구 화염 변형 | 57240 | `579983452d263ebeca78080714b6dd55a0f95fca7452f309e91a9d466de6c058` |
| `Assets/Art/VFX/KenneyParticlePack/spark_02.png` | 작은 피격 불꽃 | 101613 | `f7c5f1f849657eed79fd7b663be5a57829450a1e4fdc4a062049a05b9a406e89` |
| `Assets/Art/VFX/KenneyParticlePack/dirt_01.png` | 벽·지면 피격 파편 | 47313 | `6827a0a32a293ec9570fb98d63963b8dd6e6aaba9e1096afad932e93a378bea9` |
| `Assets/Art/VFX/KenneyParticlePack/smoke_03.png` | 소량의 피격 분진 | 39737 | `a71f8abcac64f8d73a94625cc9a10033dbeafa7eaea750560cba0daa73fe8752` |
| `Assets/Art/VFX/KenneyParticlePack/License.txt` | ZIP 원본 라이선스·크레딧 | 651 | `f9e70b81d8cc07c4e07c9f2eff1d94fd371a06070b18cb67c651c41158ec2975` |

### 검증 및 후속 적용 조건

- 다운로드 응답이 ZIP signature `PK 03 04`이며 정상 ZIP으로 파싱되는지 확인했습니다.
- PNG 5개 모두 signature `89 50 4E 47 0D 0A 1A 0A`, Pillow 디코딩 및 무결성 검사 통과, **512 × 512** 이미지입니다. HTML을 PNG 확장자로 저장한 파일은 없습니다.
- PNG 원본은 팔레트 형식(`P`)이며 투명도 정보가 있습니다. RGBA 디코딩으로 완전히 투명한 픽셀과 보이는 픽셀을 모두 확인했습니다. 원본 변환·리사이즈는 하지 않았습니다.
- 후보 이미지 시각 검토로 총구 2개, 불꽃 1개, 파편/분진 2개를 선정했습니다. 각 저장 파일은 ZIP entry와 바이트가 동일합니다.
- Unity 6/URP 런타임 표시 및 모바일 비용은 **미검증**입니다. 이 팩은 이미지이며 Unity VFX prefab이나 동작 스크립트를 제공하는 것으로 취급하지 않습니다.
- 후속 단계에서 URP `Universal Render Pipeline/Particles/Unlit`, 짧은 수명, 작은 입자 수, 풀링을 검토합니다. 텍스처 크기·플랫폼 압축은 실기 확인 후 정하며 이번 단계에서는 Import 설정과 `.meta`를 직접 변경하지 않습니다.

## Low Poly FPS Map Lite — 기존 맵

- 저장소 경로: `Assets/ThirdPartyAssets/Objects/LowPolyFPSLite/`.
- 사용 증거: `Assets/Scenes/Game/DesertHouse.unity`가 이 패키지의 건물·벽·상자·판자·항아리·계단 프리팹을 참조합니다.
- 제작자: **JustCreate**.
- 공식 배포 페이지: https://marketplace.unity.com/packages/3d/environments/low-poly-fps-map-lite-258453
- 공식 페이지 확인 내용: FREE, 버전 1.0, 2023-07-05, 원래 Unity 2021.3.0, **Standard Unity Asset Store EULA**. 페이지의 라이선스 유형 표시는 Extension Asset입니다.
- 공식 약관: https://unity.com/legal/as-terms (확인한 페이지의 개정일 2024-12-04).
- EULA 2.2.1은 상당한 독창적 콘텐츠와 결합한 Licensed Product의 내장 구성요소로 사용·배포·판매하는 범위를 허용합니다. CC0와 달리 원본 자산을 단독으로 공개 재배포할 수 있는 권한으로 해석해서는 안 됩니다. 크레딧 의무는 Standard EULA 자체에서 확인되지 않았으며 별도 제공 조건은 확인해야 합니다.
- **미확인:** 이 저장소에 반입된 파일의 최초 취득일, 실제 취득 계정/팀 권한, 원본 패키지 해시, 별도 제공자 조건. 현재 공식 상품 정보를 확인한 것이며 과거 취득 이력을 증명한 것은 아닙니다. 이번 단계에서 재다운로드·수정하지 않았습니다.
- Unity 6 공식 호환 보증은 해당 페이지에서 확인하지 못했습니다. 기존 머티리얼의 URP/Lit 참조와 실제 Unity 6 표시 검증은 구분합니다.

## 기존 오디오 — 출처·라이선스 미확인

대상은 `Assets/Audios/`의 기존 총성·발소리·재장전·수류탄·UI 효과음과 음악입니다. 예시: `FootStep.mp3`, `fire.mp3`, `Handling_Gun_01_Reload_Sq_SFX.wav`, `Pistol_01_Fire_01_SFX.wav`, `Aggressive FPS Game Music/`, `FPS Menu Music Themes Vol. 1/`, `audio/`.

파일과 폴더 이름으로 제작자·배포 사이트·상업 이용 조건을 추정하지 않습니다. 조사에서 이 파일들의 라이선스와 취득 출처를 증명하는 문서를 찾지 못했으며, **상업 이용·게임 배포·원본 재배포·출처 표기 조건은 모두 미확인**입니다. 신규 Kenney CC0 라이선스는 이 기존 오디오에 적용되지 않습니다.

배포 전 각 팩의 공식 source URL, 취득 내역, 제공 라이선스, 크레딧 요구 사항을 복원해야 합니다. 출처를 복원할 수 없는 파일은 확인 가능한 자산으로 교체할 대상으로 분류합니다. 이번 단계에서는 기존 오디오를 수정·삭제·대체하지 않았습니다.


## C3 전투 표시 적용 (2026-10-07)

- 기존 `Low Poly Weapons LITE/Weapon_02`에서 프로젝트용 `Assets/Prefabs/Weapons/RiflePresentation.prefab`을 만들었습니다. 원본은 수정하지 않았으며, 시각 모델의 축과 총구/왼손 위치만 별도 프리팹에서 구성했습니다. 기존 패키지의 취득 이력과 배포 권한 증빙은 미확인입니다.
- Kenney 총구/불꽃 PNG는 `Assets/Art/VFX/Combat/`의 URP Particles/Unlit 머티리얼과 입자 프리팹에 연결했습니다. 총구는 1개, 명중은 최대 6개 입자를 재사용하며, 새 발사 sequence에 대해서만 표시합니다. 기존 텍스처 원본은 수정하지 않았습니다.
- `Assets/Audios/Authored/RifleShot.wav`와 `ReloadTick.wav`는 이 프로젝트용으로 수학적 노이즈·사인파·감쇠 포락선을 합성한 효과음입니다. 외부 음원이나 샘플을 사용하지 않았으며, 기존 출처 미확인 오디오와 구분합니다. 생성 시드 7301/7302, 22050 Hz, 16-bit PCM, mono이며, Unity는 PCM/DecompressOnLoad/PreserveSampleRate 설정을 사용합니다. SFX 믹서 그룹에 연결했습니다.
- RifleShot: 0.16초, 7100 bytes, SHA-256 `d9c53a349d120543fa7ef1d8d7c3ad9395cf3532313a775c81475cf398ec5bcc`.
- ReloadTick: 0.22초, 9746 bytes, SHA-256 `b23486b56265debdd3aab7c6c56b6a2551e0808eee79183104219d04f3128eb7`.
- 생성 코드 및 검증 증거는 작업 백업 `sudden-force-fps-backups/20261007-c3-presentation-ui/`에 보관했습니다. 별도의 외부 라이선스 음원을 다운로드하지 않았습니다. Android 실기 비용 및 실제 다중 터치 검증은 별도 QA 대상입니다.


## 배포 전 포함 에셋 점검 (2026-10-07)

사용자는 기존 에셋을 무료이며 배포 가능한 조건으로 취득했던 것으로 기억합니다. 이 배경을 반영하되, 현재 판매 페이지의 같은 파일명만으로 로컬 파일과 상품의 동일성이나 적용 조건을 단정하지 않습니다.

읽기 전용 조사에서 미디어 794개와 활성 씬·Resources·직렬화 참조를 추적했습니다. 정적 참조는 실제 Android AAB 포함 여부를 확정하는 근거가 아니며 배포 직전 BuildReport로 확인합니다. 근거 보고서는 `D:\meee\git\sudden-force-fps-backups\20261007-license-audit\HANDOFF.txt` 및 같은 폴더의 JSON에 보존했습니다. 이번 조사에서 에셋 변경·다운로드·교체는 하지 않았습니다.

| 항목 | 확인 상태와 후속 작업 |
| --- | --- |
| Kenney 텍스처·직접 합성 효과음 2개 | 문서와 실제 파일 일치. C3 전투 프리팹 연결 확인 |
| Military 캐릭터·플레이어 애니메이션 22개·무기·기존 HUD 이미지 7개·Loading Icons | 취득 당시 출처와 조건이 현재 파일에서 확인되지 않음. 기존 취득 자료나 제작자 정보를 추가 대조 |
| Liberation Sans | 동봉 OFL 파일 확인. 배포 앱에 필요한 고지를 포함하는 경로 확인 필요 |
| EmojiOne 샘플 | 최신 Android BuildReport에서 EmojiOne.asset·EmojiOne.png 실제 포함 확인. 정확한 artwork 버전·라이선스·앱 attribution은 미확정이며 별도 조건 복원 대상. 이번 SDK 고지 변경에서 제거·교체하지 않음 |
| 기존 오디오 61개 | 정적 참조 0, Resources 밖. 실제 빌드 포함 여부를 확인하고 사용 에셋과 분리 |
| SimplePixelUI Font2 | 현재 미참조 후보. 동봉 OFL과 TTF 내부 표기 불일치가 있어 사용 전에 출처 조건 확인 |

출처가 미확인이라는 사실을 배포 불가 확정으로 해석하지 않습니다. 취득 조건을 확인한 에셋은 유지하며, 확인되지 않는 배포 포함 항목의 대응은 실제 포함 목록과 이식 영향을 검토한 뒤 결정합니다.

## SDK 고지 및 최신 Android 빌드 포함 확인 (2026-10-08)

앱의 `Assets/Legal/ThirdPartyNotices.txt`에 아래 5개 SDK의 원문 고지를 제공합니다. 기존 폰트·파티클 고지를 보존하며, 이 추가가 모든 SDK와 에셋의 배포 고지 검증 완료를 의미하지는 않습니다.

| SDK | 식별·사용 근거 | 고지 원문 출처 |
| --- | --- | --- |
| UniRx | `UpdateManager`의 `EveryUpdate` 사용. 정확한 전체 패키지 버전은 미확정이므로 버전 번호를 표시하지 않음 | [공식 MIT 원문](https://github.com/neuecc/UniRx/blob/7.1.0/LICENSE). 이 태그의 `Observable.cs`와 로컬 파일 텍스트 일치만 확인했으며 전체 패키지 버전의 증거로 취급하지 않음 |
| UniTask 2.5.10 | 로컬 `package.json` 및 씬 전환·로그인 코드 사용 | [2.5.10 MIT 원문](https://github.com/Cysharp/UniTask/blob/2.5.10/LICENSE) |
| Google Play Games plugin for Unity 2.0.0 | 로컬 package·PluginVersion 및 Android 로그인 코드 사용 | [v2.0.0 copyright 및 Apache 2.0 전문](https://github.com/playgameservices/play-games-plugin-for-unity/blob/v2.0.0/LICENSE) |
| In-game Debug Console 1.6.8 | 로컬 README, Login 씬의 prefab 참조, 최신 BuildReport의 관련 source paths 29개 확인 | [v1.6.8 MIT 원문](https://github.com/yasirkula/UnityIngameDebugConsole/blob/v1.6.8/LICENSE.txt) |
| NanoSockets | APK의 ARM64·ARMv7 `libnanosockets.so` 확인. 각 uncompressed entry SHA256가 로컬 Fusion 번들 라이브러리와 동일 | 로컬 `Assets/Photon/Fusion/Plugins/NanoSockets/libnanosockets_LICENSE.txt`의 Stanislav Denisov copyright 및 MIT 전문 |

UniRx에 포함된 Microsoft 차용 코드의 `Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.` 문구도 앱 고지에서 원문대로 보존합니다. 해당 source header의 원문은 `See License.txt in the project root for license information.`까지이며 저장소의 해당 코드 파일에 남아 있습니다. 로컬에서 그 `License.txt`는 확인하지 못했습니다. 공식 UniRx README는 Rx.NET·mono 코드 차용을 설명하므로, 원본 시대별 라이선스와 저작권 범위가 모두 해소됐다고 판단하지 않습니다.

### 실제 포함 증거 및 남은 범위

- 대조 APK: `SuddenForceFPS.UploadKeyValidation.apk`, 238,310,918 bytes, SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`.
- 해당 검증 폴더의 `packed-assets.json`은 Unity `BuildReport.GetLatestReport().packedAssets.contents.sourceAssetPath`를 모은 목록으로, source paths 2,889개를 기록합니다. 이는 최종 IL2CPP 타입·메서드 단위 사용 여부나 모든 전이 의존성의 권리 조건을 증명하는 자료는 아닙니다.
- `apk-native-inspection.json`은 native `.so` 14개를 기록합니다. ARM64의 7개는 `libc++_shared.so`, `libmain.so`, `libnanosockets.so`, `libswappywrapper.so`, `libunity.so`, `lib_burst_generated.so`, `libil2cpp.so`입니다. 이름과 포함 사실만으로 각 라이브러리 내부 OSS 목록이나 라이선스 전체를 확정하지 않습니다.
- EmojiOne의 `Assets/ThirdPartyAssets/TextMesh Pro/Resources/Sprite Assets/EmojiOne.asset` 및 `Assets/ThirdPartyAssets/TextMesh Pro/Sprites/EmojiOne.png`가 실제 packed source 목록에 있습니다. 이전 문서의 'Resources 포함 후보'에서 실제 포함 확인으로 갱신합니다. 동봉 Attribution 파일은 제작자 사이트 안내만 제공하므로 정확한 artwork 버전·라이선스·앱 attribution은 여전히 미확정입니다. 화면에서 emoji를 직접 사용하지 않는다는 이유로 미포함 처리하지 않으며, 이번 변경에서 제거·교체하거나 특정 버전의 조건을 임의 적용하지 않습니다.
- Fusion 실제 DLL/build 정보는 `2.0.13.2379`이며 `package.json`의 `1.1.0` 표기와 구분합니다. 공식 Photon OSS 문서의 runtime 내장 라이브러리, codegen 도구, 번들 폰트 범위를 별도로 대조해야 합니다. 별도 KCC 애드온은 발견되지 않았고 현재 게임은 Fusion 기본 `NetworkCharacterController`를 사용합니다.
- Unity 패키지 및 Google Play services의 최종 전이 의존성·고지, EmojiOne 조건 복원, UniRx 차용 코드의 원 권리 범위는 #42 후속 확인 대상입니다. 61-section SDK 합본을 전부 앱에 적용하지 않습니다.

근거와 원문 SHA256는 외부 작업 기록 `D:\meee\git\sudden-force-fps-backups\20261008-sdk-notices-audit\` 및 `20261008-sdk-notices-prepare\`에 보존했습니다. 고지 텍스트 제공과 실제 포함 권리 범위 확인을 구분하며 #42는 후속 확인이 끝나기 전 닫지 않습니다.
