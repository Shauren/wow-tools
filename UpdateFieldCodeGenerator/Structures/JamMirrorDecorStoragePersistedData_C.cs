using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorDecorStoragePersistedData_C
    {
        public static readonly UpdateField m_houseGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_dyesExists = new UpdateField(typeof(BlzOptionalField<AaBox>), UpdateFieldFlag.None, typeof(JamMirrorDecorStoragePersistedData_C).GetField("m_dyes", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_dyes = new UpdateField(typeof(BlzOptionalField<JamMirrorDecorStoragePersistedDataDyes_C>), UpdateFieldFlag.None);
        public static readonly UpdateField field_20 = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
