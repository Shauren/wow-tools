using System;

namespace UpdateFieldCodeGenerator
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HasMutableInterfaceAttribute : Attribute
    {
    }
}
