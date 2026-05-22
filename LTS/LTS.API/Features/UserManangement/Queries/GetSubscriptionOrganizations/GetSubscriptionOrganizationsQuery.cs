using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetSubscriptionOrganizations
{
    public class GetSubscriptionOrganizationsQuery : IRequest<ApiResponse<PaginatedResponse<OrganizationDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
