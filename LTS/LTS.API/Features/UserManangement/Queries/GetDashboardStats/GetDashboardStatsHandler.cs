using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Queries.GetDashboardStats
{
    public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery,
ApiResponse<DashboardStatsDto>>
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

            var totalOrganizations = await _context.Organizations
                .CountAsync(cancellationToken);

            var activeTrials = await _context.Organizations
                .CountAsync(o => o.IsTrialActive && o.TrialEndDate >= now, cancellationToken);

            var expiringIn3Days = await _context.Organizations
                .CountAsync(o => o.IsTrialActive && o.TrialEndDate >= now && o.TrialEndDate <= next3Days, cancellationToken);

            var paidSubscriptions = await _context.Organizations
                .CountAsync(o => o.IsSubscriptionActive && o.SubscriptionEndDate >= now, cancellationToken);

            var subscribedOrganizations = await _context.Organizations
                .Where(o => o.IsSubscriptionActive && o.SubscriptionEndDate >= now)
                .ToListAsync(cancellationToken);

            decimal totalRevenue = 0;
            foreach (var org in subscribedOrganizations)
            {
                totalRevenue += org.Plan switch
                {
                    Domain.Enums.SubscriptionPlan.Basic => 5000,
                    Domain.Enums.SubscriptionPlan.Pro => 10000,
                    Domain.Enums.SubscriptionPlan.Enterprise => 20000,
                    _ => 0
                };
            }

            var dto = SuperAdminMapper.ToDashboardStatsDto(
                totalOrganizations,
                activeTrials,
                expiringIn3Days,
                paidSubscriptions,
                totalRevenue);

            return ApiResponse<DashboardStatsDto>.Ok(dto, "Dashboard stats fetched successfully");
        }
    }
}

