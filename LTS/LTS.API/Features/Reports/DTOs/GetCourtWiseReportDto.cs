namespace LTS.API.Features.Reports.DTOs
{
    public class GetCourtWiseReportDto
    {
        public Guid CourtId { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public int TotalCases { get; set; }
    }
}
