using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Queries.GetAllOrganizations;
using LTS.API.Features.UserManangement.Queries.GetDashboardStats;
using LTS.API.Features.UserManangement.Queries.GetOrganizationById;
using LTS.API.Features.UserManangement.Queries.GetSubscriptionOrganizations;
using LTS.API.Features.UserManangement.Queries.GetTrialOrganizations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.UserManangement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.SuperAdmin))]

    public class SuperAdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SuperAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _mediator.Send(new GetDashboardStatsQuery());
            return StatusCode((int)result.Status, result);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrganizationsQuery());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("trial")]
        public async Task<IActionResult> GetTrialOrganizations()
        {
            var result = await _mediator.Send(new GetTrialOrganizationsQuery());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("subscription")]
        public async Task<IActionResult> GetSubscriptionOrganizations()
        {
            var result = await _mediator.Send(new GetSubscriptionOrganizationsQuery());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetOrganizationByIdQuery { OrganizationId = id });
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
