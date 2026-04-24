using LTS.API.Domain.Enums;

namespace LTS.API.Features.Reports.DTOs
{
    public record GetDepartmentWiseReportDto(Guid DepartmentId, string DepartmentName, int TotalCases, int FinalizedCaseStatus, int PendingCaseStatus);
   
}
