import contextlib
import io
import json
from pathlib import Path
import struct
import tempfile
import unittest
import warnings
import zipfile

import verify_native_alignment as tool


def elf(alignment=16384, relro_end=16384, include_relro=True):
    result = bytearray(512)
    result[:16] = b"\x7fELF\x02\x01\x01" + bytes(9)
    struct.pack_into("<HHIQQQIHHHHHH", result, 16, 3, 183, 1, 0, 64, 0, 0,
                     64, 56, 2 if include_relro else 1, 0, 0, 0)
    struct.pack_into("<IIQQQQQQ", result, 64, tool.PT_LOAD, 6, 0, 0, 0, 512, 32768, alignment)
    if include_relro:
        struct.pack_into("<IIQQQQQQ", result, 120, tool.PT_GNU_RELRO, 4, 256,
                         8192, 8192, 16, relro_end - 8192, 1)
    return result


class NativeAlignmentTests(unittest.TestCase):
    def setUp(self):
        self.tmp = tempfile.TemporaryDirectory()
        self.addCleanup(self.tmp.cleanup)
        self.root = Path(self.tmp.name)

    def bundle(self, entries, suffix=".aab"):
        path = self.root / ("test" + suffix)
        with zipfile.ZipFile(path, "w") as archive:
            for name, data in entries:
                archive.writestr(name, data)
        return path

    def test_aligned_load_and_relro(self):
        self.assertTrue(tool.inspect_elf(elf())["passed"])

    def test_four_k_load_fails(self):
        self.assertEqual(tool.inspect_elf(elf(alignment=4096))["failures"], ["LOAD_ALIGNMENT"])

    def test_relro_end_not_start_is_checked(self):
        result = tool.inspect_elf(elf(relro_end=12288))
        self.assertEqual(result["failures"], ["RELRO_END_ALIGNMENT"])
        self.assertEqual(result["relroSegments"][0]["remainder"], 12288)

    def test_absent_relro_is_reported_without_security_claim(self):
        result = tool.inspect_elf(elf(include_relro=False))
        self.assertTrue(result["passed"])
        self.assertTrue(result["relroAbsent"])

    def test_non_arm64_and_wrong_byte_order_rejected(self):
        for position, value in ((18, 62), (5, 2)):
            data = elf()
            data[position] = value
            with self.assertRaises(tool.InvalidInput):
                tool.inspect_elf(data)

    def test_truncated_table_and_segment_rejected(self):
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_elf(elf()[:100])
        data = elf()
        struct.pack_into("<Q", data, 64 + 32, 513)
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_elf(data)

    def test_bad_load_alignment_and_address_congruence_rejected(self):
        for alignment in (0, 24576):
            with self.assertRaises(tool.InvalidInput):
                tool.inspect_elf(elf(alignment=alignment))
        data = elf()
        struct.pack_into("<Q", data, 64 + 16, 1)
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_elf(data)

    def test_memory_overflow_and_filesize_rejected(self):
        data = elf()
        struct.pack_into("<Q", data, 64 + 40, 1)
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_elf(data)
        data = elf()
        struct.pack_into("<Q", data, 120 + 16, (1 << 64) - 1)
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_elf(data)

    def test_aab_mixed_good_bad_and_non_target_entries(self):
        path = self.bundle([("base/lib/arm64-v8a/good.so", elf()),
                            ("base/lib/arm64-v8a/bad.so", elf(relro_end=12288)),
                            ("base/lib/x86_64/ignored.so", b"not ARM64"),
                            ("base/assets/embedded.so", b"not native library")])
        before = path.read_bytes()
        result = tool.inspect_bundle(path)
        self.assertFalse(result["passed"])
        self.assertEqual(len(result["libraries"]), 2)
        self.assertEqual(path.read_bytes(), before)

    def test_apk_entry_is_selected(self):
        path = self.bundle([("lib/arm64-v8a/native.so", elf())], ".apk")
        self.assertTrue(tool.inspect_bundle(path)["passed"])

    def test_aab_feature_module_is_also_checked(self):
        path = self.bundle([("base/lib/arm64-v8a/good.so", elf()),
                            ("feature/lib/arm64-v8a/bad.so", elf(alignment=4096))])
        result = tool.inspect_bundle(path)
        self.assertEqual(len(result["libraries"]), 2)
        self.assertFalse(result["passed"])

    def test_no_libraries_and_duplicate_zip_fail(self):
        path = self.bundle([("other.txt", b"no ELF")])
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_bundle(path)
        path.unlink()
        with warnings.catch_warnings():
            warnings.simplefilter("ignore", UserWarning)
            path = self.bundle([("base/lib/arm64-v8a/n.so", elf())] * 2)
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_bundle(path)

    def test_invalid_elf_is_a_failure_not_silent_skip(self):
        path = self.bundle([("base/lib/arm64-v8a/n.so", b"invalid ELF")])
        self.assertEqual(tool.inspect_bundle(path)["libraries"][0]["failures"], ["INVALID_ELF"])

    def test_unsafe_paths_and_size_limit_rejected(self):
        path = self.bundle([("../outside", b"x"), ("base/lib/arm64-v8a/n.so", elf())])
        with self.assertRaises(tool.InvalidInput):
            tool.inspect_bundle(path)
        path.unlink()
        path = self.bundle([("base/lib/arm64-v8a/n.so", elf())])
        old_limit = tool.MAX_ELF_BYTES
        try:
            tool.MAX_ELF_BYTES = 32
            with self.assertRaises(tool.InvalidInput):
                tool.inspect_bundle(path)
        finally:
            tool.MAX_ELF_BYTES = old_limit

    def test_cli_status_and_report_no_overwrite(self):
        path = self.bundle([("base/lib/arm64-v8a/n.so", elf(relro_end=12288))])
        output = self.root / "report.json"
        with contextlib.redirect_stdout(io.StringIO()):
            self.assertEqual(tool.main([str(path), "--output", str(output)]), 1)
            original = output.read_bytes()
            self.assertEqual(tool.main([str(path), "--output", str(output)]), 2)
        self.assertEqual(output.read_bytes(), original)
        self.assertFalse(json.loads(original)["passed"])

    def test_cli_aligned_and_malformed_exit_codes(self):
        path = self.bundle([("base/lib/arm64-v8a/n.so", elf())])
        with contextlib.redirect_stdout(io.StringIO()):
            self.assertEqual(tool.main([str(path)]), 0)
            self.assertEqual(tool.main([str(path), "--output", str(path)]), 2)
            before = path.read_bytes()
            self.assertTrue(tool.inspect_bundle(path)["passed"])
            self.assertEqual(path.read_bytes(), before)
            path.write_bytes(b"not ZIP")
            output = self.root / "invalid.json"
            self.assertEqual(tool.main([str(path), "--output", str(output)]), 2)
        self.assertIn("error", json.loads(output.read_text()))

    def test_selected_native_crc_corruption_fails(self):
        path = self.bundle([("base/lib/arm64-v8a/n.so", elf())])
        data = bytearray(path.read_bytes())
        name_length, extra_length = struct.unpack_from("<HH", data, 26)
        data[30 + name_length + extra_length + 300] ^= 1
        path.write_bytes(data)
        with contextlib.redirect_stdout(io.StringIO()):
            self.assertEqual(tool.main([str(path)]), 2)

    def test_invalid_utf8_zip_filename_returns_json_input_error(self):
        path = self.bundle([("base/lib/arm64-v8a/n.so", elf())])
        data = bytearray(path.read_bytes())
        central = data.index(b"PK\x01\x02")
        struct.pack_into("<H", data, 6, 0x800)
        struct.pack_into("<H", data, central + 8, 0x800)
        data[30] = data[central + 46] = 0xff
        path.write_bytes(data)
        output = io.StringIO()
        with contextlib.redirect_stdout(output):
            self.assertEqual(tool.main([str(path)]), 2)
        self.assertFalse(json.loads(output.getvalue())["passed"])


if __name__ == "__main__":
    unittest.main()
