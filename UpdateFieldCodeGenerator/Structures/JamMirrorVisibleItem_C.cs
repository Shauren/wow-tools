namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    [HasMutableInterface]
    public class JamMirrorVisibleItem_C
    {
        public static readonly UpdateField m_itemID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_secondaryItemModifiedAppearanceID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_conditionalItemAppearanceID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_itemAppearanceModID = new UpdateField(typeof(ushort), UpdateFieldFlag.None);
        public static readonly UpdateField m_itemVisual = new UpdateField(typeof(ushort), UpdateFieldFlag.None);
        public static readonly UpdateField m_hasTransmog = new UpdateField(typeof(bool), UpdateFieldFlag.None);
        public static readonly UpdateField m_hasIllusion = new UpdateField(typeof(bool), UpdateFieldFlag.None);
        public static readonly UpdateField m_itemModifiedAppearanceID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_transmogSlotOption = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_sheatheCategory = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
