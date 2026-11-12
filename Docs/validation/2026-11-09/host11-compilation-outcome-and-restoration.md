# Host11 컴파일 완료 조건 중단과 복원

2026-11-09 KST, #347의 QA1 PC–휴대전화 연동 준비 결과입니다. **Host11은 사전검증 실패이며 두 피어 경기·보행·제품 수용이 아닙니다.** 앞서 성공한 휴대전화–LD 경기와 Native09/10 입력 관측은 각각의 기존 기록으로 보존합니다.

## 결과와 관측 범위

| 항목 | 실제 결과 |
|---|---|
| 외부 두 소스 컴파일02 | compiler1/exit0·강제종료0·held/Dispose/양 EOF·전후 입력406행 일치. DLL99,840B 생성, 로드/대상 메서드 호출/Unity 실행0 |
| 이전 컴파일01 | helperMvid의 CS0165·exit1을 보존. 새 Eval02 초기화 수정과 구분 |
| Host11 원창 | UTC 2026-11-08 17:27:09.333982 시작 /17:39:09.333982 새 작업 중지 /17:42:09.333982 전체 반환 |
| Bootstrap | CleanBuildCache 요청1/반환true·compilationStarted1/Finished1·기록된 compilerError0. GameNotRequired1·Game/Editor 완료 callback0·reload0 |
| 중단 | ACTUAL_GAME_AND_EDITOR_COMPILED_ERRORS0. H4 진입·qualification·방·경기·START·Observer0 |
| Native | 단일50168, ExitCode1/강제종료0/Dispose 오류0. 정상 종료 통과가 아닌 실패 종료 |
| 폰 | QA APK 실바이트 SHA 재확인·설치/교체0. 시작·표본2장·force-stop/HOME 등6호출 exit0, 닉네임 안내까지만 관측·방 참가0 |
| 복원 | 첫 EDM 서식 차이 차단을 보존. 정확한 서식 CAS 후 공개4,067개 rawexact·임시10파일/2폴더 부재 |
| 실제 전체 반환 | UTC 17:32:36.580794, 원 newStop/fullReturn 이전. 활성 공개8개와 private lease 회수 |

단독 컴파일의 생성 DLL SHA는 `953E5DB21E3B5604D2D3E0E1D17B7CC446ED6EC1D0717E4C1887F37CD0E79902`입니다. 독립 검토는 공개 reference DLL393개를 재해시하고 총404 입력행을 직접 대조했습니다. 도구 바이너리2개와 프로세스 생명주기는 실행 기록을 접수한 범위이며 별도 live OS 관측이 아닙니다. 생성 DLL이 실제 Unity/Game 동작을 증명하지는 않습니다.

## 콜백 해석과 다음 수정

GameNotRequired1만으로 compiler 미실행이나 캐시 재사용을 확정하지 않습니다. [Unity 6000.3.25f1 공식 태그의 C# reference](https://github.com/Unity-Technologies/UnityCsReference/blob/6000.3.25f1/Editor/Mono/Scripting/ScriptCompilation/EditorCompilation.cs#L1149-L1182)는 출력 변경이 없는 경우에도 이 callback을 내보내는 경로를 포함합니다. [CleanBuildCache 공식 설명](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityeditor/compilation/requestscriptcompilationoptions/cleanbuildcache)은 전체 스크립트 재빌드를 설명합니다. 버전명이 같은 공식 태그를 확인했으며, 설치된6000.3.25f1 바이너리와의 완전 일치·이번 실제 Csc 노드는 별도로 확인하지 않았습니다.

따라서 원 Bootstrap의 완료 callback·새 DLL emit·reload 필수 조건이 충족되지 않은 실패를 유지합니다. 다음 검증기는 새 revision에서 정상적인 출력 유지와 실제 오류를 구분하고, 입력·DLL·같은 Native 소유·시각을 다시 확인한 뒤 경기 단계로 넘겨야 합니다. 기존 실패 파일/승인 원문을 수정하거나 같은 원창을 재발급하지 않습니다.

첫 복원의 EDM 차이는 원본1,983B/SHA `01E2562923C891ACD39BCBD35BCA486695032950F41637A0FF4B8A410C2B1B6F`와 생성1,904B/SHA `4BEEB2F695E33939D4320E67237B6F5271C7B8609B15570668708504DD05E5DA`의 EOL·줄 끝 공백뿐이었습니다. 두 바이트를 외부 보존하고 같은 배타 파일 핸들의 CAS·flush/readback 뒤 별도 restore02 전체 감사를 수행했습니다. 알 수 없는 설정 변경을 자동 허용한 것이 아닙니다.

## 현재 상태

운영34/1.0.196/release12·원장196,2,34와 QA1 설치 상태를 유지합니다. QA1에는 보행 후보가 없습니다. 보행 통합03은 세 승인 SHA가 빈 값인 외부 초안입니다. 기존 진단 자산45개 생성·외부 Editor 검증과 별도로, 이 초안의 제품 바인딩·실제 compile/Weaver·평탄 구간 guard·두 피어·life/history·GPU 수용은 남아 있습니다. H4의 기존39/70 검사를 새 보행 어셈블리 승인으로 재사용하지 않습니다.

Root는 반환 뒤 prefs2 복원·760 connected-peer·Editor0/TempLock0/P13 clean을 새로 확인했습니다. 독립 검토의 공개 기록 대조와 Root의 live OS/프로젝트 관측을 구분합니다. #347/#341 및 기존 미완료 항목은 OPEN이며, 현재 필요한 사용자 조치·질문은 없습니다.

로컬 공개 근거는 `D:/codex/validation/sudden-force-fps/20261108-work/` 아래 `compile02-bounded-actual-independent-review01/`, `host-cleancompile-bootstrap01/actual-8e17834e7da34270a1e4640c1329a18f/`, `twopeer-host-control-preparation05/actual-8e17834e7da34270a1e4640c1329a18f/`, `root-resume01/device-case11-actual-return01.public.json`과 `host11-root-post-return-observation01.public.json`입니다. 원시 로그·개인키·기기 식별자·private lease 내용은 공개 저장소에 포함하지 않았습니다.
