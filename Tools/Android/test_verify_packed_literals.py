import struct
import tempfile
import unittest
import zipfile
from pathlib import Path

from verify_packed_literals import METADATA_ENTRY, count_literals, verify


def metadata(strings):
    data = b"".join(strings)
    offsets = [0]
    for value in strings:
        offsets.append(offsets[-1] + len(value))
    table = struct.pack(f"<{len(offsets)}i", *offsets)
    header = bytearray(380)
    struct.pack_into("<II6i", header, 0, 0xFAB11BAF, 39, len(header), len(table), len(offsets),
                     len(header) + len(table), len(data), len(strings))
    return bytes(header) + table + data


class PackedLiteralTests(unittest.TestCase):
    def test_empty_repeated_and_unicode_literals(self):
        name = "재시도".encode()
        total, hits = count_literals(metadata([b"", name, name, b"retry later"]), [b"", name, b"retry"])
        self.assertEqual((total, hits), (4, {b"": 1, name: 2, b"retry": 0}))

    def test_rejects_invalid_section_and_offsets(self):
        raw = metadata([b"first", b"second"])
        for position, bad in [(4, 38), (8, 32), (8, 384), (12, 24), (16, 0), (20, 381),
                              (24, len(raw)), (380, 1), (384, -1), (384, 12), (388, 4), (388, 10)]:
            with self.subTest(position=position, bad=bad):
                changed = bytearray(raw)
                struct.pack_into("<i", changed, position, bad)
                with self.assertRaises(ValueError):
                    count_literals(bytes(changed), [])

    def test_package_pair_and_required_forbidden_results(self):
        with tempfile.TemporaryDirectory() as folder:
            apk, aab = (Path(folder) / n for n in ("test.apk", "test.aab"))
            def write(path, name, raw):
                with zipfile.ZipFile(path, "w") as archive:
                    archive.writestr(name, raw)
            raw = metadata([b"current"])
            write(apk, METADATA_ENTRY, raw)
            write(aab, "base/" + METADATA_ENTRY, raw)
            self.assertTrue(verify(apk, aab, ["current"], ["old"])["passed"])
            self.assertFalse(verify(apk, aab, ["old"], ["current"])["passed"])
            write(aab, "base/" + METADATA_ENTRY, metadata([b"old"]))
            with self.assertRaisesRegex(ValueError, "differ"):
                verify(apk, aab, ["current"], [])


if __name__ == "__main__":
    unittest.main()
