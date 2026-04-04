using LTS.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LTS.API.Infrastructure.Persistence.Configurations
{
    public class UserCredentialConfiguration: IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {
            builder.HasKey(x => x.UserCredentialId);

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.Otp)
                .HasMaxLength(10);

            // Foreign Key relationship
            builder.HasOne(x => x.User)
                   .WithOne(u => u.UserCredential)
                   .HasForeignKey<UserCredential>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
