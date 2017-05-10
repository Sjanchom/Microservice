using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IProductBaseDomain
    {
        int Id { get; set; }
        int ApoClass { get; set; }
        string Code { get; set; }
        string ProductName { get; set; }
    }
}
