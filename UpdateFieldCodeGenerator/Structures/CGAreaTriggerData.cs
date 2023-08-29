namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGAreaTriggerData
    {
        public static readonly ObjectType ObjectType = ObjectType.AreaTrigger;

        public static readonly UpdateField m_overrideScaleCurve = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_extraScaleCurve = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_heightIgnoresScale = new UpdateField(typeof(bool), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_field_261 = new UpdateField(typeof(bool), UpdateFieldFlag.None, bitSize: 1);
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
        public static readonly UpdateField m_numUnitsInside = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_numPlayersInside = new UpdateField(typeof(uint), UpdateFieldFlag.None, comment: "When not 0 this causes SpellVisualEvent 14 to trigger, playing alternate visuals, typically used by \"SOAK THIS\" areatriggers");
        public static readonly UpdateField m_orbitPathTarget = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_rollPitchYaw = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_positionalSoundKitID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_visualAnim = new UpdateField(typeof(JamMirrorVisualAnim_C), UpdateFieldFlag.None);
    }
}
