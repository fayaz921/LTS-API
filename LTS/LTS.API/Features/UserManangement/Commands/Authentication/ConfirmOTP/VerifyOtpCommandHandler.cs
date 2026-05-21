using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.ConfirmOTP
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
        public VerifyOtpCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<string>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u=>u.Email==request.Email)
                .Select(u => new
            {
                u.Id,
                u.IsActive,
                u.Otp,
                u.OTPExpiry,
                u.OrganizationId
            }).FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return ApiResponse<string>.Fail("User not found");

            if (user.IsActive)
                return ApiResponse<string>.Fail("Email already verified");

            if (string.IsNullOrWhiteSpace(user.Otp) || user.Otp.Trim() != request.Otp.Trim())
                return ApiResponse<string>.Fail("Invalid OTP");

            if (user.OTPExpiry == null || user.OTPExpiry < DateTime.UtcNow)
                return ApiResponse<string>.Fail("OTP expired. Please request a new one.");
            await using var transaction =
                            await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // USER ACTIVATE
                await _context.Users
                    .Where(x => x.Id == user.Id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(x => x.IsActive, true)
                        .SetProperty(x => x.Otp, string.Empty)
                        .SetProperty(x => x.OTPExpiry, (DateTime?)null)
                        .SetProperty(x => x.UpdatedAt, DateTime.UtcNow),
                        cancellationToken);

                // ORGANIZATION ACTIVATE 
                var affectedRows = await _context.Organizations
                    .Where(x => x.Id == user.OrganizationId)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(x => x.IsActive, true)
                        .SetProperty(x => x.IsTrialActive, true)
                        .SetProperty(x => x.TrialStartDate, DateTime.UtcNow)
                        .SetProperty(x => x.TrialEndDate, DateTime.UtcNow.AddDays(7))
                        .SetProperty(x => x.UpdatedAt, DateTime.UtcNow),
                        cancellationToken);

                // ORGANIZATION NOT FOUND
                if (affectedRows == 0)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return ApiResponse<string>.Fail("Organization not found");
                }

                await transaction.CommitAsync(cancellationToken);

                return ApiResponse<string>.Ok(
                    default!,
                    "Email verified successfully. You can now login.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return ApiResponse<string>.Fail("Something went wrong.");
            }
        }
    }
}
