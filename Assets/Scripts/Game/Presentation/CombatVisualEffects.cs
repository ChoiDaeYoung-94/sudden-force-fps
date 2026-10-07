using UnityEngine;

public sealed class CombatVisualEffects : MonoBehaviour
{
    [SerializeField] private GamePlayerNetworkData _owner;
    [SerializeField] private ParticleSystem _muzzlePrefab;
    [SerializeField] private ParticleSystem _impactPrefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shotClip;
    [SerializeField] private AudioClip _reloadClip;
    private CombatPresentation _presentation;
    private ParticleSystem _muzzle, _impact;
    private bool _wasReloading;
    public int ObservedShotCount { get; private set; }

    private void OnEnable()
    {
        CombatPresentation.PresentationBound += Bind;
        CombatPresentation.PresentationUnbound += Unbind;
        if (_owner != null) Bind(_owner.GetComponent<CombatPresentation>());
    }
    private void Bind(CombatPresentation presentation)
    {
        if (presentation == null || presentation.Owner != _owner || ReferenceEquals(_presentation, presentation)) return;
        Detach();
        _presentation = presentation;
        _presentation.ShotObserved += OnShot;
        _presentation.LocalVitalsChanged += OnVitals;
        if (_presentation.TryGetSnapshot(out var snapshot)) _wasReloading = snapshot.IsReloading;
    }
    private void OnShot(ShotObservedSnapshot shot)
    {
        ObservedShotCount++;
        if (_muzzle == null && _muzzlePrefab != null) _muzzle = Instantiate(_muzzlePrefab);
        if (_impact == null && _impactPrefab != null) _impact = Instantiate(_impactPrefab);
        Burst(_muzzle, shot.VisualMuzzlePosition, shot.Direction);
        if (shot.HasHit) Burst(_impact, shot.HitPoint + shot.HitNormal * .015f, shot.HitNormal);
        if (_audioSource != null && _shotClip != null)
        {
            _audioSource.spatialBlend = _owner.Object.HasInputAuthority ? 0f : 1f;
            _audioSource.PlayOneShot(_shotClip, .5f);
        }
    }
    private static void Burst(ParticleSystem effect, Vector3 position, Vector3 direction)
    {
        if (effect == null) return;
        effect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        effect.transform.SetPositionAndRotation(position, direction.sqrMagnitude > .001f ? Quaternion.LookRotation(direction) : Quaternion.identity);
        effect.Play(true);
    }
    private void OnVitals(CombatPresentationSnapshot snapshot)
    {
        if (snapshot.IsReloading && !_wasReloading && _audioSource != null && _reloadClip != null)
        {
            _audioSource.spatialBlend = 0f;
            _audioSource.PlayOneShot(_reloadClip, .35f);
        }
        _wasReloading = snapshot.IsReloading;
    }
    private void Unbind(CombatPresentation presentation)
    {
        if (ReferenceEquals(presentation, _presentation)) { Detach(); StopEffects(); }
    }
    private void Detach()
    {
        if (!ReferenceEquals(_presentation, null))
        {
            _presentation.ShotObserved -= OnShot;
            _presentation.LocalVitalsChanged -= OnVitals;
        }
        _presentation = null;
        _wasReloading = false;
    }
    private void StopEffects()
    {
        if (_muzzle != null) _muzzle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (_impact != null) _impact.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (_audioSource != null) _audioSource.Stop();
    }
    private void OnDisable()
    {
        CombatPresentation.PresentationBound -= Bind;
        CombatPresentation.PresentationUnbound -= Unbind;
        Detach(); StopEffects();
    }
    private void OnDestroy()
    {
        if (_muzzle != null) Destroy(_muzzle.gameObject);
        if (_impact != null) Destroy(_impact.gameObject);
    }
}
