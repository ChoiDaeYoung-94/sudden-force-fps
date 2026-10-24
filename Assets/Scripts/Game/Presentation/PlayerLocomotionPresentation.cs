using UnityEngine;
using UnityEngine.Audio;

// External review candidate. Owner calls RenderPresentation once from Render().
// No simulation writes, RPCs, animator writes or owner transform writes.
[DisallowMultipleComponent]
public sealed class PlayerLocomotionPresentation : MonoBehaviour
{
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    [SerializeField, Min(.5f)] private float _walkStepDistance = 1.65f;
    [SerializeField, Min(.5f)] private float _runStepDistance = 2.1f;
    [SerializeField, Min(.5f)] private float _crouchStepDistance = 1.25f;
    [SerializeField, Range(0f, 1f)] private float _localStepVolume = .2f;
    [SerializeField, Range(0f, 1f)] private float _remoteStepVolume = .65f;
    [SerializeField, Min(1f)] private float _audibleDistance = 20f;
    private GamePlayerNetworkData _owner;
    private Fusion.NetworkCharacterController _controller;
    private AudioSource _source;
    private Vector3 _previousPosition;
    private float _distance, _phase, _blend, _speed, _reloadBlend;
    private float _previousYaw, _previousPitch;
    private Vector2 _sway;
    private bool _sampled;
    private int _life, _frame = -1, _step;
    public Vector3 LocalWeaponPositionOffset { get; private set; }
    public Quaternion LocalWeaponRotationOffset { get; private set; } = Quaternion.identity;
    public int ObservedStepCount { get; private set; }

    private void Awake()
    {
        _owner = GetComponent<GamePlayerNetworkData>();
        _controller = GetComponent<Fusion.NetworkCharacterController>();
    }

    // Stance is supplied by the owner, keeping this candidate independent of
    // any new Networked stance property and of the owner's simulation code.
    public void RenderPresentation(bool crouched, bool climbing = false)
    {
        // Direct owner calls still reach disabled MonoBehaviours.
        // Reset before the same-frame guard so disable/re-enable can seed safely.
        if (!isActiveAndEnabled)
        {
            _frame = -1;
            ResetPresentation();
            return;
        }
        if (_frame == Time.frameCount) return;
        _frame = Time.frameCount;
        if (_owner == null || _controller == null || _owner.Object == null
            || !_owner.Object.IsValid || _owner.Runner == null || !_owner.Runner.IsRunning
            || _owner.Health <= 0 || _owner.IsDead || !NetworkMatchState.AllowsCombatFor(_owner.Runner))
        {
            ResetPresentation();
            return;
        }
        bool local = _owner.Object.HasInputAuthority;
        Vector3 position = _owner.transform.position;
        float dt = Time.deltaTime;
        if (!_sampled || _life != _owner.RespawnVersion || !Finite(position)
            || dt <= 0f || dt > .15f)
        {
            Seed(position);
            return;
        }
        Vector3 delta = position - _previousPosition;
        _previousPosition = position;
        float horizontal = new Vector2(delta.x, delta.z).magnitude;
        // Teleports/corrections/stalls must not become a burst of fake steps.
        if (horizontal > 8f * dt + .35f || Mathf.Abs(delta.y) > 1.25f)
        {
            Seed(position);
            return;
        }
        float actualSpeed = horizontal / dt;
        // Grounded may be true during rail alignment: ladder motion is not walking.
        // Preserve look sway/reload presentation while removing distance/bob history.
        if (climbing)
        {
            _distance = _phase = _blend = _speed = 0f;
            if (_source != null) _source.Stop();
        }
        bool grounded = _controller.Grounded && !climbing;
        bool moving = grounded && actualSpeed > .15f;
        float follow = 1f - Mathf.Exp(-18f * dt);
        _speed = Mathf.Lerp(_speed, moving ? Mathf.Min(actualSpeed, 8f) : 0f, follow);
        _blend = Mathf.Lerp(_blend, moving ? 1f : 0f, follow);
        bool running = !crouched && _speed > 5.5f;
        float stride = crouched ? _crouchStepDistance : running ? _runStepDistance : _walkStepDistance;
        stride = Mathf.Max(.5f, stride);
        if (moving)
        {
            _distance += horizontal;
            _phase = Mathf.Repeat(_phase + horizontal / stride * Mathf.PI, Mathf.PI * 2f);
            if (_distance >= stride)
            {
                _distance %= stride;
                PlayStep(local, crouched, running);
            }
        }
        else if (!grounded || actualSpeed <= .15f)
        {
            _distance = 0f;
        }
        if (!local)
        {
            LocalWeaponPositionOffset = Vector3.zero;
            LocalWeaponRotationOffset = Quaternion.identity;
            return;
        }
        float yaw = Mathf.DeltaAngle(_previousYaw, _owner.AimYaw);
        float pitch = Mathf.DeltaAngle(_previousPitch, _owner.AimPitch);
        _previousYaw = _owner.AimYaw; _previousPitch = _owner.AimPitch;
        // Degrees per second keep sway independent of render frame rate.
        // .00001 = the original per-frame .0006 gain at 60 Hz divided by 60.
        float yawRate = yaw / dt;
        float pitchRate = pitch / dt;
        Vector2 targetSway = new Vector2(Mathf.Clamp(-yawRate * .00001f, -.012f, .012f),
            Mathf.Clamp(pitchRate * .00001f, -.009f, .009f));
        _sway = Vector2.Lerp(_sway, targetSway, follow);
        _reloadBlend = Mathf.Lerp(_reloadBlend, _owner.IsReloading ? 1f : 0f, 1f - Mathf.Exp(-12f * dt));
        float amplitude = _blend * (crouched ? .6f : running ? 1.35f : 1f);
        LocalWeaponPositionOffset = new Vector3(_sway.x + Mathf.Sin(_phase) * .012f * amplitude,
            _sway.y + (Mathf.Cos(_phase * 2f) - 1f) * .009f * amplitude + .14f * _reloadBlend,
            0f);
        LocalWeaponRotationOffset = Quaternion.Euler(0f,
            0f, Mathf.Sin(_phase) * .8f * amplitude);
    }

    private void PlayStep(bool local, bool crouched, bool running)
    {
        if (_stepClips == null || _stepClips.Length == 0) return;
        if (_sfxGroup == null)
        {
            // Preserve the player's existing SFX volume route. Never fall back
            // to an un-routed source that bypasses the user's mute preference.
            foreach (var existing in GetComponentsInChildren<AudioSource>(true))
                if (existing != _source && existing.gameObject.name == "CombatAudio")
                { _sfxGroup = existing.outputAudioMixerGroup; break; }
        }
        if (_sfxGroup == null) return;
        int index = (int)((uint)_step++ % (uint)_stepClips.Length);
        var clip = _stepClips[index];
        if (clip == null) return;
        if (_source == null)
        {
            var child = new GameObject("LocomotionAudio");
            child.transform.SetParent(transform, false);
            child.transform.localPosition = Vector3.up * .08f;
            _source = child.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.loop = false;
            _source.dopplerLevel = 0f;
            _source.priority = 160;
            _source.minDistance = 1.5f;
            _source.maxDistance = Mathf.Max(1.6f, _audibleDistance);
            _source.rolloffMode = AudioRolloffMode.Linear;
        }
        _source.outputAudioMixerGroup = _sfxGroup;
        _source.spatialBlend = local ? 0f : 1f;
        float volume = local ? _localStepVolume : _remoteStepVolume;
        volume *= crouched ? .32f : running ? 1f : .72f;
        _source.PlayOneShot(clip, volume);
        ObservedStepCount++;
    }

    private void Seed(Vector3 position)
    {
        ResetPresentation();
        if (!Finite(position)) return;
        _previousPosition = position;
        _previousYaw = _owner.AimYaw; _previousPitch = _owner.AimPitch;
        _life = _owner.RespawnVersion;
        _sampled = true;
    }
    private void ResetPresentation()
    {
        _sampled = false;
        _distance = _phase = _blend = _speed = _reloadBlend = 0f;
        _sway = Vector2.zero;
        LocalWeaponPositionOffset = Vector3.zero;
        LocalWeaponRotationOffset = Quaternion.identity;
        if (_source != null) _source.Stop();
    }
    private static bool Finite(Vector3 v) => PlayerInputSource.IsFinite(v.x)
        && PlayerInputSource.IsFinite(v.y) && PlayerInputSource.IsFinite(v.z);
    private void OnDisable() { _frame = -1; ResetPresentation(); }
    private void OnDestroy() { if (_source != null) Destroy(_source.gameObject); }
}
