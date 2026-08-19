using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    public class JamMirrorCraftingReagentBase_C
    {
        public static readonly UpdateField m_itemIDExists = new UpdateField(typeof(BlzOptionalField<int>), UpdateFieldFlag.None, typeof(JamMirrorCraftingReagentBase_C).GetField("m_itemID", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_currencyIDExists = new UpdateField(typeof(BlzOptionalField<int>), UpdateFieldFlag.None, typeof(JamMirrorCraftingReagentBase_C).GetField("m_currencyID", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_itemID = new UpdateField(typeof(BlzOptionalField<int>), UpdateFieldFlag.None);
        public static readonly UpdateField m_currencyID = new UpdateField(typeof(BlzOptionalField<int>), UpdateFieldFlag.None);
    }
}
