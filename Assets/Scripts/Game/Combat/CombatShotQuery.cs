using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public sealed class CombatShotQuery
{
    public const int ShotMask = (1 << 8) | (1 << 10); // WorldGeometry | PlayerHitbox; excludes CC layer 9.
    private readonly List<LagCompensatedHit> _hits = new List<LagCompensatedHit>(32);

    public bool TryCast(NetworkRunner runner, PlayerRef shooter, Vector3 origin, Vector3 direction, float range, out LagCompensatedHit hit)
    {
        hit = default;
        if (runner == null || !runner.IsServer || !runner.IsRunning || runner.LagCompensation == null
            || shooter == PlayerRef.None || !IsFinite(origin) || !IsFinite(direction)
            || !PlayerInputSource.IsFinite(range) || range <= 0f || direction.sqrMagnitude < 0.001f) return false;
        runner.LagCompensation.RaycastAll(origin, direction.normalized, range, shooter, _hits, ShotMask, true,
            HitOptions.IncludePhysX | HitOptions.SubtickAccuracy | HitOptions.IgnoreInputAuthority, QueryTriggerInteraction.Ignore);
        _hits.Sort(CompareHits);
        foreach (var candidate in _hits)
        {
            if (!PlayerInputSource.IsFinite(candidate.Distance) || candidate.Distance < 0f || candidate.Distance > range) continue;
            if (candidate.Collider != null)
            {
                if (candidate.Collider.isTrigger || candidate.Collider.gameObject.layer != 8) continue;
                hit = candidate;
                return true;
            }
            if (candidate.Hitbox != null && candidate.Hitbox.gameObject.layer == 10)
            {
                // The nearest hitbox still blocks a shot if its metadata/owner
                // is invalid. It must never let a shot pass through to someone else.
                hit = candidate;
                return true;
            }
        }
        return false;
    }

    public static int CompareHits(LagCompensatedHit left, LagCompensatedHit right)
    {
        int distance = left.Distance.CompareTo(right.Distance);
        if (distance != 0) return distance;
        // At an exact distance tie, static geometry wins over a player.
        return (left.Collider != null ? 0 : 1).CompareTo(right.Collider != null ? 0 : 1);
    }

    public static int DurationTicks(float duration, int tickRate)
    {
        if (!TryDurationTicks(duration, tickRate, out var ticks)) throw new ArgumentOutOfRangeException(nameof(duration));
        return ticks;
    }

    public static bool TryDurationTicks(float duration, int tickRate, out int ticks)
    {
        ticks = 0;
        if (!PlayerInputSource.IsFinite(duration) || duration <= 0f || tickRate <= 0) return false;
        double ceiling = Math.Ceiling((double)duration * tickRate);
        if (ceiling > int.MaxValue / 4) return false;
        ticks = Math.Max(1, (int)ceiling);
        return true;
    }

    public static bool IsFinite(Vector3 vector) => PlayerInputSource.IsFinite(vector.x)
        && PlayerInputSource.IsFinite(vector.y) && PlayerInputSource.IsFinite(vector.z);
}
