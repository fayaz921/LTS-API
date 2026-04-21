using LTS.API.Features.Petitioners.Commands.CreatePetitioner;
using LTS.API.Features.Petitioners.Commands.DeletePetitioner;
using LTS.API.Features.Petitioners.Commands.UpdatePetitioner;
using LTS.API.Features.Petitioners.Queries.GetAllPetitioners;
using LTS.API.Features.Petitioners.Queries.GetPetitionerById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Petitioners.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PetitionersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PetitionersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("CreatePetitioner")]
        public async Task<IActionResult> CreatePetitioner([FromBody]CreatePetitionerCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode((int)response.Status, response);
        }
        [HttpPut("UpdatePetitioner/{id}")]
        public async Task<IActionResult> UpdatePetitioner(Guid id, [FromBody] UpdatePetitionerCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode((int)response.Status, response);
        }

        [HttpDelete("DeletePetitioner/{id}")]
        public async Task<IActionResult> DeletePetitioner(Guid id)
        {
            var response = await _mediator.Send(new DeletePetitionerCommand(id));
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("GetPetitionerById/{id}")]
        public async Task<IActionResult> GetPetitionerById(Guid id)
        {
            var response = await _mediator.Send(new GetPetitionerByIdQuery(id));
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("GetAllPetitioners")]
        public async Task<IActionResult> GetAllPetitioners([FromQuery] Guid organizationId)
        {
            var response = await _mediator.Send(new GetAllPetitionersQuery(organizationId));
            return StatusCode((int)response.Status, response);
        }

    }
}
