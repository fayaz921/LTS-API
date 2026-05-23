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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCase(CreateCaseCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCase(UpdateCaseCommand request, CancellationToken ct)
        {
            var result = await _mediator.Send(request, ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCase(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteCaseCommand(id), ct);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllCases(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetAllCasesQuery
            {
                OrganizationId = Guid.Parse(User.FindFirst("OrganizationId")?.Value!),
                Page = page,
                PageSize = pageSize
            });
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCaseById(Guid id,CancellationToken ct)
        {
            var result = await _mediator.Send(new GetCaseByIdQuery(id)); 
            return StatusCode((int)result.Status, result);
        }

        /// <summary>
        /// Search cases by CaseNo, Title, or Petitioner CNIC
        /// </summary>
        /// <param name="query">Search term (CaseNo, Title, or CNIC)</param>
        /// <param name="pageNumber">Page number (default 1)</param>
        /// <param name="pageSize">Records per page (default 20)</param>
        /// <param name="status">Filter by status (Pending/Finalized)</param>
        /// <param name="fromDate">Filter from date</param>
        /// <param name="toDate">Filter to date</param>
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? query,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? status = null,
            [FromQuery] string? fromDate = null,
            [FromQuery] string? toDate = null)
        {
            var searchQuery = new SearchCasesQuery
            {
                SearchTerm = query,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status,
                DateFrom = string.IsNullOrEmpty(fromDate) ? null : DateTime.Parse(fromDate),
                DateTo = string.IsNullOrEmpty(toDate) ? null : DateTime.Parse(toDate),
                OrganizationId= Guid.Parse(User.FindFirst("OrganizationId")?.Value!)
            };

            var result = await _mediator.Send(searchQuery);
            return StatusCode((int)result.Status, result);
        }
    }
}

