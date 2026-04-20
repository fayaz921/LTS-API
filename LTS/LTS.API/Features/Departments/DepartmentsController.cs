using LTS.API.Features.Departments.Commands.UpdateDepartment;
using LTS.API.Features.Departments.Queries.GetAllDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateDepartmentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode((int)result.Status, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateDepartmentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode((int)result.Status, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteDepartmentCommand(id), ct);
        return StatusCode((int)result.Status, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllDepartmentsQuery(), ct);
        return StatusCode((int)result.Status, result);
    }
}