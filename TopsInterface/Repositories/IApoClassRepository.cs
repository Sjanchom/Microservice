using System.Linq;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IApoClassRepository : IApoBaseRepository<IApoClassDomain>
    {
        IQueryable<IApoClassDomain> GetAll(IApoDepartmentResourceParameter resourceParameters);
        IApoClassDomain GetByName(IApoClassForCreateOrEdit item);
        IQueryable<IApoClassDomain> GetByApoDepartment(int id);
    }

}
