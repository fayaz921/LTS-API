using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations
{
    public class CaseConfiguration : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CaseNo)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.CaseNo)
                .IsUnique();

            builder.Property(x => x.DAG)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Detail)
                .HasMaxLength(4000);

            builder.Property(x => x.EmailList)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.UpdatedBy)
                .HasMaxLength(100);

            builder.HasOne(x => x.Court)
                .WithMany(x => x.Cases)
                .HasForeignKey(x => x.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Cases)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
