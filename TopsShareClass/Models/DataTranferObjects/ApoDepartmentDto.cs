using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoDepartmentDto : IApoDepartmentDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public int DivisionId { get; set; }
        public int GroupId { get; set; }
        public string DivisionName { get; set; }
        public string GroupName { get; set; }
    }
}
