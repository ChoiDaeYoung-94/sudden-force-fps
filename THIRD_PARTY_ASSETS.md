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

## Unity 패키지·Photon .NET 추가 고지 (2026-10-08)

앱의 `Assets/Legal/ThirdPartyNotices.txt`에 다음 5개 섹션의 고지를 추가합니다. 기존 고지 22,019바이트 전체와 기존 SDK 5개의 저작권·라이선스 원문을 보존하며, 추가 범위를 아래 항목으로 한정합니다.

| 고지 항목 | 설치·사용 근거 | 고지 출처와 적용 범위 |
| --- | --- | --- |
| Unity uGUI 2.0.0 (TextMesh Pro 포함) | 설치본 `package.json` 버전. 대조 Android BuildReport에 `Packages/com.unity.ugui/` source paths 141개 | 설치본 `LICENSE.md`의 저작권·Unity Companion License 링크·면책 원문 전체. [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license)와 구분하며, 동봉 파일 전체를 복사한 것이 UCL 웹 전문을 복사했다는 뜻은 아님 |
| Unity Universal Render Pipeline 17.3.0 | 설치본 버전. 해당 BuildReport의 URP source paths 309개 | 설치본 `LICENSE.md` 원문 전체. Unity 패키지 본체를 MIT/Apache 라이선스로 판단하지 않음 |
| Unity Render Pipeline Core 17.3.0 | 설치본 버전. 해당 BuildReport의 Core source paths 358개 | 설치본 `LICENSE.md` 원문 전체. 패키지 안의 모든 별도 third-party 구성요소 권리 확인을 대신하지 않음 |
| Photon .NET Client SDK — BigInteger | packed `Fusion.Realtime.dll`의 AssemblyRef에 `Photon3Unity3D`. 로컬 Photon DLL에 `Photon.SocketServer.Numeric.BigInteger` 및 `DiffieHellmanCryptoProvider` 타입 확인 | [Photon 공식 .NET OSS PDF](https://doc.photonengine.com/docs/content/oss-.net_client_sdks.pdf) 2쪽의 Authors, 2003 Ben Maurer·2002 Chew Keong TAN·2004/2007 Novell 저작권, MIT 허가·면책 전문. PDF 레이아웃 공백만 정리하며 문구를 생략하지 않음 |
| Photon .NET Client SDK — ZigZag Encoding | 같은 로컬 DLL의 `Protocol18.EncodeZigZag32/64`·`DecodeZigZag32/64` 메서드 확인 | 같은 공급자 PDF 2쪽의 Apache 2.0 선언에 따라 [Apache 공식 전문](https://www.apache.org/licenses/LICENSE-2.0.txt)을 제공. 공급자 PDF의 레이아웃과 공식 전문의 바이트가 같다는 주장은 하지 않으며, GPGS Google 저작권을 이 구성요소에 적용하지 않음 |

설치된 `Fusion.Common`·`Fusion.Realtime`·`Fusion.Runtime`의 실제 DLL ProductVersion은 `2.0.13.2379+4f8b2d70`, `Photon3Unity3D.dll`은 `4.1.8.21+be37a12aeee85c41f8b2eb28c44cfff8b29e5668`입니다. 이는 로컬 DLL 식별값이며 공식 OSS PDF가 해당 버전만을 대상으로 한 BOM이라는 증거는 아닙니다. Photon SDK 본체의 상용 라이선스 또는 계정·플랜 권리를 OSS 고지로 대체하지 않습니다.

포함 근거는 기존 검증 APK SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`에 연결된 source paths 2,889개의 BuildReport 스냅샷입니다. `PhotonLibs` 경로가 그 목록에 없다는 사실만으로 Photon DLL이 불포함이라고 단정하지 않습니다. 어셈블리 참조와 내부 타입 존재도 최종 IL2CPP에서 개별 타입·메서드가 생존했다는 확증은 아닙니다. 이후 새 APK에는 해당 APK의 포함 근거를 다시 연결해야 합니다.

정확한 버전별 공급자 BOM, 조건부 Fusion·셰이더·WebSocket 구성요소, Unity Player/Burst·네이티브 라이브러리 및 Android 전이 의존성, EmojiOne artwork 조건은 여전히 후속 확인 대상입니다. 이번 5개 추가는 전체 SDK·에셋의 권리 검증 완료 또는 #42 종료를 의미하지 않으며, 61개 섹션 합본을 적용하지 않습니다.

원문 사본·SHA256·공백 정리 검증·추가 섹션별 byte offset 및 보존 비교는 외부 작업 기록 `D:\meee\git\sudden-force-fps-backups\20261008-unity-photon-notices-prepare\`에 있습니다. 조건부 항목과 EmojiOne 제외 검토는 `20261008-sdk-runtime-scope\REPORT.txt` 및 `EMOJIONE_EXCLUSION_REVIEW.txt`에 보존합니다.

### Google Play services 공급자 OSS 고지 및 Android 버전 메타데이터 (2026-10-08)

아래 6개 버전의 vendor properties가 `6a8b203` 기준 `SuddenForceFPS.UploadKeyValidation.apk`에 실제 들어 있는 것을 확인했습니다. APK의 groupId/artifactId 필드는 미존재이며, entry 이름과 `version` 값으로 식별합니다. 생성된 `launcher-debug.apk`와 외부 검증 APK의 SHA-256이 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`으로 같아 두 APK의 바이트 동일성을 확인했습니다. output metadata는 `debug`, versionCode `3`, versionName `1.0.0`, 패키지 `com.AeDeong.SuddenForceFPS`입니다. 이후 다른 소스·APK에 이 결과를 확대하지 않습니다.

| 모듈 | 실제 APK properties의 version | 공식 공급자 OSS JSON 레코드 수 | 원문 배포본 |
| --- | --- | --- | --- |
| play-services-base | 18.5.0 | 16 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-base/18.5.0/play-services-base-18.5.0.aar) |
| play-services-basement | 18.4.0 | 16 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-basement/18.4.0/play-services-basement-18.4.0.aar) |
| play-services-drive | 17.0.0 | 9 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-drive/17.0.0/play-services-drive-17.0.0.aar) |
| play-services-games-v2 | 20.1.2 | 23 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-games-v2/20.1.2/play-services-games-v2-20.1.2.aar) |
| play-services-nearby | 18.5.0 | 24 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-nearby/18.5.0/play-services-nearby-18.5.0.aar) |
| play-services-tasks | 18.2.0 | 16 | [Google Maven AAR](https://dl.google.com/dl/android/maven2/com/google/android/gms/play-services-tasks/18.2.0/play-services-tasks-18.2.0.aar) |

Google 공식 Maven에서 정확한 버전의 POM·AAR을 별도로 취득했으며, 6개 공식 AAR 각각의 SHA-256이 기존 로컬 캐시 AAR과 같습니다. 캐시 존재나 수정 시각만으로 포함을 판단하지 않았습니다. APK와 생성된 merged Java resources의 6개 properties entry/version도 일치합니다. manifest blame에서 지원 wrapper `2.0.0`과 base·basement·games-v2·nearby 좌표를 보조 확인했으나, 이전 시각의 incremental 파일이므로 이것만으로 전체 선택 그래프를 확정하지 않습니다.

각 AAR의 `third_party_licenses.json`과 `.txt`에서 공급자 고지 원문을 확보했습니다. 총 104개 레코드의 바이트 offset/length를 검증하고, 33개 공급자 이름과 28개의 서로 다른 원문 본문을 앱 고지에 매핑합니다. 같은 본문 SHA만 중복 제거하며, 같은 이름의 다른 원문 및 원문 내부 서브고지는 보존합니다. 기존 폰트·파티클·Core5 및 Unity·Photon 고지는 유지합니다. 각 공급자 이름은 고지 메타데이터의 표기이며 개별 컴포넌트가 모두 최종 앱 클래스에 남아 있다는 뜻이 아닙니다. 원문에 없는 라이브러리 버전이나 SPDX 식별자를 추정해서 추가하지 않습니다.

6개 POM은 모두 `Android Software Development Kit License`와 [Google SDK terms URL](https://developer.android.com/studio/terms.html)을 지정합니다. Unity용 GPGS wrapper의 Apache 2.0 조건은 해당 wrapper 고지이며 Google Play services 상용 SDK 전체의 라이선스로 확대하지 않습니다. 공급자 OSS 고지와 상용 SDK 약관을 구분하고, 현재 웹 약관이 각 SDK 배포 당시 snapshot과 같거나 권리 조건 전체를 충족했다고 주장하지 않습니다. [Google 공식 OSS 안내](https://developers.google.com/android/guides/opensource)는 POM 및 공급자 embedded OSS 고지의 수집·표시를 설명합니다.

이번 근거는 6개 버전 메타데이터의 실제 출하, 공식·캐시 AAR 동일성, 공급자 고지 원문과 앱 고지 후보의 대응입니다. 완전한 Gradle resolved transitive closure, AndroidX 등 추가 의존성의 최종 선택과 고지, R8/DEX 클래스·메서드 생존, 각 기능의 런타임 사용, 상용 SDK 약관 권리 완료를 증명하지 않습니다. 선언된 POM 요청 버전을 최종 선택 버전으로 간주하지 않으며, #42 전체 검증 완료로 표시하지 않습니다.

외부 증빙은 `D:\meee\git\sudden-force-fps-validation\20261008-android-transitive-readonly\`의 `bounded-shipped-metadata.json`, `official-pom-map.json`, `official-aar-oss-map.json`, `vendor-license-fragments.json`, `combined-apply-verification.json`에 보존했습니다. 제품에 적용하는 고지는 검토용 머리말 348 bytes를 제외한 Google 원문 블록 624,205 bytes와 기존·Unity·Photon 고지를 바이트 그대로 연결한 후보를 기준으로 합니다. 28개 본문 전체와 104개 source mapping의 대응 검증은 고지 텍스트의 보존 검사이며 전체 앱 라이선스 완료 검사가 아닙니다.

### 고지 페이지 UI 검증 (2026-10-08)

최종 고지 파일은 660,971 bytes이며 SHA256은 `7bc6cbcff09412812115ab699b4215a1336b4ef6587a314a7a2f47b36299df2e`입니다. 기존 22,019 bytes prefix와 추가 라이선스 전문을 보존하고, 검토용 Google 머리말만 앱 표시용 제목과 분리했습니다.

`ThirdPartyNoticePager`는 원문 범위를 보존하며 현재 페이지의 문자열만 TMP에 전달합니다. 기본 한도는 4,096 UTF-16 단위/96 LF이며 실제 합본은 183페이지, 최대 4,092 단위였습니다. 폼피드 27개는 canonical 원문에 유지하고 표시에서는 페이지 구분자로 처리합니다. 페이지 source를 이어 붙인 UTF-8 bytes는 제품 파일과 정확히 같았습니다.

Unity 6000.3.25f1의 Play=false 임시 prefab/deep font·atlas·material 복사본에서 실제 Button pointer/EventSystem 경로로 183페이지 순회와 역순 이동을 확인했습니다. 표시 코드포인트 172개의 TMP glyph 대조 불일치·글리프 경고 0, 공급자 고지 28개 시작의 viewport 접근, 마지막 문장·마침표 접근, 닫기·다시 열기·빠른 페이지 이동·focus/interactable 복원·빈/누락 고지 처리를 확인했습니다. 대표 native 화면 6개를 육안 확인했으며, 모든 28개 시작을 각각 육안 검토했다는 뜻은 아닙니다.

이는 Editor UI 검증입니다. Android 물리 입력·기기 glyph·메뉴 성능이나 최종 AAB 포함 검증을 대신하지 않습니다. 임시 QA 객체를 정리했고 원본 폰트·아틀라스·기존 변경사항을 보존했습니다. 근거는 외부 `D:\meee\git\sudden-force-fps-backups\20261008-notices-full-ui-qa\HANDOFF.md` 및 같은 폴더의 source-byte-proof/glyph-and-mapping/vendor-header-access/last-bottom-access/post-qa-preservation JSON과 PNG에 있습니다. #42 전체 완료는 아닙니다.
### 광고·결제 제외 및 iOS 광고 잔재 정리 (2026-10-08)

광고·인앱 결제 기능을 제외하는 프로젝트 범위에 따라 기존 `Assets/Plugins/iOS/`의 Google Mobile Ads Unity 브리지 잔재를 제거했습니다. `GADUAdNetworkExtras.h`, `GADTSmallTemplateView.xib`, `GADTMediumTemplateView.xib`, `unity-plugin-library.a`와 각 meta 8개, 비어진 NativeTemplates/iOS 폴더 meta 2개가 대상입니다. 제거 전 10개 파일을 프로젝트 밖에 백업하고 해시를 대조했습니다.

정적 라이브러리는 x86_64/arm64 ar의 object 54개가 GADU/GADT/GAMU 이름이며, GADU 광고 생성·초기화와 GADT 템플릿 정의 및 GADMobileAds 외부 심볼로 광고 브리지임을 확인했습니다. importer의 `gvh_version-9.1.1`은 메타데이터 label이며 공식 해당 release의 바이트 동일성을 검증한 것은 아닙니다. 제거 파일의 importer는 Android 비활성/iOS 활성이고 해당 GUID·클래스의 다른 제품 파일 참조를 찾지 못했습니다.

조사 범위의 UPM manifest에 ads/purchasing/mediation 패키지가 없고 제품 스크립트·EDM/XML의 명시적 광고·결제 API/SDK 좌표도 발견되지 않았습니다. 이름에 ads가 들어간 headshot 애니메이션·ThreadScheduler 및 Google Play Games/EDM/라이선스 원문은 제거하지 않았습니다. 이는 전체 최종 DEX/모든 전이 의존성의 광고·결제 부재 증명이 아니며 최종 Android AAB 포함 검증은 별도입니다. iOS export/build도 검증하지 않았습니다.

출처·심볼·범위 근거는 외부 `D:\meee\git\sudden-force-fps-validation\20261008-ads-iap-residue-readonly\REPORT.txt` 및 archive-ownership-summary.json에 있고, 제거 전 백업은 `D:\meee\git\sudden-force-fps-backups\20261008-ads-ios-residue-removal\before-manifest.json`에 있습니다.

## AndroidX — 기준 APK 버전 metadata 및 공식 POM 조사

기준은 외부 6a8b203 UploadKeyValidation APK(SHA256 `d11fada6c8fd2e741a74ab86981bf3d05b16d519694921908a8b167e5e1e5193`)입니다. 현재 main 또는 후속 Android QA APK로 조사 결과를 확장하지 않습니다.

APK에서 13개 명시적 AndroidX `.version` entry(각 6B, 총 78B)를 관찰했습니다. entry 이름에 따른 Maven 좌표 추정과 공식 POM의 groupId/artifactId/version 확인을 구분하며, 아래 13개 좌표는 공식 POM identity와 모두 일치합니다. 이는 전체 Gradle resolved graph나 최종 클래스 생존을 확정하는 증거가 아닙니다.

| 정확한 좌표 및 버전 | 공식 POM |
| --- | --- |
| `androidx.activity:activity:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/activity/activity/1.0.0/activity-1.0.0.pom) |
| `androidx.arch.core:core-runtime:2.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/arch/core/core-runtime/2.0.0/core-runtime-2.0.0.pom) |
| `androidx.core:core:1.2.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/core/core/1.2.0/core-1.2.0.pom) |
| `androidx.customview:customview:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/customview/customview/1.0.0/customview-1.0.0.pom) |
| `androidx.fragment:fragment:1.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/fragment/fragment/1.1.0/fragment-1.1.0.pom) |
| `androidx.lifecycle:lifecycle-livedata-core:2.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-livedata-core/2.0.0/lifecycle-livedata-core-2.0.0.pom) |
| `androidx.lifecycle:lifecycle-livedata:2.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-livedata/2.0.0/lifecycle-livedata-2.0.0.pom) |
| `androidx.lifecycle:lifecycle-runtime:2.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-runtime/2.1.0/lifecycle-runtime-2.1.0.pom) |
| `androidx.lifecycle:lifecycle-viewmodel:2.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/lifecycle/lifecycle-viewmodel/2.1.0/lifecycle-viewmodel-2.1.0.pom) |
| `androidx.loader:loader:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/loader/loader/1.0.0/loader-1.0.0.pom) |
| `androidx.savedstate:savedstate:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/savedstate/savedstate/1.0.0/savedstate-1.0.0.pom) |
| `androidx.versionedparcelable:versionedparcelable:1.1.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/versionedparcelable/versionedparcelable/1.1.0/versionedparcelable-1.1.0.pom) |
| `androidx.viewpager:viewpager:1.0.0` | [공식 POM](https://dl.google.com/dl/android/maven2/androidx/viewpager/viewpager/1.0.0/viewpager-1.0.0.pom) |

공식 POM 13개는 모두 The Apache Software License, Version 2.0을 선언합니다. 공식 AAR 총 1,088,562B를 승인된 각 2MiB/합 10MiB 한도 안에서 외부 폴더로 취득했고, 13개 모두 기존 exact cache AAR와 SHA256이 일치했습니다. POM의 dependency는 선언된 요청이며 최종 선택 그래프로 취급하지 않습니다.

AAR 및 메모리 전용 classes.jar의 승인된 LICENSE/NOTICE/COPYING 및 third_party_licenses.json/txt entry는 13개 모두 `NOTFOUND_IN_APPROVED_WHITELIST`입니다. 이는 해당 범위의 관찰이며 다른 위치의 원문이나 추가 의무가 없다는 뜻이 아닙니다. 새 AndroidX 공급자 copyright는 미확인입니다. 클래스/bytecode/DEX/general strings를 읽거나 classes.jar를 저장하지 않았습니다.

앱 고지에 적용한 AndroidX 섹션은 기존 660,971B prefix를 바이트 그대로 보존하고, 위 좌표/POM 출처와 공식 Apache 약관 전문을 독립 AndroidX 섹션으로 추가합니다. 약관 출처는 [Apache 공식 원문](https://www.apache.org/licenses/LICENSE-2.0.txt)이며 기존 외부 `Apache-2.0.txt` 11,358B를 exact 복사했습니다(SHA256 `cfc7749b96f63bd31c3c42b5c471bf756814053e847c10f3eb003417bc523d30`). 이 본문을 AAR embedded NOTICE 추출 결과로 주장하지 않습니다. 공급자 copyright를 만들지 않았고, 기존 GPGS 라이선스의 Google copyright를 AndroidX에 옮기거나 귀속 범위를 확대하지 않았습니다.

기존 GPGS 본문 재사용 여부 대조는 기존 Apache 전문 포함 확인에만 사용했습니다. 이번 독립 AndroidX 섹션은 해당 GPGS copyright 본문을 복사하지 않고 지정된 공식 Apache 약관 파일만 복사합니다. 전체 전이 closure, 최종 클래스 생존, 광고 부재 또는 전체 제품 라이선스 완료는 미확정입니다. 원문 공급자 copyright와 추가 의무는 후속 공식 릴리스 근거 확인 범위로 남깁니다.

독립 검토를 통과한 후보를 앱 고지에 적용했습니다. 적용 직전 기존 660,971B prefix와 후보 SHA256을 확인했으며 최종 파일은 675,225B, SHA256 `a58eb4b964c268847a85773f96449f7f24daaba85ef734f6032b8d30476a15d6`입니다. 위 183페이지 UI 검증은 적용 전 고지의 결과입니다. 추가된 AndroidX 섹션의 Editor 표시와 최종 Android 빌드 포함 검증은 별도로 진행합니다. 외부 조사·독립 검토 근거는 `D:\meee\git\sudden-force-fps-validation\20261008-androidx-metadata-readonly\`에 보존했습니다.

### AndroidX 추가 고지의 Editor 표시 검증 (2026-10-08)

적용 후 Unity 6000.3.25f1에서 고지 TextAsset 로딩과 실제 C# pager의 총 187페이지를 확인했습니다. source slices를 UTF-8로 재결합한 bytes는 적용 파일과 정확히 같고, 기존 660,971B prefix 및 1~182페이지 source는 보존됐습니다. 연결 경계인 183페이지부터 마지막 187페이지까지 실제 Button pointer/EventSystem 경로로 이동·왕복·끝 clamp·스크롤·닫기/재열기를 확인했습니다.

추가 구간의 표시 코드포인트 154개와 새 한글 43개를 TMP glyph와 대조했으며 불일치와 관련 경고는 0입니다. 마지막 마침표와 약관 마지막 문장이 viewport에서 접근 가능했습니다. 기존 전체 183페이지의 글리프 검증을 반복한 결과로 확대하지 않습니다. 원본 폰트·아틀라스의 깊은 복사본과 임시 prefab을 사용한 Play=false Editor 검사이며 Android 기기 표시·성능·최종 AAB 포함 검증은 별도입니다.

QA 전후 19개 보존 대상의 존재·해시가 정확히 같고 임시 객체·폰트·material·atlas·EventSystem을 정리했습니다. 외부 근거는 `D:\meee\git\sudden-force-fps-backups\20261008-androidx-notices-ui-qa\HANDOFF.md`, tail-verification.json, post-qa-preservation.json 및 Boundary/TailStart/LastBottom PNG에 있습니다. #42는 추가 에셋 출처 및 최종 빌드 검증이 남아 열린 상태로 유지합니다.