namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingStorageData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingStorage;

        public static readonly UpdateField m_decor = new UpdateField(typeof(MapUpdateField<WowGuid, JamMirrorDecorStoragePersistedData_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_decorMaxOwnedCount = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
