using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public record GetCasesDto(
     Guid Id,
    string CaseNo,
    string Title,
    string Subject,
    string DAG,
    string Status,
    DateTime DateInstitution,
    string CourtName,
    string DepartmentName,
    List<string> Petitioners
     );
}
