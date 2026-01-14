namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingPlayerHouseData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingPlayerHouse;

        public static readonly UpdateField m_bnetAccount = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_plotIndex = new UpdateField(typeof(int), UpdateFieldFlag.None);
        public static readonly UpdateField m_level = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_favor = new UpdateField(typeof(ulong), UpdateFieldFlag.None);
        public static readonly UpdateField m_interiorDecorPlacementBudget = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_exteriorDecorPlacementBudget = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_exteriorFixtureBudget = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_roomPlacementBudget = new UpdateField(typeof(uint), UpdateFieldFlag.None);
        public static readonly UpdateField m_entityGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
