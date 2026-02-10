namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorCraftingOrderNpcCustomer_C
    {
        public static readonly UpdateField m_npcCraftingOrderCustomerID = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_realmAddress = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
