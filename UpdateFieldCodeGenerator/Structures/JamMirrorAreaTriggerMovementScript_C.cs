namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorAreaTriggerMovementScript_C
    {
        public static readonly UpdateField m_spellScriptID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_center = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField m_creationTime = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
