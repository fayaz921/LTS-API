using LTS.API.Features.Benchs.Commands.CreateBench;
using LTS.API.Features.Benchs.Commands.UpdateBench;
using LTS.API.Features.Benchs.Commands.DeleteBench;
using LTS.API.Features.Bench.Queries.GetBenchByCase;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LTS.API.Features.Benchs.Queries.GetAllBenchByCase;

namespace LTS.API.Features.Benchs.Controllers;

[ApiController]
[Route("api/bench")]

public class BenchController : ControllerBase
{
    private readonly IMediator _mediator;

    public BenchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST api/bench
    [HttpPost ("createbench")]
    public async Task<IActionResult> Create([FromBody] CreateBenchCommand command)
    {
        var response = await _mediator.Send(command);
        return StatusCode((int)response.Status, response);
    }

    // PUT api/bench
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBenchCommand command)
    {
        var response = await _mediator.Send(command);
        return StatusCode((int)response.Status, response);
    }

    // DELETE api/bench/{benchId}
    [HttpDelete("{benchId}")]
    public async Task<IActionResult> Delete(Guid benchId)
    {
        var response = await _mediator.Send(new DeleteBenchCommand(benchId ));
        return StatusCode((int)response.Status, response);
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCase(Guid caseId)
    {
        var response = await _mediator.Send(new GetBenchByCaseQuery (caseId ));
        return StatusCode((int)response.Status, response);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllBenches(
    [FromQuery] GetAllBenchByCaseQuery request,
    CancellationToken ct)
    {
        var result = await _mediator.Send(request, ct);
        return StatusCode((int)result.Status, result);
    }
}