namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorForceSetAreaTriggerPositionAndRotation_C
    {
        public static readonly UpdateField triggerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField pos = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField rotation = new UpdateField(typeof(Quaternion), UpdateFieldFlag.None);
    }
}
