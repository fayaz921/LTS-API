namespace LTS.API.Domain.Entities
{
    public class UserCredential
    {
        public Guid UserCredentialId { get; set; }
        public Guid UserId { get; set; }  // ← typo fix: 'Userid' → 'UserId'
        public byte[] PasswordHash { get; set; }
        public string Otp { get; set; } = string.Empty;
        public DateTime? OTPExpiry { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;

    }
}
