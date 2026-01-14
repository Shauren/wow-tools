namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(forceMaskMask: true)]
    public class JamMirrorQuestLog_C
    {
        public static readonly UpdateField m_questID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_stateFlags = new UpdateField(typeof(ushort), UpdateFieldFlag.None);
        public static readonly UpdateField m_endTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_objectiveFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_enabledObjectivesMask = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_objectiveProgress = new UpdateField(typeof(short[]), UpdateFieldFlag.None, 24);
    }
}
