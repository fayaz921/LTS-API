using LTS.API.Features.UserManangement.Commands.UpdateProfies;
using LTS.API.Features.UserManangement.Commands.UpdateProfilePictures;
using LTS.API.Features.UserManangement.Queries.GetProfiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.UserManangement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var response = await _mediator.Send(new GetProfileQuery());
            return StatusCode((int)response.Status, response);
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode((int)response.Status, response);
        }

        [HttpPut("UpdateProfilePicture")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
        {
            var response = await _mediator.Send(new UpdateProfilePictureCommand { File = file });
            return StatusCode((int)response.Status, response);
        }
    }
}
