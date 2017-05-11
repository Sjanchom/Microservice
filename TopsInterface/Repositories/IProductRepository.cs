using System.Collections;
using System.Collections.Generic;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IProductRepository : IRepository<IProductDomain>
    {
        IEnumerable<IAttributeTypeAndValueDomain> GetProductAttribute(int productId,int apoClass);
    }
}
