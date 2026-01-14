namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorMirroredMeshObjectData_C
    {
        public static readonly UpdateField attachParentGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField positionLocalSpace = new UpdateField(typeof(Vector3), UpdateFieldFlag.None);
        public static readonly UpdateField rotationLocalSpace = new UpdateField(typeof(Quaternion), UpdateFieldFlag.None);
        public static readonly UpdateField scaleLocalSpace = new UpdateField(typeof(float), UpdateFieldFlag.None);
        public static readonly UpdateField attachmentFlags = new UpdateField(typeof(byte), UpdateFieldFlag.None);
    }
}
