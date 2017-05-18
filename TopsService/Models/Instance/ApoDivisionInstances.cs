using System.Collections.Generic;
using System.Linq;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace TopsService.Models.Instance
{
    public class ApoDivisionInstances
    {
        private static ApoDivisionInstances _instance = null;
        private readonly List<IApoDivisionDomain> _apoDivisionDomains;

        public List<IApoDivisionDomain> GetApoDivisionDomains => _apoDivisionDomains;

        private ApoDivisionInstances(IApoDivisionRepository db)
        {
            _apoDivisionDomains = db.GetAll().ToList();
        }

        private static readonly object SyncLock = new object();

        public static ApoDivisionInstances GetInstance(IApoDivisionRepository db)
        {
            lock (SyncLock)
            {
                if (_instance != null) return _instance;

                _instance = new ApoDivisionInstances(db);

                return _instance;
            }
        }

    }
}
