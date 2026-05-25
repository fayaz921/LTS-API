using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Features.Wallets.DTOs;
using LTS.API.Features.Wallets.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Wallets.Queries.GetWalletStats
{
    public class GetWalletStatsQueryHandler : IRequestHandler<GetWalletStatsQuery, ApiResponse<WalletStatsDto>>
    {
        private readonly AppDbContext _context;

        public GetWalletStatsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<WalletStatsDto>> Handle(
            GetWalletStatsQuery request,
            CancellationToken cancellationToken)
        {
            var transactions = await _context.WalletTransactions
        .AsNoTracking()
        .ToListAsync(cancellationToken);

            var paymentStats = await _context.PaymentRequests
                .AsNoTracking()
                .GroupBy(p => p.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var dto = transactions.ToStatsDto(
                approvedCount: paymentStats
                    .FirstOrDefault(p => p.Status == PaymentStatus.Approved)?.Count ?? 0,
                pendingCount: paymentStats
                    .FirstOrDefault(p => p.Status == PaymentStatus.Pending)?.Count ?? 0,
                rejectedCount: paymentStats
                    .FirstOrDefault(p => p.Status == PaymentStatus.Rejected)?.Count ?? 0
            );

            return ApiResponse<WalletStatsDto>.Ok(dto, "Wallet stats fetched successfully");
        }
    }
    }
