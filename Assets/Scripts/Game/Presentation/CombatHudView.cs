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
    [SerializeField] private GameObject _deathOverlay;
    [SerializeField] private TMP_Text _deathCountdown;
    [SerializeField] private TMP_Text _kdText;
    private bool? _showTouch;
    private bool _hasLifeSnapshot, _isDead;
    private int _respawnVersion;
    private bool _matchSuppressed;
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
        bool lifeChanged = !_hasLifeSnapshot || snapshot.IsDead != _isDead || snapshot.RespawnVersion != _respawnVersion;
        _hasLifeSnapshot = true;
        _isDead = snapshot.IsDead;
        _respawnVersion = snapshot.RespawnVersion;
        if (lifeChanged) { ResetTouchOwnership(); ClearHit(); }
        if (_deathOverlay != null) _deathOverlay.SetActive(snapshot.IsDead && !_matchSuppressed);
        if (_deathCountdown != null) _deathCountdown.text = snapshot.RespawnPending
            ? "WAITING FOR A SAFE SPAWN" : "RESPAWN IN " + Mathf.CeilToInt(Mathf.Max(0f, snapshot.RespawnRemaining)) + "s";
        if (_kdText != null) _kdText.text = "K " + snapshot.Kill + "  /  D " + snapshot.Death;
        ApplyControls();
        if (_healthBar != null) _healthBar.fillAmount = Mathf.Clamp01(snapshot.Health / 100f);
        if (_healthText != null) _healthText.text = "HP " + snapshot.Health;
        if (_ammoText != null) _ammoText.text = snapshot.Ammo + " / " + snapshot.Capacity;
        if (_reloadText != null) _reloadText.text = snapshot.IsReloading && !snapshot.IsDead && !_matchSuppressed
            ? "RELOADING " + snapshot.ReloadRemaining.ToString("0.0") + "s" : string.Empty;
    }

    private void ShowHit(ConfirmedHitSnapshot hit)
    {
        if (_isDead || _matchSuppressed) return;
        bool head = hit.BodyPart == CombatBodyPart.Head;
        var color = head ? new Color(1f, .8f, .2f) : Color.white;
        if (_hitMarker != null) { _hitMarker.color = color; _hitMarker.enabled = true; }
        if (_hitText != null)
        {
            _hitText.color = color;
            _hitText.text = (hit.Killed ? "ELIMINATED " : head ? "HEADSHOT " : "HIT ") + hit.Damage;
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
        bool visible = touch && _presentation != null && !_isDead && !_matchSuppressed;
        if (_showTouch != visible)
        {
            _showTouch = visible;
            ResetTouchOwnership();
            if (_touchControls != null)
                foreach (var control in _touchControls) if (control != null) control.SetActive(visible);
        }
        if (_desktopHint != null)
        {
            _desktopHint.enabled = !Application.isMobilePlatform && _presentation != null && !_isDead && !_matchSuppressed;
            _desktopHint.text = touch ? "EDITOR TOUCH PREVIEW" : "WASD MOVE   SHIFT SPRINT   CLICK AIM / FIRE   R RELOAD   ESC CURSOR";
        }
    }

    public void SetMatchSuppressed(bool suppressed)
    {
        if (_matchSuppressed == suppressed) return;
        _matchSuppressed = suppressed;
        if (suppressed)
        {
            ResetTouchOwnership();
            ClearHit();
            if (_deathOverlay != null) _deathOverlay.SetActive(false);
            if (_reloadText != null) _reloadText.text = string.Empty;
        }
        else if (_presentation != null && _presentation.TryGetSnapshot(out var snapshot)) ShowVitals(snapshot);
        ApplyControls();
    }

    private void ResetTouchOwnership()
    {
        if (_touchControls != null)
            foreach (var control in _touchControls)
                if (control != null)
                    foreach (var pointer in control.GetComponentsInChildren<CombatPointerControl>(true)) pointer.ResetOwnership();
        if (PlayerInputSource.Instance != null) PlayerInputSource.Instance.ResetInput();
        if (_joystick != null) _joystick.ResetInput();
    }

    private void ClearHit()
    {
        _hitUntil = 0f;
        if (_hitMarker != null) _hitMarker.enabled = false;
        if (_hitText != null) { _hitText.enabled = false; _hitText.text = string.Empty; }
    }

    private void Unbind(CombatPresentation presentation)
    {
        if (!ReferenceEquals(presentation, _presentation)) return;
        Detach();
        Clear();
        ResetTouchOwnership();
        ApplyControls();
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
        _hasLifeSnapshot = false;
        _isDead = false;
        if (_deathOverlay != null) _deathOverlay.SetActive(false);
        if (_deathCountdown != null) _deathCountdown.text = string.Empty;
        if (_kdText != null) _kdText.text = "K --  /  D --";
        ClearHit();
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
        ResetTouchOwnership();
        ApplyControls();
    }
}
