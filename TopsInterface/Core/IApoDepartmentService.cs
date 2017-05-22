using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopsInterface.Entities;

namespace TopsInterface.Core
{
    public interface IApoDepartmentService : IApoBaseService<IApoDepartmentDataTranferObject, IApoDepartmentForCreateOrEdit>
    {
        PagedList<IApoDepartmentDataTranferObject> GetAll(IApoDepartmentResourceParameter apoDepartmentResourceParameter);
        IEnumerable<IApoDepartmentDataTranferObject> GetApoDepartmentByApoGroup(int id);
    }
}
