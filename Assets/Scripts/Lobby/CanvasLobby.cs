using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLobby : MonoBehaviour
{
    [SerializeField] private GameObject _roomsPanel;
    [SerializeField] private GameObject _createRoomPanel;
    [SerializeField] private GameObject _joinPrivateRoomPanel;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Dropdown _maxPlayerDropdown;
    [SerializeField] private Toggle _privateRoomToggle;
    [SerializeField] private TMP_Text _mapNameText;
    [SerializeField] private GameObject _selectedMapIndicator;
    [SerializeField] private TMP_Text _privateRoomNameText;
    [SerializeField] private TMP_Text _sectionTitle;
    [SerializeField] private TMP_Text _sectionDescription;
    [SerializeField] private GameObject[] _navigationIndicators;

    private int _panelIndex = 0;    // 0 - Rooms, 1 - Create Room, 2 - JoinPrivateRoom
    private string _selectedMapName = string.Empty;

    private const string _roomNameMessage = "Please enter the room name...";
    private const string _mapNameMessage = "Please select a map...";

    private void OnEnable() => RefreshNavigation();

    private void RefreshNavigation()
    {
        if (_navigationIndicators != null)
            for (int i = 0; i < _navigationIndicators.Length; i++)
                if (_navigationIndicators[i] != null) _navigationIndicators[i].SetActive(i == _panelIndex);
        if (_sectionTitle != null)
            _sectionTitle.text = _panelIndex == 1 ? "CREATE MATCH" : _panelIndex == 2 ? "PRIVATE ROOM" : "PUBLIC ROOMS";
        if (_sectionDescription != null)
            _sectionDescription.text = _panelIndex == 1 ? "Choose your map and squad size."
                : _panelIndex == 2 ? "Enter the room name shared by your host." : "Join a room or create your own.";
    }

    #region Functions

    #region Panel SideMenu

    public void Rooms()
    {
        _panelIndex = 0;
        UpdatePanelMain(_panelIndex);

        // Retry only on this explicit menu action, never from a per-frame/status callback.
        var manager = NetworkRunnerManager.Instance;
        if (manager == null)
            AD.Managers.CreateNetworkRunner();
        else if (!manager.IsRetired && manager.SessionPhase == NetworkSessionPhase.Lobby)
            manager.JoinSessionLobby();
    }

    public void CreateRoom()
    {
        _panelIndex = 1;
        UpdatePanelMain(_panelIndex);
    }

    public void JoinPrivateRoom()
    {
        _panelIndex = 2;
        UpdatePanelMain(_panelIndex);
    }

    public void ExitGame()
    {
        AD.Managers.PopupM.PopupExit();
    }

    #endregion

    #region Panel Main

    private void UpdatePanelMain(int index)
    {
        RefreshNavigation();
        switch (index)
        {
            case 0:
                PanelRooms();
                break;
            case 1:
                PanelCreateRoom();
                break;
            case 2:
                PanelJoinPrivateRoom();
                break;
            default:
                AD.DebugLogger.LogError("CanvasLobby", "UpdatePanelMain, index error");
                break;
        }
    }

    private void PanelRooms()
    {
        _roomsPanel.SetActive(true);
        _createRoomPanel.SetActive(false);
        _joinPrivateRoomPanel.SetActive(false);
    }

    private void PanelCreateRoom()
    {
        _createRoomPanel.SetActive(true);
        _roomsPanel.SetActive(false);
        _joinPrivateRoomPanel.SetActive(false);
    }

    private void PanelJoinPrivateRoom()
    {
        _joinPrivateRoomPanel.SetActive(true);
        _roomsPanel.SetActive(false);
        _createRoomPanel.SetActive(false);
    }

    #endregion

    #region Rooms

    public void Confirm()
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager == null || manager.IsRetired)
        {
            AD.Managers.PopupM.PopupMessage("Lobby connection was lost. Select FIND MATCH to reconnect, then try again.");
            return;
        }

        if (!TryReadRoomName(_roomNameText, out string roomName, out string roomNameError))
        {
            AD.Managers.PopupM.PopupMessage(roomNameError);
            return;
        }

        if (string.IsNullOrEmpty(_selectedMapName))
        {
            AD.Managers.PopupM.PopupMessage(_mapNameMessage);
            return;
        }

        int maxPlayer = int.Parse(_maxPlayerDropdown.options[_maxPlayerDropdown.value].text);

        Dictionary<string, object> roomOption = new Dictionary<string, object>(){
            { "RoomName", roomName },
            { "MaxPlayers", maxPlayer },
            { "MapName", _selectedMapName },
            { "IsPrivateRoom", !_privateRoomToggle.isOn },
        };

        manager.CreateRoom(roomOption);
    }

    internal static bool TryReadRoomName(TMPro.TMP_Text label, out string roomName, out string error)
    {
        roomName = string.Empty;
        error = "Room name input is unavailable. Please reopen CREATE MATCH.";
        TMPro.TMP_InputField input = label != null
            ? label.GetComponentInParent<TMPro.TMP_InputField>(true) : null;
        if (input == null || input.textComponent != label) return false;

        // TMP's rendered label adds a caret sentinel; only input.text is user data.
        return TryNormalizeRoomName(input.text, out roomName, out error);
    }

    internal static bool TryNormalizeRoomName(string text, out string roomName, out string error)
    {
        roomName = (text ?? string.Empty).Trim();
        error = null;
        if (roomName.Length == 0)
        {
            error = _roomNameMessage;
            return false;
        }
        // The limit is UTF-16 code units; never truncate a surrogate pair.
        if (roomName.Length > 64)
        {
            error = "Keep the room name within 64 characters.";
            return false;
        }
        for (int i = 0; i < roomName.Length; i++)
        {
            char character = roomName[i];
            if (char.IsControl(character))
            {
                error = "Please write the room name on a single line without control characters.";
                return false;
            }
            if (char.IsHighSurrogate(character))
            {
                if (++i >= roomName.Length || !char.IsLowSurrogate(roomName[i]))
                {
                    error = "The room name contains an unsupported character. Please edit it and try again.";
                    return false;
                }
            }
            else if (char.IsLowSurrogate(character))
            {
                error = "The room name contains an unsupported character. Please edit it and try again.";
                return false;
            }
        }
        return true;
    }

    public void SelectMap(string mapName)
    {
        // Keep the Fusion scene key separate from the readable label.
        _selectedMapName = mapName == "DesertHouse" ? mapName : string.Empty;
        bool selected = _selectedMapName.Length > 0;
        _mapNameText.text = selected ? "Desert House" : "Choose a map below";
        if (_selectedMapIndicator != null) _selectedMapIndicator.SetActive(selected);
    }

    public void EnterPrivateRoom()
    {

    }

    #endregion

    public void ClickedUI() => AD.Managers.SoundM.UI_Click();
    public void ClickedOK() => AD.Managers.SoundM.UI_Ok();

    #endregion
}
