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
            _source.SynchronizeRespawn();
            _source.PressTouchFire();
            _source.TryCaptureTouchLook(eventData.pointerId);
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
        if (_pointer == eventData.pointerId)
            Release(true, _control == Control.Fire && IsCanceledTouch(eventData));
    }

    private static bool IsCanceledTouch(PointerEventData eventData)
    {
        // StandaloneInputModule sends pointerUp for both Ended and Canceled.
        if (eventData.pointerId < 0 || eventData.currentInputModule == null) return false;
        var input = eventData.currentInputModule.input;
        for (int i = 0; i < input.touchCount; i++)
        {
            var touch = input.GetTouch(i);
            if (touch.fingerId == eventData.pointerId) return touch.phase == TouchPhase.Canceled;
        }
        return false;
    }
    public void OnCancel(BaseEventData eventData) => Release(false);
    private void OnUnbound(CombatPresentation presentation) => Release(false);
    private void OnApplicationFocus(bool focused) { if (!focused) Release(false); }
    private void OnApplicationPause(bool paused) { if (paused) Release(false); }
    public void ResetOwnership() => Release(false);
    private void Release(bool physicalRelease, bool canceledFire = false)
    {
        if (_source != null)
        {
            if (_control == Control.Fire)
            {
                if (_pointer.HasValue) _source.ReleaseTouchLook(_pointer.Value);
                if (physicalRelease) _source.ReleaseTouchFire(canceledFire);
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
