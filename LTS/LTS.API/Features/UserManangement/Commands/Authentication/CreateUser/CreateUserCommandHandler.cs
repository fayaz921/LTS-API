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
                public CreateUserCommandHandler(AppDbContext context, IPasswordHasher<User> passwordHasher,IEmailService emailService)
                {
                    _context = context;
                    _passwordHasher = passwordHasher;
                    _emailService = emailService;
                }

                public async Task<ApiResponse<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
                {
                    // ✅ 1. Email already registered check (verified user)
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

                    if (existingUser != null && existingUser.IsActive)
                        return ApiResponse<string>.Fail("Email already registered");

                    // ✅ 2. Unverified user already exists — resend OTP
                    if (existingUser != null && !existingUser.IsActive)
                    {
                        var newOtp = GenerateOtp();
                        existingUser.Otp = newOtp;
                        existingUser.OTPExpiry = DateTime.UtcNow.AddMinutes(10);
                        await _context.SaveChangesAsync(cancellationToken);
                        await _emailService.SendRegistrationOtp(request.Email, request.OwnerName, newOtp);
                        return ApiResponse<string>.Ok(default!, "OTP resent to your email.");
                    }

                    // ✅ 3. OTP generate karo
                    var otp = GenerateOtp();

                    // ✅ 4. Organization aur User banao
                    var organization = request.ToOrganization();
                    var passwordHash = _passwordHasher.HashPassword(null!, request.Password);
                    var user = request.ToUser(organization.Id, passwordHash, otp);

                    // ✅ 5. DB mein save karo
                    await _context.Organizations.AddAsync(organization, cancellationToken);
                    await _context.Users.AddAsync(user, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    // ✅ 6. OTP email bhejo
                    await _emailService.SendRegistrationOtp(request.Email, request.OwnerName, otp);

                    return ApiResponse<string>.Ok(default!, "Registration successful. OTP sent to your email.");
                }

                private static string GenerateOtp()
                {
                    return new Random().Next(100000, 999999).ToString();
                }
            }
        }
