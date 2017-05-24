using System;
using System.Collections.Generic;

namespace TopsDataAccessLayer.Persistence.Entities
{
    public class ApoDivision
    {
        public int DivionId { get; set; }

        public string DivisionName { get; set; }
  
        public string DivisionCode { get; set; }

        public ICollection<ApoGroup> ApoGroups { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public int? LastUpdatedBy { get; set; }

        public int IsActive { get; set; }

        public string Remark { get; set; }
    }
}
