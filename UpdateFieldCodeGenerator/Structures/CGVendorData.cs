namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGVendorData
    {
        public static readonly ObjectType ObjectType = ObjectType.Vendor;

        public static readonly UpdateField m_flags = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
