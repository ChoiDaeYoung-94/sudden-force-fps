using Fusion;
using UnityEngine;

public class RoomPlayerNetworkData : NetworkBehaviour
{
    [Networked] public string NickName { get; set; }
    [Networked] public bool IsReady { get; set; }
    [Networked] public int Team { get; set; }

    public RoomPlayerUI PlayerUI;

    public override void Spawned()
    {
        if (RoomManager.Instance != null) RoomManager.Instance.RegisterPlayer(this);

        if (Object.HasInputAuthority)
        {
            RpcSetNickName(NetworkRunnerManager.Instance._nickName);
        }

        UpdatePresentation();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);

        if (RoomManager.Instance != null) RoomManager.Instance.RemovePlayer(this);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcSetNickName(string nick, RpcInfo info = default)
    {
        if (Object.HasStateAuthority && IsValidSender(info))
        {
            nick = (nick ?? string.Empty).Trim();
            NickName = nick.Length > 32 ? nick.Substring(0, 32) : nick;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcChangeTeam(int newTeam, RpcInfo info = default)
    {
        var room = RoomManager.Instance;
        if (Object.HasStateAuthority && IsValidSender(info) && room != null && room.CanChangeTeam(this, newTeam))
        {
            if (Team != newTeam)
            {
                Team = newTeam;
                IsReady = false;
            }
        }
    }

    private void SetTeam(int team)
    {
        var room = RoomManager.Instance;
        if (room == null) return;
        Transform teamPos = team == 0 ? room.RedTeam : room.BlueTeam;
        if (teamPos != null && transform.parent != teamPos) transform.SetParent(teamPos, worldPositionStays: false);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcRequestToggleReady(RpcInfo info = default)
    {
        if (Object.HasStateAuthority && IsValidSender(info) && NetworkRunnerManager.Instance != null
            && NetworkRunnerManager.Instance.SessionPhase == NetworkSessionPhase.Room)
        {
            IsReady = !IsReady;
        }
    }

    private bool IsValidSender(RpcInfo info)
    {
        // Default Host RPCs use PlayerRef.None; only the host's own object may accept that source.
        return info.Source == Object.InputAuthority || (info.Source == PlayerRef.None && Object.HasInputAuthority);
    }

    public override void Render()
    {
        UpdatePresentation();
    }

    private string _shownNickName;
    private int _shownTeam = -1;
    private bool? _shownReady;

    private void UpdatePresentation()
    {
        if (PlayerUI == null) return;
        if (_shownNickName != NickName)
        {
            _shownNickName = NickName;
            PlayerUI.SetNickName(NickName);
        }
        if (_shownTeam != Team)
        {
            _shownTeam = Team;
            SetTeam(Team);
            PlayerUI.UpdateTeamUI(Team);
        }
        if (_shownReady != IsReady)
        {
            _shownReady = IsReady;
            PlayerUI.SetReadyState(IsReady);
        }
    }
}
