using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetAllOrganizations
{
    public class GetAllOrganizationsQuery:IRequest<ApiResponse<List<OrganizationDto>>>
    {
    }
}
