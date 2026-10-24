using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public const int MaxMessageLength = 160;
    [SerializeField] private RectTransform _content;
    [SerializeField] private UnityEngine.UI.ScrollRect _scrollRect;
    [SerializeField] private TMP_InputField _inputField;

    private readonly System.Collections.Generic.Queue<PendingMessage> _pendingMessages
        = new System.Collections.Generic.Queue<PendingMessage>();
    private ChatScrollDragObserver _dragObserver;
    private bool _flushingMessages;

    private struct PendingMessage
    {
        public readonly string Text;
        public readonly bool ForceFollow;
        public PendingMessage(string text, bool forceFollow) { Text = text; ForceFollow = forceFollow; }
    }

    private void Awake()
    {
        _inputField.onSubmit.AddListener(OnSubmit);
        EnsureDragObserver();
    }

    private void OnDestroy()
    {
        if (_inputField != null) _inputField.onSubmit.RemoveListener(OnSubmit);
        _dragObserver = null;
        _pendingMessages.Clear();
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
        AddMessage($"<You> {message}", true);
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

    public void AddMessage(string text) => AddMessage(text, false);

    private void AddMessage(string text, bool forceFollow)
    {
        bool wasEmpty = _pendingMessages.Count == 0;
        _pendingMessages.Enqueue(new PendingMessage(text, forceFollow));
        // A queue left by a drag is drained only in LateUpdate, after EndDrag dispatch.
        if (wasEmpty) FlushPendingMessages();
    }

    private void LateUpdate() => FlushPendingMessages();

    private bool EnsureDragObserver()
    {
        if (_dragObserver != null) return true;
        if (_scrollRect == null) return false;
        // The observer belongs to the ScrollRect GameObject, not this manager.
        _dragObserver = _scrollRect.GetComponent<ChatScrollDragObserver>();
        if (_dragObserver == null)
            _dragObserver = _scrollRect.gameObject.AddComponent<ChatScrollDragObserver>();
        return true;
    }

    private void FlushPendingMessages()
    {
        if (_flushingMessages || _pendingMessages.Count == 0 || !isActiveAndEnabled
            || !EnsureDragObserver() || _dragObserver.IsDragging
            || !_content.gameObject.activeInHierarchy || !_scrollRect.gameObject.activeInHierarchy) return;
        _flushingMessages = true;
        try
        {
            while (_pendingMessages.Count > 0 && !_dragObserver.IsDragging)
            {
                var pending = _pendingMessages.Dequeue();
                AddMessageCore(pending.Text, pending.ForceFollow);
            }
        }
        finally { _flushingMessages = false; }
    }

    private void AddMessageCore(string text, bool forceFollow)
    {
        Canvas.ForceUpdateCanvases();
        var viewport = _scrollRect.viewport != null
            ? _scrollRect.viewport : (RectTransform)_scrollRect.transform;
        bool follow = forceFollow || _content.rect.height <= viewport.rect.height
            || _scrollRect.verticalNormalizedPosition <= .01f;
        Vector2 position = _content.anchoredPosition;
        float height = _content.rect.height;

        GameObject message = AD.Managers.PoolM.PopFromPool(AD.GameConstants.PoolObjects.Message.ToString());
        message.transform.SetParent(_content, false);
        var label = message.GetComponent<TMP_Text>();
        label.richText = false;
        label.parseCtrlCharacters = false;
        label.text = text ?? string.Empty;
        Canvas.ForceUpdateCanvases();
        if (follow) _scrollRect.verticalNormalizedPosition = 0;
        else
        {
            // Keep existing rows in place when bottom-pivot content grows.
            position.y -= (_content.rect.height - height) * (1f - _content.pivot.y);
            _content.anchoredPosition = position;
        }
        // Exclude this layout correction from the next drag velocity sample.
        _scrollRect.Rebuild(UnityEngine.UI.CanvasUpdate.PostLayout);
    }
}
