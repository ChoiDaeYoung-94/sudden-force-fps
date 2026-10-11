#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Removes the authored console instance from release build scene copies.</summary>
public sealed class ReleaseDebugConsoleSceneFilter : IProcessSceneWithReport
{
    private const string ConsolePrefabGuid = "67117722a812a2e46ab8cb8eafbf5f5e";
    private const string InitializerScriptGuid = "69395e87b254782458bbe108ec22ec29";
    private const string ConsoleScriptGuid = "6a4f16ed905adcd4ab0d7c8c11f0d72c";
    public int callbackOrder => 0;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        ProcessScene(scene, report != null, report != null ? report.summary.options : BuildOptions.None);
    }

    // Shared by the callback and scene-copy verification without fabricating a BuildReport.
    internal static void ProcessScene(Scene scene, bool hasReport, BuildOptions options)
    {
        if (!hasReport || (options & BuildOptions.Development) != 0) return;
        if (!scene.IsValid() || !scene.isLoaded)
            throw new BuildFailedException("Release console filter requires a loaded build scene.");

        var prefabPath = AssetDatabase.GUIDToAssetPath(ConsolePrefabGuid);
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
            throw new BuildFailedException("The expected debug console prefab GUID is unavailable.");

        var consoles = new HashSet<GameObject>();
        var initializers = new List<RuntimeInitialize>();
        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                // Match the prefab's root object, not a name or any of its UI children.
                if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.gameObject) == prefab)
                    consoles.Add(transform.gameObject);
            }
            initializers.AddRange(root.GetComponentsInChildren<RuntimeInitialize>(true));
        }

        // Build processing can lose prefab object identity. The fallback below identifies
        // a direct reference plus exact script contract; it does not prove prefab provenance.
        // Validate the complete scene before changing any reference or GameObject.
        var referenceOwners = new Dictionary<GameObject, RuntimeInitialize>();
        foreach (var initializer in initializers)
        {
            if (!HasScriptGuid(initializer, InitializerScriptGuid))
                throw new BuildFailedException("RuntimeInitialize script identity is unexpected.");
            var serialized = new SerializedObject(initializer);
            var reference = serialized.FindProperty("_go_console");
            if (reference == null)
                throw new BuildFailedException("RuntimeInitialize console field is unavailable.");
            if (reference.objectReferenceValue == null) continue;
            var console = reference.objectReferenceValue as GameObject;
            if (console == null || console.scene != scene || console.transform.parent != null)
                throw new BuildFailedException("Console reference must target a root in the build scene.");
            if (initializer.transform.IsChildOf(console.transform))
                throw new BuildFailedException("Console target contains its RuntimeInitialize owner.");
            if (referenceOwners.ContainsKey(console))
                throw new BuildFailedException("Console reference has ambiguous RuntimeInitialize owners.");
            referenceOwners.Add(console, initializer);
            ValidateConsoleRoot(console, scene, initializers);
            consoles.Add(console);
        }

        foreach (var console in consoles) ValidateConsoleRoot(console, scene, initializers);
        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var behaviour in root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (HasScriptGuid(behaviour, ConsoleScriptGuid) && !consoles.Contains(behaviour.gameObject))
                    throw new BuildFailedException("An unidentified debug console remains in the build scene.");
            }
        }

        foreach (var initializer in initializers)
        {
            var serialized = new SerializedObject(initializer);
            serialized.FindProperty("_go_console").objectReferenceValue = null;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
        foreach (var console in consoles) Object.DestroyImmediate(console);

        foreach (var initializer in initializers)
        {
            if (initializer == null) continue;
            if (new SerializedObject(initializer).FindProperty("_go_console").objectReferenceValue != null)
                throw new BuildFailedException("Console reference remains in a release build scene.");
        }
        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var behaviour in root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (HasScriptGuid(behaviour, ConsoleScriptGuid))
                    throw new BuildFailedException("DebugLogManager remains in a release build scene.");
            }
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.gameObject) == prefab)
                    throw new BuildFailedException("Console prefab remains in a release build scene.");
            }
        }
    }

    private static void ValidateConsoleRoot(GameObject console, Scene scene, List<RuntimeInitialize> initializers)
    {
        if (console.scene != scene || console.transform.parent != null)
            throw new BuildFailedException("Console target is not a root in the build scene.");
        foreach (var initializer in initializers)
        {
            if (initializer.transform.IsChildOf(console.transform))
                throw new BuildFailedException("Console target contains RuntimeInitialize.");
        }
        var managerCount = 0;
        foreach (var behaviour in console.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (!HasScriptGuid(behaviour, ConsoleScriptGuid)) continue;
            if (behaviour.gameObject != console)
                throw new BuildFailedException("DebugLogManager must belong to the referenced console root.");
            managerCount++;
        }
        if (managerCount != 1)
            throw new BuildFailedException("Console root must have exactly one expected DebugLogManager script.");
    }

    private static bool HasScriptGuid(MonoBehaviour behaviour, string guid)
    {
        if (behaviour == null) return false;
        var script = MonoScript.FromMonoBehaviour(behaviour);
        return script != null && AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(script)) == guid;
    }
}
#endif
