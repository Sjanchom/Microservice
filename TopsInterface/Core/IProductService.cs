using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IProductService
    {
        PagedList<IProductDataTranferObject> GetAll(int page, int pageSize, string apoClass, string searchText);
        IProductForCreateOrEdit GetById(int id);
        IProductDataTranferObject Create(IProductForCreateOrEdit product);
        IProductDataTranferObject Edit(int proudctId, IProductForCreateOrEdit product);
        bool Delete(int productId);
    }
}
