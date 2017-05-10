using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface
{
    public interface IProductService
    {
        PagedList<IProductDataTranferObject> GetAll(int page,int pageSize,int apoClass,string searchText);
        IProductForEditDataTranferObject GetById(int id);
        IProductForEditDataTranferObject Create(IProductForCreate product);
        IProductForEditDataTranferObject Edit(int proudctId, IProductForCreate product);
        bool Delete(int productId);
    }

    public interface IAttributeBaseService<T> 
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Edit(T item);
        bool Delete(int id);
    }

    public interface IAttributeTypeService : IAttributeBaseService<IAttributeTypeDataTranferObject>
    {
    }

    public interface IAttributeValueService : IAttributeBaseService<IAttributeValueDataTranferObject>
    {
        IEnumerable<IAttributeValueDataTranferObject> GetAllByType(int type);
        IAttributeValueDataTranferObject GetValueByType(int type, int valueId);
    }

    public interface IApoBaseService<T>
    {
        
    }

    public class PagedList<T> : List<T>
    {
        
            public int CurrentPage { get; private set; }
            public int TotalPages { get; private set; }
            public int PageSize { get; private set; }
            public int TotalCount { get; private set; }

            public bool HasPrevious => (CurrentPage > 1);

        public bool HasNext => (CurrentPage < TotalPages);

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
            {
                TotalCount = count;
                PageSize = pageSize;
                CurrentPage = pageNumber;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                AddRange(items);
            }

            public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
            {
                var count = source.Count();
                var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }

    }


    public interface IProductForCreate: IProductForEditDataTranferObject
    {

    }

    public interface IProductForEditDataTranferObject : IProductDataTranferObject
    {
        string ProductDescription { get; set; }
        IBrand Brand { get; set; }
        IEnumerable<IAttributeTypeAndValueDataTranferObject> ListAttributeTypeAndValueDataTranferObjects { get; set; }
    }

    public interface IAttributeTypeAndValueDataTranferObject
    {
        IAttributeTypeDataTranferObject Type { get; set; }
        IAttributeValueDataTranferObject Value { get; set; }
    }

    public interface IAttributeBase
    {
        int Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
    }

    public interface IAttributeValueDataTranferObject
    {
        int TypeId { get; set; }
    }

    public interface IAttributeTypeDataTranferObject : IAttributeBase
    {
    }

    public interface IBrand
    {
        int Id { get; set; }
        string Name { get; set; }
    }

    public interface IProductDataTranferObject
    {
        int Id { get; set; }
        int ApoClass { get; set; }
        string Code { get; set; }
        string ProductName { get; set; }
    }
}
