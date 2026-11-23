"""Verify exact UTF-8 literals in matching Unity IL2CPP v39 APK and AAB metadata."""
import argparse
import hashlib
import json
from pathlib import Path
import struct
import sys
import zipfile

METADATA_ENTRY = "assets/bin/Data/Managed/Metadata/global-metadata.dat"
SECTION = struct.Struct("<iii")
OFFSET = struct.Struct("<i")
# Magic, version, and 31 Il2CppSectionMetadata descriptors in the v39 header.
HEADER_SIZE = 8 + 31 * SECTION.size


def read_section(raw, position):
    offset, size, count = SECTION.unpack_from(raw, position)
    if offset < HEADER_SIZE or size < 0 or count < 0 or offset + size > len(raw):
        raise ValueError("Metadata section lies outside the file")
    return offset, size, count


def count_literals(raw, needles):
    if len(raw) < HEADER_SIZE or struct.unpack_from("<II", raw) != (0xFAB11BAF, 39):
        raise ValueError("Expected little-endian IL2CPP metadata version 39")
    table, size, count = read_section(raw, 8)
    data, length, _ = read_section(raw, 20)
    if table != HEADER_SIZE or count < 1 or size != count * OFFSET.size or data < table + size:
        raise ValueError("Invalid string literal offset table")

    # Unity 6000.3 v39 uses adjacent dataIndex values, including a final sentinel.
    view = memoryview(raw)
    indices = (item[0] for item in OFFSET.iter_unpack(view[table:table + size]))
    previous = next(indices)
    if previous != 0:
        raise ValueError("Invalid first string offset")
    hits = dict.fromkeys(needles, 0)
    for current in indices:
        if not previous <= current <= length:
            raise ValueError("Invalid string literal range")
        literal = view[data + previous:data + current]
        if literal in hits:
            hits[literal] += 1
        previous = current
    if previous != length:
        raise ValueError("Invalid final string offset")
    return count - 1, hits


def packed_metadata(path, entry):
    with zipfile.ZipFile(path) as archive:
        if archive.namelist().count(entry) != 1:
            raise ValueError(f"Expected exactly one {entry} in {path.name}")
        return archive.read(entry)


def verify(apk, aab, required, forbidden):
    raw = packed_metadata(apk, METADATA_ENTRY)
    if raw != packed_metadata(aab, "base/" + METADATA_ENTRY):
        raise ValueError("APK and AAB metadata differ")
    wanted = [s.encode("utf-8") for s in required]
    excluded = [s.encode("utf-8") for s in forbidden]
    total, hits = count_literals(raw, wanted + excluded)
    required_counts = [hits[s] for s in wanted]
    forbidden_counts = [hits[s] for s in excluded]
    return {
        "passed": all(required_counts) and not any(forbidden_counts),
        "metadataVersion": 39,
        "metadataSha256": hashlib.sha256(raw).hexdigest(),
        "literalCount": total,
        "requiredCounts": required_counts,
        "forbiddenCounts": forbidden_counts,
    }


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("apk", type=Path)
    parser.add_argument("aab", type=Path)
    parser.add_argument("--require", action="append", default=[], help="Exact literal that must be present")
    parser.add_argument("--forbid", action="append", default=[], help="Exact literal that must be absent")
    args = parser.parse_args()
    if not args.require and not args.forbid:
        parser.error("Supply at least one --require or --forbid literal")
    try:
        result = verify(args.apk, args.aab, args.require, args.forbid)
    except (OSError, ValueError, zipfile.BadZipFile) as exc:
        print(f"Metadata check failed: {exc}", file=sys.stderr)
        return 1
    print(json.dumps(result))
    return 0 if result["passed"] else 1


if __name__ == "__main__":
    sys.exit(main())
