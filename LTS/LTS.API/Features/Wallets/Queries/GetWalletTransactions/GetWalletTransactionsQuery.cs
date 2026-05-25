using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.Wallets.DTOs;
using MediatR;

namespace LTS.API.Features.Wallets.Queries.GetWalletTransactions
{
    public sealed record GetWalletTransactionsQuery(
         int PageNumber = 1,
         int PageSize = 10,
         WalletTransactionType? Type = null,
         DateTime? FromDate = null,
         DateTime? ToDate = null
     ) : IRequest<ApiResponse<PaginatedResponse<WalletTransactionDto>>>;
}
