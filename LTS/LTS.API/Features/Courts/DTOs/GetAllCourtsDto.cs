namespace LTS.API.Features.Courts.DTOs;

public class GetAllCourtsDto
{
    public Guid Id { get; set; }
    public string CourtName { get; set; } = string.Empty;
    public string? AddressContact { get; set; }
    public bool IsActive { get; set; }
}