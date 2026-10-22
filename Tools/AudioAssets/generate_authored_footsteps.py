"""Original deterministic footstep-like synthesis. No external samples.
Standard library only. Candidate sounds require listening and tuning in-game.
"""
import argparse, hashlib, json, math, random, struct, wave
from pathlib import Path
parser = argparse.ArgumentParser(description='Generate original deterministic footstep clips into a new folder.')
parser.add_argument('--output', required=True, type=Path)
args = parser.parse_args()
OUT = args.output.resolve()
OUT.mkdir(parents=True, exist_ok=False)
rate = 22050
rows = []
for variant in range(4):
    seed = 2026102201 + variant
    rng = random.Random(seed)
    duration = 0.135 + variant * 0.006
    frames = int(rate * duration)
    samples = []
    low = 0.0
    for n in range(frames):
        t = n / rate
        noise = rng.uniform(-1, 1)
        low = 0.82 * low + 0.18 * noise
        attack = min(1, t / 0.0018)
        impact = (math.sin(2 * math.pi * (92 + variant * 7) * t) * 0.42 + low * 0.82) * math.exp(-t * 44)
        grit = (noise - low) * 0.11 * math.exp(-t * 26)
        heel = math.sin(2 * math.pi * (57 + variant * 3) * t) * 0.12 * math.exp(-t * 22)
        tail = max(0, min(1, (duration-t) / 0.022))
        samples.append((impact + grit + heel) * attack * tail)
    peak = max(abs(s) for s in samples)
    # Fixed peak -.~9dBFS; no hard clipping/normalization at playback.
    pcm = b''.join(struct.pack('<h', round(s / peak * 0.35 * 32767)) for s in samples)
    path = OUT / f'AuthoredStep{variant+1:02d}.wav'
    with wave.open(str(path), 'wb') as f:
        f.setnchannels(1); f.setsampwidth(2); f.setframerate(rate); f.writeframes(pcm)
    rows.append(dict(path=path.name, seed=seed, sampleRate=rate, channels=1, bits=16,
        durationSeconds=frames/rate, bytes=path.stat().st_size, sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
manifest = dict(origin='Original arithmetic noise/sine synthesis, no external samples, no third-party audio',
    generator='generate_authored_footsteps.py', status='generated candidate; listening/runtime checks not performed', clips=rows)
(OUT/'manifest.json').write_text(json.dumps(manifest, ensure_ascii=False, indent=2)+'\n', encoding='utf-8')
print(json.dumps(manifest, ensure_ascii=False))
