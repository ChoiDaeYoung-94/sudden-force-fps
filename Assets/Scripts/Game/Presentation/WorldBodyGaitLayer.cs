using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

// Product Raw9 candidate playback is independent of QA login/defines.
// Activation permits execution; actual runtime acceptance belongs to separate evidence.
// Contact03/Raw18 diagnostics remain closed and unaccepted.
internal sealed class WorldBodyGaitLayer : IDisposable
{
    private enum ExecutionMode { Closed, RawLocomotionProductCandidate }
    // Source-bound product candidate mode; contract activation is not a runtime acceptance claim.
    private static readonly ExecutionMode SelectedMode = ExecutionMode.RawLocomotionProductCandidate;
    private static readonly bool Contact03ExecutionEnabled = false;
    private const string ProductCandidateRaw9ContractSha256 = "456A133706BAC9536BAC29869F09E0ADDA3649F39BE5DFC07F65CA6229FEB3F6";
    private const string Raw9MaterializationReviewSha256 = "DED75CE3B35FF299536E632A4C2A6C10D96B035D22597A1E26BE7A01E42DE590";
    private const string Raw9AssetSetIdentitySha256 = "9994B206304EFCE9D9B527212B0322149C119AA8D77C6065D3B60B34580D627A";
    private const string ApprovedPostBlend18ContractSha256 = "";
    private const string ApprovedSoleGeometrySha256 = "";
    private const string ApprovedDiagnosticEvidenceSha256 = "";
    private const string RequiredC11SourceSha256 = "31789E517B1C94D597C2B84D5AD5742686A5F4629CD4A386012E6311E39A19C3";
    private const string ResourceRoot = "WorldBodyGaitContact03/";
    private const float PhysicalMargin = .001f, RawCap = .080f;
    private static readonly string[] Names = { "LegIdle", "WalkForward", "WalkBackward", "WalkLeft", "WalkRight",
        "RunForward", "RunBackward", "RunLeft", "RunRight" };
    private static readonly string[] LegPaths = {
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_l",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_l/SFF_LOCO_calf_l",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_l/SFF_LOCO_calf_l/SFF_LOCO_calf_twist_01_l",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_l/SFF_LOCO_calf_l/SFF_LOCO_foot_l",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_l/SFF_LOCO_calf_l/SFF_LOCO_foot_l/SFF_LOCO_ball_l",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_l/SFF_LOCO_thigh_twist_01_l",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_r",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_r/SFF_LOCO_calf_r",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_r/SFF_LOCO_calf_r/SFF_LOCO_calf_twist_01_r",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_r/SFF_LOCO_calf_r/SFF_LOCO_foot_r",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_r/SFF_LOCO_calf_r/SFF_LOCO_foot_r/SFF_LOCO_ball_r",
        "SFF_LOCO_root/SFF_LOCO_pelvis/SFF_LOCO_thigh_r/SFF_LOCO_thigh_twist_01_r" };

    [Serializable] private sealed class ClipIdentity
    {
        public string name, resourcePath, serializedAssetSha256, sourceSampleSha256;
        public float duration;
        public int rotationCurveCount, otherCurveCount, eventCount;
    }
    [Serializable] private sealed class RawClipIdentity
    {
        public string name, resourcePath, serializedAssetSha256, serializedMetaSha256, assetGuid, assetPath;
        public string sourceSampleSha256, curveInputSampleSha256;
        public float duration;
        public int rotationCurveCount, otherCurveCount, eventCount, curveInputKeyCount;
        public bool loadedCurveKeysMatchInput;
    }
    [Serializable] private sealed class Raw9Contract
    {
        public string schema, mode, materializationReviewSha256, clipAssetSetIdentitySha256;
        public string[] legPaths;
        public RawClipIdentity[] raw;
        public bool productCandidateActivation, oneManualGraph, existingAnimatorOnly;
        public bool sameNormalizedPhase, rawSeamCuratedSource, legRotationOnly;
        public bool contactCorrectionEnabled, actualRuntimeAccepted;
    }
    [Serializable] private sealed class PostBlendContract
    {
        public string schema, diagnosticMode, geometrySha256, diagnosticEvidenceSha256, clipAssetSetIdentitySha256;
        public string[] legPaths;
        public ClipIdentity[] raw, corrected;
        public float authoredCardinalMarginMetres, runtimeSoleTargetMetres, fullSkinTrialMinimumMetres, rawCumulativeCapMetres;
        public bool oneManualGraph, existingAnimatorOnly, sameNormalizedPhase, rawMixerIsReference, diagnosticOnly;
    }
    [Serializable] private sealed class DiagnosticEvidence
    {
        public string schema, geometrySha256, clipAssetSetIdentitySha256;
        public string sourceC11Sha256, actualC11ReviewSha256, materializationReviewSha256, supportPlaneReviewSha256;
        public bool cardinalAuthored8mmPreserved, raw9SeamCuratedPreserved, oldBlend8mmFailurePreserved;
        public bool runtimeVertical1mmTrialAccepted, fullSkin2mmTrialAccepted, raw18MaterializationAccepted;
        public bool boots704FullWeightGeometryAccepted, flatSupportPlaneCalibrationAccepted;
        public bool runtimeAllPosesAccepted, gpuAccepted, twoPeerLifeHistoryAccepted, productActivationAccepted;
    }

    private readonly WorldBodyVisualRigBinding _rig;
    private readonly AnimationClip[] _clips, _rawClips;
    private readonly Transform[] _parents, _legs;
    private readonly bool[] _allowedLeg;
    private readonly Vector3[] _positions, _scales;
    private readonly Quaternion[] _rotations;
    private readonly Transform _rootParent;
    private readonly GaitSoleGeometryCache _geometry;
    private Vector3 _rootPosition, _rootScale;
    private Quaternion _rootRotation;
    private PlayableGraph _graph;
    private AnimationMixerPlayable _mixer, _rawMixer;
    private AnimationClipPlayable[] _players, _rawPlayers;
    private AnimationPlayableOutput _output;
    private bool _disposed;
    private readonly bool _rawOnly;

    internal static bool TryCreate(WorldBodyVisualRigBinding rig, out WorldBodyGaitLayer layer, out string status)
    {
        layer = null; status = "rifle-only-product-raw9-closed";
        // Invalid mode/pin stops before Resources; this Raw9 path does not depend on QA defines.
        if (SelectedMode != ExecutionMode.RawLocomotionProductCandidate || !IsHex(ProductCandidateRaw9ContractSha256, 64)) return false;
        try
        {
            var asset = Resources.Load<TextAsset>(ResourceRoot + "Raw9Contract");
            Need(asset != null && Hash(asset.bytes) == ProductCandidateRaw9ContractSha256, "GAIT_PRODUCT_RAW9_EXACT_CANDIDATE_CONTRACT");
            var contract = JsonUtility.FromJson<Raw9Contract>(asset.text);
            ValidateRawContract(contract);
            var raw = LoadRawClips(contract.raw);
            layer = new WorldBodyGaitLayer(rig, raw, raw, null, true);
            layer.BuildGraph(); status = "product-candidate-raw9-ready-contact-off"; return true;
        }
        catch (Exception error)
        {
            try { layer?.Dispose(); } finally { layer = null; }
            status = "rifle-only-product-raw9-unavailable:" + error.GetType().Name; return false;
        }
    }

    // Frozen03 diagnostic gate retained separately; it is never selected by the product Raw9 factory.
    private static bool TryCreateContact03Closed(WorldBodyVisualRigBinding rig, out WorldBodyGaitLayer layer, out string status)
    {
        layer = null;
        status = "rifle-only-gait03-postblend-diagnostic-unapproved";
        if (!Contact03ExecutionEnabled) return false;
#if !SUDDEN_FORCE_LOCAL_QA
        return false;
#else
        // Every hash is empty in this candidate. This branch returns before Resources,
        // parsing, cache allocation, graph creation, or any pose evaluation.
        if (!GaitSoleGeometryCache.IsSha(ApprovedPostBlend18ContractSha256)
            || !GaitSoleGeometryCache.IsSha(ApprovedSoleGeometrySha256)
            || !GaitSoleGeometryCache.IsSha(ApprovedDiagnosticEvidenceSha256)) return false;
        try
        {
            var contractAsset = Resources.Load<TextAsset>(ResourceRoot + "PostBlendContract");
            var geometryAsset = Resources.Load<TextAsset>(ResourceRoot + "SoleGeometry");
            var evidenceAsset = Resources.Load<TextAsset>(ResourceRoot + "DiagnosticEvidence");
            Need(contractAsset != null && geometryAsset != null && evidenceAsset != null
                && Hash(contractAsset.bytes) == ApprovedPostBlend18ContractSha256
                && Hash(geometryAsset.bytes) == ApprovedSoleGeometrySha256
                && Hash(evidenceAsset.bytes) == ApprovedDiagnosticEvidenceSha256, "GAIT03_THREE_EXACT_APPROVED_ASSETS");
            var contract = JsonUtility.FromJson<PostBlendContract>(contractAsset.text);
            var geometry = JsonUtility.FromJson<GaitContactGeometryContract>(geometryAsset.text);
            var evidence = JsonUtility.FromJson<DiagnosticEvidence>(evidenceAsset.text);
            ValidateContract(contract, evidence);
            var raw = LoadClips(contract.raw, "Raw/"); var corrected = LoadClips(contract.corrected, "Corrected/");
            for (int i = 0; i < 9; i++) Need(contract.raw[i].sourceSampleSha256 == contract.corrected[i].sourceSampleSha256, "GAIT03_SAME_RAW_SOURCE_PER_CLIP_PAIR");
            layer = new WorldBodyGaitLayer(rig, raw, corrected, geometry);
            layer.BuildGraph(); status = "qa-gait03-ready-not-runtime-product-accepted"; return true;
        }
        catch (Exception error)
        {
            try { layer?.Dispose(); } finally { layer = null; }
            status = "rifle-only-gait03-unavailable:" + error.GetType().Name + ":" + error.Message;
            return false;
        }
#endif
    }

    private static void ValidateRawContract(Raw9Contract c)
    {
        Need(c != null && c.schema == "SFF_GAIT_RAW9_PRODUCT_CANDIDATE_CONTRACT_V1" && c.mode == "RawLocomotionProductCandidate"
            && c.productCandidateActivation && c.oneManualGraph && c.existingAnimatorOnly
            && c.sameNormalizedPhase && c.rawSeamCuratedSource && c.legRotationOnly
            && !c.contactCorrectionEnabled && !c.actualRuntimeAccepted
            && c.materializationReviewSha256 == Raw9MaterializationReviewSha256
            && c.clipAssetSetIdentitySha256 == Raw9AssetSetIdentitySha256
            && c.legPaths != null && c.legPaths.Length == 12, "GAIT_PRODUCT_RAW9_EXPLICIT_CANDIDATE_CONTRACT");
        for (int i = 0; i < 12; i++) Need(c.legPaths[i] == LegPaths[i], "GAIT_PRODUCT_RAW9_EXACT_LEG_PATHS");
    }

    private static AnimationClip[] LoadRawClips(RawClipIdentity[] rows)
    {
        Need(rows != null && rows.Length == 9, "GAIT_PRODUCT_RAW9_EXACT_RAW_NINE");
        var clips = new AnimationClip[9];
        for (int i = 0; i < 9; i++)
        {
            var row = rows[i]; string path = ResourceRoot + "Raw/" + Names[i];
            Need(row != null && row.name == Names[i] && row.resourcePath == path
                && row.assetPath == "Assets/Resources/" + path + ".anim"
                && IsHex(row.assetGuid, 32) && IsHex(row.serializedAssetSha256, 64)
                && IsHex(row.serializedMetaSha256, 64) && IsHex(row.sourceSampleSha256, 64)
                && row.curveInputSampleSha256 == row.sourceSampleSha256
                && row.rotationCurveCount == 48 && row.otherCurveCount == 0 && row.eventCount == 0
                && row.curveInputKeyCount > 1 && row.loadedCurveKeysMatchInput,
                "GAIT_PRODUCT_RAW9_EXACT_RAW_ASSET_DECLARATION");
            clips[i] = Resources.Load<AnimationClip>(path);
            Need(clips[i] != null && clips[i].name == Names[i] && !clips[i].humanMotion && !clips[i].legacy
                && clips[i].isLooping && PlayerInputSource.IsFinite(clips[i].length) && clips[i].length > 0f
                && clips[i].length.Equals(row.duration) && clips[i].events.Length == 0, "GAIT_PRODUCT_RAW9_EXACT_GENERIC_LOOP_CLIP");
        }
        // Serialized byte/GUID/curve bindings are audited by Root's staging/import review.
        return clips;
    }

    private static bool IsHex(string value, int length)
    {
        if (value == null || value.Length != length) return false;
        for (int i = 0; i < value.Length; i++)
            if (!((value[i] >= '0' && value[i] <= '9') || (value[i] >= 'a' && value[i] <= 'f')
                || (value[i] >= 'A' && value[i] <= 'F'))) return false;
        return true;
    }

    private static void ValidateContract(PostBlendContract c, DiagnosticEvidence e)
    {
        Need(c != null && e != null && c.schema == "SFF_GAIT_RAW18_POSTBLEND_CONTACT1MM_V1"
            && c.diagnosticMode == "runtimeVerticalContact1mm"
            && c.authoredCardinalMarginMetres.Equals(.008f) && c.runtimeSoleTargetMetres.Equals(.001f)
            && c.fullSkinTrialMinimumMetres.Equals(-.002f) && c.rawCumulativeCapMetres.Equals(.080f)
            && c.oneManualGraph && c.existingAnimatorOnly && c.sameNormalizedPhase && c.rawMixerIsReference && c.diagnosticOnly
            && c.geometrySha256 == ApprovedSoleGeometrySha256 && c.diagnosticEvidenceSha256 == ApprovedDiagnosticEvidenceSha256
            && GaitSoleGeometryCache.IsSha(c.clipAssetSetIdentitySha256)
            && c.legPaths != null && c.legPaths.Length == 12, "GAIT03_EXPLICIT_SEPARATE_DIAGNOSTIC_CONTRACT");
        for (int i = 0; i < 12; i++) Need(c.legPaths[i] == LegPaths[i], "GAIT03_EXACT_LEG_PATHS");
        Need(e.schema == "SFF_GAIT_CONTACT1MM_ACTUAL_DIAGNOSTIC_EVIDENCE_V1"
            && e.geometrySha256 == ApprovedSoleGeometrySha256 && e.clipAssetSetIdentitySha256 == c.clipAssetSetIdentitySha256
            && e.sourceC11Sha256 == RequiredC11SourceSha256 && GaitSoleGeometryCache.IsSha(e.actualC11ReviewSha256)
            && GaitSoleGeometryCache.IsSha(e.materializationReviewSha256) && GaitSoleGeometryCache.IsSha(e.supportPlaneReviewSha256)
            && e.cardinalAuthored8mmPreserved && e.raw9SeamCuratedPreserved && e.oldBlend8mmFailurePreserved
            && e.runtimeVertical1mmTrialAccepted && e.fullSkin2mmTrialAccepted && e.raw18MaterializationAccepted
            && e.boots704FullWeightGeometryAccepted && e.flatSupportPlaneCalibrationAccepted,
            "GAIT03_ACTUAL_DIAGNOSTIC_MATERIALIZATION_GEOMETRY_AND_PLANE_EVIDENCE_REQUIRED");
        Need(!e.runtimeAllPosesAccepted && !e.gpuAccepted && !e.twoPeerLifeHistoryAccepted && !e.productActivationAccepted,
            "GAIT03_OFFLINE_DIAGNOSTIC_MUST_NOT_CLAIM_RUNTIME_OR_PRODUCT_ACCEPTANCE");
    }
    private static AnimationClip[] LoadClips(ClipIdentity[] rows, string role)
    {
        Need(rows != null && rows.Length == 9, "GAIT03_EXACT_NINE_PER_ROLE");
        var clips = new AnimationClip[9];
        for (int i = 0; i < 9; i++)
        {
            var row = rows[i]; string path = ResourceRoot + role + Names[i];
            Need(row != null && row.name == Names[i] && row.resourcePath == path
                && GaitSoleGeometryCache.IsSha(row.serializedAssetSha256) && GaitSoleGeometryCache.IsSha(row.sourceSampleSha256)
                && row.rotationCurveCount == 48 && row.otherCurveCount == 0 && row.eventCount == 0,
                "GAIT03_EXACT_18_CLIP_IDENTITY_DECLARATION");
            clips[i] = Resources.Load<AnimationClip>(path);
            Need(clips[i] != null && clips[i].name == Names[i] && !clips[i].humanMotion && !clips[i].legacy
                && clips[i].isLooping && PlayerInputSource.IsFinite(clips[i].length) && clips[i].length > 0f
                && clips[i].length.Equals(row.duration) && clips[i].events.Length == 0, "GAIT03_EXACT_GENERIC_LOOP_CLIP");
        }
        return clips;
    }

    private WorldBodyGaitLayer(WorldBodyVisualRigBinding rig, AnimationClip[] raw, AnimationClip[] corrected,
        GaitContactGeometryContract geometry, bool rawOnly = false)
    {
        _rawOnly = rawOnly;
        Need(rig != null && rig.Root != null && rig.Animator != null && rig.VisualBones != null
            && rig.SourceBones.Length == rig.VisualBones.Length, "GAIT_EXISTING_OWNED_RIG_REQUIRED");
        _rig = rig; _clips = corrected; _rawClips = raw; _rootParent = rig.Root.transform.parent;
        Need(_rootParent != null, "GAIT03_EXISTING_OWNER_ROOT_REQUIRED");
        int count = rig.VisualBones.Length;
        _parents = new Transform[count]; _allowedLeg = new bool[count]; _legs = new Transform[12];
        _positions = new Vector3[count]; _rotations = new Quaternion[count]; _scales = new Vector3[count];
        var allowed = new HashSet<Transform>();
        for (int i = 0; i < 12; i++)
        {
            _legs[i] = rig.Root.transform.Find(LegPaths[i]);
            Need(_legs[i] != null && allowed.Add(_legs[i]), "GAIT_12_EXISTING_LEGS_REQUIRED");
        }
        int legCount = 0;
        for (int i = 0; i < count; i++)
        {
            Need(rig.VisualBones[i] != null, "GAIT_EXISTING_VISUAL_BONE_REQUIRED");
            _parents[i] = rig.VisualBones[i].parent; _allowedLeg[i] = allowed.Contains(rig.VisualBones[i]);
            if (_allowedLeg[i]) legCount++;
        }
        Need(legCount == 12, "GAIT_12_LEG_REGISTRY_REQUIRED");
        if (!rawOnly) _geometry = new GaitSoleGeometryCache(rig, _legs, geometry);
    }
    private void BuildGraph()
    {
        if (_rawOnly) { BuildRawGraph(); return; }
        _graph = PlayableGraph.Create("SFFOwnedWorldBodyGait03Raw18");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
        _mixer = AnimationMixerPlayable.Create(_graph, 9); _rawMixer = AnimationMixerPlayable.Create(_graph, 9);
        _players = new AnimationClipPlayable[9]; _rawPlayers = new AnimationClipPlayable[9];
        for (int i = 0; i < 9; i++)
        {
            _players[i] = AnimationClipPlayable.Create(_graph, _clips[i]);
            _players[i].SetSpeed(0d); _players[i].SetApplyFootIK(false); _players[i].SetApplyPlayableIK(false);
            _graph.Connect(_players[i], 0, _mixer, i);
            _rawPlayers[i] = AnimationClipPlayable.Create(_graph, _rawClips[i]);
            _rawPlayers[i].SetSpeed(0d); _rawPlayers[i].SetApplyFootIK(false); _rawPlayers[i].SetApplyPlayableIK(false);
            _graph.Connect(_rawPlayers[i], 0, _rawMixer, i);
        }
        _rig.Animator.applyRootMotion = false; _rig.Animator.enabled = true;
        _output = AnimationPlayableOutput.Create(_graph, "OwnedRawThenCorrectedLegsOnly", _rig.Animator);
        _output.SetSourcePlayable(_mixer); _graph.Play();
    }

    private void BuildRawGraph()
    {
        _graph = PlayableGraph.Create("SFFOwnedWorldBodyGaitProductCandidateRaw9");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
        _mixer = AnimationMixerPlayable.Create(_graph, 9); _players = new AnimationClipPlayable[9];
        for (int i = 0; i < 9; i++)
        {
            _players[i] = AnimationClipPlayable.Create(_graph, _clips[i]);
            _players[i].SetSpeed(0d); _players[i].SetApplyFootIK(false); _players[i].SetApplyPlayableIK(false);
            _graph.Connect(_players[i], 0, _mixer, i);
        }
        _rig.Animator.applyRootMotion = false; _rig.Animator.enabled = true;
        _output = AnimationPlayableOutput.Create(_graph, "OwnedRaw9LegsOnlyProductCandidate", _rig.Animator);
        _output.SetSourcePlayable(_mixer); _graph.Play();
    }

    internal bool EvaluateAfterCopy(WorldBodyPoseContext context, out string status, out bool faulted)
    {
        faulted = false; status = "source-stance-gait-off";
        if (_disposed) throw new InvalidOperationException("GAIT_DISPOSED_LAYER_REUSED");
        if (!context.SourceLifeAligned || !context.Alive || context.Crouched || context.Climbing || !context.GaitEligible) return false;
        CaptureBaseline();
        try
        {
            float speed = context.LocalVelocity.magnitude;
            Need(PlayerInputSource.IsFinite(context.UnwrappedPhase) && context.UnwrappedPhase >= 0f && context.UnwrappedPhase <= 4096f
                && PlayerInputSource.IsFinite(context.LocalVelocity.x) && PlayerInputSource.IsFinite(context.LocalVelocity.y)
                && PlayerInputSource.IsFinite(speed) && speed <= 8f, "GAIT_INVALID_EXPLICIT_TICK_STATE");
            float blend = Mathf.InverseLerp(.15f, 1.5f, speed), run = Mathf.InverseLerp(5.1f, 6.8f, speed);
            float total = Mathf.Abs(context.LocalVelocity.x) + Mathf.Abs(context.LocalVelocity.y);
            float f = total > .0001f ? Mathf.Max(0f, context.LocalVelocity.y) / total : 0f;
            float b = total > .0001f ? Mathf.Max(0f, -context.LocalVelocity.y) / total : 0f;
            float l = total > .0001f ? Mathf.Max(0f, -context.LocalVelocity.x) / total : 0f;
            float r = total > .0001f ? Mathf.Max(0f, context.LocalVelocity.x) / total : 0f;
            float phase = context.UnwrappedPhase - Mathf.Floor(context.UnwrappedPhase);
            if (_rawOnly)
            {
                SetWeights(_mixer, blend, run, f, b, l, r);
                for (int i = 0; i < 9; i++) _players[i].SetTime(phase * _clips[i].length);
                _graph.Evaluate(0f); VerifyLayerIsolation();
                status = "product-candidate-raw9-before-rifle-contact-off"; return true;
            }
            SetWeights(_rawMixer, blend, run, f, b, l, r); SetWeights(_mixer, blend, run, f, b, l, r);
            for (int i = 0; i < 9; i++)
            {
                _rawPlayers[i].SetTime(phase * _rawClips[i].length);
                _players[i].SetTime(phase * _clips[i].length);
            }
            // ONE existing Animator output. No second CopySourcePose, rig or time clock.
            _output.SetSourcePlayable(_rawMixer); _graph.Evaluate(0f); VerifyLayerIsolation();
            Vector3 rawLeft = _legs[3].position, rawRight = _legs[9].position;
            _output.SetSourcePlayable(_mixer); _graph.Evaluate(0f); VerifyLayerIsolation();
            ApplyPostBlend(rawLeft, rawRight); VerifyLayerIsolation();
            status = "qa-gait03-postblend1mm-before-rifle-not-fullskin-or-product-pass"; return true;
        }
        catch (Exception error)
        {
            try { Dispose(); } finally { RestoreBaseline(); }
            faulted = true; status = "rifle-only-gait-fault:" + error.GetType().Name + ":" + error.Message; return false;
        }
    }
    private static void SetWeights(AnimationMixerPlayable m, float blend, float run, float f, float b, float l, float r)
    {
        m.SetInputWeight(0, 1f - blend);
        SetDirections(m, 1, f, b, l, r, blend * (1f - run)); SetDirections(m, 5, f, b, l, r, blend * run);
    }
    private static void SetDirections(AnimationMixerPlayable m, int first, float f, float b, float l, float r, float scale)
    {
        m.SetInputWeight(first, f * scale); m.SetInputWeight(first + 1, b * scale);
        m.SetInputWeight(first + 2, l * scale); m.SetInputWeight(first + 3, r * scale);
    }
    private void ApplyPostBlend(Vector3 rawLeft, Vector3 rawRight)
    {
        var left = _legs[3]; var right = _legs[9];
        Quaternion leftOrientation = left.rotation, rightOrientation = right.rotation;
        float plane = _rootParent.position.y + _geometry.OwnerOriginPlaneOffsetMetres;
        for (int iteration = 0; iteration < 4; iteration++)
        {
            _geometry.MeasureSoles(plane, out float a, out float b);
            float liftLeft = Mathf.Max(0f, PhysicalMargin - a), liftRight = Mathf.Max(0f, PhysicalMargin - b);
            if (liftLeft <= .00002f && liftRight <= .00002f) break;
            Vector3 leftTarget = left.position + Vector3.up * liftLeft, rightTarget = right.position + Vector3.up * liftRight;
            // Validate BOTH targets before changing either chain. Same original raw reference every iteration.
            Need(Vector3.Distance(leftTarget, rawLeft) <= RawCap && Vector3.Distance(rightTarget, rawRight) <= RawCap,
                "GAIT03_CUMULATIVE_RAW80MM_REQUEST_REJECTED");
            Need((liftLeft <= .00002f || GaitPostBlendContactSolver.CanReach(_legs[0], _legs[1], left, leftTarget))
                && (liftRight <= .00002f || GaitPostBlendContactSolver.CanReach(_legs[6], _legs[7], right, rightTarget)),
                "GAIT03_BOTH_STRICT_TRIANGLES_REQUIRED");
            if (liftLeft > .00002f) GaitPostBlendContactSolver.Solve(_legs[0], _legs[1], left, leftTarget, leftOrientation, _rootParent.forward);
            if (liftRight > .00002f) GaitPostBlendContactSolver.Solve(_legs[6], _legs[7], right, rightTarget, rightOrientation, _rootParent.forward);
        }
        _geometry.MeasureSoles(plane, out float finalLeft, out float finalRight);
        // These are the unchanged C11 numeric allowances, not another movement budget.
        Need(finalLeft >= PhysicalMargin - .00005f && finalRight >= PhysicalMargin - .00005f, "GAIT03_BOOT_SOLE_TARGET1MM_FAILED");
        Need(Vector3.Distance(left.position, rawLeft) <= RawCap + .00001f
            && Vector3.Distance(right.position, rawRight) <= RawCap + .00001f, "GAIT03_FINAL_RAW80MM_FAILED");
        Need(Quaternion.Angle(leftOrientation, left.rotation) <= .05f
            && Quaternion.Angle(rightOrientation, right.rotation) <= .05f, "GAIT03_FOOT_WORLD_ORIENTATION_CHANGED");
        // Only the 704-vertex boots are sampled here. Full-skin>=-2mm after rifle,
        // GPU/culling, terrain support, two peers and history remain actual QA gates.
    }

    private void CaptureBaseline()
    {
        if (_rig.Root == null || _rig.Root.transform.parent != _rootParent)
            throw new InvalidOperationException("GAIT_VISUAL_ROOT_PARENT_CHANGED");
        var root = _rig.Root.transform;
        _rootPosition = root.localPosition; _rootRotation = root.localRotation; _rootScale = root.localScale;
        for (int i = 0; i < _rig.VisualBones.Length; i++)
        {
            var bone = _rig.VisualBones[i];
            if (bone == null || bone.parent != _parents[i]) throw new InvalidOperationException("GAIT_VISUAL_PARENT_CHANGED");
            _positions[i] = bone.localPosition; _rotations[i] = bone.localRotation; _scales[i] = bone.localScale;
        }
    }

    private void VerifyLayerIsolation()
    {
        var root = _rig.Root.transform;
        if (root.parent != _rootParent || !Exact(root.localPosition, _rootPosition)
            || !Exact(root.localRotation, _rootRotation) || !Exact(root.localScale, _rootScale))
            throw new InvalidOperationException("GAIT_CHANGED_VISUAL_ROOT");
        for (int i = 0; i < _rig.VisualBones.Length; i++)
        {
            var bone = _rig.VisualBones[i];
            if (bone == null || bone.parent != _parents[i] || !Exact(bone.localPosition, _positions[i])
                || !Exact(bone.localScale, _scales[i]) || (!_allowedLeg[i] && !Exact(bone.localRotation, _rotations[i]))
                || !BodyPoseMath.Finite(bone.localRotation))
                throw new InvalidOperationException("GAIT_CHANGED_NONLEG_BASELINE:" + i);
        }
    }

    private void RestoreBaseline()
    {
        var root = _rig.Root.transform;
        if (root.parent != _rootParent) throw new InvalidOperationException("GAIT_CANNOT_REPARENT_TO_RESTORE");
        root.localPosition = _rootPosition; root.localRotation = _rootRotation; root.localScale = _rootScale;
        for (int i = 0; i < _rig.VisualBones.Length; i++)
        {
            var bone = _rig.VisualBones[i];
            if (bone == null || bone.parent != _parents[i]) throw new InvalidOperationException("GAIT_CANNOT_RESTORE_CHANGED_HIERARCHY");
            bone.localPosition = _positions[i]; bone.localRotation = _rotations[i]; bone.localScale = _scales[i];
            if (!Exact(bone.localPosition, _positions[i]) || !Exact(bone.localRotation, _rotations[i]) || !Exact(bone.localScale, _scales[i]))
                throw new InvalidOperationException("GAIT_EXACT_BASELINE_RESTORE_FAILED");
        }
        VerifyLayerIsolation();
    }


    private static void Need(bool ok, string code) { if (!ok) throw new InvalidOperationException(code); }
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    private struct FloatBits
    {
        [System.Runtime.InteropServices.FieldOffset(0)] internal float value;
        [System.Runtime.InteropServices.FieldOffset(0)] internal int bits;
    }
    private static int Bits(float value) => new FloatBits { value = value }.bits;
    private static bool Exact(Vector3 a, Vector3 b) => Bits(a.x) == Bits(b.x) && Bits(a.y) == Bits(b.y) && Bits(a.z) == Bits(b.z);
    private static bool Exact(Quaternion a, Quaternion b) => Bits(a.x) == Bits(b.x) && Bits(a.y) == Bits(b.y) && Bits(a.z) == Bits(b.z) && Bits(a.w) == Bits(b.w);
    private static string Hash(byte[] bytes)
    { using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", ""); }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        try { if (_graph.IsValid()) _graph.Destroy(); }
        finally { if (_rig.Animator != null) _rig.Animator.enabled = false; }
    }
}
