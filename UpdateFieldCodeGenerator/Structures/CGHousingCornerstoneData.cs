namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingCornerstoneData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingCornerstone;

        public static readonly UpdateField m_cost = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_plotIndex = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
