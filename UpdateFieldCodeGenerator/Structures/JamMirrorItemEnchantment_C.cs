namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorItemEnchantment_C
    {
        public static readonly UpdateField m_ID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_duration = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_charges = new UpdateField(typeof(short), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_A = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_B = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
