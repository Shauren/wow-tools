namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerOrbit_C
    {
        public static readonly UpdateField m_counterClockwise = new UpdateField(typeof(bool), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_center = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_radius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_initialAngle = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_blendFromRadius = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_extraTimeForBlending = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
