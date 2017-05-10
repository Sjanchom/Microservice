using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IAttributeValueService : IAttributeBaseService<IAttributeValueDataTranferObject>
    {
        IEnumerable<IAttributeValueDataTranferObject> GetAllByType(int type);
        IAttributeValueDataTranferObject GetValueByType(int type, int valueId);
    }
}
