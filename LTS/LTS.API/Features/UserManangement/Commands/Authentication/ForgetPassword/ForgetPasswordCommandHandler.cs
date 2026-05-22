using Hangfire;
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
        public ForgetPasswordCommandHandler(AppDbContext context,ILogger<ForgetPasswordCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ApiResponse<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            var user = await _context.Users
                .AsNoTracking().Where(u => u.Email == request.Email)
                .Select(x => new { x.Id,x.Email, x.Name })
                .FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                return ApiResponse<string>.Fail("Email not found", HttpStatusCode.NotFound);

            var otp = GeneraterOtp.GenerateOtp();
            await _context.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(u => u.SetProperty(x => x.Otp, otp)
                                        .SetProperty(x => x.OTPExpiry, utcNow.AddMinutes(10))
                                        .SetProperty(x => x.UpdatedAt, utcNow), cancellationToken);

            BackgroundJob.Enqueue<IEmailService>(x=>
            x.ForgetPasswordOtp(user.Email, user.Name, otp));
            _logger.LogInformation("OTP sent successfully to {Email}", user.Email);
            return ApiResponse<string>.Ok(default!, "OTP sent to your email. Valid for 10 minutes.");
        }
    }
}
