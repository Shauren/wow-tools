namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorWalkInData_C
    {
        public static readonly UpdateField m_mapID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_instanceID = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_walkInInstanceType = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_walkInPartyGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
