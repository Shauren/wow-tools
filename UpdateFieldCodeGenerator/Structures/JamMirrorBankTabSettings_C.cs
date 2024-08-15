using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorBankTabSettings_C
    {
        public static readonly UpdateField m_nameLength = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorBankTabSettings_C).GetField("m_name", BindingFlags.Static | BindingFlags.Public), bitSize: 7);
        public static readonly UpdateField m_iconLength = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorBankTabSettings_C).GetField("m_icon", BindingFlags.Static | BindingFlags.Public), bitSize: 9);
        public static readonly UpdateField m_descriptionLength = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorBankTabSettings_C).GetField("m_description", BindingFlags.Static | BindingFlags.Public), bitSize: 14);
        public static readonly UpdateField m_name = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 7);
        public static readonly UpdateField m_icon = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 9);
        public static readonly UpdateField m_description = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 14);
        public static readonly UpdateField m_depositFlags = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
