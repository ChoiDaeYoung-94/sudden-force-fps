using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public sealed class ScreenSafeArea : MonoBehaviour
{
    private RectTransform _rect;
    private Rect _lastArea;
    private int _width, _height;
    private void OnEnable() { _rect = GetComponent<RectTransform>(); Apply(); }
    private void Update()
    {
        if (Screen.width != _width || Screen.height != _height || Screen.safeArea != _lastArea) Apply();
    }
    private void Apply()
    {
        if (_rect == null || Screen.width <= 0 || Screen.height <= 0) return;
        _width = Screen.width; _height = Screen.height; _lastArea = Screen.safeArea;
        _rect.anchorMin = new Vector2(_lastArea.xMin / _width, _lastArea.yMin / _height);
        _rect.anchorMax = new Vector2(_lastArea.xMax / _width, _lastArea.yMax / _height);
        _rect.offsetMin = _rect.offsetMax = Vector2.zero;
    }
}
