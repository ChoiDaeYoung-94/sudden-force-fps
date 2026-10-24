using TMPro;
using UnityEngine;

/// <summary>
/// Displays lobby connection and room-list guidance in a dedicated Rooms label.
/// </summary>
[DisallowMultipleComponent]
public sealed class LobbyConnectionStatusView : MonoBehaviour
{
    [SerializeField] private TMP_Text _statusText;

    private NetworkRunnerManager _manager;
    private bool _globalLoading;

    private void OnEnable()
    {
        BindManager(NetworkRunnerManager.Instance);
    }

    private void Update()
    {
        // A runner may be created after this view, or replaced on returning to the lobby.
        NetworkRunnerManager current = NetworkRunnerManager.Instance;
        if (!ReferenceEquals(_manager, current))
        {
            BindManager(current);
        }
    }

    private void LateUpdate()
    {
        // Popup requests can change without another lobby-status notification.
        if (_globalLoading != IsGlobalLoading())
        {
            RefreshStatus();
        }
    }

    private static bool IsGlobalLoading()
    {
        var popup = AD.Managers.Instance != null ? AD.Managers.PopupM : null;
        return popup != null && popup.IsLoading;
    }

    private void OnDisable()
    {
        Unsubscribe();
        if (_statusText != null)
        {
            _statusText.enabled = false;
        }
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void BindManager(NetworkRunnerManager manager)
    {
        Unsubscribe();
        _manager = manager;
        if (_manager != null)
        {
            _manager.LobbyStatusChanged += OnLobbyStatusChanged;
        }

        RefreshStatus();
    }

    private void Unsubscribe()
    {
        // Unity's destroyed-object null comparison must not skip managed event cleanup.
        if (!ReferenceEquals(_manager, null))
        {
            _manager.LobbyStatusChanged -= OnLobbyStatusChanged;
        }

        _manager = null;
    }

    private void OnLobbyStatusChanged(NetworkRunnerManager sender)
    {
        if (!isActiveAndEnabled || !ReferenceEquals(sender, _manager)
            || !ReferenceEquals(sender, NetworkRunnerManager.Instance))
        {
            return;
        }

        RefreshStatus();
    }

    private void RefreshStatus()
    {
        _globalLoading = IsGlobalLoading();
        // Only the explicitly assigned status label belongs to this view.
        if (_statusText == null)
        {
            return;
        }

        string message = GetStatusMessage();
        if (_statusText.text != message)
        {
            _statusText.text = message;
        }

        // Keep the view and its subscriptions alive even if both share this GameObject.
        bool duplicateProgress = _manager != null
            && (_manager.LobbyStatus == LobbyConnectionStatus.Connecting
                || (_manager.LobbyStatus == LobbyConnectionStatus.Connected
                    && !_manager.HasReceivedSessionList));
        _statusText.enabled = message.Length > 0 && !(_globalLoading && duplicateProgress);
    }

    private string GetStatusMessage()
    {
        if (_manager == null)
        {
            return "Not connected. Select FIND MATCH to reconnect.";
        }

        switch (_manager.LobbyStatus)
        {
            case LobbyConnectionStatus.Connecting:
                return "Connecting to the lobby...";
            case LobbyConnectionStatus.Failed:
                return "Unable to connect. Select FIND MATCH to retry.";
            case LobbyConnectionStatus.Connected:
                if (!_manager.HasReceivedSessionList)
                {
                    return "Checking for rooms...";
                }

                return _manager.SessionCount > 0 ? string.Empty : "No public rooms available.";
            case LobbyConnectionStatus.Disconnected:
            default:
                return "Not connected. Select FIND MATCH to reconnect.";
        }
    }
}
