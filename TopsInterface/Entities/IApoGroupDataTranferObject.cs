namespace TopsInterface.Entities
{
    public interface IApoGroupDataTranferObject : IApoBase
    {
        int DivisionId { get; set; }
        string DivisionName { get; set; }
    }
}
