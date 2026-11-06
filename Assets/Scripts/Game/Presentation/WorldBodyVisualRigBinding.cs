using System;
using System.Collections.Generic;
using UnityEngine;

// Candidate only. Generic transform animation has no Avatar and every cloned
// bone has a new name. Never clone a GameObject or any simulation component.
public sealed class WorldBodyVisualRigBinding : IDisposable
{
    public const string Prefix = "SFF_LOCO_";
    public const string RootName = "SFF_LOCO_VisualBody";
    public readonly GameObject Root;
    public readonly Animator Animator;
    public readonly Transform[] SourceBones, VisualBones;
    private readonly SkinnedMeshRenderer[] _skins;
    private readonly Transform[][] _originalBones;
    private readonly Transform[] _originalRoots;
    private bool _disposed;

    private WorldBodyVisualRigBinding(GameObject root, Animator animator,
        Transform[] source, Transform[] visual, SkinnedMeshRenderer[] skins,
        Transform[][] bones, Transform[] roots)
    {
        Root = root; Animator = animator; SourceBones = source; VisualBones = visual;
        _skins = skins; _originalBones = bones; _originalRoots = roots;
    }

    // The caller must already have completed PlayerCrouchGeometry.Initialize.
    // Preview probes may invoke this directly on an isolated prefab instance.
    public static WorldBodyVisualRigBinding Create(GamePlayerNetworkData owner)
    {
        if (owner == null) throw new ArgumentNullException(nameof(owner));
        var bodyAnimator = owner.GetComponent<Animator>();
        var sourceRoot = owner.transform.Find("root");
        if (bodyAnimator == null || !bodyAnimator.isHuman || sourceRoot == null
            || sourceRoot.parent != owner.transform || owner.transform.Find(RootName) != null)
            throw new InvalidOperationException("Unique authored body and unused visual root required.");
        if (bodyAnimator.GetBoneTransform(HumanBodyBones.Hips) != sourceRoot)
            throw new InvalidOperationException("Original body Animator must already resolve authored Hips.");

        var skins = new List<SkinnedMeshRenderer>();
        var needed = new HashSet<Transform>();
        var names = new HashSet<string>();
        foreach (var skin in owner.GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            if (owner.ViewModelRoot != null && (skin.transform == owner.ViewModelRoot
                || skin.transform.IsChildOf(owner.ViewModelRoot))) continue;
            if (skin.rootBone == null || (skin.rootBone != sourceRoot && !skin.rootBone.IsChildOf(sourceRoot)))
                throw new InvalidOperationException("Every body skin must belong to the authored body.");
            skins.Add(skin);
            var required = new List<Transform>(skin.bones) { skin.rootBone };
            foreach (var bone in required)
            {
                if (bone == null || (bone != sourceRoot && !bone.IsChildOf(sourceRoot)))
                    throw new InvalidOperationException("Missing or foreign body skin bone.");
                for (var current = bone; current != owner.transform; current = current.parent)
                {
                    if (current == null) throw new InvalidOperationException("Detached body bone.");
                    needed.Add(current);
                }
            }
        }
        if (skins.Count == 0) throw new InvalidOperationException("Body skins required.");
        var source = new List<Transform>();
        foreach (var bone in sourceRoot.GetComponentsInChildren<Transform>(true))
        {
            if (!needed.Contains(bone)) continue;
            if (!names.Add(bone.name)) throw new InvalidOperationException("Ambiguous body bone name.");
            source.Add(bone);
        }
        if (source.Count != needed.Count) throw new InvalidOperationException("Incomplete body ancestry.");

        var root = new GameObject(RootName);
        root.transform.SetParent(owner.transform, false);
        var visual = new List<Transform>();
        var map = new Dictionary<Transform, Transform>();
        var originalBones = new Transform[skins.Count][];
        var originalRoots = new Transform[skins.Count];
        int rebound = 0;
        try
        {
            foreach (var bone in source)
            {
                var copy = new GameObject(Prefix + bone.name).transform;
                copy.SetParent(bone == sourceRoot ? root.transform : map[bone.parent], false);
                copy.localPosition = bone.localPosition;
                copy.localRotation = bone.localRotation;
                copy.localScale = bone.localScale;
                map.Add(bone, copy); visual.Add(copy);
            }
            var animator = root.AddComponent<Animator>();
            animator.avatar = null;
            animator.runtimeAnimatorController = null;
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            for (int i = 0; i < skins.Count; i++)
            {
                originalBones[i] = skins[i].bones;
                originalRoots[i] = skins[i].rootBone;
                var replacement = new Transform[originalBones[i].Length];
                for (int j = 0; j < replacement.Length; j++) replacement[j] = map[originalBones[i][j]];
                // Count before either setter so a partially completed pair rolls back.
                rebound = i + 1;
                skins[i].bones = replacement;
                skins[i].rootBone = map[originalRoots[i]];
            }
            return new WorldBodyVisualRigBinding(root, animator, source.ToArray(), visual.ToArray(),
                skins.ToArray(), originalBones, originalRoots);
        }
        catch
        {
            for (int i = 0; i < rebound; i++)
                if (skins[i] != null) { skins[i].bones = originalBones[i]; skins[i].rootBone = originalRoots[i]; }
            DestroyOwned(root);
            throw;
        }
    }

    public void CopySourcePose()
    {
        if (_disposed) return;
        for (int i = 0; i < SourceBones.Length; i++)
        {
            var source = SourceBones[i]; var visual = VisualBones[i];
            if (source == null || visual == null) continue;
            visual.localPosition = source.localPosition;
            visual.localRotation = source.localRotation;
            visual.localScale = source.localScale;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        // Restore references before deferred destruction, including disable/respawn.
        for (int i = 0; i < _skins.Length; i++)
            if (_skins[i] != null) { _skins[i].bones = _originalBones[i]; _skins[i].rootBone = _originalRoots[i]; }
        DestroyOwned(Root);
    }

    private static void DestroyOwned(GameObject owned)
    {
        if (owned == null) return;
        // Detach immediately so a same-frame re-enable can construct a fresh rig.
        owned.SetActive(false); owned.transform.SetParent(null, false);
        if (Application.isPlaying) UnityEngine.Object.Destroy(owned);
        else UnityEngine.Object.DestroyImmediate(owned);
    }
}
