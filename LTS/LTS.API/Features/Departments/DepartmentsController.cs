using LTS.API.Features.Departments.Commands.CreateDepartment;
using LTS.API.Features.Departments.Commands.DeleteDepartment;
using LTS.API.Features.Departments.Commands.UpdateDepartment;
using LTS.API.Features.Departments.Queries.GetAllDepartments;
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
    public async Task<Guid> Create(CreateDepartmentCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut]
    public async Task<bool> Update(UpdateDepartmentCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(Guid id)
    {
        return await _mediator.Send(new DeleteDepartmentCommand(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllDepartmentsQuery()));
    }
}