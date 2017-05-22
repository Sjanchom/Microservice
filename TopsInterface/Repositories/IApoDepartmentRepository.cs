using System.Linq;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IApoDepartmentRepository : IApoBaseRepository<IApoDepartmentDomain>
    {
        IQueryable<IApoDepartmentDomain> GetAll(IApoDepartmentResourceParameter resourceParameters);
        IApoDepartmentDomain GetByName(IApoDepartmentForCreateOrEdit item);
        IQueryable<IApoDepartmentDomain> GetByApoGroup(int id);
    }
}
