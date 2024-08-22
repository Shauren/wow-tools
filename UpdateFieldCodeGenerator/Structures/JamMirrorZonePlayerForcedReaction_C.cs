namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorZonePlayerForcedReaction_C
    {
        public static readonly UpdateField m_factionID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_reaction = new UpdateField(typeof(int), UpdateFieldFlag.None);
    }
}
