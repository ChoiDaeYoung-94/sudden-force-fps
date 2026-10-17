# Rifle life animations

`Assets/Animations/Player/CC0Rifle` contains two Humanoid clips and a two-state controller for the existing Military Avatar (`0e55d5c42cc96404f872b45455603f8b`). It preserves the runtime keys `Base Layer.Idle_Aiming` and `Base Layer.Death_Front`. The death falls backwards; the state name is an existing code contract, not a direction claim. Idle now loops with a small original spine breathing curve. Death holds its last frame until the independent three-second respawn logic restores idle.

## Source and adaptations

- Author: Quaternius, [Ultimate Modular Men](https://quaternius.com/packs/ultimatemodularcharacters.html), [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/). The author's 372-byte declaration is preserved with trailing whitespace only normalized (369 bytes; wording unchanged) in `Assets/Animations/Player/CC0Rifle/License.txt` and appended to the in-app notices.
- Official archive `Animations.fbx`: SHA256 `259ED6F6E5F1BB2A15215A66A9ED02E8956083A281A8098103490D1A00292B7F`. Takes: `CharacterArmature|Idle_Gun_Pointing` and `CharacterArmature|Death`.
- The animation FBX is a Generic control rig, with feet outside the leg hierarchy. Direct Humanoid import failed. Its sampled world poses were transferred to the separate official `Humanoid-Swat.fbx` FK reference, then converted with `HumanPoseHandler` to the existing Military Avatar.
- Curated changes: two-arm analytic IK around the existing rifle/OffhandGrip, finger grip, grounded feet, initial 0.18-second aim blend, rifle held above the chest during the backwards fall, final leg settling, and CPU-baked skin/weapon clearance. Original 22 animation curves were not copied.
- The normalized pose samples in `RiflePoses.json` are the curated derivative input. Idle has nine breathing keys and 7mm clearance adjustment. Death has 129 samples at 120Hz, an 8mm sampled clearance target, linear curve tangents and continuous left forearm twist. A normalized period of four for muscle43 was tested on the target Avatar: adding four changed no bone position or rotation. This avoids the interpolated long turn at about 0.404 seconds.

## Regenerate the clips

With this project's idle Unity Editor connected to Unity CLI:

```powershell
unity command eval_file --file '<project>/Tools/AnimationBaking/GenerateRifleClips.cs' --project-path '<project>' --format json
```

The script updates only the two existing clip assets, preserving their GUIDs. It does not regenerate the curated IK samples, controller or prefabs. Re-curating samples requires the original CC0 source and fresh visual/pose validation. Never derive replacement curves from the retained original 22 clips.

`PlayerLifePresentation._applyLegacyDeathGrounding` defaults to true for older controllers. Both Soldier prefabs explicitly disable it with the new controller because their death clip already includes body height. The existing legacy correction must not be applied a second time. Keep this flag and controller pairing together when changing a prefab. Network root motion remains disabled; Avatar, hitboxes, network bindings and respawn timing are unchanged.

## Validation boundary (2026-10-17)

The candidate was evaluated on Blue/Red, at identity and at `(7,2,-4)`/yaw137, over 262 time samples for each of idle/death/idle: 12 cases, 3,144 evaluations including between-key times. Lowest active skin/weapon point stayed at least 5.003mm above the owner plane, maximum death palm-to-grip distance was 3.773mm and barrel deviation 0.757 degrees. Idle grip was at most 0.114mm; network root displacement/rotation were zero. These are isolated Editor Animator checks, not a live Fusion match or device test. Original clips/controller/source assets are retained. Build packed-dependency exclusion and Android execution require a later build; the currently provided Play code21 contains the old animations.

Detailed bake trials, failed imports, wrap regression evidence, source archive and visual captures are preserved outside Git at `D:/codex/validation/sudden-force-fps/20261017-animation-retarget-01`. This path is evidence for the current PC, not an input required by the regeneration script.

A separate Play harness also confirmed advancing frames, both idle loops, last-death-pose hold and an explicit return to idle. All network Behaviours were disabled and states were switched directly, so this is not a Fusion snapshot/respawn test. The temporary scene was removed by leaving Play mode; the original clean Untitled scene and baseline EDM settings were restored. Warning counts were not zero: existing SDK deprecations/URP compatibility notices and test-only Animator body-position queries were recorded.
