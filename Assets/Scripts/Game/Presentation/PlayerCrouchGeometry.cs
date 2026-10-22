using Fusion;
using UnityEngine;

// Deterministic, authored stance geometry. No asset/importer/scale edits.
// The owner invokes this both before simulation movement and before hitbox capture.
[DisallowMultipleComponent]
[DefaultExecutionOrder(5000)]
public sealed class PlayerCrouchGeometry : MonoBehaviour
{
    public const float CrouchDrop = .6f;
    public const float CrouchSpeed = 2.5f;
    public const float PelvisBack = .08f;
    public const float CrouchCapsuleHeight = 1.3f;
    private GamePlayerNetworkData _owner;
    private CharacterController _capsule;
    private Transform _eye, _hips, _leftThigh, _leftCalf, _leftFoot, _rightThigh, _rightCalf, _rightFoot;
    private Transform[] _bones;
    private Vector3[] _positions;
    private Quaternion[] _rotations;
    private Vector3 _leftFootRoot, _rightFootRoot, _eyeBase, _standingCenter;
    private Quaternion _leftFootRotation, _rightFootRotation;
    private float _standingHeight, _standingUpperLimit, _crouchHeight;
    private int _solidMask;
    private readonly Collider[] _overlaps = new Collider[64];
    public bool Configured { get; private set; }
    public Vector3 EyeLocalPosition(bool crouched) => _eyeBase
        - (crouched ? Vector3.up * CrouchDrop + Vector3.forward * PelvisBack : Vector3.zero);

    public bool Initialize(GamePlayerNetworkData owner, CharacterController capsule, Transform eye)
    {
        if (Configured) return _owner == owner && _capsule == capsule && _eye == eye;
        _owner = owner; _capsule = capsule; _eye = eye;
        var animator = GetComponent<Animator>();
        if (owner == null || capsule == null || eye == null || animator == null || !animator.isHuman
            || animator.avatar == null || !animator.avatar.isValid
            || Vector3.Distance(transform.lossyScale, Vector3.one) > .001f
            || GetComponentsInChildren<CombatHitbox>(true).Length != 12) return false;
        // The serialized full-body Animator is on the actor root. A separate
        // first-person arm rig under ViewModelRoot must never drive hitboxes.
        if (owner.ViewModelRoot != null && (animator.transform == owner.ViewModelRoot
            || animator.transform.IsChildOf(owner.ViewModelRoot))) return false;
        // The two serialized rigs share bone names and Avatar. Resolve only the
        // authored body root, cross-checked against its actual skin bindings.
        // Animator.GetBoneTransform by itself can select the nested FPS rig in
        // a newly loaded prefab; it must match this independent body registry.
        if (owner.transform != transform || capsule.transform != transform || eye != owner.CameraRoot
            || !TryResolveBodySkeleton(owner, eye, animator, out var skeleton)
            || !TryPrepareStandingPose(animator, eye)) return false;
        var skeletonBones = new System.Collections.Generic.List<Transform>();
        foreach (var bone in skeleton.GetComponentsInChildren<Transform>(true))
        {
            if (bone == eye || bone.IsChildOf(eye) || eye.IsChildOf(bone)
                || IsViewModelBone(owner, bone)) return false;
            skeletonBones.Add(bone);
        }
        _bones = skeletonBones.ToArray();
        _positions = new Vector3[_bones.Length]; _rotations = new Quaternion[_bones.Length];
        for (int i = 0; i < _bones.Length; i++)
        { _positions[i] = _bones[i].localPosition; _rotations[i] = _bones[i].localRotation; }
        _leftFootRoot = transform.InverseTransformPoint(_leftFoot.position);
        _rightFootRoot = transform.InverseTransformPoint(_rightFoot.position);
        _leftFootRotation = Quaternion.Inverse(transform.rotation) * _leftFoot.rotation;
        _rightFootRotation = Quaternion.Inverse(transform.rotation) * _rightFoot.rotation;
        _standingHeight = capsule.height; _standingCenter = capsule.center; _eyeBase = eye.localPosition;
        _standingUpperLimit = _standingCenter.y + _standingHeight * .5f;
        float crouchHeadFit = CrouchCapsuleHeight;
        foreach (var metadata in GetComponentsInChildren<CombatHitbox>(true))
        {
            if (metadata.BodyPart != CombatBodyPart.Head) continue;
            var hitbox = metadata.Hitbox;
            Vector3 center = transform.InverseTransformPoint(hitbox.transform.TransformPoint(hitbox.Offset));
            _standingUpperLimit = Mathf.Max(_standingUpperLimit, center.y + hitbox.SphereRadius);
            // Fit the head sphere inside the capsule's rounded upper cap, not
            // just below its top plane. Pelvis-back shifts the head in Z too.
            float innerRadius = capsule.radius - hitbox.SphereRadius - .01f;
            float horizontalSquared = new Vector2(center.x - _standingCenter.x,
                center.z - PelvisBack - _standingCenter.z).sqrMagnitude;
            if (innerRadius <= 0f || horizontalSquared >= innerRadius * innerRadius) return false;
            float capAllowance = Mathf.Sqrt(innerRadius * innerRadius - horizontalSquared);
            crouchHeadFit = Mathf.Max(crouchHeadFit, center.y - CrouchDrop + capsule.radius - capAllowance);
        }
        _crouchHeight = crouchHeadFit;
        if (_crouchHeight < capsule.radius * 2f + .02f || _crouchHeight <= capsule.stepOffset) return false;
        for (int layer = 0; layer < 32; layer++)
            if (!Physics.GetIgnoreLayerCollision(gameObject.layer, layer)) _solidMask |= 1 << layer;
        _solidMask &= ~(1 << 10); // Lag compensated hitboxes never block headroom.
        Configured = true;
        return true;
    }

    private static bool IsViewModelBone(GamePlayerNetworkData owner, Transform bone)
    {
        return owner.ViewModelRoot != null && (bone == owner.ViewModelRoot || bone.IsChildOf(owner.ViewModelRoot));
    }

    private bool TryResolveBodySkeleton(GamePlayerNetworkData owner, Transform eye, Animator animator, out Transform skeleton)
    {
        skeleton = null;
        // These paths are the existing CC0 soldier's authored body contract,
        // not an unconstrained same-name search through both duplicated rigs.
        foreach (Transform child in transform)
            if (child.name == "root") { if (skeleton != null) return false; skeleton = child; }
        if (skeleton == null || skeleton == eye || skeleton.IsChildOf(eye)
            || eye.IsChildOf(skeleton) || IsViewModelBone(owner, skeleton)) return false;
        var registry = new System.Collections.Generic.HashSet<Transform>();
        int bodySkins = 0;
        foreach (var skin in GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            if (skin.transform == eye || skin.transform.IsChildOf(eye) || IsViewModelBone(owner, skin.transform)) continue;
            if (skin.rootBone == null || (skin.rootBone != skeleton && !skin.rootBone.IsChildOf(skeleton))) return false;
            bodySkins++;
            foreach (var bone in skin.bones)
            {
                if (bone == null || (bone != skeleton && !bone.IsChildOf(skeleton))
                    || bone == eye || bone.IsChildOf(eye) || IsViewModelBone(owner, bone)) return false;
                registry.Add(bone);
            }
        }
        if (bodySkins == 0) return false;
        // Preserve the source Avatar's authored mapping. For this soldier the
        // humanoid Hips maps to "root", not the child named "pelvis".
        var description = animator.avatar.humanDescription;
        _hips = FindBodyBone(description, HumanBodyBones.Hips, registry);
        _leftThigh = FindBodyBone(description, HumanBodyBones.LeftUpperLeg, registry);
        _leftCalf = FindBodyBone(description, HumanBodyBones.LeftLowerLeg, registry);
        _leftFoot = FindBodyBone(description, HumanBodyBones.LeftFoot, registry);
        _rightThigh = FindBodyBone(description, HumanBodyBones.RightUpperLeg, registry);
        _rightCalf = FindBodyBone(description, HumanBodyBones.RightLowerLeg, registry);
        _rightFoot = FindBodyBone(description, HumanBodyBones.RightFoot, registry);
        var required = new[] { _hips, _leftThigh, _leftCalf, _leftFoot, _rightThigh, _rightCalf, _rightFoot };
        foreach (var bone in required)
            if (bone == null || !registry.Contains(bone) || (bone != skeleton && !bone.IsChildOf(skeleton))
                || IsViewModelBone(owner, bone)) return false;
        if (!_leftThigh.IsChildOf(_hips) || !_rightThigh.IsChildOf(_hips)
            || _leftCalf.parent != _leftThigh || _leftFoot.parent != _leftCalf
            || _rightCalf.parent != _rightThigh || _rightFoot.parent != _rightCalf) return false;
        int heads = 0;
        foreach (var metadata in GetComponentsInChildren<CombatHitbox>(true))
        {
            if (!metadata.IsConfigured || metadata.Owner != owner
                || !metadata.transform.IsChildOf(skeleton) || IsViewModelBone(owner, metadata.transform)) return false;
            if (metadata.BodyPart != CombatBodyPart.Head) continue;
            heads++;
            if (metadata.Hitbox.Type != HitboxTypes.Sphere || metadata.Hitbox.SphereRadius <= 0f) return false;
        }
        return heads == 1;
    }

    private static Transform FindBodyBone(HumanDescription description, HumanBodyBones human, System.Collections.Generic.HashSet<Transform> registry)
    {
        string name = null;
        foreach (var mapping in description.human)
        {
            if (mapping.humanName != HumanTrait.BoneName[(int)human]) continue;
            if (name != null) return null;
            name = mapping.boneName;
        }
        if (string.IsNullOrEmpty(name)) return null;
        Transform found = null;
        foreach (var bone in registry)
        {
            if (bone.name != name) continue;
            if (found != null) return null;
            found = bone;
        }
        return found;
    }

    private bool AnimatorMatchesBody(Animator animator)
    {
        return animator.GetBoneTransform(HumanBodyBones.Hips) == _hips
            && animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg) == _leftThigh
            && animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg) == _leftCalf
            && animator.GetBoneTransform(HumanBodyBones.LeftFoot) == _leftFoot
            && animator.GetBoneTransform(HumanBodyBones.RightUpperLeg) == _rightThigh
            && animator.GetBoneTransform(HumanBodyBones.RightLowerLeg) == _rightCalf
            && animator.GetBoneTransform(HumanBodyBones.RightFoot) == _rightFoot;
    }

    private bool TryPrepareStandingPose(Animator animator, Transform eye)
    {
        // Rebinding/initial idle evaluation may restore animated defaults.
        // Preserve the independent camera/viewmodel local pose, then require
        // the actor Animator's seven identities to match the body registry.
        var protectedBones = eye.GetComponentsInChildren<Transform>(true);
        var positions = new Vector3[protectedBones.Length];
        var rotations = new Quaternion[protectedBones.Length];
        var scales = new Vector3[protectedBones.Length];
        for (int i = 0; i < protectedBones.Length; i++)
        { positions[i] = protectedBones[i].localPosition; rotations[i] = protectedBones[i].localRotation; scales[i] = protectedBones[i].localScale; }
        try
        {
            if (!AnimatorMatchesBody(animator)) animator.Rebind();
            if (!AnimatorMatchesBody(animator)) return false;
            int idle = Animator.StringToHash("Base Layer.Idle_Aiming");
            if (!animator.HasState(0, idle)) return false;
            animator.Play(idle, 0, 0f);
            animator.Update(0f);
            return AnimatorMatchesBody(animator);
        }
        finally
        {
            for (int i = 0; i < protectedBones.Length; i++)
            { protectedBones[i].localPosition = positions[i]; protectedBones[i].localRotation = rotations[i]; protectedBones[i].localScale = scales[i]; }
        }
    }

    public bool CanStand(NetworkRunner runner)
    {
        if (!Configured || runner == null || !runner.IsRunning) return false;
        var scene = runner.GetPhysicsScene();
        if (!scene.IsValid()) return false;
        // Test the upper swept capsule only: a floor supporting the current
        // crouched capsule must not make every stand attempt fail.
        float feet = _standingCenter.y - _standingHeight * .5f;
        Vector3 low = transform.TransformPoint(new Vector3(_standingCenter.x,
            feet + _crouchHeight - _capsule.radius, _standingCenter.z));
        Vector3 high = transform.TransformPoint(new Vector3(_standingCenter.x,
            _standingUpperLimit - _capsule.radius + .01f, _standingCenter.z));
        int count = scene.OverlapCapsule(low, high, _capsule.radius + .01f, _overlaps,
            _solidMask, QueryTriggerInteraction.Ignore);
        if (count >= _overlaps.Length) return false; // Saturation fails closed.
        for (int i = 0; i < count; i++)
        {
            var collider = _overlaps[i]; _overlaps[i] = null;
            if (collider == null || collider.isTrigger || collider == _capsule
                || collider.transform.IsChildOf(transform)
                || collider.GetComponentInParent<CombatHitbox>() != null) continue;
            return false;
        }
        return true;
    }

    // Required simulation geometry, not an optional visual adapter:
    // explicit owner/FUN/hitbox-capture calls intentionally apply even if enabled
    // is false. Disabling only suspends this component's automatic LateUpdate;
    // it is not a supported live crouch-off switch. The owner keeps it enabled
    // for final render pose ownership. Never force standing from OnDisable:
    // only the owner stance state machine may stand after a headroom check.
    public void Apply(bool crouched, bool alive)
    {
        if (!Configured) return;
        float height = crouched ? _crouchHeight : _standingHeight;
        Vector3 center = _standingCenter - Vector3.up * (crouched ? (_standingHeight - _crouchHeight) * .5f : 0f);
        if (Mathf.Abs(_capsule.height - height) > .0001f) _capsule.height = height;
        if ((_capsule.center - center).sqrMagnitude > .00000001f) _capsule.center = center;
        _eye.localPosition = EyeLocalPosition(crouched);
        if (!alive) return; // Existing baked death motion owns dead bones.
        for (int i = 0; i < _bones.Length; i++)
        { _bones[i].localPosition = _positions[i]; _bones[i].localRotation = _rotations[i]; }
        if (!crouched) return;
        _hips.position -= transform.up * CrouchDrop + transform.forward * PelvisBack;
        SolveLeg(_leftThigh, _leftCalf, _leftFoot, transform.TransformPoint(_leftFootRoot),
            transform.rotation * _leftFootRotation);
        SolveLeg(_rightThigh, _rightCalf, _rightFoot, transform.TransformPoint(_rightFootRoot),
            transform.rotation * _rightFootRotation);
    }

    private void SolveLeg(Transform thigh, Transform calf, Transform foot, Vector3 target, Quaternion footRotation)
    {
        float upper = Vector3.Distance(thigh.position, calf.position);
        float lower = Vector3.Distance(calf.position, foot.position);
        Vector3 direction = target - thigh.position;
        float distance = Mathf.Clamp(direction.magnitude, Mathf.Abs(upper - lower) + .0001f, upper + lower - .0001f);
        if (upper <= .0001f || lower <= .0001f || direction.sqrMagnitude <= .000001f) return;
        Vector3 axis = direction.normalized;
        Vector3 bend = Vector3.ProjectOnPlane(transform.forward, axis).normalized;
        if (bend.sqrMagnitude < .001f) bend = transform.right;
        float along = (upper * upper - lower * lower + distance * distance) / (2f * distance);
        float height = Mathf.Sqrt(Mathf.Max(0f, upper * upper - along * along));
        Vector3 knee = thigh.position + axis * along + bend * height;
        thigh.rotation = Quaternion.FromToRotation(calf.position - thigh.position, knee - thigh.position) * thigh.rotation;
        calf.rotation = Quaternion.FromToRotation(foot.position - calf.position, target - calf.position) * calf.rotation;
        foot.rotation = footRotation;
    }

    private void LateUpdate()
    {
        if (_owner != null && _owner.Object != null && _owner.Object.IsValid)
            Apply(_owner.PresentationCrouched, !_owner.IsDead && _owner.Health > 0);
    }
}







