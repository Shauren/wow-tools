using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGAreaTriggerData
    {
        public static readonly ObjectType ObjectType = ObjectType.AreaTrigger;

        public static readonly UpdateField m_overrideScaleCurve = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_extraScaleCurve = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_overrideMoveCurveX = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_overrideMoveCurveY = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_overrideMoveCurveZ = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_caster = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_duration = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTarget = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTargetScale = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTargetExtraScale = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTargetPos = new UpdateField(typeof(uint), UpdateFieldFlag.None, comment: "Linked to m_overrideMoveCurve");
        public static readonly UpdateField m_spellID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellForVisuals = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellVisual = new UpdateField(typeof(JamMirrorSpellCastVisual_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_boundsRadius2D = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_decalPropertiesID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_creatingEffectGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_orbitPathTarget = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_rollPitchYaw = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_positionalSoundKitID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_movementStartTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_creationTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_ZOffset = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_targetRollPitchYawExists = new UpdateField(typeof(BlzOptionalField<Vector3>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_targetRollPitchYaw", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_targetRollPitchYaw = new UpdateField(typeof(BlzOptionalField<Vector3>), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_visualAnim = new UpdateField(typeof(JamMirrorVisualAnim_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_scaleCurveId = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_facingCurveId = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_morphCurveId = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_moveCurveId = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_facing = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_forcedPositionAndRotationExists = new UpdateField(typeof(BlzOptionalField<JamMirrorForceSetAreaTriggerPositionAndRotation_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_forcedPositionAndRotation", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_forcedPositionAndRotation = new UpdateField(typeof(BlzOptionalField<JamMirrorForceSetAreaTriggerPositionAndRotation_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_pathType = new UpdateField(typeof(int), UpdateFieldFlag.None, updateBitGroup: "m_pathData");
        public static readonly UpdateField m_shapeType = new UpdateField(typeof(byte), UpdateFieldFlag.None, updateBitGroup: "m_shapeData");

        public static readonly UpdateField m_pathData = new UpdateField(typeof(VariantUpdateField<JamMirrorAreaTriggerSplineCalculator_C, JamMirrorAreaTriggerOrbit_C, JamMirrorAreaTriggerMovementScript_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_spline = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerSplineCalculator_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_pathData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_pathType", BindingFlags.Static | BindingFlags.Public), "== 0")
        ]);
        public static readonly UpdateField m_orbit = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerOrbit_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_pathData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_pathType", BindingFlags.Static | BindingFlags.Public), "== 1")
        ]);
        public static readonly UpdateField m_movementScript = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerMovementScript_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_pathData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_pathType", BindingFlags.Static | BindingFlags.Public), "== 3")
        ]);

        public static readonly UpdateField m_shapeData = new UpdateField(typeof(VariantUpdateField<JamMirrorAreaTriggerSphere_C, JamMirrorAreaTriggerBox_C, JamMirrorAreaTriggerPolygon_C, JamMirrorAreaTriggerCylinder_C, JamMirrorAreaTriggerDisk_C, JamMirrorAreaTriggerBoundedPlane_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_sphere = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerSphere_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_shapeData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 0")
        ]);
        public static readonly UpdateField m_box = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerBox_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_shapeData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 1")
        ]);
        public static readonly UpdateField m_polygon = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerPolygon_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_shapeData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 2", "or"),
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 3", "or"),
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 5", "or"),
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 6", "or")
        ]);
        public static readonly UpdateField m_cylinder = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerCylinder_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_shapeData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 4")
        ]);
        public static readonly UpdateField m_disk = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerDisk_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_shapeData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 7")
        ]);
        public static readonly UpdateField m_boundedPlane = new UpdateField(typeof(VariantUpdateField.Case<JamMirrorAreaTriggerBoundedPlane_C>), UpdateFieldFlag.None, typeof(CGAreaTriggerData).GetField("m_shapeData", BindingFlags.Static | BindingFlags.Public), conditions:
        [
            new UpdateField.Condition(typeof(CGAreaTriggerData).GetField("m_shapeType", BindingFlags.Static | BindingFlags.Public), "== 8")
        ]);
    }
}
