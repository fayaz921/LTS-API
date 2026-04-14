using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetTrialOrganizations
{
    public class GetTrialOrganizationsQueryHandler : IRequestHandler<GetTrialOrganizationsQuery, ApiResponse<List<OrganizationDto>>>
    {
        private readonly AppDbContext _context;

        public GetTrialOrganizationsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<OrganizationDto>>> Handle(GetTrialOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var orgs = await _context.Organizations
                .Where(o => o.IsTrialActive == true)
                .Select(o => new OrganizationDto
                {
                    Id = o.Id,
                    OrganizationName = o.OrganizationName,
                    Slug = o.Slug,
                    Plan = o.Plan.ToString(),
                    IsActive = o.IsActive,
                    MaxUsers = o.MaxUsers,
                    MaxClients = o.MaxClients,
                    CurrentUserCount = o.Users.Count,
                    IsTrialActive = o.IsTrialActive,
                    TrialStartDate = o.TrialStartDate,
                    TrialEndDate = o.TrialEndDate,
                    IsSubscriptionActive = o.IsSubscriptionActive,
                    SubscriptionStartDate = o.SubscriptionStartDate,
                    SubscriptionEndDate = o.SubscriptionEndDate,
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return ApiResponse<List<OrganizationDto>>.Ok(orgs, "Trial organizations fetched successfully");
        }
    }
}
