using System.Data.Entity.ModelConfiguration;
using TopsDataAccessLayer.Persistence.Entities;

namespace TopsDataAccessLayer.Persistence.EntityConfigurations
{
    public class ApoDivisionConfiguration : EntityTypeConfiguration<ApoDivision>
    {
        public ApoDivisionConfiguration()
        {

            ToTable("APO_DIVISION_DEV");

            HasKey(d => d.DivionId);

            Property(d => d.DivionId)
                .HasColumnName("DIVISION_ID")
                .IsRequired();

            Property(d => d.DivisionName)
                .HasColumnName("DIVISION_NAME")
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();


            Property(d => d.DivisionCode)
                .HasColumnName("DIVISION_CODE")
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
