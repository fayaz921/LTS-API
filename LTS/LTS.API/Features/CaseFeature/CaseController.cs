using LTS.API.Features.CaseFeature.Commands.CreateCase;
using LTS.API.Features.CaseFeature.Commands.DeleteCase;
using LTS.API.Features.CaseFeature.Commands.UpdateCase;
using LTS.API.Features.CaseFeature.Queries.GetById;
using LTS.API.Features.CaseFeature.Queries.GetCases;
using LTS.API.Features.CaseFeature.Queries.SearchCases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.CaseFeature
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateCase(CreateCaseCommand request, CancellationToken ct)
        {
            request.OrganizationId = Guid.Parse("8f2d5e1a-c4b3-4927-90a6-7f8e3b1d5c4a");
            var result = await _mediator.Send(request, ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCase(UpdateCaseCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteCaseCommand(id), ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCases(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllCasesQuery());
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaseById(Guid id,CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCaseByIdQuery(id)); 
            return StatusCode((int)result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> SearchCases(SearchCasesQuery request, CancellationToken ct)
        {
            request.OrganizationId = Guid.Parse("8f2d5e1a-c4b3-4927-90a6-7f8e3b1d5c4a"); // later we update it 
            var result = await _mediator.Send(request, ct);
            return StatusCode((int)result.Status, result);
        }
    }
}
