using System.Collections.Generic;
using System.Linq;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace TopsService.Models.Instance
{
    public class ApoClassInstances
    {
        private static ApoClassInstances _instance = null;
        private readonly List<IApoClassDomain> _apoClassDomains;

        public List<IApoClassDomain> GetApoDepartmentDomains => _apoClassDomains;

        private ApoClassInstances(IApoClassRepository db)
        {
            _apoClassDomains = db.GetAll().ToList();
        }

        private static readonly object SyncLock = new object();

        public static ApoClassInstances GetInstance(IApoClassRepository db)
        {
            lock (SyncLock)
            {
                if (_instance != null) return _instance;

                _instance = new ApoClassInstances(db);

                return _instance;
            }
        }

    }

}
