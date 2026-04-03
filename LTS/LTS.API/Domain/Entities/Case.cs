using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class Case : BaseEntity
    {
        public string CaseNo { get; set; } = string.Empty;
        public Guid CourtId { get; set; }
        public Guid DepartmentId { get; set; }
        public string DAG { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public DateTime DateInstitution { get; set; }
        public CaseStatus Status { get; set; }
        public string EmailList { get; set; } = string.Empty;

        // Navigation
        public Court Court { get; set; } = null!;
        public Department Department { get; set; } = null!;
        public ICollection<CasePetitioner> CasePetitioners { get; set; } = new List<CasePetitioner>();
        public ICollection<Followup> Followups { get; set; } = new List<Followup>();
        public ICollection<CaseDocument> Documents { get; set; } = new List<CaseDocument>();
        public ICollection<Bench> Benches { get; set; } = new List<Bench>();
    }
}
