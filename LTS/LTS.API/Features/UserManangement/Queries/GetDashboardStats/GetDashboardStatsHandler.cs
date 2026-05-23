using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetDashboardStats
{
    public sealed class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery,ApiResponse<DashboardStatsDto>>
    {
        private readonly AppDbContext _context;
        public GetDashboardStatsHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var next3Days = now.AddDays(3);

            var stats = await _context.Organizations
             .AsNoTracking()
             .GroupBy(_ => 1)
             .Select(g => new DashboardStatsDto
             {
                 TotalOrganizations = g.Count(),

                 ActiveTrials = g.Count(o =>
                     o.IsTrialActive && o.TrialEndDate >= now),

                 ExpiringIn3Days = g.Count(o =>
                     o.IsTrialActive &&
                     o.TrialEndDate >= now &&
                     o.TrialEndDate <= next3Days),

                 PaidSubscriptions = g.Count(o =>
                     o.IsSubscriptionActive &&
                     o.SubscriptionEndDate >= now),

                 TotalRevenue = g
                     .Where(o => o.IsSubscriptionActive && o.SubscriptionEndDate >= now)
                     .Sum(o => o.Plan == Domain.Enums.SubscriptionPlan.Basic ? 5000m
                             : o.Plan == Domain.Enums.SubscriptionPlan.Pro ? 10000m
                             : o.Plan == Domain.Enums.SubscriptionPlan.Enterprise ? 20000m
                             : 0m)
             })
             .FirstOrDefaultAsync(cancellationToken);
            stats ??= new DashboardStatsDto();
            return ApiResponse<DashboardStatsDto>.Ok( stats,"Dashboard stats fetched successfully");
        }
    }
}

