using Fusion;
using TMPro;
using UnityEngine;

namespace AD
{
    public class Room : MonoBehaviour
    {
        public RectTransform _thisRect;
        public int _sessionIndex = 0;

        private SessionInfo _sessionInfo;
        private bool _hasMapName;
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private TMP_Text _gameSceneNameText;
        [SerializeField] private TMP_Text _roomPlayerCountText;

        public void SetRoom(SessionInfo sessionInfo, int sessionIndex)
        {
            _sessionInfo = sessionInfo;
            _sessionIndex = sessionIndex;

            _roomNameText.text = _sessionInfo != null ? _sessionInfo.Name : "Unavailable room";
            _hasMapName = false;
            _gameSceneNameText.text = "Unknown map";
            if (_sessionInfo != null && _sessionInfo.Properties != null
                && _sessionInfo.Properties.TryGetValue("MapName", out var map)
                && map != null && map.IsString)
            {
                string mapName = map;
                _hasMapName = !string.IsNullOrEmpty(mapName);
                if (_hasMapName) _gameSceneNameText.text = mapName == "DesertHouse" ? "Desert House" : mapName;
            }
            _roomPlayerCountText.text = _sessionInfo != null
                ? $"{_sessionInfo.PlayerCount} / {_sessionInfo.MaxPlayers}" : "0 / 0";
        }

        public void JoinRoom()
        {
            if (_sessionInfo == null || !_hasMapName || _sessionInfo.PlayerCount >= _sessionInfo.MaxPlayers)
            {
                return;
            }

            if (NetworkRunnerManager.Instance != null) NetworkRunnerManager.Instance.JoinRoom(_sessionInfo);
        }
    }
}
