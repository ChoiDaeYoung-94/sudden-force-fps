using UnityEngine;
using UnityEngine.EventSystems;

public sealed class CombatPointerControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ICancelHandler
{
    public enum Control { Fire, Reload, Sprint }
    [SerializeField] private Control _control;
    private int? _pointer;
    private PlayerInputSource _source;
    public bool HasCapturedPointer => _pointer.HasValue;

    private void OnEnable() => CombatPresentation.LocalUnbound += OnUnbound;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_pointer.HasValue || PlayerInputSource.Instance == null) return;
        _pointer = eventData.pointerId;
        _source = PlayerInputSource.Instance;
        if (_control == Control.Fire) _source.SetFire(true);
        else if (_control == Control.Sprint) _source.SetSprint(true);
        else _source.RequestReload();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_pointer == eventData.pointerId) Release();
    }
    public void OnCancel(BaseEventData eventData) => Release();
    private void OnUnbound(CombatPresentation presentation) => Release();
    private void OnApplicationFocus(bool focused) { if (!focused) Release(); }
    private void OnApplicationPause(bool paused) { if (paused) Release(); }
    private void Release()
    {
        if (_source != null)
        {
            if (_control == Control.Fire) _source.SetFire(false);
            if (_control == Control.Sprint) _source.SetSprint(false);
        }
        _pointer = null;
        _source = null;
    }
    private void OnDisable()
    {
        CombatPresentation.LocalUnbound -= OnUnbound;
        Release();
    }
}
