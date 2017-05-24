using System.Collections.Generic;
using System.Linq;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace TopsService.Models.Instance
{
    public class ApoDepartmentInstances
    {
        private static ApoDepartmentInstances _instance = null;
        private readonly List<IApoDepartmentDomain> _apoDepartmentDomains;

        public List<IApoDepartmentDomain> GetApoDepartmentDomains => _apoDepartmentDomains;

        private ApoDepartmentInstances(IApoDepartmentRepository db)
        {
            _apoDepartmentDomains = db.All().ToList();
        }

        private static readonly object SyncLock = new object();

        public static ApoDepartmentInstances GetInstance(IApoDepartmentRepository db)
        {
            lock (SyncLock)
            {
                if (_instance != null) return _instance;

                _instance = new ApoDepartmentInstances(db);

                return _instance;
            }
        }

    }

}
