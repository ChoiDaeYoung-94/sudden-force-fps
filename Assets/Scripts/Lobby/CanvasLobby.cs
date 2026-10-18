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
        if (!AD.Utility.IsInvalid(_roomNameText, _roomNameMessage))
        {
            return;
        }

        if (string.IsNullOrEmpty(_selectedMapName))
        {
            AD.Managers.PopupM.PopupMessage(_mapNameMessage);
            return;
        }

        int maxPlayer = int.Parse(_maxPlayerDropdown.options[_maxPlayerDropdown.value].text);

        Dictionary<string, object> roomOption = new Dictionary<string, object>(){
            { "RoomName", _roomNameText.text },
            { "MaxPlayers", maxPlayer },
            { "MapName", _selectedMapName },
            { "IsPrivateRoom", !_privateRoomToggle.isOn },
        };

        NetworkRunnerManager.Instance.CreateRoom(roomOption);
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
