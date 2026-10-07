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
    public GamePlayerNetworkData Owner { get; private set; }
    private readonly CombatSequenceCursor _cursor = new CombatSequenceCursor();
    private CombatPresentationSnapshot _lastVitals;
    private bool _bound;

    public void Bind(GamePlayerNetworkData owner)
    {
        Unbind();
        if (owner == null || !owner.TryGetCombatSnapshot(out _)) return;
        Owner = owner;
        _bound = true;
        _cursor.Reset(owner.ShotSequence, owner.HitSequence);
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
            Owner.IsReloading, Owner.ReloadRemaining, Owner.RecoilOffset);
        return true;
    }

    public void RenderState()
    {
        if (!TryGetSnapshot(out var snapshot)) { Unbind(); return; }
        bool local = Owner.Object.HasInputAuthority;
        if (local && (snapshot.Health != _lastVitals.Health || snapshot.Ammo != _lastVitals.Ammo
            || snapshot.Capacity != _lastVitals.Capacity || snapshot.IsReloading != _lastVitals.IsReloading
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
        LocalVitalsChanged = null; ConfirmedHit = null; ShotObserved = null;
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
