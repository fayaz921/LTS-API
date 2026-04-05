namespace LTS.API.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string? AddressContact { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Case> Cases { get; set; } = new List<Case>();
    }
}