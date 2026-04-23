using LTS.API.Features.Reports.Queries.GetCourtWiseReport;
using LTS.API.Features.Reports.Queries.GetDepartmentWiseReport;
using LTS.API.Features.Reports.Queries.GetSummaryReport;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("DepartmentWiseReport")]
        public async Task<IActionResult> GetDepartmentWiseReport()
        {
            var organizationId = Guid.Parse("8f2d5e1a-c4b3-4927-90a6-7f8e3b1d5c4a"); // later we update it
            var result = await _mediator.Send(new GetDepartmentWiseReportQuery { OrganizationId = organizationId });
            return StatusCode((int)result.Status, result);
        }
        [HttpGet]
        [Route("CourtWiseReport")]
        public async Task<IActionResult> GetCourtWiseReport()
        {
            var result = await _mediator.Send(new GetCourtWiseReportQuery());
            return StatusCode((int)result.Status, result);
        }
        [HttpGet]
        [Route("SummaryReport")]
        public async Task<IActionResult> GetSummaryReport()
        {
            var organizationId = Guid.Parse("8f2d5e1a-c4b3-4927-90a6-7f8e3b1d5c4a"); // later we update it
            var result = await _mediator.Send(new GetSummaryReportQuery { OrganizationId = organizationId });
            return StatusCode((int)result.Status, result);
        }
    }
}
