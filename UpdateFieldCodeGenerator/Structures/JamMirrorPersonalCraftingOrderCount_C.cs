namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorPersonalCraftingOrderCount_C
    {
        public static readonly UpdateField m_professionID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_count = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
