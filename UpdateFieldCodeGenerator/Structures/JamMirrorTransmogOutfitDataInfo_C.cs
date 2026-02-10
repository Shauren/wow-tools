using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask(blockGroupSize: -1)]
    public class JamMirrorTransmogOutfitDataInfo_C
    {
        public static readonly UpdateField setType = new UpdateField(typeof(byte), UpdateFieldFlag.None);
        public static readonly UpdateField nameLength = new UpdateField(typeof(string), UpdateFieldFlag.None, typeof(JamMirrorTransmogOutfitDataInfo_C).GetField("name", BindingFlags.Static | BindingFlags.Public), bitSize: 8);
        public static readonly UpdateField name = new UpdateField(typeof(string), UpdateFieldFlag.None);
        public static readonly UpdateField icon = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField situationsEnabled = new UpdateField(typeof(bool), UpdateFieldFlag.None);
    }
}
