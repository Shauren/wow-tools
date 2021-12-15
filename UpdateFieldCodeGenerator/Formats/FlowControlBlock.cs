using System.Collections.Generic;

namespace UpdateFieldCodeGenerator.Formats
{
    public class FlowControlBlock : IStatement
    {
        public bool IsBlock => true;
        public string Statement { get; set; }

        public static bool AreChainsAlmostEqual(IReadOnlyList<IStatement> oldFlow, IReadOnlyList<IStatement> newFlow)
        {
            if (newFlow.Count == 0 || oldFlow == null)
                return false;

            var o = 0;
            var n = 0;
            do
            {
                if (o >= oldFlow.Count)
                    break;

                if (oldFlow[o].Equals(newFlow[n]))
                    ++n;

                ++o;
            } while (n < newFlow.Count);

            return n >= newFlow.Count;
        }

        public bool Equals(IStatement other)
        {
            if (other is FlowControlBlock block)
                return block.Statement == Statement;
            return false;
        }

        public override string ToString() => Statement;
    }
}
