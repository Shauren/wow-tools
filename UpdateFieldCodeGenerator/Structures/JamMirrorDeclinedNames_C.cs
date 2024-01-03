using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(forceMaskMask: true)]
    public class JamMirrorDeclinedNames_C
    {
        public static readonly UpdateField m_nameLength = new UpdateField(typeof(string[]), UpdateFieldFlag.None, typeof(JamMirrorDeclinedNames_C).GetField("m_name", BindingFlags.Static | BindingFlags.Public), 5, bitSize: 10);
        public static readonly UpdateField m_name = new UpdateField(typeof(string[]), UpdateFieldFlag.None, 5, bitSize: 10);
    }
}
