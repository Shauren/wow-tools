using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorNeighborhoodOwnershipTransfer_C
    {
        public static readonly UpdateField m_neighborhoodGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_currentOwnerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_neighborhoodNameLength = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None, typeof(JamMirrorNeighborhoodOwnershipTransfer_C).GetField("m_neighborhoodName", BindingFlags.Static | BindingFlags.Public), bitSize: 8);
        public static readonly UpdateField m_neighborhoodName = new UpdateField(typeof(DynamicString), UpdateFieldFlag.None);
    }
}
