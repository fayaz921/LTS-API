namespace LTS.API.Features.Wallets.DTOs
{
    public record WalletStatsDto(
         decimal TotalRevenue,
         decimal ThisMonthRevenue,
         decimal LastMonthRevenue,
         decimal TotalDebits,
         decimal CurrentBalance,
         int TotalTransactions,
         int TotalApprovedPayments,
         int TotalPendingPayments,
         int TotalRejectedPayments
     );
}
