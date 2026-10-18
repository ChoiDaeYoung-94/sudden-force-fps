using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AD
{
    /// <summary>
    /// 팝업 관리 클래스
    /// 게임 씬과 로비 씬에서 뒤로 가기 버튼 클릭 시 적절한 팝업을 표시
    /// </summary>
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameObject _popupCommon;
        [SerializeField] TMP_Text _messageText;
        [SerializeField] private GameObject _popupExit;
        [SerializeField] private GameObject _popupGameOver;
        [SerializeField] private GameObject _popupLoading;
        [SerializeField] private GameObject _popupLobby;
        [SerializeField] private GameObject _popupSetting;
        [SerializeField] private GameObject _popupSceneLoading;
        [SerializeField] private GameObject _popupSetNickName;
        [SerializeField] private GameObject _bgmToggle;
        [SerializeField] private GameObject _sfxToggle;

        private Stack<GameObject> _popupStack = new Stack<GameObject>();

        /// <summary>
        /// 예외처리에 사용
        /// 로비씬 -> 오퍼월
        /// 게임씬에서 아이템을 선택하여서 사용 전인지에 대한 여부 판단
        /// Flow상 MainScene 진입 시 CanvasLobby.cs에서 false
        /// </summary>
        private bool _isException = true;
        private bool _isFlow = false;
        private bool _loadingRequested;
        private bool _sceneLoadingRequested;

        public bool IsLoading => _loadingRequested || _sceneLoadingRequested;

        /// <summary>
        /// Managers - Awake() -> Init()
        /// </summary>
        public void Init()
        {
            AD.Managers.UpdateM.OnUpdateEvent -= OnUpdate;
            AD.Managers.UpdateM.OnUpdateEvent += OnUpdate;

            SetPopup();
        }

        /// <summary>
        /// 열려 있는 모든 팝업을 비활성화하고 팝업 스택을 초기화
        /// </summary>
        public void SetPopup()
        {
            // Drain the current registrations before lifecycle callbacks run.
            // A callback may register a new popup; that new request is not this reset's target.
            var closing = new List<GameObject>();
            while (_popupStack.Count > 0) closing.Add(_popupStack.Pop());
            foreach (var popup in closing)
                if (popup != null) popup.SetActive(false);
        }

        private void OnUpdate()
        {
            if (IsLoading || MatchHudView.OwnsBackInput) return;
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && !_isFlow)
                {
                    DisablePop();
                }
            }
        }

        #region Functions
        /// <summary>
        /// 팝업이 활성화될 때 스택에 추가
        /// </summary>
        public void EnablePop(GameObject popup)
        {
            if (popup == null || _popupStack.Contains(popup)) return;
            _popupStack.Push(popup);
            AD.DebugLogger.Log("PopupManager", $"_popupStack.Count: {_popupStack.Count}, 팝업 스택에 푸시됨");
        }

        /// <summary>
        /// 명시적 닫기 요청으로 최상위 활성 팝업 한 개만 닫습니다.
        /// 생명주기의 비활성 알림은 NotifyPopupDisabled에서 별도로 처리합니다.
        /// </summary>
        public void DisablePop()
        {
            if (IsLoading) return;
            AD.Managers.SoundM.UI_Click();

            if (_isException)
            {
                AD.DebugLogger.Log("PopupManager", $"{_isException} - 예외 처리 활성");
                return;
            }

            while (_popupStack.Count > 0)
            {
                GameObject popup = _popupStack.Pop();
                if (popup == null || !popup.activeSelf) continue;
                AD.DebugLogger.Log("PopupManager", $"_popupStack.Count: {_popupStack.Count} 팝업 스택에서 팝업 제거됨");
                popup.SetActive(false);
                return;
            }
        }

        // Explicit request from a popup's own close button. Unlike Back, this
        // closes the specified object even if another normal popup is above it.
        public void ClosePopup(GameObject popup)
        {
            if (IsLoading || popup == null) return;
            AD.Managers.SoundM.UI_Click();
            if (_isException) return;
            NotifyPopupDisabled(popup);
            popup.SetActive(false);
        }

        // Lifecycle bookkeeping only: never request another close or play a sound.
        public void NotifyPopupDisabled(GameObject popup)
        {
            if (ReferenceEquals(popup, null)) return;
            var retained = new Stack<GameObject>();
            while (_popupStack.Count > 0)
            {
                var current = _popupStack.Pop();
                if (current != null && !ReferenceEquals(current, popup)) retained.Push(current);
            }
            while (retained.Count > 0) _popupStack.Push(retained.Pop());
        }

        internal void PopupLoading()
        {
            _loadingRequested = true;
            RefreshLoading();
        }

        internal void ClosePopupLoading()
        {
            _loadingRequested = false;
            RefreshLoading();
        }

        public void PopupSceneLoading()
        {
            _sceneLoadingRequested = true;
            RefreshLoading();
        }

        public void ClosePopupSceneLoading()
        {
            _sceneLoadingRequested = false;
            RefreshLoading();
        }

        private void RefreshLoading()
        {
            bool showScene = _sceneLoadingRequested;
            bool showGeneral = _loadingRequested && !showScene;
            // Disable the previous visual before enabling its replacement. PopupObject
            // releases its exception flag on disable, then the new visual sets it again.
            if (!showGeneral && _popupLoading != null) _popupLoading.SetActive(false);
            if (!showScene && _popupSceneLoading != null) _popupSceneLoading.SetActive(false);
            if (showGeneral && _popupLoading != null) _popupLoading.SetActive(true);
            if (showScene && _popupSceneLoading != null) _popupSceneLoading.SetActive(true);
        }

        internal void PopupMessage(string message)
        {
            _messageText.text = message;
            _popupCommon.SetActive(true);
        }

        public void PopupGoLobby()
        {
            UnityEngine.Time.timeScale = 0;
            _popupLobby.SetActive(true);
        }

        public void PopupExit()
        {
            UnityEngine.Time.timeScale = 0;
            _popupExit.SetActive(true);
        }

        public void PopupGameOver() => _popupGameOver.SetActive(true);

        public void PopupSetting()
        {
            UnityEngine.Time.timeScale = 0;

            float bgm = PlayerPrefs.GetFloat("BGM", 1f);
            float sfx = PlayerPrefs.GetFloat("SFX", 1f);

            // 토글 오브젝트 활성화 여부: 기본 값 1이면 비활성화, 그 외 활성화
            _bgmToggle.SetActive(bgm != 1f);
            _sfxToggle.SetActive(sfx != 1f);

            _popupSetting.SetActive(true);
        }

        public void ClosePopupSetting() => UnityEngine.Time.timeScale = 1;

        public void PopupSetNickName() => _popupSetNickName.SetActive(true);
        
        public void CheckNickName(TMP_Text nickName)
        {
            if (!AD.Utility.IsInvalid(nickName, "Warning: Nickname cannot be empty. Please enter your nickname."))
            {
                return;
            }
            else
            {
                NetworkRunnerManager.Instance.SaveNickName(nickName.text);
                _popupSetNickName.SetActive(false);
            }
        }

        public void GoLobby()
        {
            UnityEngine.Time.timeScale = 1;

            //AD.Managers.GameM.SwitchMainOrGameScene(AD.GameConstants.Scenes.Main);
        }

        //public void GameOver() => AD.Managers.GameM.GameOverGoLobby();

        public void ExitGame() => Application.Quit();


        public void SetException() => _isException = true;

        public void ReleaseException() => _isException = false;

        public void SetFlow() => _isFlow = true;

        public void ReleaseFlow()
        {
            _isFlow = false;
            foreach (var popup in _popupStack)
            {
                if (popup == null || !popup.activeInHierarchy) continue;
                var state = popup.GetComponent<PopupObject>();
                if (state != null && state.IsFlowPopup)
                {
                    _isFlow = true;
                    break;
                }
            }
        }

        public void BGM()
        {
            float bgm = PlayerPrefs.GetFloat("BGM", 1f);

            if (bgm == 1f)
            {
                _bgmToggle.SetActive(true);
                bgm = 0f;
            }
            else
            {
                _bgmToggle.SetActive(false);
                bgm = 1f;
            }

            AD.Managers.SoundM.SetBGMVolume(bgm);
            PlayerPrefs.SetFloat("BGM", bgm);
        }

        public void SFX()
        {
            float sfx = PlayerPrefs.GetFloat("SFX", 1f);

            if (sfx == 1f)
            {
                _sfxToggle.SetActive(true);
                sfx = 0f;
            }
            else
            {
                _sfxToggle.SetActive(false);
                sfx = 1f;
            }

            AD.Managers.SoundM.SetSFXVolume(sfx);
            PlayerPrefs.SetFloat("SFX", sfx);
        }
        #endregion
    }
}
