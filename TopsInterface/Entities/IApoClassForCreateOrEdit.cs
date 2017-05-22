using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopsInterface.Entities
{
    public interface IApoClassForCreateOrEdit : IApoBaseForCreateOrEdit
    {
        int ApoDepartmentId { get; set; }
    }
}
