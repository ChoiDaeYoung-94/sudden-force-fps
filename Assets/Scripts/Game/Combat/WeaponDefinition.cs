using UnityEngine;

[CreateAssetMenu(menuName = "Sudden Force/Weapon Definition", fileName = "RifleDefinition")]
public sealed class WeaponDefinition : ScriptableObject
{
    [SerializeField, Min(1)] private int _magazineCapacity = 30;
    [SerializeField, Min(0.01f)] private float _range = 100f;
    [SerializeField, Min(0.01f)] private float _shotInterval = 0.1f;
    [SerializeField, Min(0.01f)] private float _reloadDuration = 2f;
    [SerializeField, Min(1)] private int _headDamage = 100;
    [SerializeField, Min(1)] private int _torsoDamage = 25;
    [SerializeField, Min(1)] private int _armDamage = 18;
    [SerializeField, Min(1)] private int _legDamage = 18;
    [Tooltip("Per accepted shot: X = yaw degrees, Y = upward pitch kick degrees. Cycles after the last entry.")]
    [SerializeField] private Vector2[] _recoilPattern =
    {
        new Vector2(0f, 1f), new Vector2(0.2f, 1.1f),
        new Vector2(-0.2f, 1.2f), new Vector2(0.3f, 1.2f),
        new Vector2(-0.3f, 1.3f)
    };
    [SerializeField, Min(0.01f)] private float _recoilRecoveryDegreesPerSecond = 10f;

    public int MagazineCapacity => _magazineCapacity;
    public float Range => _range;
    public float ShotInterval => _shotInterval;
    public float ReloadDuration => _reloadDuration;
    public float RecoilRecoveryDegreesPerSecond => _recoilRecoveryDegreesPerSecond;
    public int RecoilPatternLength => _recoilPattern == null ? 0 : _recoilPattern.Length;

    public int GetDamage(CombatBodyPart part)
    {
        switch (part)
        {
            case CombatBodyPart.Head: return _headDamage;
            case CombatBodyPart.Torso: return _torsoDamage;
            case CombatBodyPart.Arm: return _armDamage;
            case CombatBodyPart.Leg: return _legDamage;
            default: return 0; // Unknown serialized values must never become torso damage.
        }
    }

    public Vector2 GetRecoilDelta(int acceptedShotIndex)
    {
        if (acceptedShotIndex < 0 || RecoilPatternLength == 0) return Vector2.zero;
        return _recoilPattern[acceptedShotIndex % RecoilPatternLength];
    }

    // Inspector Min attributes are not runtime validation. C2 must validate the
    // complete definition before accepting shots, rather than repair it mid-match.
    public bool TryValidate(out string error)
    {
        if (_magazineCapacity <= 0 || _headDamage <= 0 || _torsoDamage <= 0 || _armDamage <= 0 || _legDamage <= 0)
            error = "Magazine capacity and body-part damage must be positive.";
        else if (!IsPositiveFinite(_range) || !IsPositiveFinite(_shotInterval) || !IsPositiveFinite(_reloadDuration) || !IsPositiveFinite(_recoilRecoveryDegreesPerSecond))
            error = "Weapon distances and durations must be finite and positive.";
        else if (RecoilPatternLength == 0)
            error = "A recoil pattern is required.";
        else
        {
            foreach (var recoil in _recoilPattern)
            {
                if (float.IsNaN(recoil.x) || float.IsInfinity(recoil.x) || float.IsNaN(recoil.y) || float.IsInfinity(recoil.y) || recoil.y < 0f)
                {
                    error = "Recoil entries must be finite; upward pitch kick cannot be negative.";
                    return false;
                }
            }
            error = string.Empty;
            return true;
        }
        return false;
    }

    private static bool IsPositiveFinite(float value) => value > 0f && !float.IsInfinity(value) && !float.IsNaN(value);
}
