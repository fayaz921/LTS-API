using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetSubscriptionOrganizations
{
    public sealed class GetSubscriptionOrganizationsQueryHandler : IRequestHandler<GetSubscriptionOrganizationsQuery, ApiResponse<PaginatedResponse<OrganizationDto>>>
    {
        private readonly AppDbContext _context;

        public GetSubscriptionOrganizationsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedResponse<OrganizationDto>>> Handle(GetSubscriptionOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Organizations
             .AsNoTracking()
             .Where(o => o.IsSubscriptionActive);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(o => o.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(o => new OrganizationDto
                {
                    Id = o.Id,
                    OrganizationName = o.OrganizationName,
                    Slug = o.Slug,
                    Plan = o.Plan.ToString(),
                    IsActive = o.IsActive,
                    MaxUsers = o.MaxUsers,
                    MaxClients = o.MaxClients,
                    CurrentUserCount = o.Users.Count(),
                    IsTrialActive = o.IsTrialActive,
                    TrialStartDate = o.TrialStartDate,
                    TrialEndDate = o.TrialEndDate,
                    IsSubscriptionActive = o.IsSubscriptionActive,
                    SubscriptionStartDate = o.SubscriptionStartDate,
                    SubscriptionEndDate = o.SubscriptionEndDate,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var paginated = PaginatedResponse<OrganizationDto>.Create(
                items, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PaginatedResponse<OrganizationDto>>.Ok(
                paginated, "Subscription organizations fetched successfully");
        }
    }
}
