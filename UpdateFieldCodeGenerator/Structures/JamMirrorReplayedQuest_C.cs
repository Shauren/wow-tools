namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorReplayedQuest_C
    {
        public static readonly UpdateField m_questID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_replayTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
