namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerSplineCalculator_C
    {
        public static readonly UpdateField m_catmullrom = new UpdateField(typeof(bool), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_points = new UpdateField(typeof(DynamicUpdateField<Vector3>), UpdateFieldFlag.None, bitSize: 16);
    }
}
