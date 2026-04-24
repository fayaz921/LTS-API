namespace LTS.API.Features.UserManangement.DTOs
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int MaxUsers { get; set; }
        public int MaxClients { get; set; }
        public int CurrentUserCount { get; set; }

        public bool IsTrialActive { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }

        public bool IsSubscriptionActive { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
