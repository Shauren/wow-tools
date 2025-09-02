using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorGameObjectAssistActionData_C
    {
        public static readonly UpdateField m_playerNameLength = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorGameObjectAssistActionData_C).GetField("m_playerName", BindingFlags.Static | BindingFlags.Public), bitSize: 6);
        public static readonly UpdateField m_monsterNameLength = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None, typeof(JamMirrorGameObjectAssistActionData_C).GetField("m_monsterName", BindingFlags.Static | BindingFlags.Public), bitSize: 11);
        public static readonly UpdateField m_playerName = new UpdateField(typeof(string), UpdateFieldFlag.None);
        public static readonly UpdateField m_monsterName = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None);
        public static readonly UpdateField m_virtualRealmAddress = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_sex = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_time = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_delveTier = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
