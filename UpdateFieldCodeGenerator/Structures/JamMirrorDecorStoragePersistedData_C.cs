using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorDecorStoragePersistedData_C
    {
        public static readonly UpdateField m_houseGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_dyesExists = new UpdateField(typeof(BlzOptionalField<AaBox>), UpdateFieldFlag.None, typeof(JamMirrorDecorStoragePersistedData_C).GetField("m_dyeSlots", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_dyeSlots = new UpdateField(typeof(BlzOptionalField<JamMirrorDecorDyeSlots_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_sourceType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_sourceValueLength = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None, typeof(JamMirrorDecorStoragePersistedData_C).GetField("m_sourceValue", BindingFlags.Static | BindingFlags.Public), bitSize: 24);
        public static readonly UpdateField m_sourceValue = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None);
    }
}
