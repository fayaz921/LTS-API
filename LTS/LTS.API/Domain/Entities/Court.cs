namespace LTS.API.Domain.Entities
{
    public class Court
    {
        public Guid Id { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public string? AddressContact { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}