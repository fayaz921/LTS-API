using LTS.API.Common.Response;
using LTS.API.Features.UserManangement.DTOs;
using MediatR;

namespace LTS.API.Features.UserManangement.Queries.GetDashboardStats
{
    public class GetDashboardStatsQuery :IRequest<ApiResponse<DashboardStatsDto>>
    {
    }
}
