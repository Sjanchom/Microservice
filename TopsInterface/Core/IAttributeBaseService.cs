using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Core
{
    public interface IAttributeBaseService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Edit(T item);
        bool Delete(int id);
    }
}
