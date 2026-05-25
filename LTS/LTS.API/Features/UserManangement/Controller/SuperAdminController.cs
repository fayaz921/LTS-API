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
    //[Authorize(Roles = nameof(UserRole.SuperAdmin))]

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 10,
          [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetAllOrganizationsQuery(page, pageSize, Search: search));
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("trial")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTrialOrganizations(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetAllOrganizationsQuery(page, pageSize, Search: search, IsTrialActive: true));
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("subscribed")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubscribedOrganizations(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetAllOrganizationsQuery(page, pageSize, Search: search, IsSubscriptionActive: true));
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("blocked")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlockedOrganizations(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetAllOrganizationsQuery(page, pageSize, Search: search, IsBlocked: true));
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("expired")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExpiredOrganizations(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _mediator.Send(new GetAllOrganizationsQuery(page, pageSize, Search: search, IsSubscriptionActive: false, IsActive: false));
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetOrganizationByIdQuery { OrganizationId = id });
            return StatusCode((int)result.Status, result);
        }
    }
}
