namespace TopsInterface.Entities
{
    public interface IApoDepartmentForCreateOrEdit : IApoBaseForCreateOrEdit
    {
        int ApoDivisionId { get; set; }
        int ApoGroupId { get; set; }
    }
}
