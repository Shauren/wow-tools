namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorPVPInfo_C
    {
        public static readonly UpdateField m_bracket = new UpdateField(typeof(sbyte), UpdateFieldFlag.None);
        public static readonly UpdateField m_pvpRatingID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_weeklyPlayed = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_weeklyWon = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_seasonPlayed = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_seasonWon = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_rating = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_weeklyBestRating = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_seasonBestRating = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_pvpTierID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_weeklyBestWinPvpTierID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_disqualified = new UpdateField(typeof(bool), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField field_28 = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField field_2C = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_weeklyRoundsPlayed = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_weeklyRoundsWon = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_seasonRoundsPlayed = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_seasonRoundsWon = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
