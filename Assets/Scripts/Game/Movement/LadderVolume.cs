using System.Collections.Generic;
using UnityEngine;

// Scene-local immutable geometry. The optional trigger is only a visual aid;
// simulation never trusts OnTriggerEnter/Exit callbacks for authority.
[DisallowMultipleComponent]
public sealed class LadderVolume : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private BoxCollider _solid;
    [SerializeField, Min(.1f)] private float _entryHalfWidth = .55f;
    [SerializeField, Min(.1f)] private float _entryDepth = .9f;
    [SerializeField, Min(.1f)] private float _surfaceStandOff = .38f;
    [SerializeField, Min(.1f)] private float _landingInward = .65f;
    private static readonly HashSet<LadderVolume> Active = new HashSet<LadderVolume>();
    public int Id => _id;
    public Vector3 Outward => Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    public Vector3 Right => Vector3.Cross(Vector3.up, Outward);
    public bool Configured => _id > 0 && _solid != null && !_solid.isTrigger && _solid.enabled
        && gameObject.activeInHierarchy && Outward.sqrMagnitude > .99f
        && (transform.lossyScale - Vector3.one).sqrMagnitude < .00001f;
    public Vector3 Bottom => transform.TransformPoint(_solid.center + new Vector3(0f, -_solid.size.y * .5f, _solid.size.z * .5f));
    public Vector3 Top => transform.TransformPoint(_solid.center + new Vector3(0f, _solid.size.y * .5f, _solid.size.z * .5f));
    public Vector3 RailAt(float feetY)
    {
        var basePoint = Vector3.LerpUnclamped(Bottom, Top, (feetY - Bottom.y) / Mathf.Max(.1f, Top.y - Bottom.y));
        return basePoint + Outward * _surfaceStandOff;
    }
    public bool BottomEntry(Vector3 feet, float yaw, float forward)
    {
        Vector3 delta = feet - Bottom;
        float distance = Vector3.Dot(delta, Outward);
        return forward > .4f && feet.y >= Bottom.y - .2f && feet.y <= Bottom.y + .45f
            && Mathf.Abs(Vector3.Dot(delta, Right)) <= _entryHalfWidth
            && distance >= .2f && distance <= _entryDepth
            && Vector3.Dot(Quaternion.Euler(0f, yaw, 0f) * Vector3.forward, -Outward) >= .5f;
    }
    public bool TopEntry(Vector3 feet, Vector3 landing, float yaw, float forward)
    {
        return forward < -.4f && Mathf.Abs(feet.y - landing.y) < .3f
            && Vector3.ProjectOnPlane(feet - landing, Vector3.up).sqrMagnitude < .65f * .65f
            && Vector3.Dot(Quaternion.Euler(0f, yaw, 0f) * Vector3.forward, Outward) >= .5f;
    }
    public bool TryLanding(PhysicsScene physics, out Vector3 feet)
    {
        feet = default;
        var origin = Top - Outward * _landingInward + Vector3.up;
        if (!physics.Raycast(origin, Vector3.down, out var hit, 2f, 1 << 8, QueryTriggerInteraction.Ignore)
            || Vector3.Dot(hit.normal, Vector3.up) < .7071f) return false;
        // Do not classify a low floor at the base as the top platform.
        if (Mathf.Abs(hit.point.y - Top.y) > .65f) return false;
        feet = hit.point + Vector3.up * .045f;
        return true;
    }
    public static LadderVolume[] InScene(UnityEngine.SceneManagement.Scene scene)
    {
        var found = new List<LadderVolume>();
        foreach (var item in Active)
            if (item != null && item.gameObject.scene == scene && item.Configured) found.Add(item);
        found.Sort((a, b) => a.Id.CompareTo(b.Id));
        // Ambiguous IDs or missing expected ladders fail closed.
        if (found.Count != 2 || found[0].Id == found[1].Id) return new LadderVolume[0];
        return found.ToArray();
    }
    private void Awake() { if (_solid == null) _solid = GetComponent<BoxCollider>(); }
    private void OnEnable() => Active.Add(this);
    private void OnDisable() => Active.Remove(this);
#if UNITY_EDITOR
    public void ConfigureEditor(int id, BoxCollider solid)
    {
        if (Application.isPlaying) throw new System.InvalidOperationException("Edit Mode only");
        _id = id; _solid = solid;
    }
#endif
}
