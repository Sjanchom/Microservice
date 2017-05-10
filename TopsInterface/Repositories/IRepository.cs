﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace TopsInterface.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);

        T GetById(int id);
        T Get(Expression<Func<T, bool>> where);

        IQueryable GetAll();
        IQueryable GetMany(Expression<Func<T, bool>> where);
    }
}
