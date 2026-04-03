using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations;

public class BenchConfiguration : IEntityTypeConfiguration<Bench>
{
    public void Configure(EntityTypeBuilder<Bench> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.JudgeName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.JudgeContactNo)
            .HasMaxLength(20);

        builder.Property(x => x.JudgeEmail)
            .HasMaxLength(100);

        builder.HasOne(x => x.Case)
            .WithMany(x => x.Benches)
            .HasForeignKey(x => x.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}