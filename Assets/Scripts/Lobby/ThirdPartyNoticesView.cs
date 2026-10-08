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
    [SerializeField] private UnityEngine.UI.Button _previousButton;
    [SerializeField] private UnityEngine.UI.Button _nextButton;
    [SerializeField] private TMP_Text _pageLabel;

    private readonly Dictionary<UnityEngine.UI.Selectable, bool> _previousInteractable = new();
    private GameObject _previousSelection;
    private Rect _lastSafeArea;
    private Vector2Int _lastScreenSize;
    private ThirdPartyNoticePager _pager;
    private int _pageIndex;
    private bool _showing;

    private void Awake()
    {
        if (_body != null)
        {
            _body.richText = false;
            _body.parseCtrlCharacters = false;
            _body.text = string.Empty;
        }
        if (_openButton != null) _openButton.onClick.AddListener(Show);
        if (_closeButton != null) _closeButton.onClick.AddListener(Close);
        if (_previousButton != null) _previousButton.onClick.AddListener(PreviousPage);
        if (_nextButton != null) _nextButton.onClick.AddListener(NextPage);
        if (_modal != null) _modal.SetActive(false);
    }

    public void Show()
    {
        if (_modal == null || _body == null || _scroll == null || _showing) return;
        _previousSelection = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
        _previousInteractable.Clear();
        foreach (var selectable in GetComponentsInChildren<UnityEngine.UI.Selectable>(true))
        {
            if (selectable.transform.IsChildOf(_modal.transform)) continue;
            _previousInteractable[selectable] = selectable.interactable;
            selectable.interactable = false;
        }
        _showing = true;
        _modal.SetActive(true);
        UpdateSafeArea();
        try
        {
            _pager ??= new ThirdPartyNoticePager(_notices != null ? _notices.text : string.Empty);
            _pageIndex = 0;
            DisplayPage();
        }
        catch (System.InvalidOperationException)
        {
            _body.text = string.Empty;
            if (_pageLabel != null) _pageLabel.text = "고지를 표시할 수 없습니다.";
            SetNavigation(false, false);
        }
        if (EventSystem.current != null && _closeButton != null)
            EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
    }

    public void Close()
    {
        _showing = false;
        if (_scroll != null) _scroll.StopMovement();
        if (_modal != null) _modal.SetActive(false);
        if (_body != null) _body.text = string.Empty;
        foreach (var state in _previousInteractable)
            if (state.Key != null) state.Key.interactable = state.Value;
        _previousInteractable.Clear();
        if (EventSystem.current != null && _previousSelection != null && _previousSelection.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(_previousSelection);
        _previousSelection = null;
    }

    private void PreviousPage()
    {
        if (!_showing || _pager == null || _pageIndex <= 0) return;
        _pageIndex--;
        DisplayPage();
    }

    private void NextPage()
    {
        if (!_showing || _pager == null || _pageIndex + 1 >= _pager.Count) return;
        _pageIndex++;
        DisplayPage();
    }

    private void DisplayPage()
    {
        _scroll.StopMovement();
        var count = _pager.Count;
        _body.text = count > 0 ? _pager.GetDisplayText(_pageIndex) : string.Empty;
        if (_pageLabel != null)
            _pageLabel.text = count == 0 ? "표시할 고지가 없습니다."
                : $"{_pageIndex + 1} / {count}" + (_pageIndex + 1 == count ? " · 고지 끝" : string.Empty);
        var selected = EventSystem.current != null ? EventSystem.current.currentSelectedGameObject : null;
        SetNavigation(_pageIndex > 0, _pageIndex + 1 < count);
        Canvas.ForceUpdateCanvases();
        if (_scroll.content != null)
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(_scroll.content);
        _scroll.Rebuild(UnityEngine.UI.CanvasUpdate.PostLayout);
        _scroll.verticalNormalizedPosition = 1f;
        // Content has a top anchor/pivot; keep the top fixed if glyph/layout settles next frame.
        if (_scroll.content != null)
            _scroll.content.anchoredPosition = new Vector2(_scroll.content.anchoredPosition.x, 0f);
        // A disabled last/first button must not retain controller focus.
        if (EventSystem.current != null)
        {
            if (_nextButton != null && selected == _nextButton.gameObject && !_nextButton.interactable)
                EventSystem.current.SetSelectedGameObject(_previousButton != null && _previousButton.interactable
                    ? _previousButton.gameObject : _closeButton != null ? _closeButton.gameObject : null);
            else if (_previousButton != null && selected == _previousButton.gameObject && !_previousButton.interactable)
                EventSystem.current.SetSelectedGameObject(_nextButton != null && _nextButton.interactable
                    ? _nextButton.gameObject : _closeButton != null ? _closeButton.gameObject : null);
        }
    }

    private void SetNavigation(bool previous, bool next)
    {
        if (_previousButton != null) _previousButton.interactable = previous;
        if (_nextButton != null) _nextButton.interactable = next;
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
        if (_previousButton != null) _previousButton.onClick.RemoveListener(PreviousPage);
        if (_nextButton != null) _nextButton.onClick.RemoveListener(NextPage);
    }
}
