namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorConversationActor_C
    {
        public static readonly UpdateField m_creatureID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_creatureDisplayInfoID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_actorGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField field_18 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_type = new UpdateField(typeof(Bits), UpdateFieldFlag.None, bitSize: 1);
    }
}
