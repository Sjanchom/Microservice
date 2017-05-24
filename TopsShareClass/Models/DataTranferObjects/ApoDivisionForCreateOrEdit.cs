using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoDivisionForCreateOrEdit : IApoDivisionForCreateOrEdit
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
