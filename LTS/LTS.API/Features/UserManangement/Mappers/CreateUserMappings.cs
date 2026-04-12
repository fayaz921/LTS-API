using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Commands.Authentication.CreateUser;

namespace LTS.API.Features.UserManangement.Mappers
{
    public static class CreateUserMappings
    {
        // ✅ Organization banao — hamesha Free Trial se shuru
        public static Organization ToOrganization(this CreateUserCommand request)
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                OrganizationName = request.OrganizationName,
                Slug = request.OrganizationName.ToLower().Trim().Replace(" ", "-"),
                Plan = SubscriptionPlan.Free,

                // Trial fields — TrialStartDate pehli login pe set hoga
                IsTrialActive = true,
                TrialStartDate = null,
                TrialEndDate = null,

                // OTP verify hone ke baad true hoga
                IsActive = false,

                // Trial limits
                MaxUsers = 0,
                MaxClients = 0,

                CreatedAt = DateTime.UtcNow,
            };
        }

        // ✅ User banao — OTP ke saath, IsVerified = false
        public static User ToUser(this CreateUserCommand request,Guid organizationId,string passwordHash,string otp)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Name = request.OwnerName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = UserRole.User,
                IsActive = false,
                Otp = otp,
                OTPExpiry = DateTime.UtcNow.AddMinutes(10),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.Email
            };
        }

        private static int GetMaxUsers(SubscriptionPlan plan) => plan switch
        {
            SubscriptionPlan.Free => 2,
            SubscriptionPlan.Basic => 5,
            SubscriptionPlan.Pro => 20,
            SubscriptionPlan.Enterprise => 100,
            _ => 2
        };
    }
}
