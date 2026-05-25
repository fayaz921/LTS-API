using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Wallets.Commands.AddDebits
{
    public sealed record AddDebitCommand(
          decimal Amount,
          string Description,
          string RecordedBy
      ) : IRequest<ApiResponse<string>>;
}
