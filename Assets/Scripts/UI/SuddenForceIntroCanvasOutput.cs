using System.Collections;
using QuadD;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[DefaultExecutionOrder(100)]
[DisallowMultipleComponent]
public sealed class SuddenForceIntroCanvasOutput : MonoBehaviour
{
    [SerializeField] private RawImage picture;
    private const float AndroidPosterSeconds = 1.25f;
    private bool deferredStart;
    private QuadDIntro intro;
    private VideoPlayer video;

    public bool HasBoundTexture { get; private set; }
    public bool OutputCompleted { get; private set; }
    public bool CleanupFailed { get; private set; }

    private void Awake()
    {
        intro = GetComponent<QuadDIntro>();
        video = GetComponent<VideoPlayer>();
        if (intro == null) return;
        if (PlayerPrefs.GetFloat("BGM", 1f) <= 0f && PlayerPrefs.GetFloat("SFX", 1f) <= 0f)
            intro.Mute = true;
        intro.OnCompleted.AddListener(Complete);
        if (Application.platform == RuntimePlatform.Android && intro.enabled && picture != null
            && picture.isActiveAndEnabled && picture.texture != null && picture.canvas != null)
        {
            // The first Android frame can take longer than the decoder's entire prepare timeout.
            deferredStart = true;
            intro.enabled = false;
        }
    }

    private IEnumerator Start()
    {
        if (!deferredStart) yield break;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(AndroidPosterSeconds);
        StartDeferredIntro();
    }

    private void StartDeferredIntro()
    {
        if (!deferredStart) return;
        deferredStart = false;
        if (intro != null) intro.enabled = true;
    }

    private void LateUpdate()
    {
        if (OutputCompleted || picture == null || video == null) return;
        // QuadDIntro chooses its camera output in Start. The overlay displays its decoded texture.
        if (video.renderMode != VideoRenderMode.APIOnly) video.renderMode = VideoRenderMode.APIOnly;
        if (!video.isPrepared || !video.isPlaying || video.frame < 0) return;
        var texture = video.texture;
        if (texture == null || texture.width <= 0 || texture.height <= 0) return;
        if (picture.texture != texture) picture.texture = texture;
        HasBoundTexture = true;
    }

    private void Complete()
    {
        OutputCompleted = true;
        // Keep the poster on screen while Login loads if the decoder never produced a frame.
        if (HasBoundTexture) HidePicture();
    }

    private void HidePicture()
    {
        // A display cleanup error must not interrupt the package's login transition.
        try { if (picture != null) picture.enabled = false; }
        catch (MissingReferenceException) { CleanupFailed = true; }
    }

    private void OnDisable()
    {
        OutputCompleted = true;
        HidePicture();
        StartDeferredIntro();
        if (intro != null) intro.OnCompleted.RemoveListener(Complete);
    }
}
