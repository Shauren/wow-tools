namespace UpdateFieldCodeGenerator
{
    public sealed class HasChangesMaskAttribute : Attribute
    {
        public bool ForceMaskMask { get; }

        public HasChangesMaskAttribute(bool forceMaskMask = false)
        {
            ForceMaskMask = forceMaskMask;
        }
    }
}
