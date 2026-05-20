using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.Mappers;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.Email;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<User> _passwordHasher;
        public CreateUserCommandHandler(AppDbContext context, IPasswordHasher<User> passwordHasher, IEmailService emailService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<ApiResponse<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var existingUserMessage = await ExistingUser(request, cancellationToken);
            if (existingUserMessage != null)
                return ApiResponse<string>.Fail(existingUserMessage);

            var otp = GenerateOtp();

            var organization = request.ToOrganization();
            var passwordHash = _passwordHasher.HashPassword(null!, request.Password);
            var user = request.ToUser(organization.Id, passwordHash, otp);

            await _context.Organizations.AddAsync(organization, cancellationToken);
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendRegistrationOtp(request.Email, request.OwnerName, otp);

            return ApiResponse<string>.Ok(default!, "Registration successful. OTP sent to your email.");
        }

        private static string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
        public async Task<string?> ExistingUser(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users
               .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null && existingUser.IsActive)
                return "Email already registered";

            if (existingUser != null && !existingUser.IsActive)
            {
                var newOtp = GenerateOtp();

                existingUser.Otp = newOtp;

                existingUser.OTPExpiry = DateTime.UtcNow.AddMinutes(10);
                var passwordHash = _passwordHasher.HashPassword(null!, request.Password);

                var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.Id == existingUser.OrganizationId, cancellationToken);

                organization!.OrganizationName = request.OrganizationName;

                organization.Slug = request.OrganizationName.ToLower().Trim().Replace(" ", "-");
                
                organization.CreatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                await _emailService.SendRegistrationOtp(request.Email, request.OwnerName, newOtp);

                return  "OTP resent to your email."; 
            }

            return null;
        }
    }
}
