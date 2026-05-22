using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;

namespace LTS.API.Features.CaseFeature.Queries.GetCases
{
    public record GetCaseDto(
     Guid Id,
    string CaseNo,
    string Title,
    string Subject,
    string DAG,
    string Status,
    DateTime DateInstitution,
    string CourtName,
    string DepartmentName,
    List<PetitionerDetailDto> Petitioners
     );

    public record PetitionerDetailDto(
       Guid Id,
       string Name,
       string? CNIC,
       string? Email,
       string? Phone
   );
}
