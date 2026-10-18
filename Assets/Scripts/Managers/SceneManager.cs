using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace AD
{
    /// <summary>Loads a new scene additively, then unloads the previous scene.</summary>
    public class SceneManager : MonoBehaviour
    {
        private CancellationTokenSource _ctsSceneChange;
        private bool _sceneChangeInProgress;

        private void Awake()
        {
            _ctsSceneChange = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _ctsSceneChange?.Cancel();
            _ctsSceneChange?.Dispose();
            _ctsSceneChange = null;
        }

        public void ChangeScene(GameConstants.Scene targetScene)
        {
            TryChangeSceneAsync(targetScene).Forget();
        }

        public async UniTask<bool> TryChangeSceneAsync(GameConstants.Scene targetScene)
        {
            if (_sceneChangeInProgress || _ctsSceneChange == null
                || _ctsSceneChange.IsCancellationRequested) return false;
            _sceneChangeInProgress = true;
            CancellationToken token = _ctsSceneChange.Token;
            string targetName = targetScene.ToString();
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var popup = Managers.Instance != null ? Managers.PopupM : null;
            var sound = Managers.Instance != null ? Managers.SoundM : null;
            bool loadingShown = false;
            bool bgmPaused = false;
            bool sourceUnloadStarted = false;
            bool targetLoadStarted = false;
            try
            {
                if (!currentScene.IsValid() || !currentScene.isLoaded
                    || !Application.CanStreamedLevelBeLoaded(targetName)) return false;
                if (currentScene.name == targetName) return true;
                var existingTarget = UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetName);
                if (existingTarget.IsValid() && existingTarget.isLoaded) return false;

                if (popup != null)
                {
                    popup.PopupSceneLoading();
                    loadingShown = true;
                }
                if (sound != null)
                {
                    sound.PauseBGM();
                    bgmPaused = true;
                }
                token.ThrowIfCancellationRequested();
                var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                    targetName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                if (loading == null) throw new InvalidOperationException("Scene loading did not start.");
                targetLoadStarted = true;
                // Cancellation ends our wait, not Unity's native operation. Leave scene
                // activation enabled so a cancelled owner cannot stall Unity's queue.
                while (!loading.isDone) await UniTask.Yield(PlayerLoopTiming.Update, token);
                token.ThrowIfCancellationRequested();
                var loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetName);
                if (!loadedScene.IsValid() || !loadedScene.isLoaded
                    || !UnityEngine.SceneManagement.SceneManager.SetActiveScene(loadedScene))
                    throw new InvalidOperationException("The target scene is not ready.");

                var unloading = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
                if (unloading == null) throw new InvalidOperationException("Source scene unloading did not start.");
                sourceUnloadStarted = true;
                while (!unloading.isDone) await UniTask.Yield(PlayerLoopTiming.Update, token);
                token.ThrowIfCancellationRequested();
                // The scene transition is committed once the old scene is unloaded.
                // Optional unused-asset cleanup must not turn it into a login retry.
                try { await Resources.UnloadUnusedAssets().ToUniTask(cancellationToken: token); }
                catch (OperationCanceledException) when (token.IsCancellationRequested) { return false; }
                catch (Exception exception)
                {
                    Debug.LogWarning($"[Scene] Unused asset cleanup failed: {exception.GetType().Name}");
                }
                return true;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                return false;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[Scene] Transition failed: {exception.GetType().Name}");
                // Before source unloading starts, restore the original active scene so
                // the still-live Login can show retry. Never unload an existing target.
                if (!sourceUnloadStarted && currentScene.IsValid() && currentScene.isLoaded)
                {
                    try
                    {
                        UnityEngine.SceneManagement.SceneManager.SetActiveScene(currentScene);
                        var failedTarget = UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetName);
                        if (targetLoadStarted && failedTarget.IsValid() && failedTarget.isLoaded)
                        {
                            var cleanup = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(failedTarget);
                            if (cleanup != null)
                                while (!cleanup.isDone) await UniTask.Yield(PlayerLoopTiming.Update, token);
                        }
                    }
                    catch (Exception cleanupException)
                    {
                        Debug.LogWarning($"[Scene] Failed scene cleanup: {cleanupException.GetType().Name}");
                    }
                }
                return false;
            }
            finally
            {
                try
                {
                    if (loadingShown && popup != null) popup.ClosePopupSceneLoading();
                }
                finally
                {
                    try { if (bgmPaused && sound != null) sound.UnpauseBGM(); }
                    finally { _sceneChangeInProgress = false; }
                }
            }
        }
    }
}