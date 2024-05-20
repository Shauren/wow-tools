using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorPetCreatureName_C
    {
        public static readonly UpdateField m_creatureID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_nameSize = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorPetCreatureName_C).GetField("m_name", BindingFlags.Static | BindingFlags.Public), bitSize: 8);
        public static readonly UpdateField m_name = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 8);
    }
}
