namespace LTS.API.Features.Departments.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? AddressContact { get; set; }
        public bool IsActive { get; set; }
    }
}