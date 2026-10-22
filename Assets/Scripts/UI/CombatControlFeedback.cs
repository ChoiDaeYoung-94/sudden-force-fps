using UnityEngine;

// Reads already captured ownership only. No pointer handlers or input writes.
public sealed class CombatControlFeedback : MonoBehaviour
{
    [SerializeField] private CombatPointerControl _pointer;
    [SerializeField] private JoyStick _joystick;
    [SerializeField] private RectTransform _visual;
    [SerializeField] private UnityEngine.UI.Graphic _face;
    [SerializeField] private UnityEngine.UI.Graphic _ring;
    [SerializeField] private Color _restFace = new Color(.07f, .10f, .13f, .94f);
    [SerializeField] private Color _pressedFace = new Color(.15f, .21f, .26f, .98f);
    [SerializeField] private Color _restRing = new Color(.48f, .70f, .83f, .75f);
    [SerializeField] private Color _pressedRing = new Color(1f, .69f, .25f, 1f);
    private float _amount;

    private void Update()
    {
        bool captured = (_pointer != null && _pointer.HasCapturedPointer)
            || (_joystick != null && _joystick.HasCapturedPointer);
        _amount = Mathf.MoveTowards(_amount, captured ? 1f : 0f, Time.unscaledDeltaTime * 14f);
        Apply();
    }

    private void Apply()
    {
        if (_visual != null) _visual.localScale = Vector3.one * Mathf.Lerp(1f, .94f, _amount);
        if (_face != null) _face.color = Color.Lerp(_restFace, _pressedFace, _amount);
        if (_ring != null) _ring.color = Color.Lerp(_restRing, _pressedRing, _amount);
    }

    private void OnEnable() { _amount = 0f; Apply(); }
    private void OnDisable() { _amount = 0f; Apply(); }
}
