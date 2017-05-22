using System.Linq;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IApoSubClassRepository : IApoBaseRepository<IApoSubClassDomain>
    {
        IQueryable<IApoSubClassDomain> GetAll(IApoClassResourceParameter resourceParameters);
        IApoSubClassDomain GetByName(IApoSubClassForCreateOrEdit item);
        IQueryable<IApoSubClassDomain> GetByApoClass(int id);
    }
}
