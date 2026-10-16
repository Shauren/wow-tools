namespace UpdateFieldCodeGenerator
{
    public sealed class HasChangesMaskAttribute : Attribute
    {
        public bool ForceMaskMask { get; }
        public int BlockGroupSize { get; }
        public UpdateFieldFlag DynamicChangesMaskFlags { get; }

        public HasChangesMaskAttribute(bool forceMaskMask = false, int blockGroupSize = 32, UpdateFieldFlag dynamicChangesMaskFlags = UpdateFieldFlag.None)
        {
            ForceMaskMask = forceMaskMask;
            BlockGroupSize = blockGroupSize;
            DynamicChangesMaskFlags = dynamicChangesMaskFlags;
        }
    }
}
