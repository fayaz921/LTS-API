using Hangfire.MemoryStorage.Dto;

namespace LTS.API.Features.UserManangement.Queries.GetAllUsers
{
    public class GetAllUsersDto
    {
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public bool OrgIsActive { get; set; }
        public DateTime EndDate { get; set; }
        public List<UserDto> Users { get; set; } = new();
    }
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

}
