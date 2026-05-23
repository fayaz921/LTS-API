using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Entities
{
    public class PaymentRequest:BaseEntity
    {
        public SubscriptionPlan RequestedPlan { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderPhone { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string ScreenshotUrl { get; set; } = string.Empty;
        public string ScreenshotPublicId { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? RejectionReason { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }

        // Navigation
        public Organization Organization { get; set; } = null!;
    }
}
