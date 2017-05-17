using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoGroupForCreateOrUpdate : IApoGroupForCreateOrEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int ApoDivisionId { get; set; }
    }
}
