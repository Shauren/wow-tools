namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingRoomComponentMeshData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingRoomComponentMesh;

        public static readonly UpdateField m_roomGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentOptionID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_20 = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_24 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_houseThemeID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentTextureID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentTypeParam = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
