using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorConversationActor_C
    {
        public static readonly UpdateField m_type = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
        public static readonly UpdateField m_id = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_creatureID = new UpdateField(typeof(uint), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorConversationActor_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 1")
        });
        public static readonly UpdateField m_creatureDisplayInfoID = new UpdateField(typeof(uint), UpdateFieldFlag.None, conditions: new []
        {
            new UpdateField.Condition(typeof(JamMirrorConversationActor_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 1")
        });
        public static readonly UpdateField m_actorGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorConversationActor_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 0")
        });
    }
}
