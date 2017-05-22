namespace TopsInterface.Entities
{
    public interface IApoDepartmentDataTranferObject : IApoBase
    {
        int DivisionId { get; set; }
        int GroupId { get; set; }
        string DivisionName { get; set; }
        string GroupName { get; set; }
    }
}
