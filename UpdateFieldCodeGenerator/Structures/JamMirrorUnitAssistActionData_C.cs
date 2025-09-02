using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorUnitAssistActionData_C
    {
        public static readonly UpdateField m_type = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_playerNameLength = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorUnitAssistActionData_C).GetField("m_playerName", BindingFlags.Static | BindingFlags.Public), bitSize: 6);
        public static readonly UpdateField m_playerName = new UpdateField(typeof(string), UpdateFieldFlag.None);
        public static readonly UpdateField m_virtualRealmAddress = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
