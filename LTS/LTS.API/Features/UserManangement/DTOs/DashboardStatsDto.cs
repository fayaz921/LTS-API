namespace LTS.API.Features.UserManangement.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalOrganizations { get; set; }
        public int ActiveTrials { get; set; }
        public int ExpiringIn3Days { get; set; }
        public int PaidSubscriptions { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
