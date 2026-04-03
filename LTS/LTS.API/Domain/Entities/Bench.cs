namespace LTS.API.Domain.Entities
{
    public class Bench
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public string JudgeName { get; set; } = string.Empty;
        public string? JudgeContactNo { get; set; }
        public string? JudgeEmail { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Case Case { get; set; } = null!;
    }
}
