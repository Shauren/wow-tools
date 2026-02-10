namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorPlayerInitiativeTaskInfo_C
    {
        public static readonly UpdateField m_taskID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_timesCompleted = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
