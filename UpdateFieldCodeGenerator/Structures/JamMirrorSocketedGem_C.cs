namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorSocketedGem_C
    {
        public static readonly UpdateField m_itemID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_bonusListIDs = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 16);
        public static readonly UpdateField m_context = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
