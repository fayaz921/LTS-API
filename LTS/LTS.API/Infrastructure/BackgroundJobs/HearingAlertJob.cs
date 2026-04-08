using LTS.API.Features.Alerts.Commands.SendHearingAlert;
using LTS.API.Features.Alerts.Queires.GetUpComingHearingAlert;
using MediatR;

namespace LTS.API.Infrastructure.BackgroundJobs
{
    public class HearingAlertJob(IMediator mediator)
    {
        public async Task ExecuteAsync()
        {
            var upcomingHearings = await mediator.Send(new GetUpComingHearingQuery());
            foreach(var hearing in upcomingHearings.Data!)
            {
                await mediator.Send(new SendHearingAlertCommand(hearing.CaseId));
            }
        }
    }
}
