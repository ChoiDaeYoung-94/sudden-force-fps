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
