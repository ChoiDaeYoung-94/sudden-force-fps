"""Check an existing Unity Gradle export with dry-run only; never reserve or sign."""
from __future__ import annotations

import argparse
import hashlib
import json
import os
from pathlib import Path
import subprocess
import sys

SUPPORTED_GRADLE = "9.3.1"
SUPPORTED_AGP = "9.0.0"
JVM_OPTIONS = ("JAVA_TOOL_OPTIONS", "_JAVA_OPTIONS", "JDK_JAVA_OPTIONS", "GRADLE_OPTS")
KEY_SUFFIXES = {".keystore", ".jks", ".p12", ".pfx"}


class PreflightError(Exception):
    pass


def require(condition, message):
    if not condition:
        raise PreflightError(message)


def sha(path):
    digest = hashlib.sha256()
    with path.open("rb") as stream:
        for chunk in iter(lambda: stream.read(1024 * 1024), b""):
            digest.update(chunk)
    return digest.hexdigest()


def snapshot(root, excluded=()):
    """Hash inputs without opening signing keys or following directory links."""
    result = {}
    for current, dirs, names in os.walk(root, followlinks=False):
        dirs[:] = sorted(d for d in dirs if d not in excluded
                         and not (Path(current) / d).is_symlink()
                         and not os.path.isjunction(Path(current) / d))
        for name in sorted(names):
            path = Path(current) / name
            if path.suffix.lower() not in KEY_SUFFIXES and not path.is_symlink():
                result[path.relative_to(root).as_posix()] = sha(path)
    return result


def project_snapshot(project):
    return {folder: snapshot(project / folder) for folder in
            ("Assets", "Packages", "ProjectSettings", "BuildInfo")}


def is_inside(path, root):
    return path == root or root in path.parents


def child_environment(parent, args, report):
    require(not any(value for key, value in parent.items() if key.upper() in JVM_OPTIONS),
            "Inherited JVM options are present; inspect them without printing values.")
    require(not any(key.upper().startswith("ORG_GRADLE_PROJECT_") for key in parent),
            "Inherited Gradle project properties are present.")
    env = {key: value for key, value in parent.items()
           if key.upper() not in JVM_OPTIONS
           and not any(word in key.upper() for word in ("PASSWORD", "TOKEN", "SECRET"))
           and key.upper() not in {"SFF_STOREPASS", "SFF_KEYPASS"}
           and not key.upper().startswith("SFF_PREFLIGHT_")}
    env.update(JAVA_HOME=str(args.java_home), GRADLE_USER_HOME=str(args.gradle_user_home),
               ANDROID_SDK_ROOT=str(args.sdk), ANDROID_HOME=str(args.sdk),
               ANDROID_NDK_ROOT=str(args.ndk), ANDROID_NDK_HOME=str(args.ndk),
               NDK_ROOT=str(args.ndk), ANDROID_USER_HOME=str(args.android_user_home),
               DOTNET_PROCESSOR_COUNT="1", SUDDEN_FORCE_PROJECT_PATH=str(args.project))
    env.update(SFF_PREFLIGHT_REPORT=str(report), SFF_PREFLIGHT_EXPORT=str(args.export),
               SFF_PREFLIGHT_CMAKE=str(args.cmake_staging),
               SFF_PREFLIGHT_CODE=str(args.expected_code),
               SFF_PREFLIGHT_NAME=args.expected_name,
               SFF_PREFLIGHT_PACKAGE="com.AeDeong.SuddenForceFPS")
    return env


def reject_extra_init_scripts(args):
    candidates = [args.gradle_user_home / "init.gradle", args.gradle_user_home / "init.gradle.kts"]
    for home in (args.gradle_user_home, args.gradle_home):
        folder = home / "init.d"
        if folder.exists():
            candidates.extend(path for path in folder.rglob("*")
                              if path.name.endswith((".gradle", ".gradle.kts")))
    require(not any(path.exists() for path in candidates), "Additional Gradle init scripts are present.")


def command(args):
    java = args.java_home / "bin" / ("java.exe" if os.name == "nt" else "java")
    jar = args.gradle_home / "lib" / f"gradle-launcher-{SUPPORTED_GRADLE}.jar"
    require(java.is_file() and jar.is_file(), "Pinned Java/Gradle launcher is missing.")
    return [str(java), "-XX:TieredStopAtLevel=1", "-XX:ActiveProcessorCount=1",
            "-Dorg.gradle.jvmargs=-Xmx4096M -XX:TieredStopAtLevel=1 -XX:ActiveProcessorCount=1",
            "-classpath", str(jar), "org.gradle.launcher.GradleMain",
            "--offline", "--no-daemon", "--no-configuration-cache", "--console=plain",
            "--max-workers=1", "-Dorg.gradle.parallel=false",
            "-Pandroid.builder.sdkDownload=false", "--init-script",
            str(Path(__file__).with_name("unsigned-preflight.init.gradle")),
            ":launcher:bundleRelease", "--dry-run"]


def validate_events(events, args):
    def one(kind):
        matches = [event for event in events if event.get("kind") == kind]
        require(len(matches) == 1, f"Expected one {kind} event.")
        return matches[0]
    jvm = one("jvm")
    require(jvm.get("c1") is True and jvm.get("heapBytes") == 4294967296
            and jvm.get("processors") == 1 and jvm.get("workers") == 1
            and jvm.get("parallel") is False and jvm.get("dryRun") is True,
            "Actual Gradle JVM or dry-run gate failed.")
    require(one("toolchain").get("gradle") == SUPPORTED_GRADLE, "Gradle version changed.")
    release = one("release")
    require(release.get("agp") == SUPPORTED_AGP
            and release.get("package") == "com.AeDeong.SuddenForceFPS"
            and release.get("code") == args.expected_code
            and release.get("name") == args.expected_name
            and release.get("unsigned") is True and release.get("debuggable") is False,
            "Existing release identity/unsigned gate failed.")
    require(one("graph").get("unsignedFinalizers") == 1
            and one("graph").get("blockedSigningTasks") == 0,
            "Unsigned task graph gate failed.")
    cmake = [event for event in events if event.get("kind") == "cmake"]
    require(bool(cmake) and all(event.get("matches") is True for event in cmake),
            "Expected CMake staging check is missing or failed.")
    outputs = one("outputs").get("identities", [])
    require(bool(outputs) and all(output.get("code") == args.expected_code
                                 and output.get("name") == args.expected_name for output in outputs),
            "Final release output identity differs from existing export.")
    require(not any(event.get("kind") == "taskStarted" for event in events),
            "A Gradle task executed during dry-run.")
    require(one("finished").get("failed") is False, "Gradle build lifecycle failed.")


def parse_args(argv=None):
    parser = argparse.ArgumentParser(description=__doc__)
    for name in ("project", "export", "java-home", "gradle-home", "gradle-user-home",
                 "sdk", "ndk", "android-user-home", "cmake-staging", "report-directory"):
        parser.add_argument("--" + name, required=True, type=lambda p: Path(p).resolve())
    parser.add_argument("--expected-code", required=True, type=int)
    parser.add_argument("--expected-name", required=True)
    return parser.parse_args(argv)


def run(args):
    require(args.expected_code > 0, "Expected code must be positive.")
    require(args.project.is_dir() and args.export.is_dir(), "Project/export does not exist.")
    require((args.project / "BuildInfo/buildinfo.txt").is_file(), "Build ledger is missing.")
    for name in ("build.gradle", "settings.gradle", "launcher/build.gradle", "unityLibrary/build.gradle"):
        require((args.export / name).is_file(), "Incomplete Gradle export.")
    require(args.sdk.is_dir() and args.ndk.is_dir(), "SDK/NDK path is missing.")
    for path in (args.report_directory, args.gradle_user_home, args.android_user_home):
        require(not is_inside(path, args.project) and not is_inside(path, args.export),
                "Reports and writable tool caches must be outside project/export.")
    require(not args.report_directory.exists(), "Report directory already exists; do not overwrite evidence.")
    reject_extra_init_scripts(args)
    cmd = command(args)
    env = child_environment(os.environ, args, args.report_directory / "events.jsonl")
    before = project_snapshot(args.project)
    export_before = snapshot(args.export, {"build", ".gradle", ".cxx"})
    bundles_before = {p.relative_to(args.export).as_posix(): sha(p) for p in args.export.rglob("*.aab")}
    args.report_directory.mkdir(parents=True)
    (args.report_directory / "inputs-before.json").write_text(
        json.dumps({"project": before, "export": export_before, "aab": bundles_before}, indent=2), encoding="utf-8")
    result = {"passed": False, "dryRunOnly": True, "buildOrSigningPerformed": False,
              "versionReservationPerformed": False, "expectedExportCode": args.expected_code,
              "expectedExportName": args.expected_name, "gradleCacheMayChange": True}
    try:
        # Output may contain signing DSL values from third-party plugins: do not log it.
        proc = subprocess.run(cmd, cwd=args.export, env=env, stdout=subprocess.DEVNULL,
                              stderr=subprocess.DEVNULL, check=False,
                              creationflags=subprocess.CREATE_NO_WINDOW if os.name == "nt" else 0)
        result["exitCode"] = proc.returncode
        require(proc.returncode == 0, "Gradle dry-run failed; see allowlisted event report.")
        events = [json.loads(line) for line in (args.report_directory / "events.jsonl")
                  .read_text(encoding="utf-8").splitlines() if line.strip()]
        validate_events(events, args)
        result["gatesPassed"] = True
    finally:
        result["projectInputsPreserved"] = project_snapshot(args.project) == before
        result["exportInputsPreserved"] = snapshot(args.export, {"build", ".gradle", ".cxx"}) == export_before
        result["existingBundlesPreserved"] = {
            p.relative_to(args.export).as_posix(): sha(p) for p in args.export.rglob("*.aab")} == bundles_before
        result["passed"] = (result.get("gatesPassed", False) and result["projectInputsPreserved"]
                            and result["exportInputsPreserved"] and result["existingBundlesPreserved"])
        (args.report_directory / "result.json").write_text(json.dumps(result, indent=2), encoding="utf-8")
    require(result["passed"], "Input or existing bundle preservation failed.")
    return result


if __name__ == "__main__":
    try:
        print(json.dumps(run(parse_args()), ensure_ascii=True))
    except PreflightError as error:
        print(json.dumps({"passed": False, "error": str(error)}))
        sys.exit(1)
    except Exception as error:
        # Do not echo arbitrary plugin output, inherited values or file contents.
        print(json.dumps({"passed": False, "errorType": type(error).__name__}))
        sys.exit(1)
