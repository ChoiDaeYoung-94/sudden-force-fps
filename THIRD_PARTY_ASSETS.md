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
