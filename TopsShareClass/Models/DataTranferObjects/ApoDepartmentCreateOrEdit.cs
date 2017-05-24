using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoDepartmentCreateOrEdit : IApoDepartmentForCreateOrEdit
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public int ApoDivisionId { get; set; }
        public int ApoGroupId { get; set; }
    }
}
