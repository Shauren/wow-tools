namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGSceneObjectData
    {
        public static readonly ObjectType ObjectType = ObjectType.SceneObject;

        public static readonly UpdateField m_scriptPackageID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_rndSeedVal = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_createdBy = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_sceneType = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
