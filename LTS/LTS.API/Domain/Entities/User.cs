using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string Otp { get; set; } = string.Empty;
        public DateTime? OTPExpiry { get; set; }
        public Organization Organization { get; set; } = null!;

    }
}
