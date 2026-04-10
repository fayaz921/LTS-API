using LTS.API.Features.UserManangement.Commands.Authentication.CreateUser;
using LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword;
using LTS.API.Features.UserManangement.Commands.Authentication.LoginUser;
using LTS.API.Features.UserManangement.Commands.Authentication.VerifyEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.UserManangement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthentiationController : ControllerBase
    {
        private readonly ILogger<AuthentiationController> _logger;
        private readonly IMediator _mediator;
        public AuthentiationController(ILogger<AuthentiationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> CreateUser(CreateUserCommand createUserCommand)
        {
            var response = await _mediator.Send(createUserCommand);
            return StatusCode((int)response.Status, response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
        {
            var response = await _mediator.Send(loginUserCommand);
            return StatusCode((int)response.Status, response);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand forgetPasswordCommand)
        {
            var response = await _mediator.Send(forgetPasswordCommand);
            return StatusCode((int)response.Status, response);
        }
        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand verifyEmailCommand)
        {
            var response = await _mediator.Send(verifyEmailCommand);
            return StatusCode((int)response.Status, response);

        }
    }
}
