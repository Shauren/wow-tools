namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorChallengeModeData_C
    {
        public static readonly UpdateField m_mapID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_initialPlayerCount = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_instanceID = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_startTime = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_keystoneOwnerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_leaverGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_instanceAbandonVoteCooldown = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_isActive = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_hasRestrictions = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_canVoteAbandon = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
    }
}
