// Android toolchain validation entry; the release build entry remains separate.
#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>Explicit development-only build with the existing Android debug key.</summary>
public static class AndroidValidationBuild
{
    private static bool pending;

    public static void Queue(string outputDirectory)
    {
        Require(!pending && !BuildPipeline.isBuildingPlayer, "Another build is running.");
        Require(!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling
            && !EditorApplication.isUpdating, "Wait for an idle Editor.");
        Require(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android, "Android must be the active target.");
        var output = Path.GetFullPath(outputDirectory);
        var project = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        Require(Path.IsPathRooted(outputDirectory) && !output.StartsWith(project + Path.DirectorySeparatorChar,
            StringComparison.OrdinalIgnoreCase), "Use an absolute output directory outside the project.");
        Require(!Directory.Exists(output) && !File.Exists(output), "Preserve existing validation outputs.");
        Require(File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".android", "debug.keystore")), "The existing Android debug key is unavailable.");
        pending = true;
        EditorApplication.CallbackFunction run = null;
        run = () => { EditorApplication.update -= run; Build(output); };
        EditorApplication.update += run;
    }

    private static void Build(string output)
    {
        bool customKey = PlayerSettings.Android.useCustomKeystore;
        bool appBundle = EditorUserBuildSettings.buildAppBundle;
        string version = PlayerSettings.bundleVersion;
        int code = PlayerSettings.Android.bundleVersionCode;
        var record = new Record { startedUtc = DateTime.UtcNow.ToString("O"), result = "Running",
            signing = "Existing Android debug key", targetSdk = (int)PlayerSettings.Android.targetSdkVersion,
            versionCode = Math.Max(3, code), version = version, development = true };
        Directory.CreateDirectory(output);
        string manifest = Path.Combine(output, "android-validation-build.json");
        string apk = Path.Combine(output, "SuddenForceFPS.DebugValidation.apk");
        try
        {
            PlayerSettings.Android.useCustomKeystore = false;
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.Android.bundleVersionCode = record.versionCode;
            record.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
            Require(record.scenes.Length > 0 && record.scenes.All(File.Exists), "Enabled scenes are missing.");
            Write(manifest, record);
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
                scenes = record.scenes, target = BuildTarget.Android, targetGroup = BuildTargetGroup.Android,
                locationPathName = apk, options = BuildOptions.Development | BuildOptions.CompressWithLz4
                    | BuildOptions.DetailedBuildReport, extraScriptingDefines = new[] { "Debug" }
            });
            Require(report != null, "No Android build report was returned.");
            record.result = report.summary.result.ToString();
            record.errors = report.summary.totalErrors;
            record.warnings = report.summary.totalWarnings;
            record.durationSeconds = report.summary.totalTime.TotalSeconds;
            record.totalBytes = report.summary.totalSize.ToString();
            record.success = report.summary.result == BuildResult.Succeeded && File.Exists(apk);
        }
        catch (Exception exception)
        {
            record.failureType = exception.GetType().Name;
            record.result = "Failed";
            Debug.LogError("[AndroidValidationBuild] Failed: " + record.failureType);
        }
        finally
        {
            PlayerSettings.Android.useCustomKeystore = customKey;
            EditorUserBuildSettings.buildAppBundle = appBundle;
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = code;
            AssetDatabase.SaveAssets();
            record.temporarySettingsRestored = PlayerSettings.Android.useCustomKeystore == customKey
                && EditorUserBuildSettings.buildAppBundle == appBundle && PlayerSettings.bundleVersion == version
                && PlayerSettings.Android.bundleVersionCode == code;
            record.finishedUtc = DateTime.UtcNow.ToString("O");
            Write(manifest, record);
            pending = false;
        }
        Debug.Log("[AndroidValidationBuild] Result=" + record.result + "; temporary settings restored=" + record.temporarySettingsRestored);
    }

    private static void Require(bool value, string message) { if (!value) throw new BuildFailedException(message); }
    private static void Write(string path, Record record) => File.WriteAllText(path, JsonUtility.ToJson(record, true));
    [Serializable] private sealed class Record
    {
        public string startedUtc, finishedUtc, result, signing, version, totalBytes, failureType;
        public string[] scenes;
        public int targetSdk, versionCode;
        public int errors, warnings;
        public double durationSeconds;
        public bool development, success, temporarySettingsRestored;
    }
}
#endif
