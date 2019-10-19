namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(forceMaskMask: true)]
    public class CGAzeriteEmpoweredItemData
    {
        public static readonly ObjectType ObjectType = ObjectType.AzeriteEmpoweredItem;

        public static readonly UpdateField m_selections = new UpdateField(typeof(int[]), UpdateFieldFlag.None, 5);
    }
}
