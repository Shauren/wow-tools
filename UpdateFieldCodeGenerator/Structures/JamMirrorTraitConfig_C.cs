using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: 4)]
    public class JamMirrorTraitConfig_C
    {
        public static readonly UpdateField m_ID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_nameSize = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorTraitConfig_C).GetField("m_name", BindingFlags.Static | BindingFlags.Public), bitSize: 9);
        public static readonly UpdateField m_name = new UpdateField(typeof(string), UpdateFieldFlag.None, bitSize: 9);
        public static readonly UpdateField m_type = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_entries = new UpdateField(typeof(DynamicUpdateField<JamMirrorTraitEntry_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_subTrees = new UpdateField(typeof(DynamicUpdateField<JamMirrorTraitSubTreeCache_C>), UpdateFieldFlag.None);
        public static readonly UpdateField m_skillLineID = new UpdateField(typeof(int), UpdateFieldFlag.None, conditions: new []
        {
            new UpdateField.Condition(typeof(JamMirrorTraitConfig_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 2")
        });
        public static readonly UpdateField m_chrSpecializationID = new UpdateField(typeof(int), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorTraitConfig_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 1")
        });
        public static readonly UpdateField m_combatConfigFlags = new UpdateField(typeof(int), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorTraitConfig_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 1")
        });
        public static readonly UpdateField m_localIdentifier = new UpdateField(typeof(int), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorTraitConfig_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 1")
        });
        public static readonly UpdateField m_traitSystemID = new UpdateField(typeof(int), UpdateFieldFlag.None, conditions: new[]
        {
            new UpdateField.Condition(typeof(JamMirrorTraitConfig_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 3")
        });
        public static readonly UpdateField m_variationID = new UpdateField(typeof(int), UpdateFieldFlag.None, conditions:
        [
            new UpdateField.Condition(typeof(JamMirrorTraitConfig_C).GetField("m_type", BindingFlags.Static | BindingFlags.Public), "== 3")
        ]);
    }
}
