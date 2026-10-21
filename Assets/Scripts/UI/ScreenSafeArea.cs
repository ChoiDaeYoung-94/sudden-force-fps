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
        Apply(Screen.safeArea, Screen.width, Screen.height);
    }

    private void Apply(Rect area, int width, int height)
    {
        if (_rect == null || !TryGetAnchors(area, width, height, out var min, out var max)) return;
        _rect.anchorMin = min;
        _rect.anchorMax = max;
        _rect.offsetMin = _rect.offsetMax = Vector2.zero;
        _width = width; _height = height; _lastArea = area;
    }

    private static bool TryGetAnchors(Rect area, int width, int height, out Vector2 min, out Vector2 max)
    {
        min = max = Vector2.zero;
        if (width <= 0 || height <= 0 || area.width <= 0 || area.height <= 0) return false;
        var bounds = new Vector4(area.xMin, area.yMin, area.xMax, area.yMax);
        for (int i = 0; i < 4; i++)
            if (float.IsNaN(bounds[i]) || float.IsInfinity(bounds[i])) return false;
        if (bounds.x < 0 || bounds.y < 0 || bounds.z > width || bounds.w > height
            || bounds.z <= bounds.x || bounds.w <= bounds.y) return false;
        min = new Vector2(bounds.x / width, bounds.y / height);
        max = new Vector2(bounds.z / width, bounds.w / height);
        return min.x < max.x && min.y < max.y;
    }
}
