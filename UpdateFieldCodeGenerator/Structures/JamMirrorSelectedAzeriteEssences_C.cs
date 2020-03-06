namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(forceMaskMask: true)]
    public class JamMirrorSelectedAzeriteEssences_C
    {
        public static readonly UpdateField m_azeriteEssenceID = new UpdateField(typeof(uint[]), UpdateFieldFlag.None, 4);
        public static readonly UpdateField m_specializationID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_enabled = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
    }
}
