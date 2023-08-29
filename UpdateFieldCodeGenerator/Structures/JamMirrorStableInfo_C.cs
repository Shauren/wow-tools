namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorStableInfo_C
    {
        public static readonly UpdateField m_pets = new UpdateField(typeof(DynamicUpdateField<JamMirrorStablePetInfo_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_stableMaster = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
