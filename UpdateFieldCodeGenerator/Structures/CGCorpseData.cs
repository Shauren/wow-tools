namespace UpdateFieldCodeGenerator.Structures
{
    public class CGCorpseData
    {
        public static readonly ObjectType ObjectType = ObjectType.Corpse;

        public static readonly UpdateField m_dynamicFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_owner = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_partyGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_guildGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_displayID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_items = new UpdateField(typeof(uint[]), UpdateFieldFlag.None, 19);
        public static readonly UpdateField m_unused = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_raceID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_sex = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_skinID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_faceID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_hairStyleID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_hairColorID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_facialHairStyleID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_factionTemplate = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_customDisplayOption = new UpdateField(typeof(byte[]), UpdateFieldFlag.None, 3);
    }
}
