using System;

namespace UpdateFieldCodeGenerator.Formats
{
    public interface IStatement : IEquatable<IStatement>
    {
        public bool IsBlock { get; }
        public string Statement { get; }
    }
}