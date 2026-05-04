using LTS.API.Features.Departments.Commands.CreateDepartment;
using LTS.API.Features.Departments.Commands.DeleteDepartment;
using LTS.API.Features.Departments.Commands.UpdateDepartment;
using LTS.API.Features.Departments.Queries.GetAllDepartments;
using LTS.API.Features.Departments.Queries.GetDepartmentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Departments.Controllers;
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateDepartmentCommand command, CancellationToken ct)
    {
        command = command with { DepartmentId = id };
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDepartmentByIdQuery(id), ct);
        return StatusCode((int)result.Status, result);
    }
}