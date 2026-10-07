using System.Collections.Generic;

public enum MatchPhase { Waiting, Running, Finished }
public enum MatchResult { None, RedWin, BlueWin, Draw, Cancelled }
public enum MatchEndReason { None, TargetScore, TimeExpired, OpponentLeft, MissingTeam }

public readonly struct MatchSnapshot
{
    public readonly MatchPhase Phase;
    public readonly MatchResult Result;
    public readonly MatchEndReason EndReason;
    public readonly int RedScore, BlueScore, ResultVersion, KillFeedSequence;
    public readonly float Remaining;
    public readonly IReadOnlyList<MatchRosterEntry> Roster;
    public readonly IReadOnlyList<KillFeedEntry> KillFeed;
    public MatchSnapshot(MatchPhase phase, MatchResult result, MatchEndReason reason, int red, int blue,
        int version, int sequence, float remaining, MatchRosterEntry[] roster, KillFeedEntry[] feed)
    {
        Phase = phase; Result = result; EndReason = reason; RedScore = red; BlueScore = blue;
        ResultVersion = version; KillFeedSequence = sequence; Remaining = remaining;
        Roster = System.Array.AsReadOnly((MatchRosterEntry[])roster.Clone());
        KillFeed = System.Array.AsReadOnly((KillFeedEntry[])feed.Clone());
    }
}
