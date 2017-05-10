using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{

    public interface IProductDomain : IProductBaseDomain
    {
        string ProductDescription { get; set; }
    }
}
