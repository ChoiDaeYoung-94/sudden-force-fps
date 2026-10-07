using Fusion;

public struct MatchRosterEntry : INetworkStruct
{
    public PlayerRef Player;
    public NetworkString<_32> Name;
    public int Team, Kill, Death;
    public NetworkBool Connected;
}
