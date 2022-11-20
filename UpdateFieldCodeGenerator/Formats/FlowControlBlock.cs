namespace UpdateFieldCodeGenerator.Formats
{
    public class FlowControlBlock
    {
        public string Statement { get; set; }

        public static bool AreChainsAlmostEqual(IReadOnlyList<FlowControlBlock> oldFlow, IReadOnlyList<FlowControlBlock> newFlow)
        {
            if (newFlow.Count == 0 || oldFlow == null)
                return false;

            var o = 0;
            var n = 0;
            do
            {
                if (o >= oldFlow.Count)
                    break;

                if (oldFlow[o].Statement == newFlow[n].Statement)
                    ++n;

                ++o;
            } while (n < newFlow.Count);

            return n >= newFlow.Count;
        }

        public override string ToString() => Statement;
    }
}
