namespace TopsInterface.Entities
{
    public interface IApoClassDataTranferObject : IApoBase
    {
        int DepartmentId { get; set; }
        string DepartmentName { get; set; }
    }
}
