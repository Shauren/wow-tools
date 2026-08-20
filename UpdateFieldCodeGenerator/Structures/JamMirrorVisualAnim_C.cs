namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class JamMirrorVisualAnim_C
    {
        public static readonly UpdateField m_animationDataID = new UpdateField(typeof(BlzOptionalField<short>), UpdateFieldFlag.None);
        public static readonly UpdateField m_animKitID = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_animProgress = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_isDecay = new UpdateField(typeof(bool), UpdateFieldFlag.None);

        public static readonly string[] WppCompatibilityDefinitions =
        [
            "uint? IVisualAnim.AnimationDataID => AnimationDataID.HasValue ? (uint)AnimationDataID.Value : uint.MaxValue;"
        ];
    }
}
