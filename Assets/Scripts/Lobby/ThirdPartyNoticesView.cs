using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class ThirdPartyNoticesView : MonoBehaviour
{
    [SerializeField] private TextAsset _notices;
    [SerializeField] private GameObject _modal;
    [SerializeField] private RectTransform _safeArea;
    [SerializeField] private TMP_Text _body;
    [SerializeField] private UnityEngine.UI.ScrollRect _scroll;
    [SerializeField] private UnityEngine.UI.Button _openButton;
    [SerializeField] private UnityEngine.UI.Button _closeButton;

    private readonly Dictionary<UnityEngine.UI.Selectable, bool> _previousInteractable = new();
    private GameObject _previousSelection;
    private Rect _lastSafeArea;
    private Vector2Int _lastScreenSize;

    private void Awake()
    {
        if (_body != null)
        {
            _body.richText = false;
            _body.text = _notices != null ? _notices.text : string.Empty;
        }
        if (_openButton != null) _openButton.onClick.AddListener(Show);
        if (_closeButton != null) _closeButton.onClick.AddListener(Close);
        if (_modal != null) _modal.SetActive(false);
    }

    public void Show()
    {
        if (_modal == null || _body == null || _notices == null || _scroll == null || _modal.activeSelf) return;
        _previousSelection = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
        _previousInteractable.Clear();
        foreach (var selectable in GetComponentsInChildren<UnityEngine.UI.Selectable>(true))
        {
            if (selectable.transform.IsChildOf(_modal.transform)) continue;
            _previousInteractable[selectable] = selectable.interactable;
            selectable.interactable = false;
        }
        _modal.SetActive(true);
        UpdateSafeArea();
        Canvas.ForceUpdateCanvases();
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(_scroll.content);
        _scroll.StopMovement();
        _scroll.verticalNormalizedPosition = 1f;
        if (EventSystem.current != null && _closeButton != null)
            EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
    }

    public void Close()
    {
        if (_modal != null) _modal.SetActive(false);
        foreach (var state in _previousInteractable)
            if (state.Key != null) state.Key.interactable = state.Value;
        _previousInteractable.Clear();
        if (EventSystem.current != null && _previousSelection != null && _previousSelection.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(_previousSelection);
        _previousSelection = null;
    }

    private void Update()
    {
        if (_modal == null || !_modal.activeSelf) return;
        UpdateSafeArea();
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
            Close();
#elif ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKeyDown(KeyCode.Escape)) Close();
#endif
    }

    private void UpdateSafeArea()
    {
        var size = new Vector2Int(Screen.width, Screen.height);
        var area = Screen.safeArea;
        if (_safeArea == null || size.x <= 0 || size.y <= 0) return;
        if (_lastScreenSize == size && _lastSafeArea == area) return;
        ApplySafeArea(area, size);
        _lastScreenSize = size;
        _lastSafeArea = area;
    }

    private void ApplySafeArea(Rect area, Vector2Int size)
    {
        _safeArea.anchorMin = new Vector2(area.xMin / size.x, area.yMin / size.y);
        _safeArea.anchorMax = new Vector2(area.xMax / size.x, area.yMax / size.y);
        _safeArea.offsetMin = Vector2.zero;
        _safeArea.offsetMax = Vector2.zero;
    }

    private void OnDisable() => Close();

    private void OnDestroy()
    {
        if (_openButton != null) _openButton.onClick.RemoveListener(Show);
        if (_closeButton != null) _closeButton.onClick.RemoveListener(Close);
    }
}
