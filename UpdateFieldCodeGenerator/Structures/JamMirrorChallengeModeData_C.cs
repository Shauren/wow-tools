namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorChallengeModeData_C
    {
        public static readonly UpdateField m_unknown_1120_1 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_unknown_1120_2 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_unknown_1120_3 = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_unknown_1120_4 = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_keystoneOwnerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_leaverGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_instanceAbandonVoteCooldown = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_isActive = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_hasRestrictions = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_canVoteAbandon = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
    }
}
