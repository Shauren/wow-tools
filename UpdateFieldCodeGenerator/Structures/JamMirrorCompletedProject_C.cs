namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorCompletedProject_C
    {
        public static readonly UpdateField m_projectID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_firstCompleted = new UpdateField(typeof(long), UpdateFieldFlag.None);
        public static readonly UpdateField m_completionCount = new UpdateField(typeof(uint), UpdateFieldFlag.None);
    }
}
