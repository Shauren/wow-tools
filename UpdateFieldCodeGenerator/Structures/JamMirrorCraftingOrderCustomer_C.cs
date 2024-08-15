namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorCraftingOrderCustomer_C
    {
        public static readonly UpdateField m_customerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_customerAccountGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
