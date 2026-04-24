using LTS.API.Common.OTPGenerators;
using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LTS.API.Features.UserManangement.Commands.Authentication.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ApiResponse<string>>
    {
        private readonly ILogger<ForgetPasswordCommandHandler> _logger;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        public ForgetPasswordCommandHandler(AppDbContext context, IEmailService emailService, ILogger<ForgetPasswordCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }
        public async Task<ApiResponse<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user is null)
                return ApiResponse<string>.Fail("Email not found", HttpStatusCode.NotFound);

            var otp = GeneraterOtp.GenerateOtp();
            user.Otp = otp;
            user.OTPExpiry = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync(cancellationToken);

            var emailSent = await _emailService.ForgetPasswordOtp(user.Email, user.Name, otp);

            if (!emailSent)
            {
                _logger.LogError("Failed to send OTP email to {Email}", user.Email);
                return ApiResponse<string>.Fail("Failed to send OTP email. Please try again.");
            }

            _logger.LogInformation("OTP sent successfully to {Email}", user.Email);
            return ApiResponse<string>.Ok(default!, "OTP sent to your email. Valid for 10 minutes.");
        }
    }
}
