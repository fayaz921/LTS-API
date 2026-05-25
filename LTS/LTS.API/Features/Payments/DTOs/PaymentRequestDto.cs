namespace LTS.API.Features.Payments.DTOs
{
    public class PaymentRequestDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;

        public string RequestedPlan { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderPhone { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string ScreenshotUrl { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }

        public DateTime SubmittedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
