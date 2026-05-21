using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Commands.Authentication.CreateUser;

namespace LTS.API.Features.UserManangement.Mappers
{
    public static class CreateUserMappings
    {
        public static Organization ToOrganization(this string organizationName)
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                OrganizationName = organizationName,
                Slug = organizationName.ToLower().Trim().Replace(" ", "-"),
                Plan = SubscriptionPlan.Free,

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

        //  User banao — OTP ke saath, IsVerified = false
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
    }
}
