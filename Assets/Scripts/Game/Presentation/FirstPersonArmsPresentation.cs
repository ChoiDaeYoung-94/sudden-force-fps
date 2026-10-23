using UnityEngine;
using UnityEngine.Rendering;

// A separate, non-networked skeleton follows the existing view weapon. It never
// writes the gameplay root, full-body bones, weapon socket, or muzzle markers.
[DefaultExecutionOrder(10000)]
public sealed class FirstPersonArmsPresentation : MonoBehaviour
{
    [SerializeField] private GamePlayerNetworkData _owner;
    [SerializeField] private Camera _ownerCamera;
    [SerializeField] private Animator _armsAnimator;
    [SerializeField] private Transform _weaponFrame;
    [SerializeField] private Transform _weaponAnchor;
    [SerializeField] private SkinnedMeshRenderer[] _visibleRenderers;
    private Transform _leftUpperArm;
    private Transform _leftLowerArm;
    private Transform _leftHand;
    [SerializeField] private Transform _viewMagazine;
    private CombatPresentation _combat;
    private Transform _magazineParent, _cachedMagazine, _cachedWeaponFrame;
    private Vector3 _magazineBasePosition, _magazineBaseScale;
    private Quaternion _magazineBaseRotation;
    private bool _hasMagazineBase, _hasReloadPose;
    private Quaternion _baseUpper, _baseLower, _baseHand, _appliedUpper, _appliedLower, _appliedHand;
    private int _reloadLife = int.MinValue, _reloadFrame = -1;
    private static readonly int IdleState = Animator.StringToHash("Base Layer.Idle_Aiming");
    private bool _configured;

    private void Awake()
    {
        _configured = ValidateReferences();
        SetVisible(false);
    }

    private void OnEnable()
    {
        _configured = ValidateReferences();
        SetVisible(false);
        if (!_configured) return;
        ResetReloadPose();
        _reloadLife = int.MinValue; _reloadFrame = -1;
        _combat = _owner.GetComponent<CombatPresentation>();
        _armsAnimator.applyRootMotion = false;
        _armsAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        _armsAnimator.Rebind();
        _armsAnimator.Play(IdleState, 0, 0f);
        _armsAnimator.Update(0f);
        ResolveReloadBones();
        TryCacheMagazineBase();
        RenderPipelineManager.beginCameraRendering += BeginSrpCamera;
        RenderPipelineManager.endCameraRendering += EndSrpCamera;
        Camera.onPreCull += BeginBuiltinCamera;
        Camera.onPostRender += EndBuiltinCamera;
    }

    private bool ValidateReferences()
    {
        if (_owner == null || _ownerCamera == null || _armsAnimator == null
            || _weaponFrame == null || _weaponAnchor == null || _owner.ViewModelRoot == null
            || _owner.ViewMuzzle == null || _owner.ViewMuzzle.parent != _weaponFrame
            || _visibleRenderers == null || _visibleRenderers.Length != 2) return false;
        var rig = _armsAnimator.transform;
        if (rig == _owner.transform || rig != transform || !rig.IsChildOf(_owner.ViewModelRoot)
            || !_weaponFrame.IsChildOf(_owner.ViewModelRoot) || _weaponFrame.IsChildOf(rig)
            || !_weaponAnchor.IsChildOf(rig) || !_ownerCamera.transform.IsChildOf(_owner.transform)
            || _armsAnimator.runtimeAnimatorController == null || !_armsAnimator.isHuman) return false;
        for (int i = 0; i < _visibleRenderers.Length; i++)
            if (_visibleRenderers[i] == null || !_visibleRenderers[i].transform.IsChildOf(rig)) return false;
        return _visibleRenderers[0] != _visibleRenderers[1];
    }

    private bool CanShow(Camera camera)
    {
        return _configured && camera == _ownerCamera && _ownerCamera.isActiveAndEnabled
            && _owner.Object != null && _owner.Object.IsValid && _owner.Object.HasInputAuthority
            && _owner.ViewModelRoot.gameObject.activeInHierarchy
            && _owner.TryGetCombatSnapshot(out var snapshot) && !snapshot.IsDead;
    }

    private void LateUpdate()
    {
        if (_reloadFrame == Time.frameCount) return;
        _reloadFrame = Time.frameCount;
        ReleasePreviousReloadPose();
        if (_configured) AlignToWeapon();
        if (!_configured || _owner.Object == null || !_owner.Object.IsValid || !_owner.Object.HasInputAuthority
            || !_owner.CanProvideInput || !_ownerCamera.isActiveAndEnabled || !_armsAnimator.isActiveAndEnabled
            || !_owner.ViewModelRoot.gameObject.activeInHierarchy || !ValidateReloadReferences())
        {
            ResetReloadPose(); _reloadLife = int.MinValue; return;
        }
        if (_combat == null) _combat = _owner.GetComponent<CombatPresentation>();
        if (_combat == null || !_combat.isActiveAndEnabled || _combat.Owner != _owner
            || !_combat.TryGetSnapshot(out var snapshot) || snapshot.IsDead || snapshot.Health <= 0)
        {
            ResetReloadPose(); _reloadLife = int.MinValue; return;
        }
        if (_reloadLife != snapshot.RespawnVersion)
        {
            ResetReloadPose(); _reloadLife = snapshot.RespawnVersion; return;
        }
        var weapon = _owner.WeaponDefinition;
        float duration = weapon != null ? weapon.ReloadDuration : 0f;
        if (!snapshot.IsReloading || !Finite(duration) || duration <= 0f
            || !Finite(snapshot.ReloadRemaining) || snapshot.ReloadRemaining < 0f)
        {
            ResetReloadPose(); return;
        }
        ApplyReloadPose(Mathf.Clamp01(1f - snapshot.ReloadRemaining / duration));
    }

    private void ResolveReloadBones()
    {
        _leftUpperArm = _leftLowerArm = _leftHand = null;
        if (_armsAnimator == null || _armsAnimator.transform != transform || !_armsAnimator.isHuman
            || _armsAnimator.avatar == null || !_armsAnimator.avatar.isValid) return;
        _leftUpperArm = _armsAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        _leftLowerArm = _armsAnimator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        _leftHand = _armsAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
    }

    private bool ValidateReloadReferences()
    {
        return _owner != null && _owner.ViewModelRoot != null
            && _armsAnimator != null && _armsAnimator.transform == transform
            && _weaponFrame != null && _weaponFrame.IsChildOf(_owner.ViewModelRoot) && !_weaponFrame.IsChildOf(transform)
            && _leftUpperArm != null && _leftUpperArm.IsChildOf(transform) && _leftUpperArm.name == "SFF_FP_upperarm_l"
            && _weaponAnchor != null && !_weaponAnchor.IsChildOf(_leftUpperArm)
            && _leftLowerArm != null && _leftLowerArm.parent == _leftUpperArm && _leftLowerArm.name == "SFF_FP_lowerarm_l"
            && _leftHand != null && _leftHand.parent == _leftLowerArm && _leftHand.name == "SFF_FP_hand_l"
            && _viewMagazine != null && _weaponFrame.Find("Model/Weapon_02_2") == _viewMagazine;
    }

    private bool TryCacheMagazineBase()
    {
        if (_leftUpperArm == null || _leftLowerArm == null || _leftHand == null) ResolveReloadBones();
        if (!ValidateReloadReferences()) return false;
        if (_hasMagazineBase) return _viewMagazine == _cachedMagazine && _weaponFrame == _cachedWeaponFrame && _viewMagazine.parent == _magazineParent;
        _cachedMagazine = _viewMagazine; _cachedWeaponFrame = _weaponFrame;
        _magazineParent = _viewMagazine.parent;
        _magazineBasePosition = _viewMagazine.localPosition;
        _magazineBaseRotation = _viewMagazine.localRotation;
        _magazineBaseScale = _viewMagazine.localScale;
        _hasMagazineBase = true;
        return true;
    }

    // Render-only pose function. The normal caller supplies a copied snapshot phase.
    // Kept separate for isolated reflection tests; never writes simulation or an Animator.
    private void ApplyReloadPose(float normalizedPhase)
    {
        ResetReloadPose();
        if (!Finite(normalizedPhase) || normalizedPhase <= 0f || normalizedPhase >= 1f || !TryCacheMagazineBase()) return;
        float phase = Mathf.Clamp01(normalizedPhase);
        float handWeight = Window(.04f, .20f, phase) * (1f - Window(.80f, .97f, phase));
        float magazineWeight = Window(.22f, .43f, phase) * (1f - Window(.55f, .80f, phase));
        Vector3 frameOffset = Vector3.down * (.18f * magazineWeight);
        // Model has its own rotation. Translate in weapon-frame axes, then convert to the unchanged parent.
        Vector3 parentOffset = _magazineParent.InverseTransformVector(_weaponFrame.TransformVector(frameOffset));
        if (!Finite(parentOffset.x) || !Finite(parentOffset.y) || !Finite(parentOffset.z)) return;
        _viewMagazine.localPosition = _magazineBasePosition + parentOffset;
        _viewMagazine.localRotation = _magazineBaseRotation;
        _viewMagazine.localScale = _magazineBaseScale;
        if (handWeight <= 0f) return;
        _baseUpper = _leftUpperArm.localRotation; _baseLower = _leftLowerArm.localRotation; _baseHand = _leftHand.localRotation;
        Quaternion handRotation = _leftHand.rotation;
        // Initial authored palm target: magazine's left side, in the existing weapon frame.
        // Source pivot + mesh bounds place its centre near (0,-.046,.144); visual acceptance is required.
        Vector3 grip = _weaponFrame.TransformPoint(new Vector3(-.035f, -.055f, .145f) + frameOffset);
        Vector3 target = Vector3.Lerp(_leftHand.position, grip, handWeight);
        if (!SolveReloadArm(target)) { ResetReloadPose(); return; }
        _leftHand.rotation = Quaternion.AngleAxis(25f * handWeight, _weaponFrame.forward) * handRotation;
        _appliedUpper = _leftUpperArm.localRotation; _appliedLower = _leftLowerArm.localRotation; _appliedHand = _leftHand.localRotation;
        _hasReloadPose = true;
    }

    private bool SolveReloadArm(Vector3 target)
    {
        Vector3 shoulder = _leftUpperArm.position, elbow = _leftLowerArm.position, hand = _leftHand.position;
        Vector3 toTarget = target - shoulder;
        float upper = Vector3.Distance(shoulder, elbow), lower = Vector3.Distance(elbow, hand), distance = toTarget.magnitude;
        if (!Finite(upper) || !Finite(lower) || !Finite(distance) || upper < .001f || lower < .001f || distance < .001f) return false;
        Vector3 axis = toTarget / distance;
        Vector3 bend = Vector3.ProjectOnPlane(elbow - shoulder, axis);
        if (bend.sqrMagnitude < .000001f) bend = Vector3.ProjectOnPlane(-_weaponFrame.right, axis);
        if (bend.sqrMagnitude < .000001f) return false;
        bend.Normalize();
        distance = Mathf.Clamp(distance, Mathf.Abs(upper - lower) + .0001f, upper + lower - .0001f);
        float along = (upper * upper - lower * lower + distance * distance) / (2f * distance);
        float height = Mathf.Sqrt(Mathf.Max(0f, upper * upper - along * along));
        Vector3 solvedElbow = shoulder + axis * along + bend * height;
        _leftUpperArm.rotation = Quaternion.FromToRotation(elbow - shoulder, solvedElbow - shoulder) * _leftUpperArm.rotation;
        Vector3 clampedTarget = shoulder + axis * distance;
        _leftLowerArm.rotation = Quaternion.FromToRotation(_leftHand.position - _leftLowerArm.position,
            clampedTarget - _leftLowerArm.position) * _leftLowerArm.rotation;
        return true;
    }

    private void ReleasePreviousReloadPose()
    {
        if (!_hasReloadPose) return;
        // Animator normally supplied this frame's base. Restore only a bone still carrying our last output.
        // This also makes repeated calls without a fresh Animator sample absolute rather than cumulative.
        if (_leftUpperArm != null && _leftUpperArm.IsChildOf(transform) && Quaternion.Angle(_leftUpperArm.localRotation, _appliedUpper) < .001f) _leftUpperArm.localRotation = _baseUpper;
        if (_leftLowerArm != null && _leftLowerArm.IsChildOf(transform) && Quaternion.Angle(_leftLowerArm.localRotation, _appliedLower) < .001f) _leftLowerArm.localRotation = _baseLower;
        if (_leftHand != null && _leftHand.IsChildOf(transform) && Quaternion.Angle(_leftHand.localRotation, _appliedHand) < .001f) _leftHand.localRotation = _baseHand;
        _hasReloadPose = false;
    }

    private void ResetReloadPose()
    {
        ReleasePreviousReloadPose();
        if (!_hasMagazineBase || _cachedMagazine == null || _magazineParent == null || _cachedWeaponFrame == null
            || _cachedMagazine.parent != _magazineParent || !_cachedMagazine.IsChildOf(_cachedWeaponFrame)) return;
        _cachedMagazine.localPosition = _magazineBasePosition;
        _cachedMagazine.localRotation = _magazineBaseRotation;
        _cachedMagazine.localScale = _magazineBaseScale;
    }

    private static float Window(float start, float end, float phase)
    {
        float t = Mathf.Clamp01((phase - start) / (end - start));
        return t * t * (3f - 2f * t);
    }
    private static bool Finite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

    // Rotate the independent rig about its right-hand weapon anchor. This keeps
    // the existing rifle fixed while the known idle's breathing animates bones.
    public void AlignToWeapon()
    {
        if (!_configured) return;
        var rig = _armsAnimator.transform;
        Quaternion delta = _weaponFrame.rotation * Quaternion.Inverse(_weaponAnchor.rotation);
        Vector3 anchorOffset = _weaponAnchor.position - rig.position;
        rig.SetPositionAndRotation(_weaponFrame.position - delta * anchorOffset, delta * rig.rotation);
    }

    private void BeginCamera(Camera camera)
    {
        bool visible = CanShow(camera);
        if (visible) AlignToWeapon();
        SetVisible(visible);
    }

    private void BeginSrpCamera(ScriptableRenderContext context, Camera camera) => BeginCamera(camera);
    private void EndSrpCamera(ScriptableRenderContext context, Camera camera) => SetVisible(false);
    private void BeginBuiltinCamera(Camera camera)
    {
        if (GraphicsSettings.currentRenderPipeline == null) BeginCamera(camera);
    }
    private void EndBuiltinCamera(Camera camera)
    {
        if (GraphicsSettings.currentRenderPipeline == null) SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (_visibleRenderers == null || _armsAnimator == null) return;
        var rig = _armsAnimator.transform;
        // Even invalid serialized references may only write this independent
        // rig's descendants. The gameplay/full-body root is never an arms rig.
        if (rig != transform || GetComponent<GamePlayerNetworkData>() != null) return;
        foreach (var renderer in _visibleRenderers)
            if (renderer != null && renderer.transform.IsChildOf(transform)
                && renderer.transform.IsChildOf(rig)) renderer.enabled = visible;
    }

    private void OnDisable()
    {
        ResetReloadPose(); _reloadLife = int.MinValue; _reloadFrame = -1;
        RenderPipelineManager.beginCameraRendering -= BeginSrpCamera;
        RenderPipelineManager.endCameraRendering -= EndSrpCamera;
        Camera.onPreCull -= BeginBuiltinCamera;
        Camera.onPostRender -= EndBuiltinCamera;
        SetVisible(false);
    }
}
