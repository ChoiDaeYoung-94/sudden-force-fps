"""Regenerate the two original menu cues with Python's standard library."""
import math
import struct
import wave
from pathlib import Path


SAMPLE_RATE = 22050
OUTPUT = Path(__file__).resolve().parents[2] / "Assets" / "Audios" / "Authored"


def tone(time, duration, frequency, decay):
    if time <= 0 or time >= duration:
        return 0.0
    attack = min(time / 0.0025, 1.0)
    release = min((duration - time) / 0.007, 1.0)
    return attack * release * math.exp(-time / decay) * math.sin(math.tau * frequency * time)


def write_cue(name, duration, notes):
    samples = bytearray()
    for index in range(round(duration * SAMPLE_RATE)):
        time = index / SAMPLE_RATE
        value = sum(level * tone(time - start, length, frequency, decay)
                    for start, length, frequency, decay, level in notes)
        samples.extend(struct.pack("<h", round(value * 32767)))
    with wave.open(str(OUTPUT / name), "wb") as stream:
        stream.setparams((1, 2, SAMPLE_RATE, 0, "NONE", "not compressed"))
        stream.writeframes(samples)


if __name__ == "__main__":
    write_cue("MenuClick.wav", 0.052, [
        (0.0, 0.052, 690, 0.013, 0.20),
        (0.0, 0.030, 1310, 0.008, 0.045),
    ])
    write_cue("MenuConfirm.wav", 0.105, [
        (0.0, 0.052, 620, 0.019, 0.15),
        (0.035, 0.070, 930, 0.023, 0.18),
    ])
