using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoDivisionDto : IApoDivisionDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
    }
}
