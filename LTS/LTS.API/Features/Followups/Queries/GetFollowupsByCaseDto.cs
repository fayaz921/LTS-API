namespace LTS.API.Features.Followups.Queries
{
    public class GetFollowupsByCaseDto
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; }
        public string? InterimOrder { get; set; }
        public string? Decision { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
