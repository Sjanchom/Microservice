using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IProductService
    {
        PagedList<IProductBaseDomain> GetAll(int page, int pageSize, string apoClass, string searchText);
        IProductForEditBaseDomain GetById(int id);
        IProductBaseDomain Create(IProductForCreate product);
        IProductBaseDomain Edit(int proudctId, IProductForCreate product);
        bool Delete(int productId);
    }
}
