using System;

namespace TopsDataAccessLayer.Persistence.Entities
{
    public class ApoGroup
    {
        public int GroupId { get; set; }

        public int DivisionId { get; set; }

        public ApoDivision Division { get; set; }

        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public int? LastUpdatedBy { get; set; }

        public int? IsActive { get; set; }

        public string Remark { get; set; }

    }
}
