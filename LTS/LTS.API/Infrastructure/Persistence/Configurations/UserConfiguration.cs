using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}


/*
 * ─────────────────────────────────────────────────────────────
 *  EF CORE CONFIGURATION REFERENCE
 * ─────────────────────────────────────────────────────────────
 *  HasKey                    → Sets primary key
 *  IsRequired                → NOT NULL constraint in database
 *  HasMaxLength(n)           → varchar(n) character limit
 *  HasIndex().IsUnique()     → Unique constraint + faster search
 *  HasIndex(x => new { })    → Unique combination of two columns
 *  HasDefaultValue           → Default value when not provided
 *  HasOne / WithMany         → Defines foreign key relationship
 *  OnDelete(Restrict)        → Blocks parent deletion if children exist
 *  OnDelete(Cascade)         → Auto deletes children when parent deleted
 * ─────────────────────────────────────────────────────────────
 */