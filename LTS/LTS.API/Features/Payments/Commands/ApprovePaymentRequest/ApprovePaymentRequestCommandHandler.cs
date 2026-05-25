using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Payments.Commands.ApprovePaymentRequest
{
    public sealed class ApprovePaymentRequestCommandHandler
        : IRequestHandler<ApprovePaymentRequestCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public ApprovePaymentRequestCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(
            ApprovePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.PaymentRequests
                .Include(p => p.Organization)
                .FirstOrDefaultAsync(p => p.Id == request.PaymentRequestId, cancellationToken);

            if (payment is null)
                return ApiResponse<string>.NotFound("Payment request not found");

            if (payment.Status != PaymentStatus.Pending)
                return ApiResponse<string>.Fail("Only pending requests can be approved");

            var plan = payment.RequestedPlan;
            var planDetails = Domain.Constants.PlanConfig.GetPlan(plan);

            // Payment approve
            payment.Status = PaymentStatus.Approved;
            payment.ReviewedAt = DateTime.UtcNow;
            payment.ReviewedBy = request.ReviewedBy;

            // Organization subscription update
            var org = payment.Organization;
            org.Plan = plan;
            org.MaxUsers = planDetails.MaxUsers;
            org.MaxPetitioners = planDetails.MaxPetitioners;
            org.MaxCases = planDetails.MaxCases;
            org.IsSubscriptionActive = true;
            org.IsTrialActive = false;
            org.SubscriptionStartDate = DateTime.UtcNow;
            org.SubscriptionEndDate = DateTime.UtcNow.AddDays(planDetails.DurationDays);
            org.IsActive = true;
            org.UpdatedAt = DateTime.UtcNow;

            // Wallet transaction record
            var walletTx = new Domain.Entities.WalletTransaction
            {
                Id = Guid.NewGuid(),
                Type = WalletTransactionType.Credit,
                Amount = payment.Amount,
                Description = $"{org.OrganizationName} - {plan} plan payment approved",
                PaymentRequestId = payment.Id,
                OrganizationId = org.Id,
                TransactionDate = DateTime.UtcNow,
                RecordedBy = request.ReviewedBy
            };

            _context.WalletTransactions.Add(walletTx);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!,"Payment approved and subscription activated successfully");
        }
    }
}
