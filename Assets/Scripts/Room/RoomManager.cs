using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : NetworkBehaviour
{
    private static RoomManager _instance;
    public static RoomManager Instance { get { return _instance; } }

    [SerializeField] private GameObject _roomPlayer;
    public Transform RedTeam;
    public Transform BlueTeam;

    public List<RoomPlayerNetworkData> RoomPlayers = new List<RoomPlayerNetworkData>();
    public RoomPlayerNetworkData LocalPlayerData;

    private void Awake()
    {
        _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }

    public void SpawnRoomPlayer(PlayerRef player)
    {
        if (RoomPlayers.Any(p => p != null && p.Object.InputAuthority == player)) return;
        NetworkRunnerManager.Instance.RoomSceneSpawn(_roomPlayer, player);
    }

    /// <summary>
    /// 플레이어가 Spawn될 때 호출되어 리스트에 추가하고, 로컬 플레이어인 경우 별도로 저장
    /// </summary>
    public void RegisterPlayer(RoomPlayerNetworkData player)
    {
        if (!RoomPlayers.Contains(player))
        {
            RoomPlayers.Add(player);
        }

        if (player.Object.HasInputAuthority)
        {
            LocalPlayerData = player;
        }
    }

    public void UnregisterPlayer(PlayerRef player)
    {
        RoomPlayerNetworkData roomPlayer = RoomPlayers.FirstOrDefault(p => p.Object.InputAuthority == player);
        if (roomPlayer != null)
        {
            NetworkRunnerManager.Instance.DeSpawn(roomPlayer.Object);
            RemovePlayer(roomPlayer);
        }
    }

    public void UnregisterAllPlayer()
    {
        for (int i = RoomPlayers.Count - 1; i >= 0; i--)
        {
            RoomPlayerNetworkData player = RoomPlayers[i];
            if (player != null) NetworkRunnerManager.Instance.DeSpawn(player.Object);
        }
        RoomPlayers.Clear();
        LocalPlayerData = null;
    }

    public void RegisterPlayerInGame()
    {
        AD.Managers.GameM.SetRoster(RoomPlayers.Where(p => p != null)
            .Select(p => new GamePlayerRosterEntry(p.Object.InputAuthority, p.NickName, p.Team)).ToArray());
    }

    public void RemovePlayer(RoomPlayerNetworkData player)
    {
        RoomPlayers.Remove(player);
        if (LocalPlayerData == player) LocalPlayerData = null;
    }

    public void OnReadyButtonClicked()
    {
        if (LocalPlayerData != null && LocalPlayerData.Object.HasInputAuthority)
        {
            LocalPlayerData.RpcRequestToggleReady();
        }
    }

    public void OnStartButtonClicked()
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager != null && manager.GetNetworkRunner().IsServer && IsReady())
        {
            StartGame();
        }
    }

    public bool IsReady() => GetStartBlockedReason() == null;

    // Presentation uses the same prerequisites as the authoritative start guard.
    public string GetStartBlockedReason()
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager == null || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Room)
            return "Please wait until the room connection is ready.";
        var runner = manager.GetNetworkRunner();
        if (runner == null || !runner.IsRunning)
            return "The room connection is unavailable. Please return to the lobby and reconnect.";
        var active = new HashSet<PlayerRef>(runner.ActivePlayers);
        if (RoomPlayers.Any(p => p != null && (p.Object == null || !p.Object.IsValid)))
            return "Player details are syncing. Please try again shortly.";
        var players = RoomPlayers.Where(p => p != null && active.Contains(p.Object.InputAuthority)).ToArray();
        if (players.Length < 2)
            return "At least 2 players are needed. Invite another player before starting.";
        if (players.Length != active.Count)
            return "Player details are syncing. Please try again shortly.";
        if (players.Any(p => (p.Team != 0 && p.Team != 1) || string.IsNullOrWhiteSpace(p.NickName)))
            return "Player details are syncing. Please try again shortly.";
        if (!players.Any(p => p.Team == 0) || !players.Any(p => p.Team == 1))
            return "Both RED TEAM and BLUE TEAM need at least one player. Use a team button to switch.";
        if (players.Any(p => p.Object.InputAuthority != runner.LocalPlayer && !p.IsReady))
            return "Waiting for the other players to select READY. Switching teams resets READY.";
        return null;
    }

    public void OnTeamSwitchButtonClicked(int teamId)
    {
        if (teamId != 0 && teamId != 1) return;

        if (LocalPlayerData != null && LocalPlayerData.Object.HasInputAuthority)
        {
            LocalPlayerData.RpcChangeTeam(teamId);
        }
    }

    public Transform GetTeamPosition()
    {
        (int red, int blue) = GetTeamCount();
        return red <= blue ? RedTeam : BlueTeam;
    }

    public bool CanChangeTeam(RoomPlayerNetworkData player, int team)
    {
        var manager = NetworkRunnerManager.Instance;
        return manager != null && manager.SessionPhase == NetworkSessionPhase.Room
            && (team == 0 || team == 1) && RoomPlayers.Contains(player)
            && RoomPlayers.Count(p => p != null && p != player && p.Team == team) < manager.GetRoomOptions().PlayerCount;
    }

    private (int, int) GetTeamCount()
    {
        int red = 0, blue = 0;

        foreach (RoomPlayerNetworkData player in RoomPlayers)
        {
            if (player == null) continue;
            if (player.Team == 0)
            {
                ++red;
            }
            else
            {
                ++blue;
            }
        }

        return (red, blue);
    }

    private void StartGame()
    {
        NetworkRunnerManager.Instance.StartGame();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcMapChange(string mapName, RpcInfo info = default)
    {
        CanvasRoom.Instance.ChangeMapName(mapName);
    }
}
