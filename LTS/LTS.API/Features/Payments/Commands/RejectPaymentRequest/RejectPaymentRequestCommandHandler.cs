using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace LTS.API.Features.Payments.Commands.RejectPaymentRequest
{
    public sealed class RejectPaymentRequestCommandHandler
        : IRequestHandler<RejectPaymentRequestCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;

        public RejectPaymentRequestCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> Handle(
            RejectPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.PaymentRequests
                .FirstOrDefaultAsync(p => p.Id == request.PaymentRequestId, cancellationToken);

            if (payment is null)
                return ApiResponse<string>.NotFound("Payment request not found");

            if (payment.Status != PaymentStatus.Pending)
                return ApiResponse<string>.Fail("Only pending requests can be rejected");

            payment.Status = PaymentStatus.Rejected;
            payment.RejectionReason = request.RejectionReason;
            payment.ReviewedAt = DateTime.UtcNow;
            payment.ReviewedBy = request.ReviewedBy;
            payment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!,"Payment request rejected successfully");
        }
    }
    
    }
