namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGPlayerHouseInfoComponentData
    {
        public static readonly ObjectType ObjectType = ObjectType.PlayerHouseInfoComponent;

        public static readonly UpdateField m_field_8 = new UpdateField(typeof(DynamicUpdateField<WowGuid>), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_field_40 = new UpdateField(typeof(WowGuid), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_houses = new UpdateField(typeof(DynamicUpdateField<JamMirrorPlayerMirrorHouse_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_88 = new UpdateField(typeof(DynamicUpdateField<WowGuid>), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_field_C0 = new UpdateField(typeof(DynamicUpdateField<WowGuid>), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_field_F8 = new UpdateField(typeof(DynamicUpdateField<WowGuid>), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_charter = new UpdateField(typeof(JamMirrorNeighborhoodCharter_C), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_field_178 = new UpdateField(typeof(byte), UpdateFieldFlag.Owner);
    }
}
