namespace LTS.API.Domain.Entities
{
    public class Petitioner : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Business { get; set; }
        public string? Email { get; set; }

        // Navigation
        public ICollection<CasePetitioner> CasePetitioners { get; set; } = new List<CasePetitioner>();
    }
}
