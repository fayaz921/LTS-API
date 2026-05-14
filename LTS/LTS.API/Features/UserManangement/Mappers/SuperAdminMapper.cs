using LTS.API.Features.UserManangement.DTOs;

namespace LTS.API.Features.UserManangement.Mappers
{
    public class SuperAdminMapper
    {
        public static DashboardStatsDto ToDashboardStatsDto(
                int totalOrganizations,
                int activeTrials,
                int expiringIn3Days,
                int paidSubscriptions,
                decimal totalRevenue)
        {
            return new DashboardStatsDto
            {
                TotalOrganizations = totalOrganizations,
                ActiveTrials = activeTrials,
                ExpiringIn3Days = expiringIn3Days,
                PaidSubscriptions = paidSubscriptions,
                TotalRevenue = totalRevenue
            };
        }
    }
}
