using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopsInterface.Entities;
using TopsInterface.Repositories;

namespace TopsService.Models.Instance
{
    public class ApoGroupInstances
    {
        private static ApoGroupInstances _instance = null;
        private readonly List<IApoGroupDomain> _apoDivisionDomains;

        public List<IApoGroupDomain> GetApoGroupDomains => _apoDivisionDomains;

        private ApoGroupInstances(IApoGroupRepository db)
        {
            _apoDivisionDomains = db.All().ToList();
        }

        private static readonly object SyncLock = new object();

        public static ApoGroupInstances GetInstance(IApoGroupRepository db)
        {
            lock (SyncLock)
            {
                if (_instance != null) return _instance;

                _instance = new ApoGroupInstances(db);

                return _instance;
            }
        }

    }

}
