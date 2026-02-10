namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorNPCAsPlayerInfo_C
    {
        public static readonly UpdateField field_0 = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_characterLoadoutID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_creatureID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_locWorldSpace = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_facingWorldSpace = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField m_transportGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
