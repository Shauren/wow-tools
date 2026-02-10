namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorLevelLinkInfo_C
    {
        public static readonly UpdateField m_targetGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_level = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
