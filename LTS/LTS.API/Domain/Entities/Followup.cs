namespace LTS.API.Domain.Entities
{
    public class Followup : BaseEntity
    {
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; }
        public string? InterimOrder { get; set; }
        public string? Decision { get; set; }
        public string? Remarks { get; set; }

        // Navigation
        public Case Case { get; set; } = null!;
    }
}
