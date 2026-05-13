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
        public async Task<IActionResult> CreateFollowup([FromBody] CreateFollowupCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetByCase(Guid caseId)
        {
            var result = await _mediator.Send(new GetFollowupsByCaseQuery(caseId));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFollowupCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteFollowupCommand(id));
            return Ok(result);
        }
    }
}
