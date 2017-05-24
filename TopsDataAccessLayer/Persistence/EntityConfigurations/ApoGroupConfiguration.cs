using System.Data.Entity.ModelConfiguration;
using TopsDataAccessLayer.Persistence.Entities;

namespace TopsDataAccessLayer.Persistence.EntityConfigurations
{
    public class ApoGroupConfiguration : EntityTypeConfiguration<ApoGroup>
    {
        public ApoGroupConfiguration()
        {

            ToTable("APO_GROUP_DEV");

            HasKey(d => d.GroupId);

            Property(d => d.GroupId)
                .HasColumnName("GROUP_ID")
                .IsRequired();

            Property(d => d.DivisionId)
                .HasColumnName("DIVISION_ID")
                .IsRequired();

            HasRequired(x => x.Division)
                .WithMany(x => x.ApoGroups)
                .HasForeignKey(x => x.DivisionId)
                .WillCascadeOnDelete(false);

            Property(d => d.GroupName)
                .HasColumnName("GROUP_NAME")
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();


            Property(d => d.GroupCode)
                .HasColumnName("GROUP_CODE")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();


            Property(d => d.CreatedDateTime)
                .HasColumnName("CREATED_DATE");

            Property(d => d.CreatedBy)
                .HasColumnName("CREATED_BY");

            Property(d => d.UpdatedDateTime)
                .HasColumnName("UPDATED_DATE");

            Property(d => d.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            Property(d => d.LastUpdatedDateTime)
                .HasColumnName("LAST_UPDATED_DATE");

            Property(d => d.LastUpdatedBy)
                .HasColumnName("LAST_UPDATED_BY");

            Property(d => d.IsActive)
                .HasColumnName("ISACTIVE");

            Property(d => d.Remark)
                .HasColumnName("REMARK")
                .IsUnicode(false)
                .HasMaxLength(255);

        }

    }

}
