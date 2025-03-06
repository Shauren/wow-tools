namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorCTROptions_C
    {
        public static readonly UpdateField m_conditionalFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_factionGroup = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_chromieTimeExpansionMask = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
