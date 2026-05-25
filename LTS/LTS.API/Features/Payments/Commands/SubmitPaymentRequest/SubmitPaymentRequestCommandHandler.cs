using LTS.API.Common.Response;
using LTS.API.Domain.Constants;
using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.Payments.Commands.SubmitPaymentRequest
{
    public class SubmitPaymentRequestCommandHandler : IRequestHandler<SubmitPaymentRequestCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinary;

        public SubmitPaymentRequestCommandHandler(
            AppDbContext context,
            ICloudinaryService cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        public async Task<ApiResponse<string>> Handle(
            SubmitPaymentRequestCommand request, CancellationToken cancellationToken)
        {
           
            var org = await _context.Organizations
                .FirstOrDefaultAsync(o => o.Id == request.OrganizationId, cancellationToken);

            if (org is null)
                return ApiResponse<string>.NotFound("Organization not found");

            if (org.IsBlocked)
                return ApiResponse<string>.Fail("Organization is blocked");

            // Duplicate pending request check
            var hasPending = await _context.PaymentRequests
                .AnyAsync(p =>
                    p.OrganizationId == request.OrganizationId &&
                    p.Status == PaymentStatus.Pending,
                    cancellationToken);

            if (hasPending)
                return ApiResponse<string>.Fail("A pending payment request already exists");

            // Plan amount validate karo
            var planDetails = PlanConfig.GetPlan(request.RequestedPlan);
            if (request.Amount != planDetails.Price)
                return ApiResponse<string>.Fail(
                    $"Invalid amount. Expected: {planDetails.Price} for {request.RequestedPlan} plan");

            // Screenshot upload
            var uploadResult = await _cloudinary.UploadFileAsync(request.Screenshot);
            if (!uploadResult.IsSuccess)
                return ApiResponse<string>.Fail($"Screenshot upload failed: {uploadResult.Message}");

            // PaymentRequest create
            var paymentRequest = new PaymentRequest
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                RequestedPlan = request.RequestedPlan,
                TransactionId = request.TransactionId,
                SenderName = request.SenderName,
                SenderPhone = request.SenderPhone,
                PaymentMethod = request.PaymentMethod,
                Amount = request.Amount,
                ScreenshotUrl = uploadResult.Url,
                ScreenshotPublicId = uploadResult.PublicId,
                Status = PaymentStatus.Pending,
                SubmittedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = org.OrganizationName
            };

            _context.PaymentRequests.Add(paymentRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!,"Payment request submitted successfully");
        }
    }
}
