namespace LTS.API.Features.UserManangement.DTOs
{
    public class GetProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
