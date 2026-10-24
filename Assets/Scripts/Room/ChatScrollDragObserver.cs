using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public sealed class ChatScrollDragObserver : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private UnityEngine.UI.ScrollRect _scrollRect;
    private bool _dragging;

    public bool IsDragging
    {
        get
        {
            // ScrollRect.OnDisable cancels its drag even if this component stays enabled.
            if (_scrollRect == null || !_scrollRect.isActiveAndEnabled) _dragging = false;
            return _dragging;
        }
    }

    private void Awake() => _scrollRect = GetComponent<UnityEngine.UI.ScrollRect>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActiveAndEnabled && _scrollRect != null && _scrollRect.isActiveAndEnabled
            && eventData.button == PointerEventData.InputButton.Left) _dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) _dragging = false;
    }

    private void OnDisable() => _dragging = false;
}
