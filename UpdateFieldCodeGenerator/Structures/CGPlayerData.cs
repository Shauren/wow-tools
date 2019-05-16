namespace UpdateFieldCodeGenerator.Structures
{
    public class CGPlayerData
    {
        public static readonly ObjectType ObjectType = ObjectType.Player;

        public static readonly UpdateField duelArbiter = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField wowAccount = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField lootTargetGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField playerFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField playerFlagsEx = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField guildRankID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField guildDeleteDate = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField guildLevel = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField skinID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField faceID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField hairStyleID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField hairColorID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField customDisplayOption = new UpdateField(typeof(byte[]), UpdateFieldFlag.None, 3);
        public static readonly UpdateField facialHairStyleID = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField partyType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField nativeSex = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField inebriation = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField pvpTitle = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField arenaFaction = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField duelTeam = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField guildTimeStamp = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField questLog = new UpdateField(typeof(JamMirrorQuestLog_C[]), UpdateFieldFlag.PartyMember, 100);
        public static readonly UpdateField visibleItems = new UpdateField(typeof(JamMirrorVisibleItem_C[]), UpdateFieldFlag.None, 19);
        public static readonly UpdateField playerTitle = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField fakeInebriation = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField virtualPlayerRealm = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField currentSpecID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField taxiMountAnimKitID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField avgItemLevel = new UpdateField(typeof(float[]), UpdateFieldFlag.None, 4);
        public static readonly UpdateField currentBattlePetBreedQuality = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField honorLevel = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField arenaCooldowns = new UpdateField(typeof(DynamicUpdateField<JamMirrorArenaCooldown_C>), UpdateFieldFlag.None);
        public static readonly UpdateField field_B0 = new UpdateField(typeof(int), UpdateFieldFlag.None); // these 2 are both quest ids and are used in condition to trigger PlayerCliQuestGiverAcceptQuest
        public static readonly UpdateField field_B4 = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
