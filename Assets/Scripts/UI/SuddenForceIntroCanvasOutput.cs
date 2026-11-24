using QuadD;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[DefaultExecutionOrder(100)]
[DisallowMultipleComponent]
public sealed class SuddenForceIntroCanvasOutput : MonoBehaviour
{
    [SerializeField] private RawImage picture;
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
        // A display cleanup error must not interrupt the package's login transition.
        try { if (picture != null) picture.enabled = false; }
        catch (MissingReferenceException) { CleanupFailed = true; }
    }

    private void OnDisable()
    {
        Complete();
        if (intro != null) intro.OnCompleted.RemoveListener(Complete);
    }
}
