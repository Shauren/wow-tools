using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGNeighborhoodMirrorData
    {
        public static readonly ObjectType ObjectType = ObjectType.NeighborhoodMirrorData;

        public static readonly UpdateField m_nameLength = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None, typeof(CGNeighborhoodMirrorData).GetField("m_name", BindingFlags.Static | BindingFlags.Public), bitSize: 8);
        public static readonly UpdateField m_ownerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_houses = new UpdateField(typeof(DynamicUpdateField<JamMirrorPlayerHouseInfo_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_managers = new UpdateField(typeof(DynamicUpdateField<JamMirrorHousingOwner_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_name = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None);
    }
}
