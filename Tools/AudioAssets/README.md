# 자체 발소리

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
