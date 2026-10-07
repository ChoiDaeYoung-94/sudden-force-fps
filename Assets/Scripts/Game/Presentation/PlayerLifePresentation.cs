using UnityEngine;

public sealed class PlayerLifePresentation : MonoBehaviour
{
    [SerializeField] private GamePlayerNetworkData _owner;
    [SerializeField] private Animator _animator;
    private CombatPresentation _presentation;
    private bool _hasSnapshot, _dead;
    private int _version;
    private HumanPoseHandler _poseHandler;
    private HumanPose _pose;
    private static readonly int DeathState = Animator.StringToHash("Base Layer.Death_Front");
    private static readonly int AliveState = Animator.StringToHash("Base Layer.Idle_Aiming");

    private void OnEnable()
    {
        CombatPresentation.PresentationBound += Bind;
        CombatPresentation.PresentationUnbound += Unbind;
        if (_owner != null) Bind(_owner.GetComponent<CombatPresentation>());
    }

    private void Bind(CombatPresentation presentation)
    {
        if (presentation == null || presentation.Owner != _owner) return;
        if (!ReferenceEquals(_presentation, presentation)) Clear();
        _presentation = presentation;
        ApplySnapshot();
    }

    // Remote players do not emit LocalVitalsChanged. Read their copied render
    // snapshot too, and only start an animation at a life/version transition.
    private void LateUpdate()
    {
        ApplySnapshot();
        GroundDeathPose();
    }

    private void GroundDeathPose()
    {
        if (!_hasSnapshot || !_dead || _animator == null || !_animator.isHuman || _owner == null
            || _animator.avatar == null || !_animator.avatar.isValid || !_animator.avatar.isHuman) return;
        // The imported death motion expects vertical root motion. Keep the
        // network root still and offset only the humanoid's rendered pose.
        float lowest = float.PositiveInfinity;
        foreach (var bone in GroundBones)
        {
            var transform = _animator.GetBoneTransform(bone);
            if (transform != null) lowest = Mathf.Min(lowest, transform.position.y);
        }
        float floor = _owner.transform.position.y + .08f;
        if (float.IsInfinity(lowest) || lowest <= floor + .0001f || _animator.humanScale <= 0f) return;
        if (_poseHandler == null) _poseHandler = new HumanPoseHandler(_animator.avatar, _animator.transform);
        // HumanPoseHandler updates the bones immediately. Animator.bodyPosition
        // alone defers its result and can accumulate an offset while paused.
        _poseHandler.GetHumanPose(ref _pose);
        // Get returns a normalized world body pose; Set consumes a root-local
        // pose. Convert both position and rotation to preserve its world X/Z.
        var worldBody = _pose.bodyPosition * _animator.humanScale;
        worldBody.y -= lowest - floor;
        _pose.bodyPosition = _animator.transform.InverseTransformPoint(worldBody) / _animator.humanScale;
        _pose.bodyRotation = Quaternion.Inverse(_animator.transform.rotation) * _pose.bodyRotation;
        _poseHandler.SetHumanPose(ref _pose);
    }

    private static readonly HumanBodyBones[] GroundBones =
    {
        HumanBodyBones.Hips, HumanBodyBones.Chest, HumanBodyBones.Head,
        HumanBodyBones.LeftHand, HumanBodyBones.RightHand,
        HumanBodyBones.LeftFoot, HumanBodyBones.RightFoot,
        HumanBodyBones.LeftLowerLeg, HumanBodyBones.RightLowerLeg,
        HumanBodyBones.LeftLowerArm, HumanBodyBones.RightLowerArm
    };

    private void ApplySnapshot()
    {
        if (_presentation == null || !_presentation.TryGetSnapshot(out var snapshot)) return;
        if (_hasSnapshot && snapshot.IsDead == _dead && snapshot.RespawnVersion == _version) return;
        _hasSnapshot = true;
        _dead = snapshot.IsDead;
        _version = snapshot.RespawnVersion;
        if (_animator == null) return;
        int state = _dead ? DeathState : AliveState;
        if (_animator.HasState(0, state)) _animator.Play(state, 0, 0f);
    }

    private void Unbind(CombatPresentation presentation)
    {
        if (ReferenceEquals(_presentation, presentation)) Clear();
    }

    private void Clear()
    {
        _presentation = null;
        _hasSnapshot = false;
        _dead = false;
        _version = 0;
        if (_poseHandler != null) { _poseHandler.Dispose(); _poseHandler = null; }
    }

    private void OnDisable()
    {
        CombatPresentation.PresentationBound -= Bind;
        CombatPresentation.PresentationUnbound -= Unbind;
        Clear();
    }

    private void OnDestroy() => Clear();
}
