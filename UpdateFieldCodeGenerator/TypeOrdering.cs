namespace UpdateFieldCodeGenerator
{
    public enum CreateTypeOrder
    {
        Default,
        JamDynamicField,
        JamDynamicFieldArray,
        Bits,
        Optional
    }

    public enum UpdateTypeOrder
    {
        Default,
        Bits,
        BlzVector,
        JamDynamicField,
        JamDynamicFieldArray,
        Array
    }
}
