using LTS.API.Domain.Enums;
using LTS.API.Features.Wallets.Commands.AddDebits;
using LTS.API.Features.Wallets.Queries.GetWalletStats;
using LTS.API.Features.Wallets.Queries.GetWalletTransactions;
using LTS.API.Infrastructure.Services.CurrentUserServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.Wallets.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public WalletsController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _mediator.Send(new GetWalletStatsQuery());
            return Ok(result);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] WalletTransactionType? type = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _mediator.Send(new GetWalletTransactionsQuery(
                pageNumber, pageSize, type, fromDate, toDate));
            return Ok(result);
        }

        [HttpPost("debit")]
        public async Task<IActionResult> AddDebit([FromBody] AddDebitRequest request)
        {
            var command = new AddDebitCommand(
                Amount: request.Amount,
                Description: request.Description,
                RecordedBy: _currentUser.UserId.ToString()
            );

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }

    public record AddDebitRequest(
        decimal Amount,
        string Description
    );
}
