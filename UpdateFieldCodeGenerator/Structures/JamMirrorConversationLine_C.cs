namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorConversationLine_C
    {
        public static readonly UpdateField m_conversationLineID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_broadcastTextID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_startTime = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_uiCameraID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_actorIndex = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_flags = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField m_chatType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
