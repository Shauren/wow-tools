using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingDecorData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingDecor;

        public static readonly UpdateField m_decorGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_attachParentGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_persistedDataExists = new UpdateField(typeof(BlzOptionalField<JamMirrorDecorStoragePersistedData_C>), UpdateFieldFlag.None, typeof(CGHousingDecorData).GetField("m_persistedData", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_persistedData = new UpdateField(typeof(BlzOptionalField<JamMirrorDecorStoragePersistedData_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_targetGameObjectGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
