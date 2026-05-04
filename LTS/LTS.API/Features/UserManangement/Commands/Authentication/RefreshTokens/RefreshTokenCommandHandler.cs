using LTS.API.Common.Response;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Services.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Features.UserManangement.Commands.Authentication.RefreshTokens
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<string>>
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenCommandHandler(AppDbContext db,ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext!;

            // Cookie se refreshToken lo
            var refreshToken = context.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return ApiResponse<string>.Fail("Refresh token not found");

            // DB mein dhundo
            var stored = await _db.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken && !r.IsRevoked, cancellationToken);

            if (stored == null)
                return ApiResponse<string>.Fail("Invalid refresh token");

            if (stored.ExpiryDate < DateTime.UtcNow)
            {
                stored.IsRevoked = true;
                await _db.SaveChangesAsync(cancellationToken);
                return ApiResponse<string>.Fail("Session expired, please login again");
            }

            // Purana revoke karo (Token Rotation)
            stored.IsRevoked = true;

            // Naya refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.GenerateToken(stored.User);

            await _db.RefreshTokens.AddAsync(new Domain.Entities.RefreshToken
            {
                UserId = stored.UserId,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            }, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            // Naya refreshToken cookie mein
            context.Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            // Naya accessToken response body mein
            return ApiResponse<string>.Ok(newAccessToken, "Token refreshed successfully");
        }
    }
}