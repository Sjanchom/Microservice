using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoClassForCreateOrEdit : IApoClassForCreateOrEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ApoDepartmentId { get; set; }
    }

}
