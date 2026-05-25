using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetAllOrganizations
{
    public record GetAllOrganizationsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null,
        SubscriptionPlan? Plan = null,
        bool? IsActive = null,
        bool? IsBlocked = null,
        bool? IsSubscriptionActive = null,
    bool? IsTrialActive = null
    ) : IRequest<ApiResponse<PaginatedResponse<OrganizationDto>>>;
}
