using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, ApiResponse<OrganizationDto>>
    {
        private readonly AppDbContext _context;

        public GetOrganizationByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<OrganizationDto>> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations
                .Where(o => o.Id == request.OrganizationId)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (org is null)
                return ApiResponse<OrganizationDto>.Fail("Organization not found");

            return ApiResponse<OrganizationDto>.Ok(org, "Organization fetched successfully");
        }
    }
}
