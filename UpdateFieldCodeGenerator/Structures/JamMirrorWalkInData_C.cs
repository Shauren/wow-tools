namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorWalkInData_C
    {
        public static readonly UpdateField m_mapID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField field_8 = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_type = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField field_18 = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
