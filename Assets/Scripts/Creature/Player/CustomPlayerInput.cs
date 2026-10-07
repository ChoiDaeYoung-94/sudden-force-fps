using Fusion;

public enum PlayerInputButton
{
    Sprint
}

public struct CustomPlayerInput : INetworkInput
{
    public float MoveX;
    public float MoveZ;
    public bool Fire; // Retained for existing callers; combat is a later stage.
    public bool HasAim;
    public float AimYaw;
    public float AimPitch;
    public NetworkButtons Buttons;
}
