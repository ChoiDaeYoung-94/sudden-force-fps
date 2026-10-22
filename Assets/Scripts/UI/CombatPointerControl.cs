using UnityEngine;
using UnityEngine.EventSystems;

public sealed class CombatPointerControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ICancelHandler
{
    public enum Control { Fire = 0, Reload = 1, Sprint = 2, Crouch = 3 }
    [SerializeField] private Control _control;
    private int? _pointer;
    private PlayerInputSource _source;
    public bool HasCapturedPointer => _pointer.HasValue;

    private void OnEnable() => CombatPresentation.LocalUnbound += OnUnbound;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActiveAndEnabled || _pointer.HasValue || PlayerInputSource.Instance == null) return;
        var presentation = CombatPresentation.LocalInstance;
        if (presentation == null || !presentation.TryGetSnapshot(out var snapshot) || snapshot.IsDead) return;
        _pointer = eventData.pointerId;
        _source = PlayerInputSource.Instance;
        if (_control == Control.Fire)
        {
            // A real new Down proves a fresh press. Lifecycle resets never claim
            // that a finger physically released, so they retain the core gate.
            _source.SynchronizeRespawn();
            _source.SetFire(false);
            _source.SetFire(true);
        }
        else if (_control == Control.Crouch)
        {
            _source.SynchronizeRespawn();
            _source.SetCrouch(false);
            _source.SetCrouch(true);
        }
        else if (_control == Control.Sprint) _source.SetSprint(true);
        else _source.RequestReload();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_pointer == eventData.pointerId) Release(true);
    }
    public void OnCancel(BaseEventData eventData) => Release(false);
    private void OnUnbound(CombatPresentation presentation) => Release(false);
    private void OnApplicationFocus(bool focused) { if (!focused) Release(false); }
    private void OnApplicationPause(bool paused) { if (paused) Release(false); }
    public void ResetOwnership() => Release(false);
    private void Release(bool physicalRelease)
    {
        if (_source != null)
        {
            if (_control == Control.Fire)
            {
                if (physicalRelease) _source.SetFire(false);
                else _source.ResetInput();
            }
            if (_control == Control.Sprint) _source.SetSprint(false);
            if (_control == Control.Crouch)
            {
                if (physicalRelease) _source.SetCrouch(false);
                else _source.ResetInput();
            }
        }
        _pointer = null;
        _source = null;
    }
    private void OnDisable()
    {
        CombatPresentation.LocalUnbound -= OnUnbound;
        Release(false);
    }
}
