using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IProductRepository : IRepository<IProductDomain>
    {
        IQueryable<IProductDomain> GetAll(IProductResourceParameters resourceParameters);
        IEnumerable<IAttributeTypeAndValueDomain> GetProductAttribute(int productId,string apoClass);
    }
}
