using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: 4)]
    public class JamMirrorCraftingOrderItem_C
    {
        public static readonly UpdateField m_orderItemID = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_orderItemType = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_itemGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_ownerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_reagent = new UpdateField(typeof(JamMirrorCraftingReagentBase_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_quantity = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_reagentQuality = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_dataSlotIndexExists = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None, typeof(JamMirrorCraftingOrderItem_C).GetField("m_dataSlotIndex", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_dataSlotIndex = new UpdateField(typeof(BlzOptionalField<byte>), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
