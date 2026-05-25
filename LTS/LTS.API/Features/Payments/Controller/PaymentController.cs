using LTS.API.Domain.Enums;
using LTS.API.Features.Payments.Commands.ApprovePaymentRequest;
using LTS.API.Features.Payments.Commands.RejectPaymentRequest;
using LTS.API.Features.Payments.Commands.SubmitPaymentRequest;
using LTS.API.Features.Payments.Queries.GetAllPaymentRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Payments.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("submit")]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Submit([FromForm] SubmitPaymentRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode((int)result.Status, result);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] PaymentStatus? status = null)
        {
            var result = await _mediator.Send(
                new GetAllPaymentRequestsQuery(page, pageSize, search, status));
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("/approve")]
        [AllowAnonymous]
        public async Task<IActionResult> Approve(Guid id, [FromQuery] string reviewedBy = "SuperAdmin")
        {
            var result = await _mediator.Send(
                new ApprovePaymentRequestCommand(id, reviewedBy));
            return StatusCode((int)result.Status, result);
        }

        [HttpPut("/reject")]
        [AllowAnonymous]
        public async Task<IActionResult> Reject(Guid id, [FromBody] RejectPaymentRequestCommand command)
        {
            var actualCommand = command with { PaymentRequestId = id };
            var result = await _mediator.Send(actualCommand);
            return StatusCode((int)result.Status, result);
        }
    }
}
