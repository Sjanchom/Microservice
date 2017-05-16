using System;
using TopsInterface.Entities;

namespace TopsService.Models.Domain
{

    public class ApoDivisionDomain : IApoDivisionDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int CreateBy { get; set; }
        public int EditBy { get; set; }
        public int LastEditBy { get; set; }
        public int IsActive { get; set; }
        public string Remark { get; set; }
    }
}
