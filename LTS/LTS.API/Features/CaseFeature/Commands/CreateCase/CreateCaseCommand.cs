using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.CaseFeature.Commands.CreateCase
{
    public record CreateCaseCommand(
        Guid CourtId,
        Guid DepartmentId,
        Guid PetitionerId,
        string DAG,
        string Title,
        string Subject,
        string Detail,
        DateTime DateInstitution,
        string EmailList
        ) : IRequest<ApiResponse<string>>
    {
        public Guid OrganizationId { get; set; }
    }

}
