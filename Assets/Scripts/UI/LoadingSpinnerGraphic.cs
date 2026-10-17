using UnityEngine;

/// <summary>A project-authored loading ring with no sprite or animation asset.</summary>
[DisallowMultipleComponent]
public sealed class LoadingSpinnerGraphic : UnityEngine.UI.MaskableGraphic
{
    private const int Segments = 48;

    protected override void OnEnable()
    {
        base.OnEnable();
        raycastTarget = false;
    }

    private void Update()
    {
        rectTransform.Rotate(0f, 0f, -180f * Time.unscaledDeltaTime);
    }

    protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vertices)
    {
        vertices.Clear();
        Rect rect = GetPixelAdjustedRect();
        float outer = Mathf.Min(rect.width, rect.height) * 0.5f;
        if (outer <= 0f) return;

        float inner = outer * 0.76f;
        Vector2 center = rect.center;
        for (int segment = 0; segment < Segments; segment++)
        {
            float start = segment * Mathf.PI * 1.5f / Segments;
            float end = (segment + 1) * Mathf.PI * 1.5f / Segments;
            Vector2 a = new Vector2(Mathf.Cos(start), Mathf.Sin(start));
            Vector2 b = new Vector2(Mathf.Cos(end), Mathf.Sin(end));
            Color tint = color;
            tint.a *= Mathf.Lerp(0.2f, 1f, (float)segment / (Segments - 1));
            int first = vertices.currentVertCount;
            vertices.AddVert(center + a * inner, tint, Vector2.zero);
            vertices.AddVert(center + a * outer, tint, Vector2.zero);
            vertices.AddVert(center + b * outer, tint, Vector2.zero);
            vertices.AddVert(center + b * inner, tint, Vector2.zero);
            vertices.AddTriangle(first, first + 1, first + 2);
            vertices.AddTriangle(first, first + 2, first + 3);
        }
    }
}
