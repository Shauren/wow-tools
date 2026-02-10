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
        public static readonly UpdateField m_field_10 = new UpdateField(typeof(bool), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_11 = new UpdateField(typeof(bool), UpdateFieldFlag.None);
    }
}
