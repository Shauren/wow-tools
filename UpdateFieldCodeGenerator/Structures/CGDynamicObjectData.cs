namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGDynamicObjectData
    {
        public static readonly ObjectType ObjectType = ObjectType.DynamicObject;

        public static readonly UpdateField m_caster = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_type = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellXSpellVisualID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_radius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_castTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    };
}
