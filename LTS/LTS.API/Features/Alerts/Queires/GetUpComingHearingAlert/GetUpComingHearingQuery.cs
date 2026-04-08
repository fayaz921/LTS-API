using LTS.API.Common.Response;
using LTS.API.Features.Alerts.DTOs;
using MediatR;

namespace LTS.API.Features.Alerts.Queires.GetUpComingHearingAlert
{
    public record GetUpComingHearingQuery() : IRequest<ApiResponse<List<GetUpComingHearingDto>>>;
 
}
