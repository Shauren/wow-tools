namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorRestInfo_C
    {
        public static readonly UpdateField m_threshold = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_stateID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
