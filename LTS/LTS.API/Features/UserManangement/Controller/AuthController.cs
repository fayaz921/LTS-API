using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.Commands.Authentication.ConfirmOTP;
using LTS.API.Features.UserManangement.Commands.Authentication.CreateUser;
using LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword;
using LTS.API.Features.UserManangement.Commands.Authentication.LoginUser;
using LTS.API.Features.UserManangement.Commands.Authentication.RefreshTokens;
using LTS.API.Features.UserManangement.Commands.Authentication.VerifyEmail;
using LTS.API.Features.UserManangement.Logouts;
using LTS.API.Features.UserManangement.Queries.GetMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.UserManangement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;
        public AuthController(ILogger<AuthController> logger, IMediator mediator)
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

            if (!response.IsSuccess)
                return StatusCode((int)response.Status, response);
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
        [HttpPost("ConfirmOtp")]
        public async Task<IActionResult> ConfirmOtp(VerifyOtpCommand confirmOtpCommand)
        {
            var response = await _mediator.Send(confirmOtpCommand);
            return StatusCode((int)response.Status, response);

        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = await _mediator.Send(new RefreshTokenCommand());
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _mediator.Send(new LogoutCommand());
            return StatusCode((int)response.Status, response);
        }
        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var response = await _mediator.Send(new GetMeQuery());
            return StatusCode((int)response.Status, response);

        }
    }
}
