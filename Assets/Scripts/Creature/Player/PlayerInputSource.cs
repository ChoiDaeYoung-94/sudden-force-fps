using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// Local, frame-sampled input. OnInput only copies a snapshot, including absolute
// angles, so multiple Fusion input polls cannot consume mouse/touch deltas twice.
public sealed class PlayerInputSource : MonoBehaviour
{
    public static PlayerInputSource Instance { get; private set; }
    private GamePlayerNetworkData _owner;
    private Vector2 _move;
    private bool _sprint;
    private bool _fire;
    private bool _physicalFireHeld;
    private bool _fireReleaseRequired;
    private int _respawnVersion;
    private bool _reloadRequested;
    private int _reloadSubmittedFrame = -1;
    private float _yaw;
    private float _pitch;
    private int _lookFinger = -1;
    private bool _paused;
    private bool _capturedCursor;
    private CursorLockMode _previousCursorLock;
    private bool _previousCursorVisible;
    private readonly List<RaycastResult> _uiHits = new List<RaycastResult>();

    public void Bind(GamePlayerNetworkData owner, float yaw, float pitch)
    {
        if (Instance != null && Instance != this) Instance.Unbind();
        Instance = this;
        _owner = owner;
        _respawnVersion = owner.RespawnVersion;
        _physicalFireHeld = false;
        _fireReleaseRequired = false;
        _yaw = Mathf.Repeat(yaw, 360f);
        _pitch = Mathf.Clamp(pitch, -85f, 85f);
        ResetInput();
    }

    public void Unbind()
    {
        ResetInput();
        ReleaseCursor();
        _owner = null;
        if (Instance == this) Instance = null;
    }

    public void SetMove(Vector2 move)
    {
        _move = IsFinite(move.x) && IsFinite(move.y) ? Vector2.ClampMagnitude(move, 1f) : Vector2.zero;
    }

    public void SetSprint(bool sprint) => _sprint = sprint;
    public void SetFire(bool fire)
    {
        _physicalFireHeld = fire;
        if (!fire) _fireReleaseRequired = false;
        _fire = fire && !_fireReleaseRequired;
    }

    public void SeedAim(float yaw, float pitch)
    {
        bool held = Application.isMobilePlatform ? _physicalFireHeld : Input.GetMouseButton(0);
        ResetInput();
        _yaw = Mathf.Repeat(yaw, 360f);
        _pitch = Mathf.Clamp(pitch, -85f, 85f);
        _fireReleaseRequired = held;
    }

    public void SynchronizeRespawn()
    {
        if (_owner == null || _owner.Object == null || !_owner.Object.IsValid || _owner.RespawnVersion <= _respawnVersion) return;
        _respawnVersion = _owner.RespawnVersion;
        SeedAim(_owner.AimYaw, _owner.AimPitch);
    }
    public void RequestReload()
    {
        _reloadRequested = true;
        _reloadSubmittedFrame = -1;
    }

    // Degrees; positive Y raises the view. Mobile controls can call this too.
    public void AddLookDelta(Vector2 delta)
    {
        if (!IsFinite(delta.x) || !IsFinite(delta.y)) return;
        _yaw = Mathf.Repeat(_yaw + delta.x, 360f);
        _pitch = Mathf.Clamp(_pitch - delta.y, -85f, 85f);
    }

    public void ResetInput()
    {
        _move = Vector2.zero;
        _sprint = false;
        _fire = false;
        _reloadRequested = false;
        _reloadSubmittedFrame = -1;
        _lookFinger = -1;
        // Preserve the view when focus returns, particularly Blue's spawn yaw.
    }

    public CustomPlayerInput Snapshot()
    {
        SynchronizeRespawn();
        var data = new CustomPlayerInput();
        if (_owner == null || !_owner.CanProvideInput || _paused || !Application.isFocused) return data;
        data.MoveX = _move.x;
        data.MoveZ = _move.y;
        data.HasAim = true;
        data.RespawnVersion = _respawnVersion;
        data.AimYaw = _yaw;
        data.AimPitch = _pitch;
        data.Buttons.Set(PlayerInputButton.Sprint, _sprint);
        data.Buttons.Set(PlayerInputButton.Fire, _fire);
        data.Buttons.Set(PlayerInputButton.Reload, _reloadRequested);
        data.Fire = _fire;
        if (_reloadRequested) _reloadSubmittedFrame = Time.frameCount;
        return data;
    }

    private void Update()
    {
        SynchronizeRespawn();
        // Retain an edge until Fusion polls it. Multiple polls in the same frame
        // see the same button, and PreviousButtons turns that into one request.
        if (_reloadSubmittedFrame >= 0 && _reloadSubmittedFrame < Time.frameCount)
        {
            _reloadRequested = false;
            _reloadSubmittedFrame = -1;
        }
        if (_owner == null || !_owner.CanProvideInput || _paused || !Application.isFocused)
        {
            ResetInput();
            return;
        }
        var joystick = UIManager.Instance != null ? UIManager.Instance.JoyStick : null;
        if (Application.isMobilePlatform)
        {
            if (joystick != null) SetMove(joystick.NormalizedMove);
            SampleTouchLook();
            return;
        }

        var keyboard = new Vector2(
            (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f),
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1f : 0f));
        SetMove(keyboard != Vector2.zero ? keyboard : joystick != null ? joystick.NormalizedMove : Vector2.zero);
        SetSprint(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
        if (Input.GetKeyDown(KeyCode.Escape)) ReleaseCursor();
        else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !IsPointerOverControl(Input.mousePosition)) CaptureCursor();
        if (_capturedCursor) AddLookDelta(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * 2f);
        SetFire(_capturedCursor && Input.GetMouseButton(0));
        if (Input.GetKeyDown(KeyCode.R)) RequestReload();
    }

    private void SampleTouchLook()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began && _lookFinger < 0 && touch.position.x >= Screen.width * 0.5f && !IsPointerOverControl(touch.position))
                _lookFinger = touch.fingerId;
            if (touch.fingerId != _lookFinger) continue;
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) _lookFinger = -1;
            else if (touch.phase == TouchPhase.Moved)
                AddLookDelta(touch.deltaPosition * (180f / Mathf.Max(Screen.width, 1)));
        }
    }

    private bool IsPointerOverControl(Vector2 position)
    {
        if (EventSystem.current == null) return false;
        _uiHits.Clear();
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = position }, _uiHits);
        foreach (var hit in _uiHits)
        {
            // Decorative full-screen Images must not swallow all camera input.
            if (ExecuteEvents.GetEventHandler<IPointerClickHandler>(hit.gameObject) != null
                || ExecuteEvents.GetEventHandler<IPointerDownHandler>(hit.gameObject) != null
                || ExecuteEvents.GetEventHandler<IDragHandler>(hit.gameObject) != null) return true;
        }
        return false;
    }

    private void CaptureCursor()
    {
        if (_capturedCursor) return;
        _previousCursorLock = Cursor.lockState;
        _previousCursorVisible = Cursor.visible;
        _capturedCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ReleaseCursor()
    {
        if (!_capturedCursor) return;
        _capturedCursor = false;
        Cursor.lockState = _previousCursorLock;
        Cursor.visible = _previousCursorVisible;
    }

    private void OnApplicationFocus(bool focused)
    {
        if (!focused) { ResetInput(); ReleaseCursor(); }
    }

    private void OnApplicationPause(bool paused)
    {
        _paused = paused;
        if (paused) { ResetInput(); ReleaseCursor(); }
    }

    private void OnDisable() => Unbind();
    private void OnDestroy() => Unbind();
    public static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
}
