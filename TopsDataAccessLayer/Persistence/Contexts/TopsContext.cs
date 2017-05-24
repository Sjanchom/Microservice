using System.Data.Entity;
using TopsDataAccessLayer.Persistence.Entities;
using TopsDataAccessLayer.Persistence.EntityConfigurations;

namespace TopsDataAccessLayer.Persistence.Contexts
{
    public  class TopsContext : DbContext
    {
        public TopsContext()
            : base("name=TopsContext")
        {
        }


        public  DbSet<ApoDivision> ApoDivision { get; set; }
        public  DbSet<ApoGroup> ApoGroup { get; set; }
  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("C##");

            modelBuilder.Configurations.Add(new ApoDivisionConfiguration());
            modelBuilder.Configurations.Add(new ApoGroupConfiguration());

        }
    }
}
