using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IAttributeBase
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
    }
}
