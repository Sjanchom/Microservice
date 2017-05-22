namespace TopsInterface.Entities
{
    public interface IApoDepartmentResourceParameter : IBaseResourceParameter
    {
        int? ApoDivisionId { get; set; }
        int? ApoGroupId { get; set; }
    }
}
