using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LTS.API.Infrastructure.Persistence.Configurations;

public class PetitionerConfiguration : IEntityTypeConfiguration<Petitioner>
{
    public void Configure(EntityTypeBuilder<Petitioner> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Address)
            .HasMaxLength(300);

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.Mobile)
            .HasMaxLength(20);

        builder.Property(x => x.Business)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(100);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(100);

        builder.HasIndex(x => x.CNIC)
            .IsUnique();
    }
}