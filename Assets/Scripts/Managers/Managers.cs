using System;
using System.Threading;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AD
{
    /// <summary>
    /// Manager 스크립트 관리
    /// </summary>
    public class Managers : MonoBehaviour
    {
        /// <summary>
        /// Singleton - 객체 오직 1
        /// Manager관련 script 모두 등록
        /// </summary>
        private static Managers _instance;
        public static Managers Instance => _instance;

        [SerializeField] private GameManager _gameM;
        public static GameManager GameM => _instance._gameM;

        private PoolManager _poolM = new PoolManager();
        public static PoolManager PoolM => _instance._poolM;

        [SerializeField] private PopupManager _popupM = null;
        public static PopupManager PopupM => _instance._popupM;

        private ResourceManager _resourceM = new ResourceManager();
        public static ResourceManager ResourceM => _instance._resourceM;

        [SerializeField] private SceneManager _sceneM = null;
        public static SceneManager SceneM => _instance._sceneM;

        [SerializeField] private SoundManager _soundM = null;
        public static SoundManager SoundM => _instance._soundM;

        [SerializeField] private UpdateManager _updateM = null;
        public static UpdateManager UpdateM => _instance._updateM;

        [SerializeField] private GameObject _networkRunnerObject = null;

        [Header("--- Managers data ---")]
        [Tooltip("Pool에 사용할 GameObject")]
        public GameObject[] PoolGameObjects = null;
        [Tooltip("Pool에 사용할 UI")]
        public GameObject[] PoolUIs = null;

        private const string LobbyScenePath = "Assets/Scenes/Lobby.unity";
        private const double ShutdownTimeoutSeconds = 30;
        private CancellationTokenSource _applicationCancellation;
        private CancellationToken _applicationToken;
        private bool _applicationStopping;
        private NetworkRunnerManager _registeredRunner;
        private long _sessionGeneration;
        private RecoveryContext _recovery;
        public string RecoveryStage { get; private set; } = "Idle";
        public string LastRecoveryError { get; private set; } = string.Empty;
        public long SessionGeneration => _sessionGeneration;
        public Task RecoveryTask => _recovery?.Task;
        private bool HasApplicationLifetime => this != null && ReferenceEquals(_instance, this)
            && Application.isPlaying && !_applicationStopping && !_applicationToken.IsCancellationRequested;

        private sealed class RecoveryContext
        {
            public NetworkRunnerManager Owner;
            public NetworkRunner Runner;
            public long Generation;
            public bool AutomaticShutdown;
            public bool ShutdownIssued;
            public bool AllowRunnerCreation;
            public NetworkRunnerManager Replacement;
            public Task Task;
        }

        private void Awake()
        {
            if (!Application.isPlaying) { _applicationStopping = true; return; }
            if (_instance != null && !ReferenceEquals(_instance, this))
            {
                _applicationStopping = true;
                Destroy(gameObject);
                return;
            }
            _instance = this;
            _applicationCancellation = new CancellationTokenSource();
            _applicationToken = _applicationCancellation.Token;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnEditorPlayModeChanged;
#endif
            DontDestroyOnLoad(this);
            // Scene Awake order is unspecified; cover a Runner that registered first.
            if (NetworkRunnerManager.Instance != null) RegisterNetworkRunner(NetworkRunnerManager.Instance);
        }

        private void Start()
        {
            if (!HasApplicationLifetime) return;
            PoolM.Init();
            SoundM.Init();
        }

        private void OnDestroy()
        {
            StopApplicationLifetime();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnEditorPlayModeChanged;
#endif
            var cancellation = _applicationCancellation;
            _applicationCancellation = null;
            if (cancellation != null) _ = DisposeAfterRecoveryAsync(cancellation, RecoveryTask);
            if (ReferenceEquals(_instance, this)) _instance = null;
        }

        private void OnApplicationQuit() => StopApplicationLifetime();
#if UNITY_EDITOR
        private void OnEditorPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode) StopApplicationLifetime();
        }
#endif
        private void StopApplicationLifetime()
        {
            if (_applicationStopping) return;
            _applicationStopping = true;
            if (_recovery != null && _recovery.Task != null && !_recovery.Task.IsCompleted)
                RecoveryStage = "Cancelled";
            try { _applicationCancellation?.Cancel(); }
            catch (AggregateException exception) { Debug.LogError($"[Network] Recovery cancellation exception: {exception.GetType().Name}"); }
        }

        private static async Task DisposeAfterRecoveryAsync(CancellationTokenSource cancellation, Task recovery)
        {
            // No Unity API access after the persistent owner has been destroyed.
            try { await (recovery ?? Task.CompletedTask).ConfigureAwait(false); }
            catch { /* Recovery reports live failures itself. */ }
            finally { cancellation.Dispose(); }
        }

        internal bool RegisterNetworkRunner(NetworkRunnerManager runner)
        {
            if (!HasApplicationLifetime) return false;
            if (_recovery != null && !_recovery.AllowRunnerCreation) return false;
            if (ReferenceEquals(_registeredRunner, runner)) return true;
            if (_registeredRunner != null && !_registeredRunner.IsRetired) return false;
            _registeredRunner = runner;
            ++_sessionGeneration;
            if (_recovery != null && _recovery.AllowRunnerCreation && _sessionGeneration == _recovery.Generation + 1)
                _recovery.Replacement = runner;
            return true;
        }

        internal Task RecoverLobbyAsync(NetworkRunnerManager owner, NetworkRunner runner, bool automaticShutdown)
        {
            if (!HasApplicationLifetime || !ReferenceEquals(_registeredRunner, owner)) return Task.CompletedTask;
            if (_recovery != null)
            {
                if (ReferenceEquals(_recovery.Owner, owner) && _recovery.Generation == _sessionGeneration)
                {
                    // A callback before our first yield may reveal SDK-owned shutdown.
                    // A callback caused by our explicit Shutdown(false) must not change its mode.
                    if (automaticShutdown && !_recovery.ShutdownIssued) _recovery.AutomaticShutdown = true;
                    return _recovery.Task;
                }
                if (_recovery.Task != null && !_recovery.Task.IsCompleted) return Task.CompletedTask;
            }
            var context = new RecoveryContext {
                Owner = owner, Runner = runner, Generation = _sessionGeneration,
                AutomaticShutdown = automaticShutdown
            };
            _recovery = context;
            LastRecoveryError = string.Empty;
            RecoveryStage = "Requested";
            context.Task = RecoverLobbyCoreAsync(context);
            return context.Task;
        }

        private bool CanRecover(RecoveryContext context) => HasApplicationLifetime
            && ReferenceEquals(_recovery, context) && context.Generation == _sessionGeneration
            && ReferenceEquals(_registeredRunner, context.Owner);

        private void SetRecoveryStage(string stage)
        {
            RecoveryStage = stage;
            Debug.Log($"[Network] Lobby recovery generation {_sessionGeneration}: {stage}");
        }

        private async Task RecoverLobbyCoreAsync(RecoveryContext context)
        {
            // Register the shared task before listeners or SDK callbacks can re-enter.
            await Task.Yield();
            if (!CanRecover(context)) return;
            var popup = _popupM;
            bool shutdownCompleted = false;
            bool replacementCreated = false;
            try
            {
                if (context.Owner != null) context.Owner.PublishLobbyReturn();
                if (!CanRecover(context)) return;
                if (popup != null) popup.PopupLoading();
                var deadline = System.Diagnostics.Stopwatch.StartNew();
                if (context.Runner != null && !context.AutomaticShutdown)
                {
                    SetRecoveryStage("WaitingForExplicitShutdown");
                    context.Runner.ProvideInput = false;
                    context.ShutdownIssued = true;
                    var shutdown = context.Runner.Shutdown(destroyGameObject: false);
                    // Observe eventual SDK faults even if app cancellation/timeout ends our wait.
                    _ = ObserveShutdownAsync(shutdown);
                    while (!shutdown.IsCompleted)
                    {
                        if (!CanRecover(context)) return;
                        if (deadline.Elapsed.TotalSeconds >= ShutdownTimeoutSeconds)
                            throw new TimeoutException("Explicit Runner shutdown did not finish within 30 seconds.");
                        await Task.Delay(16, _applicationToken);
                    }
                    await shutdown;
                }
                else if (context.Runner != null)
                {
                    SetRecoveryStage("WaitingForSdkRunnerDestruction");
                    // Fusion's automatic disconnect uses Shutdown(true, reason, true).
                    // Never issue a second shutdown to try to override its destruction flag.
                    while (context.Runner != null)
                    {
                        if (!CanRecover(context)) return;
                        if (deadline.Elapsed.TotalSeconds >= ShutdownTimeoutSeconds)
                            throw new TimeoutException("SDK shutdown did not destroy the old Runner within 30 seconds.");
                        await Task.Delay(16, _applicationToken);
                    }
                }
                if (!CanRecover(context)) return;
                shutdownCompleted = true;
                SetRecoveryStage("ClearingSession");
                if (context.Runner != null && context.Owner != null) context.Runner.RemoveCallbacks(context.Owner);
                if (_gameM != null) _gameM.ClearSession();
                if (context.Owner != null) context.Owner.ClearLobbyReturnState();
                if (!CanRecover(context)) return;
                if (SceneUtility.GetBuildIndexByScenePath(LobbyScenePath) < 0)
                    throw new InvalidOperationException($"The scene is not enabled in Build Settings: {LobbyScenePath}");
                SetRecoveryStage("LoadingLobby");
                var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(LobbyScenePath, LoadSceneMode.Single);
                if (loading == null) throw new InvalidOperationException("Unable to load the lobby scene.");
                while (!loading.isDone)
                {
                    await Task.Yield();
                    if (!CanRecover(context)) return;
                }
                if (!CanRecover(context)) return;
                // Finish old UI before the new Runner owns its connection popup.
                if (popup != null) popup.ClosePopupLoading();
                if (!CanRecover(context)) return;
                SetRecoveryStage("CreatingRunner");
                context.AllowRunnerCreation = true;
                CreateNetworkRunner();
                replacementCreated = HasApplicationLifetime && _sessionGeneration == context.Generation + 1
                    && _registeredRunner != null && !ReferenceEquals(_registeredRunner, context.Owner);
                if (!HasApplicationLifetime) return;
                if (!replacementCreated) throw new InvalidOperationException("The replacement lobby Runner was not registered.");
                SetRecoveryStage("Completed");
            }
            catch (OperationCanceledException) when (_applicationToken.IsCancellationRequested) { }
            catch (Exception exception)
            {
                // Instantiate can fail after Awake already registered the replacement.
                // Preserve that failure without granting the old context new-session authority.
                if (HasApplicationLifetime && ReferenceEquals(_recovery, context)
                    && (CanRecover(context) || (_sessionGeneration == context.Generation + 1
                        && ReferenceEquals(_registeredRunner, context.Replacement))))
                {
                    LastRecoveryError = $"{RecoveryStage}: {exception.Message}";
                    SetRecoveryStage("Failed");
                    context.AllowRunnerCreation = false;
                    Debug.LogError($"[Network] Lobby recovery failed: {LastRecoveryError}");
                    if (context.Owner != null) context.Owner.ReportLobbyRecoveryFailure(LastRecoveryError);
                }
            }
            finally
            {
                // A timeout leaves the old SDK owner untouched and creation blocked.
                if (HasApplicationLifetime && ReferenceEquals(_recovery, context))
                {
                    if (ReferenceEquals(context.Replacement, null) && popup != null) popup.ClosePopupLoading();
                    if (shutdownCompleted && context.Owner != null) Destroy(context.Owner.gameObject);
                }
            }
        }

        private static async Task ObserveShutdownAsync(Task shutdown)
        {
            try { await shutdown.ConfigureAwait(false); }
            catch { /* The recovery wait reports faults if its application lifetime is live. */ }
        }

        public static void CreateNetworkRunner()
        {
            if (_instance == null || _instance._networkRunnerObject == null)
            {
                Debug.LogError("[Network] NetworkRunner prefab is missing.");
                return;
            }

            if (!_instance.HasApplicationLifetime) return;
            if (_instance._recovery != null && !_instance._recovery.AllowRunnerCreation) return;

            var current = NetworkRunnerManager.Instance;
            if (current != null && !current.IsRetired)
            {
                return;
            }

            Instantiate(_instance._networkRunnerObject);
        }

        public static string SavedNickName { get; set; } = string.Empty;
    }
}
