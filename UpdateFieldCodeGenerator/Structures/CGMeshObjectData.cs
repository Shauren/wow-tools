using System.Reflection;

namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGMeshObjectData
    {
        public static readonly ObjectType ObjectType = ObjectType.MeshObject;

        public static readonly UpdateField m_isWMO = new UpdateField(typeof(bool), UpdateFieldFlag.None);
        public static readonly UpdateField m_isRoom = new UpdateField(typeof(bool), UpdateFieldFlag.None);
        public static readonly UpdateField m_fileDataID = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_geoboxExists = new UpdateField(typeof(BlzOptionalField<AaBox>), UpdateFieldFlag.None, typeof(CGMeshObjectData).GetField("m_geobox", BindingFlags.Static | BindingFlags.Public), bitSize: 1);
        public static readonly UpdateField m_geobox = new UpdateField(typeof(BlzOptionalField<AaBox>), UpdateFieldFlag.None);
    }
}
