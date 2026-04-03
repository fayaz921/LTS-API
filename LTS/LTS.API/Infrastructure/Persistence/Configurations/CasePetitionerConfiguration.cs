using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations;

public class CasePetitionerConfiguration : IEntityTypeConfiguration<CasePetitioner>
{
    public void Configure(EntityTypeBuilder<CasePetitioner> builder)
    {
        builder.HasKey(x => x.Id);

        // prevent duplicate — same petitioner cannot be linked to same case twice
        builder.HasIndex(x => new { x.CaseId, x.PetitionerId })
            .IsUnique();

        builder.HasOne(x => x.Case)
            .WithMany(x => x.CasePetitioners)
            .HasForeignKey(x => x.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Petitioner)
            .WithMany(x => x.CasePetitioners)
            .HasForeignKey(x => x.PetitionerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}