# 확정 닉네임의 로컬 저장과 Editor 회귀

## 변경과 현재 범위

[#376](https://github.com/ChoiDaeYoung-94/sudden-force-fps/issues/376) / [PR377](https://github.com/ChoiDaeYoung-94/sudden-force-fps/pull/377)은 정상 확인한 닉네임을 `PlayerPrefs`에 저장하고 다음 프로세스의 첫 조회에서 유효한 저장값을 읽도록 변경합니다. `Managers`, `NetworkRunnerManager`, `PopupManager` 3개 C#이 제품 변경 범위입니다.

기존 1~32 UTF-16 단위·trim·제어문자·서로게이트 검증을 유지합니다. Runner 교체와 로비 복귀의 세션 이름 대입은 메모리만 갱신하며 저장하지 않습니다. 저장값이 없거나 유효하지 않으면 기존 입력 안내로 돌아가고 원값을 임의 삭제하지 않습니다. 저장소 오류가 나면 현재 세션의 유효한 이름을 유지하며 로그에는 예외 종류만 남깁니다.

제품 검증 소스는 `8dbc9a07fa8e452bd7a57ae664c8879f581a6d7c`입니다. 이 문서 커밋과 빌드 소스를 구분합니다. 기존 QA3/code3/1.0.197-qa3 APK와 운영34에는 이 변경이 포함되지 않았습니다. 새 Android 빌드·설치·cold/갱신 후 닉네임 유지와 Play 배포는 후속입니다.

## 실제 검증

| 검증 | 결과와 한계 |
|---|---|
| P14 제품 컴파일 | 별도 main 기반 체크아웃의 실제 Assembly-CSharp에서 변경 메서드·동일 제품 MVID를 확인했습니다. `95a257a8-ee38-48c9-8412-f5c1de0021ae`이며 제품 C# 오류0입니다. |
| 원문 발췌 C#·가짜 저장소 | 54검사를 통과했습니다. 누락·유효값·한글/emoji·길이/제어문자/서로게이트·교체·메모리 setter·Get/Set/Save 예외·개인값 로그 비노출을 검사했습니다. 실제 저장소의 예외 주입이나 기기 저장성은 아닙니다. |
| Preview03 실제 콜백 | 실제 `Managers`와 `CheckNickName` 경로의 15검사가 `PASSED_RESTORED`입니다. trim 정상 확인·키 저장·cache 초기화 후 재읽기·교체·invalid33/control/null·세션 setter의 저장 부재를 검사했습니다. 실제 NetworkRunner 연결 호출은0입니다. |
| 격리 상태 회수 | 시작 키가 없을 때만 실행했습니다. 종료 후 own 키 부재·원 cache·임시 GO0·Preview0·원 empty clean scene·새 Mesh0·pending/deletionSavePending false를 확인했습니다. 별도 읽기 전용 조회도 같은 상태입니다. |
| 프로세스·공유 설정 | warm Native40456은 12:16:17.5623126 UTC에 정상 Exit0이며 capture EOF2/dispose 오류0입니다. watch05의 315 metadata 표본에서 관측한12세대의 잔류0·부재2표본·전역 Editor0, Prefs2 원값 복구를 확인했습니다. 전체 생성 이력이나 모든 자식의 추적 완전성은 증명하지 않습니다. |
| 공개 입력 반환 | Root는 공개4,082개/752,656,176B 원바이트 일치·private6 부재/indexS·Git clean을 확인했습니다. 이번 자동 변경은 DOTweenSettings/EDM 2개이며 생성본을 외부 보존한 뒤 해당 파일만 원복했습니다. ShaderGraph 변화는0입니다. |

warm 원창은 2026-11-11 12:12:26 / 새 작업 중단12:37:26 / 전체 반환12:42:26 UTC이며 실제 전체 반환은 **12:20:38.175506 UTC**입니다. 원창을 연장하지 않았고 window/binding 승인은 비활성화했습니다. 독립 검토의 후속 named-file 감사도 공개4,082개 원바이트 일치이며 Root 반환 시각의 원자적 재검증으로 확대하지 않습니다.

동일 프로세스에서 cache를 비운 뒤 읽은 결과를 실제 프로세스 cold 저장성으로 합산하지 않습니다. 기존 사용자 키가 존재한 상태의 native 예외 주입이나 모든 Runner 수명주기도 별도입니다.

## 보존한 실패

- 앞선 AdditiveScene fixture02는 unsaved Untitled scene에서 새 additive scene 생성이 거절되어 **0검사 실패**였습니다. 키 쓰기는0이며 원 상태를 확인한 뒤 정상 소유 Editor 종료로 volatile pending 상태를 회수했습니다.
- 첫 cold 회차의 watch04는 관측257세대에서 기존256 bound를 넘어 실패했습니다. 이후 별도 부재 관측과 원본 복구는 보존하지만 이 watcher를 성공으로 바꾸지 않습니다. warm 회차는 별도 nonce와 watch05/2048 bound를 사용했습니다.

## 근거 위치와 다음 확인

PC 외부 증거 루트는 `D:/codex/validation/sudden-force-fps/20261111-evening-work/`입니다. 공개 저장소에는 원시 로그나 개인값을 옮기지 않습니다.

- `root-resume01/nickname-preview03-actual01.public.json`: 실제15검사 결과.
- `root-resume01/nickname-preview03-after-readonly01.public.json`: 회수 후 별도 조회.
- `root-resume01/nickname-main-full-return-614b38edbdfa48d7beec2b514f87fa32/`: 원창·활성 binding 보존본, 자동 변경 보존·공개 입력 감사·실제 반환.
- `nickname-preview03-actual-independent-review01/`: 이번 warm 결과와 반환의 독립 검토. 앞 cold 회차의 `nickname-main-actual-independent-review01/`과 구분합니다.

다음 포함 APK에서 재설치/데이터 초기화 없이 정상 이름 확정→완전 종료→cold 실행→로비 이름 유지와 이름 교체 후 cold를 검증합니다. 기존 QA3의 닉네임 재입력 관측을 새 수정의 성공으로 사용하지 않습니다. #163/#43 등 상위 미완료 범위는 유지합니다.
