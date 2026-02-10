namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorPlayerMirrorHouse_C
    {
        public static readonly UpdateField m_guid = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_neighborhoodGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_level = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_favor = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_initiativeFavor = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_initiativeCycleID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_mapID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_plotID = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
