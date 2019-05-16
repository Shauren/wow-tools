namespace UpdateFieldCodeGenerator
{
    public abstract class DynamicUpdateField
    {
    }

    // Represents a full { blz::vector<T> values + blz::vector<uint> mask } field
    public abstract class DynamicUpdateField<T> : DynamicUpdateField
    {
    }
}
