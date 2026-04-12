using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrganizationName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.Plan)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // Trial fields
            builder.Property(x => x.TrialStartDate)
                .IsRequired(false);

            builder.Property(x => x.TrialEndDate)
                .IsRequired(false);

            builder.Property(x => x.IsTrialActive)
                .IsRequired()
                .HasDefaultValue(false);

            // Subscription fields
            builder.Property(x => x.SubscriptionStartDate)
                .IsRequired(false);

            builder.Property(x => x.SubscriptionEndDate)
                .IsRequired(false);

            builder.Property(x => x.IsSubscriptionActive)
                .IsRequired()
                .HasDefaultValue(false);

            // Status
            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            // Limits
            builder.Property(x => x.MaxUsers)
                .IsRequired()
                .HasDefaultValue(2);

            builder.Property(x => x.MaxClients)
                .IsRequired()
                .HasDefaultValue(5);

            // Audit
            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .IsRequired(false);

            // Relationship
            builder.HasMany(x => x.Users)
                .WithOne(x => x.Organization)
                .HasForeignKey(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
