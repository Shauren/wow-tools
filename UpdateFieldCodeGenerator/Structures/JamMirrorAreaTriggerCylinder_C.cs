namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerCylinder_C
    {
        public static readonly UpdateField radius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField radiusTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField height = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField heightTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField locationZOffset = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField locationZOffsetTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
    }
}
