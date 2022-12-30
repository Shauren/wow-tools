namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGAreaTriggerData
    {
        public static readonly ObjectType ObjectType = ObjectType.AreaTrigger;

        public static readonly UpdateField m_overrideScaleCurve = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_extraScaleCurve = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_C38 = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_C54 = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_C70 = new UpdateField(typeof(JamMirrorScaleCurve_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_caster = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_duration = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTarget = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTargetScale = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_timeToTargetExtraScale = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField field_B0 = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellForVisuals = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellVisual = new UpdateField(typeof(JamMirrorSpellCastVisual_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_boundsRadius2D = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_decalPropertiesID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_creatingEffectGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField field_80 = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField field_84 = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField field_88 = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField field_F8 = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_visualAnim = new UpdateField(typeof(JamMirrorVisualAnim_C), UpdateFieldFlag.None);
    }
}
