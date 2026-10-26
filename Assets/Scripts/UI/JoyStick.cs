using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    private enum Mode
    {
        Null,
        FixedArea,
        FreeArea
    }

    [Header("JoyStick 관련 세팅")]
    [SerializeField] private Mode _mode = Mode.Null;
    [SerializeField] private RectTransform _handleTransform;
    [SerializeField] private RectTransform _handleAreaTransform;
    private Vector3 _joystickVector = Vector3.zero;
    private float _joystickDistance = 0;
    public Vector2 Direction => new Vector2(_joystickVector.x, _joystickVector.y);
    public float Magnitude => _joystickDistance;
    public Vector2 NormalizedMove => _handleAreaTransform == null || _handleAreaRadius <= 0f || _joystickDistance < 5f
        ? Vector2.zero
        : Direction * Mathf.Clamp01(_joystickDistance / _handleAreaRadius);
    private float _handleAreaRadius = 0;
    private Vector3 _firstTouchPosition = Vector3.zero;
    private Vector3 _distanceVector = Vector3.zero;
    private RectTransform _inputSpace;
    private Vector3 _restHandleLocalPosition;
    private int? _pointerId;
    public bool HasCapturedPointer => _pointerId.HasValue;

    private void Awake()
    {
        _inputSpace = _handleAreaTransform != null ? _handleAreaTransform.parent as RectTransform : null;
        if (_inputSpace == null || _handleTransform == null) return;
        _restHandleLocalPosition = _handleTransform.localPosition;
        _firstTouchPosition = _inputSpace.InverseTransformPoint(_handleTransform.position);
        _distanceVector = _firstTouchPosition - _inputSpace.InverseTransformPoint(_handleAreaTransform.position);
        RefreshRadius();

        if (_mode == Mode.FixedArea)
        {
            _handleAreaTransform.gameObject.SetActive(true);
        }
        else if (_mode == Mode.FreeArea)
        {
            _handleAreaTransform.gameObject.SetActive(false);
        }
    }

    private void RefreshRadius()
    {
        // Keep the existing travel coefficient; use the input parent's UI units.
        _handleAreaRadius = _handleAreaTransform.rect.height * Mathf.Abs(_handleAreaTransform.localScale.y) * 1.3f;
    }

    private bool TryReadPoint(PointerEventData pointer, out Vector3 point)
    {
        point = Vector3.zero;
        if (_inputSpace == null || pointer == null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _inputSpace, pointer.position, pointer.pressEventCamera, out Vector2 local)) return false;
        point = new Vector3(local.x, local.y, 0f);
        return true;
    }

    #region EventTrigger
    public void PointDown(BaseEventData baseEventData)
    {
        if (!isActiveAndEnabled) return;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (_pointerId.HasValue || !TryReadPoint(pointerEventData, out Vector3 inputPos)) return;
        _pointerId = pointerEventData.pointerId;
        RefreshRadius();

        if (_mode == Mode.FreeArea)
        {
            _firstTouchPosition = inputPos;
            _handleAreaTransform.position = _inputSpace.TransformPoint(inputPos - _distanceVector);
            _handleAreaTransform.gameObject.SetActive(true);
        }

        else
        {
            // Resolve the current center after CanvasScaler/safe-area layout changes.
            _firstTouchPosition = _inputSpace.InverseTransformPoint(_handleAreaTransform.position) + _distanceVector;
        }

        _handleTransform.position = _inputSpace.TransformPoint(inputPos);
        _joystickVector = (inputPos - _firstTouchPosition).normalized;
        _joystickDistance = Vector3.Distance(inputPos, _firstTouchPosition);
    }

    public void Drag(BaseEventData baseEventData)
    {
        if (!isActiveAndEnabled) return;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (pointerEventData == null || _pointerId != pointerEventData.pointerId || !TryReadPoint(pointerEventData, out Vector3 dragPosition)) return;
        RefreshRadius();
        _joystickVector = (dragPosition - _firstTouchPosition).normalized;
        _joystickDistance = Vector3.Distance(dragPosition, _firstTouchPosition);

        if (_joystickDistance > _handleAreaRadius)
            _joystickDistance = _handleAreaRadius;

        Vector3 current = _inputSpace.InverseTransformPoint(_handleTransform.position);
        _handleTransform.position = _inputSpace.TransformPoint(Vector3.Lerp(current, _firstTouchPosition + _joystickVector * _joystickDistance, 0.7f));
    }

    public void PointUp(BaseEventData baseEventData)
    {
        if (!(baseEventData is PointerEventData pointer) || _pointerId != pointer.pointerId) return;
        ResetInput();
    }
    public void Cancel(BaseEventData baseEventData) => ResetInput();
    #endregion

    private void OnDisable()
    {
        ResetInput();
    }

    private void OnApplicationFocus(bool focused) { if (!focused) ResetInput(); }
    private void OnApplicationPause(bool paused) { if (paused) ResetInput(); }

    public void ResetInput()
    {
        _pointerId = null;
        _joystickVector = Vector3.zero;
        _joystickDistance = 0f;
        if (_handleTransform != null) _handleTransform.localPosition = _restHandleLocalPosition;
        if (_mode == Mode.FreeArea && _handleAreaTransform != null) _handleAreaTransform.gameObject.SetActive(false);
    }
}
