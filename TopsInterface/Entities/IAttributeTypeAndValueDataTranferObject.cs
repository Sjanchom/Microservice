using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IAttributeTypeAndValueDataTranferObject
    {
        IAttributeTypeDataTranferObject Type { get; set; }
        IAttributeValueDataTranferObject Value { get; set; }
    }
}
