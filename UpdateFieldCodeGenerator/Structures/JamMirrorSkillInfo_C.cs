namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorSkillInfo_C
    {
        public static readonly UpdateField m_skillLineID = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 256);
        public static readonly UpdateField m_skillStep = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 256);
        public static readonly UpdateField m_skillRank = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 256);
        public static readonly UpdateField m_skillStartingRank = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 256);
        public static readonly UpdateField m_skillMaxRank = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 256);
        public static readonly UpdateField m_skillTempBonus = new UpdateField(typeof(short[]), UpdateFieldFlag.None, 256);
        public static readonly UpdateField m_skillPermBonus = new UpdateField(typeof(ushort[]), UpdateFieldFlag.None, 256);
    }
}
