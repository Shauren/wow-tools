using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorCTROptions_C
    {
        public static readonly UpdateField m_conditionalFlagsSize = new UpdateField(typeof(BlzVectorField<uint>), UpdateFieldFlag.None, typeof(JamMirrorCTROptions_C).GetField("m_conditionalFlags", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_factionGroup = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_chromieTimeExpansionMask = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_conditionalFlags = new UpdateField(typeof(BlzVectorField<uint>), UpdateFieldFlag.None);
    }
}
