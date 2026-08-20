namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorCraftingOrder_C
    {
        public static readonly UpdateField m_data = new UpdateField(typeof(JamMirrorCraftingOrderData_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_recraftItemInfo = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None);
        public static readonly UpdateField m_enchantments = new UpdateField(typeof(DynamicUpdateField<ItemEnchantData>), UpdateFieldFlag.None, bitSize: 4);
        public static readonly UpdateField m_gems = new UpdateField(typeof(DynamicUpdateField<ItemGemData>), UpdateFieldFlag.None, bitSize: 2);
    }
}
