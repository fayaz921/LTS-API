using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Alerts.Commands.SendHearingAlert
{
    public record SendHearingAlertCommand(Guid CaseID) : IRequest<ApiResponse<bool>>;
 
}
