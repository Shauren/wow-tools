using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorTraitSubTreeCache_C
    {
        public static readonly UpdateField m_traitSubTreeID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_entriesSize = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None, typeof(JamMirrorTraitSubTreeCache_C).GetField("m_entries", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_entries = new UpdateField(typeof(BlzVectorField<JamMirrorTraitEntry_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_active = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
    }
}
