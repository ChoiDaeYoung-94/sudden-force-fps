using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public const int MaxMessageLength = 160;
    [SerializeField] private RectTransform _content;
    [SerializeField] private UnityEngine.UI.ScrollRect _scrollRect;
    [SerializeField] private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField.onSubmit.AddListener(OnSubmit);
    }

    private void OnDestroy()
    {
        if (_inputField != null) _inputField.onSubmit.RemoveListener(OnSubmit);
    }

    private void OnSubmit(string text) => SubmitChat();

    // Shared by keyboard Submit and the explicit SEND button.
    public void SubmitChat()
    {
        if (!TryNormalizeMessage(_inputField.text, out string message, out string error))
        {
            if (error != null) ShowMessage(error);
            return;
        }
        var manager = NetworkRunnerManager.Instance;
        var runner = manager != null ? manager.GetNetworkRunner() : null;
        var canvas = CanvasRoom.Instance;
        if (manager == null || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Room
            || runner == null || !runner.IsRunning || canvas == null
            || canvas.Object == null || !canvas.Object.IsValid)
        {
            ShowMessage("Chat is unavailable while the room is reconnecting. Please try again shortly.");
            return;
        }
        canvas.RpcBroadcastChat(message, runner.LocalPlayer, manager._nickName);
        AddMessage($"<You> {message}");
        _inputField.text = string.Empty;
    }

    internal static bool TryNormalizeMessage(string text, out string message, out string error)
    {
        message = (text ?? string.Empty).Trim();
        error = null;
        if (message.Length == 0) return false;
        if (message.Length > MaxMessageLength)
        {
            error = "Keep your message within 160 characters.";
            return false;
        }
        for (int i = 0; i < message.Length; i++)
        {
            char character = message[i];
            if (char.IsControl(character))
            {
                error = "Please write your message on a single line.";
                return false;
            }
            if (char.IsHighSurrogate(character))
            {
                if (++i >= message.Length || !char.IsLowSurrogate(message[i]))
                {
                    error = "Your message contains an unsupported character. Please edit it and try again.";
                    return false;
                }
            }
            else if (char.IsLowSurrogate(character))
            {
                error = "Your message contains an unsupported character. Please edit it and try again.";
                return false;
            }
        }
        return true;
    }

    private static void ShowMessage(string message)
    {
        if (AD.Managers.Instance != null && AD.Managers.PopupM != null)
            AD.Managers.PopupM.PopupMessage(message);
    }

    public void AddMessage(string text)
    {
        GameObject message = AD.Managers.PoolM.PopFromPool(AD.GameConstants.PoolObjects.Message.ToString());
        message.transform.SetParent(_content, false);
        var label = message.GetComponent<TMP_Text>();
        label.richText = false;
        label.parseCtrlCharacters = false;
        label.text = text ?? string.Empty;
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0;
    }
}
