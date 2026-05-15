using LTS.API.Domain.Enums;

namespace LTS.API.Features.UserManangement.DTOs
{
    public class GetMeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string OrganizationPlan { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
