using System;
using System.Linq;
using System.Linq.Expressions;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Update(int id,T entity);
        bool Delete(T entity);
        bool Delete(int id);
        bool Delete(Expression<Func<T, bool>> where);

        T GetById(int id);
        T Get(Expression<Func<T, bool>> where);

        IQueryable<T> GetAll(IBaseResourceParameter resourceParameters);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
