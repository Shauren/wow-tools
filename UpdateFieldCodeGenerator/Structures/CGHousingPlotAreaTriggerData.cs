namespace UpdateFieldCodeGenerator.Structures
{
    [HasChangesMask]
    public class CGHousingPlotAreaTriggerData
    {
        public static readonly ObjectType ObjectType = ObjectType.HousingPlotAreaTrigger;

        public static readonly UpdateField m_plotID = new UpdateField(typeof(uint), UpdateFieldFlag.None, comment: "PlotIndex, not id from NeighborhoodPlot.db2");
        public static readonly UpdateField m_houseOwnerGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_houseGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
        public static readonly UpdateField m_houseOwnerBnetAccountGUID = new UpdateField(typeof(WowGuid), UpdateFieldFlag.None);
    }
}
