using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IProductResourceParameters : IBaseResourceParameter
    {
        string ApoClass { get; set; }
    }
}
