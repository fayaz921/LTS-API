using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
using LTS.API.Features.UserManangement.DTOs;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.JWT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<ResponseLogin>>
    {
        private readonly AppDbContext _appDbcontext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginUserCommandHandler(AppDbContext appDbcontext, IPasswordHasher<User> passwordHasher, ITokenService tokenService,IHttpContextAccessor httpContextAccessor)
        {
            _appDbcontext = appDbcontext;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<ResponseLogin>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _appDbcontext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return ApiResponse<ResponseLogin>.Fail("Invalid email or password");

            if (!user.IsActive)
                return ApiResponse<ResponseLogin>.Fail("Please verify your OTP before login");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return ApiResponse<ResponseLogin>.Fail("Invalid email or password");

            var accesstoken = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _appDbcontext.RefreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            await _appDbcontext.SaveChangesAsync();

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", Uri.EscapeDataString(refreshToken), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return ApiResponse<ResponseLogin>.Ok(new ResponseLogin { 
             AccessToken=  accesstoken,
               RefreshToken= refreshToken
            });
        }
    }
}
