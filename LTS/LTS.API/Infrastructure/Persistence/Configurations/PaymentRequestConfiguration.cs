using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations
{
    public class PaymentRequestConfiguration : IEntityTypeConfiguration<PaymentRequest>
    {
        public void Configure(EntityTypeBuilder<PaymentRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RequestedPlan)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.TransactionId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.SenderName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.SenderPhone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.PaymentMethod)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ScreenshotUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ScreenshotPublicId)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(PaymentStatus.Pending);

            builder.Property(x => x.RejectionReason)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            builder.Property(x => x.ReviewedAt)
                .IsRequired(false);

            builder.Property(x => x.ReviewedBy)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.UpdatedBy)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.HasOne(x => x.Organization)
                 .WithMany(x => x.PaymentRequests)
                 .HasForeignKey(x => x.OrganizationId)
                 .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
