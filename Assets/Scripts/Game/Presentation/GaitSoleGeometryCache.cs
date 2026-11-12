using System;
using UnityEngine;

// External source draft. No asset is created here. All approval hashes stay empty.
// The 453 calf-influenced vertices are WITHIN the single 704-vertex boots mesh.
[Serializable] internal sealed class GaitContactGeometryContract
{
    public string schema, rendererPath, meshName, meshSourceGuid, meshSourceAssetSha256;
    public string rendererQuality, projectSkinWeights, planePolicy;
    public string[] bonePaths;
    public Vector3[] vertices;
    public float[] bindposeRowMajor, weights;
    public int[] counts, boneIndices, weightSumBits;
    public bool[] vertexIsLeft;
    public int vertexCount, boneCount, influenceCount, calfAffectedVertices, singleFootVertices;
    public float ownerOriginPlaneOffsetMetres;
}

internal sealed class GaitSoleGeometryCache
{
    private readonly SkinnedMeshRenderer _renderer;
    private readonly Mesh _mesh;
    private readonly Transform _rootBone, _rendererParent;
    private readonly Transform[] _bones, _parents;
    private readonly Matrix4x4[] _world;
    private readonly Vector3[] _bindPoints;
    private readonly int[] _offsets, _counts, _boneIndices;
    private readonly float[] _weights;
    private readonly bool[] _left;
    private readonly int _quality, _projectWeights;
    internal readonly float OwnerOriginPlaneOffsetMetres;

    internal GaitSoleGeometryCache(WorldBodyVisualRigBinding rig, Transform[] legs,
        GaitContactGeometryContract g)
    {
        Need(g != null && g.schema == "SFF_GAIT_BOOTS704_FULL_WEIGHT_LBS_V1"
            && g.vertexCount == 704 && g.boneCount == 61 && g.influenceCount == 1428
            && g.calfAffectedVertices == 453 && g.singleFootVertices == 46
            && g.planePolicy == "owner-origin-y-flat-support-diagnostic-only",
            "GAIT_FIXED_BOOT_GEOMETRY_CONTRACT");
        Need(IsSha(g.meshSourceAssetSha256) && !string.IsNullOrEmpty(g.meshSourceGuid)
            && Finite(g.ownerOriginPlaneOffsetMetres), "GAIT_GEOMETRY_IDENTITY_AND_PLANE_EVIDENCE");
        Need(rig.Root != null && rig.Root.transform.parent != null && legs.Length == 12
            && !string.IsNullOrEmpty(g.rendererPath) && !g.rendererPath.StartsWith("/")
            && g.rendererPath.IndexOf("..", StringComparison.Ordinal) < 0,
            "GAIT_EXISTING_RENDERER_PATH");
        var rendererTransform = rig.Root.transform.parent.Find(g.rendererPath);
        _renderer = rendererTransform != null ? rendererTransform.GetComponent<SkinnedMeshRenderer>() : null;
        Need(_renderer != null && _renderer.sharedMesh != null && _renderer.rootBone != null,
            "GAIT_EXISTING_BOOT_RENDERER");
        _mesh = _renderer.sharedMesh; _rootBone = _renderer.rootBone; _rendererParent = _renderer.transform.parent;
        Need(_mesh.name == g.meshName && _mesh.vertexCount == 704 && _mesh.blendShapeCount == 0
            && _renderer.GetComponent<Cloth>() == null
            && _renderer.quality.ToString() == g.rendererQuality
            && QualitySettings.skinWeights.ToString() == g.projectSkinWeights
            && g.rendererQuality == "Auto" && g.projectSkinWeights == "Unlimited",
            "GAIT_FIXED_NONBLENDSHAPE_FULL_WEIGHT_BOOT_MODEL");
        _quality = (int)_renderer.quality; _projectWeights = (int)QualitySettings.skinWeights;
        Need(g.vertices != null && g.vertices.Length == 704 && g.counts != null && g.counts.Length == 704
            && g.vertexIsLeft != null && g.vertexIsLeft.Length == 704
            && g.weightSumBits != null && g.weightSumBits.Length == 704
            && g.weights != null && g.weights.Length == 1428
            && g.boneIndices != null && g.boneIndices.Length == 1428
            && g.bonePaths != null && g.bonePaths.Length == 61
            && g.bindposeRowMajor != null && g.bindposeRowMajor.Length == 61 * 16,
            "GAIT_FULL_VERTEX_CSR_BINDPOSE_DATA_REQUIRED");
        _bones = _renderer.bones; // One bind-time allocation; never called in the per-pass sampler.
        Need(_bones.Length == 61, "GAIT_FIXED_BOOT_BONE_COUNT");
        _parents = new Transform[61]; _world = new Matrix4x4[61];
        var bindposes = new Matrix4x4[61];
        for (int bone = 0; bone < 61; bone++)
        {
            Need(_bones[bone] != null && _bones[bone] == rig.Root.transform.Find(g.bonePaths[bone])
                && (_bones[bone] == rig.Root.transform || _bones[bone].IsChildOf(rig.Root.transform)),
                "GAIT_EXACT_OWNED_VISUAL_BONE_REFERENCE");
            _parents[bone] = _bones[bone].parent;
            for (int row = 0; row < 4; row++) for (int col = 0; col < 4; col++)
            {
                float value = g.bindposeRowMajor[bone * 16 + row * 4 + col];
                Need(Finite(value), "GAIT_FINITE_BINDPOSE"); bindposes[bone][row, col] = value;
            }
        }
        _offsets = new int[704]; _counts = (int[])g.counts.Clone();
        _left = (bool[])g.vertexIsLeft.Clone(); _weights = (float[])g.weights.Clone();
        _boneIndices = (int[])g.boneIndices.Clone(); _bindPoints = new Vector3[1428];
        int offset = 0, calfVertices = 0, singleFootVertices = 0, leftCount = 0;
        for (int vertex = 0; vertex < 704; vertex++)
        {
            Need(Finite(g.vertices[vertex]) && _counts[vertex] > 0
                && offset + _counts[vertex] <= 1428, "GAIT_VERTEX_WEIGHT_RANGE");
            _offsets[vertex] = offset; float sum = 0f; bool calf = false;
            if (_left[vertex]) leftCount++;
            for (int j = offset; j < offset + _counts[vertex]; j++)
            {
                int b = _boneIndices[j]; float weight = _weights[j];
                Need(b >= 0 && b < 61 && Finite(weight) && weight > 0f && weight <= 1f,
                    "GAIT_FULL_WEIGHT_VALUE");
                var bone = _bones[b]; bool isLeft = bone == legs[1] || bone == legs[3] || bone == legs[4];
                bool isRight = bone == legs[7] || bone == legs[9] || bone == legs[10];
                Need((isLeft || isRight) && isLeft == _left[vertex], "GAIT_FULL_STREAM_SIDE_CLASSIFICATION");
                calf |= bone == legs[1] || bone == legs[7]; sum += weight;
                // Preserve C10's nested order: worldBone * (bindpose * bindVertex).
                // Do not reassociate matrices, normalize weights, or use a foot rigid hull.
                _bindPoints[j] = bindposes[b].MultiplyPoint3x4(g.vertices[vertex]);
            }
            Need(Bits(sum) == g.weightSumBits[vertex], "GAIT_EXACT_DECLARED_WEIGHT_SUM_BITS");
            if (calf) calfVertices++;
            if (_counts[vertex] == 1 && _weights[offset] == 1f
                && (_bones[_boneIndices[offset]] == legs[3] || _bones[_boneIndices[offset]] == legs[9])) singleFootVertices++;
            offset += _counts[vertex];
        }
        Need(offset == 1428 && calfVertices == 453 && singleFootVertices == 46
            && leftCount > 0 && leftCount < 704, "GAIT_NO_DROPPED_OR_DUPLICATED_BOOT_VERTICES");
        OwnerOriginPlaneOffsetMetres = g.ownerOriginPlaneOffsetMetres;
    }

    // Bind-time data is immutable inside this cache. Steady-state: no arrays, hashing,
    // mesh-data allocating getters, BakeMesh, private reflection, renderer.bones calls or source writes.
    internal void MeasureSoles(float planeWorldY, out float left, out float right)
    {
        Need(Finite(planeWorldY) && _renderer != null && _renderer.sharedMesh == _mesh
            && _renderer.rootBone == _rootBone && _renderer.transform.parent == _rendererParent
            && (int)_renderer.quality == _quality && (int)QualitySettings.skinWeights == _projectWeights,
            "GAIT_BOOT_BINDING_OR_QUALITY_CHANGED");
        for (int bone = 0; bone < 61; bone++)
        {
            Need(_bones[bone] != null && _bones[bone].parent == _parents[bone], "GAIT_BOOT_BONE_PARENT_CHANGED");
            _world[bone] = _bones[bone].localToWorldMatrix;
        }
        left = float.PositiveInfinity; right = float.PositiveInfinity;
        for (int vertex = 0; vertex < 704; vertex++)
        {
            Vector3 result = Vector3.zero;
            for (int j = _offsets[vertex]; j < _offsets[vertex] + _counts[vertex]; j++)
                result += _world[_boneIndices[j]].MultiplyPoint3x4(_bindPoints[j]) * _weights[j];
            Need(Finite(result), "GAIT_FINITE_FULL_WEIGHT_SOLE");
            float y = result.y - planeWorldY;
            if (_left[vertex]) left = Mathf.Min(left, y); else right = Mathf.Min(right, y);
        }
    }
    internal static bool IsSha(string s)
    {
        if (s == null || s.Length != 64) return false;
        for (int i = 0; i < s.Length; i++) if (!(s[i] >= '0' && s[i] <= '9') && !(s[i] >= 'A' && s[i] <= 'F')) return false;
        return true;
    }
    private static bool Finite(float v) => PlayerInputSource.IsFinite(v);
    private static bool Finite(Vector3 v) => Finite(v.x) && Finite(v.y) && Finite(v.z);
    private static void Need(bool ok, string code) { if (!ok) throw new InvalidOperationException(code); }
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    private struct FloatBits
    {
        [System.Runtime.InteropServices.FieldOffset(0)] internal float value;
        [System.Runtime.InteropServices.FieldOffset(0)] internal int bits;
    }
    private static int Bits(float value) => new FloatBits { value = value }.bits;
}
