namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorHousingDoorData_C
    {
        public static readonly UpdateField m_roomComponentID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentOffset = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomComponentType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_attachedRoomGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
