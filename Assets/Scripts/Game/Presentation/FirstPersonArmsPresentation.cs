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
        _armsAnimator.applyRootMotion = false;
        _armsAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        _armsAnimator.Rebind();
        _armsAnimator.Play(IdleState, 0, 0f);
        _armsAnimator.Update(0f);
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
        if (_configured) AlignToWeapon();
    }

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
        RenderPipelineManager.beginCameraRendering -= BeginSrpCamera;
        RenderPipelineManager.endCameraRendering -= EndSrpCamera;
        Camera.onPreCull -= BeginBuiltinCamera;
        Camera.onPostRender -= EndBuiltinCamera;
        SetVisible(false);
    }
}
