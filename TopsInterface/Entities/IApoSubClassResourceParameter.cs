using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopsInterface.Entities
{
    public interface IApoSubClassResourceParameter : IBaseResourceParameter
    {
        int? ApoClassId { get; set; }

    }
}
