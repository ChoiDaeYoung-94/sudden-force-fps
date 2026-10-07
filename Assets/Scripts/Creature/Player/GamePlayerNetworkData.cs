using Fusion;
using UnityEngine;
using UnityEngine.Rendering;

public class GamePlayerNetworkData : NetworkBehaviour
{
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private Camera _localCamera;
    [SerializeField] private AudioListener _localAudioListener;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private NetworkCharacterController _movementController;
    [SerializeField] private Renderer[] _bodyRenderers;
    public Transform CameraRoot => _cameraRoot;
    public bool MovementConfigured => _characterController != null && _movementController != null;
    public bool CanProvideInput => Object != null && Object.IsValid && Object.HasInputAuthority && Health > 0
        && NetworkRunnerManager.Instance != null && !NetworkRunnerManager.Instance.IsRetired
        && NetworkRunnerManager.Instance.SessionPhase == NetworkSessionPhase.Game;
    private PlayerInputSource _inputSource;
    private ShadowCastingMode[] _originalShadowModes;
    private bool _spawned;

    [Networked] public string NickName { get; set; }
    [Networked] public int Team { get; set; }
    [Networked] public int Health { get; set; }
    [Networked] public int Ammo { get; set; }
    [Networked] public int Kill { get; set; }
    [Networked] public int Death { get; set; }
    [Networked] public float AimYaw { get; set; }
    [Networked] public float AimPitch { get; set; }

    private void Awake()
    {
        foreach (var camera in GetComponentsInChildren<Camera>(true)) camera.enabled = false;
        foreach (var listener in GetComponentsInChildren<AudioListener>(true)) listener.enabled = false;
    }

    public override void Spawned()
    {
        _spawned = true;
        if (_characterController == null) _characterController = GetComponent<CharacterController>();
        if (_movementController == null) _movementController = GetComponent<NetworkCharacterController>();
        if (_localCamera == null) _localCamera = GetComponentInChildren<Camera>(true);
        if (_localAudioListener == null) _localAudioListener = GetComponentInChildren<AudioListener>(true);
        if (_cameraRoot == null && _localCamera != null) _cameraRoot = _localCamera.transform;
        if (_bodyRenderers == null || _bodyRenderers.Length == 0) _bodyRenderers = GetComponentsInChildren<Renderer>(true);
        _originalShadowModes = new ShadowCastingMode[_bodyRenderers.Length];
        for (int i = 0; i < _bodyRenderers.Length; i++)
            if (_bodyRenderers[i] != null) _originalShadowModes[i] = _bodyRenderers[i].shadowCastingMode;
        foreach (var animator in GetComponentsInChildren<Animator>(true)) animator.applyRootMotion = false;
        if (_movementController != null) _movementController.rotationSpeed = 0f;
        if (Object.HasStateAuthority)
        {
            Health = 100;
            Ammo = 30;
            Kill = 0;
            Death = 0;
            AimYaw = transform.eulerAngles.y;
            AimPitch = 0f;
        }
        if (Object.HasInputAuthority)
        {
            _inputSource = GetComponent<PlayerInputSource>();
            if (_inputSource == null) _inputSource = gameObject.AddComponent<PlayerInputSource>();
            _inputSource.Bind(this, AimYaw, AimPitch);
        }
        ApplyLocalPresentation();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        ReleaseLocalPresentation();
    }

    private void ApplyLocalPresentation()
    {
        bool local = _spawned && Object != null && Object.IsValid && Object.HasInputAuthority;
        if (_localCamera != null) _localCamera.enabled = local;
        if (_localAudioListener != null) _localAudioListener.enabled = local;
        if (_bodyRenderers == null || _originalShadowModes == null) return;
        for (int i = 0; i < _bodyRenderers.Length; i++)
            if (_bodyRenderers[i] != null)
                _bodyRenderers[i].shadowCastingMode = local ? ShadowCastingMode.ShadowsOnly : _originalShadowModes[i];
    }

    private void ReleaseLocalPresentation()
    {
        _spawned = false;
        if (_inputSource != null) _inputSource.Unbind();
        if (_localCamera != null) _localCamera.enabled = false;
        if (_localAudioListener != null) _localAudioListener.enabled = false;
    }
    private void OnDestroy() => ReleaseLocalPresentation();

    public override void Render()
    {
        ApplyLocalPresentation();
        if (_cameraRoot != null) _cameraRoot.localRotation = Quaternion.Euler(AimPitch, 0f, 0f);
    }

    public override void FixedUpdateNetwork()
    {
        if ((!Object.HasStateAuthority && !Object.HasInputAuthority) || !MovementConfigured) return;
        var move = Vector2.zero;
        bool sprint = false;
        if (Health > 0 && GetInput(out CustomPlayerInput input))
        {
            if (PlayerInputSource.IsFinite(input.MoveX) && PlayerInputSource.IsFinite(input.MoveZ))
                move = Vector2.ClampMagnitude(new Vector2(input.MoveX, input.MoveZ), 1f);
            if (input.HasAim && PlayerInputSource.IsFinite(input.AimYaw) && PlayerInputSource.IsFinite(input.AimPitch))
            {
                float maxTurn = 1200f * Runner.DeltaTime;
                AimYaw = Mathf.Repeat(Mathf.MoveTowardsAngle(AimYaw, Mathf.Repeat(input.AimYaw, 360f), maxTurn), 360f);
                AimPitch = Mathf.MoveTowards(AimPitch, Mathf.Clamp(input.AimPitch, -85f, 85f), maxTurn);
            }
            sprint = input.Buttons.IsSet(PlayerInputButton.Sprint);
        }
        if (Health <= 0) { _movementController.Velocity = Vector3.zero; return; }
        transform.rotation = Quaternion.Euler(0f, AimYaw, 0f);
        // SDK Move normalizes direction, so maxSpeed carries analog magnitude.
        _movementController.maxSpeed = (sprint ? 7.5f : 5f) * move.magnitude;
        _movementController.Move(transform.rotation * new Vector3(move.x, 0f, move.y));
    }
}
