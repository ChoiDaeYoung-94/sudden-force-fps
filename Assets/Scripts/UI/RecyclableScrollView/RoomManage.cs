using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RoomManage : MonoBehaviour
{
    private static RoomManage _instance;
    public static RoomManage Instance { get { return _instance; } }

    [Header("--- RoomManage data ---")]
    [SerializeField] private GameObject _contentItem;
    [SerializeField] private RectTransform _scrollViewParentRect; // 최대 사이즈(sizeDelta.y)
    [SerializeField] private RectTransform _contentRect;
    [SerializeField] private GridLayoutGroup _contentGridLayoutGroup;
    [SerializeField] private ContentSizeFitter _contentContentSizeFitter;
    [SerializeField] private int _minimumPlusLine = 4;
    [SerializeField] private GameObject _moveToTop;
    [SerializeField] private GameObject _blockScroll;
    [SerializeField, Range(0f, 1f)] private float _lerp = 0f;   // MoveToTop lerp에 사용 될 값
    private CancellationTokenSource _ctsMoveToTop;

    // 그 외 접근 불가 데이터
    private List<AD.Room> _leastItemObjectList = new List<AD.Room>();
    private LinkedList<AD.Room> _activeItemsList = new LinkedList<AD.Room>();
    private LinkedList<AD.Room> _inActiveItemsList = new LinkedList<AD.Room>();
    private float _contentHeight = 0f;
    private float _intervalHeight = 0f;
    private List<float> _anchorXList = new List<float>();
    private float _activeItemFirstLineAnchorY = 0f;
    private float _activeItemLastLineAnchorY = 0f;
    private float _anchorXIndexOflastActiveItem = 0f;
    private int _activeItemFirstIndex = 0;
    private int _activeItemLastIndex = 0;

    // Session
    private List<SessionInfo> _sessionList = new List<SessionInfo>();
    private NetworkRunnerManager _manager;
    private float _lastLayoutWidth = -1f;
    private bool _waitingForWidth;

    private void Awake()
    {
        _instance = this;

        _ctsMoveToTop = new CancellationTokenSource();
    }

    private void OnEnable()
    {
        BindManager(NetworkRunnerManager.Instance);
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void BindManager(NetworkRunnerManager manager)
    {
        Unsubscribe();
        _manager = manager;
        if (_manager != null) _manager.LobbyStatusChanged += OnLobbyStatusChanged;
        RefreshFromManager();
    }

    private void Unsubscribe()
    {
        // A destroyed Unity component can still own a managed event delegate.
        if (!ReferenceEquals(_manager, null)) _manager.LobbyStatusChanged -= OnLobbyStatusChanged;
        _manager = null;
    }

    private void OnLobbyStatusChanged(NetworkRunnerManager sender)
    {
        if (!isActiveAndEnabled || !ReferenceEquals(sender, _manager)
            || !ReferenceEquals(sender, NetworkRunnerManager.Instance)) return;
        RefreshFromManager();
    }

    private void RefreshFromManager()
    {
        if (_manager == null || _manager.IsRetired
            || _manager.LobbyStatus != LobbyConnectionStatus.Connected
            || !_manager.HasReceivedSessionList)
        {
            Init(null);
            return;
        }
        Init(new List<SessionInfo>(_manager.SessionListSnapshot));
    }

    private void OnDestroy()
    {
        Unsubscribe();
        _ctsMoveToTop?.Cancel();
        _ctsMoveToTop?.Dispose();
        _ctsMoveToTop = null;

        if (ReferenceEquals(_instance, this)) _instance = null;
    }

    public void Init(List<SessionInfo> sessionList)
    {
        // UI changes its own list; never insert/remove entries in a callback's list.
        var snapshot = sessionList == null ? new List<SessionInfo>() : new List<SessionInfo>(sessionList);
        if (_leastItemObjectList.Count == 0)
        {
            _sessionList = snapshot;
            Settings();
            if (_sessionList.Count == 0)
            {
                _waitingForWidth = false;
                ResetEmptyList();
                return;
            }
            if (!TryUpdateGridWidth())
            {
                _waitingForWidth = true;
                return;
            }
            _waitingForWidth = false;
            SetAnchorX();
            CreateContentItems();
        }
        else
        {
            TryUpdateGridWidth();
            RefreshSessionList(snapshot);
        }
    }

    private void Update()
    {
        var current = NetworkRunnerManager.Instance;
        if (!ReferenceEquals(_manager, current)) BindManager(current);
        bool widthReady = TryUpdateGridWidth();
        if (_waitingForWidth && widthReady && _leastItemObjectList.Count == 0 && _sessionList.Count > 0)
        {
            _waitingForWidth = false;
            Settings();
            CreateContentItems();
        }
        if (_contentRect.anchoredPosition.y >= 1000f)
            _moveToTop.SetActive(true);
        else
            _moveToTop.SetActive(false);

        if (_activeItemsList.Count > 0)
        {
            ScrollDown();
            ScrollUp();
        }
    }

    #region Functions

    #region Init

    /// <summary>
    /// Item, spacing, padding을 고려한 Content의 총 Height 계산
    /// 그외 필요한 부분 계산
    /// </summary>
    private void Settings()
    {
        var lineCount = Math.Ceiling((float)_sessionList.Count / _contentGridLayoutGroup.constraintCount);

        float contentSize = (float)lineCount * _contentGridLayoutGroup.cellSize.y;
        float contentSpacingSize = (float)Math.Max(0, lineCount - 1) * _contentGridLayoutGroup.spacing.y;
        float GLGTopBotPadding = _contentGridLayoutGroup.padding.top + _contentGridLayoutGroup.padding.bottom;

        _contentHeight = contentSize + contentSpacingSize + GLGTopBotPadding;

        _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, _contentHeight);

        // cellSizeY + spacingY => 각 item의 간격
        _intervalHeight = _contentGridLayoutGroup.cellSize.y + _contentGridLayoutGroup.spacing.y;
    }

    private bool TryUpdateGridWidth()
    {
        // The serialized parent is PanelRooms; the actual mask viewport can be
        // narrower when ScrollRect expands/collapses its vertical scrollbar.
        float width = _scrollViewParentRect.rect.width;
        if (float.IsNaN(width) || float.IsInfinity(width) || width <= 0f) return false;
        var viewport = _contentRect.parent as RectTransform;
        if (viewport != null)
        {
            float viewportWidth = viewport.rect.width;
            if (float.IsNaN(viewportWidth) || float.IsInfinity(viewportWidth) || viewportWidth <= 0f) return false;
            width = Mathf.Min(width, viewportWidth);
        }
        int columns = _contentGridLayoutGroup.constraintCount;
        if (columns <= 0) return false;
        float cellWidth = (width - _contentGridLayoutGroup.padding.horizontal
            - _contentGridLayoutGroup.spacing.x * (columns - 1)) / columns;
        if (float.IsNaN(cellWidth) || float.IsInfinity(cellWidth) || cellWidth <= 0f) return false;
        bool anchorsMatch = _anchorXList.Count == columns
            && Mathf.Approximately(_anchorXList[0], _contentGridLayoutGroup.padding.left)
            && (columns == 1 || Mathf.Approximately(_anchorXList[1],
                _contentGridLayoutGroup.padding.left + cellWidth + _contentGridLayoutGroup.spacing.x));
        if (Mathf.Approximately(_lastLayoutWidth, width) && anchorsMatch
            && Mathf.Approximately(_contentGridLayoutGroup.cellSize.x, cellWidth)
            && Mathf.Approximately(_contentRect.rect.width, width)) return true;

        _contentGridLayoutGroup.cellSize = new Vector2(cellWidth, _contentGridLayoutGroup.cellSize.y);
        SetAnchorX();
        _contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        _lastLayoutWidth = width;
        ApplyGridWidthToPool();
        return true;
    }

    private void ApplyGridWidthToPool()
    {
        if (_anchorXList.Count == 0) return;
        foreach (var room in _leastItemObjectList)
        {
            room._thisRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _contentGridLayoutGroup.cellSize.x);
            var position = room._thisRect.anchoredPosition;
            int column = Math.Max(0, room._sessionIndex) % _anchorXList.Count;
            room._thisRect.anchoredPosition = new Vector2(_anchorXList[column], position.y);
        }
        SetContentItems();
    }

    private void SetAnchorX()
    {
        _anchorXList.Clear();
        for (int i = 0; i < _contentGridLayoutGroup.constraintCount; i++)
        {
            float anchor = _contentGridLayoutGroup.padding.left + (_contentGridLayoutGroup.cellSize.x * i) + (_contentGridLayoutGroup.spacing.x * i);
            _anchorXList.Add(anchor);
        }
    }

    /// <summary>
    /// Item을 최소 라인수를 맞춰 생성
    /// 생성 후 필요한 값들 계산
    /// 세팅 후 GLG, CSF를 비활성화
    /// </summary>
    private void CreateContentItems()
    {
        float height = _scrollViewParentRect.sizeDelta.y - _contentGridLayoutGroup.padding.top;

        int minLine = Math.Max(2, (int)Math.Truncate(height / (_contentGridLayoutGroup.cellSize.y + _contentGridLayoutGroup.spacing.y)) + _minimumPlusLine);

        int settingAmount = minLine * _contentGridLayoutGroup.constraintCount;
        for (int i = 0; i < settingAmount; i++)
        {
            AD.Room temp_room = Instantiate(_contentItem, _contentRect).GetComponent<AD.Room>();
            _leastItemObjectList.Add(temp_room);

            _inActiveItemsList.AddLast(temp_room);

            temp_room.gameObject.SetActive(false);
        }

        settingAmount = settingAmount > _sessionList.Count ? _sessionList.Count : settingAmount;
        for (int i = 0; i < settingAmount; i++)
        {
            _leastItemObjectList[i].gameObject.SetActive(true);

            AD.Room item_room = _leastItemObjectList[i];
            item_room.SetRoom(_sessionList[i], i);

            _activeItemsList.AddLast(item_room);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRect);
        }

        foreach (AD.Room room in _activeItemsList)
            _inActiveItemsList.Remove(room);

        _activeItemFirstIndex = 0;
        _activeItemLastIndex = settingAmount - 1;

        for (int i = 0; i < _anchorXList.Count; i++)
            if (_anchorXList[i] == _activeItemsList.Last.Value._thisRect.anchoredPosition.x)
            {
                _anchorXIndexOflastActiveItem = i;
                break;
            }

        _contentGridLayoutGroup.enabled = false;
        _contentContentSizeFitter.enabled = false;
        ApplyGridWidthToPool();
        SetContentItems();
    }

    private void ResetEmptyList()
    {
        _activeItemsList.Clear();
        _inActiveItemsList.Clear();
        foreach (var room in _leastItemObjectList)
        {
            room.gameObject.SetActive(false);
            _inActiveItemsList.AddLast(room);
        }
        _activeItemFirstIndex = 0;
        _activeItemLastIndex = -1;
        _anchorXIndexOflastActiveItem = 0;
        _activeItemFirstLineAnchorY = -_contentGridLayoutGroup.padding.top;
        _activeItemLastLineAnchorY = _activeItemFirstLineAnchorY;
        var scroll = _contentRect.GetComponentInParent<UnityEngine.UI.ScrollRect>();
        if (scroll != null) scroll.StopMovement();
        _contentRect.anchoredPosition = new Vector2(_contentRect.anchoredPosition.x, 0f);
        if (_moveToTop != null) _moveToTop.SetActive(false);
        if (_blockScroll != null) _blockScroll.SetActive(false);
    }

    private void RebindEmptyWindowFromTop()
    {
        // A shrink can remove the entire scrolled window while earlier rooms remain.
        // Reuse the existing pool instead of leaving a nonempty snapshot with no rows.
        ResetEmptyList();
        int count = Math.Min(_leastItemObjectList.Count, _sessionList.Count);
        for (int i = 0; i < count; i++)
        {
            var room = _inActiveItemsList.First.Value;
            _inActiveItemsList.RemoveFirst();
            room.SetRoom(_sessionList[i], i);
            room._thisRect.anchoredPosition = new Vector2(_anchorXList[i % _anchorXList.Count],
                -_contentGridLayoutGroup.padding.top - (i / _anchorXList.Count) * _intervalHeight);
            room.gameObject.SetActive(true);
            _activeItemsList.AddLast(room);
        }
        _activeItemLastIndex = count - 1;
    }

    #endregion

    #region Scroll

    /// <summary>
    /// ScrollRect -> On Value Changed에서 호출
    /// </summary>
    public void SetContentItems()
    {
        if (_activeItemsList.Count > 0)
        {
            _activeItemFirstLineAnchorY = _activeItemsList.First.Value._thisRect.anchoredPosition.y;
            _activeItemLastLineAnchorY = _activeItemsList.Last.Value._thisRect.anchoredPosition.y;
            _anchorXIndexOflastActiveItem = _activeItemsList.Last.Value._sessionIndex % _anchorXList.Count;
        }
    }

    /// <summary>
    /// 윗 라인을 지우고 아래 라인을 채움
    /// </summary>
    void ScrollDown()
    {
        if (_activeItemLastIndex + 1 >= _sessionList.Count)
            return;

        foreach (AD.Room item in _activeItemsList)
        {
            if (item._thisRect.anchoredPosition.y - _intervalHeight >= -_contentRect.anchoredPosition.y)
            {
                _inActiveItemsList.AddLast(item);
                item.gameObject.SetActive(false);
            }
            else
                break;
        }

        foreach (AD.Room item in _inActiveItemsList)
            _activeItemsList.Remove(item);

        if (_inActiveItemsList.Count > 0)
        {
            foreach (AD.Room item in _inActiveItemsList)
            {
                if (_activeItemLastIndex + 1 < _sessionList.Count)
                {
                    ++_activeItemLastIndex;

                    item.SetRoom(_sessionList[_activeItemLastIndex], _activeItemLastIndex);
                    _activeItemsList.AddLast(item);

                    if (_anchorXIndexOflastActiveItem + 1 >= _anchorXList.Count)
                    {
                        _anchorXIndexOflastActiveItem = 0;
                        _activeItemLastLineAnchorY -= _intervalHeight;
                    }
                    else
                        ++_anchorXIndexOflastActiveItem;

                    item._thisRect.anchoredPosition = new Vector2(_anchorXList[(int)_anchorXIndexOflastActiveItem], _activeItemLastLineAnchorY);

                    item.gameObject.SetActive(true);
                }
                else
                    break;
            }
        }

        foreach (AD.Room item in _activeItemsList)
            _inActiveItemsList.Remove(item);

        _activeItemFirstIndex = _activeItemsList.First.Value._sessionIndex;
    }

    /// <summary>
    /// 아래 라인을 지우고 윗 라인을 채움
    /// </summary>
    void ScrollUp()
    {
        if (_activeItemsList.Count == 0 || _anchorXList.Count == 0) return;
        int firstIndex = _activeItemsList.First.Value._sessionIndex;
        if (firstIndex <= 0) return;

        int columns = _anchorXList.Count;
        int firstColumn = firstIndex % columns;
        float firstY = _activeItemsList.First.Value._thisRect.anchoredPosition.y;
        // A partial first row needs only its missing prefix, at the SAME Y.
        // A complete first row needs the preceding row only after it moves below
        // the viewport top. Never prepend more items than exist before index 0.
        if (firstColumn == 0 && firstY >= -_contentRect.anchoredPosition.y) return;
        int count = Math.Min(firstIndex, firstColumn == 0 ? columns : firstColumn);

        while (_inActiveItemsList.Count < count && _activeItemsList.Count > 1)
        {
            var last = _activeItemsList.Last.Value;
            _activeItemsList.RemoveLast();
            last.gameObject.SetActive(false);
            _inActiveItemsList.AddLast(last);
        }
        if (_inActiveItemsList.Count < count)
        {
            // Preserve the remaining active row if an invalid undersized pool
            // cannot supply the prefix. Normal pools contain at least two rows.
            _activeItemLastIndex = _activeItemsList.Last.Value._sessionIndex;
            SetContentItems();
            return;
        }

        int firstRow = firstIndex / columns;
        for (int i = 0; i < count; i++)
        {
            int index = firstIndex - i - 1;
            var item = _inActiveItemsList.First.Value;
            _inActiveItemsList.RemoveFirst();
            item.SetRoom(_sessionList[index], index);
            item._thisRect.anchoredPosition = new Vector2(_anchorXList[index % columns],
                firstY + (firstRow - index / columns) * _intervalHeight);
            item.gameObject.SetActive(true);
            _activeItemsList.AddFirst(item);
        }
        _activeItemFirstIndex = _activeItemsList.First.Value._sessionIndex;
        _activeItemLastIndex = _activeItemsList.Last.Value._sessionIndex;
        SetContentItems();
    }

    #endregion

    #region Scroll Effect

    /// <summary>
    /// ScrollView -> Viewport -> MoveCurLevelEffect 클릭 시 Event Trigger로 호출
    /// </summary>
    public void MoveToTop() => MoveToTopAsync(_ctsMoveToTop.Token).Forget();

    private async UniTask MoveToTopAsync(CancellationToken token)
    {
        _blockScroll.SetActive(true);

        while (_contentRect.anchoredPosition.y >= 1f && !token.IsCancellationRequested)
        {
            _contentRect.anchoredPosition = Vector2.Lerp(_contentRect.anchoredPosition, new Vector2(_contentRect.anchoredPosition.x, 0f), _lerp);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        _blockScroll.SetActive(false);
    }

    /// <summary>
    /// ScrollView -> BlockScroll 클릭 시 Event Trigger로 호출
    /// </summary>
    public void BlockScroll() => _ctsMoveToTop.Cancel();

    #endregion

    #region Refresh Items

    private void RefreshSessionList(List<SessionInfo> newList)
    {
        for (int i = 0; i < _sessionList.Count; i++)
        {
            bool isRemoved = true;
            for (int j = 0; j < newList.Count; j++)
            {
                if (_sessionList[i]?.Name == newList[j]?.Name)
                {
                    isRemoved = false;
                    break;
                }
            }

            if (isRemoved)
            {
                RemoveSessionAt(i);
                _sessionList.RemoveAt(i);
                --i;
            }
        }

        for (int i = 0; i < newList.Count; i++)
        {
            bool isAdd = true;
            for (int j = 0; j < _sessionList.Count; j++)
            {
                if (newList[i]?.Name == _sessionList[j]?.Name)
                {
                    // Membership is unchanged, but counts/properties and the join
                    // target must come from the latest snapshot object.
                    _sessionList[j] = newList[i];
                    isAdd = false;
                    break;
                }
            }

            if (isAdd)
            {
                _sessionList.Insert(0, newList[i]);
                AddSessionAtTop();
            }
        }

        Settings();
        if (_sessionList.Count == 0)
        {
            ResetEmptyList();
            return;
        }
        if (_activeItemsList.Count == 0) RebindEmptyWindowFromTop();
        foreach (AD.Room room in _activeItemsList)
            room.SetRoom(_sessionList[room._sessionIndex], room._sessionIndex);
        SetContentItems();
    }

    /// <summary>
    /// 제거해야 할 List를 받고 현재 보이는 Item을 기준으로 제거
    /// 현재 보이는 Item의 뒷 부분이라면 무시 -> 마지막에 List 덮어 씌움
    /// </summary>
    public void RemoveSessionAt(int removeIndex)
    {
        if (removeIndex < _activeItemFirstIndex)
        {
            foreach (AD.Room item in _activeItemsList)
                MoveActiveItemAfterRemoveItem(item);
        }
        else if (removeIndex <= _activeItemLastIndex)
        {
            AD.Room removedItem = FindItemByIndex(removeIndex);
            _activeItemsList.Remove(removedItem);

            removedItem.gameObject.SetActive(false);
            _inActiveItemsList.AddLast(removedItem);

            foreach (AD.Room item in _activeItemsList)
            {
                int oldIndex = item._sessionIndex;

                if (oldIndex > removeIndex)
                    MoveActiveItemAfterRemoveItem(item);
            }
        }

        if (_activeItemsList.Count > 0)
        {
            _activeItemFirstIndex = _activeItemsList.First.Value._sessionIndex;
            _activeItemLastIndex = _activeItemsList.Last.Value._sessionIndex;
        }
        else
        {
            _activeItemFirstIndex = 0;
            _activeItemLastIndex = -1;
        }
    }

    /// <summary>
    /// 아이템이 추가될 경우 항상 가장 첫번째로(index => 0) 삽입 후 
    /// 기존 Item들의 Index를 ++
    /// </summary>
    public void AddSessionAtTop()
    {
        foreach (AD.Room room in _activeItemsList)
            MoveActiveItemBeforeAddItem(room);

        if (_activeItemFirstIndex == 0)
        {
            if (_inActiveItemsList.Count > 0)
            {
                AD.Room newItem = _inActiveItemsList.First.Value;
                _inActiveItemsList.RemoveFirst();

                newItem.SetRoom(_sessionList[0], 0);
                newItem.gameObject.SetActive(true);

                newItem._thisRect.anchoredPosition =
                    new Vector2(_anchorXList[0], -_contentGridLayoutGroup.padding.top);

                _activeItemsList.AddFirst(newItem);
            }
            else
            {
                AD.Room newItem = _activeItemsList.Last.Value;
                _activeItemsList.RemoveLast();

                newItem.SetRoom(_sessionList[0], 0);

                newItem._thisRect.anchoredPosition =
                    new Vector2(_anchorXList[0], -_contentGridLayoutGroup.padding.top);

                _activeItemsList.AddFirst(newItem);
            }
        }

        _activeItemFirstIndex = _activeItemsList.First.Value._sessionIndex;
        _activeItemLastIndex = _activeItemsList.Last.Value._sessionIndex;
    }

    private AD.Room FindItemByIndex(int index)
    {
        foreach (AD.Room item in _activeItemsList)
        {
            if (item._sessionIndex == index)
                return item;
        }

        return null;
    }

    private void MoveActiveItemAfterRemoveItem(AD.Room item)
    {
        item._sessionIndex = item._sessionIndex - 1;

        for (int j = 0; j < _anchorXList.Count; j++)
        {
            if (item._thisRect.anchoredPosition.x == _anchorXList[j])
            {
                if (j == 0)
                {
                    item._thisRect.anchoredPosition =
                        new Vector2(_anchorXList[_anchorXList.Count - 1], item._thisRect.anchoredPosition.y + _intervalHeight);

                    break;
                }
                else
                {
                    item._thisRect.anchoredPosition =
                        new Vector2(_anchorXList[j - 1], item._thisRect.anchoredPosition.y);

                    break;
                }
            }
        }
    }

    private void MoveActiveItemBeforeAddItem(AD.Room item)
    {
        item._sessionIndex = item._sessionIndex + 1;

        for (int j = 0; j < _anchorXList.Count; j++)
        {
            if (item._thisRect.anchoredPosition.x == _anchorXList[j])
            {
                if (j == _anchorXList.Count - 1)
                {
                    item._thisRect.anchoredPosition =
                        new Vector2(_anchorXList[0], item._thisRect.anchoredPosition.y - _intervalHeight);

                    break;
                }
                else
                {
                    item._thisRect.anchoredPosition =
                        new Vector2(_anchorXList[j + 1], item._thisRect.anchoredPosition.y);

                    break;
                }
            }
        }
    }

    #endregion

    #endregion
}