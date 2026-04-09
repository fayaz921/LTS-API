using LTS.API.Domain.Entities;

namespace LTS.API.Features.UserManangement.Queries.GetAllUsers
{
    public static class GetAllUsersMappings
    {
        public static GetAllUsersDto ToDto(this Organization organization)
        {
            return new GetAllUsersDto
            {
                OrganizationId = organization.Id,
                OrganizationName = organization.OrganizationName,
                Plan = organization.Plan.ToString(),
                OrgIsActive = organization.IsActive,
                EndDate = organization.EndDate,
                Users = organization.Users.Select(u => u.ToUserDto()).ToList()
            };
        }
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
            };
        }
    }
}
