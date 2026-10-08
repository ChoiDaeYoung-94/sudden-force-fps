using System;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public sealed class MatchHudView : MonoBehaviour
{
    private enum MenuState { Closed, Menu, Confirm }
    private static MatchHudView _backOwner;
    public static bool OwnsBackInput => _backOwner != null && _backOwner.isActiveAndEnabled;
    [SerializeField] private CombatHudView _combatHud;
    [SerializeField] private TMP_Text _redScore, _blueScore, _clockText;
    [SerializeField] private GameObject _waitingRoot, _scoreboardRoot, _resultsRoot, _returningRoot;
    [SerializeField] private Transform _rosterTable, _scoreboardTableSlot, _resultsTableSlot;
    [SerializeField] private TMP_Text[] _rosterNames, _rosterStats, _rosterStatus;
    [SerializeField] private TMP_Text _resultTitle, _resultReason, _resultScore;
    [SerializeField] private GameObject _killFeedRoot;
    [SerializeField] private TMP_Text[] _feedKillers, _feedVictims, _feedKinds;
    [SerializeField] private TMP_Text _feedGapText;
    [SerializeField] private UnityEngine.UI.Button _returnButton;
    [SerializeField] private GameObject _mobileScoreButton, _mobileCloseButton;
    [SerializeField] private bool _previewTouchControls;
    [SerializeField] private GameObject _menuButton, _menuRoot, _menuContent, _confirmContent;
    [SerializeField] private UnityEngine.UI.Button _resumeButton, _cancelLeaveButton;
    [SerializeField] private TMP_Text _returningText;
    private MatchPresentation _presentation;
    private PlayerInputSource _blockedInput;
    private MatchSnapshot _snapshot, _finalSnapshot;
    private bool _hasSnapshot, _hasFinal, _mobileScoreOpen, _returning;
    private int _localTeam = -1;
    private float _gapUntil;
    private MenuState _menuState;
    private bool _scoreboardDismissed, _returnFailed;
    private int _lifecycleVersion;

    private bool TouchMode
    {
        get
        {
            bool touch = Application.isMobilePlatform;
#if UNITY_EDITOR
            touch |= _previewTouchControls;
#endif
            return touch;
        }
    }

    private void OnEnable()
    {
        _backOwner = this;
        _lifecycleVersion++;
        MatchPresentation.Bound += Bind;
        MatchPresentation.Unbound += Unbind;
        Bind(MatchPresentation.Instance);
        RefreshPanels();
    }

    private void Bind(MatchPresentation presentation)
    {
        Detach();
        _presentation = presentation;
        _hasSnapshot = _hasFinal = _mobileScoreOpen = false;
        _menuState = MenuState.Closed;
        _scoreboardDismissed = false;
        _localTeam = -1;
        _gapUntil = 0f;
        if (_presentation == null) { ClearDisplay(); return; }
        _presentation.Changed += ShowSnapshot;
        _presentation.KillObserved += ObserveGap;
        if (_presentation.TryGetSnapshot(out var snapshot))
        {
            var owner = NetworkRunnerManager.Instance;
            _returning = owner != null && (owner.IsRetired || owner.SessionPhase == NetworkSessionPhase.ReturningToLobby);
            if (!_returning) _returnFailed = false;
            ShowSnapshot(snapshot);
        }
    }

    private void ShowSnapshot(MatchSnapshot snapshot)
    {
        _hasSnapshot = true;
        if (snapshot.Phase == MatchPhase.Finished && !_hasFinal)
        {
            _menuState = MenuState.Closed;
            _finalSnapshot = snapshot;
            _hasFinal = true;
        }
        _snapshot = _hasFinal ? _finalSnapshot : snapshot;
        if (_presentation != null && _presentation.Owner != null && _snapshot.Roster != null)
        {
            var local = _presentation.Owner.Runner.LocalPlayer;
            foreach (var entry in _snapshot.Roster) if (entry.Player == local) { _localTeam = entry.Team; break; }
        }
        if (_redScore != null) _redScore.text = "RED  " + _snapshot.RedScore;
        if (_blueScore != null) _blueScore.text = "BLUE  " + _snapshot.BlueScore;
        if (_clockText != null)
        {
            int seconds = Mathf.CeilToInt(Mathf.Max(0f, _snapshot.Remaining));
            _clockText.text = _snapshot.Phase == MatchPhase.Waiting ? "--:--"
                : _snapshot.Phase == MatchPhase.Finished ? "FINAL" : (seconds / 60).ToString("00") + ":" + (seconds % 60).ToString("00");
        }
        DrawRoster();
        DrawFeed();
        if (_hasFinal)
        {
            if (_resultTitle != null) _resultTitle.text = ResultTitle(_snapshot.Result);
            if (_resultReason != null) _resultReason.text = ReasonText(_snapshot.EndReason);
            if (_resultScore != null) _resultScore.text = "RED " + _snapshot.RedScore + "  /  BLUE " + _snapshot.BlueScore;
        }
        RefreshPanels();
    }

    private string ResultTitle(MatchResult result)
    {
        if (result == MatchResult.Draw) return "DRAW";
        if (result == MatchResult.Cancelled) return "MATCH CANCELLED";
        if (result == MatchResult.RedWin) return _localTeam == 0 ? "VICTORY" : _localTeam == 1 ? "DEFEAT" : "RED WINS";
        if (result == MatchResult.BlueWin) return _localTeam == 1 ? "VICTORY" : _localTeam == 0 ? "DEFEAT" : "BLUE WINS";
        return "MATCH FINISHED";
    }

    private static string ReasonText(MatchEndReason reason)
    {
        switch (reason)
        {
            case MatchEndReason.TargetScore: return "TARGET SCORE REACHED";
            case MatchEndReason.TimeExpired: return "TIME EXPIRED";
            case MatchEndReason.OpponentLeft: return "OPPONENT LEFT";
            case MatchEndReason.MissingTeam: return "TEAM UNAVAILABLE";
            default: return "MATCH FINISHED";
        }
    }

    private static Color TeamColor(int team) => team == 0 ? new Color(1f, .55f, .45f) : new Color(.45f, .8f, 1f);

    private void DrawRoster()
    {
        if (_rosterNames == null) return;
        foreach (var text in _rosterNames) if (text != null) text.transform.parent.gameObject.SetActive(false);
        if (_snapshot.Roster == null) return;
        int red = 0, blue = 0;
        foreach (var entry in _snapshot.Roster)
        {
            int index = entry.Team == 0 ? red++ : NetworkMatchState.RosterCapacity + blue++;
            if (index >= _rosterNames.Length || _rosterNames[index] == null) continue;
            var name = _rosterNames[index];
            name.transform.parent.gameObject.SetActive(true);
            name.richText = false;
            name.text = entry.Name.ToString().Replace('\n', ' ').Replace('\r', ' ');
            name.color = entry.Connected ? Color.white : new Color(.6f, .65f, .7f);
            if (_rosterStats != null && index < _rosterStats.Length && _rosterStats[index] != null)
                _rosterStats[index].text = entry.Kill + " / " + entry.Death;
            if (_rosterStatus != null && index < _rosterStatus.Length && _rosterStatus[index] != null)
                _rosterStatus[index].text = entry.Connected ? string.Empty : "LEFT";
        }
    }

    private void DrawFeed()
    {
        if (_feedKillers == null) return;
        for (int i = 0; i < _feedKillers.Length; i++)
        {
            var killer = _feedKillers[i];
            if (killer == null) continue;
            int index = (_snapshot.KillFeed?.Count ?? 0) - 1 - i;
            killer.transform.parent.gameObject.SetActive(index >= 0);
            if (index < 0) continue;
            var entry = _snapshot.KillFeed[index];
            killer.richText = false;
            killer.text = entry.KillerName.ToString().Replace('\n', ' ').Replace('\r', ' ');
            killer.color = TeamColor(entry.KillerTeam);
            if (_feedVictims != null && i < _feedVictims.Length && _feedVictims[i] != null)
            {
                _feedVictims[i].richText = false;
                _feedVictims[i].text = entry.VictimName.ToString().Replace('\n', ' ').Replace('\r', ' ');
                _feedVictims[i].color = TeamColor(entry.VictimTeam);
            }
            if (_feedKinds != null && i < _feedKinds.Length && _feedKinds[i] != null)
            {
                _feedKinds[i].text = entry.Headshot ? "HEAD" : "KILL";
                _feedKinds[i].color = entry.Headshot ? new Color(1f, .8f, .2f) : Color.white;
            }
        }
    }

    private void ObserveGap(KillFeedObservation observation)
    {
        if (observation.GapBefore <= 0 || _feedGapText == null) return;
        _feedGapText.text = "EARLIER KILLS OMITTED: " + observation.GapBefore;
        _gapUntil = Time.unscaledTime + 3f;
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.Tab)) _scoreboardDismissed = false;
        if (Input.GetKeyDown(KeyCode.Escape)) HandleBack();
        RefreshPanels();
    }

    public void HandleBack()
    {
        RefreshPanels();
        if (_returning || _hasFinal) return;
        if (_menuState == MenuState.Confirm) SetMenu(MenuState.Menu);
        else if (_menuState == MenuState.Menu) SetMenu(MenuState.Closed);
        else if (_scoreboardRoot != null && _scoreboardRoot.activeSelf)
        {
            _mobileScoreOpen = false;
            _scoreboardDismissed = true;
            RefreshPanels();
        }
        else OpenMenu();
    }

    public void OpenMenu()
    {
        RefreshPanels();
        if (!_hasSnapshot || _hasFinal || _returning) return;
        SetMenu(MenuState.Menu);
    }

    public void ResumeMatch()
    {
        RefreshPanels();
        if (_returning || _hasFinal) return;
        SetMenu(MenuState.Closed);
    }

    public void RequestLeave()
    {
        RefreshPanels();
        if (_menuState != MenuState.Menu || _returning || _hasFinal) return;
        SetMenu(MenuState.Confirm);
    }

    public void CancelLeave()
    {
        RefreshPanels();
        if (_menuState != MenuState.Confirm || _returning || _hasFinal) return;
        SetMenu(MenuState.Menu);
    }

    public async void ConfirmLeave()
    {
        RefreshPanels();
        if (_menuState != MenuState.Confirm || !_hasSnapshot || _hasFinal || _returning) return;
        await BeginLobbyReturnAsync();
    }

    private void SetMenu(MenuState state)
    {
        _menuState = state;
        _mobileScoreOpen = false;
        _scoreboardDismissed = true;
        RefreshPanels();
        var selected = state == MenuState.Menu ? _resumeButton : state == MenuState.Confirm ? _cancelLeaveButton : null;
        if (UnityEngine.EventSystems.EventSystem.current != null)
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(selected != null ? selected.gameObject : null);
    }

    private static void Show(GameObject target, bool visible)
    {
        if (target != null && target.activeSelf != visible) target.SetActive(visible);
    }

    private void RefreshPanels()
    {
        var owner = NetworkRunnerManager.Instance;
        if (owner != null && (owner.IsRetired || owner.SessionPhase == NetworkSessionPhase.ReturningToLobby))
        {
            _returning = true;
            if (AD.Managers.Instance != null && AD.Managers.Instance.RecoveryStage == "Failed") _returnFailed = true;
        }
        bool finished = _hasSnapshot && _snapshot.Phase == MatchPhase.Finished;
        bool waiting = _hasSnapshot && _snapshot.Phase == MatchPhase.Waiting;
        if (finished || _returning) _menuState = MenuState.Closed;
        bool menu = _menuState != MenuState.Closed;
        bool scoreboard = _hasSnapshot && !finished && !waiting && !_returning
            && !menu && !_scoreboardDismissed && (TouchMode ? _mobileScoreOpen : Input.GetKey(KeyCode.Tab));
        bool blocked = !_hasSnapshot || waiting || finished || _returning || scoreboard || menu;
        if (_combatHud != null) _combatHud.SetMatchSuppressed(blocked);
        var input = PlayerInputSource.Instance;
        if (_blockedInput != input)
        {
            if (_blockedInput != null) _blockedInput.SetMenuInputBlocked(false);
            _blockedInput = input;
        }
        if (_blockedInput != null) _blockedInput.SetMenuInputBlocked(blocked);
        Show(_waitingRoot, waiting && !_returning && !menu);
        Show(_scoreboardRoot, scoreboard);
        Show(_resultsRoot, finished && !_returning);
        Show(_returningRoot, _returning);
        Show(_menuRoot, menu);
        Show(_menuContent, _menuState == MenuState.Menu);
        Show(_confirmContent, _menuState == MenuState.Confirm);
        Show(_menuButton, _hasSnapshot && !finished && !_returning && !menu && !scoreboard);
        Show(_killFeedRoot, _hasSnapshot && !waiting && !finished && !_returning && !scoreboard && !menu);
        Show(_mobileScoreButton, TouchMode && _hasSnapshot && !waiting && !finished && !_returning && !scoreboard && !menu);
        Show(_mobileCloseButton, TouchMode && scoreboard);
        if (_feedGapText != null) _feedGapText.enabled = Time.unscaledTime < _gapUntil;
        if (_returnButton != null) _returnButton.interactable = finished && !_returning;
        if (_returningText != null) _returningText.text = _returnFailed
            ? "LOBBY RETURN FAILED\nRESTART TO RECONNECT" : "RETURNING TO LOBBY...";
        var tableParent = finished ? _resultsTableSlot : _scoreboardTableSlot;
        if (_rosterTable != null && tableParent != null && _rosterTable.parent != tableParent)
            _rosterTable.SetParent(tableParent, false);
        if (_returningRoot != null && _returning) _returningRoot.transform.SetAsLastSibling();
        else if (_resultsRoot != null && finished) _resultsRoot.transform.SetAsLastSibling();
        else if (_menuRoot != null && menu) _menuRoot.transform.SetAsLastSibling();
        else if (_scoreboardRoot != null && scoreboard) _scoreboardRoot.transform.SetAsLastSibling();
        else if (_waitingRoot != null && waiting) _waitingRoot.transform.SetAsLastSibling();
    }

    public void ToggleScoreboard()
    {
        if (!TouchMode || !_hasSnapshot || _snapshot.Phase != MatchPhase.Running || _returning || _menuState != MenuState.Closed) return;
        _scoreboardDismissed = false;
        _mobileScoreOpen = !_mobileScoreOpen;
        RefreshPanels();
    }

    public async void ReturnToLobby()
    {
        RefreshPanels();
        if (_returning || !_hasFinal) return;
        await BeginLobbyReturnAsync();
    }

    private async System.Threading.Tasks.Task BeginLobbyReturnAsync()
    {
        if (_returning) return;
        var owner = NetworkRunnerManager.Instance;
        var recovery = AD.Managers.Instance;
        int version = _lifecycleVersion;
        _returning = true;
        _returnFailed = false;
        _menuState = MenuState.Closed;
        _mobileScoreOpen = false;
        RefreshPanels();
        if (owner == null || recovery == null) { _returnFailed = true; RefreshPanels(); return; }
        try
        {
            await owner.ReturnToLobbyAsync();
            if (this != null && version == _lifecycleVersion && ReferenceEquals(owner, NetworkRunnerManager.Instance)
                && (recovery.RecoveryStage == "Failed" || !string.IsNullOrEmpty(owner.LastRoomError)))
            {
                _returnFailed = true;
                RefreshPanels();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("[Match HUD] Lobby return failed: " + exception.GetType().Name);
            if (this != null && version == _lifecycleVersion && ReferenceEquals(owner, NetworkRunnerManager.Instance))
            {
                _returnFailed = true;
                RefreshPanels();
            }
        }
    }

    private void ClearDisplay()
    {
        if (_redScore != null) _redScore.text = "RED  --";
        if (_blueScore != null) _blueScore.text = "BLUE  --";
        if (_clockText != null) _clockText.text = "--:--";
        Show(_waitingRoot, false); Show(_scoreboardRoot, false); Show(_resultsRoot, false);
        Show(_killFeedRoot, false); Show(_mobileScoreButton, false);
        Show(_menuRoot, false); Show(_menuButton, false);
        if (_feedGapText != null) { _feedGapText.text = string.Empty; _feedGapText.enabled = false; }
    }

    private void Unbind(MatchPresentation presentation)
    {
        if (!ReferenceEquals(_presentation, presentation)) return;
        Detach();
        _hasSnapshot = _hasFinal = _mobileScoreOpen = false;
        _menuState = MenuState.Closed;
        _gapUntil = 0f;
        ClearDisplay();
        RefreshPanels();
    }

    private void Detach()
    {
        if (!ReferenceEquals(_presentation, null))
        {
            _presentation.Changed -= ShowSnapshot;
            _presentation.KillObserved -= ObserveGap;
        }
        _presentation = null;
    }

    private void OnDisable()
    {
        if (ReferenceEquals(_backOwner, this)) _backOwner = null;
        _lifecycleVersion++;
        _menuState = MenuState.Closed;
        MatchPresentation.Bound -= Bind;
        MatchPresentation.Unbound -= Unbind;
        Detach();
        _hasSnapshot = _hasFinal = _mobileScoreOpen = false;
        ClearDisplay();
        Show(_returningRoot, false);
        _returning = _returnFailed = false;
        if (_blockedInput != null) _blockedInput.SetMenuInputBlocked(false);
        _blockedInput = null;
        if (_combatHud != null) _combatHud.SetMatchSuppressed(false);
    }
}
