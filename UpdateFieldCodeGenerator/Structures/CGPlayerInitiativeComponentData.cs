namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGPlayerInitiativeComponentData
    {
        public static readonly ObjectType ObjectType = ObjectType.PlayerInitiativeComponent;

        public static readonly UpdateField m_neighborhoodGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_initiativeInfo = new UpdateField(typeof(JamMirrorPlayerInitiativeInfo_C), UpdateFieldFlag.None);
        public static readonly UpdateField m_completedTasks = new UpdateField(typeof(DynamicUpdateField<JamMirrorPlayerInitiativeTaskInfo_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_completedInitiatives = new UpdateField(typeof(DynamicUpdateField<JamMirrorNICompletedInitiativesEntry_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_houses = new UpdateField(typeof(SetUpdateField<WowGuid>), UpdateFieldFlag.Owner);
    }
}
