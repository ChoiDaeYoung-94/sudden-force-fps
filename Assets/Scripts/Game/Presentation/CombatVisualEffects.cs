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
    public int PresentedLocalShotCount { get; private set; }
    public int PresentedRemoteShotCount { get; private set; }
    public int ConfirmedImpactCount { get; private set; }
    public int PresentedReloadCount { get; private set; }

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
        _presentation.LocalShot += OnLocalShot;
        WarmEffects();
        _presentation.LocalVitalsChanged += OnVitals;
        if (_presentation.TryGetSnapshot(out var snapshot)) _wasReloading = snapshot.IsReloading;
    }
    private void OnShot(ShotObservedSnapshot shot)
    {
        ObservedShotCount++;
        // The owner already heard/saw its forward prediction (including host).
        // Authoritative observations still supply every impact and remote shot.
        if (!_owner.Object.HasInputAuthority)
        {
            PresentedRemoteShotCount++;
            PlayMuzzleAndSound(shot.VisualMuzzlePosition, shot.Direction, false);
        }
        if (shot.HasHit)
        {
            ConfirmedImpactCount++;
            Burst(_impact, shot.HitPoint + shot.HitNormal * .015f, shot.HitNormal);
        }
    }
    private void OnLocalShot(LocalShotSnapshot shot)
    {
        if (_owner == null || _owner.Object == null || !_owner.Object.IsValid
            || !_owner.Object.HasInputAuthority || shot.RespawnVersion != _owner.RespawnVersion) return;
        PresentedLocalShotCount++;
        var muzzle = _owner.ViewMuzzle;
        PlayMuzzleAndSound(muzzle != null ? muzzle.position : shot.Origin, shot.Direction, true);
    }
    private void WarmEffects()
    {
        if (_muzzle == null && _muzzlePrefab != null)
        {
            _muzzle = Instantiate(_muzzlePrefab);
            _muzzle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (_impact == null && _impactPrefab != null)
        {
            _impact = Instantiate(_impactPrefab);
            _impact.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        // The authored PCM clip has preloadAudioData=false. Move first-use work
        // to binding instead of the first accepted shot; no importer rewrite.
        if (_shotClip != null && _shotClip.loadState == AudioDataLoadState.Unloaded) _shotClip.LoadAudioData();
        if (_reloadClip != null && _reloadClip.loadState == AudioDataLoadState.Unloaded) _reloadClip.LoadAudioData();
    }
    private void PlayMuzzleAndSound(Vector3 position, Vector3 direction, bool local)
    {
        Burst(_muzzle, position, direction);
        if (_audioSource == null || _shotClip == null) return;
        _audioSource.spatialBlend = local ? 0f : 1f;
        _audioSource.PlayOneShot(_shotClip, .5f);
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
        if (snapshot.IsReloading && snapshot.ReloadRemaining > 0f && !snapshot.IsDead && !snapshot.RespawnPending && !_wasReloading
            && _audioSource != null && _audioSource.isActiveAndEnabled && _reloadClip != null)
        {
            bool local = _owner != null && _owner.Object != null && _owner.Object.IsValid
                && _owner.Object.HasInputAuthority;
            _audioSource.spatialBlend = local ? 0f : 1f;
            _audioSource.PlayOneShot(_reloadClip, .35f);
            PresentedReloadCount++;
        }
        _wasReloading = snapshot.IsReloading;
    }
    private void LateUpdate()
    {
        // Local actors already receive LocalVitalsChanged. Proxies do not.
        if (_owner == null || _owner.Object == null || !_owner.Object.IsValid
            || _owner.Object.HasInputAuthority || _presentation == null) return;
        if (_presentation.TryGetSnapshot(out var snapshot)) OnVitals(snapshot);
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
            _presentation.LocalShot -= OnLocalShot;
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
