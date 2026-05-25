using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Trial;

        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public bool IsTrialActive { get; set; }

        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsSubscriptionActive { get; set; }
       
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }

        public int MaxUsers { get; set; }
        public int MaxCases { get; set; }
        public int MaxPetitioners { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<PaymentRequest> PaymentRequests { get; set; } = new List<PaymentRequest>();
        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();


    }
}
