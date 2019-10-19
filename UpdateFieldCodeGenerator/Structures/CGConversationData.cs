using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGConversationData
    {
        public static readonly ObjectType ObjectType = ObjectType.Conversation;

        public static readonly UpdateField m_linesSize = new UpdateField(typeof(BlzVectorField<JamMirrorConversationLine_C>), UpdateFieldFlag.None, typeof(CGConversationData).GetField("m_lines", BindingFlags.Static | BindingFlags.Public));
        public static readonly UpdateField m_lastLineEndTime = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField field_1C = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_lines = new UpdateField(typeof(BlzVectorField<JamMirrorConversationLine_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_actors = new UpdateField(typeof(DynamicUpdateField<JamMirrorConversationActor_C>), UpdateFieldFlag.None);
    }
}
