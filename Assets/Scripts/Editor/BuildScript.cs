using System;
using System.Globalization;
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
    private const string KeystorePath = "src/AeDeong.keystore";
    private const string KeyAlias = "aedeong";
    private const string BuildInfoPath = "BuildInfo/buildinfo.txt";
    private const string OutputDirectory = "Build/AOS";
    private const int MaximumVersionCode = 2100000000;

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
        Require(File.Exists(KeystorePath), "The existing signing keystore is missing.");
        Require(string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) || PlayerSettings.Android.keyaliasName == KeyAlias,
            "The configured signing alias differs from the existing alias.");

        var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        Require(scenes.Length > 0 && scenes.All(File.Exists), "Enabled build scenes are missing or empty.");
        var ledger = ReadLedger();
        int knownCode = Math.Max(ledger[2], PlayerSettings.Android.bundleVersionCode);
        string requestedCode = Environment.GetEnvironmentVariable("SUDDEN_FORCE_VERSION_CODE");
        int versionCode;
        if (!string.IsNullOrEmpty(requestedCode))
        {
            Require(int.TryParse(requestedCode, NumberStyles.None, CultureInfo.InvariantCulture, out versionCode)
                && versionCode > knownCode && versionCode <= MaximumVersionCode,
                "SUDDEN_FORCE_VERSION_CODE must exceed the local ledger and PlayerSettings code, within Play's limit.");
        }
        else
        {
            Require(knownCode < MaximumVersionCode, "Android versionCode has reached Play's limit.");
            versionCode = checked(knownCode + 1);
        }

        string originalStorePassword = PlayerSettings.Android.keystorePass;
        string originalAliasPassword = PlayerSettings.Android.keyaliasPass;
        string storePassword = Environment.GetEnvironmentVariable("SUDDEN_FORCE_KEYSTORE_PASSWORD") ?? originalStorePassword;
        string aliasPassword = Environment.GetEnvironmentVariable("SUDDEN_FORCE_KEYALIAS_PASSWORD") ?? originalAliasPassword;
        Require(!string.IsNullOrEmpty(storePassword) && !string.IsNullOrEmpty(aliasPassword),
            "Provide existing signing credentials through the Editor or the two signing environment variables.");

        int week = Math.Max(ledger[0], (int)((DateTime.UtcNow.Date - new DateTime(2023, 1, 21)).TotalDays / 7));
        int build = week == ledger[0] ? checked(ledger[1] + 1) : 0;
        string version = "1.0." + week.ToString(CultureInfo.InvariantCulture);
        Directory.CreateDirectory(OutputDirectory);
        // Reserve a new code before building; failed attempts may leave a harmless gap.
        File.WriteAllText(BuildInfoPath, string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", week, build, versionCode));
        PlayerSettings.bundleVersion = version;
        PlayerSettings.productName = ProductName;
        PlayerSettings.Android.bundleVersionCode = versionCode;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = KeystorePath;
        PlayerSettings.Android.keyaliasName = KeyAlias;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        EditorUserBuildSettings.buildAppBundle = appBundle;

        try
        {
            PlayerSettings.Android.keystorePass = storePassword;
            PlayerSettings.Android.keyaliasPass = aliasPassword;
            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                locationPathName = Path.Combine(OutputDirectory, version + "." + build + (appBundle ? ".aab" : ".apk")),
                options = appBundle ? BuildOptions.CompressWithLz4HC : BuildOptions.CompressWithLz4 | BuildOptions.Development,
                // Adds only for this Player build; preserves all project defines such as DOTWEEN.
                extraScriptingDefines = appBundle ? Array.Empty<string>() : new[] { "Debug" }
            };
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report == null || report.summary.result != BuildResult.Succeeded)
                throw new BuildFailedException("Android BuildReport did not succeed.");
            Debug.Log("[AndroidBuild] Succeeded: " + options.locationPathName + ", versionCode=" + versionCode);
        }
        finally
        {
            PlayerSettings.Android.keystorePass = originalStorePassword;
            PlayerSettings.Android.keyaliasPass = originalAliasPassword;
        }
    }

    private static int[] ReadLedger()
    {
        Require(File.Exists(BuildInfoPath), "BuildInfo/buildinfo.txt is missing.");
        var fields = File.ReadAllText(BuildInfoPath).Trim().Split(',');
        Require(fields.Length == 3, "Build ledger must contain week,build,versionCode.");
        var ledger = new int[3];
        for (int i = 0; i < ledger.Length; i++)
            Require(int.TryParse(fields[i], NumberStyles.None, CultureInfo.InvariantCulture, out ledger[i]) && ledger[i] >= 0,
                "Build ledger values must be nonnegative integers.");
        Require(ledger[2] <= MaximumVersionCode, "Build ledger versionCode exceeds Play's limit.");
        return ledger;
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
