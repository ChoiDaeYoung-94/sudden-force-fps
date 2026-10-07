using TMPro;
using UnityEngine;

public sealed class CombatHudView : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image _healthBar;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _reloadText;
    [SerializeField] private TMP_Text _hitMarker;
    [SerializeField] private TMP_Text _hitText;
    [SerializeField] private JoyStick _joystick;
    [SerializeField] private GameObject[] _touchControls;
    [SerializeField] private TMP_Text _desktopHint;
    [SerializeField] private bool _previewTouchControls;
    private bool? _showTouch;
    private CombatPresentation _presentation;
    private float _hitUntil;
    public CombatPresentation BoundPresentation => _presentation;

    private void OnEnable()
    {
        CombatPresentation.LocalBound += Bind;
        CombatPresentation.LocalUnbound += Unbind;
        Bind(CombatPresentation.LocalInstance);
        ApplyControls();
    }

    private void Bind(CombatPresentation presentation)
    {
        Detach();
        _presentation = presentation;
        if (_presentation == null) { Clear(); return; }
        _presentation.LocalVitalsChanged += ShowVitals;
        _presentation.ConfirmedHit += ShowHit;
        if (_presentation.TryGetSnapshot(out var snapshot)) ShowVitals(snapshot);
    }

    private void ShowVitals(CombatPresentationSnapshot snapshot)
    {
        if (_healthBar != null) _healthBar.fillAmount = Mathf.Clamp01(snapshot.Health / 100f);
        if (_healthText != null) _healthText.text = "HP " + snapshot.Health;
        if (_ammoText != null) _ammoText.text = snapshot.Ammo + " / " + snapshot.Capacity;
        if (_reloadText != null) _reloadText.text = snapshot.IsReloading
            ? "RELOADING " + snapshot.ReloadRemaining.ToString("0.0") + "s" : string.Empty;
    }

    private void ShowHit(ConfirmedHitSnapshot hit)
    {
        bool head = hit.BodyPart == CombatBodyPart.Head;
        var color = head ? new Color(1f, .8f, .2f) : Color.white;
        if (_hitMarker != null) { _hitMarker.color = color; _hitMarker.enabled = true; }
        if (_hitText != null)
        {
            _hitText.color = color;
            _hitText.text = (head ? "HEADSHOT " : "HIT ") + hit.Damage;
            _hitText.enabled = true;
        }
        _hitUntil = Time.unscaledTime + .22f;
    }

    private void Update()
    {
        ApplyControls();
        if (Time.unscaledTime < _hitUntil) return;
        if (_hitMarker != null) _hitMarker.enabled = false;
        if (_hitText != null) _hitText.enabled = false;
    }

    private void ApplyControls()
    {
        bool touch = Application.isMobilePlatform;
#if UNITY_EDITOR
        touch |= _previewTouchControls;
#endif
        if (_showTouch == touch) return;
        _showTouch = touch;
        if (_joystick != null) _joystick.ResetInput();
        if (_touchControls != null)
            foreach (var control in _touchControls) if (control != null) control.SetActive(touch);
        if (_desktopHint != null)
        {
            _desktopHint.enabled = !Application.isMobilePlatform;
            _desktopHint.text = touch ? "EDITOR TOUCH PREVIEW" : "WASD MOVE   SHIFT SPRINT   CLICK AIM / FIRE   R RELOAD   ESC CURSOR";
        }
    }

    private void Unbind(CombatPresentation presentation)
    {
        if (!ReferenceEquals(presentation, _presentation)) return;
        Detach();
        Clear();
        if (PlayerInputSource.Instance != null) PlayerInputSource.Instance.ResetInput();
        if (_joystick != null) _joystick.ResetInput();
    }

    private void Detach()
    {
        if (!ReferenceEquals(_presentation, null))
        {
            _presentation.LocalVitalsChanged -= ShowVitals;
            _presentation.ConfirmedHit -= ShowHit;
        }
        _presentation = null;
    }

    private void Clear()
    {
        if (_healthBar != null) _healthBar.fillAmount = 0;
        if (_healthText != null) _healthText.text = "HP --";
        if (_ammoText != null) _ammoText.text = "-- / --";
        if (_reloadText != null) _reloadText.text = string.Empty;
        if (_hitMarker != null) _hitMarker.enabled = false;
        if (_hitText != null) _hitText.enabled = false;
    }

    private void OnDisable()
    {
        CombatPresentation.LocalBound -= Bind;
        CombatPresentation.LocalUnbound -= Unbind;
        Detach();
        Clear();
        if (PlayerInputSource.Instance != null) PlayerInputSource.Instance.ResetInput();
        if (_joystick != null) _joystick.ResetInput();
    }
}
