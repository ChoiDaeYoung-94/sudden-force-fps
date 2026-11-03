// _go_console never used warning
#pragma warning disable 0414

using UnityEngine;
using UnityEngine.SceneManagement;

public class RuntimeInitialize : MonoBehaviour
{
    [SerializeField] GameObject _go_console = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void FirstLoad()
    {
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().name.CompareTo("Test") == 0)
            return;

        // 빌드 첫 Intro만 허용하고 기존 개발 씬의 Login 진입 정책은 유지합니다.
        if (SceneManager.GetActiveScene().path == "Assets/Scenes/Intro.unity")
            return;

        if (SceneManager.GetActiveScene().name.CompareTo("Login") != 0)
            SceneManager.LoadScene("Login");
#endif
    }

    private void Awake()
    {
#if DEVELOPMENT_BUILD
        this._go_console.SetActive(true);
#endif
    }
}