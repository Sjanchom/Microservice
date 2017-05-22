using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoSubClassForCreateOrEdit : IApoSubClassForCreateOrEdit
    {
        public int ApoClassId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
