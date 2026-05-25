using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.Wallets.DTOs;

namespace LTS.API.Features.Wallets.Mappers
{
    public static class WalletMappingExtensions
    {
        public static WalletTransactionDto ToDto(
            this WalletTransaction transaction,
            string? organizationName = null)
        {
            return new WalletTransactionDto(
                Id: transaction.Id,
                Type: transaction.Type.ToString(),
                Amount: transaction.Amount,
                Description: transaction.Description,
                OrganizationName: organizationName,
                TransactionDate: transaction.TransactionDate,
                RecordedBy: transaction.RecordedBy
            );
        }
        public static WalletStatsDto ToStatsDto(
            this List<WalletTransaction> transactions,
            int approvedCount,
            int pendingCount,
            int rejectedCount)
        {
            var now = DateTime.UtcNow;
            var thisMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);

            var totalRevenue = transactions
                .Where(t => t.Type == WalletTransactionType.Credit)
                .Sum(t => t.Amount);

            var thisMonthRevenue = transactions
                .Where(t => t.Type == WalletTransactionType.Credit
                         && t.TransactionDate >= thisMonthStart)
                .Sum(t => t.Amount);

            var lastMonthRevenue = transactions
                .Where(t => t.Type == WalletTransactionType.Credit
                         && t.TransactionDate >= lastMonthStart
                         && t.TransactionDate < thisMonthStart)
                .Sum(t => t.Amount);

            var totalDebits = transactions
                .Where(t => t.Type == WalletTransactionType.Debit)
                .Sum(t => t.Amount);

            return new WalletStatsDto(
                TotalRevenue: totalRevenue,
                ThisMonthRevenue: thisMonthRevenue,
                LastMonthRevenue: lastMonthRevenue,
                TotalDebits: totalDebits,
                CurrentBalance: totalRevenue - totalDebits,
                TotalTransactions: transactions.Count,
                TotalApprovedPayments: approvedCount,
                TotalPendingPayments: pendingCount,
                TotalRejectedPayments: rejectedCount
            );
        }
    }
}
