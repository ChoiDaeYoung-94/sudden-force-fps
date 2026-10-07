using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public sealed class NetworkMatchState : NetworkBehaviour
{
    public const int RosterCapacity = 16, FeedCapacity = 16, TargetScore = 20, DurationSeconds = 300;
    public static NetworkMatchState Instance { get; private set; }
    [Networked] public MatchPhase Phase { get; private set; }
    [Networked] public MatchResult Result { get; private set; }
    [Networked] public MatchEndReason EndReason { get; private set; }
    [Networked] public int RedScore { get; private set; }
    [Networked] public int BlueScore { get; private set; }
    [Networked] public TickTimer MatchTimer { get; private set; }
    [Networked] public int ResultVersion { get; private set; }
    [Networked] public int RosterCount { get; private set; }
    [Networked] public int RosterVersion { get; private set; }
    [Networked] public int KillFeedSequence { get; private set; }
    [Networked, Capacity(RosterCapacity)] public NetworkArray<MatchRosterEntry> Roster => default;
    [Networked, Capacity(FeedCapacity)] public NetworkArray<KillFeedEntry> KillFeed => default;
    private readonly Dictionary<PlayerRef, int> _deathHighWater = new Dictionary<PlayerRef, int>();
    private GameManager _game;
    private MatchPresentation _presentation;
    private bool _spawned;
    public bool IsCurrent => _spawned && Object != null && Object.IsValid && Runner != null && Runner.IsRunning
        && NetworkRunnerManager.Instance != null && !NetworkRunnerManager.Instance.IsRetired
        && NetworkRunnerManager.Instance.GetNetworkRunner() == Runner
        && NetworkRunnerManager.Instance.SessionPhase == NetworkSessionPhase.Game;
    public bool CanCombat => IsCurrent && AllowsCombat(Phase, Runner.Tick.Raw, MatchTimer.TargetTick);
    public static bool AllowsCombat(MatchPhase phase, int tick, int? endTick) => phase == MatchPhase.Running && endTick.HasValue && tick < endTick.Value;
    public static bool AllowsCombatFor(NetworkRunner runner) => Instance != null && Instance.IsCurrent && Instance.Runner == runner && Instance.CanCombat;
    public static MatchResult WinnerByScore(int red, int blue) => red == blue ? MatchResult.Draw : red > blue ? MatchResult.RedWin : MatchResult.BlueWin;
    public static bool TryAwardScore(int red, int blue, int killerTeam, out int nextRed, out int nextBlue, out MatchResult winner)
    {
        nextRed = red; nextBlue = blue; winner = MatchResult.None;
        if ((killerTeam != 0 && killerTeam != 1) || red < 0 || blue < 0 || red >= TargetScore || blue >= TargetScore) return false;
        if (killerTeam == 0) nextRed++; else nextBlue++;
        if (nextRed >= TargetScore) winner = MatchResult.RedWin;
        else if (nextBlue >= TargetScore) winner = MatchResult.BlueWin;
        return true;
    }

    public override void Spawned()
    {
        if (Instance != null && Instance != this && Instance.Object != null && Instance.Object.IsValid && Instance.Runner == Runner)
            throw new InvalidOperationException("Duplicate match state for this runner.");
        _spawned = true; Instance = this;
        _game = AD.Managers.Instance != null ? AD.Managers.GameM : null;
        if (_game != null) _game.BindMatch(this);
        if (Object.HasStateAuthority)
        {
            if (_game == null) throw new InvalidOperationException("Match requires the game roster service.");
            var roster = _game.GetInitialMatchRoster();
            if (roster.Length == 0 || roster.Length > RosterCapacity) throw new InvalidOperationException("Match roster must contain 1 to 16 participants.");
            Phase = MatchPhase.Waiting; Result = MatchResult.None; EndReason = MatchEndReason.None;
            RedScore = BlueScore = ResultVersion = KillFeedSequence = 0; MatchTimer = default;
            RosterCount = roster.Length; RosterVersion = 1;
            for (int i = 0; i < roster.Length; i++) Roster.Set(i, roster[i]);
            _game.KillConfirmed += OnKillConfirmed;
        }
        _presentation = GetComponent<MatchPresentation>();
        if (_presentation == null) _presentation = gameObject.AddComponent<MatchPresentation>();
        _presentation.Bind(this);
    }

    public override void FixedUpdateNetwork()
    {
        if (!IsCurrent || !Object.HasStateAuthority || Phase == MatchPhase.Finished) return;
        EvaluateBoundary();
        if (Phase != MatchPhase.Waiting) return;
        int red = 0, blue = 0;
        bool allSpawned = true;
        for (int i = 0; i < RosterCount; i++)
        {
            var entry = Roster[i];
            if (!entry.Connected) continue;
            if (entry.Team == 0) red++; else blue++;
            if (!_game.TryGetGamePlayer(entry.Player, out _)) allSpawned = false;
        }
        if (red == 0 || blue == 0) { Finish(MatchResult.Cancelled, MatchEndReason.MissingTeam); return; }
        if (allSpawned)
        {
            MatchTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(DurationSeconds, Runner.TickRate));
            Phase = MatchPhase.Running; _game.RespawnsEnabled = true;
        }
    }

    // Called from player simulation as well, so timeout cannot depend on callback order.
    public void EvaluateBoundary()
    {
        if (IsCurrent && Object.HasStateAuthority && Phase == MatchPhase.Running && MatchTimer.Expired(Runner))
            Finish(WinnerByScore(RedScore, BlueScore), MatchEndReason.TimeExpired);
    }

    private int Find(PlayerRef player)
    {
        for (int i = 0; i < RosterCount; i++) if (Roster[i].Player == player) return i;
        return -1;
    }
    private void CopyStats(int index)
    {
        var entry = Roster[index];
        if (!_game.TryGetGamePlayer(entry.Player, out var player)) return;
        if (entry.Kill == player.Kill && entry.Death == player.Death) return;
        entry.Kill = player.Kill; entry.Death = player.Death; Roster.Set(index, entry); RosterVersion++;
    }
    public void PlayerLeft(PlayerRef player)
    {
        if (!IsCurrent || !Object.HasStateAuthority || Phase == MatchPhase.Finished) return;
        EvaluateBoundary();
        if (Phase == MatchPhase.Finished) return;
        int index = Find(player); if (index < 0) return;
        CopyStats(index);
        var departed = Roster[index]; departed.Connected = false; Roster.Set(index, departed); RosterVersion++;
        int red = 0, blue = 0;
        for (int i = 0; i < RosterCount; i++)
            if (Roster[i].Connected) { if (Roster[i].Team == 0) red++; else blue++; }
        if (red != 0 && blue != 0) return;
        if (Phase == MatchPhase.Waiting) Finish(MatchResult.Cancelled, MatchEndReason.MissingTeam);
        else Finish(red == 0 ? MatchResult.BlueWin : MatchResult.RedWin, MatchEndReason.OpponentLeft);
    }

    private void OnKillConfirmed(KillConfirmedRecord record)
    {
        if (!Object.HasStateAuthority || !CanCombat) return;
        int killerIndex = Find(record.Killer), victimIndex = Find(record.Victim);
        if (killerIndex < 0 || victimIndex < 0 || killerIndex == victimIndex || record.DeathSequence <= 0) return;
        var killer = Roster[killerIndex]; var victim = Roster[victimIndex];
        if (!killer.Connected || !victim.Connected || killer.Team == victim.Team
            || killer.Team != record.KillerTeam || victim.Team != record.VictimTeam) return;
        if (!_game.TryGetGamePlayer(record.Victim, out var target) || !target.IsDead
            || target.DeathSequence != record.DeathSequence || target.LastKiller != record.Killer) return;
        if (_deathHighWater.TryGetValue(record.Victim, out int previous) && previous >= record.DeathSequence) return;
        if (!TryAwardScore(RedScore, BlueScore, killer.Team, out int red, out int blue, out var winner)) return;
        _deathHighWater[record.Victim] = record.DeathSequence;
        CopyStats(killerIndex); CopyStats(victimIndex);
        RedScore = red; BlueScore = blue;
        int sequence = ++KillFeedSequence;
        KillFeed.Set(KillFeedCursor.Slot(sequence), new KillFeedEntry {
            Sequence = sequence, Tick = Runner.Tick.Raw, Killer = record.Killer, Victim = record.Victim,
            KillerTeam = killer.Team, VictimTeam = victim.Team, KillerName = killer.Name, VictimName = victim.Name,
            Headshot = record.Headshot, DeathSequence = record.DeathSequence, ShotSequence = record.ShotSequence });
        if (winner != MatchResult.None) Finish(winner, MatchEndReason.TargetScore);
    }

    private void Finish(MatchResult result, MatchEndReason reason)
    {
        if (Phase == MatchPhase.Finished) return;
        for (int i = 0; i < RosterCount; i++) CopyStats(i);
        Result = result; EndReason = reason; ResultVersion++; Phase = MatchPhase.Finished;
        _game.RespawnsEnabled = false;
    }
    public bool TryGetSnapshot(out MatchSnapshot snapshot)
    {
        snapshot = default; if (!IsCurrent) return false;
        var roster = new MatchRosterEntry[RosterCount];
        for (int i = 0; i < roster.Length; i++) roster[i] = Roster[i];
        int count = Math.Min(FeedCapacity, KillFeedSequence);
        var feed = new KillFeedEntry[count];
        int first = KillFeedCursor.OldestRetained(KillFeedSequence);
        for (int i = 0; i < count; i++) feed[i] = KillFeed[KillFeedCursor.Slot(first + i)];
        float remaining = Phase == MatchPhase.Running ? MatchTimer.RemainingTime(Runner) ?? 0f : 0f;
        snapshot = new MatchSnapshot(Phase, Result, EndReason, RedScore, BlueScore, ResultVersion, KillFeedSequence, remaining, roster, feed);
        return true;
    }
    public override void Render()
    {
        if (_presentation == null || !_presentation.isActiveAndEnabled) return;
        if (_presentation.Owner == null) _presentation.Bind(this);
        _presentation.RenderState();
    }
    public override void Despawned(NetworkRunner runner, bool hasState) => Release();
    public void DetachFromSession() => Release();
    private void Release()
    {
        _spawned = false;
        if (_game != null) { _game.KillConfirmed -= OnKillConfirmed; _game.UnbindMatch(this); }
        if (_presentation != null) _presentation.Unbind();
        if (Instance == this) Instance = null;
    }
    private void OnDestroy() => Release();
}
