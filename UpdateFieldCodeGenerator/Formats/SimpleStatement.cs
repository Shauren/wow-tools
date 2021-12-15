namespace UpdateFieldCodeGenerator.Formats
{
    public class SimpleStatement : IStatement
    {
        public bool IsBlock => false;
        public string Statement { get; set; }

        public bool Equals(IStatement other)
        {
            return other is SimpleStatement statement && Statement == statement.Statement;
        }
    }
}