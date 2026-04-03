using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations;

public class FollowupConfiguration : IEntityTypeConfiguration<Followup>
{
    public void Configure(EntityTypeBuilder<Followup> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InterimOrder)
            .HasMaxLength(2000);

        builder.Property(x => x.Decision)
            .HasMaxLength(2000);

        builder.Property(x => x.Remarks)
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(100);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(100);

        builder.HasOne(x => x.Case)
            .WithMany(x => x.Followups)
            .HasForeignKey(x => x.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}