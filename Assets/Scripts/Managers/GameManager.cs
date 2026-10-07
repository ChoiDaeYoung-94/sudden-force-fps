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
    [SerializeField] private NetworkObject _matchPrefab;
    public List<PlayerRef> Players = new List<PlayerRef>();
    public List<string> NickNames = new List<string>();
    public List<int> Teams = new List<int>();

    private readonly Dictionary<PlayerRef, GamePlayerRosterEntry> _roster = new Dictionary<PlayerRef, GamePlayerRosterEntry>();
    private readonly Dictionary<PlayerRef, NetworkObject> _spawned = new Dictionary<PlayerRef, NetworkObject>();
    private readonly PlayerSpawnSelector _spawnSelector = new PlayerSpawnSelector();
    private float _nextSpawnRetry;
    private readonly Dictionary<PlayerRef, GamePlayerRosterEntry> _initialRoster = new Dictionary<PlayerRef, GamePlayerRosterEntry>();
    private NetworkMatchState _match;
    private bool _matchSpawnStarted;
    private bool _gameSceneReady;
    public int RosterCount => _roster.Count;
    public event Action<KillConfirmedRecord> KillConfirmed;
    private readonly Dictionary<PlayerRef, int> _deathHighWater = new Dictionary<PlayerRef, int>();
    // C5 may additionally close this gate when a match ends.
    public bool RespawnsEnabled { get; set; }

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
        if (snapshot.Count > NetworkMatchState.RosterCapacity) throw new ArgumentException("Match supports at most 16 participants.");
        ClearSession();
        foreach (var entry in snapshot) { _roster.Add(entry.Key, entry.Value); _initialRoster.Add(entry.Key, entry.Value); }
        RefreshLegacyLists();
    }

    public void ClearSession()
    {
        if (_match != null) _match.DetachFromSession();
        _match = null; _matchSpawnStarted = false; _gameSceneReady = false; _initialRoster.Clear();
        _roster.Clear();
        _spawned.Clear();
        _spawnSelector.Clear();
        _deathHighWater.Clear();
        RespawnsEnabled = false;
        _nextSpawnRetry = 0f;
        RefreshLegacyLists();
    }

    public void RemovePlayer(NetworkRunner runner, PlayerRef player)
    {
        if (_match != null && _match.IsCurrent && _match.Runner == runner) _match.PlayerLeft(player);
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
        if (!_gameSceneReady || _match == null || !_match.IsCurrent || _match.Phase != MatchPhase.Waiting) return;
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
        if (!_gameSceneReady || _match == null || !_match.IsCurrent || _match.Phase == MatchPhase.Finished) return false;
        if (ignoredPlayer != null && (!RespawnsEnabled || ignoredPlayer.Runner != runner
            || !_match.CanCombat
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
            || !NetworkMatchState.AllowsCombatFor(runner)
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
        if (runner == null || !runner.IsServer || manager.SessionPhase != NetworkSessionPhase.Game
            || !_gameSceneReady || _match == null || !_match.IsCurrent || _match.Phase != MatchPhase.Waiting)
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

    public void EnsureMatchSpawn(NetworkRunner runner)
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager == null || manager.IsRetired || manager.GetNetworkRunner() != runner || !runner.IsServer
            || manager.SessionPhase != NetworkSessionPhase.Game) return;
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath("Assets/Scenes/Game/DesertHouse.unity");
        if (!scene.IsValid() || !scene.isLoaded) return;
        _gameSceneReady = true;
        if (_matchSpawnStarted) return;
        if (_matchPrefab == null || _matchPrefab.GetComponent<NetworkMatchState>() == null)
            throw new InvalidOperationException("A registered NetworkMatchState prefab is required.");
        _matchSpawnStarted = true;
        try
        {
            var spawned = runner.Spawn(_matchPrefab);
            if (spawned == null) throw new InvalidOperationException("Unable to spawn match state.");
            BindMatch(spawned.GetComponent<NetworkMatchState>());
        }
        catch { _matchSpawnStarted = false; throw; }
    }
    public void BindMatch(NetworkMatchState match)
    {
        var manager = NetworkRunnerManager.Instance;
        if (match == null || manager == null || manager.GetNetworkRunner() != match.Runner) return;
        if (_match != null && _match != match && _match.Object != null && _match.Object.IsValid && _match.Runner == match.Runner)
            throw new InvalidOperationException("Duplicate match state.");
        _match = match;
    }
    public void UnbindMatch(NetworkMatchState match) { if (_match == match) _match = null; }
    public bool TryGetGamePlayer(PlayerRef player, out GamePlayerNetworkData data)
    {
        data = null;
        if (!_spawned.TryGetValue(player, out var obj) || obj == null || !obj.IsValid) return false;
        data = obj.GetComponent<GamePlayerNetworkData>();
        return data != null;
    }
    public MatchRosterEntry[] GetInitialMatchRoster()
    {
        var manager = NetworkRunnerManager.Instance;
        var runner = manager != null ? manager.GetNetworkRunner() : null;
        if (runner == null || !runner.IsServer) throw new InvalidOperationException("Only the server initializes match roster.");
        var active = new HashSet<PlayerRef>(runner.ActivePlayers);
        return _initialRoster.Values.OrderBy(entry => entry.Player.RawEncoded).Select(entry => new MatchRosterEntry {
            Player = entry.Player, Name = LimitMatchName(entry.NickName), Team = entry.Team,
            Kill = 0, Death = 0, Connected = _roster.ContainsKey(entry.Player) && active.Contains(entry.Player) }).ToArray();
    }
    public static string LimitMatchName(string name)
    {
        if (string.IsNullOrEmpty(name)) return string.Empty;
        int count = Math.Min(32, name.Length);
        if (count < name.Length && char.IsHighSurrogate(name[count - 1])) count--;
        return name.Substring(0, count);
    }
}
