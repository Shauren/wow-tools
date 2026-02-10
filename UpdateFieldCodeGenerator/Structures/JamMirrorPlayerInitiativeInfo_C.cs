namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorPlayerInitiativeInfo_C
    {
        public static readonly UpdateField m_remainingDuration = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_currentInitiativeID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_currentMilestoneID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_currentCycleID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_progressRequired = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_currentProgress = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_playerTotalContribution = new UpdateField(typeof(float), UpdateFieldFlag.None);
    }
}
