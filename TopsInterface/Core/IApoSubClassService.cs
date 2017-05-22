using System.Collections.Generic;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IApoSubClassService : IApoBaseService<IApoSubClassDataTranferObject,
        IApoSubClassForCreateOrEdit>
    {
        PagedList<IApoSubClassDataTranferObject> GetAll(IApoSubClassResourceParameter apoGroupResourceParameter);
        IEnumerable<IApoSubClassDataTranferObject> GetApoGroupByApoDivision(int id);
    }
}
