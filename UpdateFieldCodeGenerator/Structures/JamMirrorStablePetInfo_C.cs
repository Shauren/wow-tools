using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorStablePetInfo_C
    {
        public static readonly UpdateField m_petSlot = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_petNumber = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_creatureID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_displayID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_experienceLevel = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_nameSize = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorStablePetInfo_C).GetField("m_name", BindingFlags.Static | BindingFlags.Public), bitSize: 8);
        public static readonly UpdateField m_name = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 8);
        public static readonly UpdateField m_petFlags = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_specialization = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
