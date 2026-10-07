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

    public bool TryGetSpawnPose(int team, out Transform pose)
    {
        pose = null;
        var manager = NetworkRunnerManager.Instance;
        var runner = manager != null ? manager.GetNetworkRunner() : null;
        if (runner == null || !runner.IsServer || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game || (team != 0 && team != 1)) return false;
        var points = SpawnPoints.Instance;
        return points != null && _spawnSelector.TrySelect(team == 0 ? points.RedTeamSpawnPoints : points.BlueTeamSpawnPoints, runner.Tick.Raw, out pose);
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
