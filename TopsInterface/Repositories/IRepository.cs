using System;
using System.Linq;
using System.Linq.Expressions;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        void Delete(Expression<Func<T, bool>> where);

        T GetById(int id);
        T Get(Expression<Func<T, bool>> where);

        IQueryable<T> GetAll(IProductResourceParameters productResourceParameters);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
