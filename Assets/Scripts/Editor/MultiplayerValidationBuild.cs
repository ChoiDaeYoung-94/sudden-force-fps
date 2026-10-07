#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>Explicitly invoked only; never switches targets or starts a Player.</summary>
public static class MultiplayerValidationBuild
{
    private const BuildTarget Target = BuildTarget.StandaloneWindows64;
    private const string ExecutableName = "SuddenForceFPS.Validation.exe";

    // Dedicated batch Editor: -executeMethod MultiplayerValidationBuild.BuildFromCommandLine
    // -sfValidationRoot <absolute external directory> -sfValidationOutput <new child directory>
    public static void BuildFromCommandLine()
    {
        if (!Application.isBatchMode)
            throw new BuildFailedException("Validation command-line entry requires a batch Editor.");

        var exitCode = 1;
        try
        {
            var root = ValidateRoot(ReadArgument("-sfValidationRoot"));
            var output = FullLocalPath(ReadArgument("-sfValidationOutput"));
            var editorLog = FullLocalPath(ReadArgument("-logFile"));
            Require(IsChild(editorLog, root), "Editor log must be inside the allowed validation root.");
            Require(!IsWithin(editorLog, output), "Editor log must be outside the new build directory.");
            RejectReparsePoints(editorLog);
            BuildWindows64(root, output);
            exitCode = 0;
        }
        catch (BuildFailedException exception)
        {
            // All messages thrown by this script are fixed text, without credentials or paths.
            Debug.LogError("[MultiplayerValidationBuild] " + exception.Message);
        }
        catch (Exception exception)
        {
            Debug.LogError("[MultiplayerValidationBuild] Failed: " + exception.GetType().Name);
        }
        EditorApplication.Exit(exitCode);
    }

    // May also be invoked in the already-owned Editor after its target switch has completed.
    // Live invocation throws on failure and leaves the Editor open.
    public static void BuildWindows64(string allowedRoot, string outputDirectory)
    {
        var root = ValidateRoot(allowedRoot);
        var output = FullLocalPath(outputDirectory);
        Require(IsChild(output, root), "Build directory must be a child of the allowed validation root.");
        RejectReparsePoints(output);
        Require(!Directory.Exists(output) && !File.Exists(output), "Build directory must be new; existing outputs are preserved.");
        Require(!EditorApplication.isPlayingOrWillChangePlaymode, "Exit Play mode before building.");
        Require(!EditorApplication.isCompiling && !EditorApplication.isUpdating, "Wait for imports and compilation before building.");
        Require(!BuildPipeline.isBuildingPlayer, "Another Player build is already running.");
        Require(EditorUserBuildSettings.activeBuildTarget == Target, "Switch to Windows64 and finish compilation before invoking this build.");
        Require(BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, Target), "Windows64 build support is unavailable.");

        var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        Require(scenes.Length > 0, "No enabled project build scenes were found.");
        Require(scenes.All(scene => File.Exists(scene)), "An enabled build scene file is missing.");

        Directory.CreateDirectory(output);
        var record = new ValidationBuildRecord
        {
            startedUtc = DateTime.UtcNow.ToString("O"),
            unityVersion = Application.unityVersion,
            playerVersion = PlayerSettings.bundleVersion,
            target = Target.ToString(),
            development = true,
            scenes = scenes,
            result = "Running"
        };
        var manifest = Path.Combine(output, "validation-build.json");
        try
        {
            WriteRecord(manifest, record);
            var executable = Path.Combine(output, ExecutableName);
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = executable,
                target = Target,
                options = BuildOptions.Development | BuildOptions.CompressWithLz4 | BuildOptions.StrictMode
            });
            Require(report != null, "Unity returned no build report.");
            var summary = report.summary;
            record.result = summary.result.ToString();
            record.errors = summary.totalErrors;
            record.warnings = summary.totalWarnings;
            record.totalBytes = summary.totalSize.ToString();
            record.durationSeconds = summary.totalTime.TotalSeconds;
            Require(summary.result == BuildResult.Succeeded && summary.totalErrors == 0, "Windows64 development build did not succeed.");
            Require(File.Exists(executable), "Build reported success but the executable is missing.");

            // Hash the full Player, not just its launcher. The manifest itself is excluded.
            record.files = Directory.GetFiles(output, "*", SearchOption.AllDirectories)
                .Where(path => !string.Equals(path, manifest, StringComparison.OrdinalIgnoreCase))
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .Select(path => new ValidationBuildFile
                {
                    path = path.Substring(output.Length + 1).Replace('\\', '/'),
                    sha256 = HashFile(path)
                }).ToArray();
            record.success = true;
        }
        catch (Exception exception)
        {
            record.success = false;
            record.failureType = exception.GetType().Name;
            if (record.result == "Running") record.result = "Failed";
            // Never publish arbitrary SDK/compiler exception messages or signing settings.
            throw new BuildFailedException("Validation build failed; inspect validation-build.json and the local Editor log.");
        }
        finally
        {
            record.finishedUtc = DateTime.UtcNow.ToString("O");
            WriteRecord(manifest, record);
        }
        Debug.Log("[MultiplayerValidationBuild] Windows64 development build succeeded; manifest and Player hashes saved.");
    }

    private static string ValidateRoot(string value)
    {
        var root = FullLocalPath(value);
        var project = Path.GetFullPath(Path.Combine(Application.dataPath, "..")).TrimEnd('\\', '/');
        Require(!IsWithin(root, project) && !IsWithin(project, root), "Validation root must be separate from the project and its ancestors.");
        Require(!string.Equals(root, Path.GetPathRoot(root).TrimEnd('\\', '/'), StringComparison.OrdinalIgnoreCase), "A drive root is not an allowed validation directory.");
        RejectReparsePoints(root);
        return root;
    }

    private static string FullLocalPath(string value)
    {
        Require(!string.IsNullOrWhiteSpace(value) && value.Length >= 3 && char.IsLetter(value[0]) && value[1] == ':' && (value[2] == '\\' || value[2] == '/'), "Use an absolute local Windows path.");
        Require(value.IndexOf(':', 2) < 0, "Alternate stream paths are not allowed.");
        return Path.GetFullPath(value).TrimEnd('\\', '/');
    }

    private static bool IsChild(string path, string parent) => path.StartsWith(parent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    private static bool IsWithin(string path, string parent) => string.Equals(path, parent, StringComparison.OrdinalIgnoreCase) || IsChild(path, parent);

    private static void RejectReparsePoints(string path)
    {
        for (var current = path; !string.IsNullOrEmpty(current); current = Path.GetDirectoryName(current))
        {
            if (Directory.Exists(current) || File.Exists(current))
                Require((File.GetAttributes(current) & FileAttributes.ReparsePoint) == 0, "Validation paths cannot contain symbolic links or junctions.");
        }
    }

    private static string ReadArgument(string name)
    {
        var args = Environment.GetCommandLineArgs();
        var indices = Enumerable.Range(0, args.Length).Where(index => string.Equals(args[index], name, StringComparison.OrdinalIgnoreCase)).ToArray();
        Require(indices.Length == 1 && indices[0] + 1 < args.Length && !args[indices[0] + 1].StartsWith("-"), "A required validation argument is missing, duplicated, or has no value.");
        return args[indices[0] + 1];
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new BuildFailedException(message);
    }

    private static void WriteRecord(string path, ValidationBuildRecord record) => File.WriteAllText(path, JsonUtility.ToJson(record, true), new UTF8Encoding(false));

    private static string HashFile(string path)
    {
        using (var stream = File.OpenRead(path))
        using (var sha = SHA256.Create())
            return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
    }

    [Serializable]
    private sealed class ValidationBuildRecord
    {
        public bool success;
        public string startedUtc, finishedUtc, unityVersion, playerVersion, target, result, failureType, totalBytes;
        public bool development;
        public int errors, warnings;
        public double durationSeconds;
        public string[] scenes;
        public ValidationBuildFile[] files;
    }

    [Serializable]
    private sealed class ValidationBuildFile
    {
        public string path, sha256;
    }
}
#endif
