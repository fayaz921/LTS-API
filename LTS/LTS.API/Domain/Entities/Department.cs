namespace LTS.API.Domain.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? AddressContact { get; set; }
        public bool IsActive { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}