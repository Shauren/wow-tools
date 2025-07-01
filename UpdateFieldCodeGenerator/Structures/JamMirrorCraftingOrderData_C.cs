using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: 6)]
    public class JamMirrorCraftingOrderData_C
    {
        public static readonly UpdateField m_field_0 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_orderID = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_skillLineAbilityID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_orderState = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_orderType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_minQuality = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_expirationTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_claimEndTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_tipAmount = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_consortiumCut = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_crafterGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_personalCrafterGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_npcCraftingOrderSetID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_npcTreasureID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_reagents = new UpdateField(typeof(DynamicUpdateField<JamMirrorCraftingOrderItem_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_customerNotesSize = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorCraftingOrderData_C).GetField("m_customerNotes", BindingFlags.Static | BindingFlags.Public), bitSize: 10);
        public static readonly UpdateField m_customerExists = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None, typeof(JamMirrorCraftingOrderData_C).GetField("m_customer", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_npcCustomerExists = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None, typeof(JamMirrorCraftingOrderData_C).GetField("m_npcCustomer", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_outputItemExists = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None, typeof(JamMirrorCraftingOrderData_C).GetField("m_outputItem", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_outputItemDataExists = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None, typeof(JamMirrorCraftingOrderData_C).GetField("m_outputItemData", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_customerNotes = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 10);
        public static readonly UpdateField m_customer = new UpdateField(typeof(BlzOptionalField<JamMirrorCraftingOrderCustomer_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_npcCustomer = new UpdateField(typeof(BlzOptionalField<JamMirrorCraftingOrderNpcCustomer_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_outputItem = new UpdateField(typeof(BlzOptionalField<JamMirrorCraftingOrderItem_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_outputItemData = new UpdateField(typeof(BlzOptionalField<ItemInstance>), UpdateFieldFlag.None);
    }
}
