using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Wallets.Commands.AddDebits
{
    public class AddDebitCommandHandler : IRequestHandler<AddDebitCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public AddDebitCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(
            AddDebitCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return ApiResponse<string>.Fail("Amount zero ya negative nahi ho sakta");

            // Balance check karo
            var totalCredit = await _context.WalletTransactions
                .Where(t => t.Type == WalletTransactionType.Credit)
                .SumAsync(t => t.Amount, cancellationToken);

            var totalDebit = await _context.WalletTransactions
                .Where(t => t.Type == WalletTransactionType.Debit)
                .SumAsync(t => t.Amount, cancellationToken);

            var currentBalance = totalCredit - totalDebit;

            if (request.Amount > currentBalance)
                return ApiResponse<string>.Fail(
                    $"Insufficient balance. Current balance: Rs. {currentBalance}");

            var debit = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                Type = WalletTransactionType.Debit,
                Amount = request.Amount,
                Description = request.Description,
                TransactionDate = DateTime.UtcNow,
                RecordedBy = request.RecordedBy
            };

            _context.WalletTransactions.Add(debit);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!,
                $"Rs. {request.Amount} debit successfully recorded");
        }
    }
}
