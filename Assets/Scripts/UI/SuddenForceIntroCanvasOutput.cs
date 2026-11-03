using QuadD;
using UnityEngine;
using UnityEngine.Video;

// 프로젝트 소유 표시 adapter입니다. 패키지의 재생/오디오/종료/장면 전환은 그대로 둡니다.
[DefaultExecutionOrder(100)]
[DisallowMultipleComponent]
public sealed class SuddenForceIntroCanvasOutput : MonoBehaviour
{
    private QuadDIntro intro;
    private VideoPlayer video;
    private GameObject ownedCanvas;
    private Canvas output;
    private UnityEngine.UI.RawImage image;
    private UnityEngine.UI.AspectRatioFitter aspect;
    private bool completed;

    public bool HasBoundTexture { get; private set; }
    public bool OutputCompleted => completed;
    public bool CleanupFailed { get; private set; }

    private void Awake()
    {
        if (!isActiveAndEnabled || !Application.isPlaying || gameObject.scene.path != "Assets/Scenes/Intro.unity") return;
        completed = false;
        HasBoundTexture = false;
        CleanupFailed = false;
        intro = GetComponent<QuadDIntro>();
        video = GetComponent<VideoPlayer>();
        if (intro == null || video == null || GetComponent<Camera>() == null) return;
        try
        {
            ownedCanvas = new GameObject("Sudden Force Intro Output", typeof(RectTransform), typeof(Canvas));
            ownedCanvas.transform.SetParent(transform, false);
            output = ownedCanvas.GetComponent<Canvas>();
            output.renderMode = RenderMode.ScreenSpaceOverlay;
            output.sortingOrder = 32760;
            output.targetDisplay = GetComponent<Camera>().targetDisplay;

            var background = new GameObject("Black Background", typeof(RectTransform), typeof(UnityEngine.UI.Image));
            background.transform.SetParent(ownedCanvas.transform, false);
            Stretch(background.GetComponent<RectTransform>());
            var black = background.GetComponent<UnityEngine.UI.Image>();
            black.color = Color.black;
            black.raycastTarget = false;

            var picture = new GameObject("Video", typeof(RectTransform), typeof(UnityEngine.UI.RawImage), typeof(UnityEngine.UI.AspectRatioFitter));
            picture.transform.SetParent(ownedCanvas.transform, false);
            Stretch(picture.GetComponent<RectTransform>());
            image = picture.GetComponent<UnityEngine.UI.RawImage>();
            image.raycastTarget = false;
            image.color = Color.white;
            image.enabled = false; // 첫 decoded texture 전에는 검정 배경만 표시합니다.
            aspect = picture.GetComponent<UnityEngine.UI.AspectRatioFitter>();
            aspect.aspectMode = UnityEngine.UI.AspectRatioFitter.AspectMode.FitInParent;
            aspect.aspectRatio = 16f / 9f;
            intro.OnCompleted.AddListener(Complete);
        }
        catch { Complete(); }
    }

    private void LateUpdate()
    {
        if (completed || output == null || video == null) return;
        // 패키지 Start가 CameraNearPlane을 설정하므로 그 뒤 표시 경로만 바꿉니다.
        video.renderMode = VideoRenderMode.APIOnly;
        var texture = video.texture; // VideoPlayer 소유 texture: 해제/파괴하지 않습니다.
        if (texture == null || texture.width <= 0 || texture.height <= 0) return;
        image.texture = texture;
        aspect.aspectRatio = (float)texture.width / texture.height;
        image.enabled = true;
        HasBoundTexture = true; // 실제 픽셀 표시/영상 내용 검증은 아닙니다.
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(.5f, .5f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
    }

    private void Complete()
    {
        completed = true;
        // cleanup 예외가 패키지 OnCompleted 이후 Login 전환을 중단하지 않게 합니다.
        try { if (image != null) { image.texture = null; image.enabled = false; } }
        catch { CleanupFailed = true; }
        try { if (output != null) output.enabled = false; }
        catch { CleanupFailed = true; }
    }

    private void OnDisable()
    {
        Complete();
        if (intro != null) intro.OnCompleted.RemoveListener(Complete);
    }

    private void OnDestroy()
    {
        Complete();
        if (intro != null) intro.OnCompleted.RemoveListener(Complete);
        if (ownedCanvas != null) Destroy(ownedCanvas);
    }
}
