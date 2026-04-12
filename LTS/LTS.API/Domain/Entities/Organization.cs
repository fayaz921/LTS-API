using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Basic;

        // 🔵 PAID TRIAL
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public bool IsTrialActive { get; set; }

        // 🔵 PAID SUBSCRIPTION
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsSubscriptionActive { get; set; }
       
        public bool IsActive { get; set; }
        public int MaxUsers { get; set; } 
        public int MaxClients { get; set; }

        // Audit fields manually
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();


       
    }
}
