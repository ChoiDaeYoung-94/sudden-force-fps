using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

// Candidate: source simulation bones/12 hitboxes/weapon sockets remain untouched.
// Apply after the authoritative stance adapter's order5000 LateUpdate.
[DefaultExecutionOrder(6000), DisallowMultipleComponent]
public sealed class PlayerWorldBodyLocomotionPresentation : MonoBehaviour
{
    // Exact contract: idle, walk F/B/L/R, run F/B/L/R. All are new Generic
    // prefixed leg-only bakes, never the retained original22 animation assets.
    [SerializeField] private AnimationClip[] _legClips = new AnimationClip[9];
    [SerializeField, Min(.5f)] private float _walkCycleDistance = 3.3f;
    [SerializeField, Min(.5f)] private float _runCycleDistance = 4.2f;
    private GamePlayerNetworkData _owner;
    private Fusion.NetworkCharacterController _controller;
    private CombatPresentation _presentation;
    private WorldBodyVisualRigBinding _rig;
    private PlayableGraph _graph;
    private AnimationMixerPlayable _mixer;
    private AnimationClipPlayable[] _players;
    private Vector3 _previousPosition;
    private Vector2 _velocity;
    private float _phase;
    private int _life;
    private bool _seeded;
    public float ObservedSpeed { get; private set; }
    public float ObservedPhase => _phase;
    public bool ObservedAnimating { get; private set; }
    public Transform VisualRoot => _rig != null ? _rig.Root.transform : null;

    private void Awake()
    {
        _owner = GetComponent<GamePlayerNetworkData>();
        _controller = GetComponent<Fusion.NetworkCharacterController>();
    }

    private void OnEnable()
    {
        CombatPresentation.PresentationBound += Bind;
        CombatPresentation.PresentationUnbound += Unbind;
        if (_owner == null) _owner = GetComponent<GamePlayerNetworkData>();
        if (_controller == null) _controller = GetComponent<Fusion.NetworkCharacterController>();
        if (_owner != null && _owner.Presentation != null) Bind(_owner.Presentation);
    }

    private void Bind(CombatPresentation presentation)
    {
        if (presentation == null || presentation.Owner != _owner || !isActiveAndEnabled) return;
        if (ReferenceEquals(_presentation, presentation) && _rig != null) return;
        Release();
        var stance = GetComponent<PlayerCrouchGeometry>();
        // Its initialization consumes the original skin.bones registry.
        if (stance == null || !stance.Configured || _controller == null || !ClipsConfigured()) return;
        try
        {
            _rig = WorldBodyVisualRigBinding.Create(_owner);
            _graph = PlayableGraph.Create("SFFWorldBodyLocomotion");
            _graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            _mixer = AnimationMixerPlayable.Create(_graph, 9);
            _players = new AnimationClipPlayable[9];
            for (int i = 0; i < 9; i++)
            {
                _players[i] = AnimationClipPlayable.Create(_graph, _legClips[i]);
                _players[i].SetApplyFootIK(false); _players[i].SetApplyPlayableIK(false);
                _players[i].SetSpeed(0d);
                _graph.Connect(_players[i], 0, _mixer, i);
            }
            var output = AnimationPlayableOutput.Create(_graph, "SFFLegsOnly", _rig.Animator);
            output.SetSourcePlayable(_mixer);
            _graph.Play();
            _presentation = presentation;
            ResetMotion();
        }
        catch (System.Exception exception)
        {
            Release();
            Debug.LogWarning("[WorldBodyLocomotion] Visual rig unavailable: " + exception.GetType().Name, this);
        }
    }

    private bool ClipsConfigured()
    {
        if (_legClips == null || _legClips.Length != 9) return false;
        foreach (var clip in _legClips)
            if (clip == null || clip.humanMotion || clip.legacy || !clip.isLooping
                || !PlayerInputSource.IsFinite(clip.length) || clip.length <= 0f) return false;
        return true;
    }

    private void LateUpdate()
    {
        if (_rig == null || _presentation == null) return;
        // Baseline/death/crouch/climb always comes from existing source geometry.
        _rig.CopySourcePose();
        ObservedAnimating = false;
        if (!_presentation.TryGetSnapshot(out var snapshot) || _owner == null
            || _owner.Object == null || !_owner.Object.IsValid || _owner.Runner == null || !_owner.Runner.IsRunning)
        { Release(); return; }
        Vector3 position = _owner.transform.position;
        float dt = Time.deltaTime;
        bool mayAnimate = !snapshot.IsDead && snapshot.Health > 0
            && NetworkMatchState.AllowsCombatFor(_owner.Runner)
            && _controller.Grounded && !_owner.PresentationClimbing && !_owner.PresentationCrouched;
        if (!mayAnimate || !Finite(position) || dt <= 0f || dt > .15f)
        { Seed(position, snapshot.RespawnVersion); return; }
        if (!_seeded || _life != snapshot.RespawnVersion)
        { Seed(position, snapshot.RespawnVersion); return; }
        Vector3 delta = position - _previousPosition;
        _previousPosition = position;
        float horizontal = new Vector2(delta.x, delta.z).magnitude;
        if (horizontal > 8f * dt + .35f || Mathf.Abs(delta.y) > 1.25f)
        { Seed(position, snapshot.RespawnVersion); return; }
        var local = _owner.transform.InverseTransformDirection(new Vector3(delta.x, 0f, delta.z)) / dt;
        var target = new Vector2(local.x, local.z);
        if (target.magnitude <= .15f) target = Vector2.zero;
        else target = Vector2.ClampMagnitude(target, 8f);
        _velocity = Vector2.Lerp(_velocity, target, 1f - Mathf.Exp(-18f * dt));
        float speed = _velocity.magnitude;
        ObservedSpeed = speed;
        float run = Mathf.InverseLerp(5.1f, 6.8f, speed);
        float cycleDistance = Mathf.Max(.5f, Mathf.Lerp(_walkCycleDistance, _runCycleDistance, run));
        // All cardinal bakes use the same left/right contact phase; reverse
        // motion selects a backward clip rather than rewinding a forward gait.
        if (target.sqrMagnitude > 0f) _phase = Mathf.Repeat(_phase + horizontal / cycleDistance, 1f);
        float blend = Mathf.InverseLerp(.15f, 1.5f, speed);
        float total = Mathf.Abs(_velocity.x) + Mathf.Abs(_velocity.y);
        float forward = total > .0001f ? Mathf.Max(0f, _velocity.y) / total : 0f;
        float backward = total > .0001f ? Mathf.Max(0f, -_velocity.y) / total : 0f;
        float left = total > .0001f ? Mathf.Max(0f, -_velocity.x) / total : 0f;
        float right = total > .0001f ? Mathf.Max(0f, _velocity.x) / total : 0f;
        _mixer.SetInputWeight(0, 1f - blend);
        SetDirectionWeights(1, forward, backward, left, right, blend * (1f - run));
        SetDirectionWeights(5, forward, backward, left, right, blend * run);
        for (int i = 0; i < 9; i++) _players[i].SetTime(_phase * _legClips[i].length);
        _graph.Evaluate(0f);
        ObservedAnimating = blend > .001f;
    }

    private void SetDirectionWeights(int first, float forward, float backward, float left, float right, float scale)
    {
        _mixer.SetInputWeight(first, forward * scale);
        _mixer.SetInputWeight(first + 1, backward * scale);
        _mixer.SetInputWeight(first + 2, left * scale);
        _mixer.SetInputWeight(first + 3, right * scale);
    }

    private void Seed(Vector3 position, int life)
    {
        ResetMotion(); _life = life;
        if (!Finite(position)) return;
        _previousPosition = position; _seeded = true;
    }

    private void ResetMotion()
    {
        _seeded = false; _velocity = Vector2.zero; _phase = 0f;
        ObservedSpeed = 0f; ObservedAnimating = false;
    }

    private void Unbind(CombatPresentation presentation)
    { if (ReferenceEquals(_presentation, presentation)) Release(); }

    private void Release()
    {
        _presentation = null;
        if (_graph.IsValid()) _graph.Destroy();
        if (_rig != null) { _rig.Dispose(); _rig = null; }
        _players = null; ResetMotion();
    }

    private static bool Finite(Vector3 value) => PlayerInputSource.IsFinite(value.x)
        && PlayerInputSource.IsFinite(value.y) && PlayerInputSource.IsFinite(value.z);
    private void OnDisable()
    {
        CombatPresentation.PresentationBound -= Bind;
        CombatPresentation.PresentationUnbound -= Unbind;
        Release();
    }
    private void OnDestroy() => Release();
}
