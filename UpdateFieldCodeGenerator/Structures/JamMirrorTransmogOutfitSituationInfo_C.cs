namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorTransmogOutfitSituationInfo_C
    {
        public static readonly UpdateField situationID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField specID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField loadoutID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField equipmentSetID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
