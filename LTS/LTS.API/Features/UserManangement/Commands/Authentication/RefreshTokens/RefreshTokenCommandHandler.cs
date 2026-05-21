using LTS.API.Common.Response;
using LTS.API.Domain.Entities;
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

            var refreshToken = context.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken)) 
                return ApiResponse<string>.Fail("Refresh token not found");

            var decodedToken = Uri.UnescapeDataString(refreshToken);
            var hashedtoken = _tokenService.HashToken(decodedToken);
            var utcNow = DateTime.UtcNow;
            var stored = await _db.RefreshTokens
                .AsNoTracking()
                .Where(x => x.Token == hashedtoken && !x.IsRevoked)
                .Select(x => new
                {
                    x.Id,
                    x.ExpiryDate,
                    x.UserId,
                    User = new
                    {
                        x.User.Id,
                        x.User.Name,
                        x.User.Email,
                        x.User.Role,
                        x.User.OrganizationId
                    }
                }).FirstOrDefaultAsync(cancellationToken);
            if (stored == null)
                return ApiResponse<string>.Fail("Invalid refresh token");

            if (stored.ExpiryDate < utcNow)
                return ApiResponse<string>.Fail("Session expired, please login again");
            await _db.RefreshTokens
                    .Where(x => x.Id == stored.Id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsRevoked, true),
                     cancellationToken);

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newHashed = _tokenService.HashToken(newRefreshToken);
            var accessToken = _tokenService.GenerateToken(new Domain.Entities.User
            {
                Id = stored.User.Id,
                Name = stored.User.Name,
                Email = stored.User.Email,
                Role = stored.User.Role,
                OrganizationId = stored.User.OrganizationId
            });
            // NEW REFRESH TOKEN SAVE
            await _db.RefreshTokens.AddAsync(new RefreshToken
            {
                UserId = stored.UserId,
                Token = newHashed,
                ExpiryDate = utcNow.AddDays(7),
                IsRevoked = false
            }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            context.Response.Cookies.Append(
                "refreshToken",
                Uri.EscapeDataString(newRefreshToken),
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = utcNow.AddDays(7),
                    MaxAge = TimeSpan.FromDays(7)
                });


            return ApiResponse<string>.Ok(accessToken, "Token refreshed successfully");
        }
    }
}