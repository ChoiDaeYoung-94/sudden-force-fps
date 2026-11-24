using System;
using System.Collections.Generic;
using UnityEngine;

// Plain per-owner configuration. No ScriptableObject or prefab serialization.
[Serializable]
public sealed class WorldRifleBodyPoseSettings
{
    public float Forward = .24f;
    public float Down = .06f;
    public float Inward = -.01f;
    public Vector3 RifleEuler = new Vector3(0f, -8f, 0f);
    public Vector3 RightGripOffset, RightGripEuler;
    public Vector3 LeftGripOffset = new Vector3(-.04695866f, .03f, -.04951340f);
    public Vector3 LeftGripEuler;

    internal WorldRifleBodyPoseSettings Freeze()
    {
        if (!BodyPoseMath.Finite(Forward) || !BodyPoseMath.Finite(Down)
            || !BodyPoseMath.Finite(Inward) || !BodyPoseMath.Finite(RifleEuler)
            || !BodyPoseMath.Finite(RightGripOffset) || !BodyPoseMath.Finite(RightGripEuler)
            || !BodyPoseMath.Finite(LeftGripOffset) || !BodyPoseMath.Finite(LeftGripEuler))
            throw new InvalidOperationException("RIFLE_SETTINGS_NONFINITE");
        return new WorldRifleBodyPoseSettings {
            Forward = Forward, Down = Down, Inward = Inward, RifleEuler = RifleEuler,
            RightGripOffset = RightGripOffset, RightGripEuler = RightGripEuler,
            LeftGripOffset = LeftGripOffset, LeftGripEuler = LeftGripEuler };
    }
}

// Frozen Blue/Red calibration from 5E20F605... actual02, not a live-pose sample.
// Both teams produced the same values. Offsets are in RiflePresentation space.
internal static class WorldRifleGripCalibration
{
    internal static readonly Vector3 RightPosition = new Vector3(1.49011612E-08f, -.0600002f, -1.75088644E-07f);
    internal static readonly Vector3 LeftPosition = new Vector3(-.06785662f, -4.632771E-05f, .280080646f);
    internal static readonly Quaternion RightRotation = new Quaternion(-.334325463f, -.84766686f, -.411030442f, .0272397026f);
    internal static readonly Quaternion LeftRotation = new Quaternion(-.0513451435f, -.9971108f, .0496897921f, -.0257952716f);
    // Render-only fit of the skinned fingers against the shared world-model mesh.
    internal static readonly Vector3 LeftRenderOffset = new Vector3(.020f, .010f, -.014f);
    internal static readonly Quaternion LeftRenderRotation = Quaternion.Euler(24f, -11f, 12f);
}

// Cosmetic fit for the separate rear mesh on the authored third-person rifle.
internal sealed class WorldRifleStockPose
{
    private const float Travel = .11f;
    private readonly Transform _frame, _parent, _part;
    private readonly Vector3 _position, _scale, _compressedScale, _frontOffset;
    internal bool Available => _part != null && _parent != null && _parent.parent == _frame && _part.parent == _parent;

    private WorldRifleStockPose(Transform frame, Transform part, Bounds bounds)
    {
        _frame = frame; _part = part; _parent = part.parent;
        _position = part.localPosition; _scale = part.localScale;
        // The model X axis points back along the rifle. Keep the front edge
        // fixed as the rear section shortens, including its original rotation.
        float ratio = 1f - Travel / (bounds.size.x * part.lossyScale.x);
        _compressedScale = new Vector3(_scale.x * ratio, _scale.y, _scale.z);
        _frontOffset = part.localRotation * Vector3.right * (bounds.min.x * (_scale.x - _compressedScale.x));
    }

    internal static WorldRifleStockPose TryBind(Transform frame)
    {
        var part = frame.Find("Model/Weapon_02_7");
        if (part == null || !part.TryGetComponent<MeshFilter>(out var filter) || filter.sharedMesh == null
            || Vector3.Dot(part.right, -frame.forward) < .999f) return null;
        var bounds = filter.sharedMesh.bounds;
        if (!BodyPoseMath.Finite(bounds.min) || !BodyPoseMath.Finite(bounds.size)
            || bounds.size.x * part.lossyScale.x <= Travel) return null;
        return new WorldRifleStockPose(frame, part, bounds);
    }

    internal void Apply(float weight)
    {
        if (!Available) return;
        _part.localPosition = _position + _frontOffset * weight;
        _part.localScale = Vector3.Lerp(_scale, _compressedScale, weight);
    }
}

internal sealed class WorldRifleFingerPose
{
    // Group by joint depth so each parent is posed before its children.
    private static readonly HumanBodyBones[] Joints = {
        HumanBodyBones.LeftIndexProximal, HumanBodyBones.LeftMiddleProximal,
        HumanBodyBones.LeftRingProximal, HumanBodyBones.LeftLittleProximal,
        HumanBodyBones.LeftIndexIntermediate, HumanBodyBones.LeftMiddleIntermediate,
        HumanBodyBones.LeftRingIntermediate, HumanBodyBones.LeftLittleIntermediate,
        HumanBodyBones.LeftIndexDistal, HumanBodyBones.LeftMiddleDistal,
        HumanBodyBones.LeftRingDistal, HumanBodyBones.LeftLittleDistal,
        HumanBodyBones.LeftThumbProximal
    };
    private readonly Transform[] _joints;
    private static readonly float[] CurlDegrees = { -8f, -19f, -16f };
    private static readonly Quaternion InverseRestHand = Quaternion.Inverse(WorldRifleGripCalibration.LeftRotation);

    private WorldRifleFingerPose(Transform[] joints) { _joints = joints; }

    internal static WorldRifleFingerPose TryBind(Animator source, Dictionary<Transform, Transform> visualBones)
    {
        var joints = new Transform[Joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            var bone = source.GetBoneTransform(Joints[i]);
            if (bone == null || !visualBones.TryGetValue(bone, out joints[i])) return null;
        }
        return new WorldRifleFingerPose(joints);
    }

    internal void Apply(Quaternion handRotation)
    {
        // BeginPose has restored the authored finger rotations for this pass.
        var basis = handRotation * InverseRestHand;
        var curlAxis = basis * Vector3.forward;
        for (int depth = 0; depth < CurlDegrees.Length; depth++)
        {
            var curl = Quaternion.AngleAxis(CurlDegrees[depth], curlAxis);
            for (int finger = 0; finger < 4; finger++)
            {
                var joint = _joints[depth * 4 + finger];
                joint.rotation = curl * joint.rotation;
            }
        }
        var thumb = _joints[Joints.Length - 1];
        thumb.rotation = Quaternion.AngleAxis(-41f, basis * Vector3.up) * thumb.rotation;
    }
}

internal static class BodyPoseMath
{
    internal static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    internal static bool Finite(Vector3 value) => Finite(value.x) && Finite(value.y) && Finite(value.z);
    internal static bool Finite(Quaternion value) => Finite(value.x) && Finite(value.y)
        && Finite(value.z) && Finite(value.w) && Quaternion.Dot(value, value) > .000001f;
    internal static bool Unit(Vector3 scale) => Finite(scale) && (scale - Vector3.one).sqrMagnitude <= .000001f;
}
