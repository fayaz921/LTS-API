using LTS.API.Features.Followups.Commands.CreateFollowup;
using LTS.API.Features.Followups.Commands.DeleteFollowup;
using LTS.API.Features.Followups.Commands.UpdateFollowup;
using LTS.API.Features.Followups.Queries.GetFollowupsByCase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Followups
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowupController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FollowupController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFollowup( [FromBody] CreateFollowupCommand command,  CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return StatusCode((int)result.Status, result);
        }

        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetByCaseId( Guid caseId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send( new GetFollowupsByCaseQuery(caseId), cancellationToken);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update( Guid id, [FromBody] UpdateFollowupCommand command,  CancellationToken cancellationToken)
        {
            command = command with { FollowupId = id };
            var result = await _mediator.Send(command, cancellationToken);

            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( Guid id,  CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteFollowupCommand(id), cancellationToken);
            return StatusCode((int)result.Status, result);
        }
    }
}
