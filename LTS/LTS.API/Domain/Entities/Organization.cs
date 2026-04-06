using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class Organization:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Basic;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int MaxUsers { get; set; } = 5;

        // Navigation
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
