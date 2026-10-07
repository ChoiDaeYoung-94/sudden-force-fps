using Fusion;
using UnityEngine;

// Classification metadata only. This component never applies damage or creates
// PhysX colliders; Fusion Hitbox provides the historical query geometry.
[DisallowMultipleComponent]
[RequireComponent(typeof(Hitbox))]
public sealed class CombatHitbox : MonoBehaviour
{
    [SerializeField] private GamePlayerNetworkData _owner;
    [SerializeField] private CombatBodyPart _bodyPart = CombatBodyPart.Torso;
    public GamePlayerNetworkData Owner => _owner;
    public CombatBodyPart BodyPart => _bodyPart;
    public Hitbox Hitbox => GetComponent<Hitbox>();

    public bool IsConfigured => _owner != null && _owner.HitboxRoot != null && Hitbox != null
        && _owner.HitboxRoot.transform == _owner.transform && Hitbox.Root == _owner.HitboxRoot
        && transform.IsChildOf(_owner.transform)
        && System.Enum.IsDefined(typeof(CombatBodyPart), _bodyPart);
}
