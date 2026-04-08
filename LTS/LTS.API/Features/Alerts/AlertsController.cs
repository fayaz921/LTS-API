using LTS.API.Features.Alerts.Commands.SendHearingAlert;
using LTS.API.Features.Alerts.Queires.GetUpComingHearingAlert;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Alerts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController(IMediator mediator): ControllerBase
    {
        [HttpGet("GetUpComingHearing")]
        public async Task<IActionResult> GetUpComingHearing()
        {
            var result = await mediator.Send(new GetUpComingHearingQuery());
            return StatusCode((int)result.Status, result);
        }
        [HttpPost("SendHearingAlert{CaseId:guid}")]
        public async Task<IActionResult> SendHearingAlert(Guid CaseId)
        {
            var result = await mediator.Send(new SendHearingAlertCommand(CaseId));
            return StatusCode((int)result.Status, result);
        }
    }
  
}
