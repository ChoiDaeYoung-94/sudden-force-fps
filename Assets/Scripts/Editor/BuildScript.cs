using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    private const string ProductName = "SuddenForceFPS";
    private const string Identifier = "com.AeDeong.SuddenForceFPS";
    private const string DefaultKeyAlias = "aedeong";
    private const string BuildInfoPath = "BuildInfo/buildinfo.txt";
    private const string OutputDirectory = "Build/AOS";

    [MenuItem("Build/AOS/APK")]
    public static void BuildAOSAPK() => Run(false);

    [MenuItem("Build/AOS/AAB")]
    public static void BuildAOSAAB() => Run(true);

    private static void Run(bool appBundle)
    {
        bool success = false;
        try
        {
            BuildAndroid(appBundle);
            success = true;
        }
        catch (PreflightException exception)
        {
            Debug.LogError("[AndroidBuild] " + exception.Message);
        }
        catch (Exception exception)
        {
            // Do not echo exception values that could contain signing credentials.
            Debug.LogError("[AndroidBuild] Failed: " + exception.GetType().Name);
        }

        // Only a dedicated batch build process owns its lifetime. Menu builds stay open.
        if (Application.isBatchMode)
            EditorApplication.Exit(success ? 0 : 1);
        else if (!success)
            throw new BuildFailedException("Android build failed. See the build log.");
    }

    private static void BuildAndroid(bool appBundle)
    {
        Require(!EditorApplication.isPlaying && !EditorApplication.isCompiling && !EditorApplication.isUpdating,
            "Wait for a stable Editor outside PlayMode before building.");
        Require(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android,
            "Select Android first, or launch the batch process with -buildTarget Android.");
        Require(PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android) == Identifier,
            "Android package differs from the existing app; confirm it before building.");
        string keystorePath = ResolveExternalKeystore(
            Environment.GetEnvironmentVariable("SUDDEN_FORCE_KEYSTORE_PATH"), Directory.GetParent(Application.dataPath).FullName);
        string externalAlias = Environment.GetEnvironmentVariable("SUDDEN_FORCE_KEY_ALIAS");
        string keyAlias = externalAlias ?? DefaultKeyAlias;
        Require(!string.IsNullOrWhiteSpace(keyAlias), "The signing alias is empty.");
        Require(externalAlias != null || string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName)
            || PlayerSettings.Android.keyaliasName == keyAlias, "Set SUDDEN_FORCE_KEY_ALIAS for a different signing alias.");

        var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        Require(scenes.Length > 0 && scenes.All(File.Exists), "Enabled build scenes are missing or empty.");
        bool originalCustomKeystore = PlayerSettings.Android.useCustomKeystore;
        string originalKeystoreName = PlayerSettings.Android.keystoreName;
        string originalAliasName = PlayerSettings.Android.keyaliasName;
        string originalStorePassword = PlayerSettings.Android.keystorePass;
        string originalAliasPassword = PlayerSettings.Android.keyaliasPass;
        string storePassword = Environment.GetEnvironmentVariable("SUDDEN_FORCE_KEYSTORE_PASSWORD") ?? originalStorePassword;
        string aliasPassword = Environment.GetEnvironmentVariable("SUDDEN_FORCE_KEYALIAS_PASSWORD") ?? originalAliasPassword;
        Require(!string.IsNullOrEmpty(storePassword) && !string.IsNullOrEmpty(aliasPassword),
            "Provide existing signing credentials through the Editor or the two signing environment variables.");

        string sharedLedger = Environment.GetEnvironmentVariable("SUDDEN_FORCE_BUILD_LEDGER_PATH");
        Require(string.IsNullOrEmpty(sharedLedger) || Path.IsPathFullyQualified(sharedLedger),
            "SUDDEN_FORCE_BUILD_LEDGER_PATH must be an absolute file path.");
        int week = (int)((DateTime.UtcNow.Date - new DateTime(2023, 1, 21)).TotalDays / 7);
        AndroidBuildVersion version;
        try
        {
            version = AndroidBuildVersion.Reserve(BuildInfoPath, sharedLedger, PlayerSettings.Android.bundleVersionCode,
                Environment.GetEnvironmentVariable("SUDDEN_FORCE_VERSION_CODE"), week, OutputDirectory, appBundle);
        }
        catch (InvalidDataException exception) { throw new PreflightException(exception.Message); }
        catch (IOException) { throw new PreflightException("Could not reserve the Android version. Check the ledger, build output directory and concurrent builds."); }

        PlayerSettings.bundleVersion = version.Name;
        PlayerSettings.productName = ProductName;
        PlayerSettings.Android.bundleVersionCode = version.Code;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        EditorUserBuildSettings.buildAppBundle = appBundle;

        try
        {
            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = keystorePath;
            PlayerSettings.Android.keyaliasName = keyAlias;
            PlayerSettings.Android.keystorePass = storePassword;
            PlayerSettings.Android.keyaliasPass = aliasPassword;
            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                locationPathName = version.OutputPath,
                // Reprocess release scenes so cached player data cannot bypass scene filters.
                options = (appBundle ? BuildOptions.CompressWithLz4HC | BuildOptions.CleanBuildCache : BuildOptions.CompressWithLz4 | BuildOptions.Development)
                    | BuildOptions.DetailedBuildReport,
                // Adds only for this Player build; preserves all project defines such as DOTWEEN.
                extraScriptingDefines = appBundle ? Array.Empty<string>() : new[] { "Debug" }
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report == null)
                throw new BuildFailedException("Android BuildReport did not succeed.");
            ValidateBuildOutput(report.summary.result, report.summary.totalErrors, version.OutputPath);
            Debug.Log("[AndroidBuild] Succeeded: " + options.locationPathName + ", versionCode=" + version.Code);
        }
        finally
        {
            PlayerSettings.Android.keystoreName = originalKeystoreName;
            PlayerSettings.Android.keyaliasName = originalAliasName;
            PlayerSettings.Android.keystorePass = originalStorePassword;
            PlayerSettings.Android.keyaliasPass = originalAliasPassword;
            PlayerSettings.Android.useCustomKeystore = originalCustomKeystore;
        }
    }

    private static string ResolveExternalKeystore(string configuredPath, string projectDirectory)
    {
        Require(!string.IsNullOrWhiteSpace(configuredPath) && Path.IsPathFullyQualified(configuredPath),
            "Set SUDDEN_FORCE_KEYSTORE_PATH to an absolute path outside the project.");
        var file = new FileInfo(configuredPath);
        Require(file.Exists, "The external signing keystore is missing.");
        string project = Path.GetFullPath(projectDirectory);
        for (FileSystemInfo entry = file; entry != null;
             entry = entry is FileInfo key ? key.Directory : ((DirectoryInfo)entry).Parent)
        {
            Require((entry.Attributes & FileAttributes.ReparsePoint) == 0,
                "Use the external signing file directly instead of a link.");
            if (entry is DirectoryInfo directory)
            {
                string git = Path.Combine(directory.FullName, ".git");
                Require(!string.Equals(directory.FullName, project, StringComparison.OrdinalIgnoreCase)
                    && !File.Exists(git) && !Directory.Exists(git),
                    "Keep the signing keystore outside project and Git working directories.");
            }
        }
        return file.FullName;
    }

    private static void ValidateBuildOutput(BuildResult result, int totalErrors, string outputPath)
    {
        if (result != BuildResult.Succeeded || totalErrors != 0)
            throw new BuildFailedException("Android BuildReport must succeed without errors.");
        if (!File.Exists(outputPath) || new FileInfo(outputPath).Length == 0)
            throw new BuildFailedException("Android build did not produce a nonempty APK or AAB.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new PreflightException(message);
    }

    private sealed class PreflightException : Exception
    {
        public PreflightException(string message) : base(message) { }
    }
}
