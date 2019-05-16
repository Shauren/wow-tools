namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorArenaCooldown_C
    {
        public static readonly UpdateField m_spellID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_charges = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_startTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_endTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_nextChargeTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_maxCharges = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
