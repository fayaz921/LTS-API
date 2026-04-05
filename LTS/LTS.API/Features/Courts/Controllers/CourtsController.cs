using MediatR;
using Microsoft.AspNetCore.Mvc;
using LTS.API.Features.Courts.Commands.CreateCourt;
using LTS.API.Features.Courts.Commands.UpdateCourt;
using LTS.API.Features.Courts.Commands.DeleteCourt;
using LTS.API.Features.Courts.Queries.GetAllCourts;

namespace LTS.API.Features.Courts.Controllers;

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
    public async Task<Guid> Create(CreateCourtCommand command)
        => await _mediator.Send(command);

    [HttpPut]
    public async Task<bool> Update(UpdateCourtCommand command)
        => await _mediator.Send(command);

    [HttpDelete("{id}")]
    public async Task<bool> Delete(Guid id)
        => await _mediator.Send(new DeleteCourtCommand(id));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllCourtsQuery()));
}