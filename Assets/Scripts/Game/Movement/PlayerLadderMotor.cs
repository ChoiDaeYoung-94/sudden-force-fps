using Fusion;
using UnityEngine;

// Owner calls TrySimulate exactly once instead of ordinary NCC.Move on a climb tick.
// No own FixedUpdateNetwork: the existing owner remains the movement dispatcher.
[DisallowMultipleComponent]
public sealed class PlayerLadderMotor : NetworkBehaviour
{
    // Unit-circle diagonals have a lateral magnitude of about .707.
    private const float SideInputThreshold = .5f;

    [Networked] public bool IsClimbing { get; set; }
    [Networked] public int LadderId { get; set; }
    [Networked] public int ClimbLifeVersion { get; set; }
    // 0 rail, 1 lift before crossing roof edge, 2 cross to landing, 3 top entry.
    [Networked] public int ExitStage { get; set; }
    [Networked] public TickTimer ReattachDelay { get; set; }
    [SerializeField, Min(.1f)] private float _climbSpeed = 2.2f;
    [SerializeField, Min(.1f)] private float _alignSpeed = 2.5f;
    private GamePlayerNetworkData _owner;
    private NetworkBehaviour.PropertyReader<bool> _climbReader;
    public bool PresentationClimbing
    {
        get
        {
            if (Object == null || !Object.IsValid) return false;
            if (!Object.HasInputAuthority && !Object.HasStateAuthority
                && TryGetSnapshotsBuffers(out var from, out var to, out var alpha))
                return alpha < 1f ? _climbReader.Read(from) : _climbReader.Read(to);
            return IsClimbing;
        }
    }
    private CharacterController _cc;
    private NetworkCharacterController _ncc;
    private readonly Collider[] _overlaps = new Collider[64];
    private readonly RaycastHit[] _sweeps = new RaycastHit[64];

    public override void Spawned()
    {
        _climbReader = GetPropertyReader<bool>(typeof(PlayerLadderMotor), nameof(IsClimbing));
        _owner = GetComponent<GamePlayerNetworkData>();
        _cc = GetComponent<CharacterController>();
        _ncc = GetComponent<NetworkCharacterController>();
        if (Object.HasStateAuthority)
        {
            IsClimbing = false; LadderId = 0; ExitStage = 0; ReattachDelay = default;
            ClimbLifeVersion = _owner != null ? _owner.RespawnVersion : 0;
        }
    }
    public bool TrySimulate(CustomPlayerInput input, bool hasInput, bool crouched)
    {
        if (Object == null || !Object.IsValid || _owner == null || _cc == null || _ncc == null
            || (!Object.HasStateAuthority && !Object.HasInputAuthority)) return false;
        // A disabled, already-detached motor must not erase normal NCC velocity each tick.
        if (!isActiveAndEnabled) { if (IsClimbing) ResetSimulation(); return false; }
        // This guard must not survive resimulation of the same tick: only
        // owner dispatch enforces exactly one call, not a local tick cursor.
        if (_owner.IsDead || _owner.Health <= 0 || !NetworkMatchState.AllowsCombatFor(Runner))
        { ResetSimulation(); return false; }
        bool wasClimbing = IsClimbing;
        if (ClimbLifeVersion != _owner.RespawnVersion)
        {
            ResetSimulation(); ClimbLifeVersion = _owner.RespawnVersion;
            return false;
        }
        if (!hasInput || !GamePlayerNetworkData.InputMatchesLife(input.RespawnVersion, _owner.RespawnVersion)
            || !PlayerInputSource.IsFinite(input.MoveX) || !PlayerInputSource.IsFinite(input.MoveZ) || crouched)
        {
            if (wasClimbing) Detach();
            return wasClimbing;
        }
        var scene = gameObject.scene;
        var physics = PhysicsSceneExtensions.GetPhysicsScene(scene);
        if (!physics.IsValid()) { if (wasClimbing) Detach(); return wasClimbing; }
        var volumes = LadderVolume.InScene(scene);
        LadderVolume ladder = null;
        foreach (var volume in volumes) if (volume.Id == LadderId) ladder = volume;
        if (IsClimbing && ladder == null) { Detach(); return true; }
        if (!IsClimbing)
        {
            if (!ReattachDelay.ExpiredOrNotRunning(Runner) || Mathf.Abs(input.MoveX) >= SideInputThreshold) return false;
            foreach (var volume in volumes)
            {
                bool bottom = volume.BottomEntry(transform.position, _owner.AimYaw, input.MoveZ);
                bool top = volume.TryLanding(physics, out var landing) && volume.TopEntry(transform.position, landing, _owner.AimYaw, input.MoveZ);
                if (!bottom && !top) continue;
                Vector3 entryTarget = volume.RailAt(top ? Mathf.Max(volume.Top.y + .10f, landing.y + .05f) : transform.position.y);
                if (!Clear(physics, entryTarget) || !SweepClear(physics, transform.position, entryTarget)) continue;
                IsClimbing = true; LadderId = volume.Id; ExitStage = top ? 3 : 0;
                ClimbLifeVersion = _owner.RespawnVersion; ladder = volume;
                break;
            }
            if (ladder == null) return false;
        }
        if (Mathf.Abs(input.MoveX) >= SideInputThreshold)
        {
            // Releasing the ladder changes movement mode; ordinary gravity
            // resumes next tick. No sideways teleport or collision disable.
            Detach(); return true;
        }
        float dt = Runner.DeltaTime;
        if (!PlayerInputSource.IsFinite(dt) || dt <= 0f) { Detach(); return true; }
        Vector3 before = transform.position;
        Vector3 target;
        float forward = Mathf.Clamp(input.MoveZ, -1f, 1f);
        bool landingValid = ladder.TryLanding(physics, out var exit) && Clear(physics, exit);
        if (ExitStage == 3)
        {
            target = ladder.RailAt(Mathf.Max(ladder.Top.y + .10f, landingValid ? exit.y + .05f : ladder.Top.y + .10f));
            if (!SweepClear(physics, before, target)) { Detach(); return true; }
            if ((before - target).sqrMagnitude < .04f * .04f) ExitStage = 0;
        }
        else if (ExitStage == 1 || ExitStage == 2)
        {
            if (forward < -.2f) ExitStage = 0;
            if (!landingValid) ExitStage = 0;
            if (ExitStage == 1)
            {
                target = ladder.RailAt(Mathf.Max(ladder.Top.y + .10f, exit.y + .05f));
                if ((before - target).sqrMagnitude < .04f * .04f) ExitStage = 2;
            }
            else target = before;
            if (ExitStage == 2)
            {
                target = exit;
                if (!SweepClear(physics, before, target)) { ExitStage = 0; target = before; }
                else if ((before - exit).sqrMagnitude < .06f * .06f) { Detach(); return true; }
            }
        }
        else
        {
            float nextY = Mathf.Clamp(before.y + forward * _climbSpeed * dt, ladder.Bottom.y, ladder.Top.y + .10f);
            target = ladder.RailAt(nextY);
            if (forward < -.2f && before.y <= ladder.Bottom.y + .05f)
            { Detach(); return true; }
            if (forward > .2f && before.y >= ladder.Top.y - .06f && landingValid)
            { ExitStage = 1; target = ladder.RailAt(Mathf.Max(ladder.Top.y + .10f, exit.y + .05f)); }
        }
        // Fail closed for an overhang during the lift phase too, including
        // the tick that first transitions from rail to lift.
        if (ExitStage == 1 && !SweepClear(physics, before, target))
        {
            ExitStage = 0;
            target = before;
        }
        Vector3 delta = target - before;
        float maxMove = Mathf.Max(_alignSpeed, _climbSpeed) * dt;
        delta = Vector3.ClampMagnitude(delta, maxMove);
        // All geometry movement is collision-constrained; NCC's normal Move
        // (and gravity) is skipped for this consumed owner tick.
        _cc.Move(delta);
        _ncc.Velocity = (transform.position - before) / dt;
        _ncc.Grounded = _cc.isGrounded;
        return true;
    }
    public void ResetSimulation()
    {
        if (Object == null || !Object.IsValid || (!Object.HasStateAuthority && !Object.HasInputAuthority)) return;
        IsClimbing = false; LadderId = 0; ExitStage = 0; ReattachDelay = default;
        if (_owner != null) ClimbLifeVersion = _owner.RespawnVersion;
        if (_ncc != null) _ncc.Velocity = Vector3.zero;
    }
    private void Detach()
    {
        IsClimbing = false; LadderId = 0; ExitStage = 0;
        ReattachDelay = TickTimer.CreateFromSeconds(Runner, .3f);
        _ncc.Velocity = Vector3.zero;
        _ncc.Grounded = _cc.isGrounded;
    }
    private void Capsule(Vector3 feet, out Vector3 bottom, out Vector3 top, out float radius)
    {
        // The existing player keeps feet origin and upright unit scale.
        radius = Mathf.Max(.01f, _cc.radius);
        float half = Mathf.Max(_cc.height * .5f, _cc.radius);
        var center = feet + _cc.center + Vector3.up * .04f;
        float segment = Mathf.Max(0f, half - _cc.radius);
        bottom = center - Vector3.up * segment;
        top = center + Vector3.up * segment;
    }
    private bool Clear(PhysicsScene physics, Vector3 feet)
    {
        Capsule(feet, out var bottom, out var top, out float radius);
        int count = physics.OverlapCapsule(bottom, top, radius, _overlaps, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (count >= _overlaps.Length) return false;
        for (int i = 0; i < count; i++) if (!Ignore(_overlaps[i])) return false;
        return true;
    }
    private bool SweepClear(PhysicsScene physics, Vector3 start, Vector3 end)
    {
        if (!Clear(physics, end)) return false;
        var delta = end - start;
        float distance = delta.magnitude;
        if (distance < .001f) return true;
        Capsule(start, out var bottom, out var top, out float radius);
        int count = physics.CapsuleCast(bottom, top, radius, delta / distance, _sweeps, distance,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (count >= _sweeps.Length) return false;
        for (int i = 0; i < count; i++) if (!Ignore(_sweeps[i].collider)) return false;
        return true;
    }
    private bool Ignore(Collider collider) => collider == null || collider.transform == transform
        || collider.transform.IsChildOf(transform) || PlayerSpawnSelector.ShouldIgnoreCollider(collider, _owner);
}

