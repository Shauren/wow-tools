namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerDisk_C
    {
        public static readonly UpdateField innerRadius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField innerRadiusTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField outerRadius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField outerRadiusTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField height = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField heightTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField locationZOffset = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField locationZOffsetTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
    }
}
