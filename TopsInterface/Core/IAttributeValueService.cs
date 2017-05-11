using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IAttributeValueService : IAttributeBaseService<IAttributeValueDomain>
    {
        IEnumerable<IAttributeValueDomain> GetAllByType(int type);
        IAttributeValueDomain GetValueByType(int type, int valueId);
    }
}
