using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace AD
{
    /// <summary>Google Play login and recovery using the existing login Canvas.</summary>
    public class Login : MonoBehaviour
    {
        [Header("--- UI Elements ---")]
        [SerializeField] private GameObject _loading;
        [SerializeField] private TMPro.TMP_Text _loadingText;
        [SerializeField] private GameObject _retry;

        private TMPro.TMP_Text _retryMessage;
        private CancellationTokenSource _loginCancellation;
        private int _attemptId;
        private int _activeSceneHandle;
        private bool _busy;
        private bool _started;
#if UNITY_ANDROID
        private bool _googleAuthenticationPending;
        private int _googleRequestId;
#endif

        private void Awake()
        {
            if (_retry != null)
            {
                foreach (var text in _retry.GetComponentsInChildren<TMPro.TMP_Text>(true))
                {
                    // Keep the existing Retry button label; update only its message.
                    if (text.GetComponentInParent<UnityEngine.UI.Button>() == null)
                    {
                        _retryMessage = text;
                        break;
                    }
                }
            }
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChanged;
            if (_started) ShowRetryPanel("Login was interrupted. Please retry.");
        }

        private void Start()
        {
            _started = true;
            TryStartLogin(false);
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            CancelAttempt();
        }

        private void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            CancelAttempt();
        }

        private void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene previous,
            UnityEngine.SceneManagement.Scene next)
        {
            CancelAttempt();
        }

        public void RetryConnection() => TryStartLogin(true);

        private void TryStartLogin(bool manual)
        {
            if (!isActiveAndEnabled || _busy) return;
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                ShowRetryPanel("Please check your connection and try again.");
                return;
            }

            _busy = true;
            int attempt = ++_attemptId;
            _activeSceneHandle = UnityEngine.SceneManagement.SceneManager.GetActiveScene().handle;
            var cancellation = new CancellationTokenSource();
            _loginCancellation = cancellation;
            if (_retry != null) _retry.SetActive(false);
            if (_loading != null) _loading.SetActive(true);
            if (_loadingText != null) _loadingText.text = "Signing in...";
            RunLoginAsync(manual, attempt, cancellation).Forget();
        }

        private async UniTask RunLoginAsync(bool manual, int attempt, CancellationTokenSource cancellation)
        {
            CancellationToken token = cancellation.Token;
            try
            {
                // Windows standalone is used only as a development/QA client.
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                await GoLobbyScene(attempt, token);
#elif UNITY_ANDROID
                PlayGamesPlatform.DebugLogEnabled = false;
                PlayGamesPlatform.Activate();
                var platform = PlayGamesPlatform.Instance;
                if (!platform.IsAuthenticated())
                {
                    // A local timeout cannot cancel the SDK's account dialog/request.
                    // Do not stack another native request while its callback is outstanding.
                    if (_googleAuthenticationPending)
                    {
                        RecoverAttempt(attempt, "Google sign-in is still finishing.\nClose the Google dialog, then retry.\nIf it stays unresponsive, restart the app.");
                        return;
                    }
                    var status = await AuthenticateGoogleAsync(platform, manual, attempt, token);
                    if (!IsCurrentAttempt(attempt)) return;
                    if (status != SignInStatus.Success)
                    {
                        RecoverAttempt(attempt, status == SignInStatus.Canceled
                            ? "Google sign-in was canceled. Please retry."
                            : "Google sign-in failed. Please retry.");
                        return;
                    }
                }
                // Keep the current GPGS success -> Lobby path; no backend auth-code request.
                await GoLobbyScene(attempt, token);
#else
                RecoverAttempt(attempt, "Sign-in is not available on this platform.");
#endif
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) { }
            catch (TimeoutException)
            {
                RecoverAttempt(attempt, "Google sign-in timed out after 30 seconds.\nClose any Google dialog, then retry.");
            }
            catch (Exception)
            {
                // Never log SDK exceptions that might contain credentials or auth responses.
                if (IsCurrentAttempt(attempt)) Debug.LogWarning("[Login] Sign-in could not complete.");
                RecoverAttempt(attempt, "Sign-in could not complete. Please retry.");
            }
            finally
            {
                if (ReferenceEquals(_loginCancellation, cancellation))
                {
                    _loginCancellation = null;
                    cancellation.Dispose();
                }
            }
        }

#if UNITY_ANDROID
        private async UniTask<SignInStatus> AuthenticateGoogleAsync(PlayGamesPlatform platform,
            bool manual, int attempt, CancellationToken token)
        {
            var completion = new UniTaskCompletionSource<SignInStatus>();
            _googleAuthenticationPending = true;
            int request = ++_googleRequestId;
            Action<SignInStatus> callback = status =>
            {
                // Installed AndroidClient marshals this callback onto the Unity game thread.
                if (this == null || request != _googleRequestId || !_googleAuthenticationPending) return;
                _googleAuthenticationPending = false;
                if (IsCurrentAttempt(attempt)) completion.TrySetResult(status);
            };
            try
            {
                if (manual) platform.ManuallyAuthenticate(callback);
                else platform.Authenticate(callback);
            }
            catch
            {
                _googleAuthenticationPending = false;
                throw;
            }

            using (var timeout = CancellationTokenSource.CreateLinkedTokenSource(token))
            using (timeout.CancelAfterSlim(TimeSpan.FromSeconds(30), DelayType.Realtime))
            {
                try { return await completion.Task.AttachExternalCancellation(timeout.Token); }
                catch (OperationCanceledException) when (!token.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }
#endif

        private async UniTask GoLobbyScene(int attempt, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token);
            if (IsCurrentAttempt(attempt)) Managers.SceneM.ChangeScene(GameConstants.Scene.Lobby);
        }

        private bool IsCurrentAttempt(int attempt) => this != null && isActiveAndEnabled && _busy
            && attempt == _attemptId
            && UnityEngine.SceneManagement.SceneManager.GetActiveScene().handle == _activeSceneHandle;

        private void RecoverAttempt(int attempt, string message)
        {
            if (!IsCurrentAttempt(attempt)) return;
            CancelAttempt();
            ShowRetryPanel(message);
        }

        private void CancelAttempt()
        {
            ++_attemptId;
            _busy = false;
            var cancellation = _loginCancellation;
            _loginCancellation = null;
            if (cancellation == null) return;
            cancellation.Cancel();
            cancellation.Dispose();
        }

        private void ShowRetryPanel(string message)
        {
            if (_loadingText != null) _loadingText.text = message;
            if (_retryMessage != null) _retryMessage.text = message;
            if (_loading != null) _loading.SetActive(false);
            if (_retry != null) _retry.SetActive(true);
        }

        public void ClickedOK() => Managers.SoundM.UI_Ok();
    }
}
