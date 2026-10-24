# Kenney 콘크리트 발소리 파생 파일

공식 [Impact Sounds 1.0](https://kenney.nl/assets/impact-sounds)의 `footstep_concrete_000–004.ogg`를 사용합니다. 원 ZIP은 800,850B/SHA256 `029D734AF1582474EDF3A694D1B0CEBC97C1C152F2F39FA34D4C2BAFC5DE77F8`이며 동봉 License.txt가 CC0·상업적 사용 가능·선택적 credit을 명시합니다. 제품의 새 License.txt는 원문에서 CRLF/CR을 LF로만 바꾼586B입니다. 프로젝트 전체 에셋의 권리 판정과는 별개입니다.

5개를 stereo 평균→필요 시 DC 보정→22/88sample fade→기존 자체 발소리 median RMS에 맞춘 하향 gain→PCM16 mono44,100Hz로 변환했습니다. resample/trim/상향 gain은 하지 않았습니다. 원본 Vorbis에서 이미 잃은 정보를 WAV로 복구한 것이 아닙니다. 출력 약0.106–0.114초·clipping0이며 RMS 최대 오차2.22e-7입니다. peak는0.491–0.603으로 기존 약0.350보다 높으므로 같은 RMS가 같은 청감 크기라는 뜻은 아닙니다.

재생성 도구는 제품 빌드 의존성이 아닙니다. Python3.12.14/numpy2.3.5/soundfile0.13.1/libsndfile1.2.2에서 생성했으며, 승인한 별도 decoder 환경과 공식 ZIP을 지정합니다. ZIP을 자동 내려받거나 패키지를 자동 설치하지 않습니다. 기존 output을 덮어쓰지 않고 실패한 부분 결과도 보존합니다.

```powershell
python Tools/AudioAssets/prepare_kenney_footsteps.py --archive D:/approved-public/kenney_impact-sounds.zip --decoder-path D:/approved-audio-toolchain/site --output D:/new-validation/kenney-steps
```

`manifest.public.json`은 원 파생 단계의5파일·License·변환·도구/decoder 버전 기록입니다. 그 당시 `unityImporterMixerPlaybackOrAttenuationChanged=false`는 외부 변환 단계에 대한 값이며 이후 제품 참조 도입이 없었다는 뜻으로 읽지 않습니다. 도구 경로/버전 검사는 모든 의존 binary를 검증하는 sandbox가 아니며, 다른 OS/decoder에서 전체 파일 바이트 재현을 보장하지 않습니다.

제품 Unity import/팀별 clip 참조와 고지 포함·최신 빌드·실제 청음/지연 검증은 각각 별도 근거를 요구합니다. 기존 자체 발소리4개는 원형 보존합니다.
