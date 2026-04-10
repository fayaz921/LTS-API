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
            // 1. User dhundo email se
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user is null)
                return ApiResponse<string>.Fail("Email not found", HttpStatusCode.NotFound);

            // 2. OTP sahi hai?
            if (user.Otp != request.Otp)
                return ApiResponse<string>.Fail("Invalid OTP", HttpStatusCode.BadRequest);

            // 3. OTP expire toh nahi hua?
            if (user.OTPExpiry is null || DateTime.UtcNow > user.OTPExpiry)
                return ApiResponse<string>.Fail("OTP has expired. Please request a new one.", HttpStatusCode.BadRequest);

            // 4. Naya password hash karo aur save karo
            user.PasswordHash = _passwordHasher.HashPassword(null!, request.NewPassword);

            // 5. OTP clear karo — ek baar use ho gaya
            user.Otp = string.Empty;
            user.OTPExpiry = null;

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Password reset successful for {Email}", user.Email);
            return ApiResponse<string>.Ok(default!, "Password reset successful. You can now login.");
        }
    }
}
