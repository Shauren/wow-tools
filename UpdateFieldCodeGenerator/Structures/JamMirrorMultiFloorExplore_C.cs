using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorMultiFloorExplore_C
    {
        public static readonly UpdateField m_worldMapOverlayIDsSize = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None, typeof(JamMirrorMultiFloorExplore_C).GetField("m_worldMapOverlayIDs", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_worldMapOverlayIDs = new UpdateField(typeof(BlzVectorField<int>), UpdateFieldFlag.None);
    }
}
