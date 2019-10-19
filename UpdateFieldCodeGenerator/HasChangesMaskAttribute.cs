using System;

namespace UpdateFieldCodeGenerator
{
    public class HasChangesMaskAttribute : Attribute
    {
        public bool ForceMaskMask { get; }

        public HasChangesMaskAttribute(bool forceMaskMask = false)
        {
            ForceMaskMask = forceMaskMask;
        }
    }
}
