"""Read-only ARM64 ELF LOAD/RELRO checks for an Android AAB or APK.

This does not check APK ZIP alignment, BundleConfig, signatures or runtime behavior.
See https://developer.android.com/guide/practices/page-sizes#relro.
"""
import argparse
import hashlib
import json
from pathlib import Path
import struct
import sys
import zipfile
import zlib

PAGE_SIZE = 16384
MAX_ELF_BYTES = 256 * 1024 * 1024
PT_LOAD = 1
PT_GNU_RELRO = 0x6474E552


class InvalidInput(ValueError):
    pass


def require(ok, message):
    if not ok:
        raise InvalidInput(message)


def inspect_elf(data):
    require(len(data) >= 64, "ELF header truncated")
    require(data[:7] == b"\x7fELF\x02\x01\x01", "Expected ELF64 little-endian version1")
    header = struct.unpack_from("<HHIQQQIHHHHHH", data, 16)
    kind, machine, version, _, phoff, _, _, ehsize, phsize, phnum, _, _, _ = header
    require(kind == 3 and machine == 183 and version == 1, "Expected ARM64 ET_DYN")
    require(ehsize == 64 and phsize == 56 and 0 < phnum < 65535,
            "Unsupported ELF/program header layout")
    require(phoff >= 64 and phoff + phnum * phsize <= len(data),
            "Program header table outside file")
    loads, relros, failures = [], [], []
    for index in range(phnum):
        kind, flags, offset, address, _, file_size, memory_size, alignment = struct.unpack_from(
            "<IIQQQQQQ", data, phoff + index * phsize)
        if kind not in (PT_LOAD, PT_GNU_RELRO):
            continue
        require(offset <= len(data) and file_size <= len(data) - offset,
                "Segment file range outside file")
        require(address + memory_size < 1 << 64, "Segment address overflow")
        if kind == PT_LOAD:
            require(file_size <= memory_size, "LOAD file size exceeds memory size")
            require(alignment != 0 and alignment & (alignment - 1) == 0,
                    "LOAD alignment must be a nonzero power of two")
            require((address - offset) % alignment == 0, "LOAD address/offset incongruent")
            aligned = alignment >= PAGE_SIZE
            loads.append({"index": index, "alignment": alignment, "passed": aligned})
            if not aligned:
                failures.append("LOAD_ALIGNMENT")
        else:
            end = address + memory_size
            aligned = end % PAGE_SIZE == 0
            relros.append({"index": index, "virtualAddress": address,
                           "memorySize": memory_size, "end": end,
                           "remainder": end % PAGE_SIZE, "passed": aligned})
            if not aligned:
                failures.append("RELRO_END_ALIGNMENT")
    require(loads, "ELF has no LOAD segments")
    return {"passed": not failures, "loadSegments": loads, "relroSegments": relros,
            "failures": sorted(set(failures)),
            "relroAbsent": not relros}


def native_entry(name, suffix):
    parts = name.split("/")
    if suffix == ".aab":
        return (len(parts) == 4 and bool(parts[0]) and parts[1:3] == ["lib", "arm64-v8a"]
                and parts[-1].endswith(".so"))
    return len(parts) == 3 and parts[:2] == ["lib", "arm64-v8a"] and parts[-1].endswith(".so")


def inspect_bundle(path):
    path = Path(path)
    require(path.suffix.lower() in (".aab", ".apk"), "Expected .aab or .apk")
    libraries = []
    with path.open("rb") as source:
        digest = hashlib.file_digest(source, "sha256").hexdigest().upper()
        source.seek(0)
        with zipfile.ZipFile(source) as archive:
            names = archive.namelist()
            require(len(names) == len(set(names)), "Duplicate ZIP entry")
            require(all(not n.startswith(("/", "\\")) and ".." not in n.replace("\\", "/").split("/")
                        for n in names), "Unsafe ZIP entry path")
            selected = [entry for entry in archive.infolist()
                        if native_entry(entry.filename, path.suffix.lower())]
            require(0 < len(selected) <= 128, "Expected 1..128 ARM64 shared libraries")
            for entry in selected:
                require(0 < entry.file_size <= MAX_ELF_BYTES, "Native entry size outside limit")
                require(not entry.flag_bits & 1, "Encrypted native entry unsupported")
                data = archive.read(entry)  # Checks the CRC of each selected library.
                result = {"entry": entry.filename, "bytes": len(data),
                          "sha256": hashlib.sha256(data).hexdigest().upper()}
                try:
                    result.update(inspect_elf(data))
                except InvalidInput as error:
                    result.update(passed=False, failures=["INVALID_ELF"], error=str(error))
                libraries.append(result)
        source.seek(0)
        require(hashlib.file_digest(source, "sha256").hexdigest().upper() == digest,
                "Artifact changed during inspection")
    return {"schemaVersion": 1, "artifact": str(path.resolve()), "sha256": digest,
            "pageSize": PAGE_SIZE, "abi": "arm64-v8a",
            "passed": all(item["passed"] for item in libraries), "libraries": libraries,
            "limits": ["ARM64 ELF program headers only; no library changes or extraction",
                       "Not APK ZIP alignment, BundleConfig, signing or runtime/device verification",
                       "A missing RELRO segment is not an alignment failure; no security hardening verdict"]}


def main(argv=None):
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("artifact", type=Path)
    parser.add_argument("--output", type=Path, help="New JSON report; refuses to overwrite")
    args = parser.parse_args(argv)
    try:
        require(args.output is None or not args.output.exists(), "Output already exists")
        require(args.output is None or args.output.resolve() != args.artifact.resolve(),
                "Report must not replace the artifact")
        report = inspect_bundle(args.artifact)
        code = 0 if report["passed"] else 1
    except (InvalidInput, OSError, UnicodeError, zipfile.BadZipFile, RuntimeError, EOFError,
            NotImplementedError, zlib.error) as error:
        report, code = {"schemaVersion": 1, "passed": False, "error": str(error)}, 2
    if args.output is not None:
        try:
            with args.output.open("x", encoding="utf-8") as output:
                json.dump(report, output, ensure_ascii=False, indent=2)
                output.write("\n")
        except OSError as error:
            print(json.dumps({"passed": False, "error": str(error)}))
            return 2
    print(json.dumps(report, ensure_ascii=False, indent=2))
    return code


if __name__ == "__main__":
    sys.exit(main())
