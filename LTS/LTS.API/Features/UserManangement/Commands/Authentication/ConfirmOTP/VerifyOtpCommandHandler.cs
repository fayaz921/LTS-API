using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.ConfirmOTP
{
    public class VerifyOtpCommandHandler:IRequestHandler<VerifyOtpCommand, ApiResponse<string>>
     {
        private readonly AppDbContext _context;
        public VerifyOtpCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<string>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
        var user = await _context.Users
            .Include(u => u.Organization)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
                return ApiResponse<string>.Fail("User not found");

            if (user.IsActive)
                return ApiResponse<string>.Fail("Email already verified");

            if (user.Otp != request.Otp)
                return ApiResponse<string>.Fail("Invalid OTP");

            if (user.OTPExpiry<DateTime.UtcNow)
                return ApiResponse<string>.Fail("OTP expired. Please register again.");

            user.IsActive = true;
            user.Otp = string.Empty;
            user.OTPExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            user.Organization.IsActive = true;
            user.Organization.IsTrialActive = true;
            user.Organization.TrialStartDate = DateTime.UtcNow;
            user.Organization.TrialEndDate = DateTime.UtcNow.AddDays(7);
            user.Organization.UpdatedAt = DateTime.UtcNow;


            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Ok(default!, "Email verified successfully. You can now login.");
        }
}
}
