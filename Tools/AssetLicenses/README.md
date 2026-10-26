# 에셋 원본 대응 기록

## DOTween 1.2.765 선택 파일의 공식 배포본 대응 (2026-10-26, #257/#42)

[원저자 목록](https://dotween.demigiant.com/download.php)의 2024-02-04판 [1.2.765 ZIP](https://dotween.demigiant.com/downloads/DOTween_1_2_765.zip)을 외부 검증 폴더에서 확인했습니다. ZIP215,500B/SHA `B359376326F333D6A132E0CBE0DC7A9CF18EE6C8DE89739193E5438E27DBB6A0`의27항목 CRC를 검사했고 runtime DLL·Editor DLL·readme 세 파일은 저장소 원본과 바이트가 같습니다. runtime DLL은 새 Windows03 내장본과도 같습니다.

공개 Cecil metadata에서 `DG.Tweening.DOTween.Version`의 정적 대입값1.2.765를 읽었으며 assemblyVersion1.0.0.0과 구분했습니다. 검사한 DLL을 metadata reader가 로드·호출한 것은 아닙니다. [선택 파일 대응표](dotween-1.2.765-binary-provenance.json)를 따릅니다.

SDK 업그레이드·DLL/원래 readme·앱 고지 변경은 없습니다. ZIP에 별도 license 파일은 없고 readme가 원저자 URL을 가리킵니다. 현재 [라이선스 페이지](https://dotween.demigiant.com/license.php)는 참고로만 연결하며 2024년 조건·모든 module/생성 파일·과거 취득·Pro 여부·전체 권리 완료로 확대하지 않습니다. #42는 남은 범위를 유지하고 아래 이전 기록을 보존합니다.

## URP BlueNoise256 7개 — 원저자 CC0 대응 (2026-10-22, #42)

코드28의 알려진 공개 packed 경로에서 추가로 열거된 `Packages/com.unity.render-pipelines.universal/Textures/BlueNoise256/LDR_LLL1_[0..6].png` 7개의 원저자 대응을 보강했습니다. 설치된 URP17.3.0의 해당7개는 [Christoph Peters의 공식 배포 페이지](https://momentsingraphics.de/BlueNoise.html)가 연결한 [원본 ZIP](https://momentsingraphics.de/Media/BlueNoise/FreeBlueNoiseTextures.zip)의 `Data/256_256/` 동명7개와 바이트·SHA256이 모두 같습니다. 저자는 해당 페이지의 `A database of blue noise textures` 절에서 texture database를 [CC0](https://creativecommons.org/publicdomain/zero/1.0/)로 공개한다고 명시합니다.

| 파일명 | bytes | 로컬·원저자 entry 공통 SHA256 |
| --- | ---: | --- |
| `LDR_LLL1_0.png` | 99,216 | `90564273ED872DA744AA7CC87D81B0090C7103EAB2F223225C357909B41DA803` |
| `LDR_LLL1_1.png` | 99,204 | `7360C0D8880110EC841114C6AEA59C39123D1FB8B261E6BF9FC9573B65B0D0EA` |
| `LDR_LLL1_2.png` | 99,123 | `D7DB3110BD3F60169E09066E061E161E522625B013A557E923EB384A97316D5F` |
| `LDR_LLL1_3.png` | 99,283 | `19C67A1256B2C20C779470719386737AAA3366BD2058197BBFC3EBEA71E5DE29` |
| `LDR_LLL1_4.png` | 99,205 | `2C928BFFFC64C339F980C02A022BF0AC2E3B37DF29B079E44625B97FDDE76058` |
| `LDR_LLL1_5.png` | 99,194 | `5C9977B8FC9B7C83D00CA60937977824BADC5EB2CE3C744539F96CB22796157B` |
| `LDR_LLL1_6.png` | 99,248 | `E35AF946A68313AD0973BC298BFB2DB18238F885EE04B4A7E760C75ACAFC777A` |

원본 ZIP의 HEAD 길이는42,288,737B입니다. 전체 ZIP을 받지 않고 exact HTTP206범위로 중앙 디렉터리1743개 항목과 선택7개를 총947,003B 읽었습니다. Content-Range·길이, central/local 파일명, 압축 방식, 복원 크기·CRC32를 검사한 뒤 로컬7개와 비교했습니다. 전체 ZIP SHA나 전체 아카이브 무결성·고정 배포 버전을 검증했다는 뜻은 아닙니다. 경로·CRC·판정은 [공개 출처 대응표](urp-blue-noise-256-provenance.json)에 있습니다.

이는 패키지 소스7개의 원저자/조건 대응입니다. 코드28 AAB 내부의 개별 PNG payload identity, 모든 URP texture·SDK 권리, 새 코드29의 실제 packed 포함을 증명하지 않습니다. 새 필수 고지 누락을 발견한 판정이 아니며 제품 고지·에셋·빌드는 변경하지 않았습니다. 새29의 Military7개는 별도 정적 참조 후보로서 이 검증에 포함하지 않습니다. 전체 #42는 남은 범위를 유지합니다.

원저자 선택7PNG와 범위 응답 SHA·검사 스크립트는 외부 `D:/codex/validation/sudden-force-fps/20261022-evening/asset-rights-followup/`에 보존합니다. 이 외부 폴더는 현재 PC의 검증 근거이며 게임 빌드 입력이 아닙니다. 원저자 PNG를 별도로 저장소에 추가하지 않았습니다.
