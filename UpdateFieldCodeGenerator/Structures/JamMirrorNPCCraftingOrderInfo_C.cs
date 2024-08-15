namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorNPCCraftingOrderInfo_C
    {
        public static readonly UpdateField m_orderID = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_npcCraftingOrderSetID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_npcTreasureID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_npcCraftingOrderCustomerID = new UpdateField(typeof(int), UpdateFieldFlag.None);

    }
}
