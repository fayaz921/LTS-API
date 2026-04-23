using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.JWT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _appDbcontext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        public LoginUserCommandHandler(AppDbContext appDbcontext, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
        {
            _appDbcontext = appDbcontext;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }
        public async Task<ApiResponse<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _appDbcontext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return ApiResponse<string>.Fail("Invalid email or password");

            if (!user.IsActive)
                return ApiResponse<string>.Fail("Please verify your OTP before login");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return ApiResponse<string>.Fail("Invalid email or password");

            var token = _tokenService.GenerateToken(user);
            return ApiResponse<string>.Ok(token, "Login Success");
        }
    }
}
