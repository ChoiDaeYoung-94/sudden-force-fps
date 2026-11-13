using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Owns only reversible local poses on the existing 12 dedicated Hitbox children
// and the existing RiflePresentation frame. It never reparents or edits shapes.
internal sealed class WorldBodyPoseAttachmentBinding : IDisposable
{
    private sealed class Attachment
    {
        internal CombatHitbox Metadata;
        internal Hitbox Hitbox;
        internal Transform Target, Parent, VisualParent;
        internal CombatBodyPart Part;
        internal HitboxTypes Type;
        internal Vector3 Position, Scale, BoxExtents, Offset;
        internal Quaternion Rotation;
        internal float SphereRadius, CapsuleRadius, CapsuleExtents;
        internal int Layer;
    }

    private static readonly HumanBodyBones[] RequiredHumans = {
        HumanBodyBones.Hips,
        HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand,
        HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand,
        HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot,
        HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot };
    private readonly GamePlayerNetworkData _owner;
    private readonly WorldBodyVisualRigBinding _rig;
    private readonly Animator _sourceAnimator;
    private readonly Transform[] _humans;
    private readonly Attachment[] _attachments;
    private readonly Transform _rifleParent, _muzzle;
    private readonly Vector3 _riflePosition, _rifleScale, _muzzlePosition, _muzzleScale;
    private readonly Quaternion _rifleRotation, _muzzleRotation;
    private readonly HitboxRoot _hitboxRoot;
    private readonly float _broadRadius;
    private readonly Vector3 _broadOffset;
    private readonly Vector3[] _positions = new Vector3[12];
    private readonly Quaternion[] _rotations = new Quaternion[12];
    private bool _disposed;

    internal readonly Transform RifleFrame;
    internal readonly Transform LeftUpperArm, LeftLowerArm, LeftHand;
    internal readonly Transform RightUpperArm, RightLowerArm, RightHand;
    internal int HitboxCount => _attachments.Length;

    internal WorldBodyPoseAttachmentBinding(GamePlayerNetworkData owner,
        WorldBodyVisualRigBinding rig, Renderer[] originalBodyRendererCache)
    {
        _owner = owner; _rig = rig; _sourceAnimator = owner.GetComponent<Animator>();
        _hitboxRoot = owner.HitboxRoot;
        if (_sourceAnimator == null || !_sourceAnimator.isHuman || _hitboxRoot == null
            || _hitboxRoot.transform != owner.transform || !BodyPoseMath.Unit(owner.transform.lossyScale))
            throw new InvalidOperationException("BODY_POSE_SOURCE_OR_ROOT_INVALID");
        var map = new Dictionary<Transform, Transform>();
        for (int i = 0; i < rig.SourceBones.Length; i++) map.Add(rig.SourceBones[i], rig.VisualBones[i]);
        _humans = new Transform[RequiredHumans.Length];
        for (int i = 0; i < _humans.Length; i++)
        {
            var source = _sourceAnimator.GetBoneTransform(RequiredHumans[i]);
            if (source == null || !map.ContainsKey(source))
                throw new InvalidOperationException("BODY_POSE_ORIGINAL_AVATAR_MAPPING_INVALID:" + RequiredHumans[i]);
            _humans[i] = source;
        }
        LeftUpperArm = map[_humans[1]]; LeftLowerArm = map[_humans[2]]; LeftHand = map[_humans[3]];
        RightUpperArm = map[_humans[4]]; RightLowerArm = map[_humans[5]]; RightHand = map[_humans[6]];
        if (LeftLowerArm.parent != LeftUpperArm || LeftHand.parent != LeftLowerArm
            || RightLowerArm.parent != RightUpperArm || RightHand.parent != RightLowerArm)
            throw new InvalidOperationException("BODY_POSE_ARM_CHAIN_INVALID");
        _muzzle = owner.WorldMuzzle;
        RifleFrame = _muzzle != null ? _muzzle.parent : null;
        _rifleParent = RifleFrame != null ? RifleFrame.parent : null;
        if (RifleFrame == null || RifleFrame.name != "RiflePresentation"
            || _rifleParent == null || _rifleParent.name != "WorldWeaponSocket"
            || _rifleParent.parent != _humans[6] || _muzzle.name != "WorldMuzzle"
            || !BodyPoseMath.Unit(RifleFrame.lossyScale) || !BodyPoseMath.Unit(_muzzle.lossyScale))
            throw new InvalidOperationException("BODY_POSE_EXISTING_RIFLE_HIERARCHY_INVALID");
        _riflePosition = RifleFrame.localPosition; _rifleRotation = RifleFrame.localRotation;
        _rifleScale = RifleFrame.localScale;
        _muzzlePosition = _muzzle.localPosition; _muzzleRotation = _muzzle.localRotation;
        _muzzleScale = _muzzle.localScale;
        ValidateRenderers(originalBodyRendererCache, map);

        var metadata = owner.GetComponentsInChildren<CombatHitbox>(true);
        if (metadata.Length != 12 || owner.GetComponentsInChildren<Hitbox>(true).Length != 12)
            throw new InvalidOperationException("BODY_POSE_EXACT_12_HITBOXES_REQUIRED");
        _attachments = new Attachment[12];
        var targets = new HashSet<Transform>();
        int head = 0, torso = 0, arm = 0, leg = 0;
        for (int i = 0; i < metadata.Length; i++)
        {
            var item = metadata[i]; var target = item.transform; var hitbox = item.Hitbox;
            if (!item.IsConfigured || item.Owner != owner || hitbox == null || hitbox.Root != _hitboxRoot
                || !targets.Add(target) || map.ContainsKey(target) || target.parent == null
                || !map.TryGetValue(target.parent, out var visualParent) || target.childCount != 0
                || target.gameObject.layer != 10 || !BodyPoseMath.Unit(target.lossyScale)
                || !BodyPoseMath.Finite(target.localPosition) || !BodyPoseMath.Finite(target.localRotation)
                || target.name != "Hitbox_" + target.parent.name || item.BodyPart != ExpectedPart(target.name))
                throw new InvalidOperationException("BODY_POSE_DEDICATED_HITBOX_CHILD_REQUIRED:" + target.name);
            // A skin bone, renderer, collider, or behaviour-bearing node must never be moved this way.
            var components = target.GetComponents<Component>();
            if (components.Length != 3)
                throw new InvalidOperationException("BODY_POSE_HITBOX_COMPONENT_SET_INVALID:" + target.name);
            foreach (var component in components)
                if (!(component is Transform) && component != item && component != hitbox)
                    throw new InvalidOperationException("BODY_POSE_HITBOX_COMPONENT_SET_INVALID:" + target.name);
            switch (item.BodyPart)
            {
                case CombatBodyPart.Head: head++; break;
                case CombatBodyPart.Torso: torso++; break;
                case CombatBodyPart.Arm: arm++; break;
                case CombatBodyPart.Leg: leg++; break;
                default: throw new InvalidOperationException("BODY_POSE_PART_INVALID");
            }
            _attachments[i] = new Attachment {
                Metadata = item, Hitbox = hitbox, Target = target, Parent = target.parent,
                VisualParent = visualParent, Part = item.BodyPart, Type = hitbox.Type,
                Position = target.localPosition, Rotation = target.localRotation, Scale = target.localScale,
                SphereRadius = hitbox.SphereRadius, CapsuleRadius = hitbox.CapsuleRadius,
                CapsuleExtents = hitbox.CapsuleExtents, BoxExtents = hitbox.BoxExtents, Offset = hitbox.Offset,
                Layer = target.gameObject.layer };
        }
        if (head != 1 || torso != 3 || arm != 4 || leg != 4)
            throw new InvalidOperationException("BODY_POSE_HITBOX_PART_COUNTS_INVALID");
        _broadRadius = _hitboxRoot.BroadRadius; _broadOffset = _hitboxRoot.Offset;
        ValidateStableContract();
    }

    private static CombatBodyPart ExpectedPart(string name)
    {
        switch (name)
        {
            case "Hitbox_head": return CombatBodyPart.Head;
            case "Hitbox_spine_01": case "Hitbox_spine_03": case "Hitbox_pelvis": return CombatBodyPart.Torso;
            case "Hitbox_upperarm_l": case "Hitbox_upperarm_r":
            case "Hitbox_lowerarm_l": case "Hitbox_lowerarm_r": return CombatBodyPart.Arm;
            case "Hitbox_thigh_l": case "Hitbox_thigh_r":
            case "Hitbox_calf_l": case "Hitbox_calf_r": return CombatBodyPart.Leg;
            default: throw new InvalidOperationException("BODY_POSE_UNKNOWN_HITBOX_CHILD:" + name);
        }
    }

    private void ValidateRenderers(Renderer[] cache, Dictionary<Transform, Transform> map)
    {
        if (cache == null || cache.Length != 15) throw new InvalidOperationException("BODY_POSE_RENDERER_CACHE_15_REQUIRED");
        var actual = new HashSet<Renderer>();
        int skins = 0, rifles = 0;
        foreach (var renderer in cache)
        {
            if (renderer == null || !actual.Add(renderer) || !renderer.transform.IsChildOf(_owner.transform)
                || (_owner.ViewModelRoot != null && (renderer.transform == _owner.ViewModelRoot
                    || renderer.transform.IsChildOf(_owner.ViewModelRoot))))
                throw new InvalidOperationException("BODY_POSE_RENDERER_CACHE_IDENTITY_INVALID");
            if (renderer is SkinnedMeshRenderer skin)
            {
                if (skin.rootBone == null || !skin.rootBone.IsChildOf(_rig.Root.transform))
                    throw new InvalidOperationException("BODY_POSE_BODY_SKIN_NOT_BOUND");
                foreach (var bone in skin.bones)
                    if (bone == null || !bone.IsChildOf(_rig.Root.transform))
                        throw new InvalidOperationException("BODY_POSE_FOREIGN_SKIN_BONE");
                skins++;
            }
            else if (renderer is MeshRenderer && renderer.transform.IsChildOf(RifleFrame)) rifles++;
            else throw new InvalidOperationException("BODY_POSE_UNEXPECTED_CACHED_RENDERER");
        }
        var rifleRenderers = RifleFrame.GetComponentsInChildren<Renderer>(true);
        if (skins != 8 || rifles != 7 || rifleRenderers.Length != 7)
            throw new InvalidOperationException("BODY_POSE_EXISTING_8_SKINS_7_RIFLE_REQUIRED");
        foreach (var renderer in rifleRenderers)
            if (!actual.Contains(renderer) || !(renderer is MeshRenderer))
                throw new InvalidOperationException("BODY_POSE_RIFLE_RENDERER_CACHE_MISMATCH");
        int worldRendererCount = 0;
        foreach (var renderer in _owner.GetComponentsInChildren<Renderer>(true))
        {
            if (_owner.ViewModelRoot != null && (renderer.transform == _owner.ViewModelRoot
                || renderer.transform.IsChildOf(_owner.ViewModelRoot))) continue;
            if (!actual.Contains(renderer)) throw new InvalidOperationException("BODY_POSE_UNCACHED_WORLD_RENDERER");
            worldRendererCount++;
        }
        if (worldRendererCount != 15) throw new InvalidOperationException("BODY_POSE_WORLD_RENDERER_COUNT_CHANGED");
        // The map contains source skin bones plus ancestry, so dedicated-child
        // rejection above also protects every original skin-bone transform.
        if (map.Count != _rig.SourceBones.Length) throw new InvalidOperationException("BODY_POSE_BONE_MAP_INVALID");
    }

    internal void ValidateStableContract()
    {
        if (_disposed || _owner == null || _hitboxRoot == null || _owner.HitboxRoot != _hitboxRoot
            || _hitboxRoot.transform != _owner.transform || !BodyPoseMath.Unit(_owner.transform.lossyScale)
            || !_hitboxRoot.BroadRadius.Equals(_broadRadius) || !_hitboxRoot.Offset.Equals(_broadOffset)
            || RifleFrame == null || RifleFrame.parent != _rifleParent || _rifleParent.parent != _humans[6]
            || _muzzle == null || _owner.WorldMuzzle != _muzzle || _muzzle.parent != RifleFrame
            || !_muzzle.localPosition.Equals(_muzzlePosition) || !_muzzle.localRotation.Equals(_muzzleRotation)
            || !_muzzle.localScale.Equals(_muzzleScale) || !RifleFrame.localScale.Equals(_rifleScale))
            throw new InvalidOperationException("BODY_POSE_EXISTING_ROOT_RIFLE_CONTRACT_CHANGED");
        for (int i = 0; i < _humans.Length; i++)
            if (_sourceAnimator.GetBoneTransform(RequiredHumans[i]) != _humans[i])
                throw new InvalidOperationException("BODY_POSE_ORIGINAL_AVATAR_IDENTITY_CHANGED:" + RequiredHumans[i]);
        foreach (var a in _attachments)
            if (a.Target == null || a.Parent == null || a.VisualParent == null || a.Metadata == null || a.Hitbox == null
                || a.Target.parent != a.Parent || a.Target.childCount != 0 || a.Target.gameObject.layer != a.Layer
                || !a.Target.localScale.Equals(a.Scale) || a.Metadata.Owner != _owner || a.Metadata.BodyPart != a.Part
                || a.Metadata.Hitbox != a.Hitbox || a.Hitbox.Root != _hitboxRoot || !a.Metadata.IsConfigured
                || a.Hitbox.Type != a.Type || !a.Hitbox.SphereRadius.Equals(a.SphereRadius)
                || !a.Hitbox.CapsuleRadius.Equals(a.CapsuleRadius) || !a.Hitbox.CapsuleExtents.Equals(a.CapsuleExtents)
                || !a.Hitbox.BoxExtents.Equals(a.BoxExtents) || !a.Hitbox.Offset.Equals(a.Offset)
                || !BodyPoseMath.Unit(a.Parent.lossyScale) || !BodyPoseMath.Unit(a.VisualParent.lossyScale))
                throw new InvalidOperationException("BODY_POSE_HITBOX_METADATA_SHAPE_OR_HIERARCHY_CHANGED:"
                    + (a.Target != null ? a.Target.name : "destroyed"));
    }

    // Called before the one source-pose copy, also on dead transitions. It cannot
    // leave last frame's gun world pose as a new death-animation attachment.
    internal void RestoreLocals()
    {
        Exception first = null;
        foreach (var a in _attachments)
            try
            {
                if (a.Target == null) continue;
                if (a.Target.parent != a.Parent) throw new InvalidOperationException("BODY_POSE_RESTORE_PARENT_CHANGED");
                a.Target.localPosition = a.Position; a.Target.localRotation = a.Rotation;
                a.Target.localScale = a.Scale;
            }
            catch (Exception error) { if (first == null) first = error; }
        try
        {
            if (RifleFrame != null)
            {
                if (RifleFrame.parent != _rifleParent) throw new InvalidOperationException("BODY_POSE_RESTORE_RIFLE_PARENT_CHANGED");
                RifleFrame.localPosition = _riflePosition; RifleFrame.localRotation = _rifleRotation;
                RifleFrame.localScale = _rifleScale;
            }
        }
        catch (Exception error) { if (first == null) first = error; }
        if (first != null) throw first;
    }

    internal void Commit(Vector3 riflePosition, Quaternion rifleRotation)
    {
        ValidateStableContract();
        if (!BodyPoseMath.Finite(riflePosition) || !BodyPoseMath.Finite(rifleRotation))
            throw new InvalidOperationException("BODY_POSE_RIFLE_POSE_NONFINITE");
        // Prepare all desired transforms before touching the original attachments.
        for (int i = 0; i < _attachments.Length; i++)
        {
            var a = _attachments[i];
            _positions[i] = a.VisualParent.TransformPoint(a.Position);
            _rotations[i] = a.VisualParent.rotation * a.Rotation;
            if (!BodyPoseMath.Finite(_positions[i]) || !BodyPoseMath.Finite(_rotations[i]))
                throw new InvalidOperationException("BODY_POSE_MAPPED_HITBOX_POSE_NONFINITE");
        }
        try
        {
            for (int i = 0; i < _attachments.Length; i++)
                _attachments[i].Target.SetPositionAndRotation(_positions[i], _rotations[i]);
            RifleFrame.SetPositionAndRotation(riflePosition, rifleRotation);
        }
        catch { RestoreLocals(); throw; }
    }

    public void Dispose()
    {
        if (_disposed) return;
        try { RestoreLocals(); } finally { _disposed = true; }
    }
}
