using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IProductService
    {
        PagedList<IProductBaseDomain> GetAll(int page, int pageSize, int apoClass, string searchText);
        IProductForEditBaseDomain GetById(int id);
        IProductForEditBaseDomain Create(IProductForCreate product);
        IProductForEditBaseDomain Edit(int proudctId, IProductForCreate product);
        bool Delete(int productId);
    }
}
