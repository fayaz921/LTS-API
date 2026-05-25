namespace LTS.API.Features.Wallets.DTOs
{
    public record WalletTransactionDto(
         Guid Id,
         string Type,
         decimal Amount,
         string Description,
         string? OrganizationName,
         DateTime TransactionDate,
         string RecordedBy
     );
}
