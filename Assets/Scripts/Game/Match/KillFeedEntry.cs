using Fusion;

public struct KillFeedEntry : INetworkStruct
{
    public int Sequence, Tick, KillerTeam, VictimTeam, DeathSequence, ShotSequence;
    public PlayerRef Killer, Victim;
    public NetworkString<_32> KillerName, VictimName;
    public NetworkBool Headshot;
}

// Initial binding exposes retained history in MatchSnapshot but emits no old FX.
// After binding, emit all retained new entries and explicitly report lost ones.
public sealed class KillFeedCursor
{
    private int _highWater;
    public void Reset(int sequence) => _highWater = sequence;
    public bool TryConsume(int newest, out int first, out int gap)
    {
        first = gap = 0;
        if (newest <= _highWater) return false;
        int expected = _highWater + 1;
        first = System.Math.Max(expected, OldestRetained(newest));
        gap = first - expected;
        _highWater = newest;
        return true;
    }
    public static int OldestRetained(int newest) => System.Math.Max(1, newest - NetworkMatchState.FeedCapacity + 1);
    public static int Slot(int sequence) => (sequence - 1) % NetworkMatchState.FeedCapacity;
}

public readonly struct KillFeedObservation
{
    public readonly KillFeedEntry Entry;
    public readonly int GapBefore;
    public KillFeedObservation(KillFeedEntry entry, int gap) { Entry = entry; GapBefore = gap; }
}
