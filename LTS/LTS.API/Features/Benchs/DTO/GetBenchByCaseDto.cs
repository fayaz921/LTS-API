namespace LTS.API.Features.Bench.DTOs;

public class GetBenchByCaseDto
{
    public Guid Id { get; set; }
    public Guid CaseId { get; set; }
    public string? JudgeName { get; set; } = string.Empty;
    public string? JudgeContactNo { get; set; } = string.Empty;
    public string? JudgeEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}