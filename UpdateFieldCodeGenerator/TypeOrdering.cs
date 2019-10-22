namespace UpdateFieldCodeGenerator
{
    public enum CreateTypeOrder
    {
        Default,
        JamDynamicField,
        JamDynamicFieldArray,
        ArrayWithBits,
        Bits,
        JamDynamicFieldWithBits,
        Optional
    }

    public enum UpdateTypeOrder
    {
        Default,
        Bits,
        BlzVector,
        JamDynamicField,
        JamDynamicFieldArray,
        Array,
        Optional
    }
}
