namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGContainerData
    {
        public static readonly ObjectType ObjectType = ObjectType.Container;

        public static readonly UpdateField m_slots = new UpdateField(typeof(WowGuid[]), UpdateFieldFlag.None, 98);
        public static readonly UpdateField m_numSlots = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
