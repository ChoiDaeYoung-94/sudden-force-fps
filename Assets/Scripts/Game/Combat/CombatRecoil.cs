using UnityEngine;

public static class CombatRecoil
{
    public static Vector2 Recover(Vector2 offset, float degreesPerSecond, float deltaTime) =>
        Vector2.MoveTowards(offset, Vector2.zero, Mathf.Max(0f, degreesPerSecond) * Mathf.Max(0f, deltaTime));

    public static Quaternion ViewRotation(float basePitch, Vector2 offset) =>
        Quaternion.Euler(Mathf.Clamp(basePitch - offset.y, -85f, 85f), offset.x, 0f);

    public static Vector3 ShotDirection(float baseYaw, float basePitch, Vector2 offset) =>
        Quaternion.Euler(0f, baseYaw, 0f) * ViewRotation(basePitch, offset) * Vector3.forward;
}
