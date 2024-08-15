using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorPlayerDataElement_C
    {
        public static readonly UpdateField m_type = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_floatValue = new UpdateField(typeof(float), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorPlayerDataElement_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 1")
        });
        public static readonly UpdateField m_int64Value = new UpdateField(typeof(long), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorPlayerDataElement_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 0")
        });
    }
}
