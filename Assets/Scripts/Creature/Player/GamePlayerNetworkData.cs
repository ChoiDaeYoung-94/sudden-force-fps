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
    [SerializeField] private WeaponDefinition _weaponDefinition;
    [SerializeField] private HitboxRoot _hitboxRoot;
    [SerializeField] private Transform _viewModelRoot;
    [SerializeField] private Transform _viewMuzzle;
    [SerializeField] private Transform _worldMuzzle;
    public Transform CameraRoot => _cameraRoot;
    public WeaponDefinition WeaponDefinition => _weaponDefinition;
    public HitboxRoot HitboxRoot => _hitboxRoot;
    public Transform ViewModelRoot => _viewModelRoot;
    public Transform ViewMuzzle => _viewMuzzle;
    public Transform WorldMuzzle => _worldMuzzle;
    public bool CombatConfigured => _weaponDefinition != null && _hitboxRoot != null && _weaponDefinition.TryValidate(out _);
    public bool MovementConfigured => _characterController != null && _movementController != null;
    public bool CanProvideInput => Object != null && Object.IsValid && Object.HasInputAuthority && Health > 0
        && NetworkRunnerManager.Instance != null && !NetworkRunnerManager.Instance.IsRetired
        && NetworkRunnerManager.Instance.SessionPhase == NetworkSessionPhase.Game;
    private PlayerInputSource _inputSource;
    private ShadowCastingMode[] _originalShadowModes;
    private bool _spawned;
    private CombatPresentation _presentation;
    private Vector3 _viewModelBasePosition;
    private Quaternion _viewModelBaseRotation;
    public CombatPresentation Presentation => _presentation;
    private readonly CombatShotQuery _shotQuery = new CombatShotQuery();
    public bool IsReloading => _spawned && Object != null && Object.IsValid && ReloadTimer.IsRunning;
    public int MagazineCapacity => _weaponDefinition != null ? _weaponDefinition.MagazineCapacity : 30;
    public float ReloadRemaining => _spawned && Object != null && Object.IsValid && Runner != null && Runner.IsRunning
        ? ReloadTimer.RemainingTime(Runner) ?? 0f : 0f;

    [Networked] public string NickName { get; set; }
    [Networked] public int Team { get; set; }
    [Networked] public int Health { get; set; }
    [Networked] public int Ammo { get; set; }
    [Networked] public int Kill { get; set; }
    [Networked] public int Death { get; set; }
    [Networked] public float AimYaw { get; set; }
    [Networked] public float AimPitch { get; set; }
    [Networked] public NetworkButtons PreviousButtons { get; set; }
    [Networked] public TickTimer NextShotTimer { get; set; }
    [Networked] public TickTimer ReloadTimer { get; set; }
    [Networked] public int ShotSequence { get; set; }
    [Networked] public int HitSequence { get; set; }
    [Networked] public Vector3 LastShotOrigin { get; set; }
    [Networked] public Vector3 LastShotDirection { get; set; }
    [Networked] public bool LastShotHit { get; set; }
    [Networked] public Vector3 LastShotHitPoint { get; set; }
    [Networked] public Vector3 LastShotHitNormal { get; set; }
    [Networked] public CombatBodyPart LastHitBodyPart { get; set; }
    [Networked] public int LastHitDamage { get; set; }
    [Networked] public PlayerRef LastHitTarget { get; set; }
    [Networked] public int LastHitShotSequence { get; set; }
    [Networked] public Vector3 LastHitPoint { get; set; }
    [Networked] public Vector3 LastHitNormal { get; set; }
    // Prediction-only visual budget mirrors authoritative Ammo on the host.
    // Actual Ammo and Health remain state-authority-only.
    [Networked] public Vector2 RecoilOffset { get; set; }
    [Networked] public int RecoilIndex { get; set; }
    [Networked] public int RecoilPreviewAmmo { get; set; }
    [Networked] public TickTimer RecoilShotTimer { get; set; }
    [Networked] public TickTimer RecoilReloadTimer { get; set; }
    [Networked] public int LastShotRecoilIndex { get; set; }
    [Networked] public Vector2 LastShotRecoilOffset { get; set; }

    private void Awake()
    {
        foreach (var camera in GetComponentsInChildren<Camera>(true)) camera.enabled = false;
        foreach (var listener in GetComponentsInChildren<AudioListener>(true)) listener.enabled = false;
    }

    public override void Spawned()
    {
        _spawned = true;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        _hasDiagnosticSnapshot = false;
        _nextRejectionLogTime = 0f;
#endif
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
        if (_viewModelRoot != null)
        {
            _viewModelBasePosition = _viewModelRoot.localPosition;
            _viewModelBaseRotation = _viewModelRoot.localRotation;
        }
        if (Object.HasStateAuthority)
        {
            Health = 100;
            Ammo = CombatConfigured ? _weaponDefinition.MagazineCapacity : 30;
            Kill = 0;
            Death = 0;
            AimYaw = transform.eulerAngles.y;
            AimPitch = 0f;
            RecoilOffset = Vector2.zero;
            RecoilIndex = 0;
            RecoilPreviewAmmo = Ammo;
            RecoilShotTimer = default;
            RecoilReloadTimer = default;
        }
        if (Object.HasInputAuthority)
        {
            _inputSource = GetComponent<PlayerInputSource>();
            if (_inputSource == null) _inputSource = gameObject.AddComponent<PlayerInputSource>();
            _inputSource.Bind(this, AimYaw, AimPitch);
        }
        ApplyLocalPresentation();
        _presentation = GetComponent<CombatPresentation>();
        if (_presentation == null) _presentation = gameObject.AddComponent<CombatPresentation>();
        _presentation.Bind(this);
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
        if (_viewModelRoot != null && _viewModelRoot.gameObject.activeSelf != local) _viewModelRoot.gameObject.SetActive(local);
        if (_bodyRenderers == null || _originalShadowModes == null) return;
        for (int i = 0; i < _bodyRenderers.Length; i++)
            if (_bodyRenderers[i] != null && (_viewModelRoot == null || !_bodyRenderers[i].transform.IsChildOf(_viewModelRoot)))
                _bodyRenderers[i].shadowCastingMode = local ? ShadowCastingMode.ShadowsOnly : _originalShadowModes[i];
    }

    private void ReleaseLocalPresentation()
    {
        _spawned = false;
        if (_presentation != null) _presentation.Unbind();
        if (_inputSource != null) _inputSource.Unbind();
        if (_localCamera != null) _localCamera.enabled = false;
        if (_localAudioListener != null) _localAudioListener.enabled = false;
        if (_viewModelRoot != null) _viewModelRoot.gameObject.SetActive(false);
        if (_bodyRenderers != null && _originalShadowModes != null)
            for (int i = 0; i < _bodyRenderers.Length; i++)
                if (_bodyRenderers[i] != null) _bodyRenderers[i].shadowCastingMode = _originalShadowModes[i];
    }
    private void OnDestroy() => ReleaseLocalPresentation();

    public override void Render()
    {
        ApplyLocalPresentation();
        if (_cameraRoot != null) _cameraRoot.localRotation = CombatRecoil.ViewRotation(AimPitch, RecoilOffset);
        if (_viewModelRoot != null && Object.HasInputAuthority)
        {
            // ViewModelRoot is a CameraRoot child and inherits its recoil rotation
            // once. Only a small visual kickback is applied in local space.
            _viewModelRoot.localRotation = _viewModelBaseRotation;
            _viewModelRoot.localPosition = _viewModelBasePosition + Vector3.back * Mathf.Min(Mathf.Max(RecoilOffset.y, 0f) * 0.01f, 0.06f);
        }
        if (_presentation != null && _presentation.isActiveAndEnabled)
        {
            if (_presentation.Owner == null) _presentation.Bind(this);
            _presentation.RenderState();
        }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        LogCombatSnapshot();
#endif
    }

    public override void FixedUpdateNetwork()
    {
        if ((!Object.HasStateAuthority && !Object.HasInputAuthority) || !MovementConfigured) return;
        var move = Vector2.zero;
        bool sprint = false;
        bool hasInput = GetInput(out CustomPlayerInput input);
        if (CombatConfigured)
            RecoilOffset = CombatRecoil.Recover(RecoilOffset, _weaponDefinition.RecoilRecoveryDegreesPerSecond, Runner.DeltaTime);
        if (Health > 0 && hasInput)
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
        if (Health <= 0)
        {
            _movementController.Velocity = Vector3.zero;
            if (Object.HasStateAuthority) { PreviousButtons = default; ReloadTimer = default; }
            ResetRecoilPreview();
            return;
        }
        transform.rotation = Quaternion.Euler(0f, AimYaw, 0f);
        // SDK Move normalizes direction, so maxSpeed carries analog magnitude.
        _movementController.maxSpeed = (sprint ? 7.5f : 5f) * move.magnitude;
        _movementController.Move(transform.rotation * new Vector3(move.x, 0f, move.y));
        if (Object.HasStateAuthority) UpdateCombat(hasInput, input);
        else if (Object.HasInputAuthority) PredictRecoil(hasInput, input);
    }

    private void UpdateCombat(bool hasInput, CustomPlayerInput input)
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager == null || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game || !CombatConfigured || Health <= 0)
        {
            PreviousButtons = default;
            ReloadTimer = default;
            ResetRecoilPreview();
            if (hasInput && input.Buttons.IsSet(PlayerInputButton.Fire)) LogCombatRejection("session-or-configuration");
            return;
        }
        // A started reload completes even when focus/input packets are missing.
        if (ReloadTimer.IsRunning && ReloadTimer.Expired(Runner))
        {
            Ammo = _weaponDefinition.MagazineCapacity;
            ReloadTimer = default;
            RecoilPreviewAmmo = Ammo;
            RecoilReloadTimer = default;
            RecoilIndex = 0;
        }
        bool valid = hasInput && input.HasAim && PlayerInputSource.IsFinite(input.AimYaw) && PlayerInputSource.IsFinite(input.AimPitch)
            && input.AimYaw >= 0f && input.AimYaw < 360f && input.AimPitch >= -85f && input.AimPitch <= 85f
            && (Team == 0 || Team == 1) && Ammo >= 0 && Ammo <= _weaponDefinition.MagazineCapacity
            && CombatShotQuery.TryDurationTicks(_weaponDefinition.ShotInterval, Runner.TickRate, out _)
            && CombatShotQuery.TryDurationTicks(_weaponDefinition.ReloadDuration, Runner.TickRate, out _);
        if (!valid)
        {
            PreviousButtons = default;
            if (hasInput && input.Buttons.IsSet(PlayerInputButton.Fire)) LogCombatRejection("input-or-ammo-or-timing");
            return;
        }
        var pressed = input.Buttons.GetPressed(PreviousButtons);
        PreviousButtons = input.Buttons;
        if (pressed.IsSet(PlayerInputButton.Reload))
        {
            if (!ReloadTimer.IsRunning && Ammo < _weaponDefinition.MagazineCapacity)
            {
                ReloadTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(_weaponDefinition.ReloadDuration, Runner.TickRate));
                RecoilReloadTimer = ReloadTimer;
            }
            // Reload wins over fire on the same tick, even when already full.
            return;
        }
        if (!input.Buttons.IsSet(PlayerInputButton.Fire)) return;
        if (ReloadTimer.IsRunning) { LogCombatRejection("reloading"); return; }
        if (Ammo <= 0) { LogCombatRejection("empty-magazine"); return; }
        if (!NextShotTimer.ExpiredOrNotRunning(Runner)) { LogCombatRejection("shot-interval"); return; }
        if (!CombatShotQuery.IsFinite(transform.position) || !PlayerInputSource.IsFinite(AimYaw) || !PlayerInputSource.IsFinite(AimPitch)) return;
        Ammo--;
        ShotSequence++;
        NextShotTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(_weaponDefinition.ShotInterval, Runner.TickRate));
        ApplyRecoilShot();
        RecoilPreviewAmmo = Ammo;
        LastShotRecoilIndex = RecoilIndex;
        LastShotRecoilOffset = RecoilOffset;
        LastShotOrigin = transform.position + Vector3.up * 1.75f;
        LastShotDirection = CombatRecoil.ShotDirection(AimYaw, AimPitch, RecoilOffset);
        LastShotHit = _shotQuery.TryCast(Runner, Object.InputAuthority, LastShotOrigin, LastShotDirection, _weaponDefinition.Range, out var hit);
        LastShotHitPoint = LastShotHit ? hit.Point : LastShotOrigin + LastShotDirection * _weaponDefinition.Range;
        LastShotHitNormal = LastShotHit ? hit.Normal : Vector3.zero;
        if (!LastShotHit || hit.Hitbox == null) return;
        var metadata = hit.Hitbox.GetComponent<CombatHitbox>();
        if (metadata == null || !metadata.IsConfigured) return;
        var target = metadata.Owner;
        if (target == this || target.Object == null || !target.Object.IsValid || !target.Object.HasStateAuthority
            || target.Runner != Runner || target.Health <= 0 || target.Team == Team || (target.Team != 0 && target.Team != 1)) return;
        int damage = _weaponDefinition.GetDamage(metadata.BodyPart);
        if (damage <= 0) return;
        int previousHealth = target.Health;
        target.Health = Mathf.Max(0, previousHealth - damage);
        HitSequence++;
        LastHitBodyPart = metadata.BodyPart;
        LastHitDamage = previousHealth - target.Health;
        LastHitTarget = target.Object.InputAuthority;
        LastHitShotSequence = ShotSequence;
        LastHitPoint = hit.Point;
        LastHitNormal = hit.Normal;
    }

    private void ApplyRecoilShot()
    {
        // Shared order: recover at tick start, then add pattern[index], advance
        // index, calculate shot/view from the resulting offset with one clamp.
        RecoilOffset += _weaponDefinition.GetRecoilDelta(RecoilIndex);
        RecoilIndex++;
        RecoilShotTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(_weaponDefinition.ShotInterval, Runner.TickRate));
    }

    private void ResetRecoilPreview()
    {
        RecoilOffset = Vector2.zero; RecoilIndex = 0; RecoilPreviewAmmo = 0;
        RecoilShotTimer = default; RecoilReloadTimer = default;
    }

    private void PredictRecoil(bool hasInput, CustomPlayerInput input)
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager == null || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game || !CombatConfigured)
        {
            PreviousButtons = default; ResetRecoilPreview(); return;
        }
        if (RecoilReloadTimer.IsRunning && RecoilReloadTimer.Expired(Runner))
        {
            RecoilPreviewAmmo = _weaponDefinition.MagazineCapacity;
            RecoilReloadTimer = default; RecoilIndex = 0;
        }
        if (!hasInput || !input.HasAim || !PlayerInputSource.IsFinite(input.AimYaw) || !PlayerInputSource.IsFinite(input.AimPitch)
            || input.AimYaw < 0f || input.AimYaw >= 360f || input.AimPitch < -85f || input.AimPitch > 85f
            || (Team != 0 && Team != 1) || RecoilPreviewAmmo < 0 || RecoilPreviewAmmo > _weaponDefinition.MagazineCapacity
            || !CombatShotQuery.TryDurationTicks(_weaponDefinition.ShotInterval, Runner.TickRate, out _)
            || !CombatShotQuery.TryDurationTicks(_weaponDefinition.ReloadDuration, Runner.TickRate, out _))
        { PreviousButtons = default; return; }
        var pressed = input.Buttons.GetPressed(PreviousButtons);
        PreviousButtons = input.Buttons;
        if (pressed.IsSet(PlayerInputButton.Reload))
        {
            if (!RecoilReloadTimer.IsRunning && RecoilPreviewAmmo < _weaponDefinition.MagazineCapacity)
                RecoilReloadTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(_weaponDefinition.ReloadDuration, Runner.TickRate));
            return;
        }
        if (!input.Buttons.IsSet(PlayerInputButton.Fire) || RecoilReloadTimer.IsRunning || RecoilPreviewAmmo <= 0
            || !RecoilShotTimer.ExpiredOrNotRunning(Runner)) return;
        RecoilPreviewAmmo--;
        ApplyRecoilShot();
        // No actual Ammo/HP/ShotSequence changes, hit query or effects here.
    }

    public bool TryGetCombatSnapshot(out CombatStateSnapshot snapshot)
    {
        snapshot = default;
        if (!_spawned || Object == null || !Object.IsValid || Runner == null || !Runner.IsRunning) return false;
        snapshot = new CombatStateSnapshot(Object.Id.ToString(), Object.InputAuthority, Object.HasStateAuthority, Runner.Tick.Raw,
            Health, Ammo, ShotSequence, HitSequence, ReloadTimer.TargetTick, LastHitBodyPart, LastHitDamage, LastHitTarget);
        return true;
    }

    private void LogCombatRejection(string reason)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (!Runner.IsForward || Time.unscaledTime < _nextRejectionLogTime) return;
        _nextRejectionLogTime = Time.unscaledTime + 1f;
        Debug.Log($"[CombatReject] id={Object.Id} input={Object.InputAuthority} tick={Runner.Tick.Raw} reason={reason}");
#endif
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private bool _hasDiagnosticSnapshot;
    private CombatStateSnapshot _diagnosticSnapshot;
    private float _nextRejectionLogTime;

    private void LogCombatSnapshot()
    {
        if (!TryGetCombatSnapshot(out var snapshot)) return;
        bool changed = !_hasDiagnosticSnapshot || snapshot.Health != _diagnosticSnapshot.Health || snapshot.Ammo != _diagnosticSnapshot.Ammo
            || snapshot.ShotSequence != _diagnosticSnapshot.ShotSequence || snapshot.HitSequence != _diagnosticSnapshot.HitSequence
            || snapshot.ReloadTargetTick != _diagnosticSnapshot.ReloadTargetTick;
        if (!changed) return;
        string stage = _hasDiagnosticSnapshot ? "changed" : "initial";
        Debug.Log($"[CombatState] {stage} id={snapshot.NetworkId} input={snapshot.InputAuthority} stateAuthority={snapshot.IsStateAuthority} tick={snapshot.Tick} hp={snapshot.Health} ammo={snapshot.Ammo} reloadEnd={snapshot.ReloadTargetTick} shot={snapshot.ShotSequence} hit={snapshot.HitSequence} part={snapshot.LastHitBodyPart} damage={snapshot.LastHitDamage} target={snapshot.LastHitTarget} recoilIndex={RecoilIndex} recoil={RecoilOffset} previewAmmo={RecoilPreviewAmmo}");
        _diagnosticSnapshot = snapshot;
        _hasDiagnosticSnapshot = true;
    }
#endif
}

// Read-only copied state for Editor/Development QA and C3 binding. No credentials
// or nicknames, and no path from the snapshot back to mutating network state.
public readonly struct CombatStateSnapshot
{
    public readonly string NetworkId;
    public readonly PlayerRef InputAuthority;
    public readonly bool IsStateAuthority;
    public readonly int Tick, Health, Ammo, ShotSequence, HitSequence;
    public readonly int? ReloadTargetTick;
    public readonly CombatBodyPart LastHitBodyPart;
    public readonly int LastHitDamage;
    public readonly PlayerRef LastHitTarget;
    public CombatStateSnapshot(string networkId, PlayerRef inputAuthority, bool isStateAuthority, int tick, int health, int ammo,
        int shotSequence, int hitSequence, int? reloadTargetTick, CombatBodyPart part, int damage, PlayerRef target)
    {
        NetworkId = networkId; InputAuthority = inputAuthority; IsStateAuthority = isStateAuthority; Tick = tick;
        Health = health; Ammo = ammo; ShotSequence = shotSequence; HitSequence = hitSequence; ReloadTargetTick = reloadTargetTick;
        LastHitBodyPart = part; LastHitDamage = damage; LastHitTarget = target;
    }
}
