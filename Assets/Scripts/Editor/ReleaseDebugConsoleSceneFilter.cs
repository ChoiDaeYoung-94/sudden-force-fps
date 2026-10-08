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

        // Validate every reference before changing the build copy. An unpacked/unexpected
        // target must fail closed instead of removing a similarly named GameObject.
        foreach (var initializer in initializers)
        {
            var serialized = new SerializedObject(initializer);
            var reference = serialized.FindProperty("_go_console");
            if (reference == null)
                throw new BuildFailedException("RuntimeInitialize console field is unavailable.");
            if (reference.objectReferenceValue != null
                && !consoles.Contains(reference.objectReferenceValue as GameObject))
                throw new BuildFailedException("Console reference does not resolve to the expected prefab root.");
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
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
            {
                if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.gameObject) == prefab)
                    throw new BuildFailedException("Console prefab remains in a release build scene.");
            }
        }
    }
}
#endif
