using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerManager : MonoBehaviour, INetworkRunnerCallbacks
{
    private static NetworkRunnerManager _instance;
    public static NetworkRunnerManager Instance { get { return _instance; } }

    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private NetworkSceneManagerDefault _networkSceneM;

    private List<SessionInfo> _sessionList = new List<SessionInfo>();
    private RoomOptions _roomOptions = new RoomOptions();

    private const string _roomNameMessage = "This room already exists...";

    public string _nickName { get; set; }

    public LobbyConnectionStatus LobbyStatus { get; private set; } = LobbyConnectionStatus.Disconnected;
    public ShutdownReason? LastLobbyShutdownReason { get; private set; }
    public StartGameResult LastLobbyResult { get; private set; }
    public string LastLobbyError { get; private set; } = string.Empty;
    public bool HasReceivedSessionList { get; private set; }
    public int SessionCount => _sessionList.Count;
    public event Action<NetworkRunnerManager> LobbyStatusChanged;

    private Task _lobbyJoinTask;
    private Task _roomOperationTask;
    private Task _returnTask;
    private bool _gameTransitionStarted;
    private bool _roomSceneReady;
    private CancellationTokenSource _connectionCancellation;
    private CancellationToken _connectionToken;
    private bool _applicationStopping;
    private bool _shutdownObserved;
    private int _peerDisconnectFrame = -1;
    // These guards protect this Runner's operations. Lobby recovery has a
    // separate application lifetime owned by persistent AD.Managers.
    private bool HasLiveOwner => this != null && _instance == this && Application.isPlaying && !_applicationStopping;
    private bool CanConnect => HasLiveOwner && !IsRetired && !_connectionToken.IsCancellationRequested;
    private bool IsCurrentCallback(NetworkRunner runner) => HasLiveOwner && runner != null && runner == _networkRunner;
    private readonly HashSet<PlayerRef> _roomSpawnedPlayers = new HashSet<PlayerRef>();
    public NetworkSessionPhase SessionPhase { get; private set; } = NetworkSessionPhase.Lobby;
    public bool IsRetired { get; private set; }
    public bool IsRoomOperationPending => _roomOperationTask != null && !_roomOperationTask.IsCompleted;
    public string LastRoomError { get; private set; } = string.Empty;
    public ShutdownReason? LastRoomShutdownReason { get; private set; }
    public event Action<NetworkRunnerManager> RoomOperationChanged;

    private const string RoomScenePath = "Assets/Scenes/Room.unity";
    private const string GameScenePath = "Assets/Scenes/Game/DesertHouse.unity";

    private void Awake()
    {
        if (!Application.isPlaying) { _applicationStopping = true; return; }
        if (_instance != null && _instance != this && !_instance.IsRetired)
        {
            Destroy(gameObject);
            return;
        }
        if (AD.Managers.Instance != null && !AD.Managers.Instance.RegisterNetworkRunner(this))
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _connectionCancellation = new CancellationTokenSource();
        _connectionToken = _connectionCancellation.Token;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += OnEditorPlayModeChanged;
#endif
        _nickName = AD.Managers.SavedNickName;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void OnDestroy()
    {
        StopApplicationLifetime();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged -= OnEditorPlayModeChanged;
#endif
        var cancellation = _connectionCancellation;
        _connectionCancellation = null;
        if (cancellation != null)
            _ = DisposeAfterConnectionTasksAsync(cancellation, _lobbyJoinTask, _roomOperationTask);
        if (_instance == this) _instance = null;
    }

    private void OnApplicationQuit() => StopApplicationLifetime();
#if UNITY_EDITOR
    private void OnEditorPlayModeChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode) StopApplicationLifetime();
    }
#endif
    private void StopApplicationLifetime()
    {
        if (_applicationStopping) return;
        _applicationStopping = true;
        IsRetired = true;
        CancelConnectionOperations();
        if (_networkRunner != null) _networkRunner.RemoveCallbacks(this);
    }
    private void CancelConnectionOperations()
    {
        if (_connectionCancellation == null || _connectionToken.IsCancellationRequested) return;
        try { _connectionCancellation.Cancel(); }
        catch (AggregateException exception) { Debug.LogError($"[Network] Cancellation callback exception: {exception.GetType().Name}"); }
    }
    private static async Task DisposeAfterConnectionTasksAsync(CancellationTokenSource cancellation, Task lobby, Task room)
    {
        // No Unity access here: wait for all consumers before disposing the CTS.
        try { await Task.WhenAll(lobby ?? Task.CompletedTask, room ?? Task.CompletedTask).ConfigureAwait(false); }
        catch { /* Business tasks already report live failures; observe teardown faults. */ }
        finally { cancellation.Dispose(); }
    }

    #region Functions

    private void Init()
    {
        if (_networkRunner != null)
        {
            _networkRunner.AddCallbacks(this);
        }

        JoinSessionLobby();
    }

    #region Photon Fusion
    public void RoomSceneSpawn(GameObject prefab, PlayerRef player)
    {
        if (!CanConnect || !_roomSceneReady || _networkRunner == null || !_networkRunner.IsServer || !_roomSpawnedPlayers.Add(player)) return;
        try
        {
            var spawned = _networkRunner.Spawn(
                prefab, Vector3.zero, Quaternion.identity, player,
                onBeforeSpawned: (runner, spawnedObj) =>
                {
                    Transform teamPosition = RoomManager.Instance.GetTeamPosition();
                    if (teamPosition == null) throw new InvalidOperationException("A room team panel is missing.");
                    int team = teamPosition == RoomManager.Instance.RedTeam ? 0 : 1;
                    spawnedObj.transform.SetParent(teamPosition, worldPositionStays: false);
                    var data = spawnedObj.GetComponent<RoomPlayerNetworkData>();
                    data.Team = team;
                    data.IsReady = player == runner.LocalPlayer;
                });
            _networkRunner.SetPlayerObject(player, spawned);
        }
        catch
        {
            _roomSpawnedPlayers.Remove(player);
            throw;
        }
    }

    public void GameSceneSpawn(GameObject prefab, string nickName, int team, PlayerRef player)
    {
        SpawnGamePlayer(prefab, nickName, team, player);
    }

    public NetworkObject SpawnGamePlayer(GameObject prefab, string nickName, int team, PlayerRef player, Transform spawnPose = null)
    {
        if (!CanConnect || _networkRunner == null || !_networkRunner.IsServer || SessionPhase != NetworkSessionPhase.Game)
            throw new InvalidOperationException("Only the game server can spawn players.");
        if (team != 0 && team != 1) throw new ArgumentOutOfRangeException(nameof(team));
        // Legacy callers also use safe selection; blocked candidates stay pending.
        if (spawnPose == null && (AD.Managers.GameM == null || !AD.Managers.GameM.TryGetSpawnPose(team, out spawnPose))) return null;
        var pose = spawnPose;
        var spawned = _networkRunner.Spawn(
            prefab,
            pose.position,
            pose.rotation,
            player,
            onBeforeSpawned: (runner, spawnedObj) =>
            {
                GamePlayerNetworkData gamePlayerNetworkData = spawnedObj.GetComponent<GamePlayerNetworkData>();
                gamePlayerNetworkData.NickName = nickName;
                gamePlayerNetworkData.Team = team;
            }
            );
        _networkRunner.SetPlayerObject(player, spawned);
        return spawned;
    }

    public void DeSpawn(NetworkObject player)
    {
        if (CanConnect && _networkRunner != null && _networkRunner.IsServer && player != null) _networkRunner.Despawn(player);
    }

    public void JoinSessionLobby()
    {
        _ = JoinSessionLobbyAsync();
    }

    public Task JoinSessionLobbyAsync()
    {
        if (!CanConnect || SessionPhase != NetworkSessionPhase.Lobby) return Task.CompletedTask;
        if (_lobbyJoinTask != null && !_lobbyJoinTask.IsCompleted)
        {
            return _lobbyJoinTask;
        }

        if (LobbyStatus == LobbyConnectionStatus.Connected && _networkRunner != null && _networkRunner.LobbyInfo.IsValid)
        {
            return Task.CompletedTask;
        }

        _lobbyJoinTask = JoinSessionLobbyCoreAsync();
        return _lobbyJoinTask;
    }

    private async Task JoinSessionLobbyCoreAsync()
    {
        // Assign the shared task before publishing events or entering the SDK.
        await Task.Yield();

        if (!CanConnect)
        {
            return;
        }

        var popupManager = AD.Managers.Instance != null ? AD.Managers.PopupM : null;
        try
        {
            LastLobbyResult = null;
            LastLobbyShutdownReason = null;
            LastLobbyError = string.Empty;
            HasReceivedSessionList = false;
            _sessionList.Clear();
            LobbyStatus = LobbyConnectionStatus.Connecting;
            NotifyLobbyStatusChanged();
            if (!CanConnect) return;

            if (popupManager != null)
            {
                popupManager.PopupLoading();
            }

            if (_networkRunner == null)
            {
                throw new InvalidOperationException("The lobby NetworkRunner is missing.");
            }

            _networkRunner.ProvideInput = false;
            var result = await _networkRunner.JoinSessionLobby(SessionLobby.ClientServer, cancellationToken: _connectionToken);
            if (!CanConnect)
            {
                return;
            }

            LastLobbyResult = result;
            LastLobbyShutdownReason = result.ShutdownReason;
            LobbyStatus = result.Ok ? LobbyConnectionStatus.Connected : LobbyConnectionStatus.Failed;
            LastLobbyError = result.Ok ? string.Empty : result.ShutdownReason.ToString();
            if (!result.Ok)
            {
                HasReceivedSessionList = false;
                _sessionList.Clear();
            }
            NotifyLobbyStatusChanged();
            if (!CanConnect) return;

            if (result.Ok)
            {
                Debug.Log($"[Lobby] Connected. Region: {_networkRunner.LobbyInfo.Region}");
                if (string.IsNullOrEmpty(_nickName) && popupManager != null)
                {
                    popupManager.PopupSetNickName();
                }
            }
            else
            {
                Debug.LogError($"[Lobby] Join failed: {result.ShutdownReason}");
            }
        }
        catch (OperationCanceledException) when (_connectionToken.IsCancellationRequested) { }
        catch (Exception exception)
        {
            if (CanConnect)
            {
                LobbyStatus = LobbyConnectionStatus.Failed;
                LastLobbyError = exception.Message;
                HasReceivedSessionList = false;
                _sessionList.Clear();
                NotifyLobbyStatusChanged();
                Debug.LogError($"[Lobby] Join exception: {exception.GetType().Name}");
            }
        }
        finally
        {
            if (CanConnect && popupManager != null)
            {
                popupManager.ClosePopupLoading();
            }
        }
    }

    private void NotifyLobbyStatusChanged()
    {
        if (!CanConnect) return;
        if (LobbyStatusChanged == null)
        {
            return;
        }

        foreach (Action<NetworkRunnerManager> subscriber in LobbyStatusChanged.GetInvocationList())
        {
            if (!CanConnect) break;
            try
            {
                subscriber(this);
            }
            catch (Exception exception)
            {
                Debug.LogError($"[Lobby] Status listener exception: {exception.GetType().Name}");
            }
        }
    }

    public void CreateRoom(object value)
    {
        _ = CreateRoomAsync(value);
    }

    public Task CreateRoomAsync(object value)
    {
        if (IsRoomOperationPending) return _roomOperationTask;
        if (!CanConnect || SessionPhase != NetworkSessionPhase.Lobby) return Task.CompletedTask;
        try
        {
            var values = value as Dictionary<string, object>;
            if (values == null || !values.TryGetValue("RoomName", out var nameValue)
                || !values.TryGetValue("MapName", out var mapValue)
                || !values.TryGetValue("MaxPlayers", out var maxValue)
                || !values.TryGetValue("IsPrivateRoom", out var visibleValue))
                throw new ArgumentException("Room settings are incomplete.");
            string name = nameValue?.ToString().Trim();
            string map = mapValue?.ToString();
            if (string.IsNullOrWhiteSpace(name) || name.Length > 64 || map != "DesertHouse"
                || !int.TryParse(maxValue?.ToString(), out int max) || max < 2 || max > 8 || max % 2 != 0
                || !bool.TryParse(visibleValue?.ToString(), out bool visible))
                throw new ArgumentException("Use a room name, DesertHouse, and an even capacity from 2 to 8.");
            if (_sessionList.Any(s => s.Name == name)) throw new ArgumentException(_roomNameMessage);
            var options = MakeRoomOptions(name, map, max, true);
            _roomOperationTask = StartRoomAsync(options, GameMode.Host, visible);
            return _roomOperationTask;
        }
        catch (Exception exception)
        {
            ReportRoomError(exception.Message);
            return Task.CompletedTask;
        }
    }

    public void JoinRoom(SessionInfo sessionInfo)
    {
        _ = JoinRoomAsync(sessionInfo);
    }

    public Task JoinRoomAsync(SessionInfo sessionInfo)
    {
        if (IsRoomOperationPending) return _roomOperationTask;
        if (!CanConnect || SessionPhase != NetworkSessionPhase.Lobby) return Task.CompletedTask;
        if (sessionInfo == null || !sessionInfo.IsValid || !sessionInfo.IsOpen || sessionInfo.PlayerCount >= sessionInfo.MaxPlayers
            || !sessionInfo.Properties.TryGetValue("MapName", out var map) || (string)map != "DesertHouse")
        {
            ReportRoomError("This room is unavailable.");
            return Task.CompletedTask;
        }
        _roomOperationTask = StartRoomAsync(MakeRoomOptions(sessionInfo.Name, (string)map, sessionInfo.MaxPlayers, false), GameMode.Client, true);
        return _roomOperationTask;
    }

    private static RoomOptions MakeRoomOptions(string name, string map, int max, bool server)
    {
        return new RoomOptions { RoomName = name, MapName = map, PlayerCount = max / 2, Players = $"{max / 2} vs {max / 2}", IsServer = server };
    }

    private static SceneRef ResolveScene(string path)
    {
        int index = SceneUtility.GetBuildIndexByScenePath(path);
        if (index < 0) throw new InvalidOperationException($"The scene is not enabled in Build Settings: {path}");
        return SceneRef.FromIndex(index);
    }

    private async Task StartRoomAsync(RoomOptions options, GameMode mode, bool visible)
    {
        await Task.Yield();
        if (!CanConnect) return;
        var popup = AD.Managers.Instance != null ? AD.Managers.PopupM : null;
        bool failed = false;
        try
        {
            if (!CanConnect) return;
            LastRoomError = string.Empty;
            LastRoomShutdownReason = null;
            await JoinSessionLobbyAsync();
            if (!CanConnect) return;
            if (LobbyStatus != LobbyConnectionStatus.Connected)
                throw new InvalidOperationException("Connect to the lobby before joining a room.");
            var scene = ResolveScene(RoomScenePath);
            // Scene and player callbacks may run before StartGame completes.
            _roomOptions = options;
            _roomSceneReady = false;
            _roomSpawnedPlayers.Clear();
            SessionPhase = NetworkSessionPhase.JoiningRoom;
            NotifyRoomOperationChanged();
            if (!CanConnect) return;
            if (popup != null) popup.PopupLoading();
            var result = await _networkRunner.StartGame(new StartGameArgs {
                GameMode = mode, SessionName = options.RoomName,
                PlayerCount = mode == GameMode.Host ? options.PlayerCount * 2 : (int?)null,
                SessionProperties = mode == GameMode.Host ? new Dictionary<string, SessionProperty> { ["MapName"] = options.MapName } : null,
                IsVisible = mode == GameMode.Host ? visible : (bool?)null,
                Scene = scene, SceneManager = _networkSceneM, StartGameCancellationToken = _connectionToken
            });
            if (!CanConnect) return;
            LastRoomShutdownReason = result.ShutdownReason;
            if (!result.Ok)
            {
                failed = true;
                ReportRoomError($"Unable to join the room: {result.ShutdownReason}");
            }
            else
            {
                Debug.Log("[Room] Session started.");
                NotifyRoomOperationChanged();
            }
        }
        catch (OperationCanceledException) when (_connectionToken.IsCancellationRequested) { }
        catch (Exception exception)
        {
            failed = true;
            if (CanConnect) ReportRoomError(exception.Message);
        }
        finally
        {
            if (CanConnect && popup != null) popup.ClosePopupLoading();
        }
        // A failed StartGame can terminate its Runner. Always recover with a new one.
        if (failed && CanConnect && SessionPhase != NetworkSessionPhase.Lobby)
            await ReturnToLobbyAsync();
    }

    private void ReportRoomError(string message)
    {
        if (!CanConnect) return;
        LastRoomError = message;
        Debug.LogError($"[Room] {message}");
        NotifyRoomOperationChanged();
        if (!CanConnect) return;
        if (AD.Managers.Instance != null && AD.Managers.PopupM != null) AD.Managers.PopupM.PopupMessage(message);
    }

    private void NotifyRoomOperationChanged()
    {
        if (!HasLiveOwner) return;
        if (RoomOperationChanged == null) return;
        foreach (Action<NetworkRunnerManager> subscriber in RoomOperationChanged.GetInvocationList())
        {
            if (!HasLiveOwner) break;
            try { subscriber(this); }
            catch (Exception exception) { Debug.LogError($"[Room] Status listener exception: {exception.GetType().Name}"); }
        }
    }

    public void StartGame()
    {
        if (!CanConnect || _gameTransitionStarted || SessionPhase != NetworkSessionPhase.Room
            || _networkRunner == null || !_networkRunner.IsServer || RoomManager.Instance == null || !RoomManager.Instance.IsReady()) return;
        _gameTransitionStarted = true;
        _ = StartMatchAsync();
    }

    private async Task StartMatchAsync()
    {
        if (!CanConnect) return;
        try
        {
            var scene = ResolveScene(GameScenePath);
            RoomManager.Instance.RegisterPlayerInGame();
            if (AD.Managers.GameM.RosterCount == 0 || AD.Managers.GameM.RosterCount > NetworkMatchState.RosterCapacity
                || _networkRunner.SessionInfo.MaxPlayers > NetworkMatchState.RosterCapacity)
                throw new InvalidOperationException("Match roster or room capacity exceeds the supported 16 participants.");
            _networkRunner.SessionInfo.IsVisible = false;
            _networkRunner.SessionInfo.IsOpen = false;
            SessionPhase = NetworkSessionPhase.LoadingGame;
            _networkRunner.ProvideInput = false;
            NotifyRoomOperationChanged();
            if (!CanConnect) return;
            var operation = _networkRunner.LoadScene(scene, LoadSceneMode.Single);
            while (!operation.IsDone)
            {
                await Task.Yield();
                if (!CanConnect) return;
            }
            if (!CanConnect) return;
            if (operation.Error != null) throw operation.Error;
        }
        catch (Exception exception)
        {
            if (CanConnect)
            {
                ReportRoomError($"Unable to load the game: {exception.Message}");
                await ReturnToLobbyAsync();
            }
        }
    }
    #endregion

    #region Public Methods
    public void Shutdown()
    {
        _ = ReturnToLobbyAsync();
    }

    public Task ReturnToLobbyAsync()
    {
        if (_returnTask != null) return _returnTask;
        if (!HasLiveOwner) return Task.CompletedTask;
        IsRetired = true;
        SessionPhase = NetworkSessionPhase.ReturningToLobby;
        CancelConnectionOperations();
        AD.Managers.SavedNickName = _nickName ?? string.Empty;
        if (AD.Managers.Instance == null)
        {
            LastRoomError = "The persistent lobby recovery owner is missing.";
            Debug.LogError($"[Network] Lobby recovery failed: {LastRoomError}");
            return _returnTask = Task.CompletedTask;
        }
        _returnTask = AD.Managers.Instance.RecoverLobbyAsync(this, _networkRunner, _shutdownObserved, _peerDisconnectFrame);
        return _returnTask;
    }

    internal void PublishLobbyReturn()
    {
        NotifyRoomOperationChanged();
    }

    internal void ClearLobbyReturnState()
    {
        _roomSpawnedPlayers.Clear();
    }

    internal void ReportLobbyRecoveryFailure(string message)
    {
        LastRoomError = message;
        NotifyRoomOperationChanged();
    }

    public void SaveNickName(string nickName)
    {
        _nickName = nickName;
        AD.Managers.SavedNickName = nickName ?? string.Empty;
    }

    public void ChangeMap(string mapName)
    {
        string originalMap = _roomOptions.MapName;

        if (!CanConnect || SessionPhase != NetworkSessionPhase.Room || mapName != "DesertHouse"
            || string.Equals(originalMap, mapName) || !_networkRunner.IsServer)
        {
            return;
        }

        var customProps = new Dictionary<string, SessionProperty>()
        {
            ["MapName"] = mapName
        };
        _roomOptions.MapName = mapName;

        if (_networkRunner.SessionInfo.UpdateCustomProperties(customProps))
        {
            RoomManager.Instance.RpcMapChange(_roomOptions.MapName);
            AD.DebugLogger.Log("NetworkRunnerManager", $"{mapName}으로 Map 업데이트 성공");
        }
        else
        {
            _roomOptions.MapName = originalMap;
            AD.DebugLogger.Log("NetworkRunnerManager", $"{mapName}으로 Map 업데이트 실패");
        }
    }

    public void ExitButtonClicked()
    {
        Shutdown();
    }

    public NetworkRunner GetNetworkRunner()
    {
        return _networkRunner;
    }

    public RoomOptions GetRoomOptions()
    {
        return _roomOptions;
    }
    #endregion

    #region INetworkRunnerCallbacks

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!IsCurrentCallback(runner) || !CanConnect || !runner.IsServer)
        {
            return;
        }

        if (_roomSceneReady && SessionPhase == NetworkSessionPhase.Room && RoomManager.Instance != null)
            RoomManager.Instance.SpawnRoomPlayer(player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!IsCurrentCallback(runner) || !CanConnect || !runner.IsServer)
        {
            return;
        }

        _roomSpawnedPlayers.Remove(player);
        if (SessionPhase == NetworkSessionPhase.Room && RoomManager.Instance != null)
            RoomManager.Instance.UnregisterPlayer(player);
        else if (AD.Managers.Instance != null && AD.Managers.GameM != null)
            AD.Managers.GameM.RemovePlayer(runner, player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!IsCurrentCallback(runner)) return;
        var data = CanConnect && SessionPhase == NetworkSessionPhase.Game && runner.LocalPlayer != PlayerRef.None && PlayerInputSource.Instance != null
            ? PlayerInputSource.Instance.Snapshot()
            : default;
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        // Normal retired-runner shutdown remains logged during lobby recovery.
        if (!IsCurrentCallback(runner)) return;
        _shutdownObserved = true;
        LastLobbyShutdownReason = shutdownReason;
        if (LobbyStatus != LobbyConnectionStatus.Connecting && LobbyStatus != LobbyConnectionStatus.Failed)
        {
            LobbyStatus = LobbyConnectionStatus.Disconnected;
        }
        HasReceivedSessionList = false;
        _sessionList.Clear();
        NotifyLobbyStatusChanged();
        LastRoomShutdownReason = shutdownReason;
        Debug.Log($"[Network] Runner shutdown: {shutdownReason}");
        if (SessionPhase != NetworkSessionPhase.Lobby)
        {
            if (!IsRetired) _ = ReturnToLobbyAsync();
            else if (AD.Managers.Instance != null)
                _ = AD.Managers.Instance.RecoverLobbyAsync(this, runner, automaticShutdown: true, peerDisconnectFrame: _peerDisconnectFrame);
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        if (!IsCurrentCallback(runner) || !CanConnect) return;
        // A simulation peer timeout does not itself initiate Runner shutdown.
        _peerDisconnectFrame = Time.frameCount;
        LastLobbyError = reason.ToString();
        if (LobbyStatus != LobbyConnectionStatus.Connecting && LobbyStatus != LobbyConnectionStatus.Failed)
        {
            LobbyStatus = LobbyConnectionStatus.Disconnected;
        }
        HasReceivedSessionList = false;
        _sessionList.Clear();
        NotifyLobbyStatusChanged();
        // Status listeners may already have returned or stopped this session.
        if (CanConnect && SessionPhase != NetworkSessionPhase.Lobby) _ = ReturnToLobbyAsync();
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (!IsCurrentCallback(runner) || !CanConnect || SessionPhase != NetworkSessionPhase.Lobby) return;
        _sessionList = new List<SessionInfo>(sessionList);
        HasReceivedSessionList = true;
        NotifyLobbyStatusChanged();
        if (!CanConnect) return;
        Debug.Log($"[Lobby] Session list received. Count: {SessionCount}");

        string name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (CanConnect && name == AD.GameConstants.Scene.Lobby.ToString() && RoomManage.Instance != null)
        {
            RoomManage.Instance.Init(sessionList);
        }
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (!IsCurrentCallback(runner) || !CanConnect) return;
        var game = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(GameScenePath);
        var room = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(RoomScenePath);
        try
        {
            if (game.IsValid() && game.isLoaded)
            {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(game);
                SessionPhase = NetworkSessionPhase.Game;
                _roomSceneReady = false;
                runner.ProvideInput = true;
                if (runner.IsServer)
                {
                    AD.Managers.GameM.EnsureMatchSpawn(runner);
                    AD.Managers.GameM.Init();
                }
            }
            else if (room.IsValid() && room.isLoaded)
            {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(room);
                SessionPhase = NetworkSessionPhase.Room;
                _roomSceneReady = true;
                _roomOptions.IsServer = runner.IsServer;
                if (CanvasRoom.Instance != null) CanvasRoom.Instance.Init(_roomOptions);
                if (runner.IsServer && RoomManager.Instance != null)
                    foreach (var player in runner.ActivePlayers.ToArray()) RoomManager.Instance.SpawnRoomPlayer(player);
            }
            NotifyRoomOperationChanged();
        }
        catch (Exception exception)
        {
            ReportRoomError($"Unable to initialize the network scene: {exception.Message}");
            _ = ReturnToLobbyAsync();
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        if (!IsCurrentCallback(runner) || !CanConnect) return;
        runner.ProvideInput = false;
        if (SessionPhase == NetworkSessionPhase.Room) SessionPhase = NetworkSessionPhase.LoadingGame;
        NotifyRoomOperationChanged();
    }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    #endregion

    #endregion
}

public class RoomOptions
{
    public bool IsServer { get; set; }
    public string RoomName { get; set; }
    public string MapName { get; set; }
    public string Players { get; set; }
    public int PlayerCount { get; set; }
}
