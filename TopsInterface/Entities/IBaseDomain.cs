using System;

namespace TopsInterface.Entities
{
    public interface IBaseDomain
    {
        DateTime? CreateDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        DateTime? LastUpdateDate { get; set; }
        int CreateBy { get; set; }
        int EditBy { get; set; }
        int LastEditBy { get; set; }
        int IsActive { get; set; }
        string Remark { get; set; }
    }
}
