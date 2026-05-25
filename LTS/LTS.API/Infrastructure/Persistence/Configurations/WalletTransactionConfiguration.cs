using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            builder.Property(x => x.RecordedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PaymentRequestId)
                .IsRequired(false);

            builder.Property(x => x.OrganizationId)
                .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.PaymentRequest)
                .WithMany()
                .HasForeignKey(x => x.PaymentRequestId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne<Organization>()
               .WithMany(x => x.WalletTransactions)
               .HasForeignKey(x => x.OrganizationId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}