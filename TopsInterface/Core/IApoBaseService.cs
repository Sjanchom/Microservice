using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IApoBaseService<T> where T : IApoBaseDomain
    {
        PagedList<T> GetAll(int page, int pageSize, string searchText);
        T GetById(int id);
        T Create(T item);
        T Edit(T item);
        bool Delete(int id);
    }
}
