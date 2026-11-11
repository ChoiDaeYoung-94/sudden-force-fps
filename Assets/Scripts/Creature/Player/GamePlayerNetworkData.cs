using Fusion;
using UnityEngine;
using UnityEngine.Rendering;

public class GamePlayerNetworkData : NetworkBehaviour, IBeforeHitboxRegistration
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
    public bool CanProvideInput => Object != null && Object.IsValid && Object.HasInputAuthority && Health > 0 && !IsDead
        && NetworkRunnerManager.Instance != null && !NetworkRunnerManager.Instance.IsRetired
        && NetworkRunnerManager.Instance.SessionPhase == NetworkSessionPhase.Game && NetworkMatchState.AllowsCombatFor(Runner);
    private PlayerInputSource _inputSource;
    private PlayerCrouchGeometry _crouchGeometry;
    private PlayerLadderMotor _ladderMotor;
    public bool PresentationClimbing => _ladderMotor != null && _ladderMotor.PresentationClimbing;
    private NetworkBehaviour.PropertyReader<NetworkBool> _crouchReader;
    [Networked] public NetworkBool IsCrouching { get; set; }
    // Protocol v2 only. Authoritative/predicted tick state, never a render clock.
    [Networked] public float WorldGaitPhase { get; set; }
    [Networked] public Vector2 WorldGaitLocalVelocity { get; set; }
    [Networked] public NetworkBool WorldGaitEligible { get; set; }
    [Networked] public NetworkBool WorldPoseClimbing { get; set; }
    private const float MaximumWorldGaitPhase = 4096f;
    private NetworkBehaviour.PropertyReader<float> _gaitPhaseReader, _poseAimPitchReader;
    private NetworkBehaviour.PropertyReader<Vector2> _gaitVelocityReader;
    private NetworkBehaviour.PropertyReader<NetworkBool> _gaitEligibleReader, _poseClimbingReader;
    private NetworkBehaviour.PropertyReader<int> _poseLifeReader, _poseHealthReader;
    private NetworkBehaviour.PropertyReader<bool> _poseDeadReader;
    public bool PresentationCrouched => ReadPresentationCrouch();
    public Vector3 SimulationShotOrigin => transform.TransformPoint(
        _crouchGeometry != null && _crouchGeometry.Configured
            ? _crouchGeometry.EyeLocalPosition(IsCrouching) : Vector3.up * 1.75f);

    private bool ReadPresentationCrouch()
    {
        return TryGetWorldBodyPoseRenderContext(out var context) && context.Crouched;
    }

    void IBeforeHitboxRegistration.BeforeHitboxRegistration()
    {
        if (!_spawned || Object == null || !Object.IsValid || _crouchGeometry == null) return;
        // Preserve the existing authority/current vs proxy/render contract.
        bool simulation = Object.HasStateAuthority || Object.HasInputAuthority;
        WorldBodyPoseContext context;
        bool sampled = simulation ? TryGetCurrentWorldBodyPoseContext(out context)
            : TryGetWorldBodyPoseRenderContext(out context);
        if (!sampled)
        {
            _worldBodyPose?.ApplySourceFallbackAfterStance(WorldBodyPosePass.HitboxCapture);
            return;
        }
        if (context.SourceLifeAligned) _crouchGeometry.Apply(context.Crouched, context.Alive);
        _worldBodyPose?.ApplyAfterStance(context, WorldBodyPosePass.HitboxCapture);
    }

    private bool PoseOwnerValid => _spawned && isActiveAndEnabled && Object != null && Object.IsValid
        && Runner != null && Runner.IsRunning;

    private bool TryGetCurrentWorldBodyPoseContext(out WorldBodyPoseContext context)
    {
        context = default;
        if (!PoseOwnerValid) return false;
        bool alive = !IsDead && Health > 0;
        float aimPitch = AimPitch;
        if (!ValidWorldPoseAimPitch(aimPitch)) return false;
        context = new WorldBodyPoseContext(alive, IsCrouching, WorldPoseClimbing, RespawnVersion,
            WorldGaitPhase, WorldGaitLocalVelocity,
            WorldGaitEligible && alive && !IsCrouching && !WorldPoseClimbing,
            false, Runner.Tick.Raw, Runner.Tick.Raw, 0f, true, aimPitch);
        return true;
    }

    public bool TryGetWorldBodyPoseRenderContext(out WorldBodyPoseContext context)
    {
        context = default;
        if (!PoseOwnerValid) return false;
        // This retains ReadPresentationCrouch's current-state policy for local/
        // state authority. Exact NCC local interpolation alignment needs measurement.
        if (Object.HasStateAuthority || Object.HasInputAuthority)
            return TryGetCurrentWorldBodyPoseContext(out context);
        if (!TryGetSnapshotsBuffers(out var from, out var to, out var alpha)
            || !PlayerInputSource.IsFinite(alpha) || alpha < 0f || alpha > 1f) return false;
        long ticks = (long)to.Tick.Raw - from.Tick.Raw;
        if (ticks < 0 || ticks > Runner.TickRate) return false; // No extrapolated/large-gap gait.
        var selected = alpha < 1f ? from : to;
        int life = _poseLifeReader.Read(selected);
        bool selectedDead = _poseDeadReader.Read(selected);
        int selectedHealth = _poseHealthReader.Read(selected);
        bool alive = !selectedDead && selectedHealth > 0;
        bool sourceLifeAligned = life == RespawnVersion && selectedDead == IsDead && selectedHealth == Health;
        bool crouched = _crouchReader.Read(selected), climbing = _poseClimbingReader.Read(selected);
        bool eligible = _gaitEligibleReader.Read(selected) && alive && !crouched && !climbing;
        float phase = _gaitPhaseReader.Read(selected);
        Vector2 velocity = _gaitVelocityReader.Read(selected);
        float aimPitch = _poseAimPitchReader.Read(selected);
        if (!ValidWorldPoseAimPitch(aimPitch)) return false;
        bool sameLife = _poseLifeReader.Read(from) == _poseLifeReader.Read(to)
            && _poseDeadReader.Read(from) == _poseDeadReader.Read(to)
            && (_poseHealthReader.Read(from) > 0) == (_poseHealthReader.Read(to) > 0);
        if (sameLife)
        {
            float fromPitch = _poseAimPitchReader.Read(from), toPitch = _poseAimPitchReader.Read(to);
            if (!ValidWorldPoseAimPitch(fromPitch) || !ValidWorldPoseAimPitch(toPitch)) return false;
            // Same GameData pair/alpha, independent of gait eligibility/phase validity.
            // Across life/death boundaries retain the existing selected-snapshot policy.
            aimPitch = Mathf.Lerp(fromPitch, toPitch, alpha);
        }
        if (!ValidWorldPoseAimPitch(aimPitch)) return false;
        bool sameMode = sameLife
            && (bool)_crouchReader.Read(from) == (bool)_crouchReader.Read(to)
            && (bool)_poseClimbingReader.Read(from) == (bool)_poseClimbingReader.Read(to)
            && (bool)_gaitEligibleReader.Read(from) == (bool)_gaitEligibleReader.Read(to);
        float fromPhase = _gaitPhaseReader.Read(from), toPhase = _gaitPhaseReader.Read(to);
        float seconds = ticks * Runner.DeltaTime;
        bool validPhasePair = ValidGaitPhase(fromPhase) && ValidGaitPhase(toPhase)
            && toPhase >= fromPhase && toPhase - fromPhase <= 2.5f * seconds + .02f;
        if (sameMode && validPhasePair)
        {
            phase = Mathf.Lerp(fromPhase, toPhase, alpha);
            velocity = Vector2.Lerp(_gaitVelocityReader.Read(from), _gaitVelocityReader.Read(to), alpha);
        }
        else if (sameMode) eligible = false;
        eligible &= ValidGaitPhase(phase) && FiniteGaitVelocity(velocity) && velocity.magnitude <= 8f;
        context = new WorldBodyPoseContext(alive, crouched, climbing, life, phase, velocity,
            eligible, true, from.Tick.Raw, to.Tick.Raw, alpha, sourceLifeAligned, aimPitch);
        return true;
    }

    private static bool ValidWorldPoseAimPitch(float value) => PlayerInputSource.IsFinite(value)
        && value >= -85f && value <= 85f;
    private static bool ValidGaitPhase(float value) => PlayerInputSource.IsFinite(value)
        && value >= 0f && value <= MaximumWorldGaitPhase;
    private static bool FiniteGaitVelocity(Vector2 value) => PlayerInputSource.IsFinite(value.x)
        && PlayerInputSource.IsFinite(value.y);

    private void ResetWorldGaitForLife()
    {
        WorldGaitPhase = 0f; WorldGaitLocalVelocity = Vector2.zero;
        WorldGaitEligible = false; WorldPoseClimbing = false;
    }

    private void DeactivateWorldGait()
    {
        WorldGaitEligible = false; WorldGaitLocalVelocity = Vector2.zero;
        WorldPoseClimbing = !IsDead && Health > 0 && _ladderMotor != null && _ladderMotor.IsClimbing;
    }

    private void UpdateWorldGaitAfterMovement(Vector3 before, bool ladderConsumedTick)
    {
        DeactivateWorldGait();
        float dt = Runner.DeltaTime;
        if (!ValidGaitPhase(WorldGaitPhase) || !PlayerInputSource.IsFinite(dt) || dt <= 0f
            || !NetworkMatchState.AllowsCombatFor(Runner) || IsDead || Health <= 0 || IsCrouching
            || ladderConsumedTick || WorldPoseClimbing || !_movementController.Grounded) return;
        Vector3 delta = transform.position - before;
        Vector3 actualVelocity = _movementController.Velocity;
        if (!PlayerInputSource.IsFinite(delta.x) || !PlayerInputSource.IsFinite(delta.y)
            || !PlayerInputSource.IsFinite(delta.z) || !PlayerInputSource.IsFinite(actualVelocity.x)
            || !PlayerInputSource.IsFinite(actualVelocity.z) || Mathf.Abs(delta.y) > 1.25f) return;
        Vector2 planar = new Vector2(actualVelocity.x, actualVelocity.z);
        Vector2 distance = new Vector2(delta.x, delta.z);
        float speed = planar.magnitude;
        if (speed > 8f || distance.magnitude > 8f * dt + .001f
            || (distance - planar * dt).magnitude > .001f) return;
        Vector3 local = transform.InverseTransformDirection(new Vector3(planar.x, 0f, planar.y));
        Vector2 localVelocity = new Vector2(local.x, local.z);
        if (!FiniteGaitVelocity(localVelocity)) return;
        if (speed <= .15f) localVelocity = Vector2.zero;
        float run = Mathf.InverseLerp(5.1f, 6.8f, speed);
        float cycleDistance = Mathf.Lerp(3.3f, 4.2f, run);
        float nextPhase = WorldGaitPhase + (speed > .15f ? distance.magnitude / cycleDistance : 0f);
        if (!ValidGaitPhase(nextPhase)) return; // Keep phase; no same-life modulo/reset.
        WorldGaitPhase = nextPhase; WorldGaitLocalVelocity = localVelocity; WorldGaitEligible = true;
    }

    private ShadowCastingMode[] _originalShadowModes;
    private bool _spawned;
    private CombatPresentation _presentation;
    private PlayerLocomotionPresentation _locomotionPresentation;
    private PlayerWorldBodyPosePresentation _worldBodyPose;
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
    [Networked] public bool IsDead { get; set; }
    [Networked] public TickTimer RespawnTimer { get; set; }
    [Networked] public TickTimer RespawnRetryTimer { get; set; }
    [Networked] public int RespawnVersion { get; set; }
    [Networked] public int DeathSequence { get; set; }
    [Networked] public PlayerRef LastKiller { get; set; }
    [Networked] public bool LastHitKilled { get; set; }
    public float RespawnRemaining => _spawned && Object != null && Object.IsValid && Runner != null && Runner.IsRunning
        ? RespawnTimer.RemainingTime(Runner) ?? 0f : 0f;
    public bool RespawnPending => _spawned && Object != null && Object.IsValid && IsDead && RespawnRemaining <= 0f;
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
            IsDead = false; IsCrouching = false; RespawnVersion = 0; DeathSequence = 0; LastKiller = PlayerRef.None;
            RespawnTimer = default; RespawnRetryTimer = default; LastHitKilled = false;
            AimYaw = transform.eulerAngles.y;
            AimPitch = 0f;
            RecoilOffset = Vector2.zero;
            RecoilIndex = 0;
            RecoilPreviewAmmo = Ammo;
            RecoilShotTimer = default;
            RecoilReloadTimer = default;
            ResetWorldGaitForLife();
        }
        if (Object.HasInputAuthority)
        {
            _inputSource = GetComponent<PlayerInputSource>();
            if (_inputSource == null) _inputSource = gameObject.AddComponent<PlayerInputSource>();
            _inputSource.Bind(this, AimYaw, AimPitch);
        }
        // Pool/re-entry: CG.Initialize must see original skin bindings, never
        // the previous visual rig. Do this before CG's source-bone registry read.
        _worldBodyPose = GetComponent<PlayerWorldBodyPosePresentation>();
        if (_worldBodyPose != null) _worldBodyPose.Unbind();
        _ladderMotor = GetComponent<PlayerLadderMotor>();
        _crouchReader = GetPropertyReader<NetworkBool>(typeof(GamePlayerNetworkData), nameof(IsCrouching));
        _gaitPhaseReader = GetPropertyReader<float>(typeof(GamePlayerNetworkData), nameof(WorldGaitPhase));
        _poseAimPitchReader = GetPropertyReader<float>(typeof(GamePlayerNetworkData), nameof(AimPitch));
        _gaitVelocityReader = GetPropertyReader<Vector2>(typeof(GamePlayerNetworkData), nameof(WorldGaitLocalVelocity));
        _gaitEligibleReader = GetPropertyReader<NetworkBool>(typeof(GamePlayerNetworkData), nameof(WorldGaitEligible));
        _poseClimbingReader = GetPropertyReader<NetworkBool>(typeof(GamePlayerNetworkData), nameof(WorldPoseClimbing));
        _poseLifeReader = GetPropertyReader<int>(typeof(GamePlayerNetworkData), nameof(RespawnVersion));
        _poseHealthReader = GetPropertyReader<int>(typeof(GamePlayerNetworkData), nameof(Health));
        _poseDeadReader = GetPropertyReader<bool>(typeof(GamePlayerNetworkData), nameof(IsDead));
        _crouchGeometry = GetComponent<PlayerCrouchGeometry>();
        if (_crouchGeometry == null) _crouchGeometry = gameObject.AddComponent<PlayerCrouchGeometry>();
        if (!_crouchGeometry.Initialize(this, _characterController, _cameraRoot))
            Debug.LogError("[Crouch] Public humanoid/12-hitbox stance configuration is incomplete.");
        if (_worldBodyPose == null) _worldBodyPose = gameObject.AddComponent<PlayerWorldBodyPosePresentation>();
        if (!_worldBodyPose.Initialize(this, _crouchGeometry, _bodyRenderers, new WorldRifleBodyPoseSettings()))
            Debug.LogError("[WorldBodyPose] Rifle pose unavailable; original attachments retained.");
        ApplyLocalPresentation();
        _locomotionPresentation = GetComponent<PlayerLocomotionPresentation>();
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
        bool showWeapon = local && !IsDead;
        if (_viewModelRoot != null && _viewModelRoot.gameObject.activeSelf != showWeapon) _viewModelRoot.gameObject.SetActive(showWeapon);
        ApplyLifeCollision();
        if (_bodyRenderers == null || _originalShadowModes == null) return;
        for (int i = 0; i < _bodyRenderers.Length; i++)
            if (_bodyRenderers[i] != null && (_viewModelRoot == null || !_bodyRenderers[i].transform.IsChildOf(_viewModelRoot)))
                _bodyRenderers[i].shadowCastingMode = local ? ShadowCastingMode.ShadowsOnly : _originalShadowModes[i];
    }

    private void ReleaseLocalPresentation()
    {
        _spawned = false;
        if (_worldBodyPose != null) _worldBodyPose.Unbind();
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
        if (_inputSource != null) _inputSource.SynchronizeRespawn();
        ApplyLocalPresentation();
        // One GameData sample supplies both stance and pose, including proxy life.
        if (TryGetWorldBodyPoseRenderContext(out var bodyPoseContext))
        {
            if (bodyPoseContext.SourceLifeAligned)
                _crouchGeometry?.Apply(bodyPoseContext.Crouched, bodyPoseContext.Alive);
            _worldBodyPose?.ApplyAfterStance(bodyPoseContext, WorldBodyPosePass.Render);
        }
        else _worldBodyPose?.ApplySourceFallbackAfterStance(WorldBodyPosePass.Render);
        if (_locomotionPresentation != null) _locomotionPresentation.RenderPresentation(PresentationCrouched, PresentationClimbing);
        if (_cameraRoot != null) _cameraRoot.localRotation = CombatRecoil.ViewRotation(AimPitch, RecoilOffset);
        if (_viewModelRoot != null && Object.HasInputAuthority)
        {
            // ViewModelRoot is a CameraRoot child and inherits its recoil rotation
            // once. Only a small visual kickback is applied in local space.
            _viewModelRoot.localRotation = _viewModelBaseRotation
                * (_locomotionPresentation != null ? _locomotionPresentation.LocalWeaponRotationOffset : Quaternion.identity);
            _viewModelRoot.localPosition = _viewModelBasePosition + Vector3.back * Mathf.Min(Mathf.Max(RecoilOffset.y, 0f) * 0.01f, 0.06f)
                + (_locomotionPresentation != null ? _locomotionPresentation.LocalWeaponPositionOffset : Vector3.zero);
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
        if (!Object.HasStateAuthority && !Object.HasInputAuthority) return;
        if (!MovementConfigured) { DeactivateWorldGait(); return; }
        var match = NetworkMatchState.Instance;
        if (Object.HasStateAuthority && match != null && match.IsCurrent && match.Runner == Runner) match.EvaluateBoundary();
        if (!NetworkMatchState.AllowsCombatFor(Runner))
        {
            if (_ladderMotor != null) _ladderMotor.ResetSimulation();
            _movementController.Velocity = Vector3.zero;
            PreviousButtons = default; ReloadTimer = default; NextShotTimer = default;
            RecoilShotTimer = default; RecoilReloadTimer = default;
            DeactivateWorldGait();
            return;
        }
        var move = Vector2.zero;
        bool sprint = false;
        bool hasInput = GetInput(out CustomPlayerInput input);
        hasInput = hasInput && InputMatchesLife(input.RespawnVersion, RespawnVersion);
        ApplyLifeCollision();
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
        if (IsDead || Health <= 0)
        {
            if (_ladderMotor != null) _ladderMotor.ResetSimulation();
            IsCrouching = false;
            if (_crouchGeometry != null) _crouchGeometry.Apply(false, false);
            _movementController.Velocity = Vector3.zero;
            if (Object.HasStateAuthority) { PreviousButtons = default; ReloadTimer = default; }
            ResetRecoilPreview();
            DeactivateWorldGait();
            if (Object.HasStateAuthority && IsDead) TryRespawn();
            return;
        }
        // Enter only from a standing stance; Ctrl is ignored while attached.
        // A retained crouch under a ceiling still uses CanStand and never force-stands.
        bool alreadyClimbing = _ladderMotor != null && _ladderMotor.IsClimbing;
        bool requestCrouch = !alreadyClimbing && hasInput && input.Buttons.IsSet(PlayerInputButton.Crouch);
        if (_crouchGeometry != null && _crouchGeometry.Configured)
        {
            // Input/state authority run the same prediction and headroom query.
            // Releasing under a solid ceiling retains crouch until it is safe.
            IsCrouching = requestCrouch || (IsCrouching && !_crouchGeometry.CanStand(Runner));
            _crouchGeometry.Apply(IsCrouching, true);
        }
        else IsCrouching = false;
        transform.rotation = Quaternion.Euler(0f, AimYaw, 0f);
        // Exactly one movement path per owner simulation tick, including resimulation.
        Vector3 gaitMovementStart = transform.position;
        bool climbingTick = _ladderMotor != null && _ladderMotor.TrySimulate(input, hasInput, IsCrouching);
        if (!climbingTick)
        {
            // SDK Move normalizes direction, so maxSpeed carries analog magnitude.
            var velocity = _movementController.Velocity;
            if (IsCrouching || velocity.y > 0f)
            {
                if (IsCrouching)
                {
                    var planar = Vector2.ClampMagnitude(new Vector2(velocity.x, velocity.z), PlayerCrouchGeometry.CrouchSpeed);
                    velocity.x = planar.x;
                    velocity.z = planar.y;
                }
                // Ordinary locomotion has no jump mode. Do not carry CC contact lift
                // into the next Move; intentional upward modes need an explicit exemption.
                if (velocity.y > 0f) velocity.y = 0f;
                _movementController.Velocity = velocity;
            }
            _movementController.maxSpeed = (IsCrouching ? PlayerCrouchGeometry.CrouchSpeed : sprint ? 7.5f : 5f) * move.magnitude;
            _movementController.Move(transform.rotation * new Vector3(move.x, 0f, move.y));
        }
        UpdateWorldGaitAfterMovement(gaitMovementStart, climbingTick);
        if (Object.HasStateAuthority) UpdateCombat(hasInput, input);
        else if (Object.HasInputAuthority) PredictRecoil(hasInput, input);
    }

    private void UpdateCombat(bool hasInput, CustomPlayerInput input)
    {
        var manager = NetworkRunnerManager.Instance;
        if (manager == null || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game
            || !NetworkMatchState.AllowsCombatFor(Runner) || !CombatConfigured || Health <= 0)
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
        LastShotOrigin = SimulationShotOrigin;
        LastShotDirection = CombatRecoil.ShotDirection(AimYaw, AimPitch, RecoilOffset);
        QueueLocalShot(LastShotOrigin, LastShotDirection);
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
        LastHitKilled = IsLethalTransition(previousHealth, target.Health, target.IsDead)
            && target.EnterDeath(this, metadata.BodyPart, ShotSequence);
    }

    public static bool IsLethalTransition(int previousHealth, int health, bool dead) => previousHealth > 0 && health <= 0 && !dead;
    public static bool InputMatchesLife(int inputVersion, int serverVersion) => inputVersion == serverVersion;

    private bool EnterDeath(GamePlayerNetworkData killer, CombatBodyPart part, int shotSequence)
    {
        if (!Object.HasStateAuthority || !NetworkMatchState.AllowsCombatFor(Runner) || IsDead || Health > 0 || killer == null || killer == this
            || !killer.Object.HasStateAuthority || killer.Runner != Runner) return false;
        IsDead = true; Death++; DeathSequence++; LastKiller = killer.Object.InputAuthority;
        DeactivateWorldGait();
        killer.Kill++;
        RespawnTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(3f, Runner.TickRate));
        RespawnRetryTimer = default;
        PreviousButtons = default; ReloadTimer = default; NextShotTimer = default;
        ResetRecoilPreview();
        if (_movementController != null) _movementController.Velocity = Vector3.zero;
        ApplyLifeCollision();
        var game = AD.Managers.Instance != null ? AD.Managers.GameM : null;
        if (game != null) game.NotifyKillConfirmed(new KillConfirmedRecord(killer.Object.InputAuthority, Object.InputAuthority,
            killer.Team, Team, part == CombatBodyPart.Head, DeathSequence, shotSequence));
        return true;
    }

    private void ApplyLifeCollision()
    {
        if (!_spawned || Object == null || !Object.IsValid) return;
        if (_hitboxRoot != null) _hitboxRoot.HitboxRootActive = !IsDead;
        if (_characterController != null) _characterController.detectCollisions = !IsDead;
    }

    private void TryRespawn()
    {
        if (!RespawnTimer.Expired(Runner) || !RespawnRetryTimer.ExpiredOrNotRunning(Runner)) return;
        var game = AD.Managers.Instance != null ? AD.Managers.GameM : null;
        if (game == null || !game.TryGetSpawnPose(Team, out var pose, this))
        {
            RespawnRetryTimer = TickTimer.CreateFromTicks(Runner, CombatShotQuery.DurationTicks(0.25f, Runner.TickRate));
            return;
        }
        _movementController.Teleport(pose.position, pose.rotation);
        _movementController.Velocity = Vector3.zero;
        AimYaw = Mathf.Repeat(pose.eulerAngles.y, 360f); AimPitch = 0f;
        Health = 100; Ammo = MagazineCapacity;
        PreviousButtons = default; ReloadTimer = default; NextShotTimer = default;
        ResetRecoilPreview(); RecoilPreviewAmmo = Ammo;
        RespawnTimer = default; RespawnRetryTimer = default;
        LastShotDirection = Vector3.zero;
        RespawnVersion++; IsDead = false; IsCrouching = false;
        if (_ladderMotor != null) _ladderMotor.ResetSimulation();
        ResetWorldGaitForLife();
        ApplyLifeCollision();
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
        if (manager == null || manager.IsRetired || manager.SessionPhase != NetworkSessionPhase.Game
            || !NetworkMatchState.AllowsCombatFor(Runner) || !CombatConfigured)
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
        if (!CombatShotQuery.IsFinite(transform.position)) return;
        RecoilPreviewAmmo--;
        ApplyRecoilShot();
        QueueLocalShot(SimulationShotOrigin,
            CombatRecoil.ShotDirection(AimYaw, AimPitch, RecoilOffset));
        // No actual Ammo/HP/ShotSequence changes or hit query. This queues a
        // cosmetic observation only; Unity effects remain in RenderState.
    }

    private void QueueLocalShot(Vector3 origin, Vector3 direction)
    {
        // Prediction state still updates during resimulation. Irreversible
        // audio/VFX observations are only queued for a fresh forward tick.
        if (!Object.HasInputAuthority || !Runner.IsForward || _presentation == null) return;
        _presentation.QueueLocalShot(RespawnVersion, Runner.Tick.Raw, origin, direction);
    }

    public bool TryGetCombatSnapshot(out CombatStateSnapshot snapshot)
    {
        snapshot = default;
        if (!_spawned || Object == null || !Object.IsValid || Runner == null || !Runner.IsRunning) return false;
        snapshot = new CombatStateSnapshot(Object.Id.ToString(), Object.InputAuthority, Object.HasStateAuthority, Runner.Tick.Raw,
            Health, Ammo, ShotSequence, HitSequence, ReloadTimer.TargetTick, LastHitBodyPart, LastHitDamage, LastHitTarget,
            IsDead, RespawnRemaining, RespawnPending, RespawnVersion, Kill, Death, DeathSequence, LastKiller, LastHitKilled);
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
            || snapshot.ReloadTargetTick != _diagnosticSnapshot.ReloadTargetTick || snapshot.IsDead != _diagnosticSnapshot.IsDead
            || snapshot.RespawnVersion != _diagnosticSnapshot.RespawnVersion || snapshot.Kill != _diagnosticSnapshot.Kill
            || snapshot.Death != _diagnosticSnapshot.Death || snapshot.RespawnPending != _diagnosticSnapshot.RespawnPending;
        if (!changed) return;
        string stage = _hasDiagnosticSnapshot ? "changed" : "initial";
        Debug.Log($"[CombatState] {stage} id={snapshot.NetworkId} input={snapshot.InputAuthority} stateAuthority={snapshot.IsStateAuthority} tick={snapshot.Tick} hp={snapshot.Health} ammo={snapshot.Ammo} reloadEnd={snapshot.ReloadTargetTick} shot={snapshot.ShotSequence} hit={snapshot.HitSequence} part={snapshot.LastHitBodyPart} damage={snapshot.LastHitDamage} target={snapshot.LastHitTarget} recoilIndex={RecoilIndex} recoil={RecoilOffset} previewAmmo={RecoilPreviewAmmo} dead={IsDead} respawnVersion={RespawnVersion} respawnRemaining={RespawnRemaining} pending={RespawnPending} kd={Kill}/{Death} deathSeq={DeathSequence} killer={LastKiller} hitKilled={LastHitKilled}");
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
    public readonly bool IsDead, RespawnPending, LastHitKilled;
    public readonly float RespawnRemaining;
    public readonly int RespawnVersion, Kill, Death, DeathSequence;
    public readonly PlayerRef LastKiller;
    public CombatStateSnapshot(string networkId, PlayerRef inputAuthority, bool isStateAuthority, int tick, int health, int ammo,
        int shotSequence, int hitSequence, int? reloadTargetTick, CombatBodyPart part, int damage, PlayerRef target,
        bool dead = false, float respawnRemaining = 0f, bool respawnPending = false, int respawnVersion = 0,
        int kill = 0, int death = 0, int deathSequence = 0, PlayerRef lastKiller = default, bool killed = false)
    {
        NetworkId = networkId; InputAuthority = inputAuthority; IsStateAuthority = isStateAuthority; Tick = tick;
        Health = health; Ammo = ammo; ShotSequence = shotSequence; HitSequence = hitSequence; ReloadTargetTick = reloadTargetTick;
        LastHitBodyPart = part; LastHitDamage = damage; LastHitTarget = target;
        IsDead = dead; RespawnRemaining = respawnRemaining; RespawnPending = respawnPending; RespawnVersion = respawnVersion;
        Kill = kill; Death = death; DeathSequence = deathSequence; LastKiller = lastKiller; LastHitKilled = killed;
    }
}
