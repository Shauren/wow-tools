using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorDelveData_C
    {
        public static readonly UpdateField field_0 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField field_8 = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField field_10 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_spellID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_started = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1, comment: "Restricts rewards to players in m_owners if set to true. Intended to prevent rewarwding players that join in-progress delve?");
        public static readonly UpdateField m_ownersSize = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None, typeof(JamMirrorDelveData_C).GetField("m_owners", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_owners = new UpdateField(typeof(BlzVectorField<WowGuid>), UpdateFieldFlag.None);
    }
}
