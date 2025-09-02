namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerSphere_C
    {
        public static readonly UpdateField radius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField radiusTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
    }
}
