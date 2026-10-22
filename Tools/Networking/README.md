# 네트워크 데이터 구조와 접속 버전

앉기의 Networked 속성이 추가된 빌드는 기존 배포본과 같은 네트워크 구조가 아닙니다. PhotonAppSettings의 `AppSettings.AppVersion`을 `sff-q20261022-ladder-v1`로 고정해 빈 접속 버전을 사용하던 Player와 로비를 분리합니다. 후속 NetworkBehaviour/필드 배치 변경은 같은 문자열을 재사용하지 않고 새 프로토콜 버전과 동일 소스의 Player를 함께 검증합니다. Android 앱 버전 코드와는 별도입니다.

현재 Manager의 JoinSessionLobby/StartGame은 custom settings를 전달하지 않아 전역 설정을 사용합니다. Lobby에서 만든 연결을 StartGame이 재사용하므로 StartGame에만 새 설정을 넣는 방식은 사용하지 않습니다. 이미 연결된 Runner에 파일만 바꾸는 시험도 하지 않습니다. 기존 SDK XML의 AppVersion/region 분리 계약과 공식 [Fusion 매치메이킹 문서](https://doc.photonengine.com/fusion/v2/manual/connection-and-matchmaking/matchmaking)을 따릅니다.

2026-10-22 PC 비교에서 같은 구 Windows02 빌드의 첫 Player로 QA 방을 만들고 두 번째 구 Player의 kr 로비 목록에 해당 방과 Count1이 나타나는 양성 대조를 확인했습니다. 새 버전을 연결 전에 적용한 Editor는 같은 kr에서 Connected/list 수신 후 frame1313·3836에 Count0/QA 방 없음이었습니다. 구 대조군은 APK27이 아닌 blank AppVersion의 Crouch39 Windows 빌드입니다. 직접 방 코드 참가 거절, 신버전끼리의 실제 경기, APK 업데이트·기기 검증을 대신하지 않습니다.

단일 SerializedProperty 저장 시도는 AppVersion 반영 후 기존 `NetworkLogging` 줄도 제거해 비대상 바이트 검사에서 실패했습니다. 이 실패와 journal은 보존했습니다. 현재 SDK metadata에서 NetworkLogging은 직렬화 field/backing field가 없는 property이며 setter는 값을 저장하지 않습니다. PR에는 기존 Git blob에서 AppVersion 한 줄만 바꾼 후보를 사용해 다른 필드와 식별값을 보존합니다. 실제 작업 파일의 legacy 줄 제거와 PR의 범위는 구분하며, 이후 Unity 저장에서도 해당 줄이 다시 제거될 수 있습니다. 전역 저장·설정 값 덤프·자격정보 getter·서명은 수행하지 않았습니다.

외부 근거: `20261022-quality/protocol-compatibility-candidate/`, `protocol-compatibility-apply-01/`, `protocol-compatibility-index-01/`, `protocol-isolation-01/`. Related #174. 이 변경은 APK27/AAB28에 미포함입니다.

최초 앉기 전용 `sff-q20261022-crouch-v1`의 목록 분리 관측은 위 날짜의 이력입니다. 현재 사다리 Motor 추가 후에는 새 `sff-q20261022-ladder-v1`을 사용하며 서로 호환되는 데이터 구조로 취급하지 않습니다. 새 Windows02 빌드 성공과 실제 두 피어 경로 검증은 [등반 기록](../Movement/README.md)에서 구분합니다.
