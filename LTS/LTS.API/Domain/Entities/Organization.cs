using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Basic;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int MaxUsers { get; set; } = 5;

        // Audit fields manually
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();


        public int GetMaxUsers() => Plan switch
        {
            SubscriptionPlan.Free => 2,
            SubscriptionPlan.Basic => 5,
            SubscriptionPlan.Pro => 20,
            SubscriptionPlan.Enterprise => 100, _ => 5
        };
    }
}
