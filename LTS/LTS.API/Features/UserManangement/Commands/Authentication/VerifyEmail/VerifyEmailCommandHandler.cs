using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.UserManangement.Commands.Authentication.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<VerifyEmailCommandHandler> _logger;

        public VerifyEmailCommandHandler(AppDbContext context,IPasswordHasher<User> passwordHasher,ILogger<VerifyEmailCommandHandler> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            var user = await _context.Users
                .AsNoTracking()
                .Where(x => x.Email == request.Email)
                .Select(x => new
                {
                    x.Id,
                    x.Otp,
                    x.OTPExpiry
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null ||
            string.IsNullOrWhiteSpace(user.Otp) ||
            user.Otp.Trim() != request.Otp.Trim())
            {
                return ApiResponse<string>.Fail("Invalid email or OTP");
            }
            if (user.OTPExpiry is null || utcNow > user.OTPExpiry)
                return ApiResponse<string>.Fail("OTP has expired. Please request a new one.", HttpStatusCode.BadRequest);

            var  passwordHash = _passwordHasher.HashPassword(null!, request.NewPassword);
            await _context.Users
            .Where(x => x.Id == user.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.PasswordHash, passwordHash)
                .SetProperty(x => x.Otp, string.Empty)
                .SetProperty(x => x.OTPExpiry, (DateTime?)null)
                .SetProperty(x => x.UpdatedAt, utcNow),
                cancellationToken);
            await _context.RefreshTokens
              .Where(x => x.UserId == user.Id && !x.IsRevoked)
              .ExecuteUpdateAsync(s => s
                  .SetProperty(x => x.IsRevoked, true),
                  cancellationToken);
            _logger.LogInformation("Password reset successful for {Email}", request.Email);
            return ApiResponse<string>.Ok(default!, "Password reset successful. You can now login.");
        }
    }
}
