namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorTransmogOutfitMetadata_C
    {
        public static readonly UpdateField situationTrigger = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField transmogOutfitID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField locked = new UpdateField(typeof(bool), UpdateFieldFlag.None);
        public static readonly UpdateField stampedOptionMainHand = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField stampedOptionOffHand = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
