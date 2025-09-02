namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorLeaverInfo_C
    {
        public static readonly UpdateField m_bnetAccountGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_leaveScore = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_seasonID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_totalLeaves = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_totalSuccesses = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_consecutiveSuccesses = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_lastPenaltyTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_leaverExpirationTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_unknown_1120 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_leaverStatus = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
    }
}
