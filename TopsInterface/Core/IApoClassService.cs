using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IApoClassService : IApoBaseService<IApoClassDataTranferObject, IApoClassForCreateOrEdit>
    {
        PagedList<IApoClassDataTranferObject> GetAll(IApoClassResourceParameter apoGroupResourceParameter);
        IEnumerable<IApoClassDataTranferObject> GetApoClassByApoDepartment(int id);
    }
}
