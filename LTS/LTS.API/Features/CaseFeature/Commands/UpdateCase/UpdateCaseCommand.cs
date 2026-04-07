using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using MediatR;

namespace LTS.API.Features.CaseFeature.Commands.UpdateCase
{
    public record UpdateCaseCommand(
    Guid Id,
    string CaseNo,
    Guid CourtId,
    Guid DepartmentId,
    string DAG,
    string Title,
    string Subject,
    string Detail,
    DateTime DateInstitution,
    CaseStatus Status,
    string EmailList
    ) : IRequest<ApiResponse<string>>;
}
