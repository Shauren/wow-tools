using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorNICompletedInitiativesEntry_C
    {
        public static readonly UpdateField m_initiativeID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_completedMilestonesSize = new UpdateField(typeof(BlzVectorField<JamMirrorConversationLine_C>), UpdateFieldFlag.None, typeof(JamMirrorNICompletedInitiativesEntry_C).GetField("m_completedMilestones", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_completedMilestones = new UpdateField(typeof(BlzVectorField<JamMirrorNICompletedMilestoneEntry_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_completed = new UpdateField(typeof(bool), UpdateFieldFlag.None);
    }
}
