using UnityEngine;

/// <summary>A project-authored circle or ring without a texture dependency.</summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasRenderer))]
public sealed class CircleGraphic : UnityEngine.UI.MaskableGraphic
{
    private const int Segments = 64;

    [SerializeField, Range(0f, 1f)] private float _innerRadiusRatio;

    protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vertices)
    {
        vertices.Clear();
        Rect rect = GetPixelAdjustedRect();
        float outer = Mathf.Min(rect.width, rect.height) * 0.5f;
        if (outer <= 0f) return;

        float ratio = Mathf.Clamp01(_innerRadiusRatio);
        Vector2 center = rect.center;
        if (ratio <= 0f)
        {
            vertices.AddVert(center, color, Vector2.zero);
            for (int segment = 0; segment <= Segments; segment++)
            {
                float angle = segment * Mathf.PI * 2f / Segments;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                vertices.AddVert(center + direction * outer, color, Vector2.zero);
                if (segment > 0) vertices.AddTriangle(0, segment, segment + 1);
            }
            return;
        }

        float inner = outer * ratio;
        for (int segment = 0; segment < Segments; segment++)
        {
            float start = segment * Mathf.PI * 2f / Segments;
            float end = (segment + 1) * Mathf.PI * 2f / Segments;
            Vector2 a = new Vector2(Mathf.Cos(start), Mathf.Sin(start));
            Vector2 b = new Vector2(Mathf.Cos(end), Mathf.Sin(end));
            int first = vertices.currentVertCount;
            vertices.AddVert(center + a * inner, color, Vector2.zero);
            vertices.AddVert(center + a * outer, color, Vector2.zero);
            vertices.AddVert(center + b * outer, color, Vector2.zero);
            vertices.AddVert(center + b * inner, color, Vector2.zero);
            vertices.AddTriangle(first, first + 1, first + 2);
            vertices.AddTriangle(first, first + 2, first + 3);
        }
    }
}
