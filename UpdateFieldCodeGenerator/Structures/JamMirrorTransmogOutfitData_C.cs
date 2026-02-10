namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorTransmogOutfitData_C
    {
        public static readonly UpdateField id = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField outfitInfo = new UpdateField(typeof(JamMirrorTransmogOutfitDataInfo_C), UpdateFieldFlag.None);
        public static readonly UpdateField situations = new UpdateField(typeof(DynamicUpdateField<JamMirrorTransmogOutfitSituationInfo_C>), UpdateFieldFlag.None);
        public static readonly UpdateField slots = new UpdateField(typeof(DynamicUpdateField<JamMirrorTransmogOutfitSlotData_C>), UpdateFieldFlag.None);
        public static readonly UpdateField flags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
