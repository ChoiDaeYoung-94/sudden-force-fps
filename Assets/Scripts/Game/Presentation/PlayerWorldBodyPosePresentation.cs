using System;
using UnityEngine;

public enum WorldBodyPosePass { HitboxCapture, Render, LateRender }

// One current simulation sample or one shared GameData render-buffer pair.
public readonly struct WorldBodyPoseContext
{
    public readonly bool Alive, Crouched, Climbing, GaitEligible, UsesRemoteSnapshots, SourceLifeAligned;
    public readonly int RespawnVersion, FromTick, ToTick;
    public readonly float UnwrappedPhase, Alpha;
    public readonly Vector2 LocalVelocity;
    // Existing rifle fixtures stay rifle-only until given an explicit gait sample.
    public WorldBodyPoseContext(bool alive, bool crouched, bool climbing, int respawnVersion)
        : this(alive, crouched, climbing, respawnVersion, 0f, Vector2.zero, false, false, 0, 0, 0f) { }
    public WorldBodyPoseContext(bool alive, bool crouched, bool climbing, int respawnVersion,
        float phase, Vector2 velocity, bool eligible, bool remote, int fromTick, int toTick, float alpha, bool sourceLifeAligned = true)
    {
        Alive = alive; Crouched = crouched; Climbing = climbing; RespawnVersion = respawnVersion;
        UnwrappedPhase = phase; LocalVelocity = velocity; GaitEligible = eligible;
        UsesRemoteSnapshots = remote; FromTick = fromTick; ToTick = toTick; Alpha = alpha;
        SourceLifeAligned = sourceLifeAligned;
    }
}

// One owner, one visual skeleton. Call only after PlayerCrouchGeometry.Apply.
// Existing source Animator, skin renderers, 7 gun renderers and simulation root
// retain their existing ownership. Only GameData writes the new protocol-v2 gait state.
[DisallowMultipleComponent]
[DefaultExecutionOrder(6000)]
public sealed class PlayerWorldBodyPosePresentation : MonoBehaviour
{
    private GamePlayerNetworkData _owner;
    private PlayerCrouchGeometry _stance;
    private Renderer[] _bodyRendererCache;
    private WorldRifleBodyPoseSettings _settings;
    private bool _hasInitialization, _faulted, _restoreIncomplete, _factoryEntered;
    private WorldBodyVisualRigBinding _rig;
    private WorldBodyGaitLayer _gait;
    public string GaitStatus { get; private set; }
    public int GaitFaultCount { get; private set; }
    public int LifeBoundaryHoldCount { get; private set; }
    private WorldBodyPoseAttachmentBinding _attachments;
    private Transform[] _sourceParents;
    private Vector3[] _sourcePositions, _sourceScales;
    private Quaternion[] _sourceRotations;
    private SkinnedMeshRenderer[] _skins;
    private Transform[][] _originalSkinBones;
    private Transform[] _originalSkinRoots;

    public bool Ready => !_faulted && _rig != null && _attachments != null;
    public string LastFailure { get; private set; }
    public string LastStatus { get; private set; }
    public WorldBodyPosePass LastPass { get; private set; }
    public WorldBodyPoseContext LastContext { get; private set; }
    public float RightReachRatio { get; private set; }
    public float LeftReachRatio { get; private set; }
    public float RightWristError { get; private set; }
    public float LeftWristError { get; private set; }

    public bool Initialize(GamePlayerNetworkData owner, PlayerCrouchGeometry stance,
        Renderer[] originalBodyRendererCache, WorldRifleBodyPoseSettings settings = null)
    {
        Unbind();
        if (_restoreIncomplete) return false; // A failed restoration cannot become a new baseline.
        _owner = owner; _stance = stance;
        try
        {
            if (owner == null || owner.transform != transform || !owner.isActiveAndEnabled || stance == null
                || stance.transform != transform || !stance.Configured || !stance.isActiveAndEnabled || !isActiveAndEnabled
                || originalBodyRendererCache == null)
                throw new InvalidOperationException("BODY_POSE_EXPLICIT_STANCE_INITIALIZATION_REQUIRED");
            _settings = (settings ?? new WorldRifleBodyPoseSettings()).Freeze();
            // Copy the references, not renderers or their render/shadow state.
            _bodyRendererCache = (Renderer[])originalBodyRendererCache.Clone();
            _hasInitialization = true;
            BindRuntime();
            return true;
        }
        catch (Exception error) { FailClosed(error); return false; }
    }

    private void BindRuntime()
    {
        if (_owner == null || !_owner.isActiveAndEnabled || _stance == null || !_stance.Configured
            || !_stance.isActiveAndEnabled || !_hasInitialization
            || _owner.Object == null || !_owner.Object.IsValid || _owner.Runner == null || !_owner.Runner.IsRunning)
            throw new InvalidOperationException("BODY_POSE_LIVE_OWNER_REQUIRED");
        // The old optional gait candidate creates its own rig. It cannot coexist
        // with this central owner. Existing PlayerLocomotionPresentation is only
        // audio/local weapon bob and remains supported.
        if (GetComponent("PlayerWorldBodyLocomotionPresentation") != null
            || transform.Find(WorldBodyVisualRigBinding.RootName) != null)
            throw new InvalidOperationException("BODY_POSE_SINGLE_VISUAL_RIG_OWNER_REQUIRED");
        CaptureOriginalSkinBindings();
        _factoryEntered = true;
        _rig = WorldBodyVisualRigBinding.Create(_owner);
        _factoryEntered = false;
        _rig.Animator.enabled = false;
        // Missing/unapproved clips deliberately retain rifle-only operation.
        WorldBodyGaitLayer.TryCreate(_rig, out _gait, out var gaitStatus);
        GaitStatus = gaitStatus;
        _attachments = new WorldBodyPoseAttachmentBinding(_owner, _rig, _bodyRendererCache);
        int count = _rig.SourceBones.Length;
        _sourceParents = new Transform[count]; _sourcePositions = new Vector3[count];
        _sourceRotations = new Quaternion[count]; _sourceScales = new Vector3[count];
        for (int i = 0; i < count; i++) _sourceParents[i] = _rig.SourceBones[i].parent;
        _faulted = false; LastFailure = null; LastStatus = "bound-awaiting-stance-pass";
    }

    private void CaptureOriginalSkinBindings()
    {
        if (_bodyRendererCache == null || _bodyRendererCache.Length != 15)
            throw new InvalidOperationException("BODY_POSE_ORIGINAL_RENDERER_CACHE_REQUIRED");
        int count = 0;
        foreach (var renderer in _bodyRendererCache) if (renderer is SkinnedMeshRenderer) count++;
        if (count != 8) throw new InvalidOperationException("BODY_POSE_ORIGINAL_8_SKINS_REQUIRED");
        _skins = new SkinnedMeshRenderer[count]; _originalSkinBones = new Transform[count][];
        _originalSkinRoots = new Transform[count];
        int index = 0;
        foreach (var renderer in _bodyRendererCache)
            if (renderer is SkinnedMeshRenderer skin)
            {
                _skins[index] = skin; _originalSkinBones[index] = skin.bones;
                _originalSkinRoots[index] = skin.rootBone; index++;
            }
    }

    public bool ApplyAfterStance(WorldBodyPoseContext context, WorldBodyPosePass pass)
    {
        if (!isActiveAndEnabled || !Ready) return false;
        try
        {
            if (_owner == null || _owner.Object == null || !_owner.Object.IsValid
                || !_owner.isActiveAndEnabled || _owner.Runner == null || !_owner.Runner.IsRunning
                || _stance == null || !_stance.Configured || !_stance.isActiveAndEnabled)
                throw new InvalidOperationException("BODY_POSE_OWNER_OR_STANCE_LOST");
            LastContext = context; LastPass = pass;
            BeginPose();
            if (!context.SourceLifeAligned)
            {
                // Existing source life Animator stays untouched. This transient
                // render boundary neither faults/unbinds nor resets gait phase.
                VerifySourceBonesUnchanged(); LifeBoundaryHoldCount++;
                GaitStatus = "source-only-life-boundary";
                LastStatus = "source-only-life-boundary-original-attachments";
                return true;
            }
            if (!context.Alive)
            {
                // Source life Animator owns death. Copy it without rifle IK or
                // attachment mapping; the restored original attachments follow it.
                VerifySourceBonesUnchanged();
                LastStatus = "source-death-pose";
                return true;
            }

            // Exactly one BeginPose copy. Gait is absolute/manual on this same rig.
            if (_gait != null)
            {
                bool gaitFault;
                _gait.EvaluateAfterCopy(context, out var gaitStatus, out gaitFault);
                GaitStatus = gaitStatus;
                if (gaitFault) { GaitFaultCount++; _gait = null; }
            }
            Vector3 desiredPosition = _attachments.RightUpperArm.position
                + transform.forward * _settings.Forward - transform.up * _settings.Down
                - transform.right * _settings.Inward;
            Quaternion desiredRotation = transform.rotation * Quaternion.Euler(_settings.RifleEuler);
            ApplyRifleArms(desiredPosition, desiredRotation);
            VerifySourceBonesUnchanged();
            _attachments.Commit(desiredPosition, desiredRotation);
            LastStatus = "rifle-and-12-hitboxes-committed";
            return true;
        }
        catch (Exception error) { FailClosed(error); return false; }
    }

    // Begin(copy), layers, commit are deliberately separate. Source transforms
    // are read once per pass after stance, never restored from an old render pose.
    private void BeginPose()
    {
        _attachments.ValidateStableContract();
        _attachments.RestoreLocals();
        for (int i = 0; i < _rig.SourceBones.Length; i++)
        {
            var source = _rig.SourceBones[i]; var visual = _rig.VisualBones[i];
            if (source == null || visual == null || source.parent != _sourceParents[i]
                || !BodyPoseMath.Finite(source.localPosition) || !BodyPoseMath.Finite(source.localRotation)
                || !BodyPoseMath.Unit(source.lossyScale))
                throw new InvalidOperationException("BODY_POSE_SOURCE_BONE_CONTRACT_CHANGED:" + i);
            _sourcePositions[i] = source.localPosition; _sourceRotations[i] = source.localRotation;
            _sourceScales[i] = source.localScale;
        }
        _rig.CopySourcePose();
    }

    private void VerifySourceBonesUnchanged()
    {
        for (int i = 0; i < _rig.SourceBones.Length; i++)
        {
            var source = _rig.SourceBones[i];
            if (source == null || source.parent != _sourceParents[i]
                || !source.localPosition.Equals(_sourcePositions[i])
                || !source.localRotation.Equals(_sourceRotations[i])
                || !source.localScale.Equals(_sourceScales[i]))
                throw new InvalidOperationException("BODY_POSE_LAYER_WROTE_SOURCE_BONE:" + i);
        }
    }

    private void ApplyRifleArms(Vector3 position, Quaternion rotation)
    {
        Vector3 rightTarget = position + rotation * (WorldRifleGripCalibration.RightPosition + _settings.RightGripOffset);
        Vector3 leftTarget = position + rotation * (WorldRifleGripCalibration.LeftPosition + _settings.LeftGripOffset);
        Quaternion rightRotation = (rotation * WorldRifleGripCalibration.RightRotation
            * Quaternion.Euler(_settings.RightGripEuler)).normalized;
        Quaternion leftRotation = (rotation * WorldRifleGripCalibration.LeftRotation
            * Quaternion.Euler(_settings.LeftGripEuler)).normalized;
        // These changes affect only the owned visual rig. No attachment is
        // committed if either arm is unreachable or any residual fails.
        SolveArm(_attachments.RightUpperArm, _attachments.RightLowerArm, _attachments.RightHand,
            rightTarget, rightRotation, transform.right, out float rightReach, out float rightError);
        SolveArm(_attachments.LeftUpperArm, _attachments.LeftLowerArm, _attachments.LeftHand,
            leftTarget, leftRotation, -transform.right, out float leftReach, out float leftError);
        RightReachRatio = rightReach; LeftReachRatio = leftReach;
        RightWristError = rightError; LeftWristError = leftError;
    }

    private void SolveArm(Transform upper, Transform lower, Transform hand, Vector3 target,
        Quaternion targetRotation, Vector3 outward, out float reachRatio, out float wristError)
    {
        reachRatio = 0f; wristError = 0f;
        if (upper == null || lower == null || hand == null || lower.parent != upper || hand.parent != lower
            || !upper.IsChildOf(_rig.Root.transform) || !BodyPoseMath.Finite(target)
            || !BodyPoseMath.Finite(targetRotation)) throw new InvalidOperationException("RIFLE_ARM_CHAIN_OR_TARGET_INVALID");
        float a = Vector3.Distance(upper.position, lower.position);
        float b = Vector3.Distance(lower.position, hand.position);
        Vector3 delta = target - upper.position; float distance = delta.magnitude;
        float maximum = a + b, minimum = Mathf.Abs(a - b);
        if (!BodyPoseMath.Finite(a) || !BodyPoseMath.Finite(b) || !BodyPoseMath.Finite(distance)
            || a <= .001f || b <= .001f || distance <= minimum + .0001f || distance >= maximum * .95f)
            throw new InvalidOperationException("RIFLE_ARM_UNREACHABLE_NO_STRETCH");
        reachRatio = distance / maximum;
        Vector3 axis = delta / distance;
        Vector3 rawPole = outward * .9f - transform.up * .85f - transform.forward * .15f;
        Vector3 pole = rawPole - axis * Vector3.Dot(rawPole, axis);
        if (!BodyPoseMath.Finite(pole) || pole.sqrMagnitude <= .000001f)
            throw new InvalidOperationException("RIFLE_ARM_POLE_DEGENERATE");
        pole.Normalize();
        float along = (a * a - b * b + distance * distance) / (2f * distance);
        float heightSquared = a * a - along * along;
        float cosine = (a * a + b * b - distance * distance) / (2f * a * b);
        if (!BodyPoseMath.Finite(heightSquared) || heightSquared < 0f || !BodyPoseMath.Finite(cosine)
            || cosine < -1f || cosine > 1f) throw new InvalidOperationException("RIFLE_ARM_TRIANGLE_INVALID");
        Vector3 elbow = upper.position + axis * along + pole * Mathf.Sqrt(heightSquared);
        upper.rotation = Quaternion.FromToRotation(lower.position - upper.position, elbow - upper.position) * upper.rotation;
        lower.rotation = Quaternion.FromToRotation(hand.position - lower.position, target - lower.position) * lower.rotation;
        hand.rotation = targetRotation;
        wristError = Vector3.Distance(hand.position, target);
        float angleError = Quaternion.Angle(hand.rotation, targetRotation);
        if (!BodyPoseMath.Finite(wristError) || !BodyPoseMath.Finite(angleError)
            || wristError > .0005f || angleError > .05f)
            throw new InvalidOperationException("RIFLE_ARM_RESIDUAL_FAILED");
    }

    private void LateUpdate()
    {
        // CG order5000 resets source stance and attachments even after Render.
        // Recompute using current owner flags, never cached Render delta/time.
        if (!Ready) return;
        if (_owner != null && _owner.TryGetWorldBodyPoseRenderContext(out var context))
            ApplyAfterStance(context, WorldBodyPosePass.LateRender);
        else ApplySourceFallbackAfterStance(WorldBodyPosePass.LateRender);
    }

    // Snapshot warm-up/gap is visible and does not permanently fault a live rig.
    // Restore mapped children and copy current source once; no stale gait commit.
    public void ApplySourceFallbackAfterStance(WorldBodyPosePass pass)
    {
        if (!isActiveAndEnabled || !Ready) return;
        try
        {
            if (_owner == null || _owner.Object == null || !_owner.Object.IsValid
                || !_owner.isActiveAndEnabled || _owner.Runner == null || !_owner.Runner.IsRunning
                || _stance == null || !_stance.Configured || !_stance.isActiveAndEnabled)
                throw new InvalidOperationException("BODY_POSE_OWNER_OR_STANCE_LOST");
            BeginPose(); VerifySourceBonesUnchanged(); LastPass = pass; LastContext = default;
            LastStatus = "awaiting-pose-snapshot-original-attachments";
        }
        catch (Exception error) { FailClosed(error); }
    }

    private void OnEnable()
    {
        if (!_hasInitialization) return; // AddComponent has not been initialized.
        if (_restoreIncomplete) return;
        try { BindRuntime(); }
        catch (Exception error) { FailClosed(error); }
    }

    private void OnDisable()
    {
        string failure = ReleaseRuntime();
        if (failure != null)
        {
            LastFailure = failure;
            Debug.LogError("[WorldBodyPose] Disable restoration incomplete: " + failure, this);
        }
        else if (!_faulted) LastStatus = "disabled-original-bindings-restored";
    }
    private void OnDestroy() => Unbind();

    private void FailClosed(Exception error)
    {
        _faulted = true; LastFailure = error.GetType().Name + ": " + error.Message;
        LastStatus = "failed-restoring-original-bindings";
        string cleanupFailure = ReleaseRuntime();
        if (cleanupFailure != null) LastFailure += " | cleanup: " + cleanupFailure;
        Debug.LogError("[WorldBodyPose] " + LastFailure, this);
    }

    // Attempts every restore even if an unrelated object was destroyed. No
    // source skeleton write, shape write, renderer enable/shadow write or reparent.
    private string ReleaseRuntime()
    {
        string failure = null;
        try { _gait?.Dispose(); }
        catch (Exception error) { failure = error.Message; }
        finally { _gait = null; }
        try { _attachments?.Dispose(); }
        catch (Exception error) { failure = (failure == null ? "" : failure + " | ") + error.Message; }
        finally { _attachments = null; }
        var oldRig = _rig; _rig = null;
        try { oldRig?.Dispose(); }
        catch (Exception error) { failure = (failure == null ? "" : failure + " | ") + error.Message; }
        if (_skins != null)
            for (int i = 0; i < _skins.Length; i++)
                try
                {
                    if (_skins[i] == null) continue;
                    _skins[i].bones = _originalSkinBones[i]; _skins[i].rootBone = _originalSkinRoots[i];
                }
                catch (Exception error) { failure = (failure == null ? "" : failure + " | ") + error.Message; }
        // The dependency normally detaches/destroys its own root. This covers an
        // exception in a skin setter before its final cleanup, using only our ref.
        if (oldRig != null && oldRig.Root != null && oldRig.Root.transform.parent == transform)
            try
            {
                oldRig.Root.SetActive(false); oldRig.Root.transform.SetParent(null, false);
                if (Application.isPlaying) Destroy(oldRig.Root); else DestroyImmediate(oldRig.Root);
            }
            catch (Exception error) { failure = (failure == null ? "" : failure + " | ") + error.Message; }
        // A factory exception normally destroys its unfinished root. If its own
        // cleanup threw before returning a reference, do not adopt/delete an
        // object by name. Restore cached skins and report the unresolved root.
        if (_factoryEntered && transform.Find(WorldBodyVisualRigBinding.RootName) != null)
            failure = (failure == null ? "" : failure + " | ") + "BODY_POSE_FACTORY_ROOT_CLEANUP_UNCONFIRMED";
        _factoryEntered = false;
        _skins = null; _originalSkinBones = null; _originalSkinRoots = null;
        _sourceParents = null; _sourcePositions = null; _sourceRotations = null; _sourceScales = null;
        RightReachRatio = LeftReachRatio = RightWristError = LeftWristError = 0f;
        if (failure != null) { _faulted = true; _restoreIncomplete = true; LastStatus = "restore-incomplete"; }
        return failure;
    }

    public void Unbind()
    {
        string cleanupFailure = ReleaseRuntime();
        _hasInitialization = false; _owner = null; _stance = null; _bodyRendererCache = null; _settings = null;
        _faulted = _restoreIncomplete;
        if (cleanupFailure != null) LastFailure = cleanupFailure;
        else if (!_restoreIncomplete) LastFailure = null;
        LastStatus = _restoreIncomplete ? "restore-incomplete" : "unbound-original-bindings-restored";
    }
}
