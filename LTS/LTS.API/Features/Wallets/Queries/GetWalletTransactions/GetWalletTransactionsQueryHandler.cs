using LTS.API.Common.Response;
using LTS.API.Features.Wallets.DTOs;
using LTS.API.Features.Wallets.Mappers;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Wallets.Queries.GetWalletTransactions
{
    public class GetWalletTransactionsQueryHandler : IRequestHandler<GetWalletTransactionsQuery, ApiResponse<PaginatedResponse<WalletTransactionDto>>>
    {
        private readonly AppDbContext _context;

        public GetWalletTransactionsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedResponse<WalletTransactionDto>>> Handle(
            GetWalletTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.WalletTransactions
                .AsNoTracking()
                .AsQueryable();

            if (request.Type.HasValue)
                query = query.Where(t => t.Type == request.Type.Value);

            if (request.FromDate.HasValue)
                query = query.Where(t => t.TransactionDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(t => t.TransactionDate <= request.ToDate.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var transactions = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var orgIds = transactions
                .Where(t => t.OrganizationId.HasValue)
                .Select(t => t.OrganizationId!.Value)
                .Distinct()
                .ToList();

            var orgNames = await _context.Organizations
                .AsNoTracking()
                .Where(o => orgIds.Contains(o.Id))
                .Select(o => new { o.Id, o.OrganizationName })
                .ToDictionaryAsync(o => o.Id, o => o.OrganizationName, cancellationToken);

            // Mapper use karo
            var items = transactions
                .Select(t => t.ToDto(
                    t.OrganizationId.HasValue
                        ? orgNames.GetValueOrDefault(t.OrganizationId.Value)
                        : null))
                .ToList();

            var paginated = PaginatedResponse<WalletTransactionDto>.Create(
                items, totalCount, request.PageNumber, request.PageSize);

            return ApiResponse<PaginatedResponse<WalletTransactionDto>>.Ok(
                paginated, "Wallet transactions fetched successfully");
        }
    }
    }
