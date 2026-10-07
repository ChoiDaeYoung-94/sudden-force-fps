using System;
using UnityEngine;

public sealed class MatchPresentation : MonoBehaviour
{
    public static MatchPresentation Instance { get; private set; }
    public static event Action<MatchPresentation> Bound;
    public static event Action<MatchPresentation> Unbound;
    public event Action<MatchSnapshot> Changed;
    public event Action<KillFeedObservation> KillObserved;
    public NetworkMatchState Owner { get; private set; }
    private readonly KillFeedCursor _cursor = new KillFeedCursor();
    private MatchSnapshot _last;
    public void Bind(NetworkMatchState owner)
    {
        Unbind();
        if (owner == null || !owner.TryGetSnapshot(out var snapshot)) return;
        if (Instance != null && Instance != this) Instance.Unbind();
        Owner = owner; Instance = this; _last = snapshot; _lastRosterVersion = owner.RosterVersion; _cursor.Reset(snapshot.KillFeedSequence);
        Notify(Bound, this); Notify(Changed, snapshot);
    }
    public bool TryGetSnapshot(out MatchSnapshot snapshot)
    {
        snapshot = default;
        return Owner != null && Owner.TryGetSnapshot(out snapshot);
    }
    public void RenderState()
    {
        if (!TryGetSnapshot(out var snapshot)) { Unbind(); return; }
        // Roster changes include disconnects that need not change the scores.
        if (Owner.RosterVersion != _lastRosterVersion || snapshot.Phase != _last.Phase || snapshot.ResultVersion != _last.ResultVersion
            || snapshot.RedScore != _last.RedScore || snapshot.BlueScore != _last.BlueScore
            || snapshot.KillFeedSequence != _last.KillFeedSequence || Mathf.Abs(snapshot.Remaining - _last.Remaining) >= .01f)
        {
            _last = snapshot; _lastRosterVersion = Owner.RosterVersion; Notify(Changed, snapshot);
        }
        if (_cursor.TryConsume(snapshot.KillFeedSequence, out int first, out int gap))
            foreach (var entry in snapshot.KillFeed)
                if (entry.Sequence >= first) { Notify(KillObserved, new KillFeedObservation(entry, gap)); gap = 0; }
    }
    private int _lastRosterVersion;
    public void Unbind()
    {
        if (Owner == null) return;
        Owner = null; if (Instance == this) Instance = null;
        Notify(Unbound, this); _cursor.Reset(0); Changed = null; KillObserved = null;
    }
    private static void Notify<T>(Action<T> handlers, T value)
    {
        if (handlers == null) return;
        foreach (Action<T> handler in handlers.GetInvocationList())
            try { handler(value); } catch (Exception exception) { Debug.LogException(exception); }
    }
    private void OnDisable() => Unbind();
    private void OnDestroy() => Unbind();
}
