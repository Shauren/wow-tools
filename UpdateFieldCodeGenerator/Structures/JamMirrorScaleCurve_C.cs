namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorScaleCurve_C
    {
        public static readonly UpdateField m_startTimeOffset = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_points = new UpdateField(typeof(Vector2[]), UpdateFieldFlag.None, 2);
        public static readonly UpdateField m_parameterCurve = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_overrideActive = new UpdateField(typeof(bool), UpdateFieldFlag.None);
    }
}
