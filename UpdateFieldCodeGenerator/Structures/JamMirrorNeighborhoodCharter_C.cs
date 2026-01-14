using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorNeighborhoodCharter_C
    {
        public static readonly UpdateField m_field_0 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_field_4 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_signaturesSize = new UpdateField(typeof(BlzVectorField<JamMirrorNeighborhoodCharterSignature_C>), UpdateFieldFlag.None, typeof(JamMirrorNeighborhoodCharter_C).GetField("m_signatures", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_signatures = new UpdateField(typeof(BlzVectorField<JamMirrorNeighborhoodCharterSignature_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_nameLength = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None, typeof(JamMirrorNeighborhoodCharter_C).GetField("m_name", BindingFlags.Static | BindingFlags.Public), bitSize: 8);
        public static readonly UpdateField m_name = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None);
    }
}
