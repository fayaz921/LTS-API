using LTS.API.Features.UserManangement.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.UserManangement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
