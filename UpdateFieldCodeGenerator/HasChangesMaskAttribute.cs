namespace UpdateFieldCodeGenerator
{
    public sealed class HasChangesMaskAttribute : Attribute
    {
        public bool ForceMaskMask { get; }
        public int BlockGroupSize { get; }

        public HasChangesMaskAttribute(bool forceMaskMask = false, int blockGroupSize = 32)
        {
            ForceMaskMask = forceMaskMask;
            BlockGroupSize = blockGroupSize;
        }
    }
}
