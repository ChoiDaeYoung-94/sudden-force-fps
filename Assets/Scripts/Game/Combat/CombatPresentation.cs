using System;
using UnityEngine;

// Render-only observer. HUD/VFX adapters subscribe after binding; simulation
// never invokes these callbacks and subscribers cannot mutate the copied data.
public sealed class CombatPresentation : MonoBehaviour
{
    public static CombatPresentation LocalInstance { get; private set; }
    public static event Action<CombatPresentation> PresentationBound;
    public static event Action<CombatPresentation> PresentationUnbound;
    public static event Action<CombatPresentation> LocalBound;
    public static event Action<CombatPresentation> LocalUnbound;
    public event Action<CombatPresentationSnapshot> LocalVitalsChanged;
    public event Action<ConfirmedHitSnapshot> ConfirmedHit;
    public event Action<ShotObservedSnapshot> ShotObserved;
    public event Action<LocalShotSnapshot> LocalShot;
    public GamePlayerNetworkData Owner { get; private set; }
    private readonly CombatSequenceCursor _cursor = new CombatSequenceCursor();
    private CombatPresentationSnapshot _lastVitals;
    private bool _bound;
    private int _localShotLife, _lastLocalShotTick;
    private bool _hasLocalShotTick;
    private bool _hasLocalShot;
    private LocalShotSnapshot _pendingLocalShot;

    public void Bind(GamePlayerNetworkData owner)
    {
        Unbind();
        if (owner == null || !owner.TryGetCombatSnapshot(out _)) return;
        Owner = owner;
        _bound = true;
        _cursor.Reset(owner.ShotSequence, owner.HitSequence);
        ResetLocalShots(owner.RespawnVersion);
        Notify(PresentationBound, this);
        if (owner.Object.HasInputAuthority)
        {
            if (LocalInstance != null && LocalInstance != this) LocalInstance.Unbind();
            LocalInstance = this;
            Notify(LocalBound, this);
            if (TryGetSnapshot(out var initial)) { _lastVitals = initial; Notify(LocalVitalsChanged, initial); }
        }
    }

    public bool TryGetSnapshot(out CombatPresentationSnapshot snapshot)
    {
        snapshot = default;
        if (!_bound || Owner == null || !Owner.TryGetCombatSnapshot(out var state)) return false;
        snapshot = new CombatPresentationSnapshot(state.InputAuthority, state.Health, state.Ammo, Owner.MagazineCapacity,
            Owner.IsReloading, Owner.ReloadRemaining, Owner.RecoilOffset, Owner.IsDead, Owner.RespawnRemaining,
            Owner.RespawnPending, Owner.RespawnVersion, Owner.Kill, Owner.Death);
        return true;
    }

    // Called by simulation, but never invokes render subscribers. Latest-state
    // coalescing avoids a catch-up burst after a stalled frame, matching remote FX.
    public void QueueLocalShot(int life, int tick, Vector3 origin, Vector3 direction)
    {
        if (!_bound || Owner == null || Owner.Object == null || !Owner.Object.IsValid
            || !Owner.Object.HasInputAuthority || Owner.Runner == null || !Owner.Runner.IsForward
            || life != Owner.RespawnVersion || !CombatShotQuery.IsFinite(origin)
            || !CombatShotQuery.IsFinite(direction)) return;
        var weapon = Owner.WeaponDefinition;
        if (weapon == null || !CombatShotQuery.TryDurationTicks(weapon.ShotInterval,
            Owner.Runner.TickRate, out var shotIntervalTicks)) return;
        if (_localShotLife != life) ResetLocalShots(life);
        // Reconciliation can rewind the predicted recoil timer and accept another
        // forward tick for an already-presented cadence slot. Keep this cosmetic
        // high-water independent of rollback. Rejected ticks never advance it.
        if (_hasLocalShotTick && (long)tick - _lastLocalShotTick < shotIntervalTicks) return;
        _lastLocalShotTick = tick;
        _hasLocalShotTick = true;
        _pendingLocalShot = new LocalShotSnapshot(Owner.Object.InputAuthority, life, tick,
            origin, direction, _hasLocalShot ? _pendingLocalShot.SequenceDelta + 1 : 1);
        _hasLocalShot = true;
    }

    private void ResetLocalShots(int life)
    {
        _localShotLife = life;
        _lastLocalShotTick = 0;
        _hasLocalShotTick = false;
        _hasLocalShot = false;
        _pendingLocalShot = default;
    }

    public void RenderState()
    {
        if (!TryGetSnapshot(out var snapshot)) { Unbind(); return; }
        bool local = Owner.Object.HasInputAuthority;
        if (_localShotLife != snapshot.RespawnVersion) ResetLocalShots(snapshot.RespawnVersion);
        if (_hasLocalShot)
        {
            var shot = _pendingLocalShot;
            _hasLocalShot = false;
            // This shot already passed simulation acceptance. The same tick can
            // end the match or kill the shooter; neither cancels accepted FX.
            // Binding/life validity still prevents replay into another life.
            if (local && shot.RespawnVersion == snapshot.RespawnVersion)
                Notify(LocalShot, shot);
        }
        if (local && (snapshot.Health != _lastVitals.Health || snapshot.Ammo != _lastVitals.Ammo
            || snapshot.Capacity != _lastVitals.Capacity || snapshot.IsReloading != _lastVitals.IsReloading
            || snapshot.IsDead != _lastVitals.IsDead || snapshot.RespawnPending != _lastVitals.RespawnPending
            || snapshot.RespawnVersion != _lastVitals.RespawnVersion || snapshot.Kill != _lastVitals.Kill || snapshot.Death != _lastVitals.Death
            || Mathf.Abs(snapshot.RespawnRemaining - _lastVitals.RespawnRemaining) >= 0.01f
            || Mathf.Abs(snapshot.ReloadRemaining - _lastVitals.ReloadRemaining) >= 0.01f))
        {
            _lastVitals = snapshot;
            Notify(LocalVitalsChanged, snapshot);
        }
        if (_cursor.ConsumeShot(Owner.ShotSequence, out var shotDelta))
            Notify(ShotObserved, new ShotObservedSnapshot(Owner, shotDelta));
        if (_cursor.ConsumeHit(Owner.HitSequence, out var hitDelta) && local)
            Notify(ConfirmedHit, new ConfirmedHitSnapshot(Owner, hitDelta));
    }

    public void Unbind()
    {
        if (!_bound) return;
        _bound = false;
        if (LocalInstance == this) { LocalInstance = null; Notify(LocalUnbound, this); }
        Notify(PresentationUnbound, this);
        Owner = null;
        _cursor.Reset(0, 0);
        ResetLocalShots(0);
        LocalVitalsChanged = null; ConfirmedHit = null; ShotObserved = null; LocalShot = null;
    }

    private static void Notify<T>(Action<T> handlers, T value)
    {
        if (handlers == null) return;
        foreach (Action<T> handler in handlers.GetInvocationList())
        {
            try { handler(value); }
            catch (Exception exception) { Debug.LogException(exception); }
        }
    }
    private void OnDisable() => Unbind();
    private void OnDestroy() => Unbind();
}
