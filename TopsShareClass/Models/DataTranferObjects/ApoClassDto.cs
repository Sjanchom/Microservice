using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{

    public class ApoClassDto : IApoClassDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
