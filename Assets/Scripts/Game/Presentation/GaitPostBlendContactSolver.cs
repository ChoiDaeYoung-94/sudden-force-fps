using System;
using UnityEngine;

// C10/C11 pole-B and stable acute-direction math, stripped of diagnostic I/O only.
// Rotation-only visual transforms. No root offsets, stretching, clamp or second budget.
internal static class GaitPostBlendContactSolver
{
    internal static bool CanReach(Transform thigh, Transform calf, Transform foot, Vector3 target)
    {
        float a = Vector3.Distance(thigh.position, calf.position), b = Vector3.Distance(calf.position, foot.position);
        float d = Vector3.Distance(thigh.position, target);
        return a > .001f && b > .001f && Finite(d)
            && d > Mathf.Abs(a - b) + .000005f && d < a + b - .000005f;
    }
    internal static void Solve(Transform thigh, Transform calf, Transform foot,
        Vector3 target, Quaternion orientation, Vector3 fallbackForward)
    {
        var hip = thigh.position; var knee = calf.position; var ankle = foot.position;
        float a = Vector3.Distance(hip, knee), b = Vector3.Distance(knee, ankle), distance = Vector3.Distance(hip, target);
        Need(a > .001f && b > .001f && distance > Mathf.Abs(a - b) + .000005f && distance < a + b - .000005f,
            "UNREACHABLE_FOOT_TARGET_NO_ROOT_OFFSET_OR_CLAMP");
        var axis = (target - hip) / distance; var bend = Vector3.ProjectOnPlane(knee - hip, axis);
        var rawPoleVector = bend;
        if (bend.sqrMagnitude < .00000001f) bend = Vector3.ProjectOnPlane(fallbackForward, axis);
        Need(bend.sqrMagnitude > .00000001f, "DEFINED_KNEE_POLE_REQUIRED"); bend.Normalize();
        var basisPole = bend;
        var anatomicalPole = Vector3.ProjectOnPlane(fallbackForward, axis);
        Need(anatomicalPole.sqrMagnitude > .00000001f, "DEFINED_ANATOMICAL_POLE_REQUIRED"); anatomicalPole.Normalize();
        float rawPoleUpperRatio = rawPoleVector.magnitude / a;
        float anatomicalBlendWeight = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(.04f, .18f, rawPoleUpperRatio));
        var regularizedPole = Vector3.Lerp(basisPole, anatomicalPole, anatomicalBlendWeight);
        Need(regularizedPole.sqrMagnitude > .00000001f, "DEFINED_REGULARIZED_POLE_REQUIRED"); regularizedPole.Normalize();
        bend = regularizedPole; // C10/C11 fixed pole-B selection; not a new runtime toggle.
        float along = (a * a - b * b + distance * distance) / (2f * distance);
        float square = a * a - along * along; Need(square >= -.000001f, "VALID_TWO_BONE_TRIANGLE_REQUIRED");
        var desiredKnee = hip + axis * along + bend * Mathf.Sqrt(Mathf.Max(0f, square));
        var thighRotationBefore = thigh.rotation;
        var thighFrom = knee - hip; var thighTo = desiredKnee - hip;
        var thighNativeDelta = Quaternion.FromToRotation(thighFrom, thighTo);
        bool thighStableAcuteFormulaUsed;
        var thighAppliedDelta = StableAcuteDirectionDelta(thighFrom, thighTo, thighNativeDelta, out thighStableAcuteFormulaUsed);
        thigh.rotation = thighAppliedDelta * thighRotationBefore;
        var calfRotationBefore = calf.rotation;
        var calfFrom = foot.position - calf.position; var calfTo = target - calf.position;
        var calfNativeDelta = Quaternion.FromToRotation(calfFrom, calfTo);
        bool calfStableAcuteFormulaUsed;
        var calfAppliedDelta = StableAcuteDirectionDelta(calfFrom, calfTo, calfNativeDelta, out calfStableAcuteFormulaUsed);
        calf.rotation = calfAppliedDelta * calfRotationBefore;
        foot.rotation = orientation;
        Need(Vector3.Distance(foot.position, target) <= .0002f, "IK_TARGET_RESIDUAL_TOO_LARGE");
    }
    private static Quaternion StableAcuteDirectionDelta(Vector3 from, Vector3 to, Quaternion nativeDelta, out bool stableAcuteFormulaUsed)
    {
        double fromLength = Math.Sqrt((double)from.x * from.x + (double)from.y * from.y + (double)from.z * from.z);
        double toLength = Math.Sqrt((double)to.x * to.x + (double)to.y * to.y + (double)to.z * to.z);
        Need(fromLength > 0d && toLength > 0d && !double.IsInfinity(fromLength) && !double.IsInfinity(toLength), "FINITE_NONZERO_DIRECTION_DELTA_REQUIRED");
        double ux = from.x / fromLength, uy = from.y / fromLength, uz = from.z / fromLength;
        double vx = to.x / toLength, vy = to.y / toLength, vz = to.z / toLength;
        double dot = Math.Max(-1d, Math.Min(1d, ux * vx + uy * vy + uz * vz));
        stableAcuteFormulaUsed = dot >= 0d;
        if (!stableAcuteFormulaUsed) return nativeDelta;
        double x = uy * vz - uz * vy, y = uz * vx - ux * vz, z = ux * vy - uy * vx, w = 1d + dot;
        double inverseNorm = 1d / Math.Sqrt(x * x + y * y + z * z + w * w);
        var result = new Quaternion((float)(x * inverseNorm), (float)(y * inverseNorm), (float)(z * inverseNorm), (float)(w * inverseNorm));
        Need(Finite(result.x) && Finite(result.y) && Finite(result.z) && Finite(result.w), "FINITE_STABLE_DIRECTION_DELTA_REQUIRED");
        return result;
    }
    private static bool Finite(float v) => PlayerInputSource.IsFinite(v);
    private static void Need(bool ok, string code) { if (!ok) throw new InvalidOperationException(code); }
}
