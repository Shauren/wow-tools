namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerPolygon_C
    {
        public static readonly UpdateField vertices = new UpdateField(typeof(DynamicUpdateField<Vector2>), UpdateFieldFlag.None);
        public static readonly UpdateField verticesTarget = new UpdateField(typeof(DynamicUpdateField<Vector2>), UpdateFieldFlag.None);
        public static readonly UpdateField height = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField heightTarget = new UpdateField(typeof(float), UpdateFieldFlag.None);
    }
}
