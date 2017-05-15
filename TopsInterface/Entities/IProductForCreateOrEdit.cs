using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IProductForCreateOrEdit : IProductBase
    {
        string ProductDescription { get; set; }
        IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects { get; set; }
    }
}
