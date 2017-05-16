using TopsInterface.Entities;

namespace TopsInterface.Repositories
{
    public interface IApoDivisionRepository : IApoBaseRepository<IApoDivisionDomain>
    {
        IApoDivisionDomain GetByName(IApoDivisionForCreateOrEdit item);
    }
}
