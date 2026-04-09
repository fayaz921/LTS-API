using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Commands.Authentication.CreateUser;

namespace LTS.API.Features.UserManangement.Mappers
{
    public static class CreateUserMappings
    {
        public static Organization ToOrganization(this CreateUserCommand request)
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                OrganizationName = request.OrganizationName,
                Slug = request.OrganizationName.ToLower().Trim().Replace(" ", "-"),
                Plan = request.SubscriptionPlan,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                IsActive = false,
                MaxUsers = GetMaxUsers(request.SubscriptionPlan),
                CreatedAt = DateTime.UtcNow,
            };
        }

        public static User ToUser(this CreateUserCommand request, Guid organizationId, string passwordHash)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Name = request.OwnerName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = UserRole.User,
                Otp = "",
                IsActive = false,
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
