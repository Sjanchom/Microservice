namespace TopsInterface.Entities
{
    public interface IApoDepartmentDomain : IApoBaseDomain
    {
        int GroupId { get; set; }
        int DivisionId { get; set; }
    }

}
