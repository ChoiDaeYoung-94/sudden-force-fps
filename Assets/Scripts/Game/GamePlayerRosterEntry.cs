using Fusion;

public readonly struct GamePlayerRosterEntry
{
    public PlayerRef Player { get; }
    public string NickName { get; }
    public int Team { get; }

    public GamePlayerRosterEntry(PlayerRef player, string nickName, int team)
    {
        Player = player;
        NickName = nickName;
        Team = team;
    }
}

public enum NetworkSessionPhase
{
    Lobby,
    JoiningRoom,
    Room,
    LoadingGame,
    Game,
    ReturningToLobby
}
