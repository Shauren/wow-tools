namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(forceMaskMask: true)]
    public class CGItemData
    {
        public static readonly ObjectType ObjectType = ObjectType.Item;

        public static readonly UpdateField m_owner = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_containedIn = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_creator = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_giftCreator = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_stackCount = new UpdateField(typeof(uint), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_expiration = new UpdateField(typeof(uint), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_spellCharges = new UpdateField(typeof(int[]), UpdateFieldFlag.Owner, 5);
        public static readonly UpdateField m_dynamicFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_enchantment = new UpdateField(typeof(JamMirrorItemEnchantment_C[]), UpdateFieldFlag.None, 13);
        public static readonly UpdateField m_durability = new UpdateField(typeof(uint), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_maxDurability = new UpdateField(typeof(uint), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_createPlayedTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_context = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_createTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_artifactXP = new UpdateField(typeof(ulong), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_itemAppearanceModID = new UpdateField(typeof(byte), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_modifiers = new UpdateField(typeof(JamMirrorItemModList_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_artifactPowers = new UpdateField(typeof(DynamicUpdateField<JamMirrorArtifactPower_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_gems = new UpdateField(typeof(DynamicUpdateField<JamMirrorSocketedGem_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_zoneFlags = new UpdateField(typeof(uint), UpdateFieldFlag.Owner);
        public static readonly UpdateField m_itemBonusKey = new UpdateField(typeof(ItemBonusKey), UpdateFieldFlag.None);
        public static readonly UpdateField m_DEBUGItemLevel = new UpdateField(typeof(ushort), UpdateFieldFlag.Owner);
    }
}
