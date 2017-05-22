using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopsInterface.Entities;

namespace TopsShareClass.Models.DataTranferObjects
{
    public class ApoSubClassDto : IApoSubClassDataTranferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int IsActive { get; set; }
        public int ApoClassId { get; set; }
        public string ApoClassName { get; set; }
    }
}
