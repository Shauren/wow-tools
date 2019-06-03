namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorQuestLog_C
    {
        public static readonly UpdateField m_questID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_stateFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_endTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_acceptTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_objectiveProgress = new UpdateField(typeof(short[]), UpdateFieldFlag.None, 24);
    }
}
