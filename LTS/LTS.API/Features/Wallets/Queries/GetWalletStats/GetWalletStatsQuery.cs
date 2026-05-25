using LTS.API.Common.Response;
using LTS.API.Features.Wallets.DTOs;
using MediatR;

namespace LTS.API.Features.Wallets.Queries.GetWalletStats
{
    public sealed record GetWalletStatsQuery
        : IRequest<ApiResponse<WalletStatsDto>>;
}
