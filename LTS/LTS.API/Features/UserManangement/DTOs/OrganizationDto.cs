using LTS.API.Domain.Entities;

namespace LTS.API.Features.UserManangement.DTOs
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }

        public int MaxUsers { get; set; }
        public int MaxPetitioners { get; set; }
        public int MaxCases { get; set; }

        public int CurrentUserCount { get; set; }
        public int CurrentPetitionerCount { get; set; }
        public int CurrentCaseCount { get; set; }

        public bool IsTrialActive { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }

        public bool IsSubscriptionActive { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public int TotalPaymentRequests { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public string? LastPaymentStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
