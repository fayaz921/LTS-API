using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetTrialOrganizations
{
    public class GetTrialOrganizationsQuery : IRequest<ApiResponse<List<OrganizationDto>>>
    {
    }
}
