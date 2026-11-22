using System;
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
}

internal static class BodyPoseMath
{
    internal static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
    internal static bool Finite(Vector3 value) => Finite(value.x) && Finite(value.y) && Finite(value.z);
    internal static bool Finite(Quaternion value) => Finite(value.x) && Finite(value.y)
        && Finite(value.z) && Finite(value.w) && Quaternion.Dot(value, value) > .000001f;
    internal static bool Unit(Vector3 scale) => Finite(scale) && (scale - Vector3.one).sqrMagnitude <= .000001f;
}
