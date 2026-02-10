namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorTransmogOutfitSlotData_C
    {
        public static readonly UpdateField slot = new UpdateField(typeof(sbyte), UpdateFieldFlag.None);
        public static readonly UpdateField slotOption = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField itemModifiedAppearanceID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField appearanceDisplayType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField spellItemEnchantmentID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField illusionDisplayType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
