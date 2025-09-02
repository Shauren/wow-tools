namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorUnitChannel_C
    {
        public static readonly UpdateField m_spellID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellVisual = new UpdateField(typeof(JamMirrorSpellCastVisual_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_startTimeMs = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_duration = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
