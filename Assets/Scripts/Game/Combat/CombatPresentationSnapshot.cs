using Fusion;
using UnityEngine;

public readonly struct CombatPresentationSnapshot
{
    public readonly PlayerRef Player;
    public readonly int Health, Ammo, Capacity;
    public readonly bool IsReloading;
    public readonly float ReloadRemaining;
    public readonly Vector2 RecoilOffset;
    public CombatPresentationSnapshot(PlayerRef player, int health, int ammo, int capacity, bool reloading, float remaining, Vector2 recoil)
    {
        Player = player; Health = health; Ammo = ammo; Capacity = capacity; IsReloading = reloading;
        ReloadRemaining = remaining; RecoilOffset = recoil;
    }
}

public readonly struct ShotObservedSnapshot
{
    public readonly PlayerRef Shooter;
    public readonly int Sequence, SequenceDelta, RecoilIndex;
    public readonly Vector3 Origin, Direction, HitPoint, HitNormal, VisualMuzzlePosition;
    public readonly Vector2 RecoilOffset;
    public readonly bool HasHit;
    public ShotObservedSnapshot(GamePlayerNetworkData player, int delta)
    {
        Shooter = player.Object.InputAuthority; Sequence = player.ShotSequence; SequenceDelta = delta;
        RecoilIndex = player.LastShotRecoilIndex; RecoilOffset = player.LastShotRecoilOffset;
        Origin = player.LastShotOrigin; Direction = player.LastShotDirection;
        HasHit = player.LastShotHit; HitPoint = player.LastShotHitPoint; HitNormal = player.LastShotHitNormal;
        var muzzle = player.Object.HasInputAuthority ? player.ViewMuzzle : player.WorldMuzzle;
        VisualMuzzlePosition = muzzle != null ? muzzle.position : Origin;
    }
}

public readonly struct ConfirmedHitSnapshot
{
    public readonly PlayerRef Shooter, Target;
    public readonly int Sequence, SequenceDelta, ShotSequence, Damage;
    public readonly CombatBodyPart BodyPart;
    public readonly Vector3 Point, Normal;
    public ConfirmedHitSnapshot(GamePlayerNetworkData player, int delta)
    {
        Shooter = player.Object.InputAuthority; Target = player.LastHitTarget;
        Sequence = player.HitSequence; SequenceDelta = delta; ShotSequence = player.LastHitShotSequence;
        Damage = player.LastHitDamage; BodyPart = player.LastHitBodyPart;
        Point = player.LastHitPoint; Normal = player.LastHitNormal;
    }
}

// Latest-state policy: emit once for a new sequence and expose a gap instead of
// inventing missing shot positions. A bind/rebind never replays historical FX.
public sealed class CombatSequenceCursor
{
    private int _shot, _hit;
    public void Reset(int shot, int hit) { _shot = shot; _hit = hit; }
    public bool ConsumeShot(int sequence, out int delta) => Consume(ref _shot, sequence, out delta);
    public bool ConsumeHit(int sequence, out int delta) => Consume(ref _hit, sequence, out delta);
    private static bool Consume(ref int previous, int sequence, out int delta)
    {
        delta = 0;
        if (sequence == previous) return false;
        // Keep the high-water mark across rollback/correction. Only explicit
        // Reset on bind/new runner may lower it, or an old shot would replay.
        if (sequence < previous) return false;
        delta = sequence - previous;
        previous = sequence;
        return true;
    }
}
