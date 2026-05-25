using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }
        public WalletTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid? PaymentRequestId { get; set; }
        public Guid? OrganizationId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string RecordedBy { get; set; } = string.Empty;

        public PaymentRequest? PaymentRequest { get; set; }
    }
}
