namespace LTS.API.Domain.Entities
{
    public class Court : BaseEntity
    {
        public string CourtName { get; set; } = string.Empty;
        public string? AddressContact { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}