import argparse
import json
from pathlib import Path
import tempfile
import unittest
from unittest.mock import patch

import verify_gradle_export as tool


def valid_events():
    return [
        {"kind": "jvm", "c1": True, "heapBytes": 4294967296, "processors": 1,
         "workers": 1, "parallel": False, "dryRun": True},
        {"kind": "toolchain", "gradle": "9.3.1"},
        {"kind": "release", "agp": "9.0.0", "package": "com.AeDeong.SuddenForceFPS",
         "code": 15, "name": "1.0.194", "unsigned": True, "debuggable": False},
        {"kind": "outputs", "identities": [{"code": 15, "name": "1.0.194"}]},
        {"kind": "graph", "unsignedFinalizers": 1, "blockedSigningTasks": 0},
        {"kind": "cmake", "matches": True}, {"kind": "finished", "failed": False}]


class PreflightTests(unittest.TestCase):
    def setUp(self):
        self.tmp = tempfile.TemporaryDirectory()
        self.addCleanup(self.tmp.cleanup)
        root = Path(self.tmp.name)
        self.args = argparse.Namespace(**{name: root / name for name in
            ("project", "export", "java_home", "gradle_home", "gradle_user_home", "sdk",
             "ndk", "android_user_home", "cmake_staging", "report_directory")},
             expected_code=15, expected_name="1.0.194")
        for folder in ("Assets", "Packages", "ProjectSettings", "BuildInfo"):
            (self.args.project / folder).mkdir(parents=True)
        (self.args.project / "BuildInfo/buildinfo.txt").write_text("194,10,18")
        for name in ("build.gradle", "settings.gradle", "launcher/build.gradle", "unityLibrary/build.gradle"):
            path = self.args.export / name
            path.parent.mkdir(parents=True, exist_ok=True)
            path.write_text("fixture")
        for directory in (self.args.sdk, self.args.ndk, self.args.java_home / "bin", self.args.gradle_home / "lib"):
            directory.mkdir(parents=True)
        (self.args.java_home / "bin" / ("java.exe" if tool.os.name == "nt" else "java")).write_bytes(b"fixture")
        (self.args.gradle_home / "lib/gradle-launcher-9.3.1.jar").write_bytes(b"fixture")
        self.bundle = self.args.export / "launcher/build/outputs/bundle/release/existing.aab"
        self.bundle.parent.mkdir(parents=True)
        self.bundle.write_bytes(b"existing release")

    def fake_gradle(self, cmd, **kwargs):
        self.assertIn("--dry-run", cmd)
        self.assertIn("--offline", cmd)
        self.assertNotIn("PASSWORD", json.dumps(kwargs["env"]))
        report = self.args.report_directory / "events.jsonl"
        report.write_text("\n".join(json.dumps(e) for e in valid_events()), encoding="utf-8")
        return argparse.Namespace(returncode=0)

    def test_existing_bundle_and_newer_ledger_are_preserved(self):
        with patch.object(tool.subprocess, "run", side_effect=self.fake_gradle), patch.dict(tool.os.environ, {}, clear=True):
            result = tool.run(self.args)
        self.assertTrue(result["passed"])
        self.assertEqual(self.bundle.read_bytes(), b"existing release")
        self.assertEqual((self.args.project / "BuildInfo/buildinfo.txt").read_text(), "194,10,18")
        self.assertEqual(self.args.expected_code, 15)

    def test_actual_jvm_and_unsigned_failures_rejected(self):
        cases = [(0, "c1", False), (0, "workers", 2), (0, "heapBytes", 1024),
                 (0, "dryRun", False), (1, "gradle", "9.8.1"), (2, "agp", "9.1.0"),
                 (2, "unsigned", False), (2, "code", 18), (4, "blockedSigningTasks", 1),
                 (5, "matches", False), (6, "failed", True)]
        for index, key, value in cases:
            with self.subTest(key=key, value=value):
                events = valid_events()
                events[index][key] = value
                with self.assertRaises(tool.PreflightError):
                    tool.validate_events(events, self.args)

    def test_missing_duplicate_or_executed_events_rejected(self):
        for events in ([], valid_events()[:-1], valid_events() + [valid_events()[0]],
                       valid_events() + [{"kind": "taskStarted"}]):
            with self.subTest(events=events), self.assertRaises(tool.PreflightError):
                tool.validate_events(events, self.args)

    def test_output_identity_cannot_override_original_export(self):
        events = valid_events()
        events[3]["identities"][0]["code"] = 18
        with self.assertRaises(tool.PreflightError):
            tool.validate_events(events, self.args)

    def test_environment_conflicts_rejected_and_credentials_removed(self):
        for parent in ({"java_tool_options": "private"}, {"ORG_GRADLE_PROJECT_storePassword": "private"}):
            with self.assertRaises(tool.PreflightError):
                tool.child_environment(parent, self.args, Path("report"))
        env = tool.child_environment({"MY_PASSWORD": "private", "GH_TOKEN": "private", "SFF_STOREPASS": "private",
                                      "SFF_PREFLIGHT_CODE": "18"}, self.args, Path("report"))
        self.assertNotIn("private", env.values())
        self.assertEqual(env["SFF_PREFLIGHT_CODE"], "15")

    def test_extra_init_script_rejected_before_process_launch(self):
        path = self.args.gradle_user_home / "init.d/foreign.gradle.kts"
        path.parent.mkdir(parents=True)
        path.write_text("fixture")
        with patch.object(tool.subprocess, "run") as process, self.assertRaises(tool.PreflightError):
            tool.run(self.args)
        process.assert_not_called()

    def test_failed_process_still_records_preservation(self):
        with patch.object(tool.subprocess, "run", return_value=argparse.Namespace(returncode=1)), \
             patch.dict(tool.os.environ, {}, clear=True), self.assertRaises(tool.PreflightError):
            tool.run(self.args)
        result = json.loads((self.args.report_directory / "result.json").read_text())
        self.assertFalse(result["passed"])
        self.assertTrue(result["projectInputsPreserved"])
        self.assertTrue(result["existingBundlesPreserved"])

    def test_source_or_bundle_change_fails_even_on_zero_exit(self):
        def mutate(cmd, **kwargs):
            result = self.fake_gradle(cmd, **kwargs)
            self.bundle.write_bytes(b"unexpected replacement")
            return result
        with patch.object(tool.subprocess, "run", side_effect=mutate), \
             patch.dict(tool.os.environ, {}, clear=True), self.assertRaises(tool.PreflightError):
            tool.run(self.args)
        self.assertFalse(json.loads((self.args.report_directory / "result.json").read_text())["existingBundlesPreserved"])

    def test_evidence_directory_cannot_be_reused(self):
        self.args.report_directory.mkdir()
        with patch.object(tool.subprocess, "run") as process, self.assertRaises(tool.PreflightError):
            tool.run(self.args)
        process.assert_not_called()

    def test_project_input_change_fails_on_zero_exit(self):
        def mutate(cmd, **kwargs):
            result = self.fake_gradle(cmd, **kwargs)
            (self.args.project / "BuildInfo/buildinfo.txt").write_text("194,11,19")
            return result
        with patch.object(tool.subprocess, "run", side_effect=mutate), \
             patch.dict(tool.os.environ, {}, clear=True), self.assertRaises(tool.PreflightError):
            tool.run(self.args)
        self.assertFalse(json.loads((self.args.report_directory / "result.json").read_text())["projectInputsPreserved"])

    def test_report_inside_project_is_rejected_before_launch(self):
        self.args.report_directory = self.args.project / "report"
        with patch.object(tool.subprocess, "run") as process, self.assertRaises(tool.PreflightError):
            tool.run(self.args)
        process.assert_not_called()

    def test_signing_key_is_never_hashed(self):
        key = self.args.project / "Assets/private.keystore"
        key.write_bytes(b"fixture only")
        original_sha = tool.sha
        def guarded_sha(path):
            self.assertNotEqual(path, key)
            return original_sha(path)
        with patch.object(tool, "sha", side_effect=guarded_sha):
            result = tool.project_snapshot(self.args.project)
        self.assertNotIn("private.keystore", result["Assets"])


if __name__ == "__main__":
    unittest.main()
