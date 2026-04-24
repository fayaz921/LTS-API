namespace LTS.API.Features.Reports.DTOs
{
    public record GetSummaryReportDto(int TotalCases, int FinalizedCaseStatus, int PendingCaseStatus);

}
