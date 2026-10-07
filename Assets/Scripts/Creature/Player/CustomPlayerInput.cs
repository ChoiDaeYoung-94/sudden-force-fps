using Fusion;

public enum PlayerInputButton
{
    Sprint = 0,
    Fire = 1,
    Reload = 2
}

public struct CustomPlayerInput : INetworkInput
{
    public float MoveX;
    public float MoveZ;
    public bool Fire; // Compatibility mirror only. Server reads Buttons.Fire.
    public bool HasAim;
    public int RespawnVersion;
    public float AimYaw;
    public float AimPitch;
    public NetworkButtons Buttons;
}
