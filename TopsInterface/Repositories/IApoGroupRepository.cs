using System.Linq;
using TopsInterface.Entities;

namespace TopsInterface.Repositories
{

    public interface IApoGroupRepository : IApoBaseRepository<IApoGroupDomain>
    {
        IQueryable<IApoGroupDomain> GetAll(IApoGroupResourceParameter resourceParameters);
        IApoGroupDomain GetByName(IApoGroupForCreateOrEdit item);
        IQueryable<IApoGroupDomain> GetByApoDivision(int id);
    }
}
