namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGMirroredPositionData
    {
        public static readonly ObjectType ObjectType = ObjectType.MirroredPositionData;

        public static readonly UpdateField m_positionData = new UpdateField(typeof(JamMirrorMirroredMeshObjectData_C), UpdateFieldFlag.None);
    }
}
