using LTS.API.Domain.Entities;
using LTS.API.Features.Payments.DTOs;

namespace LTS.API.Features.Payments.Mappers
{
    public static class PaymentRequestMapping
    {
        public static PaymentRequestDto ToDto(this PaymentRequest p)
        {
            return new PaymentRequestDto
            {
                Id = p.Id,
                OrganizationId = p.OrganizationId,
                OrganizationName = p.Organization?.OrganizationName ?? string.Empty,
                RequestedPlan = p.RequestedPlan.ToString(),
                TransactionId = p.TransactionId,
                SenderName = p.SenderName,
                SenderPhone = p.SenderPhone,
                PaymentMethod = p.PaymentMethod.ToString(),
                Amount = p.Amount,
                ScreenshotUrl = p.ScreenshotUrl,
                Status = p.Status.ToString(),
                RejectionReason = p.RejectionReason,
                SubmittedAt = p.SubmittedAt,
                ReviewedAt = p.ReviewedAt,
                ReviewedBy = p.ReviewedBy,
                CreatedAt = p.CreatedAt
            };
        }
    }
}
