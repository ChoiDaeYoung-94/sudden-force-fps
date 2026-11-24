using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AD
{
    [RequireComponent(typeof(Button))]
    public class Room : MonoBehaviour
    {
        public RectTransform _thisRect;
        public int _sessionIndex = 0;

        private SessionInfo _sessionInfo;
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private TMP_Text _gameSceneNameText;
        [SerializeField] private TMP_Text _roomPlayerCountText;

        public void SetRoom(SessionInfo sessionInfo, int sessionIndex)
        {
            _sessionInfo = sessionInfo;
            _sessionIndex = sessionIndex;

            _roomNameText.richText = false;
            _roomNameText.parseCtrlCharacters = false;
            _roomNameText.text = ToDisplayLine(_sessionInfo != null ? _sessionInfo.Name : "Unavailable room");
            _gameSceneNameText.richText = false;
            _gameSceneNameText.parseCtrlCharacters = false;
            var reason = NetworkRunnerManager.GetRoomJoinBlockReason(sessionInfo);
            _gameSceneNameText.text = reason ?? "Desert House";
            GetComponent<Button>().interactable = reason == null;
            _roomPlayerCountText.text = _sessionInfo != null
                ? $"{_sessionInfo.PlayerCount} / {_sessionInfo.MaxPlayers}" : "0 / 0";
        }

        private static string ToDisplayLine(string value)
        {
            return (value ?? string.Empty).Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
        }

        public void JoinRoom()
        {
            if (NetworkRunnerManager.Instance != null) NetworkRunnerManager.Instance.JoinRoom(_sessionInfo);
        }
    }
}
