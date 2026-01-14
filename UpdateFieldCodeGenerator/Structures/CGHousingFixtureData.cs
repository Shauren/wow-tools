namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingFixtureData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingFixture;

        public static readonly UpdateField m_exteriorComponentID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_houseExteriorWmoDataID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_exteriorComponentHookID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_houseGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_attachParentGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_guid = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_gameObjectGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_exteriorComponentType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_59 = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_size = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
