using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorDelveData_C
    {
        public static readonly UpdateField m_mapID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_tier = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_instanceID = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_playersEligibleForRewardsSize = new UpdateField(typeof(BlzVectorField<WowGuid>), UpdateFieldFlag.None, typeof(JamMirrorDelveData_C).GetField("m_playersEligibleForRewards", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_activeOptionalAffixIDsSize = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None, typeof(JamMirrorDelveData_C).GetField("m_activeOptionalAffixIDs", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_entranceType = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_playersEligibleForRewards = new UpdateField(typeof(BlzVectorField<WowGuid>), UpdateFieldFlag.None);
        public static readonly UpdateField m_activeOptionalAffixIDs = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None);
        public static readonly UpdateField m_restrictingRewardPlayers = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1, comment: "Restricts rewards to players in m_owners if set to true. Intended to prevent rewarwding players that join in-progress delve?");
    }
}
