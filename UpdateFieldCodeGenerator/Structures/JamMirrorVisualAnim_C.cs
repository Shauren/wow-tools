namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorVisualAnim_C
    {
        public static readonly UpdateField m_animationDataID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_animKitID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_animProgress = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField field_C = new UpdateField(typeof(bool), UpdateFieldFlag.None);
    }
}
