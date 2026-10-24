#!/usr/bin/env python3
"""Prepare only the five pinned Kenney Impact Sounds 1.0 concrete steps.

No extraction, resampling, up-gain, dither, installation, or Unity access.
An unsuccessful write leaves the new output directory intact for inspection.
"""
from __future__ import annotations

import argparse
import hashlib
import io
import json
import math
from pathlib import Path, PurePosixPath
import stat
import struct
import sys
import wave
import zipfile
import zlib

ARCHIVE_SHA256 = "029D734AF1582474EDF3A694D1B0CEBC97C1C152F2F39FA34D4C2BAFC5DE77F8"
LICENSE_SHA256 = "B49AA9C56B04528B95913DE13E506A0F7C5E807B9925DB9BFEF86AF1F91120DB"
SOURCES = (
    ("Audio/footstep_concrete_000.ogg", "D7267E183067757C92C169DE2A379ABEA592CFCA6B39BB2E8FEEA15221AD79FE"),
    ("Audio/footstep_concrete_001.ogg", "507A75EB19B897087855A3BE3CF11A12D7FA37D143AD535EEEBE0B8031C7325D"),
    ("Audio/footstep_concrete_002.ogg", "B21FA546D74941196B25DA8A20B7010ABD8C05C4FCF93545D1F344E66A664E13"),
    ("Audio/footstep_concrete_003.ogg", "8B513C2F0316324060B9174B8F28DF6C2147418C506E6A53B77F18E149A5547B"),
    ("Audio/footstep_concrete_004.ogg", "24DD8DB2413E5B81AD181E1205C12A1C7801DA5F85715D471B92A4B2F632E45E"),
)
TARGET_RMS = 0.04862877832654305
PEAK_CAP = 0.9
RATE = 44100
DC_THRESHOLD = 1.0 / 32768.0
FADE_IN_SECONDS = 0.0005
FADE_OUT_SECONDS = 0.002


class Guard(Exception):
    pass


def need(condition: bool, code: str) -> None:
    if not condition:
        raise Guard(code)


def digest(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest().upper()


def public_path(path: Path, *, exists: bool) -> Path:
    # Deny sensitive spellings before filesystem metadata or content access.
    need(path.is_absolute() and not str(path).startswith("\\\\"), "ABSOLUTE_LOCAL_PATH_REQUIRED")
    for part in path.parts:
        low = part.casefold()
        need(low not in {"..", ".", ".git", "private", "keys", "credentials", "key.txt", ".env"}
             and not low.startswith(".env.") and ".private." not in low
             and not low.endswith((".keystore", ".jks", ".pfx", ".p12")), "NONPUBLIC_PATH")
    for item in reversed([path, *path.parents]):
        if item.exists():
            info = item.lstat()
            need(not stat.S_ISLNK(info.st_mode)
                 and not (getattr(info, "st_file_attributes", 0) & 0x400), "REPARSE_PATH")
    need(path.exists() == exists, "INPUT_MISSING_OR_OUTPUT_EXISTS")
    need(path.resolve() == path, "CANONICAL_PATH_REQUIRED")
    return path


def inspect_archive(path: Path) -> tuple[bytes, dict[str, bytes]]:
    # Whole archive is the explicitly permitted fixed public input.
    archive = path.read_bytes()
    need(digest(archive) == ARCHIVE_SHA256, "ARCHIVE_PIN_CHANGED")
    selected: dict[str, bytes] = {}
    with zipfile.ZipFile(io.BytesIO(archive)) as z:
        infos = z.infolist()
        names: set[str] = set()
        for info in infos:
            name = info.filename
            p = PurePosixPath(name)
            need(name and not p.is_absolute() and "\\" not in name and ":" not in name
                 and not any(c in name for c in ("\0", "\r", "\n"))
                 and all(x not in {".", ".."} for x in p.parts), "UNSAFE_ZIP_NAME")
            need(name.casefold() not in names, "DUPLICATE_ZIP_MEMBER")
            names.add(name.casefold())
            need(not (info.flag_bits & 1), "ENCRYPTED_ZIP_MEMBER")
            kind = stat.S_IFMT(info.external_attr >> 16)
            need(kind in (0, stat.S_IFREG, stat.S_IFDIR), "NONREGULAR_ZIP_MEMBER")
        wanted = [name for name, _ in SOURCES] + ["License.txt"]
        for name in wanted:
            info = z.getinfo(name)
            need(not info.is_dir() and 0 < info.file_size <= 2_000_000, "SELECTED_MEMBER_BOUNDS")
            data = z.read(info)  # ZipFile also verifies CRC; no other member body is read.
            need((zlib.crc32(data) & 0xFFFFFFFF) == info.CRC, "SELECTED_MEMBER_CRC")
            selected[name] = data
    need(digest(selected["License.txt"]) == LICENSE_SHA256, "LICENSE_PIN_CHANGED")
    text = selected["License.txt"].decode("utf-8")
    need("Impact Sounds (1.0)" in text and "Creative Commons Zero, CC0" in text
         and "creativecommons.org/publicdomain/zero/1.0/" in text, "EXPECTED_CC0_NOTICE_REQUIRED")
    for name, pin in SOURCES:
        need(digest(selected[name]) == pin, "OGG_PIN_CHANGED")
    return archive, selected


def build(archive_path: Path, output: Path, decoder_path: Path) -> dict:
    public_path(archive_path, exists=True)
    public_path(decoder_path, exists=True)
    public_path(output, exists=False)
    need(archive_path.suffix.casefold() == ".zip" and archive_path.is_file()
         and decoder_path.is_dir() and output.parent.is_dir(), "INPUT_OUTPUT_SHAPE")
    archive, selected = inspect_archive(archive_path)
    sys.path.insert(0, str(decoder_path))
    import numpy as np
    import soundfile as sf

    need(sf.__version__ == "0.13.1" and Path(sf.__file__).resolve().parent == decoder_path,
         "PINNED_DECODER_REQUIRED")
    fade_in = int(round(RATE * FADE_IN_SECONDS))
    fade_out = int(round(RATE * FADE_OUT_SECONDS))
    payloads: dict[str, bytes] = {}
    records: list[dict] = []

    def metrics(x):
        peak = float(np.max(np.abs(x)))
        rms = float(np.sqrt(np.mean(x * x, dtype=np.float64)))
        need(math.isfinite(peak) and math.isfinite(rms) and peak > 0 and rms > 0, "FINITE_AUDIO_REQUIRED")
        threshold = peak * 10.0 ** (-48.0 / 20.0)
        onset = int(np.flatnonzero(np.abs(x) >= threshold)[0])
        return {"peak": peak, "rms": rms, "relative48dBOnsetFrame": onset,
                "relative48dBOnsetSeconds": onset / RATE, "mean": float(np.mean(x))}

    for index, (source, _) in enumerate(SOURCES, 1):
        stereo, rate = sf.read(io.BytesIO(selected[source]), dtype="float64", always_2d=True)
        need(rate == RATE and stereo.ndim == 2 and stereo.shape[1] == 2
             and stereo.shape[0] > fade_in + fade_out and np.all(np.isfinite(stereo)), "EXPECTED_STEREO_44100")
        mono = np.mean(stereo, axis=1, dtype=np.float64)
        original = metrics(mono)
        dc = float(np.mean(mono))
        dc_applied = abs(dc) > DC_THRESHOLD
        if dc_applied:
            mono = mono - dc
        mono[:fade_in] *= np.linspace(0.0, 1.0, fade_in, endpoint=True, dtype=np.float64)
        mono[-fade_out:] *= np.linspace(1.0, 0.0, fade_out, endpoint=True, dtype=np.float64)
        faded = metrics(mono)
        rms_gain = TARGET_RMS / faded["rms"]
        peak_gain = PEAK_CAP / faded["peak"]
        gain = min(1.0, rms_gain, peak_gain)
        need(0 < gain <= 1 and math.isfinite(gain), "DOWNWARD_GAIN_ONLY")
        scaled = mono * gain
        need(float(np.max(np.abs(scaled))) <= PEAK_CAP + 1e-12, "PEAK_CAP")
        integers = np.rint(scaled * 32768.0)
        need(np.all(integers >= -32768) and np.all(integers <= 32767), "NO_CLIPPING")
        pcm = integers.astype("<i2")
        quantized = pcm.astype(np.float64) / 32768.0
        measured = metrics(quantized)
        need(measured["peak"] <= PEAK_CAP and int(pcm[0]) == int(pcm[-1]) == 0, "PCM_BOUNDS_AND_FADE_ENDPOINTS")
        clipping = int(np.count_nonzero((pcm == -32768) | (pcm == 32767)))
        need(clipping == 0, "CLIPPED_PCM_SAMPLE")
        buffer = io.BytesIO()
        with wave.open(buffer, "wb") as wav:
            wav.setnchannels(1)
            wav.setsampwidth(2)
            wav.setframerate(RATE)
            wav.writeframes(pcm.tobytes(order="C"))
        name = f"ConcreteStep{index:02d}.wav"
        payloads[name] = buffer.getvalue()
        records.append({"sourceArchiveEntry": source, "sourceOggBytes": len(selected[source]),
                        "sourceOggSha256": digest(selected[source]), "sourceChannels": 2,
                        "sampleRate": RATE, "frames": len(pcm), "durationSeconds": len(pcm) / RATE,
                        "originalMono": original, "dcMeasured": dc, "dcRemoved": dc if dc_applied else 0.0,
                        "dcRemovalApplied": dc_applied, "afterDcAndFade": faded,
                        "rmsMatchingGain": rms_gain, "peakCapGain": peak_gain,
                        "appliedGain": gain, "gainWasUpward": False,
                        "peakLimited": peak_gain < min(1.0, rms_gain),
                        "output": {"name": name, "bytes": len(payloads[name]),
                                   "sha256": digest(payloads[name]), "channels": 1,
                                   "format": "WAV_PCM_S16LE", "metrics": measured,
                                   "absoluteRmsErrorToReference": abs(measured["rms"] - TARGET_RMS),
                                   "clippedSamples": clipping}})

    original_license = selected["License.txt"]
    license_lf = original_license.replace(b"\r\n", b"\n").replace(b"\r", b"\n")
    payloads["License.txt"] = license_lf
    manifest = {"schema": "KENNEY_CONCRETE_FOOTSTEPS_PREPARATION_V1", "package": "Kenney Impact Sounds 1.0",
                "archive": {"bytes": len(archive), "sha256": ARCHIVE_SHA256},
                "sourceUrl": "https://kenney.nl/assets/impact-sounds",
                "license": {"sourceEntry": "License.txt", "originalBytes": len(original_license),
                            "originalSha256": digest(original_license), "normalization": "CRLF_or_CR_to_LF_only",
                            "outputBytes": len(license_lf), "outputSha256": digest(license_lf)},
                "toolSha256": digest(Path(__file__).read_bytes()),
                "decoder": {"soundfile": sf.__version__, "libsndfile": sf.__libsndfile_version__,
                            "numpy": np.__version__, "python": sys.version.split()[0]},
                "processing": {"stereoToMono": "float64_arithmetic_mean_L_R", "resampled": False,
                               "sourceRatePreserved": RATE, "dcThreshold": DC_THRESHOLD,
                               "fadeInSecondsRequested": FADE_IN_SECONDS, "fadeInSamples": fade_in,
                               "fadeOutSecondsRequested": FADE_OUT_SECONDS, "fadeOutSamples": fade_out,
                               "fadeEndpoints": "linear_inclusive_zero_and_one;last_output_sample_zero",
                               "targetOldAuthoredMedianRms": TARGET_RMS, "peakCap": PEAK_CAP,
                               "gain": "min(1,targetRms/fadedRms,peakCap/fadedPeak)",
                               "quantization": "nearest_ties_to_even_PCM16_no_dither",
                               "trimmed": False, "crossOsByteIdentityClaimed": False,
                               "lossySourceInformationRestored": False},
                "clips": records, "audibleImprovementOrDeviceLatencyPassed": False,
                "unityImporterMixerPlaybackOrAttenuationChanged": False,
                "sourceArchiveRehashedUnchangedBeforeWrite": digest(archive_path.read_bytes()) == ARCHIVE_SHA256,
                "originalOggsWrittenOrExtracted": False}
    need(manifest["sourceArchiveRehashedUnchangedBeforeWrite"], "ARCHIVE_CHANGED_DURING_PROCESSING")
    payloads["manifest.public.json"] = (json.dumps(manifest, ensure_ascii=False, indent=2, allow_nan=False) + "\n").encode("utf-8")
    need(set(payloads) == {f"ConcreteStep{i:02d}.wav" for i in range(1, 6)} | {"License.txt", "manifest.public.json"},
         "OUTPUT_ALLOWLIST")
    output.mkdir(exist_ok=False)
    for name, data in payloads.items():
        with (output / name).open("xb") as stream:
            stream.write(data)
    need(set(p.name for p in output.iterdir()) == set(payloads), "OUTPUT_MEMBERSHIP")
    for name, data in payloads.items():
        need((output / name).read_bytes() == data, "OUTPUT_WRITE_READBACK")
    need(digest(archive_path.read_bytes()) == ARCHIVE_SHA256, "ARCHIVE_CHANGED_AFTER_WRITE")
    return {"schema": "KENNEY_FOOTSTEP_TOOL_COMPLETION_V1", "writtenWavCount": 5,
            "writtenFileCount": 7, "archiveUnchanged": True,
            "manifestSha256": digest(payloads["manifest.public.json"]), "audibleQualityPassed": False}


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--archive", required=True, type=Path)
    parser.add_argument("--output", required=True, type=Path)
    parser.add_argument("--decoder-path", required=True, type=Path)
    args = parser.parse_args()
    try:
        result = build(args.archive, args.output, args.decoder_path)
    except Guard as error:
        print(json.dumps({"success": False, "guard": str(error), "partialOutputPreserved": True}))
        return 1
    except Exception as error:
        print(json.dumps({"success": False, "errorType": type(error).__name__, "partialOutputPreserved": True}))
        return 1
    print(json.dumps({"success": True, **result}, allow_nan=False))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
