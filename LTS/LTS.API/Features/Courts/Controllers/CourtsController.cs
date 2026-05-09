using LTS.API.Features.Courts.Commands.CreateCourt;
using LTS.API.Features.Courts.Commands.DeleteCourt;
using LTS.API.Features.Courts.Commands.UpdateCourt;
using LTS.API.Features.Courts.Queries.GetAllCourts;
using LTS.API.Features.Courts.Queries.GetCourtById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace LTS.API.Features.Courts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourtsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourtCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCourtCommand command, CancellationToken ct)
        {
            command = command with { Id = id };
            var result = await _mediator.Send(command, ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteCourtCommand(id), ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive = null, CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetAllCourtsQuery(isActive), ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCourtByIdQuery(id), ct);
            return StatusCode((int)result.Status, result);
        }
    }
}
