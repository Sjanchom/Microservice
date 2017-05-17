using System.Collections.Generic;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IApoGroupService : IApoBaseService<IApoGroupDataTranferObject, IApoGroupForCreateOrEdit>
    {
        PagedList<IApoGroupDataTranferObject> GetAll(IApoGroupResourceParameter apoGroupResourceParameter);
        IEnumerable<IApoGroupDataTranferObject> GetApoGroupByApoDivision(int id);
    }
}
