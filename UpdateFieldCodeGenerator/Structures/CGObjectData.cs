namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGObjectData
    {
        public static readonly ObjectType ObjectType = ObjectType.Object;

        public static readonly UpdateField m_entryID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_dynamicFlags = new UpdateField(typeof(uint), UpdateFieldFlag.None, customFlag: CustomUpdateFieldFlag.ViewerDependent);
        public static readonly UpdateField m_scale = new UpdateField(typeof(float), UpdateFieldFlag.None);
    }
}
