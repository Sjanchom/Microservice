using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IAttributeValueDataTranferObject : IAttributeBase
    {
        int TypeId { get; set; }
    }
}
