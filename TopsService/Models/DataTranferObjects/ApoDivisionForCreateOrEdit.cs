using TopsInterface.Entities;

namespace TopsService.Models.DataTranferObjects
{
    public class ApoDivisionForCreateOrEdit : IApoDivisionForCreateOrEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
