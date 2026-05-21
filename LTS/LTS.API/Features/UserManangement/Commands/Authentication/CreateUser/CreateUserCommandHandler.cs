using Hangfire;
using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.Email;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
 
        private readonly IPasswordHasher<User> _passwordHasher;
        public CreateUserCommandHandler(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<string>> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            // LIGHTWEIGHT QUERY —
            var existingUser = await _context.Users
                .AsNoTracking()
                .Where(u => u.Email == request.Email)
                .Select(u => new
                {
                    u.Id,
                    u.IsActive,
                    u.OrganizationId
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (existingUser is not null && existingUser.IsActive)
                return ApiResponse<string>.Fail("Email already registered.");

            // CASE 2: Inactive user — OTP resend
            if (existingUser is not null && !existingUser.IsActive)
                return await HandleInactiveUserAsync(request, existingUser.Id, existingUser.OrganizationId, cancellationToken);

            // CASE 3: Brand new user
            return await HandleNewUserAsync(request, cancellationToken);
        }

        //   INACTIVE USER FLOW  

        private async Task<ApiResponse<string>> HandleInactiveUserAsync(
            CreateUserCommand request,
            Guid userId,
            Guid organizationId,
            CancellationToken cancellationToken)
        {
            var newOtp = GenerateOtp();

            // Direct SQL UPDATE — no entity load needed
            await _context.Users
                .Where(x => x.Id == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.Otp, newOtp)
                    .SetProperty(x => x.OTPExpiry, DateTime.UtcNow.AddMinutes(10))
                    .SetProperty(x => x.PasswordHash, _passwordHasher.HashPassword(null!, request.Password)),
                    cancellationToken);

            await _context.Organizations
                .Where(x => x.Id == organizationId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.OrganizationName, request.OrganizationName)
                    .SetProperty(x => x.Slug, GenerateSlug(request.OrganizationName))
                    .SetProperty(x => x.CreatedAt, DateTime.UtcNow),
                    cancellationToken);

            // Hangfire — reliable background email
            BackgroundJob.Enqueue<IEmailService>(x =>
                x.SendRegistrationOtp(request.Email, request.OwnerName, newOtp));

            return ApiResponse<string>.Ok(default!, "OTP resent to your email.");
        }

        // ─── New user creation

        private async Task<ApiResponse<string>> HandleNewUserAsync(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var otp = GenerateOtp();
            var organization = (request.OrganizationName).ToOrganization();
            var passwordHash = _passwordHasher.HashPassword(null!, request.Password);
            var user = request.ToUser(organization.Id, passwordHash, otp);
            await using var transaction =
                await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await _context.Organizations.AddAsync(organization, cancellationToken);
                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // Hangfire — reliable background email
                BackgroundJob.Enqueue<IEmailService>(x =>
                    x.SendRegistrationOtp(request.Email, request.OwnerName, otp));

                return ApiResponse<string>.Ok(
                    default!,
                    "Registration successful. OTP sent to your email.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                return ApiResponse<string>.Fail("Something went wrong. Please try again.");
            }
        }

        //Helpers methods 

        private static string GenerateOtp() =>
            RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        private static string GenerateSlug(string text) =>
            text.Trim().ToLower().Replace(" ", "-");
    }
}
