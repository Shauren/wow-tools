namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingRoomData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingRoom;

        public static readonly UpdateField m_houseGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_houseRoomID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_meshObjects = new UpdateField(typeof(DynamicUpdateField<WowGuid>), UpdateFieldFlag.None);
        public static readonly UpdateField m_doors = new UpdateField(typeof(DynamicUpdateField<JamMirrorHousingDoorData_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_floorIndex = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
