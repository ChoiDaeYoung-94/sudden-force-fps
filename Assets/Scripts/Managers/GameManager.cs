using Fusion;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    // Keep the prefab's NetworkedBehaviours reference intact. This local service
    // never uses inherited Runner/Object or networked state before registration.
    [SerializeField] private GameObject[] _gamePlayerObject;
    public List<PlayerRef> Players = new List<PlayerRef>();
    public List<string> NickNames = new List<string>();
    public List<int> Teams = new List<int>();

    private readonly Dictionary<PlayerRef, GamePlayerRosterEntry> _roster = new Dictionary<PlayerRef, GamePlayerRosterEntry>();
    private readonly Dictionary<PlayerRef, NetworkObject> _spawned = new Dictionary<PlayerRef, NetworkObject>();
    private readonly PlayerSpawnSelector _spawnSelector = new PlayerSpawnSelector();
    private float _nextSpawnRetry;
    public int RosterCount => _roster.Count;
    public event Action<KillConfirmedRecord> KillConfirmed;
    private readonly Dictionary<PlayerRef, int> _deathHighWater = new Dictionary<PlayerRef, int>();
    // C5 may additionally close this gate when a match ends.
    public bool RespawnsEnabled { get; set; } = true;

    public void SetRoster(IEnumerable<GamePlayerRosterEntry> players)
    {
        var snapshot = new Dictionary<PlayerRef, GamePlayerRosterEntry>();
        foreach (var entry in players)
        {
            if (entry.Player == PlayerRef.None || (entry.Team != 0 && entry.Team != 1) || snapshot.ContainsKey(entry.Player))
            {
                throw new ArgumentException("Invalid or duplicate game player roster entry.");
            }
            snapshot.Add(entry.Player, entry);
        }
        ClearSession();
        foreach (var entry in snapshot) _roster.Add(entry.Key, entry.Value);
        RefreshLegacyLists();
    }

    public void ClearSession()
    {
        _roster.Clear();
        _spawned.Clear();
        _spawnSelector.Clear();
        _deathHighWater.Clear();
        RespawnsEnabled = true;
        _nextSpawnRetry = 0f;
        RefreshLegacyLists();
    }

    public void RemovePlayer(NetworkRunner runner, PlayerRef player)
    {
        _roster.Remove(player);
        if (_spawned.TryGetValue(player, out var spawned))
        {
            _spawned.Remove(player);
            if (spawned != null && runner.IsServer)
            {
                runner.Despawn(spawned);
            }
        }
        RefreshLegacyLists();
    }

    private void RefreshLegacyLists()
    {
        Players.Clear();
        NickNames.Clear();
        Teams.Clear();
        foreach (var entry in _roster.Values)
        {
            Players.Add(entry.Player);
            NickNames.Add(entry.NickName);
            Teams.Add(entry.Team);
        }
    }

    public void Init()
    {
        SpawnGamePlayer();
    }

    private void Update()
    {
        if (_spawned.Count >= _roster.Count || Time.unscaledTime < _nextSpawnRetry) return;
        _nextSpawnRetry = Time.unscaledTime + 0.25f;
        SpawnGamePlayer();
    }

    public bool TryGetSpawnPose(int team, out Transform pose, GamePlayerNetworkData ignoredPlayer = null)
    {
        pose = null;
        var manager = NetworkRunnerManager.Instance;
        var runner = manager != null ? manager.GetNetworkRunner() : null;
        if (runner == null || !runner.IsServer || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game || (team != 0 && team != 1)) return false;
        if (ignoredPlayer != null && (!RespawnsEnabled || ignoredPlayer.Runner != runner
            || !_roster.ContainsKey(ignoredPlayer.Object.InputAuthority)
            || !runner.ActivePlayers.Contains(ignoredPlayer.Object.InputAuthority))) return false;
        var points = SpawnPoints.Instance;
        return points != null && _spawnSelector.TrySelect(team == 0 ? points.RedTeamSpawnPoints : points.BlueTeamSpawnPoints, runner.Tick.Raw, out pose, ignoredPlayer);
    }

    public void NotifyKillConfirmed(KillConfirmedRecord record)
    {
        var manager = NetworkRunnerManager.Instance;
        var runner = manager != null ? manager.GetNetworkRunner() : null;
        if (runner == null || !runner.IsServer || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game
            || record.DeathSequence <= 0 || record.Killer == record.Victim
            || !_roster.ContainsKey(record.Killer) || !_roster.ContainsKey(record.Victim)) return;
        if (_deathHighWater.TryGetValue(record.Victim, out int previous) && previous >= record.DeathSequence) return;
        _deathHighWater[record.Victim] = record.DeathSequence;
        if (KillConfirmed == null) return;
        foreach (Action<KillConfirmedRecord> handler in KillConfirmed.GetInvocationList())
        {
            try { handler(record); }
            catch (Exception exception) { Debug.LogException(exception); }
        }
    }

    public void SpawnGamePlayer()
    {
        var manager = NetworkRunnerManager.Instance;
        var runner = manager != null ? manager.GetNetworkRunner() : null;
        if (runner == null || !runner.IsServer || manager.SessionPhase != NetworkSessionPhase.Game)
        {
            return;
        }
        var activePlayers = new HashSet<PlayerRef>(runner.ActivePlayers);
        foreach (var entry in _roster.Values.ToArray())
        {
            if (_spawned.ContainsKey(entry.Player) || !activePlayers.Contains(entry.Player))
            {
                continue;
            }
            if (_gamePlayerObject == null || entry.Team >= _gamePlayerObject.Length || _gamePlayerObject[entry.Team] == null)
            {
                throw new InvalidOperationException("A team player prefab is missing.");
            }
            if (!TryGetSpawnPose(entry.Team, out var pose)) continue;
            var spawned = manager.SpawnGamePlayer(_gamePlayerObject[entry.Team], entry.NickName, entry.Team, entry.Player, pose);
            if (spawned != null) _spawned.Add(entry.Player, spawned);
        }
    }
}
