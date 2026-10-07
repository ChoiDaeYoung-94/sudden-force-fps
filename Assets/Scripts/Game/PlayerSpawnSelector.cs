using System.Collections.Generic;
using UnityEngine;

// Same-tick reservations cover spawns not yet visible to the physics scene.
public sealed class PlayerSpawnSelector
{
    private readonly List<Vector3> _reserved = new List<Vector3>();
    private int _reservationTick = int.MinValue;
    private readonly Collider[] _overlaps = new Collider[64];
    public void Clear() { _reserved.Clear(); _reservationTick = int.MinValue; }

    public bool TrySelect(Transform[] points, int tick, out Transform pose, GamePlayerNetworkData ignoredPlayer = null)
    {
        pose = null;
        if (tick != _reservationTick) { _reserved.Clear(); _reservationTick = tick; }
        if (points == null || points.Length == 0) return false;
        Physics.SyncTransforms();
        int first = Random.Range(0, points.Length);
        for (int i = 0; i < points.Length; i++)
        {
            var candidate = points[(first + i) % points.Length];
            if (candidate == null || !IsClear(candidate.position, ignoredPlayer)) continue;
            pose = candidate;
            _reserved.Add(candidate.position);
            return true;
        }
        return false;
    }

    public bool IsClear(Vector3 position, GamePlayerNetworkData ignoredPlayer = null)
    {
        const float radius = 0.30f;
        const float height = 1.8f;
        foreach (var reserved in _reserved)
            if (Mathf.Abs(position.y - reserved.y) < height && new Vector2(position.x - reserved.x, position.z - reserved.z).sqrMagnitude < 4f * radius * radius)
                return false;
        var bottom = position + Vector3.up * (radius + 0.04f);
        var top = position + Vector3.up * (height - radius + 0.04f);
        int count = Physics.OverlapCapsuleNonAlloc(bottom, top, radius, _overlaps, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        // A saturated buffer cannot prove safety; never force a spawn through it.
        if (count >= _overlaps.Length) return false;
        for (int i = 0; i < count; i++)
        {
            if (!ShouldIgnoreCollider(_overlaps[i], ignoredPlayer)) return false;
        }
        return Physics.Raycast(position + Vector3.up * 0.15f, Vector3.down, out var ground, 0.35f, 1 << 8, QueryTriggerInteraction.Ignore)
            && Vector3.Angle(ground.normal, Vector3.up) <= 45f;
    }

    public static bool ShouldIgnoreCollider(Collider collider, GamePlayerNetworkData ignoredPlayer)
    {
        if (!(collider is CharacterController)) return false;
        var player = collider.GetComponent<GamePlayerNetworkData>();
        if (player == null) return false;
        if (player == ignoredPlayer) return true;
        return player.Object != null && player.Object.IsValid && player.IsDead;
    }
}
